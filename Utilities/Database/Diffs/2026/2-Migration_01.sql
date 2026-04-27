-- [WoodseatsScouts.Coins.Development-alpha]
-- [WoodseatsScouts.Coins.Development]

BEGIN TRY
    BEGIN TRANSACTION;

    --------------------------------------------------
    -- Disable constraints
    --------------------------------------------------
    EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';

    --------------------------------------------------
    -- 1. Independent tables
    --------------------------------------------------

    SET IDENTITY_INSERT [WoodseatsScouts.Coins.Development].dbo.ActivityBases ON;
    INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ActivityBases (Id, Name)
    SELECT Id, Name FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ActivityBases;
    SET IDENTITY_INSERT [WoodseatsScouts.Coins.Development].dbo.ActivityBases OFF;

    SET IDENTITY_INSERT [WoodseatsScouts.Coins.Development].dbo.ScoutGroups ON;
    INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutGroups (Id, Name)
    SELECT Id, Name FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ScoutGroups;
    SET IDENTITY_INSERT [WoodseatsScouts.Coins.Development].dbo.ScoutGroups OFF;

    INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutSections (Code, Name)
    SELECT Code, Name FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ScoutSections;

    --------------------------------------------------
    -- 2. ScoutMembers (rename column)
    --------------------------------------------------

    SET IDENTITY_INSERT [WoodseatsScouts.Coins.Development].dbo.ScoutMembers ON;
    INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScoutMembers (
        Id, Number, FirstName, LastName,
        ScoutGroupId, ScoutSectionCode,
        Clue1State, Clue2State, Clue3State,
        IsDayVisitor, HasImage
    )
    SELECT
        Id, Number, FirstName, LastName,
        ScoutGroupId, ScoutSectionId,
        Clue1State, Clue2State, Clue3State,
        IsDayVisitor, HasImage
    FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ScoutMembers;
    SET IDENTITY_INSERT [WoodseatsScouts.Coins.Development].dbo.ScoutMembers OFF;

    --------------------------------------------------
    -- 3. Coins
    --------------------------------------------------

    SET IDENTITY_INSERT [WoodseatsScouts.Coins.Development].dbo.Coins ON;
    INSERT INTO [WoodseatsScouts.Coins.Development].dbo.Coins (
        Id, ActivityBaseSequenceNumber, ActivityBaseId,
        Value, MemberId, LockUntil
    )
    SELECT
        Id, ActivityBaseSequenceNumber, ActivityBaseId,
        Value, MemberId, LockUntil
    FROM [WoodseatsScouts.Coins.Development-alpha].dbo.Coins;
    SET IDENTITY_INSERT [WoodseatsScouts.Coins.Development].dbo.Coins OFF;

    --------------------------------------------------
    -- 4. ScanSessions
    --------------------------------------------------

    SET IDENTITY_INSERT [WoodseatsScouts.Coins.Development].dbo.ScanSessions ON;
    INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScanSessions (
        Id, ScoutMemberId, CompletedAt
    )
    SELECT
        Id, ScoutMemberId, CompletedAt
    FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ScanSessions;
    SET IDENTITY_INSERT [WoodseatsScouts.Coins.Development].dbo.ScanSessions OFF;

    --------------------------------------------------
    -- 5. ScanCoins → ScannedCoins (rename + new column)
    --------------------------------------------------

    SET IDENTITY_INSERT [WoodseatsScouts.Coins.Development].dbo.ScannedCoins ON;
    INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ScannedCoins (
        Id, ScanSessionId, CoinId, PointsOverride
    )
    SELECT
        Id, ScanSessionId, CoinId, NULL
    FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ScanCoins;
    SET IDENTITY_INSERT [WoodseatsScouts.Coins.Development].dbo.ScannedCoins OFF;

    --------------------------------------------------
    -- 6. ErrorLogs (new columns defaulted)
    --------------------------------------------------

    SET IDENTITY_INSERT [WoodseatsScouts.Coins.Development].dbo.ErrorLogs ON;
    INSERT INTO [WoodseatsScouts.Coins.Development].dbo.ErrorLogs (
        Id, LoggedAt, Message, StackTrace, Path, Method
    )
    SELECT
        Id, LoggedAt, Message, StackTrace,
        '' AS Path,
        '' AS Method
    FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ErrorLogs;
    SET IDENTITY_INSERT [WoodseatsScouts.Coins.Development].dbo.ErrorLogs OFF;

    --------------------------------------------------
    -- 7. EF Migrations History
    --------------------------------------------------

    INSERT INTO [WoodseatsScouts.Coins.Development].dbo.__EFMigrationsHistory (
        MigrationId, ProductVersion
    )
    SELECT MigrationId, ProductVersion
    FROM [WoodseatsScouts.Coins.Development-alpha].dbo.__EFMigrationsHistory;

    --------------------------------------------------
    -- Re-enable constraints
    --------------------------------------------------
    EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';

    --------------------------------------------------
    -- Verification: row counts
    --------------------------------------------------

    PRINT 'Row count verification';

    SELECT 'ActivityBases' AS TableName, COUNT(*) AS SourceCount
    FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ActivityBases
    UNION ALL
    SELECT 'ActivityBases', COUNT(*) FROM [WoodseatsScouts.Coins.Development].dbo.ActivityBases;

    SELECT 'ScoutMembers', COUNT(*) FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ScoutMembers
    UNION ALL
    SELECT 'ScoutMembers', COUNT(*) FROM [WoodseatsScouts.Coins.Development].dbo.ScoutMembers;

    SELECT 'Coins', COUNT(*) FROM [WoodseatsScouts.Coins.Development-alpha].dbo.Coins
    UNION ALL
    SELECT 'Coins', COUNT(*) FROM [WoodseatsScouts.Coins.Development].dbo.Coins;

    --------------------------------------------------
    -- Verification: differences (example tables)
    --------------------------------------------------

    -- ScoutMembers diff
    SELECT *
    FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ScoutMembers s
    EXCEPT
    SELECT *
    FROM [WoodseatsScouts.Coins.Development].dbo.ScoutMembers;

    SELECT *
    FROM [WoodseatsScouts.Coins.Development].dbo.ScoutMembers
    EXCEPT
    SELECT *
    FROM [WoodseatsScouts.Coins.Development-alpha].dbo.ScoutMembers;

    --------------------------------------------------

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;

    PRINT 'Migration failed';
    PRINT ERROR_MESSAGE();
END CATCH;