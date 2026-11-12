# Persistence tree (PE-1, PE-2a, PE-2b, PE-2c-2, PE-2c-3)

State: PE-1 is implemented for the pilot classes. PE-2a adds version 2 of the own document,
symbolic enums and sets, and fixed integer arrays as lists, 2026-11-07. PE-2b adds version 3,
padded records, the ability bit array, raw primitive values, the record catalog, and a direct
original backend, 2026-11-08. PE-2c-2 adds the lists with a stored default (`int-list` and
`byte-list`) and the keyed dynamic members (`AbilityOwner.AbilityData` and `ItemControl.Items`)
on the tree and the direct path, with no document change, 2026-11-12. PE-2c-3 adds version 4:
the list-typed stream callbacks as lists, the bit lists as set indices and the enum lists as
names, 2026-11-12. This is step 1 and step 2 of the exit from the Delphi image (`own-formats.md`
6.3), except the other PE-2c shapes (resource payloads, bit lists on the tree, map blobs, sprites
and the campaign map). The game still loads and saves
through `ReadWrite` and the converter. PE-3 is the switch.

The API lives in `src/Formats/Tree/`, namespace `Aow.Formats.Tree`. Formats does not reference
Units. An engine object takes part through `IPersistable`. Units supplies the factory and the
child collection.

## 1. The serializer

One method per class serves both directions, the same shape as `ReadWrite`. The field hook is
`PersistFields`. `IPersistable.Persist` on `EngineObject` (Delphi `TEObject`) is explicit. It refuses a class the tree
cannot store, then calls `MainPersist`. On load, `MainPersist` calls `Clear` and then
`PersistFields`. `ECustomNode.MainPersist` (Delphi `TECustomNode`) does that and then persists `Children`.
`EChangeNotifyNode.MainPersist` (Delphi `TEChangeNotifyNode`) calls `BeginUpdate` before that load and `EndUpdate` after it,
the same bracket as `MainReadWrite`. `UnitList.MainPersist` (Delphi `TUnitList`) then calls `Release` on every child,
the same loop as `MainReadWrite`. `ReadWrite` is unchanged and remains the game path.

The refusal walks the type and its bases. A type that declares `MainReadWrite` or
`ReadWriteChildren` and does not declare `MainPersist` produces
`Class <name> overrides MainReadWrite or ReadWriteChildren and has no MainPersist.`
A type that declares `ReadWrite` and does not declare `PersistFields` produces
`Class <name> overrides ReadWrite and has no PersistFields.`
`PersistRefusalTests` pins every product engine class in either state. `Enchantment` (Delphi `TEnchantment`),
`EChangeNotifyNode`, `UnitList` and `ArmyView` (Delphi `TArmyView`) are not in that set. An army that owns a unit
refuses through `Unit` (Delphi `TUnit`).

```csharp
public interface IPersistable
{
    void Persist(IPersister persister);
}

public interface IPersistableFactory
{
    string ClassName(IPersistable objectValue);
    IPersistable Create(string className);
    void Release(IPersistable objectValue) { }
}

public interface IPersister
{
    bool Loading { get; }
    IPersistableFactory Factory { get; }
    bool Has(string name);
    int Int32(string name, int value, int fallback = 0);
    byte Byte(string name, byte value, byte fallback = 0);
    sbyte SByte(string name, sbyte value, sbyte fallback = 0);
    ushort UInt16(string name, ushort value, ushort fallback = 0);
    short Int16(string name, short value, short fallback = 0);
    uint UInt32(string name, uint value, uint fallback = 0);
    TEnum Enum<TEnum>(string name, TEnum value, TEnum fallback = default) where TEnum : struct, Enum;
    Set<E> Set<E>(string name, Set<E> value) where E : unmanaged, Enum;
    bool Boolean(string name, bool value);
    string String(string name, string value);
    void Lines(string name, IList<string> lines);
    void LinesWithObjects(string name, IList<string> lines, IList<int> objects);
    void Bytes(string name, Span<byte> data);
    int RawInt32(string name, int value);
    byte RawByte(string name, byte value);
    sbyte RawSByte(string name, sbyte value);
    bool RawBool(string name, bool value);
    void BitArray(string name, int count, Span<byte> bits);
    void Object(string name, IPersistable objectValue);
    IPersistable? Created(string name, IPersistable? objectValue);
    T? Created<T>(string name, T? objectValue) where T : class, IPersistable;
    IPersistable? CreatedSlot(string name, int slot, IPersistable? objectValue);
    T? CreatedSlot<T>(string name, int slot, T? objectValue) where T : class, IPersistable;
    void Skip(string name);
    int Int32List(string name, IList<int> items, int defaultValue);
    byte ByteList(string name, IList<byte> items, byte defaultValue);
    void Children(string name, IChildCollection children);
    int Reference(string name, string domain, int key, int fallback = 0);
    void Record(string name, Action<IPersister> body);
    void List<T>(string name, IList<T> items, ItemTransfer<T> transfer);
}
```

A value member takes the value to save and returns the value to keep: the loaded value on load,
the same value on save. The caller assigns the result, so a property works where a field had to
be passed by `ref`: `Gold = persister.Int32("Gold", Gold)`. The assignment also runs on save, with
the value the property already holds, so a setter with side effects runs them on save as well. A
value call therefore targets a field or an auto-property, and the `PersistFields` generator of
PE-3 reports any other setter. The
members that move many values in place keep their shape: `Bytes` and `BitArray` take spans,
`Lines`, `LinesWithObjects`, `List` and `Children` take collections, and `Object` and `Record`
run against an object or a body. `PersistSupport.Record`, `PersistSupport.UnsignedByte` and
`RecordValues.Transfer` follow the same style.

The storage streams of the original image (`StorageStream`, `StorageReader` and `StorageWriter`,
Delphi `TEStorageStream`, `TEReadStorageStream` and `TEWriteStorageStream`) name their members
by the same CLR types: `Int32`, `Byte`, `SByte`, `UInt16`, `Boolean`, `String`, `Lines`,
`LinesWithObjects`, `Bytes` (Delphi `rwData`), `Object` (`rwEObject`), `Created`
(`rwCreateEObject`), `Children` (`rwChildren`), `SizedObject` (`rwEObjectStream`), `Payload`
(`rwStreamProc`) and `OpenProperty` (`OpenPropertyStream`). A Delphi `Ex` method is the same name
with a `fallback` argument. A `ReadWrite` call and the `PersistFields` call of the same member
differ only in the member name argument.

`Has` reports presence on load and does not consume the member. On save it is false. A second
call of the same name on save is an error (`Codes.Duplicate`) with the path. After `Persist`
returns, a member the serializer did not consume is an error (`Codes.Required`) with the path.
`Skip` consumes a member on load without reading it, the way `ReadWrite` leaves a stored property
unread, and does nothing on save or for a member already read.

`Int32List` and `ByteList` move a list with a stored default (Delphi `TIntegerList` and
`TByteList`, stored by `rwStreamProc` with `LoadFromStream` and `SaveToStream`). They take the
items as a collection and the default as a value, and return the default to keep. The default is
state, not a fallback: a read past the end of the list returns it, and shipped campaigns store
different defaults for the same member. On save the member is always stored, an empty list
included. On load a present member replaces the items and returns the stored default. An absent
member leaves the items as they are and returns the caller's default, because
`TEReadStorageStream.rwStreamProc` does not call the load when the id is absent.

`CreatedSlot` moves one created object of a keyed dynamic member. The stored id is the slot plus
the schema's `slotBase`, so the slot is the object's identity: the ability id for
`AbilityOwner.AbilityData` (ids from 50) and the item's position for `ItemControl.Items` (ids
from 10). On save the calls of one member add their slots in call order, a null object is the
stored null, and a slot stored twice is `Codes.Slot` and a negative slot `Codes.Range`, with the
path. On load the call returns the object at that slot, created through the factory with the
rules of `Created`, or null when the slot is absent or holds the stored null. The first call takes
the member. A slot the load does not ask for stays unread, and a slot asked for twice is created
twice, as `ReadWrite` does with the same ids. `AbilityOwner.PersistFields` mirrors `ReadWrite`: on
load it reads `IDs` and then one slot per stored id, in that order, so an original whose two
members disagree loads the same on both paths, and it skips both members when the ability bits
(property 3) are absent. `ItemControl.PersistFields` reads slots 0 and on until the first absent
or null slot and writes the items that have no owner at slots 0 and on.

`IChildCollection` is the stored children of one node: count, the child at an index, whether
that child is stored, its slot, whether the node keeps indexes, `Attach` before the child's
`Persist`, and `Place` after it. The engine implementation is `NodeChildren` in
`src/Engine/PersistSupport.cs`. A stored child has this node as owner, has `Stored`, and does
not have `Shared`. The slot is the child's index. `Place` clears the owner set by `Attach`
and then calls `SetArrayOwner` when the node keeps indexes, otherwise `SetOwner`.

`IItemSlot` transfers one list item: `Int32`, `Byte`, `SByte`, `UInt16`, `Int16`, `UInt32`,
`Boolean`, `String`, `Record`, and `Object`. The call stores exactly one value and takes no
fallback. A value call returns the item to keep, as the members of `IPersister` do.
`ItemTransfer<T>` is the delegate `List` takes. It receives the item and returns the item to
keep, so a transfer is a lambda such as `static (IItemSlot slot, int item) => slot.Int32(item)`.

Floating-point fields, `Payload` callbacks (Delphi `rwStreamProc`) other than the integer and
byte lists, map fields, dynamic sprites, the fixed-object slots of `AbilityControl.Abilities`
and `SpellControl.Spells`, the campaign map stream, the `bit-list` codec and the grouped
`int-list` of a unit index list collection have no member on `IPersister`. From version 4 the four
`UnitIndexList` properties are `int-list` and the two `DiplomaticRelationList` properties
`byte-list`, so `Int32List`, `ByteList` and the `PersistSupport` adapters store them.

## 2. The tree

`TreeWriter.Write` builds an `ObjectNode` with no class name and no slot and calls
`IPersistable.Persist`. `TreeReader.Read` calls `Persist` and then checks that every member was
consumed. The tree knows no schema.

| type | stores |
|---|---|
| `IntegerNode` | width, signed, value |
| `EnumNode` | C# enum type name, width, signed, integer value |
| `SetNode` | element enum type name, width, little-endian mask |
| `BooleanNode` | value |
| `StringNode` | text |
| `LinesNode` | line list, or raw text when the block is not an array of lines |
| `LinesWithValuesNode` | the same text plus one signed 32-bit object value per line |
| `BytesNode` | exact bytes |
| `ObjectNode` | optional class name, optional slot, ordered named members |
| `NullNode` | the stored null of `rwCreateEObject` |
| `ChildListNode` | child object nodes |
| `RecordNode` | ordered named fields, no class and no slot |
| `ListNode` | a variable-length list of values |
| `ReferenceNode` | domain and integer key |
| `BitArrayNode` | count, ascending set-bit indices, residue byte of the bits past the count |
| `DefaultListNode` | width (4 for signed 32-bit values, 1 for bytes), the stored default, the items |
| `SlotListNode` | the slots of a keyed dynamic member in stored order, each an object node with class name and slot, or the stored null |

`ObjectNode` members stay in call order. The document backend reorders them. `ObjectNode.References(domain)` walks every `ReferenceNode` of that domain and yields the path and the key.

## 3. Presence and fallbacks

Presence follows `persistence.md` 1.4.

| call | omitted on save | absent on load |
|---|---|---|
| `Int32`, `Byte`, `SByte`, `UInt16`, `Int16`, `UInt32`, `Reference` | value equals the fallback | the fallback is assigned |
| `Boolean` | false | false |
| `String` | empty string | empty string |
| `Lines`, `LinesWithObjects`, `List` | empty | the list is cleared |
| `Bytes` | length zero | the caller's span is left as it is |
| `RawInt32`, `RawByte`, `RawSByte` | never, including zero | the caller's value is left as it is |
| `RawBool` | never, including false | the caller's value is left as it is |
| `BitArray` | the byte length is zero | the caller's span is left as it is |
| `Enum` | never, including zero | the fallback |
| `Set` | never, including an empty mask | the caller's value is left as it is |
| `Object` | never | `Persist` runs against an empty object node |
| `Created` | never. Null is stored as JSON null | null |
| `Children` | never. An empty child list is stored | no children are placed |
| `Int32List`, `ByteList` | never. An empty list is stored with its default | the items and the default are left as they are |
| `CreatedSlot` | the member is absent when no slot is stored. A null object is stored as the stored null | the slot reads as null |
| `Record` | never. The call always stores the record | the body runs against an empty record |

A named field inside a record follows the same row. A list item does not. Inside a list, false and integer 0 are stored. The caller sizes a `Bytes` span before the call. A length that differs from the stored bytes is `Codes.Range`. A fixed `data` size in the schema, including a single zero byte, is stored. Only length zero is absence.

An explicit false on an `rwBool` member is read as absence. A raw boolean keeps an explicit false, and a raw integer keeps an explicit zero. An explicit integer equal to the fallback of `Int32`, `Byte`, `SByte`, `UInt16`, `Int16` or `UInt32` is applied and then omitted on the next save. The tree follows the engine writer. It does not keep a hand-edited default that the converter leaves in the JSON. Equivalence is against that engine rewrite.

`String` and line text go through code page 1252. A line list whose block is an array of lines (each line ends in CR LF, and no line holds CR, LF or NUL) is a JSON array under `lines` for `rwStringsEx` and a JSON array for `rwStrings`. Any other block is `{ "$text": ... }`, with `objects` beside it for `rwStringsEx`. On load, noncanonical text is split the way `StringCollection.SetTextStr` (Delphi `TStrings`) splits it (CR, LF, or CR LF, and NUL ends the text). The raw line is not preserved. Both sides of an equivalence check pass through that split.

`Object` on load persists into the object the caller already holds. It does not replace that object. A missing member still runs `Persist` on an empty node, which is what `MainReadWrite` does with an empty property table.

`Created<T>` boxes through the non-generic call and requires the runtime class to be `T` (`Codes.ClassFamily`). A non-null created object needs the factory. Absent means null. JSON null is the stored null. `{ "$class": null }` is refused.

## 4. Identity and references

A created object and a child carry a class name. A child also carries `$slot`, a non-negative integer. A fixed object carries neither. A cross reference is an integer key plus a domain name. Version 1 writes the key as a plain integer and records no domain in the document. The tree keeps the domain on `ReferenceNode`. `Reference` on load accepts a `ReferenceNode` whose domain matches, or an `IntegerNode` of signed 32-bit width. PE-1 does not validate keys. The domain the game checks today is `player-slot` (`own-formats.md` 2.7). No pilot uses `Reference`. The test schema proves it.

## 5. Records and lists

`Record` is a group of named fields with no identity and no class. `List` is a variable-length sequence of values, records or objects. Version 1 has neither shape, so the version 1 backend refuses both, before it reports an unknown member. The message is `Version 1 does not store a list.` or `Version 1 does not store a record.`, with the document path. `ChildListNode` is not a `ListNode` and version 1 stores it.

Version 2 stores a list only for a fixed integer array. Any other list, and every record, is refused with the path. The list message is `Version 2 stores a list only for a fixed integer array.` The record message is `Version 2 does not store a record.` A bit array is refused with `Version 2 does not store a bit array.` A generic list is stored when the schema sets `lists` true. The test schema sets that flag. Product code does not name a later version to decide that a list is stored.

Version 3 stores a catalog record, a counted bit array and a raw primitive. A record field that a new serializer reads and an older document omits takes that field's zero value. Document field order is field-offset order. The serializer names no offset and no width. `PersistSupport.Record<T>` moves the struct through `RecordValues.Transfer`, which reads the record catalog.

A catalog record is a JSON object of its named fields. Each field uses the shape of its C# type: an integer, an enum name, a set's name array, a nested record, or a fixed array as a list. Integer zero is omitted. An enum and a set are stored, including zero and an empty set. `Rect` (Delphi `TRect`) is `{"Left":..,"Top":..,"Right":..,"Bottom":..}`. `PackedDamageTypes` (Delphi `TP3PackedDamageTypes`) is a one-field wrapper of `Set<DamageType>` with no residue, so the value is that set's name array and not a wrapper object. `SetupPlayerBehavior` (Delphi `TSetupPlayerBehavior`) stores `PlayerType` and `Flags`. A field named `Reserved`, and any byte no field covers, is residue. Every residue byte, in offset order, is one lowercase hex member `$reserved` when any byte is nonzero. The member sits at the first residue span. Zero residue is omitted. A present `$reserved` that is not a string, that is not lowercase hex, or whose length is not the residue, is refused with the member path. The code is `Codes.Type` for a non-string or a non-hex value and `Codes.Range` for the length. The image side refuses a tree value that is not bytes the same way, and a byte span of the wrong length the same way. A missing member stays absent. `$reserved` belongs to the record. It is not an envelope member, and `own-formats.md` 2.4 does not list it.

The ability count and the ability bits are two schema properties. `AbCount` is id 2, an `rwInteger`, omitted when 0. `AbilityCount` is id 3, the bits, and its byte length is `(count + 7) >> 3`. A zero count stores no bits. A nonzero length is stored even when every in-range bit is clear, as `[]`. The clean document is a JSON array of the set-bit indices in ascending order. An index at or beyond the count, a duplicate, and an index out of order are refused, and the duplicate and the unsorted index are reported on the item path. Bits past the count in the last byte are residue. When any of them is set the value is `{"values":[...],"$reserved":".."}`, and `$reserved` is that last byte with the in-range bits cleared. The indices stay integers. Ability identity is content data, and a mod extends it. No C# enum names those indices.

A raw primitive is a plain JSON integer or boolean at the C# width and signedness. `raw` true on `u8`, `i8` or `i32` marks an `rwData` integer, so zero is present. That is the `rwData` rule of `persistence.md` 1.4, not the `rwByte` or `rwInteger` fallback rule. `raw-bool` is an `rwData` bool. 0 and 1 are `false` and `true`. Any other byte is that integer. A C# `bool` stores the raw byte. A load of 0 or 1 writes that byte. A load of any other byte writes that byte back, so a stored 02 writes back as 02.

The parser accepts `list`, `record`, `bit-array`, `raw-bool`, `raw` on an integer, and an optional domain on an `i32`. An explicit-field `record` in a test schema still has no image. A catalog record does.

## 6. Direct document backend

```csharp
public static class DocumentBackend
{
    public static JsonObject Write(ObjectNode node, SchemaClass cls, FormatSchema schema, string engine, DocumentNodePath? path = null);
    public static ObjectNode Read(JsonObject document, SchemaClass cls, FormatSchema schema, string engine, DocumentNodePath? path = null);
}
```

`Write` and `Read` use `FormatSchema` for names, order and codecs. There is no engine image and no property table on this path. Member order is schema order. Reserved members keep the order in `own-formats.md` 2.2. `$class` is written for a created object and a child, and refused on a fixed object. `$slot` is written for a child and refused otherwise. `$layout` is ignored on read and is not written. Duplicate JSON members are refused.

The backend refuses a class or member the schema does not name, a value whose type does not fit the member's codec, a dynamic member other than a keyed created member (`PE-1 does not store a dynamic member.`), a list or record the version does not store, and a class with a case mapping.

A list with a stored default is the converter's `{"default": n, "items": [...]}` in every version. The default and each item are integers in the storage range of the width (signed 32-bit, or 0 to 255), `items` is required, and another member is `Codes.UnknownMember`. A keyed created member (`AbilityOwner.AbilityData`, `ItemControl.Items`) is the converter's array of created objects in stored order, each with `$class` and `$slot`, and a stored null is `{"$class": null, "$slot": n}` with no other member. A missing `$slot` is `Codes.MissingMember`, a slot whose id falls outside the dynamic range is `Codes.Range` on `$slot`, a slot declared twice is `Codes.Slot`, and a class outside the member's family is `Codes.ClassFamily`, each with the item path. The mapping message is `Class <name> uses a case mapping. The document backend does not reproduce it.` with the path. `DiplomaticAction` (Delphi `TDiplomaticAction`) and `ProductionItem` (Delphi `TProductionItem`) are the mapped classes, and the pilot census excludes them. Each error carries the document path (`root`, then `.name` or `[index]`).

Version 1 requires a created or child class to have an id that resolves to that class in the named engine table. A later schema version requires the class to exist and, when a family is set, to inherit that family. It does not require engine enlistment. A fixed root is the class the caller passes and is not looked up that way, so a class with no engine row can still be a fixed root (`MessageEventLog` (Delphi `TMessageEventLog`), `FloodControl` (Delphi `TFloodControl`)).

Each document value rule is `WriteTree` and `ReadTree` on the codec. There is no switch on a kind. The three frozen steps are the exceptions. Each switches on the codec type of the schema it walks. The version 2 step walks the pinned version 2 schema and decodes through embedded version 3 codec specs. Those specs and the record layouts are pinned. The codec implementations are live code. A later schema reannotation or a C# field rename does not change the step. A change to a live codec implementation does. Enum and set names still come from the enum catalog, the same way the version 1 step does. The converter and the backend share one implementation of each rule that already lived in the converter:

- `TextCodec.Problem` checks code page 1252 and the string length limit.
- `HexText.IsLowerHex` and `HexText.Decode` check and decode lowercase hex.
- `StringsCodec.Join`, `Lines`, `LineCount`, `TextValue` and `TextOf` own the `rwStrings` block rule.
- `LayoutResidue.Hex` formats `rwData`.

Version 2 keeps the version 1 classes and properties. An enum property is `codec` `enum` with `enum` naming the C# type, plus `width` and `signed`. A set property is `codec` `set` with `set` naming the element type and `width`. A fixed integer array is `codec` `int-array` with `width`, `signed` and `count`. `PropertyCodec.Create` accepts widths 1, 2 and 4 for those three codecs and refuses any other width with the schema path. Version 1 and version 2 keep their historical hashes.

Version 3 keeps the version 2 classes and properties and reannotates 39 `data` properties. Five are `codec` `record` with `record` naming the C# struct and `size` the image width. One is `codec` `bit-array` with `count` naming the sibling `AbCount`. Twenty-eight are `u8` with `raw` true, one is `i8` with `raw` true, three are `i32` with `raw` true, and one is `raw-bool`. `DiplomaticAction.ActionType` and `FloodControl.OriginalTerrain` stay `data`. `versions.json` current is 3. Its schema hash is `550a5553edce148e835a87eadd81a8063874bbc2167118c4099c3731e4f60df3`. The added contracts are `record-1`, `bit-array-1` and `raw-1`. The mapping contracts stay as they were.

The migration `1-symbolic-enum-set-array` rewrites version 1 hex of the enum, set and fixed-array properties into names and lists. It then recomputes every `$layout` fingerprint, because the fingerprint covers the visible values, and it leaves the parts in place. A nested campaign map keeps its own profile on that fingerprint. Every other value stays in place. The step walks the pinned version 2 schema embedded as `schema-v2.json`, not `FormatSchema.Current`. The version 1 schema of main `a7eb5da` is embedded as `schema-v1.json` and is the schema a version 1 import uses. A value that is not lowercase hex where version 1 stores hex, including an integer, and a catalog miss, raise `ConversionException` with the document path and `Codes.Type`. A hex length the version 2 member does not store raises `ConversionException` with the path and `Codes.Range`.

Version 4 keeps the version 3 classes and properties and reannotates the lists. The four unit index lists and the move path change from `stream` to `int-list`, the three unit index list collections to `int-list` with `groups` 8, and the two legacy relation lists to `byte-list` with `enum` `DiplomaticRelation`. The move path names `record` `MapCoordinate`. The three bit lists gain `indices` true. `Leader.Spheres`, `PlayerDiplomaticRelations.Relations` and `Race.DefaultRelations` gain `enum` `MagicSphere`, `DiplomaticRelation` and `Status`. Twenty-seven list properties gain `domain`, an annotation of what their values refer to. `versions.json` current is 4. Its schema hash is `8a00a8f9d948640b5b025b244383f0a3a9812c82cd3eedfaefa6fdfcd776da94`. The added contracts are `list-element-1`, `grouped-list-1` and `bit-list-1`. A list's element is a `ListElement`: `EnumListElement` writes the member name or the integer and reads either, `RecordListElement` writes the 4-byte value as a catalog record and reads it back. `RunListCodec` writes the default and each item through it in the document, on the converter and on the tree, and keeps the stored numbers on the image and in the `DefaultListNode`. `GroupedListCodec` reads the flat run list and writes `{"default", "groups", "count"}` (`own-formats.md` 2.3). `BitListCodec` with `indices` writes `{"default", "count", "bits", "$reserved"}` with the index and residue rules of the ability bit array. The direct path refuses a grouped list and a bit list. The record catalog stores one field set of an overlapping struct: a field whose every byte narrower fields cover is a view of them, so `MapCoordinate` stores `X`, `Y`, `MapLevel` and `Tag`, and `XY` and `Packed` are views.

The migration `2-record-bit-primitive` rewrites version 2 hex of the 39 properties. It walks the pinned version 2 schema embedded as `schema-v2.json` and decodes each pinned value through an embedded version 3 codec spec. The three record layouts are embedded field lists: field names, kinds, offsets, widths, residue spans, and the enum or set type name. The codec specs and the record layouts are pinned. The codec implementations are live code. A later schema reannotation or a C# field rename does not change the step. A change to a live codec implementation does. The step does not read `FormatSchema.Current` and it does not embed a version 3 schema. `FormatSchema.Substituted` is the test hook that observes that. Production leaves it null, so `Current` is the embedded schema after one null comparison. The persist path does not read it. Enum and set names still come from the enum catalog, the same way the version 1 step does. It recomputes every `$layout` fingerprint and leaves the parts in place. A value that is not lowercase hex where version 2 stores one of those properties raises `ConversionException` with the path and `Codes.Type`. A version 1 document still loads, through both steps.

The migration `3-lists` rewrites the version 3 values of 16 properties: the ten list-typed `stream` properties, the three bit lists and the three enum byte lists. It walks the pinned version 3 schema embedded as `schema-v3.json`. Each pinned property encodes its version 3 value through its version 3 codec, so hex passes through as the stored bytes, a version 3 bit list as its exact bytes and a byte list as its canonical runs. It decodes those bytes through an embedded version 4 codec spec. The `MapCoordinate` layout is an embedded field list. A list payload whose runs the writer does not make gains a `value-encoding` part on its object, created when the object has no `$layout`. The step does not read `FormatSchema.Current`. Enum names come from the enum catalog. It recomputes every `$layout` fingerprint. A value the version 3 codec does not encode raises `ConversionException` with the path, `Codes.Type` for a value that is not lowercase hex. Documents of versions 1 to 3 still load. The converter writes version 4.

Formats does not reference Units, so it does not hold a name table. `EnumCatalog.Register` and `EnumCatalog.RegisterReflection` take catalogs. `ClassRegistry`'s module initializer registers the units assembly by reflection, one registration for every enum in that assembly, and the converter's `UnitCatalog.Register` calls the same method. That call does not run the units module initializer. `Program` runs it with `RuntimeHelpers.RunModuleConstructor` before a conversion. A second registration of the same assembly is ignored. Two catalogs that both name one type fail at the lookup. Formats tests register `SampleRank` and `OtherRank` beside that. The tree itself does not use the catalog. `Enum` and `Set` carry `typeof` at the call. The catalog is for the converter, the migration and the backends, where no object exists. `EnumCodec` and `SetCodec` keep the catalog entry after the first successful lookup.

`RecordCatalog` is the same seam for explicit-layout structs. `Register` takes a catalog and `RegisterReflection` scans one assembly. `ClassRegistry` registers the units assembly and the runtime assembly that holds `Rect`. A second registration of the same assembly is ignored. Two catalogs that both name one record fail at the lookup. Formats tests register `SampleBehavior`, `SamplePacked` and `EveryWidth`. The scan reads `Type.StructLayoutAttribute` and keeps a struct whose layout is explicit and whose size is positive. Field names are the C# field names, in `FieldOffset` order, and the field type is the C# type. A set stored as an image field plus a property of type `Set<E>` takes the property name. A field named `Reserved` is residue. `Find` and `Require` resolve a schema record name where no game object exists.

An enum value is the C# member name from `Enum.GetName`, exactly as declared. A value with no member is stored as its integer, so a byte the enum does not declare is not dropped. Strict reading refuses a name the enum does not have, including a name of a different enum. A set is a JSON array of member names in ascending ordinal order, and an unnamed bit is its integer. Duplicate bits and a bit outside the width are refused. A fixed integer array is a JSON array of integers in index order. Trailing zeros stay. The tree and the own format accept a list longer than `count`. Export to the original format refuses it with the path and does not truncate. A shorter list fills the rest with zero on export.

```csharp
public static class OriginalBackend
{
    public static byte[] Write(ObjectNode node, SchemaClass cls, FormatSchema schema, string engine);
    public static ObjectNode Read(byte[] image, SchemaClass cls, FormatSchema schema, string engine);
}
```

`OriginalBackend` writes a tree value straight to the original image and reads that image back. `WriteImage` and `ReadImage` on the codec do the value. `ImageTree` walks the schema property table: ids, order, presence, and the engine class table for a created object and a child. There is no JSON document on this path and no `ReadWrite`. `WriteImage` and `ReadImage` on each leaf the tree stores move bytes and tree nodes, except the bit array. Its write builds the JSON value `Pack` already takes and then writes those bytes. Its read unpacks the bytes into the node. The base refuses a codec that does not override them (`<type> has no image writer.` and `<type> has no image reader.`). A codec with an image refusal still reports that message. The mapped-class message is `Class <name> uses a case mapping. The original backend does not reproduce it.` A dynamic member other than a keyed created member is `PE-3 still needs a dynamic member. The direct path does not store it.` A generic list, a float, a bit list, a grouped list, map fields and a campaign map are refused, and the bit list, map fields and campaign map messages name PE-2c. A list with a stored default writes the canonical runs of `TIntegerList.SaveToStream` and `TByteList.SaveToStream`: the default, the 32-bit count, then each item, where a run of defaults is the default followed by the run length, and a byte run longer than 255 splits into runs of 255 and the rest. The read accepts any run split the stream decodes and refuses a run past the count and bytes after the items. A keyed created member writes one table entry per slot at id slot plus `slotBase`, each a class id and the object image, and reads the entries in data order. An explicit-field test record has no image (`A record has no image.`). A catalog record, a bit array, a raw primitive, an enum, a set, a fixed integer array and the PE-1 value codecs do. Remaining `data` bytes, including a resource stream callback stored as hex, round-trip as those bytes. PE-2c replaces the resource stream and map shapes. The direct path writes a canonical property table and does not keep `$layout`. Class id 0 is null. A stored child is never nil. Bytes after a nil object are corrupt. The serializers name members only. They do not mention an id or a width.

`PilotImage.Decode` and `PilotImage.Encode` (`src/Formats/Original/PilotImage.cs`) are internal. They decode and encode one object image under a class through the converter. Tests use them. They are not public API.

## 7. Factory

`EnginePersistableFactory` in `src/Engine/PersistSupport.cs` implements `IPersistableFactory`. It indexes `EngineClass.All` by `StorageKey` once. Two classes with the same name throw. `StorageKey` uses `TObject.ClassNameOf`, which is the type name with a leading `@` removed. The name index is every class `ClassRegistry` registers, including classes with no engine enlistment.

The factory is constructed with the engine of the load. `Create` looks the class up in that engine's table, the same lookup `GetEngineClass` uses. An unknown name throws `The engine has no class <name>.` A null engine throws `The engine is not running.` `ClassTable.Find` must return that same class at the class id. A missing id, or an id owned by a different class, throws `Unregistered E-ClassID found (<id>)`. That is the message `TEngine.GetEClass` raises (`Engine.pas` 8864, `Engine.cs`). `TEReadStorageStream.rwCreateEObject` wraps creation and `MainReadWrite`. It frees the new object and sets it nil when `HandleException` returns true, and it rethrows when `OnError` sets continue false (`Engine.pas` 10208). The default `OnError` is null, so continue stays true and the exception is swallowed (`Engine.pas` 10191). `rwChildren` calls `rwCreateEObject` for each child and continues when that child is nil (`Engine.pas` 10263). `Created`, `Children` and a list object's slot use the same rule. Any exception is passed to the load's error handler after `RtlUnit.Wrap`. A null handler continues. The object is released and the slot stays nil. A handler that sets continue false makes the load throw the original exception. Once a handler stops, later exceptions stop too. An empty message is reported as `Error Reading EngineObject`. `rwEObject` wraps `MainReadWrite` of an object the caller already holds (`Engine.pas` 10277). Continue leaves that instance in place. The load does not free it and does not set it nil. The rest of the parent then continues. Stop rethrows the original exception. `Object` follows that rule. A null handler continues, so a refusal inside a fixed object, a created object or a child is swallowed. The fixed instance stays. A created object or a child is released and its slot stays nil, and the parent continues. The root read is not wrapped, so a refusal on the root still fails the load. A test that asserts a strict nested refusal passes a handler that sets continue false. A list object's slot keeps the created-object rule. A test replaces the creation delegate on an `EngineClass` and restores it. `Create` does not check a fault flag. A fixed object is that instance. A test root that is not in the live table is created from `EngineClass.All`. A child still goes through this factory. The tree load has no editor caller yet. PE-3, at the editor load, passes a handler that calls `OnStreamError` the way `TAoWHSMap.ReadWrite` assigns `Stream.OnError` when the status contains `msEditting`.

`PersistSupport` also moves the engine shapes the serializer cannot name directly. `Int32List`, `ByteList` and `UnitIndexList` copy an engine list (Delphi `TIntegerList`, `TByteList`, `TUnitIndexList`) into a list and its default, call the persister, and on a load of a present member clear the list, set the stored default and add the items, as `LoadFromStream` does. An absent member leaves the engine list untouched. `UnitIndexList` is the integer list whose constructor default is -1, and a stored default that differs from it is kept. `UnsignedByte` stores an `sbyte` through `Byte` (the source byte of `rwByte`, including 255 for -1). `Int32Array` and `UInt16Array` copy a fixed array into a list, call `List`, and on load write the list back, filling a short list with zero and refusing a list longer than the array. `Lines` and `LinesEx` copy a `StringCollection` into lists, call the persister, and copy back only on load. `ObjectToInt(null)` is 0 and `IntToObject(0)` is null.

## 8. What PE-2 to PE-5 add

PE-2a, version 2, stores enum bytes, sets and fixed integer arrays as names and lists. PE-2b, version 3, stores padded records, the ability bit array and the raw primitive values, and the original backend is the direct path in section 6. PE-2c-2 stores the integer and byte lists with their default and the keyed dynamic members on the tree and the direct path, and gives `PersistFields` to `AbilityOwner` (Delphi `TAbilityOwner`), `AbilityData` and its subclasses that declare `ReadWrite` (`CommandAbilityData`, `CommandedAbilityData`, `DispelMagicAbilityData`, `DurationAbilityData`, `HealingAbilityData`, `MultiLevelAbilityData`, `LeadershipAbilityData`, `PossessedAbilityData`), `Item` (Delphi `TItem`), `ItemControl`, `TutorialControl`, `PlayerDiplomaticRelations` and `ItemExplorationSiteEventLog`. `Item.PersistFields` leaves out the text-only branches of `ReadWrite`, which only the text and translate streams take. PE-2c-3 gives `PersistFields` to `RaceResource` (Delphi `TRaceResource`), the first caller of the `UnitIndexList` adapter, and stores its image library name on every path, as `Item` does. PE-2c still owns the resource `stream` callbacks, the bit lists and the grouped lists on the tree, the map blobs, the sprites and the campaign map. The direct path refuses those shapes. PE-3 still owns mapped classes, the switch off `ReadWrite`, the preview skip of the ability-bit cleanup and of `ItemControl`'s items, and the fixed-object slots of `AbilityControl` and `SpellControl`. `Troop` (Delphi `TAbstractUnit`), `Unit`, `Hero` (Delphi `THero`) and `Leader` (Delphi `TLeader`) declare their own `ReadWrite` and stay refused until each has `PersistFields`. `UnitView` (Delphi `TUnitView`), `UnitResourceAbilityOwner` (Delphi `TUnitResourceAbilityOwner`) and `HeroResourceAbilityOwner` (Delphi `THeroResourceAbilityOwner`) do not declare `ReadWrite`. `CustomAbilityList.ReadWrite` (Delphi `TCustomAbilityList`) skips that cleanup when `smPreview` is set. The tree load does not carry preview state, so `PersistFields` always cleans until a preview load passes that state.

PE-3 rewrites each `ReadWrite` into `Persist` and moves own load and save onto `DocumentBackend`. Historical document decoders and original import and export stay. Until that brief, no game load or save uses this path.

PE-4 separates token-event transport from disk persistence. Packet widths stay on the legacy adapters until networking changes them.

PE-5 proves extended arrays and richer cases, lossless own reload, strict refusal of unrepresentable exports, historical migrations, and the G1, G2 and G-own gate, and checks that no Delphi image remains on the own disk path or in the stable mod interface.

## 9. Pilot census

A class is eligible when every effective property (`SchemaClass.Complete`) uses a PE-1 codec, and the same holds for every fixed-object class it embeds and every class in a created family it names. The PE-1 codecs are `i32`, `u8`, `i8`, `u16`, `bool`, `string`, `strings`, `strings-ex`, `data`, `object`, `created` and `children`. An unbounded created member fails eligibility. A children member is a PE-1 codec and is not expanded to every class the engine can attach, because the schema names no child family. Treating every engine class as a possible child leaves no eligible class that uses children.

The schema has 970 classes. 582 use only PE-1 codecs on their own effective properties. 554 stay eligible after the object and created closure. Among those 554, the classes that use each codec are: `i32` 273, `u8` 224, `data` 138, `children` 117, `string` 55, `bool` 27, `object` 27, `strings` 18, `i8` 7, `u16` 6, `created` 6, `strings-ex` 1. Across every class, effective uses outside that set include `stream` 584, `int-list` 39, `bit-list` 27, `byte-list` 15, `map-fields` 3, `campaign-map` 2 and `float32` 1. Those counts are the PE-1 census. Version 2 reannotates 92 declared `data` properties as `enum` (60), `set` (27) and `int-array` (5). The extended eligibility rule treats those three codecs as closed and still excludes a class with a case mapping.

Created families that are fully eligible are `Animation` (Delphi `TAnimation`) (13), `DiplomaticAction` (1), `GlobalEnchantment` (Delphi `TGlobalEnchantment`) (16), `ProductionItem` (1) and `UnitEnchantment` (Delphi `TUnitEnchantment`) (2, itself and `CosmeticSurgeryEnchantment` (Delphi `TCosmeticSurgeryEnchantment`)). Mapped classes and `Animation` are not piloted. The global-enchantment pilot is the exact class plus the nested `FloodControl`, not every subclass.

`ItemLibrary` (Delphi `TItemLibrary`) and `HeroLibrary` (Delphi `THeroLibrary`) are eligible as classes (`children`, `i32`, `string`). The files are not a closed PE-1 graph: their children are `Item` and `Hero`, which use `int-list`. They are not piloted. No eligible class is a corpus-safe home for a non-zero `i32`, `u8` or `u16` fallback. `rwByteEx` is only on ineligible `Structure` (Delphi `TStructure`) and `Leader`. `rwWordEx` is never called. The test schema proves those fallbacks.

The pilots, and the codecs they add:

| classes | codecs |
|---|---|
| `MessageEventLog`, `EventLog` (Delphi `TEventLog`) | `strings`, `string`, `enum` (`Layout`, `Buttons`), `i32`, `u8` |
| `ResourceEditSettings` (Delphi `TEResourceEditSettings`), `ControlStyleObject` (Delphi `TEControlStyleObject`) | `strings-ex`, `set` (`ControlStyle`, including a zero mask) |
| `ItemGraphics` (Delphi `TItemGFX`), `EngineResource` (Delphi `TEResource`) | `bool`, `string`, `i32`, `enum` (`ItemType`) |
| `AIBudgetManager` (Delphi `TAIBudgetManager`), `AIBudgetList` (Delphi `TAIBudgetList`), `AIBudget` (Delphi `TAIBudget`) and the budgets that do not override `ReadWrite` (`AISavingsBudget` (Delphi `TAISavingsBudget`), `AITransporterBudget` (Delphi `TAITransporterBudget`), `AICityUpgradeBudget` (Delphi `TAICityUpgradeBudget`), `AICityResearchBudget` (Delphi `TAICityResearchBudget`), `AICityFortifyBudget` (Delphi `TAICityFortifyBudget`)) | `object`, `children`, `i32` with fallback 0 |
| `UnitEnchantmentAbilityData` (Delphi `TUnitEnchantmentAbilityData`), `UnitEnchantment`, `Enchantment`, `CosmeticSurgeryEnchantment` | `created` (present and null), `u16` fallback 0, `i8` fallback -1, `i32`, `u8` |
| `FrozenWaterHexSprite` (Delphi `TFrozenWaterHS`) | `i8` fallback 0 |
| `FloodEnchantment` (Delphi `TFloodEnchantment`), `GlobalEnchantment`, `FloodControl` | variable `data`, embedded `object`, `bool`, `i8` fallback -1, `u8`, `i32`, `string`, `enum` (`EnchantmentType`) |
| `DiplomaticRelationHistoryItem` (Delphi `TDiplomaticRelationHistoryItem`) | `enum` (`DiplomaticRelation` (Delphi `TDiplomaticRelation`)), `i32` |
| `ChangeGlobalMagicTokenEvent` (Delphi `TChangeGlobalMagicTE`), `TokenEvent` (Delphi `TTokenEvent`) | `enum` (`ChangeGlobalMagicTokenEventMode` (Delphi `TChangeGlobalMagicMode`)), `u8`, `i32` (`TokenCounter` included) |
| `UnitSpellCombatAction` (Delphi `TUnitSpellCA`) | `int-array` width 4 count 2, `i32`, `u8` |
| `GlobalMagicControl` (Delphi `TGlobalMagicControl`), `CityEnchantment` (Delphi `TCityEnchantment`), `HatredEnchantment` (Delphi `THatredEnchantment`), `HarmonyEnchantment` (Delphi `THarmonyEnchantment`) | `int-array` width 4 count 9, `object`, `enum` (`CityEnchantmentId` (Delphi `TCityEnchantmentID`)), `i32` |
| `SingleTargetCombatAction` (Delphi `TSingleTargetCA`) | `set` (`SingleTargetModeElement` (Delphi `TSingleTargetModeElement`)), `u8` |
| `Army` (Delphi `TArmy`), `ArmyView` (constructed only) | `set`, `enum` (`GroupBehavior` (Delphi `TGroupBehavior`)), `int-array` width 2 count 8 and width 4 count 8, `u8`, `u16` |
| `Animation` | `record` (`Rect` as `Dimensions`), `u8` (`HexX`, `HexY`, `HexLevel`), `i32` |
| `DamageCombatAction` (Delphi `TDamageCA`) and subclasses that do not declare `ReadWrite` | `record` stored as the set `DamageType` (Delphi `TDamageType`) (`Effects`, `DamageTypes`), `u8` |
| `RemotePlayer` (Delphi `TRemotePlayer`) | raw `i32` (`RemoteID`), `enum` (`MapValidationInfo` (Delphi `TMapValidationInfo`)), `string`, `u8`, `i32` |
| `AIGroup` (Delphi `TAIGroup`) and subclasses that do not declare `ReadWrite` | raw `i8` (`IndependentRelation`), raw `u8` (`TargetMode`), `enum`, `i32`, `u8` |
| `SetupSettings` (Delphi `TSetupSettings`) | raw `u8` (`PlayMode`, `VictoryCondition`, `IndependentAIMode`, `ConnectionType`, `TacticalCombatAvailability`), `bool`, `string`, `i32`, embedded `object` |
| `CustomAbilityList`, `AbilityList` (Delphi `TAbilityList`) | `bit-array` (`AbilityCount`, count sibling `AbCount`) |

Constructor defaults are not stream fallbacks. `Enchantment` constructs dispel resistance 10, upkeep 1 and source -1, and those values are stored. Stream fallback 0 omits only a real 0. `UnitEnchantmentAbilityData` constructs player and duration as -1, which matches the `rwShortintEx` fallback, so those members are omitted until they change. Since PE-2c-2 `AbilityData` (Delphi `TAbilityData`) has `PersistFields`, and the class is in the equivalence. `ControlStyleObject` constructs the set bit `csSaveLoad`. The const alias `Stored` is not an enum member, so the document stores `csSaveLoad` and the image byte stays `01`. `ArmyView` is not a corpus pilot. The only width-2 array sits on it, and its child closure is the unit tree. `Army.PersistFields` mirrors `ReadWrite`. On a save with connection type 1 it omits the three destinations and `UserMode`, and on load it reads those members when they are present and clears `amMoving` from `Mode`. The required corpus asserts do not name `Army` or `ArmyView`. An army is proved when its closure finishes and blocked when a child refuses. A constructed test covers connection type 1, a load that clears `amMoving`, and an army that owns a unit, which refuses through `Unit`. The army tests install a map marked `msDestroying` so `Army.Destroy` does not touch a null map.

`PlayerSetupSettings` (Delphi `TPlayerSetupSettings`) is not a pilot. Its `ReadWrite` stores both behavior records and then a `Leader`, and the leader closure uses an integer list and dynamic ability data. `SetupPlayerBehavior`, including a nonzero `Reserved`, round-trips in `RecordShapeTests` through the document backend and the original backend on the real schema. A stub `IPersistable` in `PersistEquivalenceTests` calls `Transfer<SetupPlayerBehavior>` with a nonzero `Reserved` and checks the same document and image. `CustomAbilityList.PersistFields` mirrors the cleanup in `ReadWrite`. After the bits are stored, an in-range bit whose ability is missing is cleared. A constructed case places abilities in the fixture's ability list, so several in-range bits survive both paths. A second case sets a bit whose ability is missing, together with residue past the count, and both paths clear that bit and keep the residue. `ReadWrite` skips the cleanup when `smPreview` is set. `PersistFields` always runs it. The tree load does not carry preview state. PE-3, when a tree load is a preview, skips that cleanup the way `ReadWrite` does when `smPreview` is set. Until then `PersistFields` always cleans. `Animation` and `AIGroup` are not in the live engine table. `HuntAIGroup` (Delphi `THuntAG`), `AbilityOwner`, `AnimationTokenEvent` (Delphi `TAnimationTE`), `StrikeCombatAction` (Delphi `TStrikeCA`) and `RangedAttackCombatAction` (Delphi `TRangedAttackCA`) declare their own `ReadWrite` and stay refused. `SelfDestructCombatAction` (Delphi `TSelfDestructCA`) and `TurnUndeadCombatAction` (Delphi `TTurnUndeadCA`) do not, so they are eligible through `DamageCombatAction`.

## 10. Proof

`src/Game.Tests/PersistEquivalenceTests.cs` builds every pilot, old and new, at zero, default, nondefault and boundary values. It covers each stored enum's declared members, an undeclared byte, an empty set, a full set, fixed arrays at zero, mixed and boundary values, a record residue, and a bit past the ability count. `Persist` through the tree writer and `DocumentBackend` matches `PilotImage.Decode` of the image `MainReadWrite` writes. The same tree through `OriginalBackend` matches that image byte for byte. The read comparison is against a `MainReadWrite` load of that same image. When the object has no dropped child, the tree read also matches `PilotImage.Encode` of the converter document. Three constructed states own a child the live table does not enlist (`AIBudgetManager` with budgets, `GlobalMagicControl` with a city enchantment, and `GlobalMagicControl` with hatred and harmony). Both loads drop that child, and the converter document still holds it, so those three compare the tree read only with the `MainReadWrite` load. A child whose persist throws is dropped when the handler continues, and the load throws when the handler stops, on the tree and on `MainReadWrite` with the same handler. An image read through `OriginalBackend` and the tree reader matches the same load. The same original image loaded through `MainReadWrite` into a fresh object, then written with `MainReadWrite`, matches that rewrite. An overlong `SpellData` list is kept by the document backend and refused by `OriginalBackend` at `root.SpellData`. `UnitEnchantmentAbilityData` is in that equality since PE-2c-2. The list cases cover both widths: an empty list, an all-default list, a list with the extreme values, a default that differs from the constructor's, a byte run of 255, 256 and over 255 (written as runs of 255 and the rest, the bytes `MainReadWrite` writes), and an absent member, which leaves the list and its default on `MainReadWrite`, the original backend and the document backend alike. `PlayerDiplomaticRelations` resets its default to `Unknown` after a load on both paths. A `UnitIndexList` keeps a stored default other than -1. `Item` with nine ability data objects of every family member, and `ItemControl` with items that carry ability data, match the converter and both loads. A document whose `IDs` name an absent slot and leave a stored slot unread, a document without the ability bits, an item list with a gap at slot 1 and an item list whose slot 0 is the stored null load the same through `MainReadWrite`, the original backend and the document backend.

`src/Game.Tests/PersistCorpusTests.cs` walks every instance whose `TreePersistenceRefusal` is null, in the corpus files the converter reads, when `AOW_GAME_ROOT` (or the default install) has `Release/Release.hss` and `Scenario/Custom`. The files are `Scenario/Custom/*.hsm`, `Scenario/Campaign/*.csm`, `Scenario/Campaign/*.cam`, `Release/*.ahl`, `Release/*.hss`, `TCMaps/*.hsm` and `Save/*.asg`. `Army` and `ArmyView` stay out of that walk's assertions. The comparison is the converter equivalence and the original-backend equivalence, against the engine rewrite of each instance, not against the imported document, because an import can carry `$layout`, an explicit default, or a non-dense slot. The same original image is loaded through `Persist` and through `MainReadWrite`, and the two `MainReadWrite` images are equal. The walk enters a class only when `TreePersistenceRefusal` is null. An instance is proved only when its write comparison and both read comparisons finish. A refusal anywhere in its closure blocks that instance. Both loads use a handler that stops, so a nested part that fails to load fails the comparison instead of vanishing from both sides. The fixture loads no game resources and registers no editor classes, so an instance whose original load needs one (a unit resource, an unregistered class id) is blocked under a `fixture:` label and proves nothing. A tree refusal names the refusing class. A direct-path refusal names its diagnostic. A byte mismatch fails the test. `UnitEnchantmentAbilityData` stays refused through `AbilityData` and is not counted. Every import is version 4. Each corpus file is also imported with the frozen version 1 schema of main `a7eb5da`, the frozen version 2 schema and the frozen version 3 schema, and each migrated document equals the direct version 4 import. Import decodes with `FormatSchema.Current`. The version 2 step decodes the 39 pinned properties and the version 3 step the 16 pinned properties through embedded codec specs, so those comparisons are not one codec compared with itself. Every step recomputes `$layout` fingerprints. The test asserts a proved instance of `GuardAIGroup` (Delphi `TGuardAG`), `PassiveAIGroup` (Delphi `TPassiveAG`), `FortifyAIGroup` (Delphi `TFortifyAG`), `NormalAIGroup` (Delphi `TNormalAG`), `SkirmishAIGroup` (Delphi `TSkirmishAG`), `AIBudgetManager`, `ResourceEditSettings`, `GlobalMagicControl`, `Item`, `ItemControl`, `PlayerDiplomaticRelations`, `TutorialControl` and `RaceResource`. It prints proved and blocked counts for every tree-persistable class the corpus holds, the watched names even at zero, and the refusing classes. The class line keeps the first label. A later line sums each refusing class and each direct-path diagnostic. Since PE-2c-2 `CustomAbilityList.PersistFields` has corpus coverage through `Item`: 4,620 items prove the bit array, the ability-id list and the keyed ability data, and 68 item controls prove the keyed items. 125 `PlayerDiplomaticRelations` prove the byte list, 67 `TutorialControl` the integer list, and the ability data classes 5,000 more instances (`DispelMagicAbilityData` 285, `DurationAbilityData` 50, `HealingAbilityData` 61, `LeadershipAbilityData` 1,023, `MultiLevelAbilityData` 3,521, `UnitEnchantmentAbilityData` 60). `CommandAbilityData`, `CommandedAbilityData`, `PossessedAbilityData` and `ItemExplorationSiteEventLog` are absent from these files, and the constructed tests cover them. Since PE-2c-3 12 `RaceResource` instances of `Release.hss` prove the two unit index lists of each race through the `UnitIndexList` adapter. No line names `PE-2c still needs a run-length list` or a dynamic member. Class id 80 is shared with `EngineObject`, so a created object is not named `AbilityList`, and these files hold no instance of that class. `Hero` and `Leader` children are in the files. Each declares `ReadWrite` and has no `PersistFields`, so the army that holds one is blocked before the bit array is read. A descendant that declares `ReadWrite` stays refused until it has `PersistFields`. `RaidAIGroup` (Delphi `TRaidAG`), `ScoutAIGroup` (Delphi `TScoutAG`) and the other behavior groups are absent while sibling AI groups are counted. Storm and animation sprites are absent from these file kinds. `SetupSettings` is a setup-screen node and is not a member of a map, campaign, hero list or save. `AIGroup` itself is zero because the files instantiate subclasses. The test skips when the install is absent. The game root is `InstalledSave.Root`.

`src/Formats.Tests/TreeTests.cs` loads `TreeSchema.json` (version 3) and `TreeSchemaV1.json`. Every value type round trips in schema order. Presence and fallbacks hold, including a non-zero fallback and each width's minimum and maximum. An unconsumed member, an unknown member and a duplicate member are refused with the path. `Has` does not consume. A list of 300 integers, including 0, round trips. A record that gains a field reads an older document with the fallback. Version 1 refuses a list and a record with the path, and writes a reference as a plain integer. Version 2 refuses a list on a non-array member and a record, with the path.

`src/Formats.Tests/ListAndSlotTests.cs` round-trips both list widths through the document and the original image on a test schema, with the canonical runs and the split of a byte run over 255 checked byte for byte, stores an empty list with its default, leaves an absent list and its default, and refuses an out-of-range item or default, a non-integer item, a missing `items`, an unknown member and an array with the path. It refuses a run past the stored count on the image. A keyed member round-trips its slots in stored order, the stored null included, through both backends. A load asks for slots in any order, gets null for an absent slot and a fresh object for a slot asked twice, and leaves an unasked slot unread only when the serializer skips the member. A slot stored twice or below zero is refused on save, and a duplicate, negative or missing `$slot`, a member on a stored null, a class outside the family and a member that is not an array are refused on load with the path. A dynamic member without a slot base stays refused with the old messages on both backends.

`src/Formats.Tests/SymbolicTests.cs` round trips names and lists through the original backend, migrates constructed version 1 hex and the golden hero list, and refuses an unknown name, a name of the wrong enum, a non-integer list item, and a list longer than the original count. Constructed version 1 documents place a reannotated member inside a child, a created object, a dynamic item, a map-field row and a nested campaign map, and each one migrates to the version 3 value. A constructed document with `$layout` migrates, and each fingerprint matches the rewritten values. The nested campaign map fingerprint uses profile `world-hsm`. Malformed hex, an integer where version 1 stores hex, a missing catalog entry and an unsupported width are refused with the path. The golden hero list stays a version 2 input.

`src/Formats.Tests/RecordShapeTests.cs` round-trips a catalog record, a bare set record, a bit array and a raw boolean through the document and the original image. It refuses a record field of the wrong type, an unknown record field, a nonsorted or duplicate bit index, a bit index at or beyond the count, and an out-of-range primitive, on the tree path and on the converter. A malformed `$reserved` is refused on both sides. A raw boolean of byte 02 round-trips through the persister. Every integer width the catalog accepts transfers, including `u32` maximum and `i16` minimum. An unsigned 32-bit record field at `0x80000000` and `0xFFFFFFFF` round-trips through `IntegerCodec`, the original image and the document. `RecordMigrationTests` places a version 2 hex member in a child, a created object, a dynamic item, a map-field row and a nested campaign map, and each one migrates to the version 3 value. A migrated document loads. The recomputed fingerprint is accepted. A fingerprint that is not 64 lowercase hex is refused with `Codes.Layout`. A fingerprint of 64 lowercase hex that is not the recomputed digest is refused under `--exact-layout` with `Codes.Layout`. The step's output is the same when `FormatSchema.Current` is replaced. That comparison covers a raw byte, a record with `$reserved`, a bit array with residue and a raw boolean.

`src/Formats.Tests/ListShapeTests.cs` decodes and encodes the version 4 lists through the real schema's codecs: a bit list with set indices, residue past the count and a default byte 2, enum lists of `DiplomaticRelation` and `MagicSphere` with an undeclared value as its integer, collections with trimmed groups, an inner -1, a count that is not a multiple of 8, a default other than -1 and no groups, a move path of map coordinates, and a unit index list. It refuses a bit index at the count, an unsorted index, a duplicate index, `$reserved` on a whole last byte or inside the count, hex bits and an unknown member, an unknown enum name in an item and in a default, a group of 9, a count that does not fit the groups, a last group longer than the count leaves, a non-integer value, a non-array group, missing groups, a reserved member and an unknown record field, each with its code and path. The version 3 step turns version 3 hex, a version 3 bit list and a sphere list into the version 4 shapes, keeps a list encoding the writer does not make as a `value-encoding` part that the export uses, refuses a value that is not hex, and gives the same output when `FormatSchema.Current` is replaced. Each of the 16 pinned properties is in the version 3 schema.

## 11. rwData census

The census is every `Bytes` and `TransferSet` site of the storage streams (Delphi `rwData` and `RwSet`) under `src/Game` and `src/Engine`, joined to the schema property of that class and id. 142 sites. 7 of them are stream callbacks or the campaign map bytes and have no `data` property. The other 135 match a schema property.

| shape | sites | what version 3 does |
|---|---:|---|
| enum byte | 60 | member name, or the integer when the enum has no member. All 60 are unsigned bytes. Unchanged from version 2 |
| set | 27 | names in ascending ordinal order, unnamed bits as integers. Width 1 except `Ability.SelectionTypes` (Delphi `TAbility`), `CombatData` (Delphi `TCombatData`) damage types and `WizardsTower.MagicSpheres` (Delphi `TWizardsTower`), which are 2 bytes. Unchanged from version 2 |
| fixed integer array | 5 | a JSON array of the original length. Width 4 except `ArmyView.UnitConcealment`, which is 8 unsigned words. Unchanged from version 2 |
| opaque integer or bool | 33 | a plain integer or boolean, present even when zero or false. 28 unsigned bytes, one signed byte (`AIGroup.IndependentRelation`), three signed 32-bit values (`RangedAttackCombatAction.AbilityID`, `RemotePlayer.RemoteID`, `WizardsTower.SpellID`) and one raw boolean (`UnitResource.Champion` (Delphi `TUnitResource`)). `City.WallType` (Delphi `TCity`), `CombatWall.WallType` (Delphi `TCombatWall`) and `CombatData.WallType` are bytes, not `WallType` (Delphi `TWallType`). A raw boolean other than 0 or 1 stays that integer, and the object keeps that byte |
| padded record | 5 | named fields. `Animation.Dimensions` (`Rect`, 16), `DamageCombatAction.Effects` and `DamageTypes` (`PackedDamageTypes`, 2, stored as the set), `PlayerSetupSettings.Behavior` and `OriginalBehavior` (`SetupPlayerBehavior`, 8, with `$reserved` for nonzero `Reserved`) |
| bit array | 2 sites, 1 property | ascending bit indices. `CustomAbilityList.AbilityCount` is the schema name of `AbilityBits`. `AbCount` is the separate count |
| variable byte array | 1 | stays hex. `FloodControl.OriginalTerrain` is a terrain map blob for PE-2c and remains a PE-1 `Bytes` pilot |
| mapped discriminator | 1 | `DiplomaticAction.ActionType`, the one-byte kind. The class stays on its case mapping |
| variable blob | 7 | PE-2c. Two `Campaign.Stream` (Delphi `TAoWCampaign`) sites are the campaign map member. The other five are the storage-stream callbacks (`StorageStream` (Delphi `TEStorageStream`), `StorageReader` (Delphi `TEReadStorageStream`), `StorageWriter` (Delphi `TEWriteStorageStream`), and Delphi `TCompareStorageStream` twice, which the port does not have). The direct path still stores those bytes as hex and refuses a decoded map or run |

Declared schema codecs after version 3: `data` 2, `record` 5, `bit-array` 1, `raw-bool` 1, plus `raw` true on 28 `u8`, 1 `i8` and 3 `i32`. `enum` 60, `set` 27, `int-array` 5, `stream` 17, `int-list` 17, `byte-list` 12 and `bit-list` 3 are unchanged. After version 4: `stream` 7 (the resource payloads), `int-list` 25 (3 with `groups`, 1 with `record`), `byte-list` 14 (5 with `enum`) and `bit-list` 3 (all with `indices`). The list and stream figures are declarations, not the inherited uses of the PE-1 census. `AIPlayerControl.DefenseStrengthMultiplier` (Delphi `TAIPlayerControl`) is `float32`, not `data`. The full site table of the earlier shapes is in the PE-2a status file. PE-2c takes the stream callbacks, the map blobs (`OriginalTerrain`, map fields, the campaign map) and the run-length lists. PE-3 takes mapped classes, dynamic members and the switch off `ReadWrite`. The C# record structs stay explicit-layout. `ReadWrite` still copies their bytes, and the catalog names the fields without changing the struct.
