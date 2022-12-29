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