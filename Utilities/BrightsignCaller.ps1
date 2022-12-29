import-module .\DCModule.psm1

$APIHost = "localhost"

while (1 -eq 1)
{
    $ScoutCode = read-host "Please scan your tag"

    $URL = "http://$APIHost/Home/GetScoutInfoFromCode?code=$ScoutCode"
    
    $MemberObject = invoke-restmethod $url

    $URL = "http://$APIHost/Home/GetClueStatus?ScoutID=$($MemberObject.ScoutID)"

    $ClueObject = invoke-restmethod $url

    if ($ClueObject.clue1State -eq "Released")
    {
        Play-Brightsign -Message "Colour"
        #Print-WinningTicket
    }
}