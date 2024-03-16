<#
// todo something
//todo something
// TODO something
//ToDo something
// todosomething
//todo something todo
// TODO something // todo
// todo: something
// todo : something
    // todo : something
createMemberViewModel.Section, // Todo: rename in client?
#>

$includedDirectories = Get-ChildItem -Recurse -Directory | Where-Object { $_.FullName -notlike '*\node_modules*' -and $_.FullName -notlike '*\bin*' -and $_.FullName -notlike '*\obj*'} 
$files = $includedDirectories | Get-ChildItem -Include *.cs,*.js,*.ts,*.ps1,*.html,*jsx,*.json,*.css,*.sass

$options = [System.Text.RegularExpressions.RegexOptions]:: IgnoreCase -bor [System.Text.RegularExpressions.RegexOptions]:: Multiline
$regex = [Regex]::new("todo(.*)$", $options)

$results = @()

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    if ($content.ToLowerInvariant().Contains("todo")) {
        $item = [pscustomobject]@{
            FileName = $file.FullName
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

$lines = Get-Content ".\Todo.txt"
$lines | ForEach-Object { 
    $results += [pscustomobject] @{
        FileName = "Todo.txt"
        Todo = $_
    } }



$results | ft