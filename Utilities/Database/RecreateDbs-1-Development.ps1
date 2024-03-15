. .\Utilities\Database\_Functions.ps1

Set-Location "./Src/WoodseatsScouts.Coins.Api"

RecreateDb -DatabaseName "WoodseatsScouts.Coins.Development" -CloneSourceDatabaseName "WoodseatsScouts.Coins.Tests.Source" -ProjectEnvironment "Development"
CopyDbData -DatabaseFromName "WoodseatsScouts.Coins.Development" -DatabaseToName "WoodseatsScouts.Coins.Tests.Source" -Tables "Sections"
