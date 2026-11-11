# Rules for every Codex run in this repository

This is the product repository of OpenAoW, a C# port of Age of Wonders (1999). It holds `docs/`
(the design, the process, the milestones and the specifications) and `src/` (the .NET solution).
The port's tooling, catalogues, probes and correspondence live in `D:\git\open-aow-private`, and
the recovered Delphi 3 source in `D:\git\aow`. `docs/process.md` describes how work flows.

- Work from the brief you were given. It names the specification, the files to change and the
  done measure. Read `docs/design.md` and the named specification before writing code.
- Do not commit, do not switch branches, do not push, do not change git configuration.
- The game library under `src/Game/` and `src/Engine/` is source and is edited by hand. Every edit keeps
  `python tools/check.py` passing from `D:\git\open-aow-private`. The transpiler emits only
  reference diffs outside the product repository.
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
- Native runs of the original game (`AoW.exe`, `AoWEd.exe`) are allowed again (user, 2026-10-26,
  after a pause because they disturbed full-screen applications). They run with sound and music
  volume 0, through the harness's per-run registry setup, and the registry is verified restored.
- Report what was done, what was not done and why, and anything the brief did not anticipate. A
  report that claims completion carries the done measure's command output.
- Prose in documents and comments is plain English: no "should", "may", "could", "would",
  "might", no semicolons, no em-dashes.

## How new code is written

These rules apply to code written from now on. Existing code moves to them over time through
tooling and reviews, so a change does not rewrite old code to them unless its brief says so. A
tool that generates C# emits code that follows them.

- Use the modern C# features the toolchain supports, up to C# 15.
- Data is immutable by default. Mutable state needs a reason.
- An immutable type is `readonly`: a `record class` or `record struct` as fits, with a primary
  constructor where possible. Members use `required` and `readonly` where they apply.
- A property is an auto-property where possible. Otherwise its backing store is the `field`
  keyword when nothing outside the accessors needs it. A property set only during
  initialization has `init`, not `set`.
- All fields are private. Anything visible outside its type is a property, get-only when
  nothing writes it after construction.
- Names follow the .NET identifier-names guide
  (https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names):
  PascalCase for types, members and constants, camelCase for parameters and locals, `_camelCase`
  for private instance fields, `s_camelCase` for private static fields. Whole words, no
  abbreviations except widely known ones, judged in this project's context (`Aow`, `Hex`,
  `Info`, `Id`, `IO`, `UI`, `AI`, `Tcp`, `Cpu` and the file format names). Two-letter acronyms
  are uppercase, longer ones Pascal-cased.
- Code that works with data uses pattern matching and deconstruction where they make it clearer.
- Calls use named arguments. A call with one argument needs none. From the second argument on,
  name the arguments, except where a strong positional convention carries the meaning (`x, y`,
  `x, y, z`, `width, height` and the like). A boolean argument is always named unless it is the
  first argument.
