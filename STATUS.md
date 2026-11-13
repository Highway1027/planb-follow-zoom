# Status

Last update: 13-11-2026

## Now

- Version 0.2.0 built, installed and fully tested in game (Sitting B passed, 13-11-2026): zoom while following (wheel and keys), the zoom stays when a follow stops or hops, and the "Keep zoom after follow (Follow Zoom)" checkbox in Options > General works and saves to `BepInEx\config\highway1027.planb.followzoom.cfg`.
- Ready to release. Not merged to `main` yet.

## Next

1. Release 0.2.0 on GitHub, waiting for Tim's OK. Prepared 13-11-2026: player README with install steps, `CHANGELOG.md`, `scripts/package.ps1` (zip checked: same code as the tested DLL). Steps on OK: merge `develop` to `main`, tag `v0.2.0`, make the repo public (it is private now), `gh release create` with the zip.
2. Then Tim posts the announcement in the Steam Workshop Discussions and General Discussions (link to the release). Same route later for Rail Depot.

## Open

- Tim's OK to release and make the repo public (Next 1).

## Inbox

(Follow Zoom ideas only; other mod ideas go to the Plan B Mod Kit Inbox.)

- 11-11-2026: Settings menu like Zomboid's mod options. Researched: the Lua API has none. The Options menu checkbox is now built (0.2.0). Still open: a `GFollowZoom` Lua global so a Lua mod can set it (about 10 lines). Only worth it with a Workshop release.
- 11-11-2026: Options checkbox label in other languages (now English only).
