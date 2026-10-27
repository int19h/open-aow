# Rules for every Codex run in this repository

This is the product repository of OpenAoW, a C# port of Age of Wonders (1999). It holds `docs/`
(the design, the process, the milestones and the specifications) and `src/` (the .NET solution).
The port's tooling, catalogues, probes and correspondence live in `D:\git\open-aow-private`, and
the recovered Delphi 3 source in `D:\git\aow`. `docs/process.md` describes how work flows.

- Work from the brief you were given. It names the specification, the files to change and the
  done measure. Read `docs/design.md` and the named specification before writing code.
- Do not commit, do not switch branches, do not push, do not change git configuration.
- Never edit `src/Generated/` by hand. It is the transpiler's output (the transpiler lives in the
  private repository). A member that needs hand-written behavior gets an override under
  `src/Overrides/`.
- Never change anything under `D:\git\aow` or `C:\git\int19h\aow`.
- Text files are UTF-8 without a byte order mark and end lines with LF (`.gitattributes`). The
  Delphi sources are read as Windows code page 1252 and never written.
- The build is `dotnet build` with warnings as errors (`Directory.Build.props`), the tests are
  `dotnet test`. The brief's done measure is checked by a command, not by reading.
- The user works on this PC while runs go on. Every executable you build and run (tests, tools,
  probes in C#) calls `SetErrorMode(SEM_FAILCRITICALERRORS | SEM_NOGPFAULTERRORBOX |
  SEM_NOOPENFILEERRORBOX)` first thing, so a crash ends the process instead of opening a dialog,
  and nothing opens a window on the desktop (use the headless host's null display, or a hidden
  window). Never play sound.
- Do not run the original game (`AoW.exe` or `AoWEd.exe`, from the GOG install or a copy, under
  the playtest harness or otherwise) until the user lifts this rule. Even on a hidden desktop it
  disturbs full-screen applications on this PC (user, 2026-10-26). Compare with the recorded
  reference runs under `D:\Build\oracle` instead, and report any task that needs a new native run.
- Report what was done, what was not done and why, and anything the brief did not anticipate. A
  report that claims completion carries the done measure's command output.
- Prose in documents and comments is plain English: no "should", "may", "could", "would",
  "might", no semicolons, no em-dashes.
