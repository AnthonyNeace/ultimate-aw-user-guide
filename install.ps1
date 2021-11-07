# Download the Ultimate AW User Guide from GitHub and backup the current default.awh
Param
(
    [string] $AWHelpPath = "C:\Users\$env:username\AppData\Local\ActiveWorlds 3D\Help\",
    [string] $Default = "default.awh"		
)

$timestamp = (Get-Date -f yyyyMMdd-HHmm)

# Create a backup of the existing help file.
Move-Item "$AWHelpPath$Default" $AWHelpPath"backup-"$timestamp".awh" -Force

(Invoke-WebRequest -URI "https://raw.github.com/AnthonyNeace/AWUserGuideUtilities/master/UserGuides/ultimateawuserguide.awh").Content | Out-File "$AWHelpPath$Default" -Encoding "UTF8"
