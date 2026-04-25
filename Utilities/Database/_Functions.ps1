Import-Module SqlServer

function _ExecuteQuery {
    param(
        [Parameter(Mandatory)]
        $Query,
        [Parameter()]
        $DatabaseName        
    )
    
    Write-Warning "Executing Query. Query is shown in verbose"
    Write-Verbose "Executing query $Query"

    $arguments = @{
        ServerInstance =  "localhost,1433"
        Query =  $Query
        Username =  "SA"
        Password =  "Pa55w0rd123"
        Encrypt =  "Optional"
        TrustServerCertificate = $true
    }

    if ($null -ne $DatabaseName) {
        $arguments.Database = $DatabaseName
    }

    return Invoke-Sqlcmd @arguments
}

function RecreateDb {
    param(        
        [Parameter(Mandatory)]
        $DatabaseName,        
        $CloneSourceDatabaseName,
        [Parameter(Mandatory)]
        $ProjectEnvironment
    )

    Write-Host "Deleting Migrations Folder"
    
    try {
        remove-item Migrations -Force -Recurse
    }
    catch {
        Write-Warning "Did not delete Migrations folder, it might not exist"
        Write-Warning $_
    }
    Write-Host "Deleting Database"
    try {
        _ExecuteQuery "alter database [$DatabaseName] set single_user with rollback immediate;"
        _ExecuteQuery "Drop database [$DatabaseName];"
    }
    catch {
        Write-Warning "Did not delete database $DatabaseName, it might not exist."
        Write-Warning $_
    }
    Write-Host "EF Migrations"
    dotnet ef  --startup-project "." migrations add Initial
    dotnet ef  --startup-project "." database update -- --environment $ProjectEnvironment

    if ($null -ne $CloneSourceDatabaseName) {
        try {
            Write-Host "Deleting clone source db..."
            _ExecuteQuery "alter database [$CloneSourceDatabaseName] set single_user with rollback immediate;"
            _ExecuteQuery "Drop database [$CloneSourceDatabaseName];"
        }
        catch {
            Write-Warning "Did not delete clone source database $CloneSourceDatabaseName, it might not exist."
            Write-Warning $_
        }

        try {
            Write-Host "Creating TestDB Source"    
            _ExecuteQuery "DBCC CLONEDATABASE ([$DatabaseName], [$CloneSourceDatabaseName]);"
        
        }
        catch {
            Write-Warning $_
            Write-Error "Did not clone source database $CloneSourceDatabaseName"
        }
    }
    else {
        Write-Host "No clone source database supplied, skipping step"
    }
    
    Write-Host "Completed recreating SQL Server instance"
}

function CloneDb {
    param(        
        [Parameter(Mandatory)]
        $DatabaseName,        
        $SourceDatabaseName,
        [Parameter(Mandatory)]
        $ProjectEnvironment
    )

    try {
        Write-Output  "Deleting source db '$DatabaseName'..."
        _ExecuteQuery "alter database [$DatabaseName] set single_user with rollback immediate;"
        _ExecuteQuery "Drop database [$DatabaseName];"
    }
    catch {
        Write-Output "Did not delete clone source database $DatabaseName, it might not exist."
        Write-Output $_
    }

    try {
        Write-Output  "Creating $DatabaseName..."    
        _ExecuteQuery "DBCC CLONEDATABASE ([$SourceDatabaseName], [$DatabaseName]);"
    
    }
    catch {
        Write-Output "Did not clone source database $DatabaseName"
        Write-Output $_
    }

    Write-Output  "Completed recreating SQL Server instance"
}

function CopyDbData {
    param(        
        [Parameter(Mandatory)]
        [string] $DatabaseFromName,        
        [Parameter(Mandatory)]
        [string] $DatabaseToName,
        [Parameter(Mandatory)]
        [string[]] $Tables
    )

    $Tables | ForEach-Object { 
        $tableName = $_
        Write-Output "Copying data, table: '$tableName'"
        $query = "INSERT INTO [$DatabaseToName].[dbo].[$tableName] SELECT * FROM [$DatabaseFromName].[dbo].[$tableName]"
        _ExecuteQuery $query
    }

}

function CreateCoinData {
    param(
        [Parameter(Mandatory)]    
        [string] $DatabaseName
    )

    Push-Location "../../Utilities/QRCodes/WoodseatsScouts.QRCodes.Net10/WoodseatsScouts.QRCodes.Net10/bin/Debug/net10.0"
    try {
        dotnet ./WoodseatsScouts.QRCodes.Net10.dll $DatabaseName "/home/developer/dev/temp/woodseats-scouts-qrcodes"
    }
    finally {
        Pop-Location
    }   
}

function CreateAdditionalDbObjects {
    param(        
        [Parameter(Mandatory)]
        $DatabaseName
    )

    Write-Host "Creating Views"
    
    try {
        $viewQuery = "
        CREATE VIEW [ScoutMembersSessionCoins] AS
        SELECT 
            ScoutMembers.FirstName as 'Scout Member First Name',
            ScoutMembers.LastName as 'Scout Member Last Name', 
            ScoutMembers.Code as 'Scout Member Code',
            ScoutMembers.Number as 'Scout Member Number',
            ScoutMembers.HasImage as 'Scout Member Has Image',
            ScoutGroups.Name as 'Scout Group',
            ScoutSections.Name as 'Scout Section',
            Coins.Code as 'Coin Code',
            Coins.Value as 'Coin Value',
            ActivityBases.Name as 'Activity Base'
        FROM ScoutMembers
        join ScanSessions
        on ScanSessions.ScoutMemberId = ScoutMembers.Id
        join ScannedCoins
        on ScannedCoins.ScanSessionId = ScanSessions.Id
        join ScoutGroups
        on ScoutGroups.Id = ScoutMembers.ScoutGroupId
        join ScoutSections
        on ScoutSections.Code = ScoutMembers.ScoutSectionCode
        join Coins
        on Coins.Id = ScannedCoins.CoinId
        join ActivityBases
        on ActivityBases.Id = Coins.ActivityBaseId
        "
        
        _ExecuteQuery $viewQuery $DatabaseName
    }
    catch {
        Write-Warning "Could not create Views"
        Write-Warning $_
    }    
}