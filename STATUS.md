# Status

Last update: 11-11-2026

## Now

- Version 0.2.0 built and installed in the game's `BepInEx\plugins\FollowZoom\` (11-11-2026). Zoom while following works in game (0.1.0, 11-11-2026).
- New in 0.2.0, not tested in game yet: the zoom you pick stays when the follow stops or hops to the next vehicle. Setting `KeepZoomAfterFollow` (default true) in `BepInEx\config\highway1027.planb.followzoom.cfg`. See `docs/HOW_IT_WORKS.md`.
- Also in 0.2.0, not tested in game yet: a "Keep zoom after follow (Follow Zoom)" checkbox at the bottom of the game's Options > General tab, saved in the same `.cfg` setting.

## Next

1. Sitting B in `TESTING.md` (keys, max zoom out, keep zoom on stop and hop, the Options checkbox), tonight 11-11-2026. Tim wants to test before deciding the release.
2. Release route, after Sitting B: my recommendation is a GitHub release (Nexus optional) with install steps, announced in the Steam Workshop Discussions and General Discussions. No Workshop item or Lua layer for now: a Workshop item cannot install BepInEx or load a DLL, and a link-only item risks removal. The official Workshop has 22 items, all Lua. Same question for Rail Depot.

## Open

- Release route decision (Next 2).

## Inbox

(Follow Zoom ideas only; other mod ideas go to the Plan B Mod Kit Inbox.)

- 11-11-2026: Settings menu like Zomboid's mod options. Researched: the Lua API has none. The Options menu checkbox is now built (0.2.0). Still open: a `GFollowZoom` Lua global so a Lua mod can set it (about 10 lines). Only worth it with a Workshop release.
- 11-11-2026: Options checkbox label in other languages (now English only).
