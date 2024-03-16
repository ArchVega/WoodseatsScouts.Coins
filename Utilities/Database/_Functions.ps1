Import-Module SqlServer

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
        Invoke-Sqlcmd -ServerInstance . -Query "alter database [$DatabaseName] set single_user with rollback immediate;" -TrustServerCertificate
        Invoke-Sqlcmd -ServerInstance . -Query "Drop database [$DatabaseName];" -TrustServerCertificate
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
            Invoke-Sqlcmd -ServerInstance . -Query "alter database [$CloneSourceDatabaseName] set single_user with rollback immediate;" -TrustServerCertificate
            Invoke-Sqlcmd -ServerInstance . -Query "Drop database [$CloneSourceDatabaseName];" -TrustServerCertificate        
        }
        catch {
            Write-Warning "Did not delete clone source database $CloneSourceDatabaseName, it might not exist."
            Write-Warning $_
        }

        try {
            Write-Host "Creating TestDB Source"    
            Invoke-Sqlcmd -ServerInstance . -Query "DBCC CLONEDATABASE ([$DatabaseName], [$CloneSourceDatabaseName]);" -TrustServerCertificate
        
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
        Invoke-Sqlcmd -ServerInstance . -Query "alter database [$DatabaseName] set single_user with rollback immediate;" -TrustServerCertificate
        Invoke-Sqlcmd -ServerInstance . -Query "Drop database [$DatabaseName];" -TrustServerCertificate        
    }
    catch {
        Write-Output "Did not delete clone source database $DatabaseName, it might not exist."
        Write-Output $_
    }

    try {
        Write-Output  "Creating $DatabaseName..."    
        Invoke-Sqlcmd -ServerInstance . -Query "DBCC CLONEDATABASE ([$SourceDatabaseName], [$DatabaseName]);" -TrustServerCertificate
    
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
        Invoke-Sqlcmd -ServerInstance . -Query $query -TrustServerCertificate
    }

}

function CreateCoinData {
    param(
        [Parameter(Mandatory)]    
        [string] $DatabaseName
    )

    Push-Location "D:\Dev\Archvega\WoodseatsScouts\Utilities\QRCodes\WoodseatsScouts.QRCodes\WoodseatsScouts.QRCodes\bin\Debug"
    try {
        .\WoodseatsScouts.QRCodes.exe $DatabaseName "D:\Temp\WoodseatsScouts.QRCodes"
    } finally {
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
        CREATE VIEW [MembersScavengedCoins] AS
        SELECT 
            Members.FirstName as 'Member First Name',
            Members.LastName as 'Member Last Name', 
            Members.Code as 'Member Code',
            Members.Number as 'Member Number',
            Members.HasImage as 'Member Image Exists?',
            Troops.Name as 'Troop',
            Sections.Name as 'Section',
            Coins.Code as 'Coin Code',
            Coins.Value as 'Coin Value'
        FROM Members
        join ScavengeResults
        on ScavengeResults.MemberId = Members.Id
        join ScavengedCoins
        on ScavengedCoins.ScavengeResultId = ScavengeResults.Id
        join Troops
        on Troops.Id = Members.TroopId
        join Sections
        on Sections.Code = Members.SectionId
        join Coins
        on Coins.Code = ScavengedCoins.Code
        "
        
        Invoke-Sqlcmd -ServerInstance . -Database $DatabaseName -Query $viewQuery -TrustServerCertificate
    }
    catch {
        Write-Warning "Could not create Views"
        Write-Warning $_
    }    
}