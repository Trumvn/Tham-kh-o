-- use aegona;

BEGIN TRANSACTION [AsiaMoneyerSetupTransaction1]

BEGIN TRY

/*** *********************************************************
SYSTEM SETTINGS
*** *********************************************************/	
INSERT INTO [dbo].[SysSettings](Id, [Key], [Lang], [Value]) VALUES(31, 'default_accounts', 'en', 'Wallet, ATM');
INSERT INTO [dbo].[SysSettings](Id, [Key], [Lang], [Value]) VALUES(32, 'default_income_categories', 'en', 'Salary, Other');
INSERT INTO [dbo].[SysSettings](Id, [Key], [Lang], [Value]) VALUES(33, 'default_expense_categories', 'en', 'Food, Grocery, Clothing, Travelling, Shopping, Education, Nothing');

-- Time Frequencies
INSERT INTO [dbo].[TimeFrequencies] ([Id], [TimeFrequencyTitle], [weeks], [IsActive], [SortOrder]) VALUES(1, 'Never', 0, 1, 0);
INSERT INTO [dbo].[TimeFrequencies] ([Id], [TimeFrequencyTitle], [weeks], [IsActive], [SortOrder]) VALUES(2, 'Daily', 0, 1, 1);
INSERT INTO [dbo].[TimeFrequencies] ([Id], [TimeFrequencyTitle], [weeks], [IsActive], [SortOrder]) VALUES(3, 'Weekly', 1, 1, 2);
INSERT INTO [dbo].[TimeFrequencies] ([Id], [TimeFrequencyTitle], [weeks], [IsActive], [SortOrder]) VALUES(4, 'Monthly', 4, 1, 3);
INSERT INTO [dbo].[TimeFrequencies] ([Id], [TimeFrequencyTitle], [weeks], [IsActive], [SortOrder]) VALUES(5, 'Yearly', 52, 1, 4);
INSERT INTO [dbo].[TimeFrequencies] ([Id], [TimeFrequencyTitle], [weeks], [IsActive], [SortOrder]) VALUES(6, 'Periodically', 0, 0, 5);

insert into [dbo].[AppObjectTypes](Id, [AppObjectTypeTitle]) values(100, N'Global Logs');
insert into [dbo].[AppObjectTypes](Id, [AppObjectTypeTitle]) values(600, N'Project Logs');
insert into [dbo].[AppObjectTypes](Id, [AppObjectTypeTitle]) values(620, N'Transaction Logs');
insert into [dbo].[AppObjectTypes](Id, [AppObjectTypeTitle]) values(650, N'Budget Logs');
insert into [dbo].[AppObjectTypes](Id, [AppObjectTypeTitle]) values(670, N'Account Logs');
insert into [dbo].[AppObjectTypes](Id, [AppObjectTypeTitle]) values(690, N'Category Logs');

insert into [dbo].[ProjectPermissions](Id, [PermissionTitle]) values(502, N'Owner');
insert into [dbo].[ProjectPermissions](Id, [PermissionTitle]) values(402, N'Member');

/*** *********************************************************
AUTO MESSAGING
*** *********************************************************/
insert into dbo.AutoMessagingTypes(Id, [MessagingTypeTitle], IsEnable) values (102, N'System', 1);
insert into dbo.AutoMessagingTypes(Id, [MessagingTypeTitle], IsEnable) values (105, N'IT', 1);
insert into dbo.AutoMessagingTypes(Id, [MessagingTypeTitle], IsEnable) values (108, N'Monitoring', 1);
insert into dbo.AutoMessagingTypes(Id, [MessagingTypeTitle], IsEnable) values (202, N'Customer Service', 1);

insert into dbo.AutoMessagingSenders(Id, [MessagingSenderTitle], [ProviderName], [ProviderHost], [ProviderPort], [ProviderEnableSsl], [CredentialUserName], [DisplayName], [CredentialPasswordHash], [SecurityStamp], IsEnable) 
	values (101, N'Gmail', N'Google', N'smtp.gmail.com', 587, 1, N'aegonaglobal@gmail.com', N'Aegona Global', 'EAAAAAKxWCK2lS0qU49+8zqCNEcmMe3u1YNBPFSe1JEosgf0', '1450508073', 1);	

-- ---------------------------------------------------------
insert into dbo.AutoMessagingTemplates(Id, AutoMessagingTypeId, AutoMessagingSenderId, [MessagingTemplateName], IsPublish, CreatedDate)
	values ('41D0DB82-9539-47C9-BBC2-1C91D5A57A8D', 102, 101, 'System', 1, getdate());

insert into dbo.AutoMessagingTemplates(Id, AutoMessagingTypeId, AutoMessagingSenderId, [MessagingTemplateName], IsPublish, CreatedDate)
	values ('972325A5-0621-49C2-B4D8-46A39E734CDD', 105, 101, 'Application Error', 1, getdate());

insert into dbo.AutoMessagingTemplates(Id, AutoMessagingTypeId, AutoMessagingSenderId, [MessagingTemplateName], IsPublish, CreatedDate)
	values ('50D38B29-DAB2-4BF8-B46F-7B8E551B23E8', 108, 101, 'Monitoring', 1, getdate());

insert into dbo.AutoMessagingTemplates(Id, AutoMessagingTypeId, AutoMessagingSenderId, [MessagingTemplateName], IsPublish, CreatedDate)
	values ('8F4AB8C8-CBF6-46D8-BAF6-E13CB448C04C', 202, 101, 'Customer Service', 1, getdate());
-- ---------------------------------------------------------
insert into dbo.AutoMessagingTemplateContents(Id, AutoMessagingTemplateId, [Lang], [MessagingSubject], [MessagingFromName], [MessagingFromEmailAddress], [MessagingTo], [MessagingCc], [MessagingBcc], [MessagingContent], [Tags], IsPublish, FromDate, EndDate, CreatedDate)
	values ('C1EA8AAB-AD71-44FD-ABC1-EED27F3BE9A6', '972325A5-0621-49C2-B4D8-46A39E734CDD', 'en', N'Application Error', 'Aegona IT', 'aegonaglobal@gmail.com', 'aegonaglobal@gmail.com', null, null, 'Hey! Need you superman :){{binding_text}}', null, 1, getdate()-3, null, getdate());

insert into  dbo.AutoMessagingDataMapping(Id, [MappingName], [TokenKey], [TableName], [ColumnName], [RequiredColumnName], [Format], [SqlQuery], [Value], IsPublish, CreatedDate)
	values (newid(), N'System IT Email', '{{it_email}}', null, null, null, null, null, 'aegonaglobal@gmail.com', 1, getdate()),
			(newid(), N'Client Email', '{{client_email}}', 'Clients', 'EmailAddress', 'UserId', 'email', 'select EmailAddress from Clients where UserId=''{0}''', null, 1, getdate()),
			(newid(), N'Client Fullname', '{{client_fullname}}', 'Clients', 'FirstName, LastName', 'UserId', 'text', 'select FirstName + '' '' + LastName as fullname from Clients where UserId=''{0}''', null, 1, getdate()),
			(newid(), N'Client First', '{{client_firstname}}', 'Clients', 'FirstName', 'UserId', 'text', 'select FirstName from Clients where UserId=''{0}''', null, 1, getdate()),
			(newid(), N'Dynamic Text', '{{binding_text}}', null, null, null, 'text', null, '{{binding_text}}', 1, getdate());

/*** *********************************************************
SECURITY
*** *********************************************************/
declare @role_id_admin varchar(128) = newid();
declare @role_id_manager varchar(128) = newid();
declare @role_id_user varchar(128) = newid();

insert into dbo.Roles(Id, Name) values(@role_id_admin, 'Admin');
insert into dbo.Roles(Id, Name) values(@role_id_manager, 'Manager');
insert into dbo.Roles(Id, Name) values(@role_id_user, 'User');

declare @group_id_admins varchar(128) = newid();
declare @group_id_managers varchar(128) = newid();
declare @group_id_users varchar(128) = newid();

insert into dbo.Groups(Id, [Name], [Description]) values (@group_id_admins, 'Admins', 'Superman');
insert into dbo.Groups(Id, [Name], [Description]) values (@group_id_managers, 'Managers', 'Hero');
insert into dbo.Groups(Id, [Name], [Description]) values (@group_id_users, 'Users', 'Warrior');

insert into dbo.GroupRoles(GroupId, RoleId) values (@group_id_admins, @role_id_admin);
insert into dbo.GroupRoles(GroupId, RoleId) values (@group_id_managers, @role_id_manager);
insert into dbo.GroupRoles(GroupId, RoleId) values (@group_id_users, @role_id_user);

/*** *********************************************************
BILLING
*** *********************************************************/
declare @target_market_vn int = 39;
declare @target_market_global int = 79;

insert into dbo.TargetMarkets(Id, TargetmMarketName, TargetmMarketTitle, TargetmMarketDesc, IsActive, CreatedDate, IsDeleted) values
	(@target_market_vn, N'VN', N'VN', N'Vietnamese Market', 1, getdate(), 0);
insert into dbo.TargetMarkets(Id, TargetmMarketName, TargetmMarketTitle, TargetmMarketDesc, IsActive, CreatedDate, IsDeleted) values
	(@target_market_global, N'GLOBAL', N'GLOBAL', N'Global Market', 1, getdate(), 0);

declare @payment_method_paypal int = 19;
declare @payment_method_onepay int = 29;

insert into dbo.PaymentMethods(Id, PaymentMethodName, PaymentMethodTitle, PaymentMethodDesc, IsActive, IsDeleted) values
	(@payment_method_paypal, N'Paypal', N'Paypal', N'Global Payment', 1, 0),
	(@payment_method_onepay, N'1Pay', N'1Pay', N'1Pay - Only in Vietnam', 1, 0);

declare @Subscription_Type_Monthly int = 119;
declare @Subscription_Type_Three_Months int = 129;
declare @Subscription_Type_Six_Months int = 139;
declare @Subscription_Type_Yearly int = 169;

insert into dbo.SubscriptionTypes(Id, SubscriptionTypeName, SubscriptionTypeTitle, SubscriptionTypeDesc, MonthValue, IsActive, CreatedDate, IsDeleted) values
	(@Subscription_Type_Monthly, N'Month', N'Month', N'Month', 1, 1, getdate(), 0),
	(@Subscription_Type_Three_Months, N'Three Months', N'Three Months', N'Three Months', 3, 1, getdate(), 0),
	(@Subscription_Type_Six_Months, N'Six Months', N'Six Months', N'Six Months', 6, 1, getdate(), 0),
	(@Subscription_Type_Yearly, N'Year', N'Year', N'Year', 12, 1, getdate(), 0);

declare @free_product_id varchar(128) = newid();
declare @small_product_id varchar(128) = newid();
declare @large_product_id varchar(128) = newid();

insert into dbo.Products(Id, ProductName, ProductTitle, ProductDesc, MaxProject, MaxUserMember, IsActive, UpgradeLevel, ValidDate, ExpiredDate, CreatedDate, IsDeleted) values
	(@free_product_id, N'Fee', N'Free', N'Free', 1, 1, 1, 0, getdate(), null, getdate(), 0),
	(@small_product_id, N'Small Group', N'Small Group', N'Small Group', 3, 3 , 1, 3, getdate(), null, getdate(), 0),
	(@large_product_id, N'Large Group', N'Large Group', N'Large Group', 10, 10, 1, 9, getdate(), null, getdate(), 0);

-- Price List
-- VN
insert into dbo.ProductPrices(Id, ProductId, SubscriptionTypeId, TargetMarketId, Price, TaxValue, ValidDate) values 
	(newid(), @free_product_id, @Subscription_Type_Monthly, @target_market_vn, 0, 0, getdate()),
	(newid(), @free_product_id, @Subscription_Type_Three_Months, @target_market_vn, 0, 0, getdate()),
	(newid(), @free_product_id, @Subscription_Type_Six_Months, @target_market_vn, 0, 0, getdate()),
	(newid(), @free_product_id, @Subscription_Type_Yearly, @target_market_vn, 0, 0, getdate()),

	(newid(), @small_product_id, @Subscription_Type_Monthly, @target_market_vn, 1, 0, getdate()),
	(newid(), @small_product_id, @Subscription_Type_Three_Months, @target_market_vn, 3, 0, getdate()),
	(newid(), @small_product_id, @Subscription_Type_Six_Months, @target_market_vn, 6, 0, getdate()),
	(newid(), @small_product_id, @Subscription_Type_Yearly, @target_market_vn, 12, 0, getdate()),

	(newid(), @large_product_id, @Subscription_Type_Monthly, @target_market_vn, 3, 0, getdate()),
	(newid(), @large_product_id, @Subscription_Type_Three_Months, @target_market_vn, 6, 0, getdate()),
	(newid(), @large_product_id, @Subscription_Type_Six_Months, @target_market_vn, 18, 0, getdate()),
	(newid(), @large_product_id, @Subscription_Type_Yearly, @target_market_vn, 36, 0, getdate());

COMMIT TRANSACTION [AsiaMoneyerSetupTransaction1]

END TRY
BEGIN CATCH
  ROLLBACK TRANSACTION [AsiaMoneyerSetupTransaction1]
    SELECT 
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_SEVERITY() AS ErrorSeverity,
        ERROR_STATE() as ErrorState,
        ERROR_PROCEDURE() as ErrorProcedure,
        ERROR_LINE() as ErrorLine,
        ERROR_MESSAGE() as ErrorMessage;
END CATCH  

GO