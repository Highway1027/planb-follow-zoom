# Builds Follow Zoom and makes the release zip: dist\FollowZoom-<version>.zip.
# The zip holds BepInEx\plugins\FollowZoom\FollowZoom.dll plus README.md, so players
# unzip it straight into the game folder.
# Usage: powershell -File scripts/package.ps1 [-GameDir "C:\...\Plan B Terraform"]
param(
    [string]$GameDir = "D:\Program Files (x86)\Steam\steamapps\common\Plan B Terraform"
)
$ErrorActionPreference = "Stop"
$repo = Split-Path -Parent $PSScriptRoot

# The version comes from Plugin.cs, so the zip name always matches the DLL.
$match = Select-String -Path "$repo\src\FollowZoom\Plugin.cs" -Pattern 'PluginVersion = "([^"]+)"'
if (-not $match) { throw "PluginVersion not found in Plugin.cs" }
$version = $match.Matches[0].Groups[1].Value

dotnet build "$repo\src\FollowZoom" -c Release "-p:GameDir=$GameDir"
if ($LASTEXITCODE -ne 0) { throw "Build failed" }

$dist = Join-Path $repo "dist"
New-Item -ItemType Directory -Force $dist | Out-Null
$zip = Join-Path $dist "FollowZoom-$version.zip"
if (Test-Path $zip) { Remove-Item -Force $zip }

# Not Compress-Archive: in Windows PowerShell 5.1 it writes "\" in entry names, which
# some unzip tools turn into one file named "BepInEx\plugins\..." instead of folders.
Add-Type -AssemblyName System.IO.Compression, System.IO.Compression.FileSystem
$archive = [System.IO.Compression.ZipFile]::Open($zip, "Create")
try {
    $files = @{
        "BepInEx/plugins/FollowZoom/FollowZoom.dll" = "$repo\src\FollowZoom\bin\Release\netstandard2.1\FollowZoom.dll"
        "BepInEx/plugins/FollowZoom/README.md"      = "$repo\README.md"
    }
    foreach ($entry in $files.Keys) {
        [System.IO.Compression.ZipFileExtensions]::CreateEntryFromFile($archive, $files[$entry], $entry) | Out-Null
    }
} finally { $archive.Dispose() }
Write-Output "Made $zip"
