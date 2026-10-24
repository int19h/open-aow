# Persistence specification

State: draft, 2026-10-24. Covers the engine's object persistence (property tables, storage
streams, the compressed container), what the port must preserve, how the transpiled persistence
code meets the shim, and the round trip milestone that proves it. The format description comes
from `D:\git\aow\notes\persistence.md` and from `src/delphi3/Engine.pas` as read on 2026-10-24.

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
original game inflates, so saves written by the port load in the original game.

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
- `Exception.Message` as an `AnsiString`, because `HandleException` passes it to an event.

## 4. Milestones

### M1a: round trip on the reduced model

The `aow` repository has a reduced persistence engine, `src/recovered/Engine.pas` (1,711 lines)
with `EngineLists.pas`, and a generated object model `src/model/AoWModel.pas` (489 classes with
`ReadWrite` methods replayed from the decompiled code, 10,161 lines, plus hand-written overrides in
`src/model/hand/`). `tests/roundtrip.pas` loads a save through them and writes it back
byte-identically. These are Free Pascal units in Delphi mode, written by hand or generated, with no
byte-matching idioms. They are the smallest input that exercises the whole persistence path.

M1a transpiles these three units and a C# port of `roundtrip.pas`, and the inflated output equals
the inflated input for both shipped saves (`After the Fall.asg`, `autosave.asg`) and, after one
load and save, for all 23 shipped maps. This is the first end-to-end test of the transpiler, the
shim's streams and strings, the metaclass registry and the layout model, on about 12,000 lines.

Caveat: the reduced engine's `TEObject` differs from the real one (`CreateEObject` takes a
`TEObject` owner, the storage mode is an integer), so the units are a separate transpile input,
under `tests/roundtrip-model/`, not part of the game's transpile set.

### M1b: round trip on the real engine

The same test with the real `Engine.pas` and the rules packages, which is the full transpile set
minus the UI. The measure is the same byte equality.

### Validation tool

`tools/asg_dump.py` in the `aow` repository walks a save with the recovered schema and reports
unexplained property ids. The port adds a C# counterpart only if a difference in M1a or M1b needs
one. Until then the Python tool is the reference reader.

## 5. Open points

1. Whether the AoWModel units can be transpiled as they are or need a small adapter for the
   reduced engine's API differences. Decided when M1a starts.
2. Multiplayer desync checks (`TCompareStorageStream`) and localization
   (`TTranslateStorageStream`) are transpiled but have no test until networking and localization
   are in scope.
