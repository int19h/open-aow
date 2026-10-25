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
| `graphics.md` | the surface layer: `TSurfaceDesc`, pixel formats, the clipping rules, the 39 assembler blitters and their conformance probe, blend tables, the image library format and show dispatch, fonts and sequences, the display controller, the frame loop and the presenter, the frame oracle | draft |
| `ui-layout.md` | the form files as data: the text form grammar, the component classes and published property model, the module loading order and the VCL reference fixup rules the loader reproduces, the generated property, class and handler tables, translators, the path from loaded objects to the frame, the form checks | draft |
| `platform.md` | the SDL host: startup and the main form's role, window and presenter, input mapping, clock, audio behind the original `TSoundSystem` abstraction, the intro video, the two-layer case-insensitive file service with current directory emulation, settings in place of the registry, message boxes, plug-ins, the networking transport hook, the headless host | draft |
| `testing.md` | the layers, the probe runner and probe set, the headless oracle built on the recovery's playtest harness, frame comparison, fixtures | draft |
| `localization.md` | the Multilizer dictionaries (format reversed in `../../data/localization/`), the exact-match lookup, the three translation paths of the engine, the translator components, English as shipped and the four other languages as a setting | draft |
| `networking.md` | the token protocol as recovered and the socket transport that replaces DirectPlay | later |
| `modding.md` | the data model exposure, the code mod API and the scripting layer | later |

Order of writing: transpiler, rtl-shim, persistence, testing, graphics, ui-layout, platform,
localization. The first four are what the first two milestones need. State on 2026-10-25: all eight
are drafts; networking and modding wait for their scope decisions (`../design.md` open questions 7
and 9).
