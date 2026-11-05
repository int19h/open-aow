# Testing specification

State: draft, 2026-10-24. The verification ladder is in `../process.md` section 6. This document
specifies the rungs that need tooling: the probes, the round trip, the headless oracle and the
frame comparison, and how they fit together in one command. The tooling under `tools/` and
`tests/` lives in the private repository (`../design.md` section 8). The xUnit projects under
`src/` live with the product. Section 6, the strings checks of milestone 9.5, was added on
2026-11-05 (brief 9.5-2).

## 1. Layers

| layer | proves | tool | where specified |
|---|---|---|---|
| unit tests | one C# member behaves as documented | xUnit in `src/*.Tests` | `rtl-shim.md` section 8 |
| probes | the transpiler and the shim together reproduce Delphi semantics | `tests/probes/run.py` | `transpiler.md` section 11, this document section 2 |
| round trip | the persistence path is byte exact | `tests/roundtrip/` | `persistence.md` section 4 |
| headless oracle | the rules play the same game as the Delphi build | `tools/oracle/` | section 3 |
| frame comparison | the renderer draws the same pixels | `tools/oracle/frames.py` | section 4 |
| strings checks | text stays one code page 1252 char per byte, no game string is null, and no swallowed error goes unseen | `StringsRewrite --check`, the strict switch, the exception census, the sort dump | section 6 |
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
2. Transpiles the probe with the transpiler (`transpiler.md` section 1) into
   `tests/probes/generated/<name>/`,
   builds it against `src/Rtl` with `dotnet build`, runs it and captures the output.
3. Compares the two outputs line by line and prints the first differing line with both texts.
   A difference the specification accepts is listed in `tests/probes/accepted/<name>.txt`, one
   label or label pattern per line with the section that accepts it, and the runner then prints
   `match (N accepted)`. Two exist: the raw byte of a non-canonical Boolean (`integers`,
   `transpiler.md` section 7 trap 9) and the order of the `AnsiCompareText` pairs that
   `rtl-shim.md` section 3.2 accepts, read from the shim's pinned list so that both checks use one
   set (sign only, equality still compared on every pair). Any other difference fails.

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
| `pointers` | `^T` dereference, `@` on variables, fields and elements, `Inc` and `Dec` on typed pointers and `PChar`, `PChar` indexing, `GetMem` and `FreeMem`, `Move` and `FillChar`, `nil` and `Assigned`, `SizeOf(Pointer)`, a record viewed through a pointer to another type, all printed as address differences and contents, never addresses |
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
transpiler's layout model (the layout tests in `tools/transpile/Transpiler.Tests/`), and the
transpiler must pass it before it emits any explicit layout.

## 3. Headless oracle

The headless host has a map viewport, because the engine reads it while saving (`persistence.md`
section 4, M1b). It is display state, as in the original: 0x0 until the game first shows the map
view, then the configured size, 1600x940 by default, the size of the oracle runs' playtest window.
The two states explain the stored view center of all 417 oracle autosaves (the day-01 save is
written during initialization, before the map view has a size).

Run settings of the native harness and the host (2026-10-26). Native runs play with the game's
sound and music volume at 0 and the enable flags unchanged, so that no sound code path changes.
Both sides take `--movement-speed normal|fast|turbo` (`General\Movement Speed`, read by
`TAoWRegistry.GetMovementSpeed`). Comparisons with the reference runs use normal, as those runs
did. Turbo is safe for new traces: a native and a port run at turbo equal each other for 20 turns,
and the port at turbo equals the normal-speed reference on all days, so the speed changes the
animation steps and nothing that is saved. The host's frame rate (`FFPS`, which shortens moves
below 25) is fixed at 30, while the native readings ranged 0 to 37, and turbo moves take two
steps whatever the frame rate.

### 3.1 What the recovery already does

`tools/playtest.py` in the `aow` repository runs the original and the rebuilt game under a debugger
on a hidden desktop. It prepares a scenario as a save with every player set to computer and the
map's two seed properties (`TAoWHSMap` ids `$1C FDefendersSeed` and `$24 FInitialRandomSeed`) set to
the seed, forces `System.RandSeed` to the seed through a breakpoint that returns from
`System.Randomize` without running it, starts `AoW.exe <save>` with the registry's root and startup
directory pointed at a copy of the game, lets the game play, and collects one save per day. The
game's own `AutoSave` runs only for human players (`PlayerC.PAS:643-644`), so the harness breaks on
`TTurnPlayerControl.NewDay`, steps out of it and calls `AutoSave` itself. Day 01 is the initialized
state and day 21 follows twenty completed turns. `compare_runs` inflates each pair of saves,
normalizes them (it ignores `TAoWHSMap.FTurnTimeElapsed`, id `$37`, and `FTurnTimeBeep`, id `$3A`,
the two fields fed by the turn timer's `timeGetTime` reads), compares the bytes, dumps both with
`asg_dump.py` and writes the first difference as a unified diff and as a list of changed fields.
The rebuilt game equals the original on all seven scenarios for 20 turns, and on Mountain Clash
for 50.

The reference data is kept under `D:\Build\oracle\` (33 MB, copied from the worker build
directories on 2026-10-25, nothing rerun), with a `README.md` that documents every run and the
command that made it:

| path | content |
|---|---|
| `inputs\<scenario>.asg` | the seven prepared seeded saves: seed 123456789, play mode turn based, all players CPU. `small`, `large`, `underground` and `large_igor` are what `playtest.py prepare` makes with CPU type 1; `heroes`, `magic` and `water` were made with `seed-map --cpu 3`, and today's `prepare` does not reproduce them |
| `original\<scenario>\<label>\`, `rebuilt\<scenario>\<label>\` | one directory per run: `run.json` (input and module hashes, flags, saves), `day-NN.asg`, `cdb.log`, the two cdb scripts, `modules.json` |
| `cdb\<variant>\<label>\` | `startup.cdb` and `run-commands.cdb` of each run |
| `comparisons\compare-<left>-<right>\comparison.json` | 16 results of `playtest.py compare`, all equal |

| scenario | map | size, levels | CPU players | turns |
|---|---|---|---|---:|
| `small` | Mountain Clash | 48 by 48, 1 | 2 | 50 |
| `large` | After the Fall | 127 by 127, 2 | 12 | 20 |
| `underground` | Mine All Mine | 48 by 48, 3 | 3 | 20 |
| `large_igor` | Igor's Folly | 96 by 96, 2 | 2 | 20 |
| `heroes` | Love thy Neighbor | 96 by 96, 2 | 4 | 20 |
| `magic` | Rocky Hollow | 96 by 96, 2 | 5 | 20 |
| `water` | Blood Isles | 64 by 64, 1 | 4 | 20 |

The original's runs are the port's reference, not the rebuilt ones. Gaps the README records: no
rebuilt run for plain `magic`, the plain `heroes` rebuilt run predates the transporter fix, no
20-turn `small` run in the final build, and no run reached the end of a game. Two run families
used debugger-forced variants: `--tactical --tactical-battles 3 --exercise-items` on `heroes`, and
`--global-spell 46` on `magic`. The port's harness reproduces `--tactical` (section 3.3); the
other three flags are read from `playtest.py` when the harness is written, and until then those
runs are compared only from day 01 to the first day a flag acts.

### 3.2 What the port adds

The port is driven by calls, not by a debugger. `tools/oracle/run.py` does, for one scenario:

1. Take the prepared seeded save `D:\Build\oracle\inputs\<scenario>.asg`, or prepare one the same
   way (`seed_map` is copied into `tools/oracle/asg.py` with the table reader it needs, so the
   port's tools do not import from the `aow` repository at run time; the copy keeps the `--cpu 3`
   variant that made `heroes`, `magic` and `water`).
2. Take the reference: the original's saves for that scenario under
   `D:\Build\oracle\original\<scenario>\<label>\day-NN.asg`. The port reruns neither the original
   nor the Delphi build. The reference saves are inputs.
3. Run the port headless: `dotnet run --project src/Headless -- --load <save> --seed 123456789
   --turns 20 --out <dir>`. The headless host implements `IRtlHost` with a null display, a null
   sound system, a scripted dialog service that answers every dialog with its default button and
   logs it, and a fixed clock. It drives the game the way the original's frame loop does, with the
   display left out. The original's path, verified by the recovery in the source on 2026-10-25:
   - `AoW.exe` reads only `ParamStr(1)`. A `.asg` or `.acg` path shows the title scene and is
     loaded on the first frame (`AoWUnit.pas:403-417`) from `TMainForm.DisplayUpdateFrame` through
     `OpenSelectedGame`, `TFileDlgWindow.LoadSelectedGame`, `LoadGame`, `StartGame` and
     `TAoWHSMap.Activate`. The frame loop is synchronous: the main form sets `UpdateFrameMode =
     umSynchrone` and `FrameRate` 30, so `TCustomDisplay.StartUpdateFrameLoop` calls `UpdateFrame`
     and `Application.ProcessMessages` in the main thread. No thread or timer is involved;
     `TUpdateFrameThread` serves only the editor's asynchronous mode.
   - `TAoWHSMap.Activate` with `FDay = 0` runs `StartNewMap`, `TTurnPlayerControl.StartNewMap`,
     `NewDay` (day 1, which calls `Randomize` and stores `RandSeed` in `FInitialRandomSeed` for
     local games), `NewTurn` of the first player and `FAIExecuter.ActivatePlayer` for a CPU player.
   - Every frame, `TAoWHSMap.NewFrame` calls `FPlayerControl.NewFrame`,
     `FPlayerCommunicator.Process`, `FAIExecuter.Process` and `FTokenManager.Process`, in that
     order. `TAIExecuter.Process` runs `TAIPlayerControl.Process` until the counter budget is used,
     and `TEndTurnAIPA.Process` queues a `TEndTurnTE`. `TTokenManager.Process` executes it:
     `TTurnPlayerControl.EndTurn`, and after the last player `NewDay`, then `NewTurn` of the next
     player and `ActivatePlayer`.
   The harness loads the save through the game's load path, sets `RandSeed` to the seed and makes
   `Randomize` a no-op (the same two interventions as the debugger), calls `Activate`, then calls
   `TAoWHSMap.NewFrame` in a loop. It hooks `TTurnPlayerControl.NewDay`: when the day changes it
   calls `AutoSave` into `<out>/day-<n>.asg`, and it repeats until the requested turns or until
   `TAoWHSMap.TriggerGameTerminatedEvent` fires. Dialog and display calls reached from this path
   go to the null services.
4. Compare with `tools/oracle/compare.py`, the port's own comparator, started as a copy of
   `compare_runs` and `normalized_save`. It never compares compressed bytes. Its normalizations,
   each listed in its output when applied:
   - It inflates both payloads (`persistence.md` 1.1) and compares the five header bytes and the
     inflated bytes.
   - It masks the fields the user accepts as floating point divergence once such fields are known.
   - It takes a list of properties whose children are written in hash table order and compares
     those children as sets. The list is empty in the first stage, where the hash table code is
     transpiled (`../design.md` section 5 says how it grows in the second).
   - In model mode, which the second stage adds (`../milestones.md` 9.1), it reads both files
     through the port's readers into the property tree and compares trees, so that the port's own
     formats compare against the original's saves.

The result is one JSON per scenario with the first differing day, the changed fields and the diff,
in the format `compare_runs` writes, so that the same reading habits apply.

### 3.3 Determinism requirements on the port

The headless run must be deterministic by itself before it is compared: two runs of the port with
the same input give identical saves. What the recovery verified about the original (reply of
2026-10-25) and what the port does with it:

- Random numbers. The game logic draws from `TAoWHSMap.Random`, which loads `RandSeed` from
  `FDefendersSeed` before every call and stores it back after (`AoWHSMap.PAS:2703-2711`), and
  `ReadWrite` ends with `RandSeed := FDefendersSeed`. Drawing code sets `RandSeed` from frame
  counters and positions (`GlobalEnchantmentSpells.pas:707, 1597`, `Animation.pas:547`, restored at
  571) without reaching the logic, and a clock-seeded `System.Random` exists only in e-mail games
  (`Diplomacy.pas:2547, 2569`). The shim's `Random` is exact (`rtl-shim.md`), and the harness fixes
  the seed as above.
- The clock. There is no `GetTickCount` in the source. `timeGetTime` is read only in
  `GetElapsedMilliSeconds` (`GFXE.pas:12857`), in editor profiling and in the DirectPlay ping. The
  turn timer (`AoWHSMap.PAS:3093-3120`) feeds it into the two ignored fields, and `DisableTurnTimer`
  subtracts the start time even when it is -1, so those fields are garbage by design. `IClock` in
  the harness returns a fixed value, and the comparison still masks `$37` and `$3A`.
- The AI budget is a counter, not wall clock. `TAIExecuter.Create` sets `ptmStatic` and
  `FProcessTime` 5000 (`AI.pas:564-565`), nothing else assigns the mode, and `RequestProcessTime`
  and `GetProcessTimeLeft` compare against `FProcessTimeUsed`, which only `UseProcessTime` raises by
  fixed costs. The wall-clock branch (`AI.pas:598, 962`) is dead code and stays so in the port.
- Combat. With no human in a battle, `SetupCombat` falls back to `ctFast` (`MoveT.PAS:2692,
  2710-2711`), so all-CPU runs exercise fast combat only. The recovery's `--tactical` runs forced
  `TArmyCombatMoveTE.FCombatType` to `ctTactical` when `ExecuteCombat` starts and set
  `TTacticalCombat.FHostPlayer` to 0 in `Execute`, because without a human it stays -1
  (`AoWTC.pas:5103-5107`). The harness has the same option (`--tactical`), applied at the same two
  points, so the tactical resolver is covered by the `heroes` tactical runs.
- Object addresses: `InstanceId` replaces them (`transpiler.md` N7). Saves hold no paths or
  addresses: the original gave equal saves from runs in different directories.
- Uninitialized reads. The recovered source marks six (`AoWReg.pas:268, 295`, `Ability.PAS:352`,
  `AoWHSMap.PAS:2379`, `Army.PAS:1443`, `GlobalSpells.pas:269`). None produced a difference between
  two runs of the original. The port reads zero there (C# default initialization), a defined value
  the original did not promise. If the oracle first diverges at one of these, the original's value
  is read with the debugger and the port matches it.
- Directory enumeration: sorted (`rtl-shim.md` 3.5).
- Hash iteration: the engine's hash lists are transpiled code with deterministic order.
- Dialogs: the scripted service answers deterministically.
- Floating point: `double` everywhere, deterministic on one platform. Cross-platform differences
  in `Math.Sin` and `Math.Pow` are possible and accepted.

Functions the oracle cannot vouch for. The recovery's equivalence review of the functions that do
not byte-match leaves six `UNSURE`: `TChatWindow.ChatEditReturn` in `AoW.exe`, and in AoWEPACK
`TAIGroupTargetSelector.Process`, `TAoWEngine.Create`, `TDiplomacyAIPA.ExecuteDiplomacy`,
`ShowMovePaths` and `TTransportMEC.CalculateMoves`. The seven scenarios ran equal through them, so
they are right on those paths, but a divergence of the port that traces into one of them is settled
against the original binary, not the rebuilt one. Everything else either byte-matches (97 percent
of functions) or was reviewed equivalent, and the 22 functions once marked different carry their
fixes in the current source.

### 3.4 Done measures

- O1: the port's headless run is self-deterministic on Mountain Clash for 20 turns.
- O2: the port equals the Delphi build on Mountain Clash for 20 turns, with the masks listed.
- O3: the same on all seven scenarios, and Mountain Clash for 50 turns.
- O4: the port equals the original on the `heroes` tactical run, with the harness reproducing the
  run's flags.

## 4. Frame comparison

For the graphics layer. `graphics.md` section 9 specifies it: the reference frames come from the
rebuilt game in windowed mode, which renders in `pfRGB555`, captured by a debugger breakpoint on
`TWindowsDC.BltSurface` for a list of scripted fixtures (`tests/frames/fixtures.toml`: a save or
scene, a frame number, the registry settings, the mouse position, the fixed `RandSeed`); the port
renders the same fixtures through its controller with a presenter that writes the frame to a file;
`tools/oracle/frames.py` captures and `tools/oracle/compare_frames.py` compares pixel for pixel,
with per-fixture masks and a difference image. Done means zero differing pixels per fixture outside
the masks. The blitter and blend table probes of `graphics.md` run first, so that a frame
difference points at layout or logic rather than at a blitter.

## 5. Fixtures from the oracle compiler

Beyond the probes, some tests need data only the Delphi build can produce: the expected inflated
bytes of a save written by the original after a load (for the map round trip), the reference
frames of section 4, and `initialization-order.json`. These live under `tests/fixtures/` with a
`README.md` that names the tool and the command that produced each file. Nothing under
`tests/fixtures/` is game data: a save of a shipped scenario is derived data and is checked with
the user before it is committed, because the repository ships no game files.

## 6. The strings checks

Milestone 9.5 (`strings.md` sections 2.2, 3.4 and 4) added these checks to the gate, and they stay
after the pass. `python tools/check.py` in the private repository runs them.

| check | what it proves | where |
|---|---|---|
| strings check | no `AnsiString` (the shim type 9.5-1 retired) in the tree, no explicit conversion from `char` to `byte` or to `char` from a non-constant integer or a constant of 128 and above, no `==` or relational operator between a `char` and a `byte`, no sort, search or sorted collection over strings without a comparer, no `Cp1252` call in a `ReadWrite` override, and in `src/Units`, `src/Layout` and the text routines of `src/Rtl` no .NET comparison, case, character class, trim, split, search or encoding call that differs from Delphi (`strings.md` 3.4) | `StringsRewrite --check <product>/src --allow tools/rewrite/strings/allow.txt`, a step of the `product` layer after the build. It prints `Strings check: N findings`, and the layer fails unless N is 0. The allowlist names 31 members by documentation comment id, each group with its reason |
| strict strings | a null game string throws `NullStringException` in every routine that reads the switch, instead of reading as empty (`rtl-shim.md` 1.2) | `check.py` sets `AOW_STRICT_STRINGS=1` for every command it starts. Game, Headless, HostChecks and Localization.Tests read it at entry through `GateSwitches.Enter`. `Rtl.Tests` and `Units.Tests` turn the switch on in a module initializer |
| exception census | a null or range error swallowed by a broad catch, `Rtl.Wrap` or `AoWE.Format` still moves a number | `ExceptionCensus` in `src/Rtl`. When `AOW_EXCEPTION_CENSUS` names a file, a host counts first-chance exceptions of six kinds, `NullReferenceException`, `IndexOutOfRangeException`, `ERangeError`, `EAccessViolation`, `EConvertError` and `NullStringException`, each under the first kind among its type and its bases, and appends one JSON line per run with the host, its arguments and the counts. Game, Headless, HostChecks, Localization.Tests and RoundTrip.Tests start it. `check.py` points the variable at one file per layer and copies the files to `census/` in the log folder |
| sort dump | the order of the Units comparisons and of a sorted `TStringList` over the English and German names of units, items, abilities, spells and texts (`CompareItemNames`, `CompareAbilityNames`, `P4CompareSpellNames`, `P4CompareSpellLevels`, `CompareTextControlItems`) | HostChecks `--sort-dump <file>`, one child process per language, in the `product` layer. The gate copies the dump to `sorts.txt` in the log folder, and two dumps compare with a plain file comparison |
| glyph index | for every byte `b` of every installed font, `TextWidth` and the glyph `ShowText` draws for the one-char string of `b` equal glyph `b`, so bytes 128 to 159 reach the font as their letters | `GlyphIndexTests` in `Units.Tests`, skipped with the path when the installed fonts are absent |
| typed-name round trip | a leader name typed through `InputMapper.Text` with €, „, ä and ß keeps the bytes 0x80, 0x84, 0xE4 and 0xDF through save and load | byte mode: `TypedNameRoundTripTests` in `Units.Tests`. Model mode: `tools/oracle/typed_name.py` in the private repository, where the Headless host saves a game with that leader name and the oracle's independent model reader finds the bytes |
| dictionary order | the sorted entries of each of the eight dictionaries are in `Cp1252.Compare` order | `tests/localization` in the `localization` layer |
| host paths, static tables | names outside the table are found, opened, cased and told apart (`HostPathTests`), and no static string table element is null after unit initialization (`StaticStringTableTests`) | `Rtl.Tests`, `Units.Tests` |
| culture guard | text comparisons and number formats follow the invariant culture only | Game and Headless stop at start when `System.Globalization.Invariant` is off (`GateSwitches.CultureError`), and `GateSwitchesTests` asserts the switch in the test process |

Base recordings. Brief 9.5-P5 recorded two gate runs on the base of the pass (`b6a1c3d`) in the
private repository: `data/rewrite/9.5-exceptions-base.json` holds the census of 143 runs, every
count 0, and `data/rewrite/9.5-sorts-base.txt` the sort dump, 2,994 lines. The gate does not compare
with them itself. After the 9.5-1 pass the gate's census holds every count at 0, and its
`sorts.txt` equals the base dump byte for byte (`strings.md` section 8).
