. .\Utilities\Database\Functions.ps1

Set-Location "./Src/WoodseatsScouts.Coins.Api"

RecreateDb -DatabaseName "WoodseatsScouts.Coins.Dev" -CloneSourceDatabaseName "WoodseatsScouts.Coins.Tests.Source" -ProjectEnvironment "Development"
CopyDbData -DatabaseFromName "WoodseatsScouts.Coins.Dev" -DatabaseToName "WoodseatsScouts.Coins.Tests.Source" -Tables "Sections"
