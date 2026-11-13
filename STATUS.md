# Status

Last update: 13-11-2026

## Now

- Version 0.2.0 built, installed and fully tested in game (Sitting B passed, 13-11-2026): zoom while following (wheel and keys), the zoom stays when a follow stops or hops, and the "Keep zoom after follow (Follow Zoom)" checkbox in Options > General works and saves to `BepInEx\config\highway1027.planb.followzoom.cfg`.
- Ready to release. Not merged to `main` yet.

## Next

1. Decide the release route. My recommendation: a GitHub release (Nexus optional) with install steps, announced in the Steam Workshop Discussions and General Discussions. No Workshop item or Lua layer for now: a Workshop item cannot install BepInEx or load a DLL, and a link-only item risks removal. The official Workshop has 22 items, all Lua. Same question for Rail Depot.
2. After the decision: merge `develop` to `main` as 0.2.0 and publish (Tim's OK first).

## Open

- Release route decision (Next 1).

## Inbox

(Follow Zoom ideas only; other mod ideas go to the Plan B Mod Kit Inbox.)

- 11-11-2026: Settings menu like Zomboid's mod options. Researched: the Lua API has none. The Options menu checkbox is now built (0.2.0). Still open: a `GFollowZoom` Lua global so a Lua mod can set it (about 10 lines). Only worth it with a Workshop release.
- 11-11-2026: Options checkbox label in other languages (now English only).
