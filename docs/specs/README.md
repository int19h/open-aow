# Specifications

One file per subsystem. Each specification states inputs, outputs, the algorithm or mapping in
enough detail that two implementers write the same behavior, the edge cases, and a done measure
that a command can check. The design document (`../design.md`) holds the decisions; a
specification holds the detail needed to build one part.

| file | covers | state |
|---|---|---|
| `transpiler.md` | the Delphi 3 grammar subset, the construct by construct mapping to C#, the metaclass design, the byte-match idiom normalization, the override mechanism, the report format, the semantic traps, the probe suite | draft |
| `rtl-shim.md` | the Delphi RTL and VCL subset the transpiled code calls: `Random`, strings, `DelphiList` (Delphi `TList`), `StringList` (Delphi `TStringList`), streams, `Component` (Delphi `TComponent`), exceptions, sets, the host interface, with exact semantics and probe tests | draft |
| `persistence.md` | the engine's property tables, storage streams, class registry and the save container, what the port must preserve, the shim requirements and the round trip milestones | draft |
| `graphics.md` | the surface layer: `SurfaceDesc` (Delphi `TSurfaceDesc`), pixel formats, the clipping rules, the 39 assembler blitters and their conformance probe, blend tables, the image library format and show dispatch, fonts and sequences, the display controller, the frame loop and the presenter, the frame oracle | draft |
| `ui-layout.md` | the form files as data: the text form grammar, the component classes and published property model, the module loading order and the VCL reference fixup rules the loader reproduces, the generated property, class and handler tables, translators, the path from loaded objects to the frame, the form checks | draft |
| `platform.md` | the SDL host: startup and the main form's role, window and presenter, input mapping, clock, audio behind the original `SoundSystem` (Delphi `TSoundSystem`) abstraction, the intro video, the two-layer case-insensitive file service with current directory emulation, settings in place of the registry, message boxes, plug-ins, the networking transport hook, the headless host | draft |
| `testing.md` | the layers, the probe runner and probe set, the headless oracle built on the recovery's playtest harness, frame comparison, fixtures | draft |
| `localization.md` | the Multilizer dictionaries (format reversed in `data/localization/` of the private repository), the exact-match lookup, the three translation paths of the engine, the translator components, English as shipped and the four other languages as a setting | draft |
| `metaclass-retirement.md` | milestone 9.4: the `EClass` descriptor, class ids, instance virtuals, descriptor families, the deletion of the shim's `Meta` (superseded in part by the construction form of the private repository) | done, CF-3 (PR #81) deleted `Meta` and `MetaRegistry` on 2026-10-30 |
| `strings.md` | milestone 9.5: `AnsiString` to `string` with one code page 1252 char per byte, the byte boundaries, the null rule, host text entry points, the rewrite tool and its residue, the proofs | implemented, 9.5-1 merged on 2026-11-05 |
| `unions-and-kinds.md` | milestone 9.6: unions for data closed by nature, virtual methods and registries for the kind families (views, production orders, diplomatic proposals), no `closed` types so code mods can add kinds | implemented, 9.6-1 to 9.6-7 merged on 2026-11-05 |
| `own-formats.md` | milestone 9.7: the port's own save and map formats (canonical JSON, pretty printed for hand editing), the converter to and from the original formats, validation of hand-edited files, the round trip guarantees, the gate through the converter, the exit from the binary-image bridge | implemented, 9.7-1 (PR #94) and 9.7-3 (PR #111) merged by 2026-11-06. Step 1 of the exit is implemented for the pilot scope (`persistence-tree.md`). Step 2 is implemented except PE-2c (stream callbacks, map blobs and run-length lists). Steps 3 to 5 stay open |
| `persistence-tree.md` | PE-1, PE-2a and PE-2b: the typed persistence tree, presence and fallbacks, identity and references, the version 3 document, the enum and record catalogs, the direct original backend, records, bit arrays and raw primitives, and the rwData census | implemented for the pilot classes, 2026-11-08. `ReadWrite` and the converter remain the game path until PE-3 |
| `networking.md` | the token protocol as recovered and the socket transport that replaces DirectPlay | later: multiplayer and play by e-mail are wanted and out of scope for the first release (`../design.md` open questions 7 and 9); the stand-ins are in `platform.md` 9 |
| `modding.md` | the data model exposure, the code mod API and the scripting layer | later: after the oracle passes (`../design.md` section 6) |

Order of writing: transpiler, rtl-shim, persistence, testing, graphics, ui-layout, platform,
localization. The first four are what the first two milestones need. State on 2026-10-25: all eight
are drafts, revised the same evening for the two-stage plan of `../design.md` (mechanical
translation first, idiomatic C# second), the C# 15 target and the record and metaclass surveys in
`data/` of the private repository (`../design.md` section 8). On 2026-10-26 the transpiler became
a C# program on Roslyn and the string conversion of the second stage became one pass. Networking
and modding are scheduled after the first release.
