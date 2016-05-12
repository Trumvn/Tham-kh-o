--use aegona;

BEGIN TRANSACTION [AsiaMoneyerRollbackTransaction1]

BEGIN TRY
/*** *********************************************************
AUDIT LOG
*** *********************************************************/
if (OBJECT_ID('dbo.AuditLogs','U') IS NOT NULL)
	drop table AuditLogs;

/*** *********************************************************
AUTO EMAIL
*** *********************************************************/

if (OBJECT_ID('dbo.AutoMessagingMessages','U') IS NOT NULL)
	drop table AutoMessagingMessages;

if (OBJECT_ID('dbo.AutoMessagingDataMapping','U') IS NOT NULL)
	drop table AutoMessagingDataMapping;

if (OBJECT_ID('dbo.AutoMessagingTemplateContents','U') IS NOT NULL)
	drop table AutoMessagingTemplateContents;

if (OBJECT_ID('dbo.AutoMessagingTemplates','U') IS NOT NULL)
	drop table AutoMessagingTemplates;

if (OBJECT_ID('dbo.AutoMessagingSenders','U') IS NOT NULL)
	drop table AutoMessagingSenders;

if (OBJECT_ID('dbo.AutoMessagingTypes','U') IS NOT NULL)
	drop table AutoMessagingTypes;

/*** *********************************************************
AEGONA
*** *********************************************************/

if (OBJECT_ID('dbo.UserComments','U') IS NOT NULL)
	drop table UserComments;

if (OBJECT_ID('dbo.AppObjectTypes','U') IS NOT NULL)
	drop table AppObjectTypes;

if (OBJECT_ID('dbo.Transactions','U') IS NOT NULL)
	drop table Transactions;

if (OBJECT_ID('dbo.RecurringTransactions','U') IS NOT NULL)
	drop table RecurringTransactions;

if (OBJECT_ID('dbo.CategoryBudgets','U') IS NOT NULL)
	drop table CategoryBudgets;

if (OBJECT_ID('dbo.Categories','U') IS NOT NULL)
	drop table Categories;

if (OBJECT_ID('dbo.Accounts','U') IS NOT NULL)
	drop table Accounts;
	
if (OBJECT_ID('dbo.ProjectMembers','U') IS NOT NULL)
	drop table ProjectMembers;

if (OBJECT_ID('dbo.ProjectPermissions','U') IS NOT NULL)
	drop table ProjectPermissions;

if (OBJECT_ID('dbo.Projects','U') IS NOT NULL)
	drop table Projects;

if (OBJECT_ID('dbo.TimeFrequencies','U') IS NOT NULL)
	drop table TimeFrequencies;

if (OBJECT_ID('dbo.UserPhotos','U') IS NOT NULL)
	drop table UserPhotos;

if (OBJECT_ID('dbo.Clients','U') IS NOT NULL)
	drop table Clients;

if (OBJECT_ID('dbo.FrequentlyAskedQuestions','U') IS NOT NULL)
	drop table FrequentlyAskedQuestions;


/*** *********************************************************
BILLING
*** *********************************************************/	
IF OBJECT_ID('uq_subscribers_promotioncodes', 'C') IS NOT NULL
	ALTER TABLE SubscriberPromotionCodes DROP CONSTRAINT uq_subscribers_promotioncodes;
IF OBJECT_ID('uq_products_features', 'C') IS NOT NULL
	ALTER TABLE ProductFeatures DROP CONSTRAINT uq_products_features;

if (OBJECT_ID('dbo.Invoices','U') IS NOT NULL)
	drop table Invoices;
if (OBJECT_ID('dbo.PaymentGateways','U') IS NOT NULL)
	drop table PaymentGateways;
if (OBJECT_ID('dbo.PaymentMethods','U') IS NOT NULL)
	drop table PaymentMethods;

if (OBJECT_ID('dbo.Subscriptions','U') IS NOT NULL)
	drop table Subscriptions;
	
if (OBJECT_ID('dbo.ProductPrices','U') IS NOT NULL)
	drop table ProductPrices;
if (OBJECT_ID('dbo.Products','U') IS NOT NULL)
	drop table Products;
if (OBJECT_ID('dbo.SubscriptionTypes','U') IS NOT NULL)
	drop table SubscriptionTypes;

if (OBJECT_ID('dbo.TargetMarkets','U') IS NOT NULL)
	drop table TargetMarkets;
	
/*** *********************************************************
SECURITY
*** *********************************************************/
IF OBJECT_ID('fk_UserGroupToUser', 'C') IS NOT NULL
	ALTER TABLE UserGroups DROP CONSTRAINT fk_UserGroupToUser;
IF OBJECT_ID('fk_UserGroupToGroup', 'C') IS NOT NULL
	ALTER TABLE UserGroups DROP CONSTRAINT fk_UserGroupToGroup;
IF OBJECT_ID('fk_GroupRoleToRole', 'C') IS NOT NULL
	ALTER TABLE GroupRoles DROP CONSTRAINT fk_GroupRoleToRole;
IF OBJECT_ID('fk_GroupRoleToGroup', 'C') IS NOT NULL
	ALTER TABLE GroupRoles DROP CONSTRAINT fk_GroupRoleToGroup;

IF OBJECT_ID('fk_UserRoleToRole', 'C') IS NOT NULL
	ALTER TABLE UserRoles DROP CONSTRAINT fk_UserRoleToRole;

IF OBJECT_ID('fk_UserLoginToUser', 'C') IS NOT NULL
	ALTER TABLE UserLogins DROP CONSTRAINT fk_UserLoginToUser;

IF OBJECT_ID('fk_UserClaimToUser', 'C') IS NOT NULL
	ALTER TABLE UserClaims DROP CONSTRAINT fk_UserClaimToUser;

IF OBJECT_ID('fk_UserRoleToUser', 'C') IS NOT NULL
	ALTER TABLE UserRoles DROP CONSTRAINT fk_UserRoleToUser;

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='UserLoginsByUserIdIndex' AND object_id = OBJECT_ID('[dbo].[UserLogins]'))    
	DROP INDEX UserLoginsByUserIdIndex ON UserLogins;

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='UserClaimsByUserIdIndex' AND object_id = OBJECT_ID('[dbo].[UserClaims]'))
	DROP INDEX UserClaimsByUserIdIndex ON UserClaims;

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='UserRolesByUserIdIndex' AND object_id = OBJECT_ID('[dbo].[UserRoles]'))
	DROP INDEX UserRolesByUserIdIndex ON UserRoles;

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='UserRolesByRoleIdIndex' AND object_id = OBJECT_ID('[dbo].[UserRoles]'))
	DROP INDEX UserRolesByRoleIdIndex ON UserRoles;

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='UserNameIndex' AND object_id = OBJECT_ID('[dbo].[Users]'))
	DROP INDEX UserNameIndex ON Users;

IF EXISTS (SELECT *  FROM sys.indexes  WHERE name='RoleNameIndex' AND object_id = OBJECT_ID('[dbo].[Roles]'))
	DROP INDEX RoleNameIndex ON Roles;	

if (OBJECT_ID('dbo.UserGroups','U') IS NOT NULL)
	drop table UserGroups;

if (OBJECT_ID('dbo.GroupRoles','U') IS NOT NULL)
	drop table GroupRoles;

if (OBJECT_ID('dbo.Groups','U') IS NOT NULL)
	drop table Groups;

if (OBJECT_ID('dbo.UserLogins','U') IS NOT NULL)
	drop table UserLogins;

if (OBJECT_ID('dbo.UserClaims','U') IS NOT NULL)
	drop table UserClaims;

if (OBJECT_ID('dbo.UserRoles','U') IS NOT NULL)
	drop table UserRoles;

if (OBJECT_ID('dbo.Users','U') IS NOT NULL)
	drop table Users;

if (OBJECT_ID('dbo.Roles','U') IS NOT NULL)
	drop table Roles;

/*** *********************************************************
END SECURITY
*** *********************************************************/	

/*** *********************************************************
SYSTEM SETTINGS
*** *********************************************************/	
if (OBJECT_ID('dbo.SysSettings','U') IS NOT NULL)
	drop table SysSettings;

COMMIT TRANSACTION [AsiaMoneyerRollbackTransaction1]

END TRY
BEGIN CATCH
  ROLLBACK TRANSACTION [AsiaMoneyerRollbackTransaction1]
      SELECT 
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_SEVERITY() AS ErrorSeverity,
        ERROR_STATE() as ErrorState,
        ERROR_PROCEDURE() as ErrorProcedure,
        ERROR_LINE() as ErrorLine,
        ERROR_MESSAGE() as ErrorMessage;
END CATCH  

GO
