# How Follow Zoom works

Last checked against the game: 11-11-2026 (BepInEx log reports Unity 6000.0.58).

## Why zoom is locked while following (vanilla)

The follow button starts `SSceneSingleton<SSceneCinematic>.Inst.cameraFollow` (`CinematicCameraFollow`). While it plays:

1. `CinematicCameraFollow.Play()` saves the current zoom and sets `GCamera.zoomLevelTarget = 1`.
2. `CinematicCameraFollow.Update()` runs every frame just before `SCamera.Update()` (`SMain` calls `SSceneCinematic.UpdateJustBeforeCamera()`). For vehicles, drones and watched buildings it sets `GCamera.zoomLog = Lerp(start zoom, GCamera.zoomLogs[1], ...)`: after one second the zoom is pinned to level 1.
3. The same `Update()` stops the follow when the right or middle mouse button is held, or when `Input.mouseScrollDelta` is not zero. So the mouse wheel does not zoom: it ends the follow. `Stop()` then puts the old zoom back.
4. `SMouse.Update()` still turns the wheel and the zoom keys into `GCamera.zoomLevelTarget`, but it only animates `GCamera.zoomLog` when `SSceneCinematic.IsCameraOverrided()` is false. The follow cinematic always returns true.

`zoomLevelTarget` is an index (0 = closest, 7 = whole planet) into `GCamera.zoomDistances` {10, 22, 42, 70, 170, 700, 1650, 2000}; `GCamera.zoomLogs` holds the matching log10 values. `GCamera.zoomLog` is what the camera actually uses.

## Why not Lua

A Lua mod could write `GCamera.zoomLevelTarget`, but the wheel still ends the follow (Lua cannot block that check) and the follow overwrites `zoomLog` every frame right before the camera update. So C# it is.

## The zoom patches (`src/FollowZoom/Plugin.cs`)

Both only act during a "normal" follow: vehicles, drones and watched buildings. Animals, satellites, space lifts and rockets set their own zoom and stay vanilla.

1. **Transpiler on `CinematicCameraFollow.Update`**: the one read of `Input.mouseScrollDelta` in the stop check is replaced by a call that returns zero during a normal follow. Right and middle mouse still stop the follow. If the game changes and the read is not found, a warning is logged and the wheel ends the follow as before.
2. **Postfix on `CinematicCameraFollow.Update`**: after the game pins `zoomLog`, we overwrite it with our own value that moves towards `GCamera.zoomLogs[GCamera.zoomLevelTarget]`, with the same smoothing `SMouse` uses (`SMisc.SmoothDamp_realtime`, speed 4, fade 0.5). Because `Play()` sets the target to 1, a new follow still zooms in to level 1 first, as in vanilla.

The Update postfix also notes whether the player zoomed: `zoomLevelTarget` differs from the level `Play()` set.

## Keeping the zoom (0.2.0, setting `KeepZoomAfterFollow`, default true)

Vanilla `Stop()` sets `zoomLevelTarget` back to `_initialZoomLevel`, saved by `Play()`. Clicking follow again hops to the next vehicle by calling `Play()` again. That saves the player's zoom as "before the follow" and resets the zoom to level 1.

3. **Prefix + postfix on `CinematicCameraFollow.Play`**: on a hop during a normal follow, put back the real `_initialZoomLevel` from before the first follow. If the player had zoomed, also put back their zoom level.
4. **Prefix + postfix on `CinematicCameraFollow.Stop`**: if the player zoomed during a normal follow, put their zoom level back after vanilla `Stop()`. `SMouse` then holds the camera there. If they never zoomed, vanilla restores the old zoom.

With the setting false, patches 3 and 4 do nothing (vanilla). The setting lives in `BepInEx\config\highway1027.planb.followzoom.cfg`, created on the first game start.

## Options menu checkbox (`src/FollowZoom/OptionsMenu.cs`)

5. **Prefix on the private `SSceneOptions.Refresh`**: the Options menu builds its rows fresh from `GOptions.listOptions[tab]` each time. Just before that, we add `KeepZoomOption` to the end of tab 0 (General) once. It is a game `COptionBool` that reads and saves the BepInEx config entry instead of PlayerPrefs, so the `.cfg` file stays the only place the setting lives. The game's Confirm, Cancel and Default buttons handle it like any other option. The game's text table has no label for it, so `InstantiateUI` sets the label and tooltip itself (English only). If adding it fails, it logs once and the checkbox is missing; the zoom patches keep working.

## Possible Lua layer (not built)

The game's Lua engine makes every `[MoonSharpUserData]` class whose name starts with `G` or `S` a Lua global, in any loaded DLL (`SLua.GetLuaGlobals`). A static `GFollowZoom` class in this plugin would let a Lua mod set `GFollowZoom.keepZoomAfterFollow`. See the Plan B Mod Kit `API_NOTES.md`.

Any exception turns the mod off and logs once; the game then behaves as vanilla.
