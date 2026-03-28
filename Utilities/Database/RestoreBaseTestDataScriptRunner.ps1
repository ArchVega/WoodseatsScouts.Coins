. .\Utilities\Database\_TestData.ps1

# ------------------------------------------------------------------------------vv
# These MUST be run on linux using sudo pwsh.
# ------------------------------------------------------------------------------vv
# Set-PSRepository -Name PSGallery -InstallationPolicy Trusted
# Install-Module SqlServer -Scope AllUsers -Force

Write-Host "Ready"

Set-Location "./Src/WoodseatsScouts.Coins.Api"

RestoreBaseTestData -DatabaseName "WoodseatsScouts.Coins.Tests.Integration" -Path "Integration.xlsx"