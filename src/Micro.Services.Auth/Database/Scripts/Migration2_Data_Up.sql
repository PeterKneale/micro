SET IDENTITY_INSERT [User] ON
INSERT INTO [dbo].[User]([Id],[TenantId],[Email],[Password]) VALUES (1,1,'user1@a.com','password')
INSERT INTO [dbo].[User]([Id],[TenantId],[Email],[Password]) VALUES (2,1,'user2@a.com','password')
INSERT INTO [dbo].[User]([Id],[TenantId],[Email],[Password]) VALUES (3,2,'user1@b.com','password')
INSERT INTO [dbo].[User]([Id],[TenantId],[Email],[Password]) VALUES (4,2,'user2@b.com','password')
SET IDENTITY_INSERT [User] OFF

-- User Administration
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (1,'user.create')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (1,'user.delete')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (1,'user.edit')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (1,'user.view')

-- Team Administration
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (1,'team.create')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (1,'team.delete')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (1,'team.edit')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (1,'team.view')

-- Content Administration
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (2,'content.create')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (2,'content.delete')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (2,'content.edit')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (2,'content.view')

-- User Administration
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (3,'user.create')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (3,'user.delete')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (3,'user.edit')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (3,'user.view')

-- Team Administration
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (3,'team.create')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (3,'team.delete')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (3,'team.edit')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (3,'team.view')

-- Content Administration
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (4,'content.create')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (4,'content.delete')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (4,'content.edit')
INSERT INTO [dbo].[UserPermission]([UserId],[Name]) VALUES (4,'content.view')
