# Persistence tree (PE-1)

State: implemented for the pilot classes, 2026-11-07. This is step 1 of the exit from the
Delphi image (`own-formats.md` 6.3). The game still loads and saves through `ReadWrite` and the
converter. PE-3 is the switch.

The API lives in `src/Formats/Tree/`, namespace `Aow.Formats.Tree`. Formats does not reference
Units. An engine object takes part through `IPersistable`. Units supplies the factory and the
child collection.

## 1. The serializer

One method per class serves both directions, the same shape as `ReadWrite`. `TEObject.Persist`
is empty. `IPersistable.Persist` on `TEObject` is explicit and calls `MainPersist`. On load,
`MainPersist` calls `Clear` and then `Persist`. `TECustomNode.MainPersist` does that and then
persists `Children`. `ReadWrite` is unchanged and remains the game path.

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
`Record`, and `Object`. The call stores exactly one value. `ItemTransfer<T>` is the delegate
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

`Record` is a group of named fields with no identity and no class. `List` is a variable-length sequence of values, records or objects. Version 1 has neither shape (`own-formats.md` 2.3 stores every `rwData` as hex), so the version 1 backend refuses both, before it reports an unknown member. The message is `Version 1 does not store a list.` or `Version 1 does not store a record.`, with the document path. `ChildListNode` is not a `ListNode` and version 1 stores it.

A record field that a new serializer reads and an older document omits takes that field's fallback. Document field order is the schema field order, not the call order. Fallbacks are arguments of the serializer, not schema data.

These shapes exist so PE-2 and later schema versions have a target. The version 1 schema file is unchanged. The parser accepts `list`, `record`, and an optional domain on an `i32` so a test schema and a later version can name them.

## 6. Direct document backend

```csharp
public static class DocumentBackend
{
    public static JsonObject Write(ObjectNode node, SchemaClass cls, FormatSchema schema, string engine, DocPath? path = null);
    public static ObjectNode Read(JsonObject document, SchemaClass cls, FormatSchema schema, string engine, DocPath? path = null);
}
```

`Write` and `Read` use `FormatSchema` for names, order and codecs. There is no engine image and no property table on this path. Member order is schema order. Reserved members keep the order in `own-formats.md` 2.2. `$class` is written for a created object and a child, and refused on a fixed object. `$slot` is written for a child and refused otherwise. `$layout` is ignored on read and is not written. Duplicate JSON members are refused.

The backend refuses a class or member the schema does not name, a value whose type does not fit the member's codec, a dynamic member (`PE-1 does not store a dynamic member.`), and a list or record under version 1. Each error carries the document path (`root`, then `.name` or `[index]`).

Version 1 requires a created or child class to have an id that resolves to that class in the named engine table. A later schema version requires the class to exist and, when a family is set, to inherit that family. It does not require engine enlistment. A fixed root is the class the caller passes and is not looked up that way, so a class with no engine row can still be a fixed root (`TMessageEventLog`, `TFloodControl`).

Each document value rule is `WriteTree` and `ReadTree` on the codec. There is no switch on a kind. The converter and the backend share one implementation of each rule that already lived in the converter:

- `TextCodec.Problem` checks code page 1252 and the string length limit.
- `HexText.IsLowerHex` and `HexText.Decode` check and decode lowercase hex.
- `StringsCodec.Join`, `Lines`, `LineCount`, `TextValue` and `TextOf` own the `rwStrings` block rule.
- `LayoutResidue.Hex` formats `rwData`.

The converter's output is unchanged. `schema.json` and `versions.json` are unchanged.

`PilotImage.Decode` and `PilotImage.Encode` (`src/Formats/Original/PilotImage.cs`) are internal. They decode and encode one object image under a class through the converter. Tests use them. They are not public API.

## 7. Factory

`EnginePersistableFactory` in `src/Units/PersistSupport.cs` implements `IPersistableFactory`. It indexes `EClass.All` by `ClassName` once. Two classes with the same name throw. `Create` calls `EClass.Create(null)`. `ClassName` uses `TObject.ClassNameOf`, which is the type name with a leading `@` removed. The catalog is every class `ClassRegistry` registers, including classes with no engine enlistment.

`PersistSupport` also moves the engine shapes the serializer cannot name directly. `UnsignedByte` stores an `sbyte` through `Byte` (the source byte of `rwByte`, including 255 for -1). `Set` stores the low byte of a `TSet` image and, on load, rebuilds the set without changing the upper bits. `ByteImage` does the same for one unmanaged byte (an enum or `TItemType`). `Lines` and `LinesEx` copy a `TStrings` into lists, call the persister, and copy back only on load. `ObjectToInt(null)` is 0 and `IntToObject(0)` is null.

## 8. What PE-2 to PE-5 add

PE-2 puts original ids, widths, raw layouts, order and compression behind legacy codec adapters over this tree, and replaces raw persisted record copies with explicit field serialization. The record and list members are the target for that explicit form.

PE-3 rewrites each `ReadWrite` into `Persist` and moves own load and save onto `DocumentBackend`. Historical document decoders and original import and export stay. Until that brief, no game load or save uses this path.

PE-4 separates token-event transport from disk persistence. Packet widths stay on the legacy adapters until networking changes them.

PE-5 proves extended arrays and richer cases, lossless own reload, strict refusal of unrepresentable exports, historical migrations, and the G1, G2 and G-own gate, and checks that no Delphi image remains on the own disk path or in the stable mod interface.

## 9. Pilot census

A class is eligible when every effective property (`SchemaClass.Complete`) uses a PE-1 codec, and the same holds for every fixed-object class it embeds and every class in a created family it names. The PE-1 codecs are `i32`, `u8`, `i8`, `u16`, `bool`, `string`, `strings`, `strings-ex`, `data`, `object`, `created` and `children`. An unbounded created member fails eligibility. A children member is a PE-1 codec and is not expanded to every class the engine can attach, because the schema names no child family. Treating every engine class as a possible child leaves no eligible class that uses children.

The schema has 970 classes. 582 use only PE-1 codecs on their own effective properties. 554 stay eligible after the object and created closure. Among those 554, the classes that use each codec are: `i32` 273, `u8` 224, `data` 138, `children` 117, `string` 55, `bool` 27, `object` 27, `strings` 18, `i8` 7, `u16` 6, `created` 6, `strings-ex` 1. Across every class, effective uses outside that set include `stream` 584, `int-list` 39, `bit-list` 27, `byte-list` 15, `map-fields` 3, `campaign-map` 2 and `float32` 1.

Created families that are fully eligible are `TAnimation` (13), `TDiplomaticAction` (1), `TGlobalEnchantment` (16), `TProductionItem` (1) and `TUnitEnchantment` (2, itself and `TCosmeticSurgeryEnchantment`). Mapped classes and `TAnimation` are not piloted. The global-enchantment pilot is the exact class plus the nested `TFloodControl`, not every subclass.

`TItemLibrary` and `THeroLibrary` are eligible as classes (`children`, `i32`, `string`). The files are not a closed PE-1 graph: their children are `TItem` and `THero`, which use `int-list`. They are not piloted. No eligible class is a corpus-safe home for a non-zero `i32`, `u8` or `u16` fallback. `rwByteEx` is only on ineligible `TStructure` and `TLeader`. `rwWordEx` is never called. The test schema proves those fallbacks.

The pilots, and the codecs they add:

| classes | codecs |
|---|---|
| `TMessageEventLog`, `TEventLog` | `strings`, `string`, `data` (size 1), `i32`, `u8` |
| `TEResourceEditSettings`, `TEControlStyleObject` | `strings-ex`, `data` (size 1, including a zero byte) |
| `TItemGFX`, `TEResource` | `bool`, `string`, `i32`, `data` |
| `TAIBudgetManager`, `TAIBudgetList`, `TAIBudget` and the budgets that do not override `ReadWrite` (`TAISavingsBudget`, `TAITransporterBudget`, `TAICityUpgradeBudget`, `TAICityResearchBudget`, `TAICityFortifyBudget`) | `object`, `children`, `i32` with fallback 0 |
| `TUnitEnchantmentAbilityData`, `TUnitEnchantment`, `TEnchantment`, `TCosmeticSurgeryEnchantment` | `created` (present and null), `u16` fallback 0, `i8` fallback -1, `i32`, `u8` |
| `TFrozenWaterHS` | `i8` fallback 0 |
| `TFloodEnchantment`, `TGlobalEnchantment`, `TFloodControl` | variable `data`, embedded `object`, `bool`, `i8` fallback -1, `u8`, `i32`, `string`, `data` size 1 |

Constructor defaults are not stream fallbacks. `TEnchantment` constructs dispel resistance 10, upkeep 1 and source -1, and those values are stored. Stream fallback 0 omits only a real 0. `TUnitEnchantmentAbilityData` constructs player and duration as -1, which matches the `rwShortintEx` fallback, so those members are omitted until they change. `TEControlStyleObject` constructs `csStored`, whose image byte is `01`.

## 10. Proof

`src/Units.Tests/PersistEquivalenceTests.cs` builds pilot objects at zero, default, nondefault and boundary values. `Persist` through the tree writer and `DocumentBackend` matches `PilotImage.Decode` of the image `MainReadWrite` writes, byte for byte through `JsonWriter.ToUtf8`. The same JSON, read through `DocumentBackend` and the tree reader into a fresh factory object, then `MainReadWrite`, matches `PilotImage.Encode` of that JSON.

`src/Units.Tests/PersistCorpusTests.cs` walks every instance of those exact classes in the corpus files the converter reads, when `AOW_GAME_ROOT` (or the default install) has `Release/Release.hss` and `Scenario/Custom`. The comparison is against the engine rewrite of each instance, not against the imported document, because an import can carry `$layout`, an explicit default, or a non-dense slot. The test skips when the install is absent.

`src/Formats.Tests/TreeTests.cs` loads `TreeSchema.json` (version 2) and `TreeSchemaV1.json`. Every value type round trips in schema order. Presence and fallbacks hold, including a non-zero fallback and each width's minimum and maximum. An unconsumed member, an unknown member and a duplicate member are refused with the path. `Has` does not consume. A list of 300 integers, including 0, round trips. A record that gains a field reads an older document with the fallback. Version 1 refuses a list and a record with the path, and writes a reference as a plain integer.
