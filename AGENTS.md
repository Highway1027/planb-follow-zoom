# Follow Zoom

Rules and context for every AI coding agent (Claude Code, Antigravity, Codex). This is the single source; tool-specific files only point here.

Paths use `<drive>`: `D:` on Tim's desktop, `C:` on his other PCs.

**Start of a session:** read `STATUS.md`.

## What this is

A Plan B Terraform mod: while the camera follows a vehicle (the follow button on a train, truck or boat's selection panel), the mouse wheel and zoom keys zoom in and out instead of stopping the follow. For Tim first, Steam Workshop later.

C# BepInEx plugin (`src/FollowZoom`), because the follow camera forces the zoom every frame and stops on any mouse wheel input, and Lua cannot change either. How it works: `docs/HOW_IT_WORKS.md`.

Related: `<drive>:\Game mods\Plan B Terraform\Plan B Mod Kit` holds the shared API notes (`docs/API_NOTES.md`). Add general API findings there, mod-specific ones here. Rail Depot (same folder) is another BepInEx mod by Tim; both can be installed together.

## Commands

| What | Command |
| --- | --- |
| Build | `dotnet build src/FollowZoom -c Release` (.NET SDK 10; on a C: PC add `-p:GameDir="..."`) |
| Install locally | `powershell -File scripts/install.ps1` (builds, then copies the DLL to `<game folder>\BepInEx\plugins\FollowZoom\`; game must be closed) |
| Release zip | `powershell -File scripts/package.ps1` (builds, then makes `dist/FollowZoom-<version>.zip` with the `BepInEx/plugins/FollowZoom/` layout; version from `Plugin.cs`) |
| Test | No unit tests: the mod has no logic apart from the game's own camera code. In-game checks in `docs/TESTING.md`. |
| Read game code | `ilspycmd -p -o decompiled "<game folder>\Plan B Terraform_Data\Managed\Assembly-CSharp.dll"` (`decompiled/` is git-ignored: never commit game code) |
| BepInEx log | `<game folder>\BepInEx\LogOutput.log` (look for "Follow Zoom") |

## Rules

- **Automated test first.** Any logic that does not need Unity goes in a plain C# class with unit tests. `docs/TESTING.md` holds only what needs the game.
- **Never break a save.** The mod stores nothing; keep it that way unless there is a good reason.
- **Patch little.** Prefer reading game state and calling the game's own methods over replacing them. Every patch gets a comment: what it changes and why.
- **Guard everything.** A Follow Zoom error logs once and turns the mod off (the game then behaves as vanilla); it never crashes the game.

## Git and releases

Work on `develop`. Merging to `main` or publishing to the Workshop is a release: only with Tim's OK. Downloads (tools, libraries) need Tim's OK first.

## Focus, skills, writing

- At session start name the top open items from `STATUS.md`. When a new idea comes up mid-task, ask: **"Now, or park it in the backlog?"** Parked ideas: one dated line in the `STATUS.md` Inbox (ideas for other mods: the Plan B Mod Kit Inbox).
- Routines live in `.claude/skills/<name>/SKILL.md` (Antigravity loads them through `.agent/skills.json`): `followzoom-wrap-up`. Fix a skill as soon as it proves wrong; propose one when a routine repeats.
- Plain English, short sentences, dates as DD-MM-YYYY. Files here must make sense on a PC without Tim's personal notes.

## Docs index

| File | Read when |
| --- | --- |
| `STATUS.md` | Every session start |
| `docs/HOW_IT_WORKS.md` | Before changing the patches |
| `Plan B Mod Kit/docs/API_NOTES.md` | Before using a game class |
| `docs/TESTING.md` | Hand tests still open |
| `SESSION_HISTORY.md` | Troubleshooting, or what changed when |
