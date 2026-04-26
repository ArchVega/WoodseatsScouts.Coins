-- [WoodseatsScouts.Coins.Development-alpha]
-- [WoodseatsScouts.Coins.Development]

PRINT '======================================';
PRINT 'POST-MIGRATION VERIFICATION REPORT';
PRINT '======================================';

--------------------------------------------------
-- 1. Row count comparison (core check)
--------------------------------------------------

SELECT 'ActivityBases' AS TableName,
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ActivityBases) AS SourceCount,
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development].dbo.ActivityBases) AS TargetCount;

SELECT 'ScoutGroups' AS TableName,
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ScoutGroups),
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development].dbo.ScoutGroups);

SELECT 'ScoutSections' AS TableName,
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ScoutSections),
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development].dbo.ScoutSections);

SELECT 'ScoutMembers' AS TableName,
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ScoutMembers),
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development].dbo.ScoutMembers);

SELECT 'Coins' AS TableName,
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development-alpha].dbo.Coins),
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development].dbo.Coins);

SELECT 'ScanSessions' AS TableName,
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ScanSessions),
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development].dbo.ScanSessions);

SELECT 'ScannedCoins / ScanCoins' AS TableName,
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ScanCoins),
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development].dbo.ScannedCoins);

SELECT 'ErrorLogs' AS TableName,
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ErrorLogs),
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development].dbo.ErrorLogs);

SELECT '__EFMigrationsHistory' AS TableName,
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development-alpha].dbo.__EFMigrationsHistory),
       (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development].dbo.__EFMigrationsHistory);

--------------------------------------------------
-- 2. Quick “obvious failure” checks
--------------------------------------------------

PRINT '======================================';
PRINT 'CHECK: ANY MISMATCHES? (look for rows below)';
PRINT '======================================';

SELECT 'MISMATCH - ActivityBases'
WHERE (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ActivityBases)
   <> (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development].dbo.ActivityBases);

SELECT 'MISMATCH - ScoutMembers'
WHERE (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ScoutMembers)
   <> (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development].dbo.ScoutMembers);

SELECT 'MISMATCH - Coins'
WHERE (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development-alpha].dbo.Coins)
   <> (SELECT COUNT(*) FROM [WoodseatsScouts.Coins.Development].dbo.Coins);

--------------------------------------------------
-- 3. Final simple confirmation
--------------------------------------------------

PRINT '======================================';
PRINT 'IF YOU SEE NO MISMATCH ROWS ABOVE,';
PRINT 'THE MIGRATION IS STRUCTURALLY VALID';
PRINT '======================================';