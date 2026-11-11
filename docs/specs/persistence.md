# Persistence specification

State: draft, 2026-10-24. Covers the engine's object persistence (property tables, storage
streams, the compressed container), what the port must preserve, how the transpiled persistence
code meets the shim, and the round trip milestone that proves it. The format description comes
from `D:\git\aow\doc\persistence.md` and from `src/delphi3/Engine.pas` as read on 2026-10-24.
Sections 1 and 5 were revised on 2026-11-06 for milestone 9.7 (brief 9.7-4): the format is the
engine image and the format of the original files, and the disk format is the port's own
(`own-formats.md`). Section 5 was revised on 2026-11-07 for PE-1, step 1 of the exit from the
image (`persistence-tree.md`).

The persistence code itself is transpiled, not rewritten: `PropertyTable` (Delphi `TPropertyTable`), `EStorageStream` (Delphi `TEStorageStream`),
`EReadStorageStream` (Delphi `TEReadStorageStream`), `EWriteStorageStream` (Delphi `TEWriteStorageStream`), `CompareStorageStream` (Delphi `TCompareStorageStream`),
`TranslateStorageStream` (Delphi `TTranslateStorageStream`) and `CompressStream` (Delphi `TCompressStream`) are classes of the Engine unit, and every game
class's `ReadWrite` is in the rules packages. This document exists so that the shim provides what
that code needs, so that test tooling can read the files, and so that the first milestone has a
precise measure.

## 1. The format

This section describes the original format. Since milestone 9.7 it serves two purposes. It is
the format of the original files the port reads and `aowconv to-original` writes, and it is the
engine image: the bytes the transpiled stream code reads and writes inside the port. The game
writes no original file. On disk it writes the port's own JSON documents (`own-formats.md`), and
the file boundary converts between a document and the engine image on every open and save (1.7).

### 1.1 Containers

| file | container |
|---|---|
| `.asg` save, `.hsm` map, `.csm` campaign map | `CompressStream`: 4 bytes `CFS\0`, 1 byte level (`Level` (Delphi `TCompressLevel`): 0 none, 1 fastest, 2 default, 3 max, v1.36 writes 2), then the payload. Level 0 means the payload follows uncompressed. Otherwise the payload is a zlib stream (`78 9C` for level 2). A file without the magic is read as uncompressed from byte 0 |
| `.hss` scenario set | no container: a 52-byte header (its size, the identifier `#4'HSS'#0`, the set name of up to 40 bytes), the stored root, then a CRC-32 of all preceding bytes |
| `.cam` campaign, `.acg` campaign save | an uncompressed stored object with its class id, no container. A campaign save can hold a map as an embedded HSM file image in its CFS container (`Campaign` (Delphi `TAoWCampaign`) ids 5 and 6) |
| `.ahl`, `.ail` user lists (heroes, items) | storage streams written by the rules code (`Hero.PAS`, `Army.PAS`), the root without class id |

The game inflates with zlib 1.0.4 linked into Enginep.dpl and deflates with level 2 (zlib default).
The port uses `System.IO.Compression.ZLibStream` for both directions. The inflated bytes are what
the game reads, so the oracle comparison of saves is a comparison of inflated payloads plus the
five header bytes, never of compressed bytes (`testing.md`). Any zlib encoder produces a stream the
original game inflates, so an original written by `aowconv to-original` loads in the original
game. That is a fact, not a requirement: compatibility is one way (`../design.md` section 5), the
port must read the original's files and writes its own format since milestone 9.7.

### 1.2 Inflated payload of a map or save

A 16-byte header record, then the root object:

```
Integer 16          size of this header
'HSM' #0            HsmHeader.ID
Integer 0           reserved
Integer ClassID     class id of the root object (AoWHSMap is 131089)
```

`HSEngine.LoadHSMfromStream` (Delphi `THSEngine`) seeks to `Header.Size`, creates the root from the class id through
`CreateEObject`, and calls `Engine.ReadEObject` (Delphi `TEngine`), which creates a `EReadStorageStream` on the
stream and calls `rwEObject(root, 0)`.

### 1.3 Property table

Every object is a property table followed by its data. The table (`PropertyTable.LoadFromStream`):

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
stack of (`First`, `NextIndex`, `MaxID`), so one `PropertyTable` instance serves the whole tree.

### 1.4 Values

| call | written bytes | written when | read default |
|---|---|---|---|
| `rwInteger(I, id)` | 4 | `I <> 0` | 0 |
| `rwIntegerEx(I, Default, id)` | 4 | `I <> Default` | `Default` |
| `rwByte`, `rwShortint`, `rwWord` and their `Ex` forms | 1, 1, 2 | value differs from 0 or from `Default` | 0 or `Default` |
| `rwBool(B, id)` | none, the entry's presence is the value | `B` | `False` |
| `rwString(S, id)` | length prefix then bytes: one byte length, or `$FF` then `Integer` length when the length is `$FF` or more | `S <> ''` | `''` |
| `rwStrings(L, id)` | `StringsToStream`: an `Integer` byte length, then that many bytes of `L.Text` (every line ended by CR LF), cut at its first NUL | `Count <> 0` | `Clear` |
| `rwStringsEx(L, id)` | `StringsToStreamEx`: the block of `rwStrings`, then one `Integer` per line, the line's object value | `Count <> 0` | `Clear` |
| `rwData(Buf, Size, id)` | `Size` raw bytes | `Size <> 0` | the buffer is left as it is |
| `rwStreamProc(id, Load, Save)` | whatever `Save(stream)` writes | always | `Load(stream)` is called inside a `try` that swallows every exception |
| `rwEObject(Obj, id)` | a nested property table, then the nested data | always | `Obj.MainReadWrite` runs with an empty table when the id is missing |
| `rwCreateEObject(Obj, Owner, id)` | `Integer` class id (0 for `nil`), then a nested table and data | always | `Obj := nil` when the id is missing or the class id is 0. Otherwise `Engine.CreateEObject(classId, nil)`, owner set, `MainReadWrite`, then `SetOwner(Owner)` |
| `rwChildren(Node, id)` | a nested table whose entries are the children: entry id is the child's index in `Node.Children`, value as `rwCreateEObject` | always | each entry creates a child by `rwCreateEObject(child, Node, entryId)`, and with `csKeepIndex` in the node's control style the child is placed at its index by `SetArrayOwner` |
| `rwEObjectStream(Obj, id)` | `Integer` length, then a table and data | always | skip by the length on error |
| `OpenPropertyStream(id)` | the caller writes raw bytes | always | the caller reads from the property offset |

A property id written twice in one object raises `DoublePriorityIdException` (Delphi `EDoublePrioriyID`). The writer collects an
object's data in a `BufferStream` (Delphi `TEBufferStream`) and writes the table and the buffer to the outer stream when the
object ends, so nested tables appear before their data in the file.

`ECustomNode.MainReadWrite` (Delphi `TECustomNode`) calls `Clear` first in read mode, then `ReadWrite`, then
`ReadWriteChildren`, which is `rwChildren(Self, 1)`: every node keeps its stored children under
id 1. A child is stored when its `Owner` is the node and its control style has `csStored` and not
`csShared`.

### 1.5 Class ids and the registry

Every `EngineObject` (Delphi `TEObject`) class has a virtual class method `ClassID` that returns a constant. `Engine`
keeps a `EngineObjectClassList` (Delphi `TEObjectClassList`) and a hash table from class id to class, filled by `RegisterEClasses`
from the units' initialization code, and `CreateEObject(id, owner)` looks the class up and calls
its virtual constructor `Create(owner)`. A double class id raises at registration. The port keeps
the engine tables as transpiled code. Since milestone 9.4 the class side is the `EngineClass`
descriptor that `ClassRegistry.cs` registers (`metaclass-retirement.md`), each engine's
`FClassTable` maps an id to its `EngineClass`, and `CreateEObject` is `GetEClass(id).Create(owner)`.
The class ids are the values in the file, so the port's class ids are the original constants, and
`tools/asg_dump.py` in the `aow` repository already maps ids to names.

### 1.6 Save files

`AoWHSMap.SaveGame` (Delphi `TAoWHSMap`) writes the map object as above. The recovery's round trip (`doc/persistence.md`,
section "Round trip") found two details that the port inherits because it transpiles the same
code: embedded objects flagged shared are kept by `Clear` and skipped by `rwChildren`, and a child
remembers the slot it was read from because owners have unstored children before the stored ones.
Shipped `.hsm` maps carry properties the 1.36 code reads but never writes, so they round trip only
after one load and save.

### 1.7 Profiles, the G1 domains and the engine image

The converter reads a file only under a profile (`own-formats.md` 1.1), which fixes the
container, the engine class table, the root family and the header and checksum rules. An
extension alone never selects one, and a `.hsm` file names its profile through the root class id
of its HSM header.

| profile | files | engine table, root | G1 domain B | engine image |
|---|---|---|---|---|
| `world-hsm` | `.asg`, `.hsm` world maps, `.csm` | game, `AoWHSMap` | the five CFS bytes and the inflated payload, or the whole file for raw HSM. CFS level 0 and raw HSM stay distinct | the payload: HSM header and root |
| `tactical-hsm` | `.hsm` under `TCMaps` | tactical, `CombatMap` (Delphi `TAoWCombatMap`) | as `world-hsm` | the payload |
| `campaign` | `.cam`, `.acg` | campaign, `Campaign` | the whole file | the whole file |
| `scenario-set` | `.hss` | game, `AoWHSSet` (Delphi `TAoWHSSet`) | the whole file, header and CRC-32 included | the whole file |
| `hero-list` | `.ahl` | game, `HeroLibrary` (Delphi `THeroLibrary`) | the whole file | the whole file |
| `item-list` | `.ail` | game, `ItemLibrary` (Delphi `TItemLibrary`) | the whole file | the whole file |

G1 holds when an original file imported to a document and strictly exported again gives the same
B. B never covers the compressed bytes of a top-level CFS stream. A campaign's embedded map keeps
its exact compressed bytes, so the outer campaign is equal byte for byte. The game set files
(`.pfs`) and the e-mail game index have no profile and keep the original format until milestone
10.

The engine image is a separate thing from the disk format. The engine's stream code reads and
writes images exactly as section 1 describes, and `ReadWrite` stays the engine's protocol. The
file boundary (`own-formats.md` 4.1) gives the engine an image built from an own document or from
an original that passed G1, and turns the image the engine wrote into an own document. Since
milestone 9.7 the bytes of a CFS container, an HSM header, a CRC or a property table exist on disk
only in original files. A document records what it needs to rebuild them (its `$container` and
any `$layout` residue).

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
- `StringCollection` (Delphi `TStrings`) with `LoadFromStream` and `SaveToStream` are not used here. `StringsToStream` and
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
overrides (`AbilityOwner` (Delphi `TAbilityOwner`), `CustomAbilityList` (Delphi `TCustomAbilityList`), `ItemControl` (Delphi `TItemControl`), `MapContainer` (Delphi `TMapContainer`), `MapField` (Delphi `TMapField`), `MapLevel` (Delphi `TMapLevel`),
`MapObject` (Delphi `TMapObject`), `Player` (Delphi `TPlayer`), `Race` (Delphi `TRace`)) and 62 do not, at least 22 of which occur in the shipped files
(AoWHSMap, `Army` (Delphi `TArmy`), `City` (Delphi `TCity`), `Hero` (Delphi `THero`), `Item` (Delphi `TItem`), `Unit` (Delphi `TUnit`), `Leader` (Delphi `TLeader`) and `CombatUnit` (Delphi `TCombatUnit`) among them). The TODOs lose
in-memory behavior (exception frames, getter side effects such as Unit's unit resource lookup,
flag masks), not bytes, which is why the round trip still holds and why M1a is a persistence test
only: nothing built on the reduced model is a rules test.

M1a transpiles these three units and a C# port of `roundtrip.pas`, and the inflated output equals
the inflated input for both shipped saves (`After the Fall.asg`, `autosave.asg`) and, after one
load and save, for all 23 shipped maps. This is the first end-to-end test of the transpiler, the
shim's streams and strings, the metaclass registry and the layout model, on about 12,000 lines.

Caveat: the reduced engine's `EngineObject` differs from the real one (`CreateEObject` takes a
`EngineObject` owner, the storage mode is an integer), so the units are a separate transpile input,
under `tests/roundtrip-model/`, not part of the game's transpile set.

### M1b: round trip on the real engine

The same test with the real `Engine.pas` and the rules packages, which is the full transpile set
minus the UI. The reference changes with the engine. The reduced model writes back what it read,
but the real engine runs game code while it saves: `Player` writes the view center that
`GetViewCenter` computes from the stored pixel origin and the live viewport size, which no save
holds (`Player.PAS:1051`, `HSMAP.PAS:579`), and the load path recomputes morale and budgets,
extends diplomatic history and rounds the bit lists' capacity to 32 bits, as the original's own
decompile does. A file the original wrote before any load (a shipped save or map) therefore does
not round trip in the original either. Found by brief 2.2 on 2026-10-26.

The measure is therefore the original engine's own output after a load: the oracle's autosaves
(`D:\Build\oracle\original\<scenario>\<label>\day-NN.asg`, `testing.md` section 3), which the
original game wrote from a loaded game. Each one, loaded and saved by the port with the headless
host's viewport set to the size the oracle runs had, equals itself after inflation, ignoring only
`AoWHSMap` ids `$37` and `$3A` (the wall-clock turn time, the same ids `playtest.py compare`
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

Milestone 9.7 (`own-formats.md`) moved the disk format and kept the engine image (1.7). Every own
load and save still passes through the property tables, the widths and the record layouts above,
so a persisted `rwData` record keeps its explicit layout in the engine image. Version 3 of the
own document names that record's fields. The struct stays explicit-layout, because `ReadWrite`
still copies the bytes. Until the exit finishes, `ReadWrite`, the numeric `rw*` calls and raw
`rwData` are transitional and no stable mod API.

Step 1 of that exit is implemented for the pilot classes named in `persistence-tree.md`. It adds
`PersistFields` beside `ReadWrite` on those classes, a typed tree in `Aow.Formats.Tree`, and a
direct document backend. Step 2 is implemented except for the PE-2c shapes. Version 2 stores
enum bytes, sets and fixed integer arrays as C# member names, name lists and integer lists.
Version 3 stores a padded record as named fields, the ability bit array as ascending indices,
and a primitive `rwData` value as a plain integer or boolean. The original backend writes the
tree to the property-table image and back. `FloodControl.OriginalTerrain` (Delphi `TFloodControl`) and
`DiplomaticAction.ActionType` (Delphi `TDiplomaticAction`) stay exact hex. Stream callbacks, map blobs and run-length lists
stay for PE-2c. The C# record structs keep their explicit layout while `ReadWrite` still copies
their bytes. The catalog names the fields. It does not replace the struct with a plain record.
The game still loads and saves through `ReadWrite` and the converter. Step 3 is the switch.

Milestone 9.6 (`unions-and-kinds.md` section 4) changed the model of two persisted classes and no
byte of a file:

- The engine's message slots (`MsgArg`, `rtl-shim.md` 1.5) and the arguments of `Format`
  (`FormatArg`, 1.6) are not persisted. No message record reaches a stream or a packet.
- `ProductionItem` (Delphi `TProductionItem`) holds a `ProductionOrder` in place of its production id, parameter and group id,
  and `DiplomaticAction` holds a `DiplomaticProposal` in place of its action type, gold, mana and
  message. Each `ReadWrite` writes the same bytes as before, with the same calls, property ids and
  order (20 to 22 for the production item, 20 to 31 for the diplomatic action, the action type as
  the one-byte `rwData` at 25) and the zero rule of 1.4: it takes the stored values from the record
  (`ToFields`, and the proposal's message) before the calls and, when reading, builds the record
  with the registry's `FromFields` after them. Saves, maps, `ProductionTokenEvent` (Delphi `TProductionTE`) and
  `DiplomaticActionTokenEvent` (Delphi `TDiplomaticActionTE`) reach both classes through the same `ReadWrite`.
- An id that no kind registered reads as `UnknownOrder` or `UnknownProposal`, which writes the
  stored values back unchanged.
- A registered kind holds only its own fields and drops a value that a file holds in a field the
  kind does not use, so that value writes back as 0 and the zero rule omits it (decision X20 of
  `data/rewrite/roadmap.md`). The 9.6-0 census (`data/rewrite/9.6-census.md`) found no such value
  in the 401 files it read. Two values outside the kinds' own fields stay in the common part:
  `FOtherPlayer`, which the census finds written for every kind (255 for the kinds that name no
  other player), stays a field of `DiplomaticAction`, and the message is a member of the
  `DiplomaticProposal` base.
- `ShowInfo` (Delphi `TShowInfo`) keeps its 8-byte ILB image (`unions-and-kinds.md` 3.3). ILB files are game data,
  outside the save and map formats.
- The converter's mappings for `ProductionItem` and `DiplomaticAction` (`own-formats.md` 2.3 and
  3.1), which call `ToFields` and the registries' `FromFields`, arrived with brief 9.7-3 in
  version 1 of the own format. A document names the kind as `$case` with the kind's own fields,
  and an unregistered id reads as the case `Unknown`, which keeps the id and every value. The 9.6
  numeric knowledge (property ids, the one-byte kind, the zero rule) lives in two places during the
  transition: in `ReadWrite` for the engine image and in the converter's schema and case tables
  for the files.

## 6. Open points

1. Whether the AoWModel units can be transpiled as they are or need a small adapter for the
   reduced engine's API differences. Decided when M1a starts.
2. Multiplayer desync checks (`CompareStorageStream`) and localization
   (`TranslateStorageStream`) are transpiled but have no test until networking and localization
   are in scope.
