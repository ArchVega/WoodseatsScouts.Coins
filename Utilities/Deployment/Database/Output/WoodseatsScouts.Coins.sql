
while(exists(select 1 from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_TYPE='FOREIGN KEY'))
begin
 declare @sql nvarchar(2000)
 SELECT TOP 1 @sql=('ALTER TABLE ' + TABLE_SCHEMA + '.[' + TABLE_NAME + '] DROP CONSTRAINT [' + CONSTRAINT_NAME + ']')
 FROM information_schema.table_constraints
 WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'
 exec (@sql)
end

while(exists(select 1 from INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'))
begin
 declare @sql1 nvarchar(2000)
 SELECT TOP 1 @sql1=('DROP TABLE ' + TABLE_SCHEMA + '.[' + TABLE_NAME + ']')
 FROM INFORMATION_SCHEMA.TABLES
 WHERE TABLE_TYPE = 'BASE TABLE'
exec (@sql1) 
end

--USE [WoodseatsScouts.Coins]
--GO

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

CREATE TABLE [ActivityBases] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_ActivityBases] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ErrorLogs] (
    [Id] int NOT NULL IDENTITY,
    [LoggedAt] datetime2 NOT NULL,
    [Message] nvarchar(max) NOT NULL,
    [StackTrace] nvarchar(max) NULL,
    [Path] nvarchar(max) NOT NULL,
    [Method] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_ErrorLogs] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ScoutGroups] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    CONSTRAINT [PK_ScoutGroups] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ScoutSections] (
    [Code] char(1) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_ScoutSections] PRIMARY KEY ([Code])
);
GO

CREATE TABLE [ScoutMembers] (
    [Id] int NOT NULL IDENTITY,
    [Code] AS     'M'
    + RIGHT('000' + CAST([ScoutGroupId] AS VARCHAR(3)), 3)
    + ScoutSectionCode
    + RIGHT('000' + CAST([Number] AS VARCHAR(3)), 3),
    [Number] int NOT NULL,
    [FirstName] nvarchar(100) NOT NULL,
    [LastName] nvarchar(100) NULL,
    [ScoutGroupId] int NOT NULL,
    [ScoutSectionCode] char(1) NOT NULL,
    [Clue1State] nvarchar(100) NULL,
    [Clue2State] nvarchar(100) NULL,
    [Clue3State] nvarchar(100) NULL,
    [IsDayVisitor] bit NOT NULL,
    [HasImage] bit NOT NULL,
    CONSTRAINT [PK_ScoutMembers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ScoutMembers_ScoutGroups_ScoutGroupId] FOREIGN KEY ([ScoutGroupId]) REFERENCES [ScoutGroups] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ScoutMembers_ScoutSections_ScoutSectionCode] FOREIGN KEY ([ScoutSectionCode]) REFERENCES [ScoutSections] ([Code]) ON DELETE CASCADE
);
GO

CREATE TABLE [Coins] (
    [Id] int NOT NULL IDENTITY,
    [ActivityBaseSequenceNumber] int NOT NULL,
    [ActivityBaseId] int NOT NULL,
    [Value] int NOT NULL,
    [Code] AS     'C'
    + RIGHT('0000' + CAST([ActivityBaseSequenceNumber] AS VARCHAR(4)), 4)
    + RIGHT('000' + CAST([ActivityBaseId] AS VARCHAR(3)), 3)
    + RIGHT('000' + CAST([Value] AS VARCHAR(3)), 3),
    [MemberId] int NULL,
    [LockUntil] datetime2 NULL,
    CONSTRAINT [PK_Coins] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Coins_ActivityBases_ActivityBaseId] FOREIGN KEY ([ActivityBaseId]) REFERENCES [ActivityBases] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Coins_ScoutMembers_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [ScoutMembers] ([Id])
);
GO

CREATE TABLE [ScanSessions] (
    [Id] int NOT NULL IDENTITY,
    [ScoutMemberId] int NOT NULL,
    [CompletedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_ScanSessions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ScanSessions_ScoutMembers_ScoutMemberId] FOREIGN KEY ([ScoutMemberId]) REFERENCES [ScoutMembers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [ScannedCoins] (
    [Id] int NOT NULL IDENTITY,
    [ScanSessionId] int NOT NULL,
    [CoinId] int NOT NULL,
    [PointsOverride] int NULL,
    CONSTRAINT [PK_ScannedCoins] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ScannedCoins_Coins_CoinId] FOREIGN KEY ([CoinId]) REFERENCES [Coins] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ScannedCoins_ScanSessions_ScanSessionId] FOREIGN KEY ([ScanSessionId]) REFERENCES [ScanSessions] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[ActivityBases]'))
    SET IDENTITY_INSERT [ActivityBases] ON;
INSERT INTO [ActivityBases] ([Id], [Name])
VALUES (1, N'Archery'),
(2, N'Abseiling'),
(3, N'Aerial Trek'),
(4, N'Aeroball'),
(5, N'Bouldering'),
(6, N'Bushcraft'),
(7, N'Campfire'),
(8, N'Canoeing'),
(9, N'Caving'),
(10, N'Fencing'),
(11, N'Hike'),
(12, N'Hillwalking'),
(13, N'Kayaking'),
(14, N'Orienteering'),
(15, N'Pioneering'),
(16, N'Powerboating'),
(17, N'Raft Building'),
(18, N'Sailing'),
(19, N'Tomahawk throwing'),
(20, N'Zip wire'),
(21, N'46th St Pauls'),
(22, N'146th Old Norton'),
(23, N'173rd Woodhouse'),
(24, N'186th Manor'),
(25, N'219th Stradbroke'),
(26, N'229th Greenhill'),
(27, N'246th Beauchief'),
(28, N'270th Intake'),
(29, N'273rd Handsworth'),
(31, N'280th Norton'),
(32, N'297th Bradway'),
(33, N'49th Beighton'),
(34, N'69th Mosborough'),
(35, N'74th Oak Street'),
(99, N'Misc');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[ActivityBases]'))
    SET IDENTITY_INSERT [ActivityBases] OFF;
GO

CREATE INDEX [IX_Coins_ActivityBaseId] ON [Coins] ([ActivityBaseId]);
GO

CREATE UNIQUE INDEX [IX_Coins_Code] ON [Coins] ([Code]);
GO

CREATE INDEX [IX_Coins_MemberId] ON [Coins] ([MemberId]);
GO

CREATE INDEX [IX_ScannedCoins_CoinId] ON [ScannedCoins] ([CoinId]);
GO

CREATE INDEX [IX_ScannedCoins_ScanSessionId] ON [ScannedCoins] ([ScanSessionId]);
GO

CREATE INDEX [IX_ScanSessions_ScoutMemberId] ON [ScanSessions] ([ScoutMemberId]);
GO

CREATE UNIQUE INDEX [IX_ScoutMembers_Code] ON [ScoutMembers] ([Code]);
GO

CREATE INDEX [IX_ScoutMembers_ScoutGroupId] ON [ScoutMembers] ([ScoutGroupId]);
GO

CREATE INDEX [IX_ScoutMembers_ScoutSectionCode] ON [ScoutMembers] ([ScoutSectionCode]);
GO

CREATE UNIQUE INDEX [IX_ScoutSections_Code] ON [ScoutSections] ([Code]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260425164919_Initial', N'8.0.3');
GO

COMMIT;
GO



--if not exists(select * from sys.database_principals where name = 'ScoutsUser')
--begin
--CREATE USER ScoutsUser FOR LOGIN ScoutsUser WITH DEFAULT_SCHEMA = [WoodseatsScouts.Coins]
--end

INSERT [dbo].[ScoutSections] ([Code], [Name]) VALUES (N'A', N'Adults')
INSERT [dbo].[ScoutSections] ([Code], [Name]) VALUES (N'B', N'Beavers')
INSERT [dbo].[ScoutSections] ([Code], [Name]) VALUES (N'C', N'Cubs')
INSERT [dbo].[ScoutSections] ([Code], [Name]) VALUES (N'E', N'Explorers')
INSERT [dbo].[ScoutSections] ([Code], [Name]) VALUES (N'S', N'Scouts')
INSERT [dbo].[ScoutSections] ([Code], [Name]) VALUES (N'Q', N'Squirrels')
GO

-- coins
SET IDENTITY_INSERT dbo.Coins ON;
    
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (1, 1, 99, 3, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (2, 1, 99, 5, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (3, 1, 99, 9, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (4, 1, 99, 7, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (5, 1, 1, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (6, 1, 1, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (7, 1, 2, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (8, 1, 2, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (9, 1, 3, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (10, 1, 3, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (11, 1, 4, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (12, 1, 4, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (13, 1, 5, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (14, 1, 5, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (15, 1, 6, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (16, 1, 6, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (17, 1, 7, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (18, 1, 7, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (19, 1, 8, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (20, 1, 8, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (21, 1, 9, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (22, 1, 9, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (23, 1, 10, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (24, 1, 10, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (25, 1, 11, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (26, 1, 11, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (27, 1, 12, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (28, 1, 12, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (29, 1, 13, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (30, 1, 13, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (31, 1, 14, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (32, 1, 14, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (33, 1, 15, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (34, 1, 15, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (35, 1, 16, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (36, 1, 16, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (37, 1, 17, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (38, 1, 17, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (39, 1, 18, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (40, 1, 18, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (41, 1, 19, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (42, 1, 19, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (43, 1, 20, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (44, 1, 20, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (45, 2, 99, 3, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (46, 2, 99, 5, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (47, 2, 99, 9, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (48, 2, 99, 7, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (49, 2, 1, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (50, 2, 1, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (51, 2, 2, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (52, 2, 2, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (53, 2, 3, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (54, 2, 3, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (55, 2, 4, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (56, 2, 4, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (57, 2, 5, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (58, 2, 5, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (59, 2, 6, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (60, 2, 6, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (61, 2, 7, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (62, 2, 7, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (63, 2, 8, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (64, 2, 8, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (65, 2, 9, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (66, 2, 9, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (67, 2, 10, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (68, 2, 10, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (69, 2, 11, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (70, 2, 11, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (71, 2, 12, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (72, 2, 12, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (73, 2, 13, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (74, 2, 13, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (75, 2, 14, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (76, 2, 14, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (77, 2, 15, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (78, 2, 15, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (79, 2, 16, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (80, 2, 16, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (81, 2, 17, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (82, 2, 17, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (83, 2, 18, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (84, 2, 18, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (85, 2, 19, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (86, 2, 19, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (87, 2, 20, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (88, 2, 20, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (89, 3, 99, 3, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (90, 3, 99, 5, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (91, 3, 99, 9, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (92, 3, 99, 7, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (93, 3, 1, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (94, 3, 1, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (95, 3, 2, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (96, 3, 2, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (97, 3, 3, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (98, 3, 3, 20, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (99, 3, 4, 10, null, null);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.Coins (Id, ActivityBaseSequenceNumber, ActivityBaseId, Value, MemberId, LockUntil) VALUES (100, 3, 4, 20, null, null)

    SET IDENTITY_INSERT dbo.Coins OFF;

-- scoutgroups
SET IDENTITY_INSERT dbo.ScoutGroups ON;
    
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutGroups (Id, Name) VALUES (46, N'St Pauls');
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutGroups (Id, Name) VALUES (69, N'Mosborough');
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutGroups (Id, Name) VALUES (74, N'Oak Street');
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutGroups (Id, Name) VALUES (99, N'Woodseats Explorers');
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutGroups (Id, Name) VALUES (146, N'Old Norton');
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutGroups (Id, Name) VALUES (181, N'St Chads');
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutGroups (Id, Name) VALUES (229, N'Greenhill Methodist');
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutGroups (Id, Name) VALUES (246, N'Beauchief');
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutGroups (Id, Name) VALUES (265, N'Greenhill');
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutGroups (Id, Name) VALUES (280, N'Norton');
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutGroups (Id, Name) VALUES (297, N'Bradway');
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutGroups (Id, Name) VALUES (500, N'229th Greenhill');
 INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutGroups (Id, Name) VALUES (501, N'Woodseats Explorers');
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutGroups (Id, Name) VALUES (502, N'270th Intake');
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutGroups (Id, Name) VALUES (503, N'280th Norton');
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutGroups (Id, Name) VALUES (504, N'92nd Woodhouse');

    SET IDENTITY_INSERT dbo.ScoutGroups OFF;

-- members
SET IDENTITY_INSERT dbo.ScoutMembers ON;
    
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (1, 1, N'Alicia', N'Buckley', 46, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (2, 5, N'Isabella', N'Rose', 46, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (3, 2, N'Harriet', N'Lane', 46, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (4, 6, N'Esme', N'Hunt', 46, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (5, 3, N'Faith', N'Carr', 46, N'B', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (6, 4, N'Freya', N'Ferguson', 46, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (7, 7, N'Lauren', N'McDonald', 46, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (8, 8, N'Jade', N'Faulkner', 46, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (9, 1, N'Bradley', N'Bolton', 46, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (10, 5, N'Deborah', N'Vaughan', 46, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (11, 2, N'Zane', N'Sutton', 46, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (12, 3, N'Rose', N'Ross', 46, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (13, 4, N'Delilah', N'Godfrey', 46, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (14, 6, N'Edith', N'English', 46, N'C', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (15, 7, N'Paul', N'Poole', 46, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (16, 8, N'Julianne', N'Brooks', 46, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (17, 9, N'Damian', N'McLean', 46, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (18, 10, N'Jocelyn', N'York', 46, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (19, 11, N'Andrea', N'Mercer', 46, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (20, 12, N'Adele', N'Cassidy', 46, N'C', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (21, 13, N'Owen', N'Hamilton', 46, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (22, 14, N'Marianne', N'Knight', 46, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (23, 15, N'Adeline', N'Cook', 46, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (24, 16, N'Jacob', N'Peck', 46, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (25, 17, N'Katherine', N'Travis', 46, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (26, 1, N'Lily', N'Carroll', 46, N'S', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (27, 2, N'Ferdinand', N'Griffiths', 46, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (28, 3, N'Cedric', N'Holmes', 46, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (29, 4, N'Leah', N'Boyd', 46, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (30, 5, N'Carl', N'Steele', 46, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (31, 6, N'Keira', N'Ingram', 46, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (32, 7, N'Francis', N'Underwood', 46, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (33, 8, N'Henry', N'Hutchinson', 46, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (34, 9, N'Gabriella', N'Palmer', 46, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (35, 10, N'Olivia', N'Nicholson', 46, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (36, 11, N'Bobby', N'Osborne', 46, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (37, 12, N'Alice', N'Collins', 46, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (38, 13, N'Joanna', N'Stephenson', 46, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (39, 14, N'Grant', N'Allan', 46, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (40, 15, N'Toby', N'Lawson', 46, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (41, 1, N'Isla', N'Mitchell', 74, N'B', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (42, 2, N'John', N'Orton', 74, N'B', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (43, 3, N'Austin', N'Harvey', 74, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (44, 4, N'Jesse', N'Maloney', 74, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (45, 5, N'Rhys', N'Ball', 74, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (46, 6, N'Gwen', N'Crawford', 74, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (47, 7, N'James', N'Parker', 74, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (48, 8, N'Francis', N'Rae', 74, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (49, 9, N'Daisy', N'Dunn', 74, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (50, 10, N'Jonathan', N'Bain', 74, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (51, 11, N'Cara', N'Macdonald', 74, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (52, 12, N'Elliot', N'Cole', 74, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (53, 13, N'Fabian', N'Preston', 74, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (54, 14, N'Rebecca', N'Stokes', 74, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (55, 1, N'Anna', N'Green', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (56, 2, N'Darryl', N'Griffith', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (57, 3, N'Gemma', N'Blair', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (58, 4, N'Harvey', N'Austin', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (59, 5, N'Kevin', N'Mackenzie', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (60, 6, N'Eleanor', N'Rothwell', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (61, 7, N'Jessica', N'Ellison', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (62, 8, N'Ella', N'White', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (63, 9, N'Greg', N'Clements', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (64, 10, N'Riley', N'Bright', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (65, 11, N'Vincent', N'Bradshaw', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (66, 12, N'Harry', N'Winthrop', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (67, 13, N'Bethany', N'Whitman', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (68, 15, N'June', N'Riley', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (69, 16, N'Jackson', N'Lewis', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (70, 17, N'Lisa', N'Wood', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (71, 18, N'Andrew', N'Adams', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (72, 19, N'Martin', N'Clark', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (73, 20, N'Byron', N'Finch', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (74, 21, N'Kaitlyn', N'Walsh', 74, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (75, 1, N'Amanda', N'Newman', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (76, 2, N'Jordan', N'Kingston', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (77, 3, N'Frank', N'Davidson', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (78, 4, N'Sadie', N'Benson', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (79, 5, N'Lucy', N'Atkinson', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (80, 6, N'Spencer', N'Barrett', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (81, 8, N'Christopher', N'McGregor', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (82, 9, N'Everett', N'McMillan', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (83, 10, N'Angela', N'Muir', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (84, 11, N'Molly', N'Dyer', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (85, 12, N'Elizabeth', N'Carson', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (86, 13, N'Naomi', N'Sargent', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (87, 14, N'Julian', N'Long', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (88, 15, N'Sean', N'Jordan', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (89, 16, N'Gareth', N'Davies', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (90, 17, N'Johnny', N'Bull', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (91, 18, N'Jane', N'Taylor', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (92, 19, N'Bernice', N'Wilkinson', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (93, 20, N'Blake', N'Warner', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (94, 21, N'Adam', N'McIntosh', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (95, 22, N'Evan', N'Watson', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (96, 23, N'Annabelle', N'Andrews', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (97, 24, N'Joseph', N'Leonard', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (98, 25, N'Samuel', N'Curran', 74, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (99, 1, N'Oliver', N'Delaney', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (100, 2, N'Melissa', N'Howell', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (101, 3, N'Erica', N'Baxter', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (102, 4, N'Peter', N'Elliott', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (103, 5, N'Danielle', N'Stone', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (104, 6, N'Simone', N'Briggs', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (105, 7, N'Megan', N'Neal', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (106, 8, N'Asher', N'Farrell', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (107, 9, N'Stuart', N'Cross', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (108, 10, N'Albert', N'Barker', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (109, 11, N'Nathan', N'Rowe', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (110, 12, N'Rachel', N'Hood', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (111, 13, N'Amelia', N'Ward', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (112, 14, N'Theodore', N'Lucas', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (113, 15, N'Diana', N'Woods', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (114, 16, N'Charles', N'Byrne', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (115, 17, N'Maria', N'Cormack', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (116, 18, N'Hamish', N'Ryan', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (117, 19, N'Noah', N'McKay', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (118, 20, N'Marcus', N'Mills', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (119, 21, N'Mary', N'Grant', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (120, 22, N'Sam', N'Thomas', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (121, 23, N'Jackie', N'Galloway', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (122, 24, N'Hugo', N'Mann', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (123, 25, N'Mabel', N'Short', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (124, 26, N'Gregory', N'Ramsey', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (125, 27, N'Liam', N'Pitt', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (126, 28, N'Evelyn', N'Reid', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (127, 29, N'Ruby', N'McCarthy', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (128, 30, N'Savannah', N'Baldwin', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (129, 31, N'Karen', N'Bennett', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (130, 32, N'Frances', N'Warren', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (131, 33, N'David', N'Radcliffe', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (132, 34, N'Edward', N'Doyle', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (133, 35, N'Martha', N'Sullivan', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (134, 36, N'Finnian', N'Black', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (135, 37, N'Ingrid', N'Sadler', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (136, 38, N'Michael', N'Fleming', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (137, 39, N'Horace', N'Bond', 99, N'E', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (138, 1, N'Celeste', N'Kent', 146, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (139, 2, N'Iris', N'Douglas', 146, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (140, 3, N'Susan', N'Thornton', 146, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (141, 4, N'Cody', N'Bruce', 146, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (142, 5, N'Gwendolyn', N'Branson', 146, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (143, 6, N'Dennis', N'Saunders', 146, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (144, 7, N'Emma', N'Hart', 146, N'B', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (145, 8, N'Eden', N'Harper', 146, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (146, 9, N'Robin', N'Howard', 146, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (147, 10, N'Ivy', N'Quinn', 146, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (148, 11, N'Felix', N'Wallace', 146, N'B', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (149, 12, N'Dylan', N'McFadden', 146, N'B', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (150, 13, N'Lachlan', N'Harris', 146, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (151, 14, N'Mason', N'Hall', 146, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (152, 15, N'Chelsea', N'Oliver', 146, N'B', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (153, 16, N'Phoebe', N'Booth', 146, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (154, 17, N'Gilbert', N'Frost', 146, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (155, 1, N'Josephine', N'Cunningham', 146, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (156, 2, N'Desmond', N'Field', 146, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (157, 3, N'Alison', N'Ainsworth', 146, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (158, 4, N'Edwin', N'Carey', 146, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (159, 5, N'Madeleine', N'Brennan', 146, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (160, 6, N'Tahlia', N'Miller', 146, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (161, 7, N'Franklin', N'Bates', 146, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (162, 8, N'Natalie', N'Allison', 146, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (163, 9, N'Clyde', N'Mooney', 146, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (164, 10, N'Gordon', N'Eaton', 146, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (165, 11, N'Ryan', N'Gates', 146, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (166, 12, N'Laura', N'Young', 146, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (167, 13, N'Sarah', N'Simpson', 146, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (168, 14, N'Conrad', N'Morris', 146, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (169, 15, N'Nathaniel', N'Ellis', 146, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (170, 16, N'Ethan', N'Rogers', 146, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (171, 17, N'Matthew', N'Stewart', 146, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (172, 18, N'Robert', N'Drake', 146, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (173, 19, N'Hugh', N'Mason', 146, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (174, 20, N'Clive', N'Wilson', 146, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (175, 21, N'Lillian', N'Pearson', 146, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (176, 22, N'Jack', N'Lowe', 146, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (177, 23, N'Joshua', N'Kerr', 146, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (178, 1, N'Madison', N'Lambert', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (179, 2, N'Kimberly', N'Gibson', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (180, 3, N'Felicity', N'Rodgers', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (181, 4, N'Jamie', N'Lindsay', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (182, 5, N'Michelle', N'Best', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (183, 6, N'Amy', N'Connor', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (184, 7, N'Chester', N'Rice', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (185, 8, N'Brodie', N'Bentley', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (186, 9, N'Audrey', N'Hudson', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (187, 10, N'Craig', N'Hill', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (188, 11, N'Hannah', N'Cameron', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (189, 12, N'Matilda', N'Clayton', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (190, 13, N'Tara', N'Smythe', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (191, 14, N'Esther', N'Robb', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (192, 15, N'Imogen', N'Bradley', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (193, 16, N'Graham', N'Martin', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (194, 17, N'Brandon', N'Scott', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (195, 18, N'Renee', N'Blackwell', 146, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (196, 1, N'Paige', N'Neill', 181, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (197, 2, N'Lara', N'Gill', 181, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (198, 3, N'Luke', N'Berry', 181, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (199, 4, N'Steven', N'Webster', 181, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (200, 5, N'Emmanuel', N'Williamson', 181, N'S', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (201, 6, N'Jake', N'Kennedy', 181, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (202, 7, N'Scarlett', N'Banks', 181, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (203, 8, N'Ian', N'Dixon', 181, N'S', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (204, 9, N'Archie', N'Armstrong', 181, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (205, 10, N'Fiona', N'Leslie', 181, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (206, 11, N'Barney', N'Mair', 181, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (207, 1, N'Claudia', N'Giles', 229, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (208, 2, N'Clara', N'Barton', 229, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (209, 3, N'Charlotte', N'Lee', 229, N'B', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (210, 4, N'Eva', N'Dawson', 229, N'B', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (211, 5, N'Damian', N'Paterson', 229, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (212, 6, N'Benjamin', N'Hunter', 229, N'B', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (213, 8, N'George', N'Hayes', 229, N'B', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (214, 9, N'Dominic', N'Casey', 229, N'B', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (215, 10, N'Ada', N'Fowler', 229, N'B', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (216, 11, N'Chloe', N'Allanby', 229, N'B', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (217, 12, N'Jasper', N'Burton', 229, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (218, 13, N'Jasmine', N'Roy', 229, N'B', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (219, 1, N'William', N'Archer', 229, N'C', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (220, 3, N'Suzanne', N'Shepherd', 229, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (221, 4, N'Alexander', N'Lang', 229, N'C', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (222, 5, N'Lucas', N'Barnes', 229, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (223, 6, N'Glenn', N'Fuller', 229, N'C', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (224, 7, N'Angus', N'Watt', 229, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (225, 8, N'Caroline', N'Stuart', 229, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (226, 9, N'Victor', N'Nelson', 229, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (227, 10, N'Georgia', N'Henry', 229, N'C', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (228, 12, N'Elias', N'Hurst', 229, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (229, 13, N'Dorothy', N'King', 229, N'C', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (230, 14, N'Cecilia', N'Jenkins', 229, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (231, 15, N'Beth', N'Day', 229, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (232, 16, N'Alan', N'James', 229, N'C', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (233, 17, N'Charlie', N'Garrett', 229, N'C', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (234, 1, N'Ross', N'Higgins', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (235, 2, N'Brian', N'Turner', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (236, 3, N'Blaine', N'Porter', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (237, 4, N'Patricia', N'Mead', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (238, 5, N'Dominic', N'Lloyd', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (239, 6, N'Fletcher', N'Park', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (240, 7, N'Daphne', N'Fletcher', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (241, 8, N'Edgar', N'Flynn', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (242, 9, N'Camilla', N'Barber', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (243, 10, N'Isaac', N'Matthews', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (244, 11, N'Colin', N'Wells', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (245, 12, N'Sebastian', N'Morton', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (246, 13, N'Patrick', N'Robertson', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (247, 14, N'Hector', N'Cooper', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (248, 15, N'Darren', N'Bailey', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (249, 16, N'Jennifer', N'Carter', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (250, 17, N'Julia', N'Norris', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (251, 18, N'Kayla', N'Chapman', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (252, 19, N'Tabitha', N'Savage', 229, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (253, 1, N'Sabrina', N'Hardy', 246, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (254, 2, N'Bianca', N'Clarke', 246, N'B', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (255, 1, N'Emily', N'Heath', 246, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (256, 2, N'Anthony', N'Moran', 246, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (257, 3, N'Aaron', N'Blackburn', 246, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (258, 4, N'Richard', N'Robinson', 246, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (259, 5, N'Callum', N'McBride', 246, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (260, 6, N'Nicholas', N'Gilligan', 246, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (261, 7, N'Kathleen', N'Daniels', 246, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (262, 8, N'Karl', N'Rourke', 246, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (263, 9, N'Jenna', N'Richardson', 246, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (264, 10, N'Margaret', N'Spencer', 246, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (265, 11, N'Tristan', N'Gallagher', 246, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (266, 12, N'Gregory', N'Fox', 246, N'C', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (267, 13, N'Chris', N'Gardner', 246, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (268, 14, N'Aidan', N'Todd', 246, N'C', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (269, 1, N'Annie', N'Campbell', 246, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (270, 2, N'Alfie', N'Williams', 246, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (271, 3, N'Holly', N'Hammond', 246, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (272, 4, N'Riley', N'Vance', 246, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (273, 5, N'Jayden', N'Forbes', 246, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (274, 6, N'Lydia', N'Anderson', 246, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (275, 7, N'Shannon', N'Faraday', 246, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (276, 8, N'April', N'Phillips', 246, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (277, 9, N'Benson', N'Slater', 246, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (278, 10, N'Penelope', N'Maxwell', 246, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (279, 11, N'Eugene', N'Daly', 246, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (280, 1, N'Bruce', N'Sinclair', 280, N'B', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (281, 2, N'Eli', N'West', 280, N'B', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (282, 3, N'Nina', N'Duncan', 280, N'B', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (283, 4, N'Geraldine', N'Thompson', 280, N'B', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (284, 5, N'Rory', N'Harrison', 280, N'B', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (285, 6, N'Natasha', N'Hilton', 280, N'B', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (286, 7, N'Bonnie', N'Fraser', 280, N'B', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (287, 8, N'Gareth', N'Wright', 280, N'B', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (288, 9, N'Ellis', N'Morrison', 280, N'B', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (289, 10, N'Heidi', N'Bryant', 280, N'B', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (290, 11, N'Jason', N'Lawrence', 280, N'B', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (291, 12, N'Christine', N'Cullen', 280, N'B', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (292, 13, N'Louis', N'Miles', 280, N'B', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (293, 14, N'Scott', N'Allingham', 280, N'B', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (294, 1, N'Bella', N'Craig', 280, N'C', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (295, 2, N'Flora', N'Holt', 280, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (296, 3, N'Zachary', N'Cannon', 280, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (297, 4, N'Sheila', N'Beck', 280, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (298, 5, N'Beatrice', N'Duffy', 280, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (299, 6, N'Kyle', N'Reynolds', 280, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (300, 7, N'Stephen', N'Randall', 280, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (301, 8, N'Selina', N'Keane', 280, N'C', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (302, 9, N'Reece', N'Dean', 280, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (303, 10, N'Sienna', N'Ray', 280, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (304, 11, N'Claire', N'Evans', 280, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (305, 12, N'Sophie', N'Aldridge', 280, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (306, 13, N'Sylvia', N'Horn', 280, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (307, 14, N'Oscar', N'Shaw', 280, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (308, 15, N'Gabriel', N'Perry', 280, N'C', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (309, 16, N'Linda', N'Baker', 280, N'C', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (310, 17, N'Philip', N'Russell', 280, N'C', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (311, 1, N'Abigail', N'Brock', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (312, 2, N'Ruth', N'Greene', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (313, 3, N'Simon', N'Webb', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (314, 4, N'Samantha', N'Parsons', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (315, 5, N'Dean', N'Kirk', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (316, 6, N'Mark', N'Barr', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (317, 7, N'Caitlin', N'Brewer', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (318, 8, N'Marjorie', N'Abbott', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (319, 9, N'Wesley', N'Kelly', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (320, 10, N'Jeremy', N'Henderson', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (321, 11, N'Finn', N'Jones', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (322, 12, N'Grace', N'Hughes', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (323, 13, N'Mia', N'Ford', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (324, 14, N'Alistair', N'Fisher', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (325, 15, N'Kai', N'Johnston', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (326, 16, N'Bridget', N'Stevenson', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (327, 17, N'Daniel', N'Bell', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (328, 18, N'Marie', N'Hicks', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (329, 19, N'Denise', N'North', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (330, 20, N'Catherine', N'Tucker', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (331, 22, N'Glenn', N'Yates', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (332, 23, N'Heather', N'Townsend', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (333, 24, N'Ellie', N'Foster', 280, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (334, 1, N'Dana', N'Reilly', 297, N'B', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (335, 2, N'Arthur', N'Kane', 297, N'B', N'NULL', N'NULL', N'NULL', 1, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (336, 3, N'Kirsten', N'Hopkins', 297, N'B', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (337, 4, N'Stella', N'Stevens', 297, N'B', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (338, 5, N'Thomas', N'Richards', 297, N'B', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (339, 1, N'Katie', N'Brady', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (340, 2, N'Nora', N'Donnell', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (341, 3, N'Cameron', N'Graham', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (342, 4, N'Logan', N'Murray', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (343, 5, N'Felix', N'Payne', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (344, 6, N'Nicole', N'Jackson', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (345, 7, N'Poppy', N'Page', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (346, 8, N'Brooke', N'Marshall', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (347, 9, N'Joel', N'Roche', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (348, 10, N'Hazel', N'Gordon', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (349, 11, N'Gary', N'Gray', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (350, 12, N'Walter', N'Buchanan', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (351, 13, N'Melanie', N'Patterson', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (352, 14, N'Harvey', N'Pearce', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (353, 15, N'Lois', N'Blake', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 0);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (354, 16, N'Tyler', N'Butler', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (355, 17, N'Stephanie', N'McKenzie', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (356, 18, N'Jordan', N'Morgan', 297, N'S', N'NULL', N'NULL', N'NULL', 0, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (357, 6, N'Connor', N'Smith', 297, N'B', N'NULL', N'NULL', N'NULL', 1, 1);
INSERT INTO [WoodseatsScouts.Coins.2026].dbo.ScoutMembers (Id, Number, FirstName, LastName, ScoutGroupId, ScoutSectionCode, Clue1State, Clue2State, Clue3State, IsDayVisitor, HasImage) VALUES (358, 18, N'Courtney', N'Brown', 146, N'B', N'NULL', N'NULL', N'NULL', 1, 1);

    SET IDENTITY_INSERT dbo.ScoutMembers OFF;
GO

