# Session history

Newest first, 3 to 6 lines per session: what changed, decisions and why, gotchas, what was released. Keep about two months.

## 11-11-2026: release research, 0.2.0 keeps the zoom

- Research: Plan B C# mods ship as GitHub release zips with BepInEx config files; a Workshop (Lua) mod cannot load a DLL; no Valve rule found on link-only Workshop items. Decision (recommended, Tim to confirm): GitHub/Nexus release with a config file, no Lua layer for now.
- Found: any `[MoonSharpUserData]` `G*` class in a plugin becomes a Lua global, and a plugin can add its own row to the game's Options menu (`GOptions.listOptions`). Both noted in Mod Kit `API_NOTES.md`, not tried in game.
- 0.2.0: setting `KeepZoomAfterFollow` (default true). Patches on `CinematicCameraFollow.Play` and `Stop` keep the player's zoom on stop and on hopping to the next vehicle. Tim saw the reset on stopping; the hop case was found in the code (`Play()` overwrites `_initialZoomLevel`).
- Options menu checkbox (`OptionsMenu.cs`): a `COptionBool` added to `GOptions.listOptions[0]` in a prefix on `SSceneOptions.Refresh`, saving to the BepInEx config entry so the .cfg stays the only store. Label set by hand (SLoc has no entry for new ids).
- Where mods are shared: the official Workshop is live with 22 Lua items, no C# mods; C# mods are on GitHub; Steam Workshop Discussions is the place to announce. Tim wants to test before deciding the release.
- Built and installed; not run in game yet (TESTING Sitting B, merged with the rest of Sitting A).

## 11-11-2026: project set up, 0.1.0 built

- Repo created with the docs convention, copied from Rail Depot (csproj, plugin layout).
- Decompiled the game: the follow camera ends on any mouse wheel input and pins `zoomLog` to level 1 every frame (`docs/HOW_IT_WORKS.md`).
- Decision: C#, not Lua. Lua cannot stop the wheel from ending the follow, and the follow overwrites the zoom right before the camera update.
- 0.1.0: transpiler + postfix on `CinematicCameraFollow.Update`. Built and installed; not run in game yet (TESTING Sitting A).
- Gotcha: `ilspycmd` and `dotnet` are on the PowerShell PATH, not in Git Bash.
- Gotcha: the first build did not load in game. NuGet's HarmonyX 2.16 needs `MonoMod.Backports`, which BepInEx 5.4.23.5 does not ship (it has Harmony 2.9). Fix: the csproj references `BepInEx\core\BepInEx.dll` and `0Harmony.dll` from the game folder. Error shows in Unity's `Player.log`, not in `LogOutput.log`.
- BepInEx plugins never show in the game's Mods menu (Lua mods only).
- Tim's first test: zooming while following works. Next: keep the chosen zoom instead of going back to the old one.
