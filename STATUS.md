# Status

Last update: 11-11-2026

## Now

- Version 0.1.0 built and installed in the game's `BepInEx\plugins\FollowZoom\` (11-11-2026). Two Harmony patches on `CinematicCameraFollow.Update` (see `docs/HOW_IT_WORKS.md`).
- Loads in game and works: Tim zoomed in and out while following (first try, 11-11-2026).

## Next

1. Keep the zoom the player picked: it must not go back to the old zoom (Tim, 11-11-2026). Most likely this is `CinematicCameraFollow.Stop()` restoring `_initialZoomLevel`. Fix: patch `Stop()` to keep `zoomLevelTarget` once the player has zoomed during a follow. First ask Tim when he saw the zoom go back: on stopping the follow, or during it.
2. The rest of Sitting A in `TESTING.md` (keys, max zoom out, right mouse stop), tomorrow evening (12-11-2026).
3. Later: decide how to release. The Workshop delivers Lua mods; a BepInEx plugin also needs BepInEx installed by hand. Options: GitHub release or Nexus Mods with install steps, and maybe a Workshop page pointing there. Check how other Plan B Terraform BepInEx mods ship.

## Open

- Release route for BepInEx mods (see Next 3). Same question for Rail Depot.

## Inbox

(Follow Zoom ideas only; other mod ideas go to the Plan B Mod Kit Inbox.)

- 11-11-2026: Settings menu (like Zomboid's mod options): a Lua Workshop layer where the player picks "keep zoom" or "go back to old zoom" after a follow. Could justify a Workshop release. First research: does the Lua API have mod settings (none found in `PlanB_api_doc.lua`, only the game's own `GOptions`), and how do other Plan B mods do settings and ship BepInEx.
