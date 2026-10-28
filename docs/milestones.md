# Milestones

State: draft, 2026-10-25. The order in which the specifications are implemented, what each step
depends on, and the done measure that closes it. The measures are the ones the specifications
define; this file only sequences them. `process.md` says how a step becomes a brief for the
implementer.

## 1. Tooling and the transpiler

| step | work | depends on | done |
|---|---|---|---|
| 1.1 | Probe runner and the first probes (`integers`, `floats`, `random`, `strings`, `format`, `sets`, `enums`, `records-layout`, `arrays`, `pointers`) with the oracle compiler's expected outputs committed | DCC32 at `C:\Tools\Delphi3` | the runner compiles and runs every probe under DCC32 (`testing.md` 2) |
| 1.2 | Transpiler front end: preprocessor, lexer, parser, resolver over the transpile set and the RTL interface sections | `units.toml` | T1, T2 (`transpiler.md` 11) |
| 1.3 | Layout model and the catalogues: `class-layouts.json`, `layout-views.json`, `shape-casts.json`, `initialization-order.json`, `asm-routines.json` as inputs with their checks | 1.2 | the `records-layout` probe passes against the model; the catalogue checks report zero disagreements |
| 1.4 | Emitter for Engine with the shim's core types (`TObject` and metaclasses, `AnsiString`, `TSet`, `Ptr`, exceptions, streams, `TList`, `TStringList`) | 1.2, 1.3 | T3; probes `with`, `case`, `loops`, `exceptions`, `classes`, `properties`, `evalorder`, `lists`, `streams`, `typed-constants`, `init-order` pass (T4) |
| 1.5 | Emitter for the whole transpile set with the overrides manifest and the generated A1 to A3 shapes | 1.4 | T5 with the TODO count reported |

## 2. Persistence and the headless oracle

| step | work | depends on | done |
|---|---|---|---|
| 2.1 | Round trip on the reduced model (`tests/roundtrip-model/`). Skipped on 2026-10-26: the whole set already transpiled when 2.2 started, and the reduced model is a separate Free Pascal dialect | 1.4 | M1a (`persistence.md` 4) |
| 2.2 | Round trip on the real engine and rules. Done on 2026-10-26 as the load and save path: every world file loads and saves, and byte equality moved to 2.4 because the native load changes the game (`persistence.md` 4) | 1.5 | M1b |
| 2.3 | Headless host: `IRtlHost` null services, the load path, `Activate` and the `NewFrame` loop, `AutoSave` at `NewDay`, `--tactical` | 2.2, the asm overrides of `bitop`, `fill`, `hex-coords` and `other` (`transpiler.md` 8.2) | O1 self-determinism on Mountain Clash |
| 2.4 | Save comparison against `D:\Build\oracle` with masks | 2.3 | O2 Mountain Clash, then O3 all seven scenarios and 50 turns, O4 the tactical run (`testing.md` 3.4) Done on 2026-10-26: Mountain Clash equals the original for 50 turns, the six other scenarios for 20, the flagged tactical `heroes` run included, with no mask beyond the two turn-time ids |

Step 2.4 is the first result that proves the port plays the game the original did. Every
divergence found here is a transpiler, shim or override bug until shown otherwise. It is also the
point where the generated tree stops being regenerated and becomes the source of truth
(`../design.md` section 3), and where the second stage of section 9 below opens.

## 3. Graphics

| step | work | depends on | done |
|---|---|---|---|
| 3.1 | Memory surfaces, `TSurfaceDesc`, the rewritten blitters, the `blitters` probe | 1.5 | G1 (`graphics.md` 10) |
| 3.2 | Blend tables and their probe | 3.1 | G2 |
| 3.3 | Image libraries end to end and the `libraries` probe over all 538 files | 3.1, 3.2 | G3 |
| 3.4 | `TSdlDC`, the presenter, the frame loop and the software cursor on the SDL host (step 5.1) | 3.3, 5.1 | a frame renders |
| 3.5 | Frame capture from the rebuilt game, fixtures, comparison | 3.4, 4.2 | G4, G5 |

## 4. UI layout

| step | work | depends on | done |
|---|---|---|---|
| 4.1 | Form file reader, generated property, class and handler tables, `check.py` | 1.5 | U1 (`ui-layout.md` 9) |
| 4.2 | Loading the 77 modules in order with fixups, `dump.py` expectations | 4.1 | U2 |
| 4.3 | The title scene drawn through the manager | 3.4, 4.2 | U3, which is the first frame fixture |

## 5. Platform

| step | work | depends on | done |
|---|---|---|---|
| 5.1 | SDL window, presenter, event pump, clock, file service, settings | 1.5 | P2, P3, P4 (`platform.md` 11) |
| 5.2 | Audio backend: mixer, WAV reader, libopenmpt songs, the fade state machine | 5.1 | P5 |
| 5.3 | Startup through `TMainForm`, the title scene with music | 3.4, 4.3, 5.2 | P1 |
| 5.4 | Intro video through an optional FFmpeg decoder | 5.3 | P6 |

## 6. Localization

| step | work | depends on | done |
|---|---|---|---|
| 6.1 | Dictionary reader, `TIvDictionary` and `TIvTranslator` behavior, `ResStr.mld` at startup | 1.5 | L1, L2 (`localization.md` 7); L3 and L4 with 4.3 |

## 7. Playable game

With 2.4, 3.5, 4.3, 5.3 and 6.1 closed, a player starts a scenario and plays it. What remains
before a release is the content of `../design.md` section 5 milestone 4: saves and loads from the
file dialog, every dialog opened and closed, combat, diplomacy, spells, the campaign, with the
frame fixtures grown along the way. Networking, play by e-mail and the editor are wanted and out of
scope for the first release, so their stand-ins stay. Mobile follows the desktop release
(`../design.md` open questions 7 to 10).

## 8. What is parallel

Steps 1.1 and 1.2 start together. After 1.5, the four tracks 2, 3, 4 and 5.1 to 5.2 are independent
and can run as separate implementer sessions; 3.4, 4.3 and 5.3 join them. The oracle work of track
2 has priority when sessions are limited, because it is the proof that the transpilation is right,
and everything else builds on that proof.

## 9. The second stage: idiomatic C#

Opens after 2.4 and runs alongside tracks 3 to 6, on the pieces those tracks are not touching.
The rules are in `../design.md` section 9. The first passes, in order of payoff:

| step | work | done |
|---|---|---|
| 9.0 | Hand-over to editable src/Units and Aow.Units, with folded partial files and direct layout compilation | Done on 2026-10-27. All 13 verification layers pass with unchanged counts and zero build warnings. The report is `data/9.0-report.md` in the private repository |
| 9.1 | The comparator learns the hash-ordered properties and the model-level comparison, so that later passes can change save order and format | Done on 2026-10-27. All seven oracle scenarios (177 saves) and the 23 scenarios (92 states) equal the original in byte and model modes, and all 13 verification layers pass. The report is `data/oracle/9.1-report.md` in the private repository |
| 9.2 | `Ptr<T>` retired from Engine, HSEngine and the rules: arrays, spans and indices in its place | Done on 2026-10-28. No `Ptr<T>` remains in Engine, HSEngine and the rules (1541 pointer source lines before, 676 after, all in the graphics, image library and byte string code), and all 13 verification layers pass with unchanged counts. The report is `data/rewrite/9.2-report.md` in the private repository |
| 9.3 | Split constructors to constructors, `with` blocks to plain code, the override shapes A1 to A3 folded into their classes | O3 holds; `report.md` no longer lists them |
| 9.4 | Metaclass objects retired: the engine's class registry carries creation by class id, generics with static abstract interface members carry the remaining class-side dispatch | O3 holds; the shim's `Meta` type is deleted |
| 9.5 | Strings: `AnsiString` to `string` with bytes at the data and font boundaries, in one pass over the whole tree (`../design.md` open question 1, resolved 2026-10-26) | O3 holds; the frame fixtures hold |
| 9.6 | Tagged unions and closed families: `union` and `closed` hierarchies where section 9 of the design names them | O3 holds |
| 9.7 | The port's own save and map formats behind converting readers, when a pass above wants a format change | the comparator reads both formats; every shipped file loads |

Each step is one or more briefs, each brief one unit or class family, and the oracle runs after
each brief.
