#Import-Module \\desktop-APO7M33\DistrictCamp\DCModule.psm1
$Names = Get-Content "C:\Users\rober\Downloads\District Camp Names - Data.csv" | ConvertFrom-Csv
foreach ($name in $names)
{
    write-host $name
    switch ($Name.Section)
    {
        "Beavers"
        {
            $Section = "B"
        }
        "Cubs"
        {
            $Section = "C"
        }
        "Scouts"
        {
            $section = "S"
        }
        "Explorers"
        {
            $Section = "E"
        }
    }

    Switch ($name.'Camper/Day Visitor')
    {
        "Camper"
        {
            $dayVisitor = $false
        }
        "Day Visitor"
        {
            $dayVisitor = $true
        }
    }
    Create-member -Troop $name.Group -Section $Section -Name $name.Name -DayVisitor:$dayVisitor -PrintWristband
    pause
}