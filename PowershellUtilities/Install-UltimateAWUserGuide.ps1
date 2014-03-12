# Download the Ultimate AW User Guide from GitHub and backup the current default.awh
Param
(
    [string] $AWHelpPath = "C:\ActiveWorlds\Help\",
    [string] $Default = "default.awh"		
)

$timestamp = (Get-Date -f yyyyMMdd-HHmm)

# Create a backup of the existing help file.
Move-Item "$AWHelpPath$Default" $AWHelpPath"backup-"$timestamp".awh" -Force

# Note: Out-File seems to handle UTF8 encoding correctly, whereas Set-Content does not.  I will need to test this more rigorously later.
(Invoke-WebRequest -URI "https://raw.github.com/AnthonyNeace/AWUserGuideUtilities/master/UserGuides/ultimateawuserguide.awh").Content | Out-File "$AWHelpPath$Default" -Encoding "UTF8"
