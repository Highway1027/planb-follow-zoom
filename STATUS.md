# Status

Last update: 11-11-2026

## Now

- Version 0.2.0 built and installed in the game's `BepInEx\plugins\FollowZoom\` (11-11-2026). Zoom while following works in game (0.1.0, 11-11-2026).
- New in 0.2.0, not tested in game yet: the zoom you pick stays when the follow stops or hops to the next vehicle. Setting `KeepZoomAfterFollow` (default true) in `BepInEx\config\highway1027.planb.followzoom.cfg`. See `docs/HOW_IT_WORKS.md`.

## Next

1. Sitting B in `TESTING.md` (keys, max zoom out, keep zoom on stop and hop, the setting), tonight 11-11-2026.
2. Release route: my recommendation is a GitHub release (Nexus optional) with install steps and the config file, no Workshop or Lua layer for now. Reasons: a Workshop item cannot install BepInEx or load a DLL, and a link-only Workshop item risks removal. Tim to confirm. Same question for Rail Depot.
3. Maybe later: a "Keep zoom after follow" checkbox in the game's own Options menu, added by the plugin itself (no Lua needed; see Plan B Mod Kit `API_NOTES.md`, "Settings, Lua and C# together").

## Open

- Release route decision (Next 2).

## Inbox

(Follow Zoom ideas only; other mod ideas go to the Plan B Mod Kit Inbox.)

- 11-11-2026: Settings menu like Zomboid's mod options. Researched: the Lua API has none. Options: the plugin adds a row to the game's Options menu (Next 3), or a `GFollowZoom` Lua global so a Lua mod can set it (about 10 lines). Only worth it with a Workshop release.
