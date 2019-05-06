
SET IDENTITY_INSERT [Tenant] ON
INSERT INTO [dbo].[Tenant]([Id],[Name],[Host]) VALUES (1,'a','a.simplicate.net')
INSERT INTO [dbo].[Tenant]([Id],[Name],[Host]) VALUES (2,'b','b.simplicate.net')
SET IDENTITY_INSERT [Tenant] OFF

SET IDENTITY_INSERT [Team] ON
INSERT INTO [dbo].[Team]([Id],[TenantId],[Name]) VALUES (1,1,'Administrators')
INSERT INTO [dbo].[Team]([Id],[TenantId],[Name]) VALUES (2,1,'Content Team')
INSERT INTO [dbo].[Team]([Id],[TenantId],[Name]) VALUES (3,2,'Administrators')
INSERT INTO [dbo].[Team]([Id],[TenantId],[Name]) VALUES (4,2,'Content Team')
SET IDENTITY_INSERT [Team] OFF

SET IDENTITY_INSERT [User] ON
INSERT INTO [dbo].[User]([Id],[TenantId],[FirstName],[LastName],[Email],[Password]) VALUES (1,1,'user a1','user a1','user1@a.com','password')
INSERT INTO [dbo].[User]([Id],[TenantId],[FirstName],[LastName],[Email],[Password]) VALUES (2,1,'user a2','user a2','user2@a.com','password')
INSERT INTO [dbo].[User]([Id],[TenantId],[FirstName],[LastName],[Email],[Password]) VALUES (3,2,'user b1','user b1','user1@b.com','password')
INSERT INTO [dbo].[User]([Id],[TenantId],[FirstName],[LastName],[Email],[Password]) VALUES (4,2,'user b2','user b2','user2@b.com','password')
SET IDENTITY_INSERT [User] OFF

SET IDENTITY_INSERT [Role] ON
INSERT INTO [dbo].[Role]([Id],[TenantId],[Name]) VALUES (1,1,'User Administration')
INSERT INTO [dbo].[Role]([Id],[TenantId],[Name]) VALUES (2,1,'Team Administration')
INSERT INTO [dbo].[Role]([Id],[TenantId],[Name]) VALUES (3,1,'Content Administration')
INSERT INTO [dbo].[Role]([Id],[TenantId],[Name]) VALUES (4,2,'User Administration')
INSERT INTO [dbo].[Role]([Id],[TenantId],[Name]) VALUES (5,2,'Team Administration')
INSERT INTO [dbo].[Role]([Id],[TenantId],[Name]) VALUES (6,2,'Content Administration')
SET IDENTITY_INSERT [Role] OFF

-- User Administration
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (1,1,'user.create')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (1,1,'user.delete')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (1,1,'user.edit')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (1,1,'user.view')

-- Team Administration
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (1,2,'team.create')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (1,2,'team.delete')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (1,2,'team.edit')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (1,2,'team.view')

-- Content Administration
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (1,3,'content.create')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (1,3,'content.delete')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (1,3,'content.edit')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (1,3,'content.view')

-- User Administration
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (2,4,'user.create')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (2,4,'user.delete')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (2,4,'user.edit')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (2,4,'user.view')

-- Team Administration
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (2,5,'team.create')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (2,5,'team.delete')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (2,5,'team.edit')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (2,5,'team.view')

-- Content Administration
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (2,6,'content.create')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (2,6,'content.delete')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (2,6,'content.edit')
INSERT INTO [dbo].[RolePermission]([TenantId],[RoleId],[Name]) VALUES (2,6,'content.view')

-- User Teams
INSERT INTO [dbo].[UserTeam]([TenantId],[UserId],[TeamId]) VALUES (1,1,1) 
INSERT INTO [dbo].[UserTeam]([TenantId],[UserId],[TeamId]) VALUES (1,2,2)
INSERT INTO [dbo].[UserTeam]([TenantId],[UserId],[TeamId]) VALUES (2,3,3)
INSERT INTO [dbo].[UserTeam]([TenantId],[UserId],[TeamId]) VALUES (2,4,4)

-- Team Roles
INSERT INTO [dbo].[TeamRole]([TenantId],[TeamId],[RoleId]) VALUES (1,1,1) 
INSERT INTO [dbo].[TeamRole]([TenantId],[TeamId],[RoleId]) VALUES (1,1,2)
INSERT INTO [dbo].[TeamRole]([TenantId],[TeamId],[RoleId]) VALUES (1,2,3)
INSERT INTO [dbo].[TeamRole]([TenantId],[TeamId],[RoleId]) VALUES (2,3,4) 
INSERT INTO [dbo].[TeamRole]([TenantId],[TeamId],[RoleId]) VALUES (2,3,5) 
INSERT INTO [dbo].[TeamRole]([TenantId],[TeamId],[RoleId]) VALUES (2,4,6) 