$includedDirectories = Get-ChildItem -Recurse -Directory | Where-Object { $_.FullName -notlike '*\node_modules*' -and $_.FullName -notlike '*\bin*' -and $_.FullName -notlike '*\obj*' } 
$files = $includedDirectories | Get-ChildItem -Include *.cs, *.js, *.ts, *.ps1, *.html, *jsx, *.json, *.css, *.sass

$options = [System.Text.RegularExpressions.RegexOptions]:: IgnoreCase -bor [System.Text.RegularExpressions.RegexOptions]:: Multiline
$regex = [Regex]::new("todo(.*)$", $options)

$results = @()

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    if ($null -ne $content) {
        if ($content.ToLowerInvariant().Contains("todo")) {
            $item = [pscustomobject]@{
                Name     = $file.Name
                FileName = $file.FullName.Replace("D:\Dev\Archvega\WoodseatsScouts\", "")
                Todo     = $null
            }
    
            $m = $regex.Matches($content)
    
            foreach ($i in $m) {
                $todoGroup = $m.Groups[1]
                if (-not [String]::IsNullOrWhiteSpace($todoGroup.Value)) {
                    $value = $todoGroup.Value.trim()     
                    if ($value.StartsWith(":")) {
                        $value = $value.Substring(1).Trim()
                    }       
                    $item.Todo = $value
                }            
            }    
    
            $results += $item
        }    
    }
    else {
        Write-Warning "$($file.FullName) had null content"
    }
}

$lines = Get-Content ".\Todo.txt"
$lines | ForEach-Object { 
    $results += [pscustomobject] @{
        Name = "Todo.txt"
        Todo     = $_
    } }


$TempFile = "$(New-TemporaryFile).html"
$results | ConvertTo-Html | Out-File $TempFile
&'C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe' @($TempFile) 