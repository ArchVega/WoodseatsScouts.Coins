$location = Get-Location
. .\Utilities\Database\RecreateDbs-1-Development.ps1
Set-Location $location
. .\Utilities\Database\RecreateDbs-2-IntegrationTests.ps1
Set-Location $location
. .\Utilities\Database\RecreateDbs-3-AcceptanceTests.ps1