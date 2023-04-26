$payload = @{
    name = "Name"
    troopId = 123
    section = "S"
    # DayVisitor = $false
}

 $URL = "https://localhost:44402/Home/CreateMember"
# $URL = "https://woodseatsscoutscoinsapp.azurewebsites.net/Home/CreateMember"
Invoke-RestMethod -uri $URL -Method Post -body $payload -ContentType application/json


# Invoke-RestMethod -uri https://localhost:44402 -Method get 


 