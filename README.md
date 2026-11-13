# Follow Zoom

A mod for Plan B Terraform. When the camera follows a train, truck or boat (the follow button on its selection panel), the mouse wheel and the zoom keys zoom in and out instead of ending the follow.

- Zoom in close to watch a train load, or zoom out to see the whole line, and keep following.
- The zoom you pick stays when you hop to the next vehicle (follow button again) and when the follow stops.
- Right or middle mouse still stops the follow, as in the base game.
- A checkbox "Keep zoom after follow (Follow Zoom)" at the bottom of Options > General. Untick it to get the old behaviour back: the zoom returns to where it was before the follow.

It stores nothing in your save. Remove the mod and the game is vanilla again.

## Install

Follow Zoom is a BepInEx plugin, not a Steam Workshop mod, so it does not show in the game's Mods menu.

1. Install BepInEx 5 once (skip this if you already use another BepInEx mod):
   - Download `BepInEx_win_x64_5.4.23.5.zip` from https://github.com/BepInEx/BepInEx/releases (version 5, not 6).
   - Unzip it into the game folder, next to `Plan B Terraform.exe`. To find the folder: Steam > right-click Plan B Terraform > Manage > Browse local files.
   - Start the game once and quit. BepInEx makes its folders (`BepInEx\plugins`, `BepInEx\config`).
2. Download `FollowZoom-<version>.zip` from the Releases page of this repo.
3. Unzip it into the game folder. It holds `BepInEx\plugins\FollowZoom\FollowZoom.dll`, so it lands in the right place.
4. Start the game. `BepInEx\LogOutput.log` in the game folder shows "Follow Zoom <version> loaded".

To update: replace `FollowZoom.dll` with the new one. To remove: delete the `BepInEx\plugins\FollowZoom` folder.

## Settings

Options > General > "Keep zoom after follow (Follow Zoom)", or the same setting `KeepZoomAfterFollow` in `BepInEx\config\highway1027.planb.followzoom.cfg` (made on the first start).

## Compatibility

- Tested with BepInEx 5.4.23.5 on Windows.
- Works together with other BepInEx mods, such as Rail Depot.
- If the mod hits an error, it logs it once in `BepInEx\LogOutput.log` and turns itself off. The game then plays as vanilla.
- A game update can change the camera code. If the zoom stops working after an update, check the log and look for a newer release.

## For developers

Build: `dotnet build src/FollowZoom -c Release` (.NET SDK; set `-p:GameDir="..."` if the game is not on D:). Release zip: `powershell -File scripts/package.ps1`. How the patches work: `docs/HOW_IT_WORKS.md`.
