. ./Utilities/Database/_Functions.ps1
. ./Utilities/Database/_TestData.ps1

# ------------------------------------------------------------------------------vv
# These MUST be run on linux using sudo pwsh.
# ------------------------------------------------------------------------------vv
# Set-PSRepository -Name PSGallery -InstallationPolicy Trusted
# Install-Module SqlServer -Scope AllUsers -Force
# Install-Module ImportExcel -Scope AllUsers -Force
# note sure if this is required, but maybe install these - make sure to chose the right os version
# https://learn.microsoft.com/en-us/sql/linux/sql-server-linux-setup-tools?view=sql-server-ver17&tabs=ubuntu-install%2Codbc-ubuntu-2404#ubuntu

Write-Host "Ready"

Set-Location "./Src/WoodseatsScouts.Coins.Api"

RestoreBaseTestData -DatabaseName "WoodseatsScouts.Coins.Tests.Integration" -Path "Integration.xlsx"