$baseUri = "http://localhost:7167"

$headers = @{
    "Content-Type" = "application/json"
}

function CreateTroop {
    param(
        [int] $Id, 
        [string] $Name
    )

    $uri = "Admin/CreateTroop"

    $payload = @{
        id   = IntDefault $Id
        name = StringDefault $Name
    } | ConvertTo-Json
    
    return Invoke-RestMethod "$baseUri/$uri" -Method 'Post' -Headers $headers -Body $payload
}

function CreateMember {
    param(
        [string] $FirstName, 
        [string] $LastName, 
        [int] $TroopId, 
        [string] $Section, 
        [bool] $IsDayVisitor
    )

    $uri = "Home/CreateMember"

    $payload = @{
        firstName    = StringDefault $FirstName
        lastName     = StringDefault $LastName
        troopId      = IntDefault $TroopId
        section      = CharDefault $Section
        isDayVisitor = BoolDefault $IsDayVisitor
    } | ConvertTo-Json
    
    return Invoke-RestMethod "$baseUri/$uri" -Method 'Post' -Headers $headers -Body $payload
}

function UploadMemberImage {
    param(
        [int] $MemberId,
        [string] $Path
    )

    $uri = "Home/SaveMemberPhoto"

    $photo = [System.Convert]::ToBase64String((Get-Content $Path -AsByteStream))

    $payload = @{
        memberId = $MemberId
        Photo    = $photo
    } | ConvertTo-Json
    
    return Invoke-RestMethod "$baseUri/$uri" -Method 'Post' -Headers $headers -Body $payload
}

###################

function StringDefault([string] $value) {
    if ($value -eq "") {
        return [Guid]::NewGuid().ToString().Substring(0, 6)
    }

    return $value
}

function IntDefault([int] $value) {
    if ($value -eq 0) {
        return 1
    }

    return $value
}

function CharDefault([System.Object] $value) {
    if ($value -eq "") {
        return 'A'
    }

    return $value
}

function BoolDefault([bool] $value) {
    return $value
}