# Persistence specification

State: draft, 2026-10-24. Covers the engine's object persistence (property tables, storage
streams, the compressed container), what the port must preserve, how the transpiled persistence
code meets the shim, and the round trip milestone that proves it. The format description comes
from `D:\git\aow\doc\persistence.md` and from `src/delphi3/Engine.pas` as read on 2026-10-24.

The persistence code itself is transpiled, not rewritten: `TPropertyTable`, `TEStorageStream`,
`TEReadStorageStream`, `TEWriteStorageStream`, `TCompareStorageStream`,
`TTranslateStorageStream` and `TCompressStream` are classes of the Engine unit, and every game
class's `ReadWrite` is in the rules packages. This document exists so that the shim provides what
that code needs, so that test tooling can read the files, and so that the first milestone has a
precise measure.

## 1. The format

### 1.1 Containers

| file | container |
|---|---|
| `.asg` save, `.hsm` map | `TCompressStream`: 4 bytes `CFS\0`, 1 byte level (`TCompressLevel`: 0 none, 1 fastest, 2 default, 3 max, v1.36 writes 2), then the payload. Level 0 means the payload follows uncompressed. Otherwise the payload is a zlib stream (`78 9C` for level 2). A file without the magic is read as uncompressed from byte 0 |
| `.hss` scenario set, `.cam` campaign | an uncompressed storage stream, no container |
| `.ahl`, `.ail` user lists (heroes, items) | storage streams written by the rules code (`Hero.PAS`, `Army.PAS`) |

The game inflates with zlib 1.0.4 linked into Enginep.dpl and deflates with level 2 (zlib default).
The port uses `System.IO.Compression.ZLibStream` for both directions. The inflated bytes are what
the game reads, so the oracle comparison of saves is a comparison of inflated payloads plus the
five header bytes, never of compressed bytes (`testing.md`). Any zlib encoder produces a stream the
original game inflates, so saves written by the first stage load in the original game. That is a
fact, not a requirement: compatibility is one way (`../design.md` section 5), the port must read
the original's files and is free to write its own formats in the second stage.

### 1.2 Inflated payload of a map or save

A 16-byte header record, then the root object:

```
Integer 16          size of this header
'HSM' #0            THSMHeader.ID
Integer 0           reserved
Integer ClassID     class id of the root object (TAoWHSMap is 131089)
```

`THSEngine.LoadHSMfromStream` seeks to `Header.Size`, creates the root from the class id through
`CreateEObject`, and calls `TEngine.ReadEObject`, which creates a `TEReadStorageStream` on the
stream and calls `rwEObject(root, 0)`.

### 1.3 Property table

Every object is a property table followed by its data. The table (`TPropertyTable.LoadFromStream`):

1. One count byte. If bit 7 is set, the low 7 bits are the number of short entries and an
   `Integer` number of long entries follows. Otherwise the byte is the number of short entries and
   there are no long entries.
2. Short entries, 2 bytes each: `id` in the low byte, `offset` in the high byte. An entry is short
   when `id <= $7F` and `offset <= $FF`.
3. Long entries, 8 bytes each: `Integer id`, `Integer offset`.

Offsets are relative to the data base, the stream position right after the table. The writer
(`SaveToStream`) emits short entries first and then long entries, both in the order the properties
were added, which is the order of the `rw*` calls in `ReadWrite`. The reader finds an id with
`FindOffset`, which first tries the entry after the last one found (`FNextIndex`) and then scans
from `FFirst`. A missing id gives the default. `PropertyExist` scans up to `FMaxID`.

Nested objects save and restore the table state with `BackupSettings` and `RestoreSettings`, a
stack of (`First`, `NextIndex`, `MaxID`), so one `TPropertyTable` instance serves the whole tree.

### 1.4 Values

| call | written bytes | written when | read default |
|---|---|---|---|
| `rwInteger(I, id)` | 4 | `I <> 0` | 0 |
| `rwIntegerEx(I, Default, id)` | 4 | `I <> Default` | `Default` |
| `rwByte`, `rwShortint`, `rwWord` and their `Ex` forms | 1, 1, 2 | value differs from 0 or from `Default` | 0 or `Default` |
| `rwBool(B, id)` | none, the entry's presence is the value | `B` | `False` |
| `rwString(S, id)` | length prefix then bytes: one byte length, or `$FF` then `Integer` length when the length is `$FF` or more | `S <> ''` | `''` |
| `rwStrings(L, id)` | `StringsToStream`: an `Integer` count then each string with the prefix above | `Count <> 0` | `Clear` |
| `rwData(Buf, Size, id)` | `Size` raw bytes | `Size <> 0` | the buffer is left as it is |
| `rwStreamProc(id, Load, Save)` | whatever `Save(stream)` writes | always | `Load(stream)` is called inside a `try` that swallows every exception |
| `rwEObject(Obj, id)` | a nested property table, then the nested data | always | `Obj.MainReadWrite` runs with an empty table when the id is missing |
| `rwCreateEObject(Obj, Owner, id)` | `Integer` class id (0 for `nil`), then a nested table and data | always | `Obj := nil` when the id is missing or the class id is 0. Otherwise `Engine.CreateEObject(classId, nil)`, owner set, `MainReadWrite`, then `SetOwner(Owner)` |
| `rwChildren(Node, id)` | a nested table whose entries are the children: entry id is the child's index in `Node.Children`, value as `rwCreateEObject` | always | each entry creates a child by `rwCreateEObject(child, Node, entryId)`, and with `csKeepIndex` in the node's control style the child is placed at its index by `SetArrayOwner` |
| `rwEObjectStream(Obj, id)` | `Integer` length, then a table and data | always | skip by the length on error |
| `OpenPropertyStream(id)` | the caller writes raw bytes | always | the caller reads from the property offset |

A property id written twice in one object raises `EDoublePrioriyID`. The writer collects an
object's data in a `TEBufferStream` and writes the table and the buffer to the outer stream when the
object ends, so nested tables appear before their data in the file.

`TECustomNode.MainReadWrite` calls `Clear` first in read mode, then `ReadWrite`, then
`ReadWriteChildren`, which is `rwChildren(Self, 1)`: every node keeps its stored children under
id 1. A child is stored when its `Owner` is the node and its control style has `csStored` and not
`csShared`.

### 1.5 Class ids and the registry

Every `TEObject` class has a virtual class method `ClassID` that returns a constant. `TEngine`
keeps a `TEObjectClassList` and a hash table from class id to class, filled by `RegisterEClasses`
from the units' initialization code, and `CreateEObject(id, owner)` looks the class up and calls
its virtual constructor `Create(owner)`. A double class id raises at registration. The port keeps
all of this as transpiled code: `ClassID` is a virtual method of the metaclass, the registry holds
`TEObject.Meta` objects, and `CreateEObject` is `meta.Create(owner)` (`transpiler.md` 4.4). The
class ids are the values in the file, so the port's class ids are the original constants, and
`tools/asg_dump.py` in the `aow` repository already maps ids to names.

### 1.6 Save files

`TAoWHSMap.SaveGame` writes the map object as above. The recovery's round trip (`doc/persistence.md`,
section "Round trip") found two details that the port inherits because it transpiles the same
code: embedded objects flagged shared are kept by `Clear` and skipped by `rwChildren`, and a child
remembers the slot it was read from because owners have unstored children before the stored ones.
Shipped `.hsm` maps carry properties the 1.36 code reads but never writes, so they round trip only
after one load and save.

## 2. What the port must preserve

In the first stage, while the persistence code is transpiled and the round trip is the measure:

1. The byte layout of every value above. The storage stream code is transpiled, so this holds if
   the shim's `TStream.Read(ref T)` and `Write(in T)` write the Delphi layout of `T`
   (`transpiler.md` 4.3). The types used are `Integer`, `Byte`, `Shortint`, `Word`, `Boolean`,
   `Char` arrays and the `rwData` records.
2. The order of `rw*` calls in every `ReadWrite`, which the transpiler keeps.
3. The children order of every node, which comes from the game logic, not from persistence.
4. The default rules: a zero value is not written. A port that writes zeros still loads in the
   original, but it fails the oracle comparison.
5. `rwData` of a record writes the record's Delphi layout, including padding bytes. The transpiler
   gives records an explicit layout. For `rwData(FState, 1, 20)` style calls on single fields the
   size is given and no layout question arises.
6. `rwData(FAbilityBits^, (FAbilityCount + 7) shr 3, 3)`: raw bit arrays through a pointer, which
   is the `Ptr<byte>` overload.
7. Strings are bytes, written as they are (`SaveStringToStream` writes `PChar(S)^`).

## 3. Shim requirements

From the persistence code specifically:

- `TStream.Read(ref T, int)` and `Write(in T, int)` for unmanaged `T`, `Read(Ptr<byte>, int)`,
  `Read(Span<byte>)`; `Position`, `Size`, `Seek`, `CopyFrom`.
- `TMemoryStream` with `Memory` exposed, and `TFileStream`.
- `TCompressionStream(level, dest)` and `TDecompressionStream(source)` with the `zlib.pas`
  interface (`Read`, `Write`, `CopyFrom`, `OnProgress`), backed by `ZLibStream`. The compression
  levels map 1 to `Fastest`, 2 to `Optimal`, 3 to `SmallestSize`. These two classes replace the
  `zlib` unit, which is a C binding (`../design.md` section 3).
- `TStrings` with `LoadFromStream` and `SaveToStream` are not used here. `StringsToStream` and
  `StringsFromStream` are Engine routines, transpiled.
- `Exception.Message` as a `string`, because `HandleException` passes it to an event.

## 4. Milestones

### M1a: round trip on the reduced model

The `aow` repository has a reduced persistence engine, `src/recovered/Engine.pas` (1,711 lines)
with `EngineLists.pas`, hand-recovered and frozen since 2026-10-19, and a generated object model
`src/model/AoWModel.pas` (489 classes with `ReadWrite` methods replayed from the decompiled code,
10,161 lines), generated once by `tools/translate_readwrite.py` on 2026-10-19 and regenerated by no
build or test. The hand-written overrides are `src/model/hand/<Class>.decl.inc` and
`<Class>.impl.inc` (the header's mention of `AoWModelOverrides.inc` is stale). `tests/roundtrip.pas`
loads a save through them and writes it back byte-identically. These are Free Pascal units in
Delphi mode, written by hand or generated, with no byte-matching idioms. They are the smallest
input that exercises the whole persistence path.

Scope of the TODOs. `AoWModel.pas.report.txt` marks 71 classes with TODOs. Nine have hand
overrides (TAbilityOwner, TCustomAbilityList, TItemControl, TMapContainer, TMapField, TMapLevel,
TMapObject, TPlayer, TRace) and 62 do not, at least 22 of which occur in the shipped files
(TAoWHSMap, TArmy, TCity, THero, TItem, TUnit, TLeader and TCombatUnit among them). The TODOs lose
in-memory behavior (exception frames, getter side effects such as TUnit's unit resource lookup,
flag masks), not bytes, which is why the round trip still holds and why M1a is a persistence test
only: nothing built on the reduced model is a rules test.

M1a transpiles these three units and a C# port of `roundtrip.pas`, and the inflated output equals
the inflated input for both shipped saves (`After the Fall.asg`, `autosave.asg`) and, after one
load and save, for all 23 shipped maps. This is the first end-to-end test of the transpiler, the
shim's streams and strings, the metaclass registry and the layout model, on about 12,000 lines.

Caveat: the reduced engine's `TEObject` differs from the real one (`CreateEObject` takes a
`TEObject` owner, the storage mode is an integer), so the units are a separate transpile input,
under `tests/roundtrip-model/`, not part of the game's transpile set.

### M1b: round trip on the real engine

The same test with the real `Engine.pas` and the rules packages, which is the full transpile set
minus the UI. The reference changes with the engine. The reduced model writes back what it read,
but the real engine runs game code while it saves: `TPlayer` writes the view center that
`GetViewCenter` computes from the stored pixel origin and the live viewport size, which no save
holds (`Player.PAS:1051`, `HSMAP.PAS:579`), and the load path recomputes morale and budgets,
extends diplomatic history and rounds the bit lists' capacity to 32 bits, as the original's own
decompile does. A file the original wrote before any load (a shipped save or map) therefore does
not round trip in the original either. Found by brief 2.2 on 2026-10-26.

The measure is therefore the original engine's own output after a load: the oracle's autosaves
(`D:\Build\oracle\original\<scenario>\<label>\day-NN.asg`, `testing.md` section 3), which the
original game wrote from a loaded game. Each one, loaded and saved by the port with the headless
host's viewport set to the size the oracle runs had, equals itself after inflation, ignoring only
`TAoWHSMap` ids `$37` and `$3A` (the wall-clock turn time, the same ids `playtest.py compare`
ignores). The shipped saves and maps are loaded without reaching a TODO, and their differences
are listed by cause. The tactical maps under `TCMaps` load only inside a tactical combat and
belong to the tactical oracle run.

Result of brief 2.2 (2026-10-26). All 444 world files load and save through the generated engine
without reaching a TODO. 32 of the 417 oracle autosaves compare equal under the oracle profile.
The others differ because the native load itself changes the game, which the installed
`Enginep.dpl` confirms when a Delphi harness calls its own routines: a bit list shorter than 32
bits is rounded up to 32 and its enlarged read takes bytes past the property, an absent sprite
direction becomes `03`, hero groups are reset and empty groups destroyed. The original game does
not reproduce its own autosaves either. A reference transform for the bit lists was built and
proven on 7,200 native fixtures, but a transcription of every load-time change rebuilds the load
logic beside the port, which is the wrong check. Byte equality therefore moves to the oracle
comparison (`testing.md` section 3, milestone 2.4): the port loads the same seeded inputs the
original runs loaded and its autosaves are compared with theirs, so the original's own load is
on both sides. M1b is closed as the load and save path: every file loads and saves, and the
round-trip tool stays as the diagnostic it was in this brief.

### Validation tool

`tools/asg_dump.py` in the `aow` repository walks a save with the recovered schema and reports
unexplained property ids. The port adds a C# counterpart only if a difference in M1a or M1b needs
one. Until then the Python tool is the reference reader.

## 5. The second stage

`../design.md` section 9. Once the readers for `.asg`, `.hsm`, `.hss`, `.cam`, `.ahl` and `.ail`
exist and the comparator compares at model level (`testing.md` 3.2), the port's own save and map
formats can replace the property tables: the original formats stay as converting readers, the
shipped maps and campaigns load through them, and nothing requires that the original loads a file
the port wrote. The explicit-layout records of `transpiler.md` 4.3 exist for `rwData` in the first
stage and become plain `record struct` types with explicit serialization when the format moves.

Milestone 9.6 (`unions-and-kinds.md` section 4) changed the model of two persisted classes and no
byte of a file:

- The engine's message slots (`MsgArg`, `rtl-shim.md` 1.5) and the arguments of `Format`
  (`FormatArg`, 1.6) are not persisted. No message record reaches a stream or a packet.
- `TProductionItem` holds a `ProductionOrder` in place of its production id, parameter and group id,
  and `TDiplomaticAction` holds a `DiplomaticProposal` in place of its action type, gold, mana and
  message. Each `ReadWrite` writes the same bytes as before, with the same calls, property ids and
  order (20 to 22 for the production item, 20 to 31 for the diplomatic action, the action type as
  the one-byte `rwData` at 25) and the zero rule of 1.4: it takes the stored values from the record
  (`ToFields`, and the proposal's message) before the calls and, when reading, builds the record
  with the registry's `FromFields` after them. Saves, maps, `TProductionTE` and
  `TDiplomaticActionTE` reach both classes through the same `ReadWrite`.
- An id that no kind registered reads as `UnknownOrder` or `UnknownProposal`, which writes the
  stored values back unchanged.
- A registered kind holds only its own fields and drops a value that a file holds in a field the
  kind does not use, so that value writes back as 0 and the zero rule omits it (decision X20 of
  `data/rewrite/roadmap.md`). The 9.6-0 census (`data/rewrite/9.6-census.md`) found no such value
  in the 401 files it read. Two values outside the kinds' own fields stay in the common part:
  `FOtherPlayer`, which the census finds written for every kind (255 for the kinds that name no
  other player), stays a field of `TDiplomaticAction`, and the message is a member of the
  `DiplomaticProposal` base.
- `TShowInfo` keeps its 8-byte ILB image (`unions-and-kinds.md` 3.3). ILB files are game data,
  outside the save and map formats.
- The converter's mappings for `TProductionItem` and `TDiplomaticAction` (`own-formats.md` 3.1),
  which call `ToFields` and the registries' `FromFields`, arrive with brief 9.7-3, which adds them.
  The 9.6 briefs ran before it merged and left the converter's schema-driven reading of these
  properties as it was.

## 6. Open points

1. Whether the AoWModel units can be transpiled as they are or need a small adapter for the
   reduced engine's API differences. Decided when M1a starts.
2. Multiplayer desync checks (`TCompareStorageStream`) and localization
   (`TTranslateStorageStream`) are transpiled but have no test until networking and localization
   are in scope.
