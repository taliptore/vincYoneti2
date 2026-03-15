-- Apply Role, Permission, Menu, RolePermission, MenuRole tables
-- Run this against your database if EF migrations did not apply.

IF OBJECT_ID(N'[Roles]') IS NULL
BEGIN
    CREATE TABLE [Roles] (
        [Id] uniqueidentifier NOT NULL,
        [RoleName] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
    );
    CREATE UNIQUE INDEX [IX_Roles_RoleName] ON [Roles] ([RoleName]);
END
GO

IF OBJECT_ID(N'[Permissions]') IS NULL
BEGIN
    CREATE TABLE [Permissions] (
        [Id] uniqueidentifier NOT NULL,
        [ModuleName] nvarchar(100) NOT NULL,
        [ActionName] nvarchar(50) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Permissions] PRIMARY KEY ([Id])
    );
END
GO

IF OBJECT_ID(N'[Menus]') IS NULL
BEGIN
    CREATE TABLE [Menus] (
        [Id] uniqueidentifier NOT NULL,
        [Title] nvarchar(200) NOT NULL,
        [Icon] nvarchar(100) NULL,
        [Route] nvarchar(500) NULL,
        [ParentId] uniqueidentifier NULL,
        [OrderNo] int NOT NULL,
        [ModuleName] nvarchar(100) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Menus] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Menus_Menus_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Menus] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_Menus_ParentId] ON [Menus] ([ParentId]);
END
GO

IF OBJECT_ID(N'[RolePermissions]') IS NULL
BEGIN
    CREATE TABLE [RolePermissions] (
        [RoleId] uniqueidentifier NOT NULL,
        [PermissionId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_RolePermissions] PRIMARY KEY ([RoleId], [PermissionId]),
        CONSTRAINT [FK_RolePermissions_Permissions_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [Permissions] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RolePermissions_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_RolePermissions_PermissionId] ON [RolePermissions] ([PermissionId]);
END
GO

IF OBJECT_ID(N'[MenuRoles]') IS NULL
BEGIN
    CREATE TABLE [MenuRoles] (
        [MenuId] uniqueidentifier NOT NULL,
        [RoleId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_MenuRoles] PRIMARY KEY ([MenuId], [RoleId]),
        CONSTRAINT [FK_MenuRoles_Menus_MenuId] FOREIGN KEY ([MenuId]) REFERENCES [Menus] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_MenuRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_MenuRoles_RoleId] ON [MenuRoles] ([RoleId]);
END
GO

-- Mark migrations as applied (if not already)
IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260315120000_AddSliderAndGallery')
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260315120000_AddSliderAndGallery', N'8.0.11');
IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260315140000_AddRoleAndMenuTables')
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260315140000_AddRoleAndMenuTables', N'8.0.11');
GO
