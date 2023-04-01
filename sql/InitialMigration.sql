SET QUOTED_IDENTIFIER ON;

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'DomiviceUsers')
BEGIN
        CREATE DATABASE [DomiviceUsers]
END;
GO

USE [DomiviceUsers]
GO
    
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Users] (
    [Id] uniqueidentifier NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [PhoneNumber] nvarchar(max) NOT NULL,
    [PhoneCountryCode] nvarchar(max) NOT NULL,
    [DisplayLanguage] nvarchar(max) NOT NULL,
    [UserBio] nvarchar(max) NOT NULL,
    [Website] nvarchar(max) NOT NULL,
    [EntryInstructions] nvarchar(max) NOT NULL,
    [Created] datetime2 NOT NULL,
    [LastModified] datetime2 NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [HomeAddresses] (
    [UserId] uniqueidentifier NOT NULL,
    [Street] nvarchar(max) NOT NULL,
    [City] nvarchar(max) NOT NULL,
    [State] nvarchar(max) NOT NULL,
    [Country] nvarchar(max) NOT NULL,
    [PostalCode] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_HomeAddresses] PRIMARY KEY ([UserId]),
    CONSTRAINT [FK_HomeAddresses_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserLanguages] (
    [Id] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    [LanguageCode] nvarchar(max) NOT NULL,
    [Created] datetime2 NOT NULL,
    [LastModified] datetime2 NOT NULL,
    CONSTRAINT [PK_UserLanguages] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserLanguages_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserSocialMediaUrls] (
    [Id] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    [Site] nvarchar(max) NOT NULL,
    [Url] nvarchar(max) NOT NULL,
    [Created] datetime2 NOT NULL,
    [LastModified] datetime2 NOT NULL,
    CONSTRAINT [PK_UserSocialMediaUrls] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserSocialMediaUrls_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_UserLanguages_UserId] ON [UserLanguages] ([UserId]);
GO

CREATE INDEX [IX_UserSocialMediaUrls_UserId] ON [UserSocialMediaUrls] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230307232453_Initial', N'6.0.14');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'Website');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Users] ALTER COLUMN [Website] nvarchar(max) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'UserBio');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Users] ALTER COLUMN [UserBio] nvarchar(max) NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'EntryInstructions');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Users] ALTER COLUMN [EntryInstructions] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230317000008_UpdatesNullableFields', N'6.0.14');
GO

COMMIT;
GO

