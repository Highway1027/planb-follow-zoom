# Changelog

## 0.2.0 (13-11-2026)

First public release.

- Zoom while following: while the camera follows a train, truck or boat, the mouse wheel and the zoom keys zoom in and out instead of ending the follow. Right or middle mouse still stops it.
- Your zoom stays: when you hop to the next vehicle (follow button again) or the follow stops, the camera keeps the zoom you picked instead of jumping back.
- Options > General gets a checkbox "Keep zoom after follow (Follow Zoom)" (on by default). Untick it for the old jump-back behaviour. Also in `BepInEx\config\highway1027.planb.followzoom.cfg` as `KeepZoomAfterFollow`.
- Stores nothing in your save. On any error the mod logs once and turns itself off.
- Tested with BepInEx 5.4.23.5 on Windows.
