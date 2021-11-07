# Download the Ultimate AW User Guide from GitHub and backup the current default.awh
[CmdletBinding()]
Param
(
    [string] $AWHelpPath = "C:\Users\$env:username\AppData\Local\ActiveWorlds 3D\Help\",
    [string] $Default = "default.awh",
    [Switch] $Remote = $false,
    [Switch] $SkipBackup = $false
)

if($SkipBackup)
{
    Write-Debug "Skipping backup"
}
else 
{
    # Backup existing default.awh file in the same directory
    $timestamp = (Get-Date -f yyyyMMdd-HHmm)

    Write-Debug "Backing up $AWHelpPath$Default to $($AWHelpPath)backup-$($timestamp).awh"

    # Create a backup of the existing help file.
    Move-Item "$AWHelpPath$Default" $AWHelpPath"backup-"$timestamp".awh" -Force
}

if($Remote)
{
    Write-Debug "Copying remote to $($AWHelpPath)default.awh"

    (Invoke-WebRequest -URI "https://raw.github.com/AnthonyNeace/AWUserGuideUtilities/main/user-guides/ultimateawuserguide.awh").Content | Out-File "$AWHelpPath$Default" -Encoding "UTF8"
}
else 
{
    Write-Debug "Copying to $($AWHelpPath)default.awh"

    Copy-Item ".\user-guides\ultimateawuserguide.awh" "$($AWHelpPath)default.awh" -Force
}