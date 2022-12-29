###############################################################################################################
Function WritetoDB ($MemberBarcode,$TroopNumber,$MemberNumber,$BaseBarcode,$BaseNumber,$BasePoints,[switch]$PastFile)
{
    Try
    {
        #Write to Full Log
        $ExecuteCommand = "INSERT INTO [$SQLHost].[$database].[DistrictCamp].[pointslog] (MemberNumber, MemberNumber_Troop, MemberNumber_Member, BaseNumber, BaseNumber_Base, BaseNumber_Points) VALUES ('$MemberBarcode','$TroopNumber','$MemberNumber','$BaseBarcode','$BaseNumber','$BasePoints')"
        Invoke-Sqlcmd $ExecuteCommand -Username $user -Password $pass -ServerInstance tcp:$SQLHost -Database $database

        #Read personal total
        $CurrentmemberTotal = 0/1
        $ExecuteCommand = "Select * FROM [$SQLHost].[$database].[districtcamp].[memberlookup] WHERE MemberNumber LIKE '$MemberBarcode'"
        $SQLQryOurput = Invoke-Sqlcmd $ExecuteCommand -Username $user -Password $pass -ServerInstance tcp:$SQLHost -Database $database
        $UpdatedMemberTotal = ($SQLQryOurput.memberpoints /1) + $BasePoints

        #Write personal total
        $ExecuteCommand = "UPDATE [$SQLHost].[$database].[districtcamp].[memberlookup] set MemberPoints='$UpdatedmemberTotal' where MemberNumber like '$MemberBarcode'"
        Invoke-Sqlcmd $ExecuteCommand -Username $user -Password $pass -ServerInstance tcp:$SQLHost -Database $database

        Write-host "$BasePoints awarded to $MemberBarcode for base $BaseNumber"        

        if (!$pastfile)
        {
            if (!$(Test-Path \\rob-vaio\DistrictCamp2017\UpdateAvaliable.txt)) #Don't refresh 
            {
                New-Item \\rob-vaio\DistrictCamp2017\UpdateAvaliable.txt -ErrorAction SilentlyContinue | out-null
            }
            

            foreach ($CacheFile in $(Get-ChildItem "$PSScriptRoot\DatabaseCache")) #process DB write failures
            {
                if ($conn.State -ne "Open") {initialise-SQL | Out-Null}
                $PastDetails = $($CacheFile.name).split(".")
                WritetoDB -MemberBarcode $PastDetails[0] -TroopNumber $PastDetails[1] -MemberNumber $PastDetails[2] -BaseBarcode $PastDetails[3] -BaseNumber $PastDetails[4] -BasePoints $PastDetails[5] -PastFile
                Remove-Item $CacheFile.fullName
            }
        }

        if ($GroupsEnabled)
        {
            Check-GroupMembership -MemberNumber $MemberBarcode
        }
        
        if  ($CluesEnabled)
        {
            Check-CluesToPrint -MemberNumber $MemberBarcode
        }

    }
    Catch
    {
        Write-host "SQL server error"
        $SoundLocation = $PSScriptRoot + "\buzzer.wav"
        (new-object Media.SoundPlayer $SoundLocation).play()
        if (!$pastFile) #If it's not the processing of a past file, create the past file
        {
            New-Item -Path "$PSScriptRoot\DatabaseCache" -Name "$MemberBarcode.$TroopNumber.$MemberNumber.$BaseBarcode.$BaseNumber.$BasePoints" -ItemType File -Force -ErrorAction SilentlyContinue | Out-Null
        }
    }
}
###############################################################################################################

###############################################################################################################
Function OKPressed ($Global:Barcode) {

    Switch -wildcard ($Barcode) {

    "M*"{  #Member Barcode    
            #Split up member code
            $Global:MemberBarcode = $Barcode.substring(1,7)
            $Global:TroopNumber = $Barcode.substring(1,3)
            $Global:MemberNumber = $Barcode.substring(5,3)
            #$objTextBox.Text = ""
            Write-Host "MEMBER SCANNED $MemberBarcode"

            Check-GroupMembership -MemberNumber $MemberBarcode
            }
    "B*"{  #Base Barcode
            #Split up Base Code
            $Global:BaseBarcode = $Barcode.substring(1,6)
            $Global:BaseNumber = $Barcode.substring(1,3)
            $Global:BasePoints = $Barcode.substring(4,3)

            WritetoDB $MemberBarcode $TroopNumber $MemberNumber $BaseBarcode $BaseNumber $BasePoints
            #$objTextBox.text = ""
            Write-Host "BASE SCANNED $BaseBarcode"
            }
    "Quit" {
            $global:running = $false
           }
    Default {Write-Host "ERROR: Unable to determine barcode type"
            $objTextBox.text = ""
            $SoundLocation = $PSScriptRoot + "\buzzer.wav"
            (new-object Media.SoundPlayer $SoundLocation).play()
            }
    }
}
###############################################################################################################

###############################################################################################################
Function Find-Points_Full ($MemberNumber)
{
    $ExecuteCommand = "Select BaseNumber_Points FROM [$SQLHost].[$database].[districtcamp].[pointslog] where MemberNumber like '$membernumber'"
    $Points = Invoke-Sqlcmd $ExecuteCommand -Username $user -Password $pass -ServerInstance tcp:$SQLHost -Database $database

    $sum = 0
    $points.BaseNumber_points | foreach {$sum += ($_ + 0)}

    $sum = $sum / 10 #To resolve bug with 3 figures in data
    Write-Host "$membernumber has $Sum points"
}
###############################################################################################################

###############################################################################################################
Function Find-Points ($MemberNumber)
{
    $ExecuteCommand = "Select BaseNumber_Points FROM [$SQLHost].[$database].[districtcamp].[pointslog] where MemberNumber like '$membernumber'"
    $Points = Invoke-Sqlcmd $ExecuteCommand -Username $user -Password $pass -ServerInstance tcp:$SQLHost -Database $database

    $sum = 0
    $points.BaseNumber_points | foreach {$sum += ($_ + 0)}

    $sum = $sum / 10 #To resolve bug with 3 figures in data
    Write-Host "$membernumber has $Sum points"

    $sum

}
###############################################################################################################

###############################################################################################################
Function CheckCache
{
    $CachedFiles = get-childitem "$PSScriptRoot\DatabaseCache"

    if ($CachedFiles.count -gt 0)
    {
        1
    }
    else
    {
        0
    }
}
###############################################################################################################

###############################################################################################################
Function Find-MemberName ($MemberNumber)
{
    $a = "'"
    $ExecuteCommand = "Select MemberName FROM [$SQLHost].[$database].[districtcamp].[MemberLookup] where MemberNumber like '$membernumber'"
    $member = Invoke-Sqlcmd $ExecuteCommand -Username $user -Password $pass -ServerInstance tcp:$SQLHost -Database $database

    #$output = $Member.MemberName
    $output = "$membernumber - $($Member.MemberName)"

    if ($([string]::IsNullOrWhiteSpace($($Member.MemberName))))
    {
        $output = $membernumber
        Write-Host -BackgroundColor Red "Member doesn't have a name - Advise them to speak to Rob"
    }

    $output
}
###############################################################################################################

###############################################################################################################
Function Check-CluesToPrint ($MemberNumber)
{
    Write-Host "Checking $MemberNumber for printing"
    foreach ($Number in 1..15)
    {
        $ExecuteCommand = "Select ClueGroup,Clue$number FROM [$SQLHost].[$database].[districtcamp].[MemberLookup] where MemberNumber like '$membernumber'"
        $Clues = Invoke-Sqlcmd $ExecuteCommand -Username $user -Password $pass -ServerInstance tcp:$SQLHost -Database $database

        $ClueEligability = $Clues | Select-Object -Property "Clue$number"
        $Group = $Clues | Select-Object -Property "ClueGroup" -ExpandProperty ClueGroup

        $ClueEligability = $ClueEligability | Select-Object -ExpandProperty Clue* -Property Clue*

        if ($ClueEligability -contains "Eligible")
        {
            Write-Host "Eligible for a clue!"
            $SoundLocation = $PSScriptRoot + "\bleep.wav"
            (new-object Media.SoundPlayer $SoundLocation).play()
            try
            {
                print-Clue -ClueGroup $group -ClueNumber $number

                $ExecuteCommand = "UPDATE [$SQLHost].[$database].[districtcamp].[memberlookup] set Clue$number='Printed' where MemberNumber like '$MemberNumber'"
                Invoke-Sqlcmd $ExecuteCommand -Username $user -Password $pass -ServerInstance tcp:$SQLHost -Database $database
            }
            catch
            {
                Write-Host "Error printing"
            }
        }
    }
}
###############################################################################################################

###############################################################################################################
Function Print-Clue ($ClueGroup,$ClueNumber)
{
    Write-Host "Seeking $ClueNumber for $ClueGroup"
    $ExecuteCommand = "Select Clue$ClueNumber FROM [$SQLHost].[$database].[districtcamp].[AgentLookup] where AllocatedGroup like '$ClueGroup'"
    $Clue = Invoke-Sqlcmd $ExecuteCommand -Username $user -Password $pass -ServerInstance tcp:$SQLHost -Database $database

    $ClueData = $clue | Select-Object -ExpandProperty Clue*

    $CluePrint = "$(Find-MemberName -MemberNumber $MemberBarcode)`n`n`n`n$ClueData"

    $Output = New-Item -Name "tempCluePrint_$env:COMPUTERNAME.txt" -Path $PSScriptRoot -ItemType File -Value $CluePrint -Force

    $c = Start-Process -FilePath $Output.FullName -Verb Print -Wait
    
    Remove-Item $Output.FullName

    Write-Host $ClueData
}
###############################################################################################################

###############################################################################################################
Function Check-GroupMembership ($MemberNumber)
{
    $ExecuteCommand = "Select ClueGroup FROM [$SQLHost].[$database].[districtcamp].[MemberLookup] where MemberNumber like '$membernumber'"
    $results = Invoke-Sqlcmd $ExecuteCommand -Username $user -Password $pass -ServerInstance tcp:$SQLHost -Database $database

        if ($([string]::IsNullOrWhiteSpace($($Results.ClueGroup))))
    {
        Write-host -BackgroundColor Red "Member Not assigned a group"
        Assign-memberGroup -MemberNumber $membernumber
    }
    else
    {
        Write-Host "Member assigned to $($Results.ClueGroup)"
    }
}
###############################################################################################################

###############################################################################################################
Function Assign-memberGroup ($MemberNumber)
{
    $filter = $membernumber.substring(0,3)
    $ExecuteCommand = "Select top 1 MembersAssigned,AllocatedGroup FROM [$SQLHost].[$database].[districtcamp].[agentlookup] where AgentTroop IS NULL OR AgentTroop NOT LIKE '$filter' ORDER BY MembersAssigned asc"
    $results = Invoke-Sqlcmd $ExecuteCommand -Username $user -Password $pass -ServerInstance tcp:$SQLHost -Database $database

    if ($([string]::IsNullOrWhiteSpace($($Results.MembersAssigned))))
    {
        $NewTotal = 1
    }
    else
    {
        $NewTotal = $($results.MembersAssigned) + 1
    }

    Write-Host -BackgroundColor DarkGreen "Assigning $membernumber to group $($Results.AllocatedGroup)"

    $ExecuteCommand = "UPDATE [$SQLHost].[$database].[districtcamp].[agentlookup] set MembersAssigned='$NewTotal' where AllocatedGroup like '$($Results.AllocatedGroup)'"
    Invoke-Sqlcmd $ExecuteCommand -Username $user -Password $pass -ServerInstance tcp:$SQLHost -Database $database

    $ExecuteCommand = "UPDATE [$SQLHost].[$database].[districtcamp].[memberlookup] set ClueGroup='$($Results.AllocatedGroup)' where MemberNumber like '$membernumber'"
    Invoke-Sqlcmd $ExecuteCommand -Username $user -Password $pass -ServerInstance tcp:$SQLHost -Database $database

}
###############################################################################################################

###############################################################################################################
Function Play-Brightsign ($Message)
{
    # Define port and target IP address 
    # Random here! 
    [int] $Port = 61002 
    $IP = "192.168.1.255" 
    $Address = [system.net.IPAddress]::Parse($IP) 

    # Create IP Endpoint 
    $End = New-Object System.Net.IPEndPoint $address, $port 

    # Create Socket 
    $Saddrf   = [System.Net.Sockets.AddressFamily]::InterNetwork 
    $Stype    = [System.Net.Sockets.SocketType]::Dgram 
    $Ptype    = [System.Net.Sockets.ProtocolType]::UDP 
    $Sock     = New-Object System.Net.Sockets.Socket $saddrf, $stype, $ptype 
    $Sock.TTL = 26 

    # Connect to socket 
    $sock.Connect($end) 

    # Create encoded buffer 
    $Enc     = [System.Text.Encoding]::ASCII 
    $Message = $Message
    $Buffer  = $Enc.GetBytes($Message) 

    # Send the buffer 
    $Sent   = $Sock.Send($Buffer) 
    "{0} characters sent to: {1} " -f $Sent,$IP 
    "Message is:" 
    $Message 
}
###############################################################################################################

###############################################################################################################
Function Print-WinningTicket
{
    $CluePrint = "WINNER!!!!"

    $Output = New-Item -Name "tempCluePrint_$env:COMPUTERNAME.txt" -Path $PSScriptRoot -ItemType File -Value $CluePrint -Force

    $c = Start-Process -FilePath $Output.FullName -Verb Print -Wait
    
    Remove-Item $Output.FullName
}
###############################################################################################################

###############################################################################################################
Function Print-WristbandImage ($ScoutCode)
{
    $URL = "http://desktop-APO7M33/Home/GetScoutInfoFromCode?code=$ScoutCode"
    $MemberDetails = Invoke-RestMethod $url

    if (!$(Get-Module -ListAvailable QRCodeGenerator))
    {
        Install-Module -Name QRCodeGenerator
    }

    $FilesLocation = "C:\DistrictCamp"
    $tempLocation = "$FilesLocation\Temp"

    $name = $MemberDetails.scoutName
    $Memberbarcode = $ScoutCode

    if ($name.Contains(" "))
    {
        if ($($name.split(" ")).Length -eq 2)
        {
            $FirstName = $($name.split(" "))[0]
            $LastName = $($name.split(" "))[1]
        }
        elseif ($($name.split(" ")).Length -eq 3)
        {
            $FirstName = $($name.split(' '))[0]
            $LastName = "$($($name.split(' '))[1]) $($($name.split(' '))[2])"
        }
        elseif ($($name.split(" ")).Length -gt 3)
        {
            Throw "name error $name"
        }
    }
    else
    {
        $FirstName = $Name
        $LastName = ""
    }

    
    ################Left hand blank
        $LeftHandSize = 250
        Add-Type -AssemblyName System.Drawing
        $filename = "$tempLocation\LHBlank.png"
        $bmp = new-object System.Drawing.Bitmap $LeftHandSize,165
        $font = "Trebuchet MS"
        $font = new-object System.Drawing.Font $font,15
        $brushBg = [System.Drawing.Brushes]::White
        $brushFg = [System.Drawing.Brushes]::Black
        $bmp.Save($filename)
    ################Left hand blank

    ################Create Text Section
        #Write-Host "$Memberbarcode - $Name"
        Add-Type -AssemblyName System.Drawing
        $filename = "$tempLocation\Text.png"
        $bmp = new-object System.Drawing.Bitmap 200,145
        $font = "Trebuchet MS"
        $font = new-object System.Drawing.Font $font,17
        $brushBg = [System.Drawing.Brushes]::White
        $brushFg = [System.Drawing.Brushes]::Black
        #Write out Member Barcode
        $graphics = [System.Drawing.Graphics]::FromImage($bmp) 
        $graphics.FillRectangle($brushBg,0,0,$bmp.Width,$bmp.Height) 
        $graphics.DrawString("$MemberBarcode",$font,$brushFg,10,35) 
        $graphics.Dispose()
        #Write out Troop
        $graphics = [System.Drawing.Graphics]::FromImage($bmp)  
        $graphics.DrawString($FirstName,$font,$brushFg,10,55) 
        $graphics.Dispose()
        #Write out Member Number
        $graphics = [System.Drawing.Graphics]::FromImage($bmp)  
        $graphics.DrawString($LastName,$font,$brushFg,10,75) 
        $graphics.Dispose()
        $bmp.Save($filename)
    ################Create Text Section

    ################Create QR Section
        New-QRCodeText -Text $MemberBarcode -OutPath "$tempLocation\QRCode.jpg" -Width 10
    ################Create QR Section

    ################Right hand blank
        $RightHandSize = 900
        Add-Type -AssemblyName System.Drawing
        $filename = "$tempLocation\RHBlank.png"
        $bmp = new-object System.Drawing.Bitmap $RightHandSize,145
        $font = "Trebuchet MS"
        $font = new-object System.Drawing.Font $font,15
        $brushBg = [System.Drawing.Brushes]::White
        $brushFg = [System.Drawing.Brushes]::Black
        $bmp.Save($filename)
    ################Right hand blank

    $command = "$FilesLocation\imagemagick\Magick.exe"
    #$Arguments = "Montage","-mode","concatenate","-geometry","+0+0","-background","none","$tempLocation\LHBlank.png","$tempLocation\IMF.png","$tempLocation\Text.png","C:\QRExport\Members\QR\$MemberBarcode.png","C:\QRExport\2019\$MemberBarcode.png"
    #$Arguments = "convert","-background","none","$tempLocation\LHBlank.png","$tempLocation\QRCode.jpg[145x145]","$tempLocation\Text.png","C:\DistrictCamp\Logo.png[145x145]","$tempLocation\RHBlank.png","+append","$tempLocation\Output.png"
    $Arguments = "Montage","-mode","concatenate","-geometry","+0+0","-tile","5x1","-background","none","-gravity","south","$tempLocation\LHBlank.png","$tempLocation\QRCode.jpg[145x145]","$tempLocation\Text.png","C:\DistrictCamp\Logo.png[145x145]","$tempLocation\RHBlank.png","$tempLocation\Output.png"
    &$command $arguments

    #Start-Process -FilePath  -Verb Print
    Write-Host "$tempLocation\Output.png"
    #print-image -imageName "$tempLocation\Output.png" -fitImageToPaper $false

    Start-Process  C:\windows\system32\mspaint.exe -Arg '/p "C:\DistrictCamp\temp\Output.png"'

    #Start-Process -FilePath "$tempLocation\Output.png" -Verb Print

    Start-Sleep -Seconds 2
    
    Remove-Item "$tempLocation\QRCode.jpg"
    Remove-Item "$tempLocation\LHBlank.png"
    Remove-Item "$tempLocation\RHBlank.png"
    Remove-item "$tempLocation\Text.png"

    
    while (test-path "$tempLocation\Output.png")
    {
        Remove-Item "$tempLocation\Output.png" -ErrorAction SilentlyContinue
        Start-Sleep -Seconds 0.5
    }
    
}
###############################################################################################################

###############################################################################################################
function print-image
{
    param([string]$imageName = $(throw "Enter image name to print"),
    [string]$printer = "",
    [bool]$fitImageToPaper = $true)

    trap { break; }

    # check out Lee Holmes' blog(http://www.leeholmes.com/blog/HowDoIEasilyLoadAssembliesWhenLoadWithPartialNameHasBeenDeprecated.aspx)
    # on how to avoid using deprecated "LoadWithPartialName" function
    # To load assembly containing System.Drawing.Printing.PrintDocument
    [void][System.Reflection.Assembly]::LoadWithPartialName("System.Drawing")

    # Bitmap image to use to print image
    $bitmap = $null

    $doc = new-object System.Drawing.Printing.PrintDocument
    # if printer name not given, use default printer
    if ($printer -ne "") {
    $doc.PrinterSettings.PrinterName = $printer
    }
 
    $doc.DocumentName = [System.IO.Path]::GetFileName($imageName)

    $doc.add_BeginPrint({
    Write-Host "==================== $($doc.DocumentName) ===================="
    })
 
    # clean up after printing...
    $doc.add_EndPrint({
    if ($bitmap -ne $null) {
    $bitmap.Dispose()
    $bitmap = $null
    }
    Write-Host "xxxxxxxxxxxxxxxxxxxx $($doc.DocumentName) xxxxxxxxxxxxxxxxxxxx"
    })
 
    # Adjust image size to fit into paper and print image
    $doc.add_PrintPage({
    Write-Host "Printing $imageName..."
 
    $g = $_.Graphics
    $pageBounds = $_.MarginBounds
    $img = new-object Drawing.Bitmap($imageName)
  
    $adjustedImageSize = $img.Size
    $ratio = [double] 1;
  
    # Adjust image size to fit on the paper
    if ($fitImageToPaper) {
    $fitWidth = [bool] ($img.Size.Width > $img.Size.Height)
    if (($img.Size.Width -le $_.MarginBounds.Width) -and
    ($img.Size.Height -le $_.MarginBounds.Height)) {
    $adjustedImageSize = new-object System.Drawing.SizeF($img.Size.Width, $img.Size.Height)
    } else {
    if ($fitWidth) {
        $ratio = [double] ($_.MarginBounds.Width / $img.Size.Width);
    } else {
        $ratio = [double] ($_.MarginBounds.Height / $img.Size.Height)
    }
    
    $adjustedImageSize = new-object System.Drawing.SizeF($_.MarginBounds.Width, [float]($img.Size.Height * $ratio))
    }
    }

    # calculate destination and source sizes
    $recDest = new-object Drawing.RectangleF($pageBounds.Location, $adjustedImageSize)
    $recSrc = new-object Drawing.RectangleF(0, 0, $img.Width, $img.Height)
  
    # Print to the paper
    $_.Graphics.DrawImage($img, $recDest, $recSrc, [Drawing.GraphicsUnit]"Pixel")
  
    $_.HasMorePages = $false; # nothing else to print
    })
 
    $doc.Print()
}
###############################################################################################################

###############################################################################################################
Function Create-member ([int]$Troop,[String]$Section,[String]$Name, [switch]$DayVisitor, [switch]$PrintWristband)
{
    $payload = @{"Name" = $Name
                 "TroopNumber" = $troop
                 "Section" = $Section
                 "DayVisitor" = $DayVisitor}

    $URL = "http://desktop-APO7M33/Home/CreateMember"
    $CreatedMember = Invoke-RestMethod -uri $url -Method post -ContentType application/x-www-form-urlencoded -body $payload

    if ($PrintWristband)
    {
        $Strtroop = $troop.ToString().padleft(3,"0")
        $MemberNumer = $($($CreatedMember.MemberNumber).ToString()).padleft(3,"0")
        $WristbandCode = "M$Strtroop$section$MemberNumer"
        Print-WristbandImage $WristbandCode
    }
}
###############################################################################################################