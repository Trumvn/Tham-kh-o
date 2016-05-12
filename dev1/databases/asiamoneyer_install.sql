-- use aegona;

BEGIN TRANSACTION [AsiaMoneyerInstallTransaction1]

BEGIN TRY
/*** *********************************************************
SYSTEM SETTING
*** *********************************************************/
create table dbo.SysSettings
(
	Id int not null primary key clustered,
	[Key] varchar(256) NOT NULL,
	[Lang] varchar(2),
	[Value] varchar(800) NOT NULL
);

/*** *********************************************************
SECURITY
*** *********************************************************/
create table dbo.Roles
(
	Id nvarchar(128) not null primary key clustered,
	Name nvarchar(128) not null
);
create unique index RoleNameIndex on dbo.Roles(Name);

create table dbo.Users
(
	Id nvarchar(128) not null primary key clustered,
	FirstName nvarchar(128) not null,
	LastName nvarchar(128),
	JoinDate datetime,
	Email varchar(256),
	EmailConfirmed bit not null,
	PasswordHash varchar(MAX),
	SecurityStamp varchar(MAX),
	PhoneNumber varchar(MAX),
	PhoneNumberConfirmed bit not null,
	TwoFactorEnabled bit not null,
	LockoutEndDateUtc datetime,
	LockoutEnabled bit not null,
	AccessFailedCount int not null,
	UserName varchar(256) not null,
	Passcode varchar(256),
	Discriminator varchar(MAX)
);
create unique index UserNameIndex on dbo.Users(UserName);

create table dbo.UserRoles
(
	UserId nvarchar(128) not null, 
	RoleId nvarchar(128) not null,
	PRIMARY KEY (UserId, RoleId),
	CONSTRAINT fk_UserRoleToRole FOREIGN KEY (RoleId) REFERENCES dbo.Roles(Id) ON DELETE CASCADE,
	CONSTRAINT fk_UserRoleToUser FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE,
);
create index UserRolesByUserIdIndex on dbo.UserRoles(UserId);
create index UserRolesByRoleIdIndex on dbo.UserRoles(RoleId);

create table dbo.UserClaims
(
	Id int identity(1, 1) not null primary key clustered,
	UserId nvarchar(128) not null,
	ClaimType varchar(MAX),
	ClaimValue varchar(MAX),
	CONSTRAINT fk_UserClaimToUser FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE,
);
create index UserClaimsByUserIdIndex on dbo.UserClaims(UserId);

create table dbo.UserLogins
(
	LoginProvider nvarchar(128) not null,
	ProviderKey nvarchar(128) not null,
	UserId nvarchar(128) not null,
	PRIMARY KEY (LoginProvider, ProviderKey, UserId),
	CONSTRAINT fk_UserLoginToUser FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE,
);
create index UserLoginsByUserIdIndex on dbo.UserLogins(UserId);

create table dbo.Groups
(
	Id nvarchar(128) not null primary key clustered,
	[Name] nvarchar(128) not null,
	[Description] nvarchar(128)
);

create table dbo.GroupRoles
(
	GroupId nvarchar(128) not null, 
	RoleId nvarchar(128) not null,
	PRIMARY KEY (GroupId, RoleId),
	CONSTRAINT fk_GroupRoleToRole FOREIGN KEY (RoleId) REFERENCES dbo.Roles(Id) ON DELETE CASCADE,
	CONSTRAINT fk_GroupRoleToGroup FOREIGN KEY (GroupId) REFERENCES dbo.Groups(Id) ON DELETE CASCADE,
);

create table dbo.UserGroups
(
	GroupId nvarchar(128) not null, 
	UserId nvarchar(128) not null,
	PRIMARY KEY (GroupId, UserId),
	CONSTRAINT fk_UserGroupToUser FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE,
	CONSTRAINT fk_UserGroupToGroup FOREIGN KEY (GroupId) REFERENCES dbo.Groups(Id) ON DELETE CASCADE,
);

/*** *********************************************************
END SECURITY
*** *********************************************************/

-- -----------------------------------------------------------
create table dbo.Clients
(
	Id nvarchar(128) not null primary key clustered,
	UserId nvarchar(128) foreign key references Users(Id),
	FirstName varchar(256) NOT NULL,
	LastName varchar(256),
	EmailAddress varchar(256) NOT NULL,
	PhoneNumber varchar(256),
	Birthday datetime,
	Gender bit default 1,
	IsActive bit default 1,
	Lang varchar(2),
	IsDeleted bit default 0,
	CreatedDate datetime default(getdate()) not null	
);

create table dbo.UserPhotos
(
	Id nvarchar(128) not null primary key clustered,
	UserId nvarchar(128) foreign key references dbo.Users(Id),
	Photo varchar(MAX),
	PhotoSmall varchar(MAX),
	IsActive bit default 1,
	IsDeleted bit default 0,
	CreatedDate datetime default(getdate()) not null	
);

create table dbo.Projects
(
	Id nvarchar(128) not null primary key clustered,
	[ProjectTitle] nvarchar(800) not null,
	[ProjectDesc] ntext,
	[HighlightColor] varchar(16),
	[WorkingEmail] nvarchar(320),
	[Currency] nvarchar(3),
	[FinanceYearStartMonth] tinyint default(1),
	[FinanceYearMonths] tinyint default(12),
	IsPrivate bit default 1,
	IsDeleted bit default(0),
	CreatedDate datetime default(getdate()) not null	
);

create table dbo.ProjectPermissions
(
	Id int NOT NULL,
	[PermissionTitle] varchar(256) NOT NULL UNIQUE,
	PRIMARY KEY (Id)
);

create table dbo.ProjectMembers
(
	Id nvarchar(128),
	ClientId nvarchar(128) foreign key references dbo.Clients(Id),
	ProjectId nvarchar(128) foreign key references dbo.Projects(Id),
	PermissionId int foreign key references dbo.ProjectPermissions(Id),
	[HighlightColor] varchar(16),
	IsFollowing bit default(1),
	IsFavorite bit default 0,
	IsArchived bit default(0),
	ViewFilterByType int default(0),
	ViewFilterByAccount nvarchar(128),
	ViewFilterByCategory nvarchar(128),
	ViewFilterFromDate datetime,
	ViewFilterEndDate datetime,
	StartDate datetime default(getdate()),
	EndDate datetime,
	PRIMARY KEY (ClientId, ProjectId)
);

create table dbo.TimeFrequencies
(
	Id int NOT NULL,
	[TimeFrequencyTitle] nvarchar(256) NOT NULL UNIQUE,
	Weeks tinyint default 0,
	SortOrder int default(0),
	IsActive bit default(0),
	PRIMARY KEY (Id)
);

create table dbo.Accounts
(
	Id nvarchar(128) not null primary key clustered,
	ProjectId nvarchar(128) foreign key references dbo.Projects(Id),
	[AccountTitle] nvarchar(320),
	[AccountDescription] ntext,
	[HighlightColor] varchar(16),
	IsPrimary bit default(0),
	IsClosed bit default(0),
	IsDeleted bit default(0),
	OpenDate datetime,
	CloseDate datetime,
	CreatedDate datetime default(getdate()) not null	
);

create table dbo.Categories
(
	Id nvarchar(128) not null primary key clustered,
	ProjectId nvarchar(128) foreign key references dbo.Projects(Id),
	ParentId nvarchar(128) foreign key references dbo.Categories(Id),
	[CategoryTitle] nvarchar(320),
	[CategoryDescription] ntext,
	[HighlightColor] varchar(16),
	IsIncome bit default 0,
	IsClosed bit default(0),
	ClosedDate datetime,
	IsDeleted bit default(0),
	CreatedDate datetime default(getdate()) not null	
);

create table dbo.CategoryBudgets
(
	Id nvarchar(128) not null primary key clustered,
	CategoryId nvarchar(128) foreign key references dbo.Categories(Id),
	StartDate datetime,
	EndDate datetime,
	BudgetAmount decimal default(0),
	TimeFrequencyId int foreign key references dbo.TimeFrequencies(Id),
	IsDeleted bit default(0),
	CreatedDate datetime default(getdate()) not null	
);

create table dbo.RecurringTransactions
(
	Id nvarchar(128) not null primary key clustered,
	ProjectId nvarchar(128) foreign key references dbo.Projects(Id),
	TimeFrequencyId int foreign key references dbo.TimeFrequencies(Id), --Recurrence 
	TransactionDate datetime,
	GeneratedDate datetime,
	GeneratedToDate datetime,
	StartDate datetime,
	EndDate datetime,
	RecurringTimes int default(0),
	IsDeleted bit default(0),
	CreatedDate datetime default(getdate()) not null	
);

create table dbo.Transactions
(
	Id nvarchar(128) not null primary key clustered,
	ClientId nvarchar(128) foreign key references dbo.Clients(Id),
	ProjectId nvarchar(128) foreign key references dbo.Projects(Id),
	AccountId nvarchar(128) foreign key references dbo.Accounts(Id),
	CategoryId nvarchar(128) foreign key references dbo.Categories(Id),
	RecurringTransactionId nvarchar(128) foreign key references dbo.RecurringTransactions(Id),
	[TransactionTitle] nvarchar(800),
	[TransactionDesc] ntext,
	IsIncome bit default 0,
	[TransactionDate] datetime,
	Amount decimal default(0),
	[Payee] nvarchar(320),
	IsClear bit default(0),
	IsDeleted bit default(0),
	CreatedDate datetime default(getdate()) not null	
);

create table dbo.AppObjectTypes
(
	Id int NOT NULL,
	[AppObjectTypeTitle] varchar(256) NOT NULL UNIQUE,
	PRIMARY KEY (Id)
);

create table dbo.UserComments
(
	Id nvarchar(128) not null primary key clustered,
	UserId nvarchar(128) foreign key references dbo.Users(Id),
	AppObjectTypeId int foreign key references dbo.AppObjectTypes(Id),
	[ObjectId] nvarchar(128),
	CommentDate datetime default(getdate()) not null,
	CommentText ntext
);

create table dbo.FrequentlyAskedQuestions
(
	Id nvarchar(128) not null primary key clustered,
	[Lang] char(2) not null,
	[FullName] nvarchar(320) not null,
	[EmailAddress] nvarchar(320),
	[Question] nvarchar(800) not null,
	CreatedDate datetime default(getdate()) not null,
	AssistantName nvarchar(320),
	[Answer] nvarchar(1200),
	[Tags] nvarchar(320),
	[Voting] int default(0),
	[DisplayOrder] int default(0),
	IsPublish bit default 0
);

/*** *********************************************************
AUTO EMAIL
*** *********************************************************/
create table dbo.AutoMessagingTypes
(
	Id int NOT NULL,
	[MessagingTypeTitle] nvarchar(256) NOT NULL UNIQUE,
	IsEnable bit default 1,
	PRIMARY KEY (Id)
);

create table dbo.AutoMessagingSenders
(
	Id int NOT NULL,
	[MessagingSenderTitle] nvarchar(256) NOT NULL UNIQUE,
	[ProviderName] nvarchar(320),
	[ProviderHost] nvarchar(320),
	[ProviderPort] int,
	[ProviderEnableSsl] bit default(1),
	[CredentialUserName] nvarchar(320),
	[DisplayName] nvarchar(320),
	[CredentialPasswordHash] nvarchar(320),
	[SecurityStamp] nvarchar(320),
	IsEnable bit default 1,
	PRIMARY KEY (Id)
);

create table dbo.AutoMessagingTemplates
(
	Id nvarchar(128) not null primary key clustered,
	AutoMessagingTypeId int foreign key references dbo.AutoMessagingTypes(Id),
	AutoMessagingSenderId int foreign key references dbo.AutoMessagingSenders(Id),
	[MessagingTemplateName] nvarchar(320) not null,
	IsPublish bit default 0,
	CreatedDate datetime default(getdate()) not null,
);

create table dbo.AutoMessagingTemplateContents
(
	Id nvarchar(128) not null primary key clustered,
	AutoMessagingTemplateId nvarchar(128) foreign key references dbo.AutoMessagingTemplates(Id) not null,
	[Lang] char(2) not null,
	[MessagingSubject] nvarchar(320) not null,
	[MessagingFromName] nvarchar(320) not null,
	[MessagingFromEmailAddress] nvarchar(320) not null,
	[MessagingTo] nvarchar(320),
	[MessagingCc] nvarchar(320),
	[MessagingBcc] nvarchar(320),
	[MessagingContent] ntext,
	[Tags] nvarchar(320),
	IsPublish bit default 0,
	FromDate datetime,
	EndDate datetime,
	CreatedDate datetime default(getdate()) not null
);

create table dbo.AutoMessagingDataMapping
(
	Id nvarchar(128) not null primary key clustered,
	[MappingName] nvarchar(320) not null,
	[TokenKey] nvarchar(320) not null,
	[TableName] nvarchar(320),
	[ColumnName] nvarchar(320),
	[RequiredColumnName] nvarchar(320),
	[Format] nvarchar(320),
	[SqlQuery] nvarchar(800),
	[Value] nvarchar(320),
	IsPublish bit default 0,
	CreatedDate datetime default(getdate()) not null
);

create table dbo.AutoMessagingMessages
(
	Id nvarchar(128) not null primary key clustered,
	AutoMessagingTemplateContentId nvarchar(128) foreign key references dbo.AutoMessagingTemplateContents(Id) not null,
	AutoMessagingSenderId int foreign key references dbo.AutoMessagingSenders(Id),
	[MessagingSubject] nvarchar(320) not null,
	[MessagingFromName] nvarchar(320) not null,
	[MessagingFromEmailAddress] nvarchar(320) not null,
	[MessagingTo] nvarchar(320),
	[MessagingCc] nvarchar(320),
	[MessagingBcc] nvarchar(320),
	[MessagingContent] ntext,
	[Tags] nvarchar(320),
	IsSent bit default 0,
	IsMarkedAsRead bit default 0,
	SentDate datetime,
	CreatedDate datetime default(getdate()) not null
);

/*** *********************************************************
AUDIT LOG
*** *********************************************************/
create table dbo.AuditLogs
(
        ID UNIQUEIDENTIFIER NOT NULL,
        UserID NVARCHAR(128) NOT NULL,
        EventDateUTC DATETIME NOT NULL,
        EventType CHAR(1) NOT NULL,
        TableName NVARCHAR(100) NOT NULL,
        RecordID NVARCHAR(128) NOT NULL,
        ColumnName NVARCHAR(100) NOT NULL,
        OriginalValue NVARCHAR(MAX) NULL,
        NewValue NVARCHAR(MAX) NULL
        CONSTRAINT PK_AuditLog PRIMARY KEY NONCLUSTERED (ID)
);


/*** *********************************************************
END - AUDIT LOG
*** *********************************************************/

/*** *********************************************************
BILLING
*** *********************************************************/
create table dbo.TargetMarkets
(
	Id int not null primary key clustered,
	TargetmMarketName nvarchar(320) not null,
	TargetmMarketTitle nvarchar(320) not null,
	TargetmMarketDesc nvarchar(800),
	IsActive bit default 1,
	CreatedDate datetime default(getdate()) not null,
	IsDeleted bit default 0
);

create table dbo.SubscriptionTypes
(
	Id int not null primary key clustered,
	SubscriptionTypeName nvarchar(320) not null,
	SubscriptionTypeTitle nvarchar(320) not null,
	SubscriptionTypeDesc nvarchar(800),
	MonthValue tinyint default(1),
	IsActive bit default 1,
	CreatedDate datetime default(getdate()) not null,
	IsDeleted bit default 0
);

create table dbo.Products
(
	Id nvarchar(128) not null primary key clustered,
	ProductName nvarchar(320) not null,
	ProductTitle nvarchar(320) not null,
	ProductDesc nvarchar(800),
	IsActive bit default 1,
	UpgradeLevel smallint default(0),
	ValidDate datetime,
	ExpiredDate datetime,
	MaxProject int,
	MaxUserMember int,
	CreatedDate datetime default(getdate()) not null,
	IsDeleted bit default 0
);

create table dbo.ProductPrices
(
	Id nvarchar(128) not null primary key clustered,
	ProductId nvarchar(128) foreign key references dbo.Products(Id),
	SubscriptionTypeId int foreign key references dbo.SubscriptionTypes(Id),
	TargetMarketId int foreign key references dbo.TargetMarkets(Id),
	Price decimal default(0),
	TaxValue decimal default(0),
	TaxCode nvarchar(32),
	IsActive bit default 1,
	ValidDate datetime,
	ExpiredDate datetime,
	CreatedDate datetime default(getdate()) not null,
	IsDeleted bit default 0
);

create table dbo.Subscriptions
(
	Id nvarchar(128) not null primary key clustered,
	SubscriptionTypeId int foreign key references dbo.SubscriptionTypes(Id),
	UserId nvarchar(128) foreign key references dbo.Users(Id),
	ProductId nvarchar(128) foreign key references dbo.Products(Id),
	IsActive bit default 1,
	ValidDate datetime,
	ExpiredDate datetime,
	NextInvoiceDate datetime,
	LastInvoiceDate datetime,
	LastPaymentDate datetime,
	IsDeleted bit default 0,
	CreatedDate datetime default(getdate()) not null
);

create table dbo.PaymentMethods
(
	Id int not null primary key clustered,
	PaymentMethodName nvarchar(128) not null,
	PaymentMethodTitle nvarchar(128) not null,
	PaymentMethodDesc nvarchar(800),
	IsActive bit default 1,
	IsDeleted bit default 0
);

create table dbo.PaymentGateways
(
	Id nvarchar(128) not null primary key clustered,
	PaymentMethodId int foreign key references dbo.PaymentMethods(Id),
	PaymentGatewayName nvarchar(128) not null,
	PaymentGatewayTitle nvarchar(128) not null,
	PaymentGatewayDesc nvarchar(800),
	IsActive bit default 1,
	ValidDate datetime,
	ExpiredDate datetime,
	CreatedDate datetime default(getdate()) not null,
	IsDeleted bit default 0
);

create table dbo.Invoices
(
	Id nvarchar(128) not null primary key clustered,
	UserId nvarchar(128) foreign key references dbo.Users(Id),
	SubscriptionId nvarchar(128)  foreign key references dbo.Subscriptions(Id),
	PaymentMethodId int foreign key references dbo.PaymentMethods(Id),
	ProductPriceId nvarchar(128) foreign key references dbo.ProductPrices(Id),
	InvoiceDate datetime,
	PaymentDate datetime,
	Price decimal default(0),
	Quality decimal default(1),
	Amount decimal default(0),
	Discount decimal default(0),
	PaidAmount decimal default(0),
	IsCompleted bit default 0,
	TransactionId nvarchar(128),
	CreatedDate datetime default(getdate()) not null,
	IsDeleted bit default 0
);

/*** *********************************************************
END - BILLING
*** *********************************************************/

COMMIT TRANSACTION [AsiaMoneyerInstallTransaction1]

END TRY
BEGIN CATCH
  ROLLBACK TRANSACTION [AsiaMoneyerInstallTransaction1]
  SELECT 
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_SEVERITY() AS ErrorSeverity,
        ERROR_STATE() as ErrorState,
        ERROR_PROCEDURE() as ErrorProcedure,
        ERROR_LINE() as ErrorLine,
        ERROR_MESSAGE() as ErrorMessage;
END CATCH  

GO