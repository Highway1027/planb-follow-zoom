# Follow Zoom testing

Blocking right now: nothing.

Automated checks: dotnet build src/FollowZoom -c Release; last run 11-11-2026: passed.
No unit tests: the mod has no logic of its own apart from the game's camera code.

---

No open sittings.

## Results so far

- 13-11-2026: Sitting B passed (B1-B10): zoom keys, max zoom out, keep zoom on stop and
  hop, vanilla zoom-back without zooming, Options checkbox on and off, setting saved.
- 11-11-2026: Sitting A merged into Sitting B (rest of A plus the keep-zoom checks in 0.2.0).
- 11-11-2026: A2 passed: the wheel zooms in and out while following. Found: the zoom goes
  back to the old level; it should stay where the player put it (STATUS Next 1).
- 11-11-2026: first try failed: the plugin did not load (built against NuGet HarmonyX 2.16,
  needs MonoMod.Backports). Rebuilt against BepInEx's own Harmony 2.9: loads, patches apply.
- 11-11-2026: project set up; 0.1.0 built and installed.

## How this file works

Same rules in every project (source: testing-format.md in Tim's ai-setup repo).

- Automated first: unit tests and the self-test check what a script can. This file
  holds only what they cannot: the look, real clicks and keys, a second player or
  device, real AI text, live data.
- "Blocking right now" names only a test whose result changes the next step.
- A sitting is one topic Tim can test in one go, with an estimated time.
- Each sitting takes the next letter (after Z start at A again). Steps use that letter:
  D1, D2, sub-steps D2.1. A big sitting can have parts; each part takes the next letter.
- "Before you start" holds only what Tim must do or check, and how he sees he has the
  right build (which version, shown where). The agent builds, installs and deploys
  beforehand; if it could not, the sitting says what Tim must do instead.
- When a self-test or scenario covers part of a sitting, the first step runs it and
  says which result to expect.
- One action per line, then what you should see on its own line, starting with "-> ".
- Name the exact button, menu, page or test tool.
- Plain text in steps: no bold, italics or backticks. A blank line between steps.
  Sub-steps on their own lines, indented under the step. Lines under about 90 characters.
- A step earlier results make quite safe says "(assumed, based on <result>)": a glance.
- Set-up a step needs (items, objects, sample data) comes from a test tool the build
  adds, never from Tim spawning it by hand.
- Each time a sitting is added or changed, re-read the whole file: one self-test run
  per session of sittings, sittings that share a set-up merged, steps in the order Tim
  walks through them.
- Tim reports by id ("D3 failed"). A step he does not mention counts as passed.
- Passed steps are removed, with one dated line under "Results so far" (newest first).
  When that list gets long, older lines move to TESTING_HISTORY.md.
