$headers = @{
    "Content-Type" = "application/json"
}

function CreateTroop {
    param(
        [Parameter(Mandatory)]
        [string] $BaseUri,
        [Parameter(Mandatory)]
        [int] $Id, 
        [string] $Name
    )

    $uri = "Admin/Troop"

    $payload = @{
        id   = IntDefault $Id
        name = StringDefault $Name
    } | ConvertTo-Json
    
    return Invoke-RestMethod "$baseUri/$uri" -Method 'Post' -Headers $headers -Body $payload
}

function CreateMember {
    param(
        [Parameter(Mandatory)]
        [string] $BaseUri,        
        [string] $FirstName, 
        [string] $LastName, 
        [int] $TroopId, 
        [string] $Section, 
        [bool] $IsDayVisitor
    )

    $uri = "Admin/Member"

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
        [Parameter(Mandatory)]
        [string] $BaseUri,
        [Parameter(Mandatory)]
        [int] $MemberId,
        [string] $Path
    )

    $uri = "Members/$MemberId/Photo"

    $photo = [System.Convert]::ToBase64String((Get-Content $Path -AsByteStream))

    $payload = @{
        memberId = $MemberId
        Photo    = $photo
    } | ConvertTo-Json
    
    return Invoke-RestMethod "$baseUri/$uri" -Method 'Put' -Headers $headers -Body $payload
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