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
| 9.3 | `with` blocks to plain code and the assembler shapes A1 to A3 to plain C# (brief 9.3a), the split constructors to constructors (briefs 9.3b-1 to 9.3b-20 and CF-1), and one constructor rule with three shapes, plain, guarded and pass-through (CF-4, decision D1 of `data/rewrite/review-9x.md`) | O3 holds. No `w__N` local, no recovery thunk, no `Create__init` declaration and no raw constructor remains in `src`, apart from the absent-base rows of `data/rewrite/ctor-progress.md`. The guarded classes are exactly the ones that inventory lists with a failure normal play reaches |
| 9.4 | **Done, 2026-10-30, CF-3.** Metaclass objects retired (briefs 9.4-1 to 9.4-6, CF-2 and CF-3): the engine's class registry carries creation by class id, descriptor families with static abstract interface members carry the remaining class-side dispatch, and one catalog type and one registration file, `ClassRegistry.cs`, hold every descriptor and kind | O3 holds. The shim's `Meta` type and `MetaRegistry.cs` are deleted, and the registry test finds exactly one registration line for every class |
| 9.5 | Strings: `AnsiString` to `string`, one code page 1252 char per byte, bytes at the data and font boundaries, in one pass over the whole tree after the first cleanup window of 9.8 (briefs 9.5-0 to 9.5-2, `../design.md` open question 1, resolved 2026-10-26) | O3 holds and the frame fixtures hold. `AnsiString` appears nowhere under `src`, and the strings check of the `product` layer passes |
| 9.6 | Unions for data closed by nature (`FormatArg`, `MsgArg`, `BlendValue`) and virtual methods for the kind families, presentation included: structure and event-log views, production orders and diplomatic proposals as open records with registries for new kinds (briefs 9.6-0 to 9.6-8). No `closed` type, because `closed` bars code mods | O3 holds. No screen method tests a structure or event-log type, no switch selects on a production id or a diplomatic kind outside the kind classes, and `IntOrRef` remains only in `THashItem` |
| 9.7 | The port's own save and map formats: versioned JSON documents the game writes pretty printed, a converter library and command-line tool to and from the original formats, and the gate through the converter (briefs 9.7-0 to 9.7-4) | Every corpus file round trips byte for byte through the converter, the port writes only own documents, and O3 and the scenarios layer hold through the converter |
| 9.8 | Translation artifacts and transitional scaffolding, the cleanup briefs of `data/rewrite/review-9x.md`: the `TCombatData` object fields (C1), construction tests without hash baselines (C6), recovery comments and padding (C9), qualified names, casts and usings (C7), numbered temporaries, `Result` shapes and loops (C8a to C8c), layout ownership (C11), files and names (C10), object ids in typed fields (C12) | O3 holds and the build has zero warnings with the Units `NoWarn` list reduced to CS0465, CS0649 and CA2255. The analysis build reports no redundant cast, parenthesis, `using` or qualification, no numbered temporary remains beyond the residue the tool lists with reasons, no hash baseline remains in the tests, and no object id sits in an `int` field |

The steps do not run in row order. CF-1 to CF-4 close 9.3 and 9.4 together. 9.8 runs in windows
around the others: C1 at once, C6 to C8c between 9.4 and 9.5 (decision D5), C11 and C10 after
9.5-1, C12 after 9.6-7. 9.6 and 9.7 run together after C10, and 9.7-3 follows 9.6-4. Each step is
one or more briefs, each brief one unit or class family, and the oracle runs after each brief. The
order of the briefs is `data/rewrite/roadmap.md` in the private repository.

## 10. Mods

Opens when 9.7 is done and 9.6-2, 9.6-3, 9.6-6 and 9.6-7 have opened the structure, event-log,
order and proposal kinds. The design is `data/rewrite/mods-design.md` in the private repository
until step 10.11 moves it to `specs/modding.md`. Every step keeps the gate's layers at their counts
with the harness under `--no-mods`, and a game with no mods writes the same bytes as before.
Network agreement between peers with different mod sets waits for the networking work, which
section 7 keeps out of the first release.

| step | work | done |
|---|---|---|
| 10.0 | Census: every persisted integer that names a class, race, ability, spell, unit, hero, item, map object, order or proposal kind, the index constants in code, the race tables and every race packed into bits, the 12 `.pfs` files, and every `FixedArray_*` type classified as keep, named constant or data, with whether a `ReadWrite` persists it | `data/mods/10.0-census.json` in the private repository classifies every `rw*` site and every `FixedArray_*` type |
| 10.1 | Game set documents: the converter kind `game-set`, `ref` annotations on id-valued properties, a version bump | G1 and G2 hold over the game set files |
| 10.2 | Fixed arrays of the keep and named-constant classes (issue #72): one named constant per game constant and a role name per kept layout, never a generated type per size | the census lists only kept layouts, named constants and the data class that 10.7 and 10.8 take |
| 10.3 | The mod loader (`src/Modding`): manifest, discovery, order, scopes, load contexts, consent, file layers, the `LoadExternalPlugIns` hook, `--mods` and `--no-mods` | fixture mods prove order, refusals, cycles, shadowing and path escapes |
| 10.4 | Registries: the facade, id assignment, `[ModdingApi]` and the API baseline test | two load orders of a two-mod set give one id table |
| 10.5 | Persistence: `$mods`, qualified names, schema fragments, `$ext`, mod migrations, refusals of missing mods, the export refusal, `aowconv --mods` | fixture documents round trip and every refusal names the mod |
| 10.6 | Data and strings: game set patches, string tables, new code page 1252 languages, asset layers | a patched unit shows in a city's production list in a headless run |
| 10.7 | Races as data (issue #72): the race tables move into race resources, fixed-13 types become lists sized by the race count, every `race < 13` assumption and bit-packed race is widened, and a `races` patch collection opens races to mods | a fixture adds a 14th race through game set data, and a headless game creates its units and flags without index errors |
| 10.8 | Content counts as data (issue #72): the other arrays of the data class become lists sized by the game set or the rules data, family by family | the census lists only the keep and named-constant classes |
| 10.9 | First proof: the sample mod `example.frost` and a gate layer `mods`, on the Windows and the Linux gate | a seeded headless run with the mod plays three days, raises its event and runs its renderer, the save names the mod, a run with shifted ids writes the same document, and a load or export without the mod is refused by name |
| 10.10 | The mod SDK: reference assemblies, analyzers, `aowmod schema`, `pack` and `check`, a template | the sample builds from the template with zero warnings |
| 10.11 | Documents: `../design.md` section 6, `specs/modding.md`, this file | the documents name the code as it is |
