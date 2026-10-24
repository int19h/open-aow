# Testing specification

State: draft, 2026-10-24. The verification ladder is in `../process.md` section 6. This document
specifies the rungs that need tooling: the probes, the round trip, the headless oracle and the
frame comparison, and how they fit together in one command.

## 1. Layers

| layer | proves | tool | where specified |
|---|---|---|---|
| unit tests | one C# member behaves as documented | xUnit in `src/*.Tests` | `rtl-shim.md` section 8 |
| probes | the transpiler and the shim together reproduce Delphi semantics | `tests/probes/run.py` | `transpiler.md` section 11, this document section 2 |
| round trip | the persistence path is byte exact | `tests/roundtrip/` | `persistence.md` section 4 |
| headless oracle | the rules play the same game as the Delphi build | `tools/oracle/` | section 3 |
| frame comparison | the renderer draws the same pixels | `tools/oracle/frames.py` | section 4 |
| play | a person plays a scenario | none | `../process.md` |

`python tools/check.py` runs the first four layers in that order and stops at the first failure.
It is the command the implementer runs before reporting, and the command the design session runs
before committing.

## 2. Probes

A probe is a Delphi 3 console program under `tests/probes/<name>.pas` that prints lines, one fact
per line, and nothing that varies between runs (no addresses, no times). The runner:

1. Compiles the probe with `C:\Tools\Delphi3\BIN\DCC32.EXE -CC -Q -E<tmp> <probe>` and runs the
   executable. The output is the expected text, stored in `tests/probes/expected/<name>.txt` and
   committed, with the probe source. A probe is recompiled only when its source changes, by a
   hash recorded next to the expected text.
2. Transpiles the probe with `tools/transpile/transpile.py` into `tests/probes/generated/<name>/`,
   builds it against `src/Rtl` with `dotnet build`, runs it and captures the output.
3. Compares the two outputs line by line and prints the first differing line with both texts.

The probe set and what each one covers:

| probe | covers |
|---|---|
| `integers` | wrap on `+ - *`, `div` and `mod` with negative operands, `shl`, `shr` on negative values, `Byte`/`Word` promotion, `Ord(True)`, `Boolean(n)` |
| `floats` | `Round` at halves, `Trunc`, `Int`, `Frac`, `/` on integers |
| `random` | 1,000 values of `Random(n)` for several `n` and of `Random` after `RandSeed := 123456789`, and `RandSeed` after each |
| `strings` | `Copy`, `Delete`, `Insert`, `Pos` at the edges, comparison operators on prefixes and bytes above 127, `S[i] := c` aliasing, `UpperCase`, `CompareText`, `Trim`, `StrToInt` on accepted and rejected inputs, `IntToStr`, `IntToHex`, `TStrings.Text` round trips, `AnsiCompareText` equality over all byte pairs (65,536 lines) |
| `format` | `%s`, `%d`, `%x`, width, precision, `-`, `*`, index, `%%`, errors |
| `sets` | construction, `in`, union, difference, intersection, subset, `Byte(S)` masks, `Include`, `Exclude` |
| `enums` | `Ord`, casts, `Succ`, `Pred`, `Inc`, `Low`, `High`, `for` over a range, `SizeOf` |
| `records-layout` | `SizeOf` and field offsets of 40 records: packed and unpacked, every field size, nested records, arrays, variant parts |
| `arrays` | static arrays with non-zero low bounds, two dimensions, open array parameters, `High`, `Low`, `Length`, array constants |
| `with` | nested `with`, `with` on a record field, on an array element and on a function result, name shadowing |
| `case` | ranges, lists, `else`, `Char` and enumeration selectors |
| `loops` | bounds evaluated once, `downto`, a `Byte` variable to 255, `repeat`, `Break`, `Continue`, `Exit` from nested loops |
| `exceptions` | `finally` order, nested handlers, `on E:` selection, re-raise, exception in a constructor, `except` catching an access violation and a division by zero |
| `classes` | virtual constructors through class references, class methods from instances, `inherited` chains, `inherited;` with no parent method, destructors through `Free`, `is`, `as`, `ClassName`, `ClassType`, `InheritsFrom`, method pointer equality, `Assigned` |
| `properties` | indexed, default array, `index` specifier, read-only, event properties |
| `evalorder` | a tracing function in argument positions (1 to 5 arguments), as operands of `+`, `*`, `and`, `or`, as array indices, on both sides of `:=`, in `with`, in `for` bounds, in `case` selectors |
| `lists` | `TList.Sort` on arrays with many equal keys, `Grow`, `Insert`, `Delete`, `IndexOf`, `TStringList` sorted insertion with each `Duplicates` value, `Find`, `IndexOf` on sorted and unsorted lists |
| `streams` | `TMemoryStream` writes past the end, `Position` and `Size` after each operation, `CopyFrom`, `Seek` with each origin |
| `typed-constants` | mutation of typed constants, record and array constants |
| `init-order` | a program with three units whose `initialization` sections print, in two `uses` orders |
| `files` | `FindFirst` order over a prepared directory, attribute filtering, `ExtractFileName` and the other path functions |

The `records-layout` probe is special: its expected output is also the test data of the
transpiler's layout model (`tools/transpile/tests/test_layout.py`), and the transpiler must pass
it before it emits any explicit layout.

## 3. Headless oracle

### 3.1 What the recovery already does

`D:\git\aow\tools\playtest.py` runs the original and the rebuilt game under a debugger on a hidden
desktop. It prepares a scenario as a save with every player set to computer and the map's two seed
properties (ids `$1C` and `$24` of `TAoWHSMap`) set to the seed, sets `RandSeed` after
`Randomize` through a breakpoint, lets the game play, and collects one autosave per day. Day 1 is
the initialized state and day 21 follows twenty completed turns. `compare_runs` inflates each pair
of saves, normalizes them (it ignores `TAoWHSMap.FTurnTimeElapsed`, id `$37`, and
`FTurnTimeBeep`, id `$3A`), compares the bytes, dumps both with `asg_dump.py` and writes the first
difference as a unified diff and as a list of changed fields. Seven scenarios are prepared:
Mountain Clash, After the Fall, Mine All Mine, Igor's Folly, Love thy Neighbor, Rocky Hollow and
Blood Isles. The rebuilt game equals the original on all of them for 20 turns, and on Mountain
Clash for 50.

### 3.2 What the port adds

The port is driven by calls, not by a debugger. `tools/oracle/run.py` does, for one scenario:

1. Take the prepared seeded save from the recovery's build directory (`<build>/inputs/<name>.asg`),
   or prepare one the same way (`seed_map` is copied into `tools/oracle/asg.py` with the table
   reader it needs, so the port's tools do not import from the `aow` repository at run time).
2. Run the reference: the Delphi build's saves for that scenario, produced once by `playtest.py`
   and kept under `<build>/playtest/<label>/`. The port does not rerun the Delphi build. The
   reference saves are inputs.
3. Run the port headless: `dotnet run --project src/Headless -- --load <save> --seed 123456789
   --turns 20 --out <dir>`. The headless host implements `IRtlHost` with a null display, a null
   sound system, a scripted dialog service that answers every dialog with its default button and
   logs it, and a fixed clock. It loads the save as the game does (`THSEngine.LoadHSMfromStream`
   through the game's load path), sets `RandSeed`, and then drives the turn loop: it calls the
   same engine entry points the main form's frame update calls until the day changes (the hook is
   `TPlayerControl.NewDay`, the breakpoint `playtest.py` uses), calls `TPlayerControl.AutoSave`
   into `<out>/day-<n>.asg`, and repeats until the requested turns or until
   `TAoWHSMap.TriggerGameTerminatedEvent` fires. Which engine entry points the frame update calls
   is read from `AoWUnit.pas` (`TMainForm.DisplayUpdateFrame`) when the harness is written.
4. Compare with `tools/oracle/compare.py`, a copy of `compare_runs` and `normalized_save` with
   two changes: it compares inflated payloads (the port's compressed bytes differ, `persistence.md`
   1.1), and it masks the fields the user accepts as floating point divergence once such fields
   are known, listing every mask in its output.

The result is one JSON per scenario with the first differing day, the changed fields and the diff,
in the format `compare_runs` writes, so that the same reading habits apply.

### 3.3 Determinism requirements on the port

The headless run must be deterministic by itself before it is compared: two runs of the port with
the same input give identical saves. Sources of nondeterminism to remove, each with its place:

- The clock: `GetTickCount`, `timeGetTime`, `Now`, `Time` come from `IClock`, fixed in the harness.
  The AI's `RequestProcessTime` budgets are checked against this clock in the survey of the AI
  code before the first run. A budget that cuts AI work by wall time makes the original
  nondeterministic too, and the recovery's equal runs say that it does not happen.
- Object addresses: `InstanceId` replaces them (`transpiler.md` N7).
- Directory enumeration: sorted (`rtl-shim.md` 3.5).
- Hash iteration: the engine's hash lists are transpiled code with deterministic order.
- Dialogs: the scripted service answers deterministically.
- Floating point: `double` everywhere, deterministic on one platform. Cross-platform differences
  in `Math.Sin` and `Math.Pow` are possible and accepted.

### 3.4 Done measures

- O1: the port's headless run is self-deterministic on Mountain Clash for 20 turns.
- O2: the port equals the Delphi build on Mountain Clash for 20 turns, with the masks listed.
- O3: the same on all seven scenarios, and Mountain Clash for 50 turns.

## 4. Frame comparison

For the graphics layer (`graphics.md`). The Delphi build renders a frame into its back surface,
and the port renders the same scene into its framebuffer. The reference frames come from the Delphi
build through a debugger script that dumps the back surface after `DisplayUpdateFrame` for a set
of scripted scenes (the title screen, a map view at fixed coordinates in a loaded save, a dialog,
the combat map), saved as raw 16-bit images with their size. The port's headless host renders the
same scenes with the real graphics layer and a null presenter, and `tools/oracle/frames.py`
compares pixel for pixel and writes a difference image. Done means zero differing pixels per scene.
The scene list grows with the port.

## 5. Fixtures from the oracle compiler

Beyond the probes, some tests need data only the Delphi build can produce: the expected inflated
bytes of a save written by the original after a load (for the map round trip), the reference
frames of section 4, and `initialization-order.json`. These live under `tests/fixtures/` with a
`README.md` that names the tool and the command that produced each file. Nothing under
`tests/fixtures/` is game data: a save of a shipped scenario is derived data and is checked with
the user before it is committed, because the repository ships no game files.
