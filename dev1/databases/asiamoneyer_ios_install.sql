-- use aegona;

BEGIN TRANSACTION [AsiaMoneyerInstallTransaction1]

BEGIN TRY

/*** *********************************************************
SECURITY
*** *********************************************************/
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
	Discriminator varchar(MAX)
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
	LanguageUsage varchar(2),
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
	[Description] ntext,
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