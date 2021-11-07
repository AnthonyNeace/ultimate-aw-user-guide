# Backup the current user guide.  If the "Default" parameter isn't set, it will default to backing up "default.awh".
Param
(
    [string] $AWHelpPath = "C:\Users\$env:username\AppData\Local\ActiveWorlds 3D\Help\",
    [string] $Default = "default.awh"		
)

$timestamp = (Get-Date -f yyyyMMdd-HHmm)

# Create a backup of the existing help file.
Copy-Item "$AWHelpPath$Default" $AWHelpPath"backup-"$timestamp".awh" -Force