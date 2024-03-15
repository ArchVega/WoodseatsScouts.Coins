$script:csvTestDataRootDirectory = Join-Path (Get-Location) "Utilities\Database\TestData"

function RestoreBaseTestData {
    param(
        [Parameter(Mandatory)]    
        [string] $DatabaseName,
        [Parameter(Mandatory)]    
        [string] $Path
    )

    Write-Host "Inserting data from '$Path' into '$DatabaseName'..."

    $csvFilePath = Join-Path $script:csvTestDataRootDirectory -ChildPath $Path
    $worksheets = Get-ExcelSheetInfo $csvFilePath
    $dataSets = $worksheets | ForEach-Object { 
        @{
            Table = $_.Name
            Data  = Import-Excel $csvFilePath -WorksheetName $_.Name 
        }        
    }
    
    $tables = @("Sections", "Troops", "Members", "ScavengedCoins", "ScavengeResults")
    $tables | ForEach-Object { Invoke-Sqlcmd -ServerInstance . -Database $DatabaseName -Query "DELETE FROM $_" -TrustServerCertificate }

    $tablesWithoutIdentities = @("Sections")

    foreach ($dataSet in $dataSets) {
        $tableName = $dataSet.Table
                
        $query = "SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '$tableName'"
        $tableSchema = Invoke-Sqlcmd -ServerInstance . -Database $DatabaseName -Query $query -TrustServerCertificate

        $insertQueryStringBuilder = [System.Text.StringBuilder]::new()
        
        if (!$tablesWithoutIdentities.Contains($tableName)) {
            $insertQueryStringBuilder.AppendLine("SET IDENTITY_INSERT $tableName ON;")
        }
        $dataSet.Data | ForEach-Object { $insertQueryStringBuilder.AppendLine((CreateSqlQuery $tableSchema $tableName $_)) }
        if (!$tablesWithoutIdentities.Contains($tableName)) {
            $insertQueryStringBuilder.AppendLine("SET IDENTITY_INSERT $tableName OFF;")
        }        

        $insertQuery = $insertQueryStringBuilder.ToString();
        Invoke-Sqlcmd -ServerInstance . -Database $DatabaseName -Query $insertQuery -TrustServerCertificate
    }    

    Write-Host "Finished inserting data from '$Path' into '$DatabaseName'"
}

function CreateSqlQuery($tableSchema, $tableName, $row) {
    $columnsAndValues = $row.psobject.Properties
    $columns = ($columnsAndValues | ForEach-Object { $_.Name }) -join ","

    $formattedValues = $columnsAndValues | ForEach-Object {
        $name = $_.Name
        $columnType = $tableSchema | Where-Object { $_.COLUMN_NAME -eq $name }
        return FormatValue $columnType.DATA_TYPE $_.Value
    }

    $formattedValues = $formattedValues -join ","

    return "INSERT INTO [$tableName] ($columns) VALUES ($formattedValues);"
}

function FormatValue($columnType, $value) {
    switch ($columnType) {
        "char" {
            return "'$value'"
        }
        "nvarchar" {
            return "'$value'"
        }
        Default {
            return $value
        }
    }
} 