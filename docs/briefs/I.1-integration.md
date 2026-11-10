# Brief I.1: integration after the parallel tracks

The four parallel tracks of milestones 3 to 6 are merged (graphics 3.1, UI layout 4.1, platform 5.1,
localization 6.1). Each had write limits and a frozen transpiler, so each left integration work
behind. This brief does that work. The transpiler is no longer frozen for this brief.

You work in `D:\git\open-aow` on the branch the prompt names, and in `D:\git\open-aow-private`.
Read both `AGENTS.md` files. The sandbox has no network (the usual `--no-restore` and `APPDATA`
rules, no NuGet configuration files).

## What to do

1. **The layout's manifest bypass.** `src/Layout/Layout.targets` removes the generated
   `AoWImage.cs` from compilation to replace `AOWImage` (Delphi `TAOWImage`). Replace that with the regular mechanism:
   skip entries in `src/Overrides/overrides.toml` for exactly the members the layout override
   replaces, the override code under `src/Overrides`, regeneration, and no `Compile Remove` of a
   generated file anywhere.
2. **The platform follow-ups** listed in `D:\git\open-aow-private\data\platform\5.1-report.md`
   (section "integration steps for the design session"): the headless host on the platform's
   `LayeredFileSystem` (keeping the harness's autosave target and scripted settings), the frame
   clock in the shared clock contract (`IClock` gains what `IFrameClock` has, or the shim consumes
   `IFrameClock`, whichever keeps one contract), `Rtl.ExtractRelativePath` on the emulated file
   service with its virtual drive, and the three registry popup-event overrides the report names.
3. **The layouts against the regenerated code.** The graphics track regenerated `src/Generated`
   after the layouts were generated. Rerun the layout generator (`tools/forms`, README) against the
   current `src/Generated` and confirm the output is byte-identical, or commit the regenerated
   layouts with a note of what changed and why.
4. **One check command.** `testing.md` section 1 names `python tools/check.py`, which runs the
   layers in order. Write it in the private repository: the product build and tests, the
   transpiler tests, the forms tests, the localization checks, the transpiled probes, and the
   oracle's `small` run for 50 turns compared with the original, stopping at the first failure and
   printing one line per layer.

## Done measure

1. `python tools/check.py` passes every layer, and its output is in the report.
2. No `Compile Remove` of a generated file remains, and the headless host uses `LayeredFileSystem`.
3. The seven oracle scenarios still equal the original (run them, report the counts).

Report the outputs, what changed in each item, and anything not done. Keep a status file at
`D:\git\open-aow-private\tools\obj\I.1-status.md`.
