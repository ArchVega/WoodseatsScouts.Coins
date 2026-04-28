-- [WoodseatsScouts.Coins.Development]

--------------------------------------------------
-- Deletes any existing data from the target db.
--------------------------------------------------
-- Disable all constraints (FKs etc.)
EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';

-- Delete all data
-- CHILD TABLES FIRST
DELETE FROM [WoodseatsScouts.Coins.Development].dbo.ScannedCoins;
DELETE FROM [WoodseatsScouts.Coins.Development].dbo.ScanCoins;

DELETE FROM [WoodseatsScouts.Coins.Development].dbo.ScanSessions;
DELETE FROM [WoodseatsScouts.Coins.Development].dbo.Coins;

-- PARENT TABLES
DELETE FROM [WoodseatsScouts.Coins.Development].dbo.ScoutMembers;

DELETE FROM [WoodseatsScouts.Coins.Development].dbo.ScoutSections;
DELETE FROM [WoodseatsScouts.Coins.Development].dbo.ScoutGroups;
DELETE FROM [WoodseatsScouts.Coins.Development].dbo.ActivityBases;

DELETE FROM [WoodseatsScouts.Coins.Development].dbo.ErrorLogs;
DELETE FROM [WoodseatsScouts.Coins.Development].dbo.__EFMigrationsHistory;

-- Reset identity columns (important since you're preserving IDs)
DBCC CHECKIDENT ('[WoodseatsScouts.Coins.Development].dbo.ActivityBases', RESEED, 0);
DBCC CHECKIDENT ('[WoodseatsScouts.Coins.Development].dbo.ScoutGroups', RESEED, 0);
DBCC CHECKIDENT ('[WoodseatsScouts.Coins.Development].dbo.ScoutMembers', RESEED, 0);
DBCC CHECKIDENT ('[WoodseatsScouts.Coins.Development].dbo.Coins', RESEED, 0);
DBCC CHECKIDENT ('[WoodseatsScouts.Coins.Development].dbo.ScanSessions', RESEED, 0);

-- Re-enable constraints
EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';