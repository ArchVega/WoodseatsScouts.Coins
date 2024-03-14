. .\Utilities\Database\Functions.ps1

Set-Location "./Src/WoodseatsScouts.Coins.Api"

CloneDb -DatabaseName "WoodseatsScouts.Coins.Tests.Acceptance" -SourceDatabaseName "WoodseatsScouts.Coins.Tests.Source" -ProjectEnvironment "AcceptanceTest"
CopyDbData -DatabaseFromName "WoodseatsScouts.Coins.Tests.Source" -DatabaseToName "WoodseatsScouts.Coins.Tests.Acceptance" -Tables "Sections"
CreateCoinData