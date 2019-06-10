CREATE TABLE [dbo].[User]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UNIQUE_UserEmail] UNIQUE NONCLUSTERED ([Email] ASC),
)

CREATE TABLE [dbo].[UserPermission]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	CONSTRAINT [PK_UserPermission] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UNIQUE_UserPermissionName] UNIQUE NONCLUSTERED ([UserId] ASC, [Name] ASC)
)

ALTER TABLE [dbo].[UserPermission]	ADD CONSTRAINT FK_UserPermission_User	FOREIGN KEY ( UserId )		REFERENCES [dbo].[User]		( Id ) ON UPDATE NO ACTION ON DELETE NO ACTION 
