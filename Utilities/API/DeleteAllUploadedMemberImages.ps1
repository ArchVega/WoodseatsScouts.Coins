. .\Utilities\API\_Functions.ps1

Get-ChildItem -Path "Src\woodseatsscouts.coins.web\public\member-images" -Filter "*.jpg" | ForEach-Object { 
    Remove-Item $_ 
}