---
name: followzoom-wrap-up
description: End-of-session routine for the Follow Zoom repo - update STATUS, SESSION_HISTORY and TESTING, run the build, commit and push develop. Use when Tim says wrap up, or a topic is done.
---

# Follow Zoom wrap-up

1. `dotnet build src/FollowZoom -c Release` (through PowerShell). Note the result in `TESTING.md` "Automated checks". If the game is closed and the DLL changed, run `scripts/install.ps1`.
2. `STATUS.md`: update Now / Next / Open, set "Last update". New Follow Zoom ideas: Inbox here; ideas for other mods: Plan B Mod Kit Inbox.
3. `SESSION_HISTORY.md`: new entry on top, 3 to 6 lines (what changed, decisions and why, gotchas, releases). Drop entries older than about two months.
4. `TESTING.md`: add every check you could not do yourself as a sitting (format: rules block at the bottom). Remove passed steps with a dated line under "Results so far".
5. General API findings go to `Plan B Mod Kit/docs/API_NOTES.md` (commit there too). Do not offer `/mod-notes`: Rodi does not play Plan B Terraform.
6. Skills: fix any that proved wrong; propose a new one if a routine repeated.
7. `git status`, commit on `develop` with a message that says what and why, `git fetch`, push `develop` (your own commits only).
8. Report in 2 to 3 lines: what changed, what is untested, what is next.
