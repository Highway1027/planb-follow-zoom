# Session history

Newest first, 3 to 6 lines per session: what changed, decisions and why, gotchas, what was released. Keep about two months.

## 11-11-2026: project set up, 0.1.0 built

- Repo created with the docs convention, copied from Rail Depot (csproj, plugin layout).
- Decompiled the game: the follow camera ends on any mouse wheel input and pins `zoomLog` to level 1 every frame (`docs/HOW_IT_WORKS.md`).
- Decision: C#, not Lua. Lua cannot stop the wheel from ending the follow, and the follow overwrites the zoom right before the camera update.
- 0.1.0: transpiler + postfix on `CinematicCameraFollow.Update`. Built and installed; not run in game yet (TESTING Sitting A).
- Gotcha: `ilspycmd` and `dotnet` are on the PowerShell PATH, not in Git Bash.
- Gotcha: the first build did not load in game. NuGet's HarmonyX 2.16 needs `MonoMod.Backports`, which BepInEx 5.4.23.5 does not ship (it has Harmony 2.9). Fix: the csproj references `BepInEx\core\BepInEx.dll` and `0Harmony.dll` from the game folder. Error shows in Unity's `Player.log`, not in `LogOutput.log`.
- BepInEx plugins never show in the game's Mods menu (Lua mods only).
