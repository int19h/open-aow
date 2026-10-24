# Specifications

One file per subsystem. Each specification states inputs, outputs, the algorithm or mapping in
enough detail that two implementers write the same behavior, the edge cases, and a done measure
that a command can check. The design document (`../design.md`) holds the decisions; a
specification holds the detail needed to build one part.

| file | covers | state |
|---|---|---|
| `transpiler.md` | the Delphi 3 grammar subset, the construct by construct mapping to C#, the metaclass design, the byte-match idiom normalization, the override mechanism, the report format, the semantic traps, the probe suite | draft |
| `rtl-shim.md` | the Delphi RTL and VCL subset the transpiled code calls: `Random`, strings, `TList`, `TStringList`, streams, `TComponent`, exceptions, sets, the host interface, with exact semantics and probe tests | draft |
| `persistence.md` | the engine's property tables, storage streams, class registry and the save container, what the port must preserve, the shim requirements and the round trip milestones | draft |
| `graphics.md` | the surface layer: `TSurfaceDesc`, pixel formats, the blitters, RLE sprites, blend tables, palettes and remaps, the display controller and the frame loop | planned |
| `ui-layout.md` | the form file conversion: the form file grammar, the component and property model, cross-module references, translator components, the loader and handler binding | planned |
| `platform.md` | the SDL host: window, input, timing, audio mixing, module playback, settings, data path resolution and the mobile data install | planned |
| `testing.md` | the layers, the probe runner and probe set, the headless oracle built on the recovery's playtest harness, frame comparison, fixtures | draft |
| `localization.md` | the Multilizer dictionary format and the string table that replaces it | planned |
| `networking.md` | the token protocol as recovered and the socket transport that replaces DirectPlay | later |
| `modding.md` | the data model exposure, the code mod API and the scripting layer | later |

Order of writing: transpiler, rtl-shim, persistence, testing, graphics, ui-layout, platform,
localization. The first four are what the first two milestones need.
