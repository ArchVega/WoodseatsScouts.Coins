$ErrorActionPreference = "Stop"

# Make sure to match the database name and database server name to whatever is used in the app itself.
$appEFProjectDirectory = "WoodseatsScouts.Coins.App"
$databaseName = "WoodseatsScouts.Coins"
$databaseServerName = "DESKTOP-P0R3JOS"

Set-Location ..
Set-Location $appEFProjectDirectory
Write-Host "Deleting Migrations Folder"
try {
    remove-item Migrations -Force -Recurse
}
catch {
    Write-Warning "Did not delete Migrations folder, it might not exist"
    Write-Warning $_
}
Write-Host "Deleting Database"
try {
    Invoke-Sqlcmd -ServerInstance $databaseServerName -Query "alter database [$databaseName] set single_user with rollback immediate;"
    Invoke-Sqlcmd -ServerInstance $databaseServerName -Query "Drop database [$databaseName];"
}
catch {
    Write-Warning "Did not delete database $databaseName, it might not exist."
    Write-Warning $_
}
Write-Host "EF Migrations"
dotnet ef  --startup-project "." migrations add Initial
dotnet ef  --startup-project "." database update
Write-Host "Completed recreating SQL Server Express instance"