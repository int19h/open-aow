# Rules for the parallel tracks of milestones 3 to 6

From 2026-10-26 four implementer sessions run at once (`docs/milestones.md` section 8): graphics
(brief 3.1), UI layout (brief 4.1), platform (brief 5.1) and localization (brief 6.1). Each brief
names its track's directories. These rules apply to all four on top of both `AGENTS.md` files.

- Each track works in its own git worktree of the product repository, named in its prompt, on its
  own branch. Write only in that worktree and in the private repository paths the brief names.
- The transpiler (`D:\git\open-aow-private\tools\transpile`) is frozen while the tracks run, and
  `src/Generated` is not regenerated. A track that needs a transpiler change writes an override
  instead, and if no override can do it, stops and reports the change it needs.
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
  2026-10-26 the layouts as generated C# (`ui-layout.md` section 1), are product source.
- Done means the brief's measure, plus: the root solution builds with zero warnings, all product
  tests pass, and `python tests/probes/run.py --transpiled` still matches.
