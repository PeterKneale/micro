IF OBJECT_ID('dbo.RolePermission', 'U') IS NOT NULL 
	DROP TABLE [dbo].[RolePermission];
IF OBJECT_ID('dbo.TeamRole', 'U') IS NOT NULL 
	DROP TABLE [dbo].[TeamRole];
IF OBJECT_ID('dbo.UserTeam', 'U') IS NOT NULL 
	DROP TABLE [dbo].[UserTeam];
IF OBJECT_ID('dbo.Role', 'U') IS NOT NULL 
	DROP TABLE [dbo].[Role];
IF OBJECT_ID('dbo.Team', 'U') IS NOT NULL 
	DROP TABLE [dbo].[Team];
IF OBJECT_ID('dbo.User', 'U') IS NOT NULL 
	DROP TABLE [dbo].[User];
IF OBJECT_ID('dbo.Tenant', 'U') IS NOT NULL 
	DROP TABLE [dbo].[Tenant];