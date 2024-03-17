. .\Utilities\Database\_Functions.ps1
. .\Utilities\Database\_TestData.ps1

Set-Location "./Src/WoodseatsScouts.Coins.Api"

CloneDb -DatabaseName "WoodseatsScouts.Coins.Tests.Integration" -SourceDatabaseName "WoodseatsScouts.Coins.Tests.Source" -ProjectEnvironment "IntegrationTest"
CopyDbData -DatabaseFromName "WoodseatsScouts.Coins.Tests.Source" -DatabaseToName "WoodseatsScouts.Coins.Tests.Integration" -Tables "Sections"
RestoreBaseTestData -DatabaseName "WoodseatsScouts.Coins.Tests.Integration" -Path "Integration.xlsx"