# Follow Zoom testing

Blocking right now: nothing.

Automated checks: dotnet build src/FollowZoom -c Release; last run 11-11-2026: passed.
No unit tests: the mod has no logic of its own apart from the game's camera code.

---

## Sitting A: zoom while following a vehicle (about 5 min)

Before you start:
- Restart the game if it is running: Follow Zoom 0.1.0 loads on game start.
- Load a save that has at least one train or truck line with a vehicle on it.

A1. Select a train or truck, then click the follow button on its selection panel.
-> The camera flies to the vehicle and zooms in close, as before.

A2. Scroll the mouse wheel out a few notches, then back in.
-> The camera zooms out and in smoothly and keeps following the vehicle.

A3. Press the zoom keys (the ones set under zoom in and zoom out in the game's controls).
-> Same as A2: zoom changes, the follow goes on.

A4. Scroll all the way out, then keep scrolling.
-> It stops at the whole-planet view without errors. Note if the follow looks odd there.

A5. Hold the right mouse button.
-> The follow stops and the zoom goes back to what it was before A1 (vanilla behaviour).

A6. Quit the game. Tell me when done: I read the log myself.
-> (I check BepInEx\LogOutput.log for "Follow Zoom 0.1.0 loaded" and no Follow Zoom errors.)

## Results so far

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
