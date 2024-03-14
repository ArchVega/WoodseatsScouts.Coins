$location = Get-Location
. .\Utilities\Database\RecreateDbs-Development.ps1
Set-Location $location
# . .\Utilities\Database\RecreateDbs-IntegrationTests.ps1
Set-Location $location
. .\Utilities\Database\RecreateDbs-AcceptanceTests.ps1