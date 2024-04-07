
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

USE [WoodseatsScouts.Coins]
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

CREATE TABLE [ErrorLogs] (
    [Id] int NOT NULL IDENTITY,
    [LoggedAt] datetime2 NOT NULL,
    [Message] nvarchar(max) NOT NULL,
    [StackTrace] nvarchar(max) NULL,
    CONSTRAINT [PK_ErrorLogs] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Sections] (
    [Code] char(1) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Sections] PRIMARY KEY ([Code])
);
GO

CREATE TABLE [Troops] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Troops] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Members] (
    [Id] int NOT NULL IDENTITY,
    [Code] AS 'M' + (FORMAT(TroopId, '000'))  + [SectionId] + (FORMAT(Number, '000')),
    [Number] int NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NULL,
    [TroopId] int NOT NULL,
    [SectionId] char(1) NOT NULL,
    [Clue1State] nvarchar(max) NULL,
    [Clue2State] nvarchar(max) NULL,
    [Clue3State] nvarchar(max) NULL,
    [IsDayVisitor] bit NOT NULL,
    [HasImage] bit NOT NULL,
    CONSTRAINT [PK_Members] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Members_Sections_SectionId] FOREIGN KEY ([SectionId]) REFERENCES [Sections] ([Code]) ON DELETE CASCADE,
    CONSTRAINT [FK_Members_Troops_TroopId] FOREIGN KEY ([TroopId]) REFERENCES [Troops] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Coins] (
    [Id] int NOT NULL IDENTITY,
    [BaseValueId] int NOT NULL,
    [Base] int NOT NULL,
    [Value] int NOT NULL,
    [Code] AS 'C' + (FORMAT([BaseValueId], '0000'))  + (FORMAT([Base], '000')) + (FORMAT([Value], '000')),
    [MemberId] int NULL,
    [LockUntil] datetime2 NULL,
    CONSTRAINT [PK_Coins] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Coins_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id])
);
GO

CREATE TABLE [ScavengeResults] (
    [Id] int NOT NULL IDENTITY,
    [MemberId] int NOT NULL,
    [CompletedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_ScavengeResults] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ScavengeResults_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [ScavengedCoins] (
    [Id] int NOT NULL IDENTITY,
    [ScavengeResultId] int NOT NULL,
    [Code] nvarchar(max) NOT NULL,
    [BaseNumber] int NOT NULL,
    [PointValue] int NOT NULL,
    CONSTRAINT [PK_ScavengedCoins] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ScavengedCoins_ScavengeResults_ScavengeResultId] FOREIGN KEY ([ScavengeResultId]) REFERENCES [ScavengeResults] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Coins_MemberId] ON [Coins] ([MemberId]);
GO

CREATE INDEX [IX_Members_SectionId] ON [Members] ([SectionId]);
GO

CREATE INDEX [IX_Members_TroopId] ON [Members] ([TroopId]);
GO

CREATE INDEX [IX_ScavengedCoins_ScavengeResultId] ON [ScavengedCoins] ([ScavengeResultId]);
GO

CREATE INDEX [IX_ScavengeResults_MemberId] ON [ScavengeResults] ([MemberId]);
GO

CREATE UNIQUE INDEX [IX_Sections_Code] ON [Sections] ([Code]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240407120856_Initial', N'8.0.3');
GO

COMMIT;
GO



if not exists(select * from sys.database_principals where name = 'ScoutsUser')
begin
CREATE USER ScoutsUser FOR LOGIN ScoutsUser WITH DEFAULT_SCHEMA = [WoodseatsScouts.Coins]
end

INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (1, 1, 10, N'C0001001010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (2, 1, 20, N'C0002001020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (3, 1, 3, N'C0003001003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (4, 1, 9, N'C0004001009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (5, 1, 11, N'C0005001011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (6, 2, 10, N'C0006002010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (7, 2, 20, N'C0007002020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (8, 2, 3, N'C0008002003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (9, 2, 9, N'C0009002009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (10, 2, 11, N'C0010002011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (11, 3, 10, N'C0011003010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (12, 3, 20, N'C0012003020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (13, 3, 3, N'C0013003003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (14, 3, 9, N'C0014003009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (15, 3, 11, N'C0015003011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (16, 4, 10, N'C0016004010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (17, 4, 20, N'C0017004020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (18, 4, 3, N'C0018004003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (19, 4, 9, N'C0019004009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (20, 4, 11, N'C0020004011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (21, 5, 10, N'C0021005010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (22, 5, 20, N'C0022005020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (23, 5, 3, N'C0023005003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (24, 5, 9, N'C0024005009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (25, 5, 11, N'C0025005011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (26, 6, 10, N'C0026006010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (27, 6, 20, N'C0027006020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (28, 6, 3, N'C0028006003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (29, 6, 9, N'C0029006009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (30, 6, 11, N'C0030006011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (31, 7, 10, N'C0031007010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (32, 7, 20, N'C0032007020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (33, 7, 3, N'C0033007003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (34, 7, 9, N'C0034007009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (35, 7, 11, N'C0035007011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (36, 8, 10, N'C0036008010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (37, 8, 20, N'C0037008020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (38, 8, 3, N'C0038008003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (39, 8, 9, N'C0039008009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (40, 8, 11, N'C0040008011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (41, 9, 10, N'C0041009010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (42, 9, 20, N'C0042009020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (43, 9, 3, N'C0043009003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (44, 9, 9, N'C0044009009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (45, 9, 11, N'C0045009011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (46, 10, 10, N'C0046010010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (47, 10, 20, N'C0047010020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (48, 10, 3, N'C0048010003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (49, 10, 9, N'C0049010009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (50, 10, 11, N'C0050010011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (51, 11, 10, N'C0051011010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (52, 11, 20, N'C0052011020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (53, 11, 3, N'C0053011003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (54, 11, 9, N'C0054011009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (55, 11, 11, N'C0055011011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (56, 12, 10, N'C0056012010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (57, 12, 20, N'C0057012020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (58, 12, 3, N'C0058012003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (59, 12, 9, N'C0059012009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (60, 12, 11, N'C0060012011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (61, 13, 10, N'C0061013010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (62, 13, 20, N'C0062013020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (63, 13, 3, N'C0063013003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (64, 13, 9, N'C0064013009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (65, 13, 11, N'C0065013011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (66, 14, 10, N'C0066014010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (67, 14, 20, N'C0067014020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (68, 14, 3, N'C0068014003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (69, 14, 9, N'C0069014009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (70, 14, 11, N'C0070014011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (71, 15, 10, N'C0071015010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (72, 15, 20, N'C0072015020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (73, 15, 3, N'C0073015003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (74, 15, 9, N'C0074015009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (75, 15, 11, N'C0075015011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (76, 16, 10, N'C0076016010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (77, 16, 20, N'C0077016020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (78, 16, 3, N'C0078016003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (79, 16, 9, N'C0079016009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (80, 16, 11, N'C0080016011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (81, 17, 10, N'C0081017010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (82, 17, 20, N'C0082017020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (83, 17, 3, N'C0083017003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (84, 17, 9, N'C0084017009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (85, 17, 11, N'C0085017011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (86, 18, 10, N'C0086018010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (87, 18, 20, N'C0087018020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (88, 18, 3, N'C0088018003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (89, 18, 9, N'C0089018009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (90, 18, 11, N'C0090018011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (91, 19, 10, N'C0091019010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (92, 19, 20, N'C0092019020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (93, 19, 3, N'C0093019003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (94, 19, 9, N'C0094019009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (95, 19, 11, N'C0095019011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (96, 20, 10, N'C0096020010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (97, 20, 20, N'C0097020020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (98, 20, 3, N'C0098020003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (99, 20, 9, N'C0099020009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (100, 20, 11, N'C0100020011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (101, 21, 10, N'C0101021010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (102, 21, 20, N'C0102021020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (103, 21, 3, N'C0103021003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (104, 21, 9, N'C0104021009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (105, 21, 11, N'C0105021011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (106, 22, 10, N'C0106022010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (107, 22, 20, N'C0107022020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (108, 22, 3, N'C0108022003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (109, 22, 9, N'C0109022009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (110, 22, 11, N'C0110022011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (111, 23, 10, N'C0111023010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (112, 23, 20, N'C0112023020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (113, 23, 3, N'C0113023003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (114, 23, 9, N'C0114023009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (115, 23, 11, N'C0115023011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (116, 24, 10, N'C0116024010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (117, 24, 20, N'C0117024020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (118, 24, 3, N'C0118024003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (119, 24, 9, N'C0119024009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (120, 24, 11, N'C0120024011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (121, 25, 10, N'C0121025010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (122, 25, 20, N'C0122025020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (123, 25, 3, N'C0123025003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (124, 25, 9, N'C0124025009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (125, 25, 11, N'C0125025011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (126, 26, 10, N'C0126026010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (127, 26, 20, N'C0127026020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (128, 26, 3, N'C0128026003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (129, 26, 9, N'C0129026009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (130, 26, 11, N'C0130026011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (131, 27, 10, N'C0131027010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (132, 27, 20, N'C0132027020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (133, 27, 3, N'C0133027003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (134, 27, 9, N'C0134027009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (135, 27, 11, N'C0135027011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (136, 28, 10, N'C0136028010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (137, 28, 20, N'C0137028020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (138, 28, 3, N'C0138028003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (139, 28, 9, N'C0139028009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (140, 28, 11, N'C0140028011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (141, 29, 10, N'C0141029010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (142, 29, 20, N'C0142029020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (143, 29, 3, N'C0143029003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (144, 29, 9, N'C0144029009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (145, 29, 11, N'C0145029011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (146, 30, 10, N'C0146030010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (147, 30, 20, N'C0147030020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (148, 30, 3, N'C0148030003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (149, 30, 9, N'C0149030009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (150, 30, 11, N'C0150030011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (151, 31, 10, N'C0151031010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (152, 31, 20, N'C0152031020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (153, 31, 3, N'C0153031003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (154, 31, 9, N'C0154031009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (155, 31, 11, N'C0155031011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (156, 32, 10, N'C0156032010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (157, 32, 20, N'C0157032020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (158, 32, 3, N'C0158032003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (159, 32, 9, N'C0159032009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (160, 32, 11, N'C0160032011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (161, 33, 10, N'C0161033010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (162, 33, 20, N'C0162033020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (163, 33, 3, N'C0163033003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (164, 33, 9, N'C0164033009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (165, 33, 11, N'C0165033011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (166, 34, 10, N'C0166034010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (167, 34, 20, N'C0167034020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (168, 34, 3, N'C0168034003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (169, 34, 9, N'C0169034009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (170, 34, 11, N'C0170034011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (171, 35, 10, N'C0171035010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (172, 35, 20, N'C0172035020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (173, 35, 3, N'C0173035003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (174, 35, 9, N'C0174035009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (175, 35, 11, N'C0175035011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (176, 36, 10, N'C0176036010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (177, 36, 20, N'C0177036020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (178, 36, 3, N'C0178036003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (179, 36, 9, N'C0179036009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (180, 36, 11, N'C0180036011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (181, 37, 10, N'C0181037010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (182, 37, 20, N'C0182037020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (183, 37, 3, N'C0183037003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (184, 37, 9, N'C0184037009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (185, 37, 11, N'C0185037011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (186, 38, 10, N'C0186038010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (187, 38, 20, N'C0187038020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (188, 38, 3, N'C0188038003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (189, 38, 9, N'C0189038009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (190, 38, 11, N'C0190038011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (191, 39, 10, N'C0191039010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (192, 39, 20, N'C0192039020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (193, 39, 3, N'C0193039003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (194, 39, 9, N'C0194039009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (195, 39, 11, N'C0195039011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (196, 40, 10, N'C0196040010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (197, 40, 20, N'C0197040020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (198, 40, 3, N'C0198040003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (199, 40, 9, N'C0199040009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (200, 40, 11, N'C0200040011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (201, 41, 10, N'C0201041010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (202, 41, 20, N'C0202041020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (203, 41, 3, N'C0203041003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (204, 41, 9, N'C0204041009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (205, 41, 11, N'C0205041011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (206, 42, 10, N'C0206042010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (207, 42, 20, N'C0207042020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (208, 42, 3, N'C0208042003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (209, 42, 9, N'C0209042009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (210, 42, 11, N'C0210042011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (211, 43, 10, N'C0211043010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (212, 43, 20, N'C0212043020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (213, 43, 3, N'C0213043003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (214, 43, 9, N'C0214043009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (215, 43, 11, N'C0215043011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (216, 44, 10, N'C0216044010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (217, 44, 20, N'C0217044020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (218, 44, 3, N'C0218044003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (219, 44, 9, N'C0219044009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (220, 44, 11, N'C0220044011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (221, 45, 10, N'C0221045010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (222, 45, 20, N'C0222045020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (223, 45, 3, N'C0223045003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (224, 45, 9, N'C0224045009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (225, 45, 11, N'C0225045011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (226, 46, 10, N'C0226046010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (227, 46, 20, N'C0227046020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (228, 46, 3, N'C0228046003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (229, 46, 9, N'C0229046009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (230, 46, 11, N'C0230046011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (231, 47, 10, N'C0231047010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (232, 47, 20, N'C0232047020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (233, 47, 3, N'C0233047003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (234, 47, 9, N'C0234047009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (235, 47, 11, N'C0235047011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (236, 48, 10, N'C0236048010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (237, 48, 20, N'C0237048020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (238, 48, 3, N'C0238048003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (239, 48, 9, N'C0239048009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (240, 48, 11, N'C0240048011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (241, 49, 10, N'C0241049010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (242, 49, 20, N'C0242049020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (243, 49, 3, N'C0243049003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (244, 49, 9, N'C0244049009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (245, 49, 11, N'C0245049011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (246, 50, 10, N'C0246050010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (247, 50, 20, N'C0247050020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (248, 50, 3, N'C0248050003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (249, 50, 9, N'C0249050009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (250, 50, 11, N'C0250050011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (251, 51, 10, N'C0251051010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (252, 51, 20, N'C0252051020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (253, 51, 3, N'C0253051003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (254, 51, 9, N'C0254051009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (255, 51, 11, N'C0255051011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (256, 52, 10, N'C0256052010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (257, 52, 20, N'C0257052020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (258, 52, 3, N'C0258052003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (259, 52, 9, N'C0259052009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (260, 52, 11, N'C0260052011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (261, 53, 10, N'C0261053010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (262, 53, 20, N'C0262053020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (263, 53, 3, N'C0263053003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (264, 53, 9, N'C0264053009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (265, 53, 11, N'C0265053011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (266, 54, 10, N'C0266054010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (267, 54, 20, N'C0267054020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (268, 54, 3, N'C0268054003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (269, 54, 9, N'C0269054009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (270, 54, 11, N'C0270054011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (271, 55, 10, N'C0271055010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (272, 55, 20, N'C0272055020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (273, 55, 3, N'C0273055003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (274, 55, 9, N'C0274055009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (275, 55, 11, N'C0275055011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (276, 56, 10, N'C0276056010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (277, 56, 20, N'C0277056020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (278, 56, 3, N'C0278056003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (279, 56, 9, N'C0279056009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (280, 56, 11, N'C0280056011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (281, 57, 10, N'C0281057010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (282, 57, 20, N'C0282057020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (283, 57, 3, N'C0283057003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (284, 57, 9, N'C0284057009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (285, 57, 11, N'C0285057011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (286, 58, 10, N'C0286058010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (287, 58, 20, N'C0287058020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (288, 58, 3, N'C0288058003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (289, 58, 9, N'C0289058009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (290, 58, 11, N'C0290058011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (291, 59, 10, N'C0291059010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (292, 59, 20, N'C0292059020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (293, 59, 3, N'C0293059003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (294, 59, 9, N'C0294059009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (295, 59, 11, N'C0295059011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (296, 60, 10, N'C0296060010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (297, 60, 20, N'C0297060020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (298, 60, 3, N'C0298060003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (299, 60, 9, N'C0299060009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (300, 60, 11, N'C0300060011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (301, 61, 10, N'C0301061010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (302, 61, 20, N'C0302061020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (303, 61, 3, N'C0303061003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (304, 61, 9, N'C0304061009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (305, 61, 11, N'C0305061011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (306, 62, 10, N'C0306062010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (307, 62, 20, N'C0307062020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (308, 62, 3, N'C0308062003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (309, 62, 9, N'C0309062009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (310, 62, 11, N'C0310062011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (311, 63, 10, N'C0311063010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (312, 63, 20, N'C0312063020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (313, 63, 3, N'C0313063003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (314, 63, 9, N'C0314063009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (315, 63, 11, N'C0315063011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (316, 64, 10, N'C0316064010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (317, 64, 20, N'C0317064020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (318, 64, 3, N'C0318064003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (319, 64, 9, N'C0319064009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (320, 64, 11, N'C0320064011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (321, 65, 10, N'C0321065010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (322, 65, 20, N'C0322065020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (323, 65, 3, N'C0323065003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (324, 65, 9, N'C0324065009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (325, 65, 11, N'C0325065011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (326, 66, 10, N'C0326066010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (327, 66, 20, N'C0327066020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (328, 66, 3, N'C0328066003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (329, 66, 9, N'C0329066009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (330, 66, 11, N'C0330066011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (331, 67, 10, N'C0331067010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (332, 67, 20, N'C0332067020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (333, 67, 3, N'C0333067003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (334, 67, 9, N'C0334067009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (335, 67, 11, N'C0335067011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (336, 68, 10, N'C0336068010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (337, 68, 20, N'C0337068020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (338, 68, 3, N'C0338068003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (339, 68, 9, N'C0339068009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (340, 68, 11, N'C0340068011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (341, 69, 10, N'C0341069010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (342, 69, 20, N'C0342069020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (343, 69, 3, N'C0343069003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (344, 69, 9, N'C0344069009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (345, 69, 11, N'C0345069011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (346, 70, 10, N'C0346070010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (347, 70, 20, N'C0347070020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (348, 70, 3, N'C0348070003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (349, 70, 9, N'C0349070009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (350, 70, 11, N'C0350070011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (351, 71, 10, N'C0351071010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (352, 71, 20, N'C0352071020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (353, 71, 3, N'C0353071003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (354, 71, 9, N'C0354071009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (355, 71, 11, N'C0355071011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (356, 72, 10, N'C0356072010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (357, 72, 20, N'C0357072020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (358, 72, 3, N'C0358072003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (359, 72, 9, N'C0359072009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (360, 72, 11, N'C0360072011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (361, 73, 10, N'C0361073010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (362, 73, 20, N'C0362073020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (363, 73, 3, N'C0363073003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (364, 73, 9, N'C0364073009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (365, 73, 11, N'C0365073011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (366, 74, 10, N'C0366074010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (367, 74, 20, N'C0367074020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (368, 74, 3, N'C0368074003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (369, 74, 9, N'C0369074009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (370, 74, 11, N'C0370074011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (371, 75, 10, N'C0371075010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (372, 75, 20, N'C0372075020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (373, 75, 3, N'C0373075003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (374, 75, 9, N'C0374075009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (375, 75, 11, N'C0375075011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (376, 76, 10, N'C0376076010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (377, 76, 20, N'C0377076020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (378, 76, 3, N'C0378076003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (379, 76, 9, N'C0379076009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (380, 76, 11, N'C0380076011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (381, 77, 10, N'C0381077010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (382, 77, 20, N'C0382077020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (383, 77, 3, N'C0383077003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (384, 77, 9, N'C0384077009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (385, 77, 11, N'C0385077011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (386, 78, 10, N'C0386078010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (387, 78, 20, N'C0387078020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (388, 78, 3, N'C0388078003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (389, 78, 9, N'C0389078009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (390, 78, 11, N'C0390078011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (391, 79, 10, N'C0391079010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (392, 79, 20, N'C0392079020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (393, 79, 3, N'C0393079003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (394, 79, 9, N'C0394079009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (395, 79, 11, N'C0395079011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (396, 80, 10, N'C0396080010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (397, 80, 20, N'C0397080020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (398, 80, 3, N'C0398080003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (399, 80, 9, N'C0399080009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (400, 80, 11, N'C0400080011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (401, 81, 10, N'C0401081010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (402, 81, 20, N'C0402081020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (403, 81, 3, N'C0403081003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (404, 81, 9, N'C0404081009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (405, 81, 11, N'C0405081011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (406, 82, 10, N'C0406082010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (407, 82, 20, N'C0407082020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (408, 82, 3, N'C0408082003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (409, 82, 9, N'C0409082009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (410, 82, 11, N'C0410082011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (411, 83, 10, N'C0411083010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (412, 83, 20, N'C0412083020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (413, 83, 3, N'C0413083003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (414, 83, 9, N'C0414083009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (415, 83, 11, N'C0415083011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (416, 84, 10, N'C0416084010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (417, 84, 20, N'C0417084020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (418, 84, 3, N'C0418084003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (419, 84, 9, N'C0419084009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (420, 84, 11, N'C0420084011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (421, 85, 10, N'C0421085010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (422, 85, 20, N'C0422085020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (423, 85, 3, N'C0423085003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (424, 85, 9, N'C0424085009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (425, 85, 11, N'C0425085011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (426, 86, 10, N'C0426086010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (427, 86, 20, N'C0427086020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (428, 86, 3, N'C0428086003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (429, 86, 9, N'C0429086009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (430, 86, 11, N'C0430086011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (431, 87, 10, N'C0431087010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (432, 87, 20, N'C0432087020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (433, 87, 3, N'C0433087003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (434, 87, 9, N'C0434087009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (435, 87, 11, N'C0435087011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (436, 88, 10, N'C0436088010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (437, 88, 20, N'C0437088020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (438, 88, 3, N'C0438088003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (439, 88, 9, N'C0439088009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (440, 88, 11, N'C0440088011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (441, 89, 10, N'C0441089010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (442, 89, 20, N'C0442089020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (443, 89, 3, N'C0443089003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (444, 89, 9, N'C0444089009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (445, 89, 11, N'C0445089011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (446, 90, 10, N'C0446090010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (447, 90, 20, N'C0447090020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (448, 90, 3, N'C0448090003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (449, 90, 9, N'C0449090009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (450, 90, 11, N'C0450090011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (451, 91, 10, N'C0451091010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (452, 91, 20, N'C0452091020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (453, 91, 3, N'C0453091003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (454, 91, 9, N'C0454091009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (455, 91, 11, N'C0455091011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (456, 92, 10, N'C0456092010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (457, 92, 20, N'C0457092020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (458, 92, 3, N'C0458092003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (459, 92, 9, N'C0459092009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (460, 92, 11, N'C0460092011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (461, 93, 10, N'C0461093010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (462, 93, 20, N'C0462093020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (463, 93, 3, N'C0463093003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (464, 93, 9, N'C0464093009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (465, 93, 11, N'C0465093011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (466, 94, 10, N'C0466094010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (467, 94, 20, N'C0467094020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (468, 94, 3, N'C0468094003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (469, 94, 9, N'C0469094009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (470, 94, 11, N'C0470094011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (471, 95, 10, N'C0471095010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (472, 95, 20, N'C0472095020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (473, 95, 3, N'C0473095003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (474, 95, 9, N'C0474095009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (475, 95, 11, N'C0475095011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (476, 96, 10, N'C0476096010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (477, 96, 20, N'C0477096020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (478, 96, 3, N'C0478096003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (479, 96, 9, N'C0479096009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (480, 96, 11, N'C0480096011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (481, 97, 10, N'C0481097010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (482, 97, 20, N'C0482097020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (483, 97, 3, N'C0483097003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (484, 97, 9, N'C0484097009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (485, 97, 11, N'C0485097011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (486, 98, 10, N'C0486098010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (487, 98, 20, N'C0487098020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (488, 98, 3, N'C0488098003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (489, 98, 9, N'C0489098009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (490, 98, 11, N'C0490098011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (491, 99, 10, N'C0491099010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (492, 99, 20, N'C0492099020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (493, 99, 3, N'C0493099003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (494, 99, 9, N'C0494099009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (495, 99, 11, N'C0495099011', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (496, 100, 10, N'C0496100010', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (497, 100, 20, N'C0497100020', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (498, 100, 3, N'C0498100003', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (499, 100, 9, N'C0499100009', NULL)
GO
INSERT [dbo].[Coins] ([Id], [Base], [Value], [Code], [MemberId]) VALUES (500, 100, 11, N'C0500100011', NULL)
GO
INSERT [dbo].[Sections] ([Code], [Name]) VALUES (N'A', N'Adults')
GO
INSERT [dbo].[Sections] ([Code], [Name]) VALUES (N'B', N'Beavers')
GO
INSERT [dbo].[Sections] ([Code], [Name]) VALUES (N'C', N'Cubs')
GO
INSERT [dbo].[Sections] ([Code], [Name]) VALUES (N'E', N'Explorers')
GO
INSERT [dbo].[Sections] ([Code], [Name]) VALUES (N'S', N'Scouts')
GO

