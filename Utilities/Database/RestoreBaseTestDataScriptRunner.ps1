. .\Utilities\Database\_TestData.ps1

Set-Location "./Src/WoodseatsScouts.Coins.Api"

RestoreBaseTestData -DatabaseName "WoodseatsScouts.Coins.Tests.Integration" -Path "Integration.xlsx"