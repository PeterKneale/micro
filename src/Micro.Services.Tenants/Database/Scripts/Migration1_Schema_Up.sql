CREATE TABLE [dbo].[Tenant]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Host] [nvarchar](255) NOT NULL,
	CONSTRAINT [PK_Tenant] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UNIQUE_Tenant_Name] UNIQUE NONCLUSTERED ([Name] ASC),
	CONSTRAINT [UNQIUE_Tenant_Host] UNIQUE NONCLUSTERED ([Host] ASC)
)

CREATE TABLE [dbo].[User]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NOT NULL,
	[FirstName] [nvarchar](255) NOT NULL,
	[LastName] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UNIQUE_User_Email] UNIQUE NONCLUSTERED ([Email] ASC, [TenantId] ASC),
)

CREATE TABLE [dbo].[Team]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL
	CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UNIQUE_Team_Name] UNIQUE NONCLUSTERED ([Name] ASC, [TenantId] ASC)
)

CREATE TABLE [dbo].[Role]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UNIQUE_Role_Name] UNIQUE NONCLUSTERED ([Name] ASC, [TenantId] ASC)
)

CREATE TABLE [dbo].[UserTeam]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[TeamId] [int] NOT NULL,
	CONSTRAINT [PK_UserTeam] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UNIQUE_UserTeam_User_Team] UNIQUE NONCLUSTERED ([UserId] ASC, [TeamId] ASC)
)

CREATE TABLE [dbo].[TeamRole]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NOT NULL,
	[TeamId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	CONSTRAINT [PK_TeamRole] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UNIQUE_TeamRole_Team_Role] UNIQUE NONCLUSTERED ([TeamId] ASC, [RoleId] ASC)
)

CREATE TABLE [dbo].[RolePermission]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	CONSTRAINT [PK_RolePermission] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UNIQUE_RolePermission_Name] UNIQUE NONCLUSTERED ([RoleId] ASC, [Name] ASC)
)

ALTER TABLE [dbo].[User]			ADD CONSTRAINT FK_User_Tenant			FOREIGN KEY ( TenantId )	REFERENCES [dbo].[Tenant]	( Id ) ON UPDATE NO ACTION ON DELETE NO ACTION 
ALTER TABLE [dbo].[Team]			ADD CONSTRAINT FK_Team_Tenant			FOREIGN KEY ( TenantId )	REFERENCES [dbo].[Tenant]	( Id ) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [dbo].[Role]			ADD CONSTRAINT FK_Role_Tenant			FOREIGN KEY ( TenantId )	REFERENCES [dbo].[Tenant]	( Id ) ON UPDATE NO ACTION ON DELETE NO ACTION
ALTER TABLE [dbo].[UserTeam]		ADD CONSTRAINT FK_UserTeam_User			FOREIGN KEY ( UserId )		REFERENCES [dbo].[User]		( Id ) ON UPDATE NO ACTION ON DELETE NO ACTION 
ALTER TABLE [dbo].[UserTeam]		ADD CONSTRAINT FK_UserTeam_Team			FOREIGN KEY ( TeamId )		REFERENCES [dbo].[Team]		( Id ) ON UPDATE NO ACTION ON DELETE NO ACTION  
ALTER TABLE [dbo].[TeamRole]		ADD CONSTRAINT FK_TeamRole_Team			FOREIGN KEY ( TeamId )		REFERENCES [dbo].[Team]		( Id ) ON UPDATE NO ACTION ON DELETE NO ACTION  
ALTER TABLE [dbo].[TeamRole]		ADD CONSTRAINT FK_TeamRole_Role			FOREIGN KEY ( RoleId )		REFERENCES [dbo].[Role]		( Id ) ON UPDATE NO ACTION ON DELETE NO ACTION   
ALTER TABLE [dbo].[RolePermission]	ADD CONSTRAINT FK_RolePermission_Role	FOREIGN KEY ( RoleId )		REFERENCES [dbo].[Role]		( Id ) ON UPDATE NO ACTION ON DELETE NO ACTION 
