# Follow Zoom testing

Blocking right now: nothing.

Automated checks: dotnet build src/FollowZoom -c Release; last run 11-11-2026: passed.
No unit tests: the mod has no logic of its own apart from the game's camera code.

---

## Sitting B: zoom while following, tonight (about 10 min)

Before you start:
- Follow Zoom 0.2.0 is installed. To check: after starting the game, BepInEx\LogOutput.log
  shows "Follow Zoom 0.2.0 loaded". I can read it for you.
- Load a save with a train or truck line that has at least two vehicles on it.

B1. Select a train or truck, then click the follow button on its selection panel.
-> The camera flies to the vehicle and zooms in close, as before.

B2. Press the zoom keys (the ones set under zoom in and zoom out in the game's controls).
-> The zoom changes and the follow goes on, same as the mouse wheel.

B3. Scroll all the way out, then keep scrolling.
-> It stops at the whole-planet view without errors. Note if the follow looks odd there.

B4. Scroll back in to a middle zoom. Click the follow button again.
-> The camera hops to the next vehicle on the line and keeps your middle zoom.

B5. Hold the right mouse button.
-> The follow stops and the zoom stays where you put it. It does not jump back.

B6. Select a vehicle and click follow, but do not zoom. Then hold the right mouse button.
-> The follow stops and the zoom goes back to what it was before the follow (vanilla).

B7. Quit the game. Open BepInEx\config\highway1027.planb.followzoom.cfg in Notepad.
-> It has KeepZoomAfterFollow = true. Set it to false and save.

B8. Start the game, follow a vehicle, zoom out a bit, then hold the right mouse button.
-> The zoom goes back to the zoom from before the follow (the old behaviour).
   Set KeepZoomAfterFollow back to true afterwards if you prefer that.

B9. Quit the game. Tell me when done: I read the log myself.
-> (I check BepInEx\LogOutput.log for Follow Zoom errors.)

## Results so far

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
