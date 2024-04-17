. .\Utilities\Database\_Functions.ps1
. .\Utilities\Database\_TestData.ps1

Set-Location "./Src/WoodseatsScouts.Coins.Api"

CloneDb -DatabaseName "WoodseatsScouts.Coins.Tests.Acceptance" -SourceDatabaseName "WoodseatsScouts.Coins.Tests.Source" -ProjectEnvironment "AcceptanceTest"
CopyDbData -DatabaseFromName "WoodseatsScouts.Coins.Tests.Source" -DatabaseToName "WoodseatsScouts.Coins.Tests.Acceptance" -Tables "Sections"
RestoreBaseTestData -DatabaseName "WoodseatsScouts.Coins.Tests.Acceptance" -Path "Acceptance.xlsx"
# CreateCoinData "WoodseatsScouts.Coins.Tests.Acceptance"