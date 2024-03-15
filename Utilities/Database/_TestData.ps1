$script:testDataCsvFilePath = Join-Path (Get-Location) "Utilities\Database\TestData\TestData.xlsx"

function InsertTestData {
    param(
        [Parameter(Mandatory)]    
        [string] $DatabaseName,
        [ValidateSet("_cfg_dev", "_cfg_int", "_cfg_accept")]
        [Parameter(Mandatory)]    
        [string] $Configuration
    )

    $worksheets = Get-ExcelSheetInfo $script:testDataCsvFilePath
    $dataSets = $worksheets | ForEach-Object { 
        @{
            Table = $_.Name
            Data  = Import-Excel $script:testDataCsvFilePath -WorksheetName $_.Name 
        }        
    }
    
    
    foreach ($dataSet in $dataSets) {
        $tableName = $dataSet.Table
        $dataSetForSelectedConfiguration = $dataSet.Data | Where-Object { $_.$configuration }
        
        if ($null -ne $dataSetForSelectedConfiguration) {
            $query = "SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '$tableName'"
            $tableSchema = Invoke-Sqlcmd -ServerInstance . -Database $DatabaseName -Query $query -TrustServerCertificate

            $insertQueryStringBuilder = [System.Text.StringBuilder]::new()
            
            $insertQueryStringBuilder.AppendLine("SET IDENTITY_INSERT $tableName ON;")
            $dataSetForSelectedConfiguration | ForEach-Object { $insertQueryStringBuilder.AppendLine((CreateSqlQuery $tableSchema $tableName $_)) }
            $insertQueryStringBuilder.AppendLine("SET IDENTITY_INSERT $tableName OFF;")

            $insertQuery = $insertQueryStringBuilder.ToString();
            Write-Host $insertQuery            
            Invoke-Sqlcmd -ServerInstance . -Database $DatabaseName -Query $insertQuery -TrustServerCertificate
        }
    }    
}

function CreateSqlQuery($tableSchema, $tableName, $row) {
    $columnsAndValues = $row.psobject.Properties | Where-Object { -Not $_.Name.StartsWith("_cfg_") }
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