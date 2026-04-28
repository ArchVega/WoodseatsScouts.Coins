# only works with POSTs not PUTs!
-v 2 for verbose

$url = "http://localhost:7167/api/scouts/members/1/coins"
$number = 500
$jsonPath = "./Utilities/API/ApacheBenchTestPayload.json"

ab -n $number -c 5 -T application/json -p $jsonPath -H "X-Coins-Authentication-Token: test" $url

