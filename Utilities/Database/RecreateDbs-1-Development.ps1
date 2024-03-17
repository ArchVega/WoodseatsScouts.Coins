. .\Utilities\Database\_Functions.ps1
. .\Utilities\Database\_TestData.ps1

Set-Location "./Src/WoodseatsScouts.Coins.Api"

RecreateDb -DatabaseName "WoodseatsScouts.Coins.Development" -CloneSourceDatabaseName "WoodseatsScouts.Coins.Tests.Source" -ProjectEnvironment "Development"
CreateAdditionalDbObjects -DatabaseName "WoodseatsScouts.Coins.Development"
CopyDbData -DatabaseFromName "WoodseatsScouts.Coins.Development" -DatabaseToName "WoodseatsScouts.Coins.Tests.Source" -Tables "Sections"
RestoreBaseTestData -DatabaseName "WoodseatsScouts.Coins.Development" -Path "Development.xlsx"
CreateCoinData -DatabaseName "WoodseatsScouts.Coins.Development"