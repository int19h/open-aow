# Rules for the parallel tracks of milestones 3 to 6

From 2026-10-26 four implementer sessions run at once (`docs/milestones.md` section 8): graphics
(brief 3.1), UI layout (brief 4.1), platform (brief 5.1) and localization (brief 6.1). Each brief
names its track's directories. These rules apply to all four on top of both `AGENTS.md` files.

- Each track works in its own git worktree of the product repository, named in its prompt, on its
  own branch. Write only in that worktree and in the private repository paths the brief names.
- Since the hand-over on 2026-10-27, `src/Units` is source and is edited by hand. Every edit
  keeps `python tools/check.py` passing from `D:\git\open-aow-private`. The transpiler stays
  in the private repository and emits reference diffs outside the product repository.
- The probe runner (`tests/probes/run.py`) is shared: add probes, change the runner only where a
  brief says so.
- The sandbox has no network. The design session restores the packages a brief names before the
  run. Pass `--no-restore` to `dotnet` commands, set `APPDATA` under the solution's `obj` folder
  for a package-free restore of a new project, add no NuGet configuration files and no packages.
- No git commands that change the index or the working tree. Keep a status file at the path the
  prompt names, so that a resumed turn knows where it stands.
- Game files under `D:\Games\GOG\Age of Wonders` are read only and never copied into a tracked
  path. The product repository carries no game data (images, sounds, dictionaries, maps): the
  port reads them from the user's install at run time, and test expectations derived from game data
  live in the private repository. Code is the exception: the transpiled game code, and since
  2026-10-26 the layouts as editable C# (`ui-layout.md` section 1), are product source.
- The user works on this PC while runs go on. Every executable you build and run (tests, tools,
  probes in C#) calls `SetErrorMode(SEM_FAILCRITICALERRORS | SEM_NOGPFAULTERRORBOX |
  SEM_NOOPENFILEERRORBOX)` first thing, so a crash ends the process instead of opening a dialog,
  and nothing opens a window on the desktop (use the headless host's null display, or a hidden
  window). Never play sound.
- Native runs of the original game (`AoW.exe`, `AoWEd.exe`) are allowed again (user, 2026-10-26,
  after a pause because they disturbed full-screen applications). They run with sound and music
  volume 0, through the harness's per-run registry setup, and the registry is verified restored.
- Done means the brief's measure, plus: the root solution builds with zero warnings, all product
  tests pass, and `python tests/probes/run.py --transpiled` still matches.
