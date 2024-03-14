. .\Utilities\API\_Functions.ps1

$baseUri = "http://localhost:7167"
$uri = "Member/GetMembersWithPoints"

$headers = @{
    "Content-Type" = "application/json"
}

$getUri = "$baseUri/$uri"
$members = Invoke-RestMethod $getUri -Method 'Get' -Headers $headers -Body $payload

$member = $members[0]

$payload = @{
    FirstName    = "Boom"
    LastName = "Shakalaka"
} | ConvertTo-Json

$putUri = "$baseUri/Admin/Member/$($member.id)"
return Invoke-RestMethod $putUri -Method 'Put' -Headers $headers -Body $payload