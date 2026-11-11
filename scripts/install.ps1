# Builds Follow Zoom and copies the DLL into the game's BepInEx plugins folder.
# Usage: powershell -File scripts/install.ps1 [-GameDir "C:\...\Plan B Terraform"]
# The game must be closed (Windows locks the loaded DLL).
param(
    [string]$GameDir = "D:\Program Files (x86)\Steam\steamapps\common\Plan B Terraform"
)
$ErrorActionPreference = "Stop"
$repo = Split-Path -Parent $PSScriptRoot
dotnet build "$repo\src\FollowZoom" -c Release "-p:GameDir=$GameDir"
if ($LASTEXITCODE -ne 0) { throw "Build failed" }
$target = Join-Path $GameDir "BepInEx\plugins\FollowZoom"
New-Item -ItemType Directory -Force $target | Out-Null
Copy-Item "$repo\src\FollowZoom\bin\Release\netstandard2.1\FollowZoom.dll" $target -Force
Write-Output "Installed to $target"
