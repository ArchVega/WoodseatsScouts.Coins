#$name = "FirstName LastName ThirdName"

if (!$(Get-Module -ListAvailable QRCodeGenerator))
{
    Install-Module -Name QRCodeGenerator
}

$FilesLocation = "C:\DistrictCamp"
$tempLocation = "$FilesLocation\Temp"

$name = "Temp Guy"
$Memberbarcode = "M280B001"

if ($name.Contains(" "))
{
    if ($($name.split(" ")).Length -gt 2)
    {
        Throw "name error $name"
    }
        
    #$QR = 
    $FirstName = $($name.split(" "))[0]
    $LastName = $($name.split(" "))[1]
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
    $bmp = new-object System.Drawing.Bitmap $LeftHandSize,145
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
$Arguments = "convert","-background","none","$tempLocation\LHBlank.png","$tempLocation\QRCode.jpg[145x145]","$tempLocation\Text.png","C:\DistrictCamp\Logo.png[145x145]","$tempLocation\RHBlank.png","+append","$tempLocation\Output.png"
&$command $arguments

#Start-Process -FilePath  -Verb Print
print-image -imageName "$tempLocation\Output.png" -fitImageToPaper $false

Remove-Item "$tempLocation\QRCode.jpg"
Remove-Item "$tempLocation\LHBlank.png"
Remove-Item "$tempLocation\RHBlank.png"
Remove-item "$tempLocation\Text.png"
Remove-Item "$tempLocation\Output.png"
