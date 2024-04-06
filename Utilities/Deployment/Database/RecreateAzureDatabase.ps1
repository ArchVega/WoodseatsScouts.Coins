$sqlOutputFile = Join-Path (Get-Location) -ChildPath "Utilities/Deployment/Database/Output/WoodseatsScouts.Coins.sql"
$removeAllTables = Join-Path (Get-Location) -ChildPath "Utilities/Deployment/Database/RemoveAllTables.sql"
$productionTestDataFile = Join-Path (Get-Location) -ChildPath "Utilities/Deployment/Database/ProductionTestData.sql"

Set-Location "./Src/WoodseatsScouts.Coins.Api"

dotnet ef migrations script -o $sqlOutputFile

$schemaSql = Get-Content $sqlOutputFile -Raw

$output = "
$(Get-Content $removeAllTables -Raw)

USE [WoodseatsScouts.Coins]
GO

$schemaSql

if not exists(select * from sys.database_principals where name = 'ScoutsUser')
begin
CREATE USER ScoutsUser FOR LOGIN ScoutsUser WITH DEFAULT_SCHEMA = [WoodseatsScouts.Coins]
end

$(Get-Content $productionTestDataFile -Raw)
"

Set-Content -Path $sqlOutputFile -Value $output

Write-Warning "------------------------------------------------------------------------------------------------------------------"
Write-Warning 'Remove to overwrite connection string, https://scoutsapi.scm.azurewebsites.net/dev/wwwroot/appsettings.Production.json'
Write-Warning "------------------------------------------------------------------------------------------------------------------"