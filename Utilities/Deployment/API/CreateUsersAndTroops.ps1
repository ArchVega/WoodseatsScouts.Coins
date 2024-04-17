. .\Utilities\Api\_Functions.ps1

$baseUri = "https://scoutsapi.azurewebsites.net"
CreateTroop -BaseUri $baseUri -Id 1 -Name "Crimson"
CreateTroop -BaseUri $baseUri -Id 2 -Name "Jet"
CreateTroop -BaseUri $baseUri -Id 3 -Name "Royal"
CreateTroop -BaseUri $baseUri -Id 4 -Name "Saffron" 

CreateMember -BaseUri $baseUri -TroopId 1 -FirstName "Olivine" -LastName "Crimson" -Section A -IsDayVisitor $false
CreateMember -BaseUri $baseUri -TroopId 1 -FirstName "Icterine" -LastName "Crimson" -Section B -IsDayVisitor $false
CreateMember -BaseUri $baseUri -TroopId 2 -FirstName "Pumpkin" -LastName "Jet" -Section C -IsDayVisitor $false
CreateMember -BaseUri $baseUri -TroopId 2 -FirstName "Glaucous" -LastName "Jet" -Section E -IsDayVisitor $false
CreateMember -BaseUri $baseUri -TroopId 1 -FirstName "Turquoise" -LastName "Crimson" -Section S -IsDayVisitor $false
CreateMember -BaseUri $baseUri -TroopId 2 -FirstName "Pistachio" -LastName "Jet" -Section A -IsDayVisitor $false
CreateMember -BaseUri $baseUri -TroopId 1 -FirstName "Charcoal" -LastName "Crimson" -Section B -IsDayVisitor $false
CreateMember -BaseUri $baseUri -TroopId 3 -FirstName "Asparagus" -LastName "Royal" -Section C -IsDayVisitor $false
CreateMember -BaseUri $baseUri -TroopId 2 -FirstName "Red" -LastName "Jet" -Section E -IsDayVisitor $false
CreateMember -BaseUri $baseUri -TroopId 3 -FirstName "Cerise" -LastName "Royal" -Section S -IsDayVisitor $false
CreateMember -BaseUri $baseUri -TroopId 3 -FirstName "Ghost" -LastName "Royal" -Section A -IsDayVisitor $false
CreateMember -BaseUri $baseUri -TroopId 3 -FirstName "Jasper" -LastName "Royal" -Section B -IsDayVisitor $false
CreateMember -BaseUri $baseUri -TroopId 4 -FirstName "Hunter" -LastName "Saffron" -Section C -IsDayVisitor $false
CreateMember -BaseUri $baseUri -TroopId 4 -FirstName "Oxford" -LastName "Saffron" -Section E -IsDayVisitor $false
CreateMember -BaseUri $baseUri -TroopId 4 -FirstName "Rosewood" -LastName "Saffron" -Section S -IsDayVisitor $false
CreateMember -BaseUri $baseUri -TroopId 4 -FirstName "Violet" -LastName "Saffron" -Section A -IsDayVisitor $false

UploadMemberImage -BaseUri $baseUri -MemberId 1 -Path "Tests\WoodseatsScouts.Coins.Tests.Acceptance\testImages\Crimson-Olivine.png"
UploadMemberImage -BaseUri $baseUri -MemberId 2 -Path "Tests\WoodseatsScouts.Coins.Tests.Acceptance\testImages\Crimson-Icterine.png"
UploadMemberImage -BaseUri $baseUri -MemberId 3 -Path "Tests\WoodseatsScouts.Coins.Tests.Acceptance\testImages\Jet-Pumpkin.png"
UploadMemberImage -BaseUri $baseUri -MemberId 4 -Path "Tests\WoodseatsScouts.Coins.Tests.Acceptance\testImages\Jet-Glaucous.png"
UploadMemberImage -BaseUri $baseUri -MemberId 5 -Path "Tests\WoodseatsScouts.Coins.Tests.Acceptance\testImages\Crimson-Turquoise.png"
UploadMemberImage -BaseUri $baseUri -MemberId 6 -Path "Tests\WoodseatsScouts.Coins.Tests.Acceptance\testImages\Jet-Pistachio.png"
UploadMemberImage -BaseUri $baseUri -MemberId 7 -Path "Tests\WoodseatsScouts.Coins.Tests.Acceptance\testImages\Crimson-Charcoal.png"
UploadMemberImage -BaseUri $baseUri -MemberId 8 -Path "Tests\WoodseatsScouts.Coins.Tests.Acceptance\testImages\Royal-Asparagus.png"
UploadMemberImage -BaseUri $baseUri -MemberId 9 -Path "Tests\WoodseatsScouts.Coins.Tests.Acceptance\testImages\Jet-Red.png"
UploadMemberImage -BaseUri $baseUri -MemberId 10 -Path "Tests\WoodseatsScouts.Coins.Tests.Acceptance\testImages\Royal-Cerise.png"
UploadMemberImage -BaseUri $baseUri -MemberId 11 -Path "Tests\WoodseatsScouts.Coins.Tests.Acceptance\testImages\Royal-Ghost.png"
UploadMemberImage -BaseUri $baseUri -MemberId 12 -Path "Tests\WoodseatsScouts.Coins.Tests.Acceptance\testImages\Royal-Jasper.png"
UploadMemberImage -BaseUri $baseUri -MemberId 13 -Path "Tests\WoodseatsScouts.Coins.Tests.Acceptance\testImages\Saffron-Hunter.png"
UploadMemberImage -BaseUri $baseUri -MemberId 14 -Path "Tests\WoodseatsScouts.Coins.Tests.Acceptance\testImages\Saffron-Oxford.png"
UploadMemberImage -BaseUri $baseUri -MemberId 15 -Path "Tests\WoodseatsScouts.Coins.Tests.Acceptance\testImages\Saffron-Rosewood.png"
UploadMemberImage -BaseUri $baseUri -MemberId 16 -Path "Tests\WoodseatsScouts.Coins.Tests.Acceptance\testImages\Saffron-Violet.png"
