. .\Utilities/Deployment/Database/InsertCoinData.ps1
. .\Utilities/Deployment/Database/InsertMemberData.ps1
. .\Utilities/Deployment/Database/InsertScoutGroups.ps1

$sqlOutputFile = Join-Path (Get-Location) -ChildPath "Utilities/Deployment/Database/Output/WoodseatsScouts.Coins.sql"
$removeAllTables = Join-Path (Get-Location) -ChildPath "Utilities/Deployment/Database/RemoveAllTables.sql"

Set-Location "./Src/WoodseatsScouts.Coins.Api"

dotnet ef migrations script -o $sqlOutputFile

$schemaSql = Get-Content $sqlOutputFile -Raw

$insertData = GetCoinData
$insertData2 = GetMemberData
$insertData3 = GetScoutGroups

$output = "
$(Get-Content $removeAllTables -Raw)

--USE [WoodseatsScouts.Coins]
--GO

$schemaSql

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
$insertData

-- scoutgroups
$insertData3

-- members
$insertData2
GO
"

$output = $output.Replace("[WoodseatsScouts.Coins.Development]", "[WoodseatsScouts.Coins.2026]")

Set-Content -Path $sqlOutputFile -Value $output

Write-Warning "------------------------------------------------------------------------------------------------------------------"
Write-Warning 'Remove to overwrite connection string, https://scoutsapi.scm.azurewebsites.net/dev/wwwroot/appsettings.Production.json'
Write-Warning "------------------------------------------------------------------------------------------------------------------"