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

CREATE TABLE [Departments] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Logs] (
    [Id] int NOT NULL IDENTITY,
    [LogLevel] nvarchar(max) NOT NULL,
    [Message] nvarchar(max) NOT NULL,
    [Exception] nvarchar(max) NOT NULL,
    [TimeStamp] datetime2 NOT NULL,
    CONSTRAINT [PK_Logs] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Employees] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [Role] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [DepartmentId] int NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Employees_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Tasks] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Status] int NOT NULL,
    [Priority] int NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [DueDate] datetime2 NOT NULL,
    [EmployeeId] int NOT NULL,
    CONSTRAINT [PK_Tasks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Tasks_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [TaskFiles] (
    [Id] int NOT NULL IDENTITY,
    [FileName] nvarchar(max) NOT NULL,
    [FilePath] nvarchar(max) NOT NULL,
    [ContentType] nvarchar(max) NOT NULL,
    [UploadedDate] datetime2 NOT NULL,
    [TaskId] int NOT NULL,
    CONSTRAINT [PK_TaskFiles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TaskFiles_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [Tasks] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Employees_DepartmentId] ON [Employees] ([DepartmentId]);
GO

CREATE INDEX [IX_TaskFiles_TaskId] ON [TaskFiles] ([TaskId]);
GO

CREATE INDEX [IX_Tasks_EmployeeId] ON [Tasks] ([EmployeeId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260701075826_InitialCreate', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[Departments]'))
    SET IDENTITY_INSERT [Departments] ON;
INSERT INTO [Departments] ([Id], [Description], [Name])
VALUES (1, N'Şirketin idari ve genel yönetim işleri.', N'Yönetim'),
(2, N'Yazılım projeleri geliştirme ve Ar-Ge süreçleri.', N'Yazılım Geliştirme'),
(3, N'İşe alım, eğitim ve çalışan ilişkileri süreçleri.', N'İnsan Kaynakları');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[Departments]'))
    SET IDENTITY_INSERT [Departments] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedDate', N'DepartmentId', N'Email', N'FirstName', N'IsActive', N'LastName', N'PasswordHash', N'Role') AND [object_id] = OBJECT_ID(N'[Employees]'))
    SET IDENTITY_INSERT [Employees] ON;
INSERT INTO [Employees] ([Id], [CreatedDate], [DepartmentId], [Email], [FirstName], [IsActive], [LastName], [PasswordHash], [Role])
VALUES (1, '2026-07-01T00:00:00.0000000', 1, N'admin@sirket.com', N'Ahmet', CAST(1 AS bit), N'Yılmaz', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Admin');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedDate', N'DepartmentId', N'Email', N'FirstName', N'IsActive', N'LastName', N'PasswordHash', N'Role') AND [object_id] = OBJECT_ID(N'[Employees]'))
    SET IDENTITY_INSERT [Employees] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260701075904_AddSeedData', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Tasks]') AND [c].[name] = N'Title');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Tasks] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Tasks] ALTER COLUMN [Title] nvarchar(max) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Tasks]') AND [c].[name] = N'Description');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Tasks] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Tasks] ALTER COLUMN [Description] nvarchar(max) NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TaskFiles]') AND [c].[name] = N'FilePath');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [TaskFiles] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [TaskFiles] ALTER COLUMN [FilePath] nvarchar(max) NULL;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TaskFiles]') AND [c].[name] = N'FileName');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [TaskFiles] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [TaskFiles] ALTER COLUMN [FileName] nvarchar(max) NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TaskFiles]') AND [c].[name] = N'ContentType');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [TaskFiles] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [TaskFiles] ALTER COLUMN [ContentType] nvarchar(max) NULL;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Logs]') AND [c].[name] = N'Message');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Logs] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Logs] ALTER COLUMN [Message] nvarchar(max) NULL;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Logs]') AND [c].[name] = N'LogLevel');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Logs] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Logs] ALTER COLUMN [LogLevel] nvarchar(max) NULL;
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Logs]') AND [c].[name] = N'Exception');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Logs] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Logs] ALTER COLUMN [Exception] nvarchar(max) NULL;
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employees]') AND [c].[name] = N'Role');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Employees] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [Employees] ALTER COLUMN [Role] nvarchar(max) NULL;
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employees]') AND [c].[name] = N'PasswordHash');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Employees] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [Employees] ALTER COLUMN [PasswordHash] nvarchar(max) NULL;
GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employees]') AND [c].[name] = N'LastName');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Employees] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [Employees] ALTER COLUMN [LastName] nvarchar(max) NULL;
GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employees]') AND [c].[name] = N'FirstName');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Employees] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [Employees] ALTER COLUMN [FirstName] nvarchar(max) NULL;
GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employees]') AND [c].[name] = N'Email');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Employees] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [Employees] ALTER COLUMN [Email] nvarchar(max) NULL;
GO

ALTER TABLE [Employees] ADD [IsPasswordResetRequested] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Departments]') AND [c].[name] = N'Name');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Departments] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [Departments] ALTER COLUMN [Name] nvarchar(max) NULL;
GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Departments]') AND [c].[name] = N'Description');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Departments] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [Departments] ALTER COLUMN [Description] nvarchar(max) NULL;
GO

UPDATE [Employees] SET [IsPasswordResetRequested] = CAST(0 AS bit)
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260702112151_AddPasswordResetRequestColumn', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Employees] ADD [Title] nvarchar(max) NULL;
GO

UPDATE [Employees] SET [Role] = N'Yönetici', [Title] = N'Genel Müdür'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260704074233_AddTitleColumnToEmployee', N'8.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [TaskComments] (
    [Id] int NOT NULL IDENTITY,
    [TaskId] int NOT NULL,
    [EmployeeId] int NOT NULL,
    [CommentText] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_TaskComments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TaskComments_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [Tasks] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TaskComments_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Announcements] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    [EmployeeId] int NOT NULL,
    CONSTRAINT [PK_Announcements] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Announcements_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_TaskComments_TaskId] ON [TaskComments] ([TaskId]);
GO
CREATE INDEX [IX_TaskComments_EmployeeId] ON [TaskComments] ([EmployeeId]);
GO
CREATE INDEX [IX_Announcements_EmployeeId] ON [Announcements] ([EmployeeId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260722125500_AddCommentsAndAnnouncements', N'8.0.0');
GO

ALTER TABLE [Tasks] ADD [Visibility] int NOT NULL DEFAULT 0;
GO

COMMIT;
GO

