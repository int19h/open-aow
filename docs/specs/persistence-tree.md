# Persistence tree (PE-1, PE-2a)

State: PE-1 is implemented for the pilot classes, and PE-2a adds version 2 of the own document,
symbolic enums and sets, fixed integer arrays as lists, and the original backend, 2026-11-07.
This is step 1 and the first part of step 2 of the exit from the Delphi image (`own-formats.md`
6.3). The game still loads and saves through `ReadWrite` and the converter. PE-3 is the switch.

The API lives in `src/Formats/Tree/`, namespace `Aow.Formats.Tree`. Formats does not reference
Units. An engine object takes part through `IPersistable`. Units supplies the factory and the
child collection.

## 1. The serializer

One method per class serves both directions, the same shape as `ReadWrite`. The field hook is
`PersistFields`. `IPersistable.Persist` on `TEObject` is explicit. It refuses a class the tree
cannot store, then calls `MainPersist`. On load, `MainPersist` calls `Clear` and then
`PersistFields`. `TECustomNode.MainPersist` does that and then persists `Children`.
`TEChangeNotifyNode.MainPersist` calls `BeginUpdate` before that load and `EndUpdate` after it,
the same bracket as `MainReadWrite`. `TUnitList.MainPersist` then calls `Release` on every child,
the same loop as `MainReadWrite`. `ReadWrite` is unchanged and remains the game path.

The refusal walks the type and its bases. A type that declares `MainReadWrite` or
`ReadWriteChildren` and does not declare `MainPersist` produces
`Class <name> overrides MainReadWrite or ReadWriteChildren and has no MainPersist.`
A type that declares `ReadWrite` and does not declare `PersistFields` produces
`Class <name> overrides ReadWrite and has no PersistFields.`
`PersistRefusalTests` pins every product engine class in either state. `TEnchantment`,
`TEChangeNotifyNode`, `TUnitList` and `TArmyView` are not in that set. An army that owns a unit
refuses through `TUnit`.

```csharp
public interface IPersistable
{
    void Persist(IPersister persister);
}

public interface IPersistableFactory
{
    string ClassName(IPersistable obj);
    IPersistable Create(string className);
}

public interface IPersister
{
    bool Loading { get; }
    IPersistableFactory Factory { get; }
    bool Has(string name);
    void Int(string name, ref int value, int fallback = 0);
    void Byte(string name, ref byte value, byte fallback = 0);
    void SByte(string name, ref sbyte value, sbyte fallback = 0);
    void UShort(string name, ref ushort value, ushort fallback = 0);
    void Enum<TEnum>(string name, ref TEnum value, TEnum fallback = default) where TEnum : struct, Enum;
    void Set<E>(string name, ref TSet<E> value) where E : unmanaged, Enum;
    void Bool(string name, ref bool value);
    void String(string name, ref string value);
    void Lines(string name, IList<string> lines);
    void LinesWithObjects(string name, IList<string> lines, IList<int> objects);
    void Bytes(string name, Span<byte> data);
    void Object(string name, IPersistable obj);
    void Created(string name, ref IPersistable? obj);
    void Created<T>(string name, ref T? obj) where T : class, IPersistable;
    void Children(string name, IChildCollection children);
    void Reference(string name, string domain, ref int key, int fallback = 0);
    void Record(string name, Action<IPersister> body);
    void List<T>(string name, IList<T> items, ItemTransfer<T> transfer);
}
```

`Has` reports presence on load and does not consume the member. On save it is false. A second
call of the same name on save is an error (`Codes.Duplicate`) with the path. After `Persist`
returns, a member the serializer did not consume is an error (`Codes.Required`) with the path.

`IChildCollection` is the stored children of one node: count, the child at an index, whether
that child is stored, its slot, whether the node keeps indexes, `Attach` before the child's
`Persist`, and `Place` after it. The engine implementation is `NodeChildren` in
`src/Units/PersistSupport.cs`. A stored child has this node as owner, has `csStored`, and does
not have `csShared`. The slot is the child's index. `Place` clears the owner set by `Attach`
and then calls `SetArrayOwner` when the node keeps indexes, otherwise `SetOwner`.

`IItemSlot` transfers one list item: `Int`, `Byte`, `SByte`, `UShort`, `Bool`, `String`,
`Record`, and `Object`. The call stores exactly one value and takes no fallback. `ItemTransfer<T>` is the delegate
`List` takes. A lambda cannot capture a `ref` local or parameter, so a transfer is usually an
instance method or a static method. An explicitly typed lambda also works.

Floating-point fields, `rwStreamProc` payloads, map fields, dynamic sprites, the campaign map
stream, and the list codecs `int-list`, `bit-list` and `byte-list` have no member on
`IPersister`.

## 2. The tree

`TreeWriter.Write` builds an `ObjectNode` with no class name and no slot and calls
`IPersistable.Persist`. `TreeReader.Read` calls `Persist` and then checks that every member was
consumed. The tree knows no schema.

| type | stores |
|---|---|
| `IntegerNode` | width, signed, value |
| `EnumNode` | C# enum type name, width, signed, integer value |
| `SetNode` | element enum type name, width, little-endian mask |
| `BoolNode` | value |
| `StringNode` | text |
| `LinesNode` | line list, or raw text when the block is not an array of lines |
| `LinesExNode` | the same text plus one signed 32-bit object value per line |
| `BytesNode` | exact bytes |
| `ObjectNode` | optional class name, optional slot, ordered named members |
| `NullNode` | the stored null of `rwCreateEObject` |
| `ChildListNode` | child object nodes |
| `RecordNode` | ordered named fields, no class and no slot |
| `ListNode` | a variable-length list of values |
| `ReferenceNode` | domain and integer key |

`ObjectNode` members stay in call order. The document backend reorders them. `ObjectNode.References(domain)` walks every `ReferenceNode` of that domain and yields the path and the key.

## 3. Presence and fallbacks

Presence follows `persistence.md` 1.4.

| call | omitted on save | absent on load |
|---|---|---|
| `Int`, `Byte`, `SByte`, `UShort`, `Reference` | value equals the fallback | the fallback is assigned |
| `Bool` | false | false |
| `String` | empty string | empty string |
| `Lines`, `LinesWithObjects`, `List` | empty | the list is cleared |
| `Bytes` | length zero | the caller's span is left as it is |
| `Enum` | never, including zero | the fallback |
| `Set` | never, including an empty mask | the caller's value is left as it is |
| `Object` | never | `Persist` runs against an empty object node |
| `Created` | never. Null is stored as JSON null | null |
| `Children` | never. An empty child list is stored | no children are placed |
| `Record` | never. The call always stores the record | the body runs against an empty record |

A named field inside a record follows the same row. A list item does not. Inside a list, false and integer 0 are stored. The caller sizes a `Bytes` span before the call. A length that differs from the stored bytes is `Codes.Range`. A fixed `data` size in the schema, including a single zero byte, is stored. Only length zero is absence.

An explicit false in a document is read as absence. An explicit integer equal to the fallback is applied and then omitted on the next save. The tree follows the engine writer. It does not keep a hand-edited default that the converter leaves in the JSON. Equivalence is against that engine rewrite.

`String` and line text go through code page 1252. A line list whose block is an array of lines (each line ends in CR LF, and no line holds CR, LF or NUL) is a JSON array under `lines` for `rwStringsEx` and a JSON array for `rwStrings`. Any other block is `{ "$text": ... }`, with `objects` beside it for `rwStringsEx`. On load, noncanonical text is split the way `TStrings.SetTextStr` splits it (CR, LF, or CR LF, and NUL ends the text). The raw line is not preserved. Both sides of an equivalence check pass through that split.

`Object` on load persists into the object the caller already holds. It does not replace that object. A missing member still runs `Persist` on an empty node, which is what `MainReadWrite` does with an empty property table.

`Created<T>` boxes through the non-generic call and requires the runtime class to be `T` (`Codes.ClassFamily`). A non-null created object needs the factory. Absent means null. JSON null is the stored null. `{ "$class": null }` is refused.

## 4. Identity and references

A created object and a child carry a class name. A child also carries `$slot`, a non-negative integer. A fixed object carries neither. A cross reference is an integer key plus a domain name. Version 1 writes the key as a plain integer and records no domain in the document. The tree keeps the domain on `ReferenceNode`. `Reference` on load accepts a `ReferenceNode` whose domain matches, or an `IntegerNode` of signed 32-bit width. PE-1 does not validate keys. The domain the game checks today is `player-slot` (`own-formats.md` 2.7). No pilot uses `Reference`. The test schema proves it.

## 5. Records and lists

`Record` is a group of named fields with no identity and no class. `List` is a variable-length sequence of values, records or objects. Version 1 has neither shape, so the version 1 backend refuses both, before it reports an unknown member. The message is `Version 1 does not store a list.` or `Version 1 does not store a record.`, with the document path. `ChildListNode` is not a `ListNode` and version 1 stores it.

Version 2 stores a list only for a fixed integer array. Any other list, and every record, is refused with the path. The list message is `Version 2 stores a list only for a fixed integer array.` The record message is `Version 2 does not store a record.` A generic list is stored when the schema sets `lists` true. The test schema sets that flag, and its records round trip because the schema version is 3. Product code does not name a later version to decide that a list is stored.

A record field that a new serializer reads and an older document omits takes that field's fallback. Document field order is the schema field order, not the call order. Fallbacks are arguments of the serializer, not schema data.

The parser accepts `list`, `record`, and an optional domain on an `i32` so a test schema and a later version can name them. Production records stay refused until PE-2b.

## 6. Direct document backend

```csharp
public static class DocumentBackend
{
    public static JsonObject Write(ObjectNode node, SchemaClass cls, FormatSchema schema, string engine, DocPath? path = null);
    public static ObjectNode Read(JsonObject document, SchemaClass cls, FormatSchema schema, string engine, DocPath? path = null);
}
```

`Write` and `Read` use `FormatSchema` for names, order and codecs. There is no engine image and no property table on this path. Member order is schema order. Reserved members keep the order in `own-formats.md` 2.2. `$class` is written for a created object and a child, and refused on a fixed object. `$slot` is written for a child and refused otherwise. `$layout` is ignored on read and is not written. Duplicate JSON members are refused.

The backend refuses a class or member the schema does not name, a value whose type does not fit the member's codec, a dynamic member (`PE-1 does not store a dynamic member.`), a list or record the version does not store, and a class with a case mapping. The mapping message is `Class <name> uses a case mapping. The document backend does not reproduce it.` with the path. `TDiplomaticAction` and `TProductionItem` are the mapped classes, and the pilot census excludes them. Each error carries the document path (`root`, then `.name` or `[index]`).

Version 1 requires a created or child class to have an id that resolves to that class in the named engine table. A later schema version requires the class to exist and, when a family is set, to inherit that family. It does not require engine enlistment. A fixed root is the class the caller passes and is not looked up that way, so a class with no engine row can still be a fixed root (`TMessageEventLog`, `TFloodControl`).

Each document value rule is `WriteTree` and `ReadTree` on the codec. There is no switch on a kind. The frozen version 1 to 2 step is the exception. It switches on the codec type of its pinned version 2 schema, so a later version leaves that step alone. The converter and the backend share one implementation of each rule that already lived in the converter:

- `TextCodec.Problem` checks code page 1252 and the string length limit.
- `HexText.IsLowerHex` and `HexText.Decode` check and decode lowercase hex.
- `StringsCodec.Join`, `Lines`, `LineCount`, `TextValue` and `TextOf` own the `rwStrings` block rule.
- `LayoutResidue.Hex` formats `rwData`.

Version 2 keeps the version 1 classes and properties. An enum property is `codec` `enum` with `enum` naming the C# type, plus `width` and `signed`. A set property is `codec` `set` with `set` naming the element type and `width`. A fixed integer array is `codec` `int-array` with `width`, `signed` and `count`. `PropertyCodec.Create` accepts widths 1, 2 and 4 for those three codecs and refuses any other width with the schema path. `versions.json` current is 2. Version 1 keeps its historical hash.

The migration `1-symbolic-enum-set-array` rewrites version 1 hex of those properties into names and lists. It then recomputes every `$layout` fingerprint, because the fingerprint covers the visible values, and it leaves the parts in place. A nested campaign map keeps its own profile on that fingerprint. Every other value stays in place. The step walks the pinned version 2 schema embedded as `schema-v2.json`, not `FormatSchema.Current`. The version 1 schema of main `a7eb5da` is embedded as `schema-v1.json` and is the schema a version 1 import uses. A value that is not lowercase hex where version 1 stores hex, including an integer, and a catalog miss, raise `ConversionException` with the document path and `Codes.Type`. A hex length the version 2 member does not store raises `ConversionException` with the path and `Codes.Range`.

Formats does not reference Units, so it does not hold a name table. `EnumCatalog.Register` and `EnumCatalog.RegisterReflection` take catalogs. `ClassRegistry`'s module initializer registers the units assembly by reflection, one registration for every enum in that assembly, and the converter's `UnitCatalog.Register` calls the same method. That call does not run the units module initializer. `Program` runs it with `RuntimeHelpers.RunModuleConstructor` before a conversion. A second registration of the same assembly is ignored. Two catalogs that both name one type fail at the lookup. Formats tests register `TSampleRank` and `TOtherRank` beside that. The tree itself does not use the catalog. `Enum` and `Set` carry `typeof` at the call. The catalog is for the converter, the migration and the backends, where no object exists. `EnumCodec` and `SetCodec` keep the catalog entry after the first successful lookup.

An enum value is the C# member name from `Enum.GetName`, exactly as declared. A value with no member is stored as its integer, so a byte the enum does not declare is not dropped. Strict reading refuses a name the enum does not have, including a name of a different enum. A set is a JSON array of member names in ascending ordinal order, and an unnamed bit is its integer. Duplicate bits and a bit outside the width are refused. A fixed integer array is a JSON array of integers in index order. Trailing zeros stay. The tree and the own format accept a list longer than `count`. Export to the original format refuses it with the path and does not truncate. A shorter list fills the rest with zero on export.

```csharp
public static class OriginalBackend
{
    public static byte[] Write(ObjectNode node, SchemaClass cls, FormatSchema schema, string engine);
    public static ObjectNode Read(byte[] image, SchemaClass cls, FormatSchema schema, string engine);
}
```

`OriginalBackend` is an interim composition. `Write` refuses a mapped class, asks `DocumentBackend` for a JSON object, and encodes that object with `ObjectImage.Encode`. `Read` decodes the image into a JSON object and asks `DocumentBackend` to build the tree. The backend can write only what the own document version stores. A mapped class, a dynamic member, a record and a list that is not a fixed integer array are refused. Each call allocates a JSON tree for the object. `DocumentBackend` drops `$layout` on read and does not write it. Direct tree-to-image virtuals on the codecs are PE-2b or PE-3 work. There is no `ReadWrite` on this path. The mapped-class message is `The original backend does not reproduce it.` The serializers name members only. They do not mention an id or a width.

`PilotImage.Decode` and `PilotImage.Encode` (`src/Formats/Original/PilotImage.cs`) are internal. They decode and encode one object image under a class through the converter. Tests use them. They are not public API.

## 7. Factory

`EnginePersistableFactory` in `src/Units/PersistSupport.cs` implements `IPersistableFactory`. It indexes `EClass.All` by `ClassName` once. Two classes with the same name throw. `Create` calls `EClass.Create(null)`. `ClassName` uses `TObject.ClassNameOf`, which is the type name with a leading `@` removed. The catalog is every class `ClassRegistry` registers, including classes with no engine enlistment.

`PersistSupport` also moves the engine shapes the serializer cannot name directly. `UnsignedByte` stores an `sbyte` through `Byte` (the source byte of `rwByte`, including 255 for -1). `Int32Array` and `UInt16Array` copy a fixed array into a list, call `List`, and on load write the list back, filling a short list with zero and refusing a list longer than the array. `Lines` and `LinesEx` copy a `TStrings` into lists, call the persister, and copy back only on load. `ObjectToInt(null)` is 0 and `IntToObject(0)` is null.

## 8. What PE-2 to PE-5 add

PE-2a, this document's version 2, puts enum bytes, sets and fixed integer arrays behind the original backend and stores them as names and lists. PE-2b does the same for padded records and bit arrays. PE-2c does it for the `stream` callbacks, map blobs and run-length lists. The record and list members stay the target for those shapes. Until a later brief adds direct tree-to-image virtuals, the original backend stays the composition in section 6.

PE-3 rewrites each `ReadWrite` into `Persist` and moves own load and save onto `DocumentBackend`. Historical document decoders and original import and export stay. Until that brief, no game load or save uses this path.

PE-4 separates token-event transport from disk persistence. Packet widths stay on the legacy adapters until networking changes them.

PE-5 proves extended arrays and richer cases, lossless own reload, strict refusal of unrepresentable exports, historical migrations, and the G1, G2 and G-own gate, and checks that no Delphi image remains on the own disk path or in the stable mod interface.

## 9. Pilot census

A class is eligible when every effective property (`SchemaClass.Complete`) uses a PE-1 codec, and the same holds for every fixed-object class it embeds and every class in a created family it names. The PE-1 codecs are `i32`, `u8`, `i8`, `u16`, `bool`, `string`, `strings`, `strings-ex`, `data`, `object`, `created` and `children`. An unbounded created member fails eligibility. A children member is a PE-1 codec and is not expanded to every class the engine can attach, because the schema names no child family. Treating every engine class as a possible child leaves no eligible class that uses children.

The schema has 970 classes. 582 use only PE-1 codecs on their own effective properties. 554 stay eligible after the object and created closure. Among those 554, the classes that use each codec are: `i32` 273, `u8` 224, `data` 138, `children` 117, `string` 55, `bool` 27, `object` 27, `strings` 18, `i8` 7, `u16` 6, `created` 6, `strings-ex` 1. Across every class, effective uses outside that set include `stream` 584, `int-list` 39, `bit-list` 27, `byte-list` 15, `map-fields` 3, `campaign-map` 2 and `float32` 1. Those counts are the PE-1 census. Version 2 reannotates 92 declared `data` properties as `enum` (60), `set` (27) and `int-array` (5). The extended eligibility rule treats those three codecs as closed and still excludes a class with a case mapping.

Created families that are fully eligible are `TAnimation` (13), `TDiplomaticAction` (1), `TGlobalEnchantment` (16), `TProductionItem` (1) and `TUnitEnchantment` (2, itself and `TCosmeticSurgeryEnchantment`). Mapped classes and `TAnimation` are not piloted. The global-enchantment pilot is the exact class plus the nested `TFloodControl`, not every subclass.

`TItemLibrary` and `THeroLibrary` are eligible as classes (`children`, `i32`, `string`). The files are not a closed PE-1 graph: their children are `TItem` and `THero`, which use `int-list`. They are not piloted. No eligible class is a corpus-safe home for a non-zero `i32`, `u8` or `u16` fallback. `rwByteEx` is only on ineligible `TStructure` and `TLeader`. `rwWordEx` is never called. The test schema proves those fallbacks.

The pilots, and the codecs they add:

| classes | codecs |
|---|---|
| `TMessageEventLog`, `TEventLog` | `strings`, `string`, `enum` (`Layout`, `Buttons`), `i32`, `u8` |
| `TEResourceEditSettings`, `TEControlStyleObject` | `strings-ex`, `set` (`ControlStyle`, including a zero mask) |
| `TItemGFX`, `TEResource` | `bool`, `string`, `i32`, `enum` (`ItemType`) |
| `TAIBudgetManager`, `TAIBudgetList`, `TAIBudget` and the budgets that do not override `ReadWrite` (`TAISavingsBudget`, `TAITransporterBudget`, `TAICityUpgradeBudget`, `TAICityResearchBudget`, `TAICityFortifyBudget`) | `object`, `children`, `i32` with fallback 0 |
| `TUnitEnchantmentAbilityData`, `TUnitEnchantment`, `TEnchantment`, `TCosmeticSurgeryEnchantment` | `created` (present and null), `u16` fallback 0, `i8` fallback -1, `i32`, `u8` |
| `TFrozenWaterHS` | `i8` fallback 0 |
| `TFloodEnchantment`, `TGlobalEnchantment`, `TFloodControl` | variable `data`, embedded `object`, `bool`, `i8` fallback -1, `u8`, `i32`, `string`, `enum` (`EnchantmentType`) |
| `TDiplomaticRelationHistoryItem` | `enum` (`TDiplomaticRelation`), `i32` |
| `TChangeGlobalMagicTE`, `TTokenEvent` | `enum` (`TChangeGlobalMagicMode`), `u8`, `i32` (`TokenCounter` included) |
| `TUnitSpellCA` | `int-array` width 4 count 2, `i32`, `u8` |
| `TGlobalMagicControl`, `TCityEnchantment`, `THatredEnchantment`, `THarmonyEnchantment` | `int-array` width 4 count 9, `object`, `enum` (`TCityEnchantmentID`), `i32` |
| `TSingleTargetCA` | `set` (`TSingleTargetModeElement`), `u8` |
| `TArmy`, `TArmyView` (constructed only) | `set`, `enum` (`TGroupBehavior`), `int-array` width 2 count 8 and width 4 count 8, `u8`, `u16` |

Constructor defaults are not stream fallbacks. `TEnchantment` constructs dispel resistance 10, upkeep 1 and source -1, and those values are stored. Stream fallback 0 omits only a real 0. `TUnitEnchantmentAbilityData` constructs player and duration as -1, which matches the `rwShortintEx` fallback, so those members are omitted until they change. The class refuses the tree through `TAbilityData`, which overrides `ReadWrite` and has no `PersistFields`. `TEControlStyleObject` constructs the set bit `csSaveLoad`. The const alias `csStored` is not an enum member, so the document stores `csSaveLoad` and the image byte stays `01`. `TArmyView` is not a corpus pilot. The only width-2 array sits on it, and its child closure is the unit tree. `TArmy.PersistFields` mirrors `ReadWrite`. On a save with connection type 1 it omits the three destinations and `UserMode`, and on load it reads those members when they are present and clears `amMoving` from `Mode`. Corpus proof skips `TArmy` and `TArmyView`. A constructed test covers connection type 1, a load that clears `amMoving`, and an army that owns a unit, which refuses through `TUnit`. The army tests install a map marked `msDestroying` so `TArmy.Destroy` does not touch a null map.

## 10. Proof

`src/Units.Tests/PersistEquivalenceTests.cs` builds every pilot, old and new, at zero, default, nondefault and boundary values. It covers each stored enum's declared members, an undeclared byte, an empty set, a full set, and fixed arrays at zero, mixed and boundary values. `Persist` through the tree writer and `DocumentBackend` matches `PilotImage.Decode` of the image `MainReadWrite` writes. The same tree through `OriginalBackend` matches that image byte for byte. The same JSON, read through `DocumentBackend` and the tree reader into a fresh factory object, then `MainReadWrite`, matches `PilotImage.Encode` of that JSON. An image read through `OriginalBackend` and the tree reader, then `MainReadWrite`, matches the image. The same original image loaded through `MainReadWrite` into a fresh object, then written with `MainReadWrite`, matches that rewrite. An overlong `SpellData` list is kept by the document backend and refused by `OriginalBackend` at `root.SpellData`. `TUnitEnchantmentAbilityData` is not in that equality. Its write throws `Class TAbilityData overrides ReadWrite and has no PersistFields.`

`src/Units.Tests/PersistCorpusTests.cs` walks every instance of those exact classes in the corpus files the converter reads, when `AOW_GAME_ROOT` (or the default install) has `Release/Release.hss` and `Scenario/Custom`. `TArmy` and `TArmyView` are not in that list. The comparison is the converter equivalence and the original-backend equivalence, against the engine rewrite of each instance, not against the imported document, because an import can carry `$layout`, an explicit default, or a non-dense slot. The same original image is loaded through `Persist` and through `MainReadWrite`, and the two `MainReadWrite` images are equal. An instance whose closure refuses the tree throws that refusal. The test checks the message against `TreePersistenceRefusal` and still counts the instance. Every import is version 2. Each corpus file is also imported with the frozen version 1 schema of main `a7eb5da` and then migrated, and that document equals the direct version 2 import. The step recomputes `$layout` fingerprints, so that comparison covers them. The test asserts `TAIBudgetManager`, `TUnitEnchantmentAbilityData`, `TEResourceEditSettings` and `TGlobalMagicControl` are found, and it prints the count of every pilot class. The test skips when the install is absent. The game root is `InstalledSave.Root`.

`src/Formats.Tests/TreeTests.cs` loads `TreeSchema.json` (version 3) and `TreeSchemaV1.json`. Every value type round trips in schema order. Presence and fallbacks hold, including a non-zero fallback and each width's minimum and maximum. An unconsumed member, an unknown member and a duplicate member are refused with the path. `Has` does not consume. A list of 300 integers, including 0, round trips. A record that gains a field reads an older document with the fallback. Version 1 refuses a list and a record with the path, and writes a reference as a plain integer. Version 2 refuses a list on a non-array member and a record, with the path.

`src/Formats.Tests/SymbolicTests.cs` round trips names and lists through the original backend, migrates constructed version 1 hex and the golden hero list, and refuses an unknown name, a name of the wrong enum, a non-integer list item, and a list longer than the original count. Constructed version 1 documents place a reannotated member inside a child, a created object, a dynamic item, a map-field row and a nested campaign map, and each one migrates to the version 2 value. A constructed document with `$layout` migrates, and each fingerprint matches the rewritten values. The nested campaign map fingerprint uses profile `world-hsm`. Malformed hex, an integer where version 1 stores hex, a missing catalog entry and an unsupported width are refused with the path.

## 11. rwData census

The census is every `rwData` and `RwSet` site under `src/Units`, joined to the schema property of that class and id. 142 sites. 7 of them are stream callbacks or the campaign map bytes and have no `data` property. The other 135 match a schema property.

| shape | sites | what version 2 does |
|---|---:|---|
| enum byte | 60 | member name, or the integer when the enum has no member. All 60 are unsigned bytes |
| set | 27 | names in ascending ordinal order, unnamed bits as integers. Width 1 except `TAbility.SelectionTypes`, `TCombatData` damage types and `TWizardsTower.MagicSpheres`, which are 2 bytes |
| fixed integer array | 5 | a JSON array of the original length. Width 4 except `TArmyView.UnitConcealment`, which is 8 unsigned words |
| opaque integer or bool | 33 | stays lowercase hex. The field type is `byte`, `sbyte`, `int` or `bool`, not an enum. `TCity.WallType`, `TCombatWall.WallType` and `TCombatData.WallType` are bytes, not `TWallType` |
| padded record | 5 | stays hex until PE-2b. `TAnimation.Dimensions` (`TRect`, 16), `TDamageCA.Effects` and `DamageTypes` (`TP3PackedDamageTypes`, 2), `TPlayerSetupSettings.Behavior` and `OriginalBehavior` (`TSetupPlayerBehavior`, 8) |
| bit array | 2 sites, 1 property | stays hex until PE-2b. `TCustomAbilityList.AbilityCount` is the schema name of `FAbilityBits` |
| variable byte array | 1 | stays hex. `TFloodControl.OriginalTerrain` is already a PE-1 `Bytes` pilot |
| mapped discriminator | 1 | `TDiplomaticAction.ActionType`, the one-byte kind. The class stays on its case mapping |
| variable blob | 7 | PE-2c. Two `TAoWCampaign.FStream` sites are the campaign map member. The other five are the storage-stream callbacks (`TEStorageStream`, `TEReadStorageStream`, `TEWriteStorageStream`, and `TCompareStorageStream` twice) |

Declared schema codecs after the reannotation: `data` 41, `enum` 60, `set` 27, `int-array` 5, `stream` 17, `int-list` 17, `byte-list` 12, `bit-list` 3. The `stream`, `int-list`, `byte-list` and `bit-list` figures are declarations, not the inherited uses of the PE-1 census. `TAIPlayerControl.DefenseStrengthMultiplier` is `float32`, not `data`. The full site table is in the PE-2a status file. PE-2b takes the padded records and the bit array. PE-2c takes the stream callbacks, the map blobs and the run-length lists.
