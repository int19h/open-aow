using System.Text;
using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Original;

/// <summary>Lowercase hexadecimal, shared by the converter and the document backend.</summary>
internal static class HexText
{
    public static bool IsLowerHex(string text)
        => text.Length % 2 == 0 && text.All(static character => character is (>= '0' and <= '9') or (>= 'a' and <= 'f'));

    public static byte[] Decode(string text) => Convert.FromHexString(text);

    public static string Encode(ReadOnlySpan<byte> bytes) => Convert.ToHexStringLower(bytes);
}

internal sealed partial class IntegerCodec
{
    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        long number;
        if (value is IntegerNode integer)
        {
            if (integer.Width != width || integer.Signed != signed)
            {
                throw site.Mismatch($"{Name(site)} is an integer.");
            }

            number = integer.Value;
        }
        else if (value is ReferenceNode reference && width == 4 && signed)
        {
            if (domain is not null && reference.Domain != domain)
            {
                throw site.Fail(Codes.Reference, message: $"{Name(site)} is in domain {reference.Domain}.", value: reference.Domain, constraint: domain);
            }

            number = reference.Key;
        }
        else
        {
            throw site.Mismatch($"{Name(site)} is an integer.");
        }

        return number < Min || number > Max
            ? throw site.Fail(Codes.Range, message: $"{Name(site)} is outside its storage range.", value: Text(number), constraint: $"{Min} to {Max}")
            : (JsonValue)new JsonInteger(number);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        return value is not JsonInteger number
            ? throw site.Mismatch($"{Name(site)} is an integer.", at: value)
            : number.Value < Min || number.Value > Max
            ? throw site.Fail(Codes.Range, message: $"{Name(site)} is outside its storage range.", at: value, value: Text(number.Value), constraint: $"{Min} to {Max}")
            : domain is not null ? new ReferenceNode(domain, key: (int)number.Value) : new IntegerNode(width, signed, number.Value);
    }

    private static string Name(DocumentSite site) => site.Property?.Name ?? "The value";
}

internal sealed partial class BooleanCodec
{
    internal override bool StoresImage(TreeValue value) => value is not BooleanNode { Value: false };

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        return value is not BooleanNode node
            ? throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a boolean.")
            : !node.Value && !site.Element ? null : (JsonValue)new JsonBoolean(node.Value);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        return value is not JsonBoolean node
            ? throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a boolean.", at: value)
            : !node.Value && !site.Element ? null : (TreeValue)new BooleanNode(node.Value);
    }
}

internal sealed partial class StringCodec
{
    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not StringNode node)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a string.");
        }

        site.RequireText(node.Value);
        return new JsonString(node.Value);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is not JsonString text)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a string.", at: value);
        }

        site.RequireText(text.Value, at: text);
        return new StringNode(text.Value);
    }
}

internal partial class StringsCodec
{
    internal override bool AcceptsDocumentArray(int version) => true;

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not LinesNode node)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is lines.");
        }

        string text = node.RawText ?? Join(node.Lines ?? []);
        site.RequireText(text);
        return TextValue(text);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is JsonObject objectValue)
        {
            CheckOnly(site.Context, objectValue, site.Path, "$text");
        }

        string text = TextOf(site.Context, value, site.Path, lines: out _);
        site.RequireText(text, at: value);
        return LinesNode.FromText(text);
    }

    internal static string Join(IReadOnlyList<string> lines)
    {
        StringBuilder builder = new();
        foreach (string line in lines)
        {
            _ = builder.Append(line).Append("\r\n");
        }

        return builder.ToString();
    }
}

internal sealed partial class StringsWithValuesCodec
{
    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not LinesWithValuesNode node)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is lines with objects.");
        }

        string text = node.RawText ?? Join(node.Lines ?? []);
        int lines = LineCount(text);
        if (lines != node.Objects.Count)
        {
            throw site.Fail(Codes.Corrupt, message: $"The text has {lines} lines and {node.Objects.Count} object values.");
        }

        site.RequireText(text);
        JsonArray objects = new();
        objects.Items.AddRange(node.Objects.Select(static item => new JsonInteger(item)));
        // The converter stores an array only when the block rule reproduces the bytes. A line that
        // holds CR, LF or NUL stays $text, the same choice Decode makes for the engine image.
        List<string>? canonical = node.RawText is null ? Lines(text) : null;
        if (canonical is null)
        {
            return new JsonObject().Add("$text", new JsonString(text)).Add("objects", objects);
        }

        JsonArray array = new();
        array.Items.AddRange(canonical.Select(static line => new JsonString(line)));

        return new JsonObject().Add("lines", array).Add("objects", objects);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is not JsonObject objectValue)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an object with lines or $text, and objects.", at: value);
        }

        (string? text, List<int>? ids) = ReadMembers(site.Context, objectValue, site.Path);
        site.RequireText(text, at: value);
        return LinesWithValuesNode.FromText(text, objects: ids);
    }
}

internal sealed partial class DataCodec
{
    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        return value is not BytesNode node
            ? throw site.Mismatch($"{site.Property?.Name ?? "The value"} is lowercase hexadecimal bytes.")
            : Size is int n && node.Data.Length != n
            ? throw site.Fail(Codes.Range, message: $"{site.Property?.Name ?? "The value"} holds exactly {n} bytes.", value: Text(node.Data.Length), constraint: $"{n} bytes")
            : node.Data.Length > site.Context.Limits.DecompressedBytes
            ? throw site.Fail(Codes.Limit, message: "A payload is too large.")
            : (JsonValue)new JsonString(HexText.Encode(node.Data));
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is not JsonString text || !HexText.IsLowerHex(text.Value))
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is lowercase hexadecimal bytes.", at: value);
        }

        if (text.Value.Length / 2 > site.Context.Limits.DecompressedBytes)
        {
            throw site.Fail(Codes.Limit, message: "A payload is too large.", at: text);
        }

        byte[] bytes = HexText.Decode(text.Value);
        return Size is int n && bytes.Length != n
            ? throw site.Fail(Codes.Range, message: $"{site.Property?.Name ?? "The value"} holds exactly {n} bytes.", at: text, value: Text(bytes.Length), constraint: $"{n} bytes")
            : (TreeValue)new BytesNode(bytes);
    }
}

internal sealed partial class FixedObjectCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not ObjectNode node)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property.Name} is an object.", at: null, frame.Path);
        }

        ImageTree.WriteBody(node, classValue: context.Schema.Get(ClassName), context, data, frame.Path, allowIdentity: false);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
        => ImageTree.ReadBody(context, start, end, classValue: context.Schema.Get(ClassName), frame.Path, className: null, slot: null);

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        return value is not ObjectNode node
            ? throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an object.")
            : (JsonValue)DocumentBackend.WriteObject(node, classValue: site.Schema.Get(ClassName), site, created: false, slotted: false);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        return value is not JsonObject objectValue
            ? throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an object.", at: value)
            : (TreeValue)DocumentBackend.ReadObject(objectValue, classValue: site.Schema.Get(ClassName), site, created: false, slotted: false);
    }

    // A fixed dynamic member (TAbilityControl, TSpellControl) stores each object at its slot plus the
    // slot base, as rwEObject stores one, with no class id. The object is never nil.
    internal override void WriteSlotsImage(TreeValue value, EncodeContext context, ObjectWriter writer, ImageFrame frame)
    {
        string name = frame.Property.Name;
        if (frame.Property.SlotBase is not int slotBase)
        {
            throw context.Fail(Codes.Type, message: $"{name} has no slot base.", at: null, path: frame.Path);
        }

        if (value is not SlotListNode node)
        {
            throw context.Fail(Codes.Type, message: $"{name} is fixed objects at slots.", at: null, path: frame.Path);
        }

        if (node.Entries.Count > context.Limits.CollectionItems)
        {
            throw context.Fail(Codes.Limit, message: "A collection has too many items.", at: null, path: frame.Path, value: null, constraint: $"at most {context.Limits.CollectionItems} items");
        }

        SchemaClass classValue = context.Schema.Get(ClassName);
        HashSet<int> used = [];
        for (int i = 0; i < node.Entries.Count; i++)
        {
            (int slot, TreeValue? item) = node.Entries[i];
            DocumentNodePath itemPath = frame.Path.Item(i);
            long id = (long)slot + slotBase;
            if (slot < 0 || id < frame.Property.DynamicMinimum!.Value || id > int.MaxValue)
            {
                throw context.Fail(Codes.Range, message: "The $slot is outside the stored id range.", at: null, path: itemPath.Member("$slot"), value: Text(slot));
            }

            if (!used.Add(slot))
            {
                throw context.Fail(Codes.Slot, message: $"Slot {Text(slot)} of {name} is declared twice.", at: null, path: itemPath);
            }

            if (item is not ObjectNode { ClassName: null } objectNode || (objectNode.Slot is int stated && stated != slot))
            {
                throw context.Fail(Codes.Type, message: $"An item of {name} is a fixed object at its slot.", at: null, path: itemPath);
            }

            writer.Begin((int)id, prop: frame.Property, elementIndex: i);
            ImageTree.WriteBody(objectNode, classValue, context, writer.Data, path: itemPath, allowIdentity: true);
            writer.End();
        }
    }

    internal override TreeValue ReadSlotsImage(DecodeContext context, IReadOnlyList<SlotImage> entries, ImageFrame frame)
    {
        if (frame.Property.SlotBase is not int slotBase)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property.Name} has no slot base.", offset: entries[0].Start, frame.Path);
        }

        context.CheckItems(entries.Count, offset: entries[0].Start, frame.Path);
        SchemaClass classValue = context.Schema.Get(ClassName);
        SlotListNode node = new();
        for (int i = 0; i < entries.Count; i++)
        {
            (int id, int start, int end) = entries[i];
            node.Add(id - slotBase, value: ImageTree.ReadBody(context, start, end, classValue, path: frame.Path.Item(i), className: null, slot: id - slotBase));
        }
        return node;
    }

    internal override JsonValue WriteSlotsTree(TreeValue value, DocumentSite site)
    {
        string name = site.Property!.Name;
        if (site.Schema.Version < SlotVersion || site.Property.SlotBase is not int slotBase)
        {
            throw site.Fail(Codes.Type, message: $"Version {site.Schema.Version} stores no slot of {name}.");
        }

        if (value is not SlotListNode node)
        {
            throw site.Mismatch($"{name} is fixed objects at slots.");
        }

        if (node.Entries.Count > site.Context.Limits.CollectionItems)
        {
            throw site.Fail(Codes.Limit, message: "A collection has too many items.", constraint: $"at most {site.Context.Limits.CollectionItems} items");
        }

        SchemaClass classValue = site.Schema.Get(ClassName);
        JsonArray array = new();
        HashSet<int> used = [];
        for (int i = 0; i < node.Entries.Count; i++)
        {
            (int slot, TreeValue? item) = node.Entries[i];
            DocumentSite at = site.At(site.Path.Item(i), element: false);
            long id = (long)slot + slotBase;
            if (slot < 0 || id < site.Property.DynamicMinimum!.Value || id > int.MaxValue)
            {
                throw at.At(at.Path.Member("$slot"), element: false).Fail(Codes.Range, message: "The $slot is outside the stored id range.", value: Text(slot));
            }

            if (!used.Add(slot))
            {
                throw at.Fail(Codes.Slot, message: $"Slot {Text(slot)} of {name} is declared twice.");
            }

            if (item is not ObjectNode { ClassName: null } objectNode || objectNode.Slot != slot)
            {
                throw at.Mismatch($"An item of {name} is a fixed object at its slot.");
            }

            _ = array.Add(DocumentBackend.WriteObject(objectNode, classValue, site: at, created: false, slotted: true));
        }
        return array;
    }

    internal override TreeValue ReadSlotsTree(JsonValue value, DocumentSite site)
    {
        string name = site.Property!.Name;
        if (site.Schema.Version < SlotVersion || site.Property.SlotBase is not int slotBase)
        {
            throw site.Fail(Codes.Type, message: $"Version {site.Schema.Version} stores no slot of {name}.", at: value);
        }

        if (value is not JsonArray array)
        {
            throw site.Mismatch($"{name} is an array.", at: value);
        }

        if (array.Items.Count > site.Context.Limits.CollectionItems)
        {
            throw site.Fail(Codes.Limit, message: "A collection has too many items.", at: array, value: null, constraint: $"at most {site.Context.Limits.CollectionItems} items");
        }

        SchemaClass classValue = site.Schema.Get(ClassName);
        SlotListNode node = new();
        HashSet<long> used = [];
        for (int i = 0; i < array.Items.Count; i++)
        {
            DocumentSite at = site.At(site.Path.Item(i), element: false);
            if (array.Items[i] is not JsonObject item)
            {
                throw at.Mismatch($"An item of {name} is an object.", at: array.Items[i]);
            }

            if (item["$slot"] is not JsonInteger slot)
            {
                throw at.Fail(Codes.MissingMember, message: $"An item of {name} needs an integer $slot.", at: item);
            }

            long id = slot.Value + slotBase;
            if (slot.Value < 0 || id < site.Property.DynamicMinimum!.Value || id > int.MaxValue)
            {
                throw at.At(at.Path.Member("$slot"), element: false).Fail(Codes.Range, message: "The $slot is outside the stored id range.", at: slot, value: Text(slot.Value));
            }

            if (!used.Add(slot.Value))
            {
                throw at.Fail(Codes.Slot, message: $"Slot {Text(slot.Value)} of {name} is declared twice.", at: item);
            }

            node.Add((int)slot.Value, value: DocumentBackend.ReadObject(item, classValue, site: at, created: false, slotted: true));
        }
        return node;
    }
}

internal sealed partial class CreatedObjectCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is NullNode)
        {
            data.Int32(0);
            return;
        }
        if (value is not ObjectNode node || node.ClassName is not string name)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property.Name} is an object or null.", at: null, frame.Path);
        }

        SchemaClass classValue = Resolve(context, name, frame);
        (string Rule, SchemaClass Shape)? shape = context.Schema.LegacyShape($"{frame.Property.Owner.Name}.{frame.Property.Name}", className: classValue.Name);
        data.Int32(classValue.Id!.Value);
        ImageTree.WriteBody(node, classValue: shape?.Shape ?? classValue, context, data, frame.Path, allowIdentity: true);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
        => ReadCreated(context, start, end, frame, slot: null);

    // A keyed member's entry stores the same image as a created object. Its id less the slot base is the slot.
    internal override void WriteSlotsImage(TreeValue value, EncodeContext context, ObjectWriter writer, ImageFrame frame)
    {
        if (frame.Property.SlotBase is not int slotBase)
        {
            WriteSequenceImage(value, context, writer, frame);
            return;
        }
        string name = frame.Property.Name;
        if (value is not SlotListNode node)
        {
            throw context.Fail(Codes.Type, message: $"{name} is created objects at slots.", at: null, frame.Path);
        }

        if (node.Entries.Count > context.Limits.CollectionItems)
        {
            throw context.Fail(Codes.Limit, message: "A collection has too many items.", at: null, frame.Path, value: null, constraint: $"at most {context.Limits.CollectionItems} items");
        }

        HashSet<int> used = [];
        for (int i = 0; i < node.Entries.Count; i++)
        {
            (int slot, TreeValue? item) = node.Entries[i];
            DocumentNodePath itemPath = frame.Path.Item(i);
            long id = (long)slot + slotBase;
            if (slot < 0 || id < frame.Property.DynamicMinimum!.Value || id > int.MaxValue)
            {
                throw context.Fail(Codes.Range, message: "The $slot is outside the stored id range.", at: null, path: itemPath.Member("$slot"), value: Text(slot));
            }

            if (!used.Add(slot))
            {
                throw context.Fail(Codes.Slot, message: $"Slot {Text(slot)} of {name} is declared twice.", at: null, path: itemPath);
            }

            if (item is ObjectNode { Slot: int stated } && stated != slot)
            {
                throw context.Fail(Codes.Slot, message: $"An item of {name} states slot {Text(stated)} at slot {Text(slot)}.", at: null, path: itemPath);
            }

            writer.Begin((int)id, prop: frame.Property, elementIndex: i);
            WriteImage(item, context, writer.Data, frame: new ImageFrame { Property = frame.Property, Id = (int)id, Path = itemPath, Parent = frame.Parent });
            writer.End();
        }
    }

    internal override TreeValue ReadSlotsImage(DecodeContext context, IReadOnlyList<SlotImage> entries, ImageFrame frame)
    {
        if (frame.Property.SlotBase is not int slotBase)
        {
            return ReadSequenceImage(context, entries, frame);
        }

        context.CheckItems(entries.Count, offset: entries[0].Start, frame.Path);
        SlotListNode node = new();
        for (int i = 0; i < entries.Count; i++)
        {
            (int id, int start, int end) = entries[i];
            node.Add(id - slotBase, value: ReadCreated(frame: new ImageFrame { Property = frame.Property, Id = id, Path = frame.Path.Item(i), Parent = frame.Parent }, context: context, start: start, end: end, slot: id - slotBase));
        }
        return node;
    }

    internal override JsonValue WriteSlotsTree(TreeValue value, DocumentSite site)
    {
        if (site.Property?.SlotBase is not int slotBase)
        {
            return WriteSequenceTree(value, site);
        }

        string name = site.Property.Name;
        if (value is not SlotListNode node)
        {
            throw site.Mismatch($"{name} is created objects at slots.");
        }

        if (node.Entries.Count > site.Context.Limits.CollectionItems)
        {
            throw site.Fail(Codes.Limit, message: "A collection has too many items.", constraint: $"at most {site.Context.Limits.CollectionItems} items");
        }

        JsonArray array = new();
        HashSet<int> used = [];
        for (int i = 0; i < node.Entries.Count; i++)
        {
            (int slot, TreeValue? item) = node.Entries[i];
            DocumentSite at = site.At(site.Path.Item(i), element: false);
            long id = (long)slot + slotBase;
            if (slot < 0 || id < site.Property.DynamicMinimum!.Value || id > int.MaxValue)
            {
                throw at.At(at.Path.Member("$slot"), element: false).Fail(Codes.Range, message: "The $slot is outside the stored id range.", value: Text(slot));
            }

            if (!used.Add(slot))
            {
                throw at.Fail(Codes.Slot, message: $"Slot {slot} of {name} is declared twice.");
            }

            _ = item is NullNode
                ? array.Add(new JsonObject().Add("$class", new JsonNull()).Add("$slot", new JsonInteger(slot)))
                : item is ObjectNode { ClassName: string className } objectNode && objectNode.Slot == slot
                    ? array.Add(DocumentBackend.WriteObject(objectNode, classValue: DocumentBackend.RequireClass(at, name: className, Family, at: null), site: at, created: true, slotted: true))
                    : throw at.Mismatch($"An item of {name} is a created object or null at its slot.");
        }
        return array;
    }

    internal override TreeValue ReadSlotsTree(JsonValue value, DocumentSite site)
    {
        if (site.Property?.SlotBase is not int slotBase)
        {
            return ReadSequenceTree(value, site);
        }

        string name = site.Property.Name;
        if (value is not JsonArray array)
        {
            throw site.Mismatch($"{name} is an array.", at: value);
        }

        if (array.Items.Count > site.Context.Limits.CollectionItems)
        {
            throw site.Fail(Codes.Limit, message: "A collection has too many items.", at: array, value: null, constraint: $"at most {site.Context.Limits.CollectionItems} items");
        }

        SlotListNode node = new();
        HashSet<long> used = [];
        for (int i = 0; i < array.Items.Count; i++)
        {
            DocumentSite at = site.At(site.Path.Item(i), element: false);
            if (array.Items[i] is not JsonObject item)
            {
                throw at.Mismatch($"An item of {name} is an object.", at: array.Items[i]);
            }

            if (item["$slot"] is not JsonInteger slot)
            {
                throw at.Fail(Codes.MissingMember, message: $"An item of {name} needs an integer $slot.", at: item);
            }

            long id = slot.Value + slotBase;
            if (slot.Value < 0 || id < site.Property.DynamicMinimum!.Value || id > int.MaxValue)
            {
                throw at.At(at.Path.Member("$slot"), element: false).Fail(Codes.Range, message: "The $slot is outside the stored id range.", at: slot, value: Text(slot.Value));
            }

            if (!used.Add(slot.Value))
            {
                throw at.Fail(Codes.Slot, message: $"Slot {slot.Value} of {name} is declared twice.", at: item);
            }

            if (item["$class"] is JsonNull)
            {
                foreach (JsonMember member in item.Members)
                {
                    if (member.Name is not ("$class" or "$slot"))
                    {
                        throw at.At(at.Path.Member(member.Name), element: false).Fail(Codes.UnknownMember, message: "A nil object has no members.", at: member.Value, value: member.Name);
                    }
                }

                node.Add((int)slot.Value, value: NullNode.Instance);
                continue;
            }
            if (item["$class"] is not JsonString className)
            {
                throw at.Fail(Codes.MissingMember, message: "A created object needs $class.", at: item);
            }

            SchemaClass classValue = DocumentBackend.RequireClass(at, name: className.Value, Family, at: className);
            node.Add((int)slot.Value, value: DocumentBackend.ReadObject(item, classValue, site: at, created: true, slotted: true));
        }
        return node;
    }

    // A dynamic member without a slot base stores its objects at ids from the dynamic minimum in
    // order (TMapField sprites from 50). The id carries no identity, so the document has no $slot.
    private void WriteSequenceImage(TreeValue value, EncodeContext context, ObjectWriter writer, ImageFrame frame)
    {
        string name = frame.Property.Name;
        if (value is not SequenceNode node)
        {
            throw context.Fail(Codes.Type, message: $"{name} is a sequence of created objects.", at: null, frame.Path);
        }

        if (node.Items.Count > context.Limits.CollectionItems)
        {
            throw context.Fail(Codes.Limit, message: "A collection has too many items.", at: null, frame.Path, value: null, constraint: $"at most {context.Limits.CollectionItems} items");
        }

        int first = frame.Property.DynamicMinimum!.Value;
        if ((long)first + node.Items.Count - 1 > int.MaxValue)
        {
            throw context.Fail(Codes.Range, message: $"{name} has more items than its ids hold.", at: null, frame.Path);
        }

        for (int i = 0; i < node.Items.Count; i++)
        {
            TreeValue item = node.Items[i];
            if (item is ObjectNode { Slot: not null })
            {
                throw context.Fail(Codes.ReservedMember, message: $"Items of {name} have no $slot.", at: null, path: frame.Path.Item(i).Member("$slot"), value: "$slot");
            }

            writer.Begin(first + i, prop: frame.Property, elementIndex: i);
            WriteImage(item, context, writer.Data, frame: new ImageFrame { Property = frame.Property, Id = first + i, Path = frame.Path.Item(i), Parent = frame.Parent });
            writer.End();
        }
    }

    // The entries in data order, the order the converter lists them. A stored id that is not the
    // writer's is accepted and not kept, as the direct path keeps no layout.
    private TreeValue ReadSequenceImage(DecodeContext context, IReadOnlyList<SlotImage> entries, ImageFrame frame)
    {
        context.CheckItems(entries.Count, offset: entries[0].Start, frame.Path);
        TreeValue[] items = new TreeValue[entries.Count];
        for (int i = 0; i < entries.Count; i++)
        {
            (int id, int start, int end) = entries[i];
            items[i] = ReadCreated(context, start, end, frame: new ImageFrame { Property = frame.Property, Id = id, Path = frame.Path.Item(i), Parent = frame.Parent }, slot: null);
        }
        return new SequenceNode(items);
    }

    private JsonValue WriteSequenceTree(TreeValue value, DocumentSite site)
    {
        string name = site.Property!.Name;
        if (value is not SequenceNode node)
        {
            throw site.Mismatch($"{name} is a sequence of created objects.");
        }

        if (node.Items.Count > site.Context.Limits.CollectionItems)
        {
            throw site.Fail(Codes.Limit, message: "A collection has too many items.", constraint: $"at most {site.Context.Limits.CollectionItems} items");
        }

        JsonArray array = new();
        for (int i = 0; i < node.Items.Count; i++)
        {
            DocumentSite at = site.At(site.Path.Item(i), element: false);
            _ = node.Items[i] is NullNode
                ? array.Add(new JsonObject().Add("$class", new JsonNull()))
                : node.Items[i] is ObjectNode { ClassName: string className, Slot: null } objectNode
                    ? array.Add(DocumentBackend.WriteObject(objectNode, classValue: Shape(at, classValue: DocumentBackend.RequireClass(at, name: className, family: LegacyFamily(at, className), at: null)), site: at, created: true, slotted: false))
                    : throw at.Mismatch($"An item of {name} is a created object without a slot, or null.");
        }
        return array;
    }

    private TreeValue ReadSequenceTree(JsonValue value, DocumentSite site)
    {
        string name = site.Property!.Name;
        if (value is not JsonArray array)
        {
            throw site.Mismatch($"{name} is an array.", at: value);
        }

        if (array.Items.Count > site.Context.Limits.CollectionItems)
        {
            throw site.Fail(Codes.Limit, message: "A collection has too many items.", at: array, value: null, constraint: $"at most {site.Context.Limits.CollectionItems} items");
        }

        TreeValue[] items = new TreeValue[array.Items.Count];
        for (int i = 0; i < array.Items.Count; i++)
        {
            DocumentSite at = site.At(site.Path.Item(i), element: false);
            if (array.Items[i] is not JsonObject item)
            {
                throw at.Mismatch($"An item of {name} is an object.", at: array.Items[i]);
            }

            if (item.Find("$slot") is { } extra)
            {
                throw at.At(at.Path.Member("$slot"), element: false).Fail(Codes.ReservedMember, message: $"Items of {name} have no $slot.", at: extra.Value, value: "$slot");
            }

            if (item["$class"] is JsonNull)
            {
                foreach (JsonMember member in item.Members)
                {
                    if (member.Name != "$class")
                    {
                        throw at.At(at.Path.Member(member.Name), element: false).Fail(Codes.UnknownMember, message: "A nil object has no members.", at: member.Value, value: member.Name);
                    }
                }

                items[i] = NullNode.Instance;
                continue;
            }
            if (item["$class"] is not JsonString className)
            {
                throw at.Fail(Codes.MissingMember, message: "A created object needs $class.", at: item);
            }

            items[i] = DocumentBackend.ReadObject(item, classValue: Shape(classValue: DocumentBackend.RequireClass(at, name: className.Value, family: LegacyFamily(at, className: className.Value), at: className), site: at), site: at, created: true, slotted: false);
        }
        return new SequenceNode(items);
    }

    // A legacy shape (schema legacyShapes) stores a class under another property set where its
    // context names it, as the image side reads it. The document keeps the class name.
    private static SchemaClass Shape(DocumentSite site, SchemaClass classValue)
        => site.Property is { } property ? site.Schema.LegacyShape($"{property.Owner.Name}.{property.Name}", className: classValue.Name)?.Shape ?? classValue : classValue;

    // The family check does not apply to a class the legacy shape names, as on the image side.
    private string? LegacyFamily(DocumentSite site, string className)
        => site.Property is { } property && site.Schema.LegacyShape($"{property.Owner.Name}.{property.Name}", className) is not null ? null : Family;

    private TreeValue ReadCreated(DecodeContext context, int start, int end, ImageFrame frame, int? slot)
    {
        context.Need(start, count: 4, end, frame.Path, what: "class id");
        int classId = context.Int32(start);
        if (classId == 0)
        {
            return end != start + 4
                ? throw context.Fail(Codes.Corrupt, message: "Bytes follow a nil object.", offset: start + 4, frame.Path)
                : (TreeValue)NullNode.Instance;
        }
        SchemaClass classValue = context.Schema.Resolve(context.Engine, classId)
            ?? throw context.Fail(Codes.UnknownClass, message: string.Create(System.Globalization.CultureInfo.InvariantCulture, handler: $"Class id {classId} is not in the {context.Engine} engine table."), offset: start, frame.Path, value: classId.ToString(System.Globalization.CultureInfo.InvariantCulture));
        (string Rule, SchemaClass Shape)? shape = context.Schema.LegacyShape($"{frame.Property.Owner.Name}.{frame.Property.Name}", className: classValue.Name);
        if (shape is null && Family is not null && !classValue.InheritsFrom(Family))
        {
            throw context.Fail(Codes.ClassFamily, message: $"Class {classValue.Name} is not a {Family}.", offset: start, frame.Path, value: classValue.Name, constraint: Family);
        }

        if (shape is { } legacy)
        {
            context.LegacyRules[legacy.Rule] = context.LegacyRules.GetValueOrDefault(legacy.Rule) + 1;
        }

        return ImageTree.ReadBody(context, start: start + 4, end, classValue: shape?.Shape ?? classValue, frame.Path, className: classValue.Name, slot);
    }

    private SchemaClass Resolve(EncodeContext context, string name, ImageFrame frame)
    {
        SchemaClass classValue = context.Schema.Find(name)
            ?? throw context.Fail(Codes.UnknownClass, message: $"Unknown class {name}.", at: null, frame.Path, value: name);
        return classValue.Id is not int id || context.Schema.Resolve(context.Engine, classId: id) != classValue
            ? throw context.Fail(Codes.UnknownClass, message: $"Class {classValue.Name} is not in the {context.Engine} engine table.", at: null, frame.Path, value: classValue.Name)
            : Family is not null && context.Schema.LegacyShape($"{frame.Property.Owner.Name}.{frame.Property.Name}", className: classValue.Name) is null && !classValue.InheritsFrom(Family)
            ? throw context.Fail(Codes.ClassFamily, message: $"Class {classValue.Name} is not a {Family}.", at: null, frame.Path, value: classValue.Name, constraint: Family)
            : classValue;
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        return value is NullNode
            ? new JsonNull()
            : value is not ObjectNode node || node.ClassName is not string name
            ? throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an object or null.")
            : (JsonValue)DocumentBackend.WriteObject(classValue: DocumentBackend.RequireClass(site, name, Family, at: null), node: node, site: site, created: true, slotted: false);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        return value is JsonNull
            ? NullNode.Instance
            : value is not JsonObject objectValue
            ? throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an object or null.", at: value)
            : objectValue["$class"] is not JsonString name
            ? throw site.Fail(Codes.MissingMember, message: "A created object needs $class.", at: objectValue)
            : (TreeValue)DocumentBackend.ReadObject(classValue: DocumentBackend.RequireClass(site, name: name.Value, Family, at: name), document: objectValue, site: site, created: true, slotted: false);
    }
}

internal sealed partial class ChildrenCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not ChildListNode node)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property.Name} is an array.", at: null, frame.Path);
        }

        if (node.Children.Count > context.Limits.CollectionItems)
        {
            throw context.Fail(Codes.Limit, message: "A collection has too many items.", at: null, frame.Path, value: null, constraint: $"at most {context.Limits.CollectionItems} items");
        }

        ObjectWriter writer = new();
        HashSet<int> slots = [];
        for (int i = 0; i < node.Children.Count; i++)
        {
            ObjectNode child = node.Children[i];
            DocumentNodePath itemPath = frame.Path.Item(i);
            if (child.Slot is not int slot || slot < 0)
            {
                throw context.Fail(Codes.MissingMember, message: "A child needs a non-negative integer $slot.", at: null, path: itemPath);
            }

            if (!slots.Add(slot))
            {
                throw context.Fail(Codes.Slot, message: $"Child slot {slot} is declared twice.", at: null, path: itemPath.Member("$slot"));
            }

            if (child.ClassName is not string name)
            {
                throw context.Fail(Codes.MissingMember, message: "A child needs a class.", at: null, path: itemPath);
            }

            SchemaClass classValue = context.Schema.Find(name)
                ?? throw context.Fail(Codes.UnknownClass, message: $"Unknown class {name}.", at: null, path: itemPath, value: name);
            if (classValue.Id is not int id || context.Schema.Resolve(context.Engine, classId: id) != classValue)
            {
                throw context.Fail(Codes.UnknownClass, message: $"Class {classValue.Name} is not in the {context.Engine} engine table.", at: null, path: itemPath, value: classValue.Name);
            }

            (string Rule, SchemaClass Shape)? shape = context.Schema.LegacyShape($"{frame.Property.Owner.Name}.{frame.Property.Name}", className: classValue.Name);
            writer.Begin(slot, prop: frame.Property, elementIndex: i);
            writer.Data.Int32(id);
            ImageTree.WriteBody(child, classValue: shape?.Shape ?? classValue, context, writer.Data, path: itemPath, allowIdentity: true);
            writer.End();
        }
        ObjectImage.Place(context, writer, frame.Path, output: data);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        PropertyTable table = PropertyTable.Read(context, start, end, frame.Path);
        int db = table.DataBase;
        int[] boundaries = [.. table.Entries.Select(entry => entry.Offset).Append(end - db).Distinct().Order()];
        List<TableEntry> order = [.. table.Entries.OrderBy(entry => entry.Offset).ThenBy(entry => entry.Index)];
        HashSet<int> slots = [];
        List<ObjectNode> children = [];
        for (int i = 0; i < order.Count; i++)
        {
            TableEntry entry = order[i];
            if (i + 1 < order.Count && order[i + 1].Offset == entry.Offset)
            {
                throw context.Fail(Codes.Layout, message: "Two children share one offset.", offset: db + entry.Offset, frame.Path);
            }

            if (!slots.Add(entry.Id))
            {
                throw context.Fail(Codes.Duplicate, message: $"Child slot {entry.Id} is stored twice.", offset: db + entry.Offset, frame.Path);
            }

            int next = boundaries[Array.BinarySearch(boundaries, value: entry.Offset) + 1];
            int childStart = db + entry.Offset;
            int childEnd = db + next;
            context.Need(childStart, count: 4, end: childEnd, path: frame.Path.Item(children.Count), what: "class id");
            int classId = context.Int32(childStart);
            if (classId == 0)
            {
                throw context.Fail(Codes.Type, message: "A stored child is never nil.", offset: childStart, path: frame.Path.Item(children.Count));
            }

            SchemaClass classValue = context.Schema.Resolve(context.Engine, classId)
                ?? throw context.Fail(Codes.UnknownClass, message: string.Create(System.Globalization.CultureInfo.InvariantCulture, handler: $"Class id {classId} is not in the {context.Engine} engine table."), offset: childStart, path: frame.Path.Item(children.Count), value: classId.ToString(System.Globalization.CultureInfo.InvariantCulture));
            (string Rule, SchemaClass Shape)? shape = context.Schema.LegacyShape($"{frame.Property.Owner.Name}.{frame.Property.Name}", className: classValue.Name);
            if (shape is { } legacy)
            {
                context.LegacyRules[legacy.Rule] = context.LegacyRules.GetValueOrDefault(legacy.Rule) + 1;
            }

            children.Add(ImageTree.ReadBody(path: frame.Path.Item(children.Count), context: context, start: childStart + 4, end: childEnd, classValue: shape?.Shape ?? classValue, className: classValue.Name, slot: entry.Id));
        }
        return children.Count > context.Limits.CollectionItems
            ? throw context.Fail(Codes.Limit, message: "A collection has too many items.", offset: start, frame.Path, value: null, constraint: $"at most {context.Limits.CollectionItems} items")
            : (TreeValue)new ChildListNode(children);
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not ChildListNode node)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an array.");
        }

        if (node.Children.Count > site.Context.Limits.CollectionItems)
        {
            throw site.Fail(Codes.Limit, message: "A collection has too many items.", constraint: $"at most {site.Context.Limits.CollectionItems} items");
        }

        JsonArray array = new();
        HashSet<int> slots = [];
        for (int i = 0; i < node.Children.Count; i++)
        {
            ObjectNode child = node.Children[i];
            DocumentSite item = site.At(site.Path.Item(i), element: false);
            if (child.Slot is not int slot || !slots.Add(slot))
            {
                throw item.Fail(Codes.Slot, message: child.Slot is int used ? $"Child slot {used} is declared twice." : "A child needs a non-negative integer $slot.");
            }

            if (child.ClassName is not string name)
            {
                throw item.Fail(Codes.MissingMember, message: "A child needs a class.");
            }

            _ = array.Add(DocumentBackend.WriteObject(classValue: DocumentBackend.RequireClass(item, name, family: null, at: null), node: child, site: item, created: true, slotted: true));
        }
        return array;
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is not JsonArray array)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an array.", at: value);
        }

        if (array.Items.Count > site.Context.Limits.CollectionItems)
        {
            throw site.Fail(Codes.Limit, message: "A collection has too many items.", at: array, value: null, constraint: $"at most {site.Context.Limits.CollectionItems} items");
        }

        List<ObjectNode> children = [with(array.Items.Count)];
        HashSet<int> slots = [];
        for (int i = 0; i < array.Items.Count; i++)
        {
            DocumentSite item = site.At(site.Path.Item(i), element: false);
            if (array.Items[i] is not JsonObject child)
            {
                throw item.Mismatch("A child is an object.", at: array.Items[i]);
            }

            if (child["$class"] is JsonNull)
            {
                throw item.Fail(Codes.Type, message: "A stored child is never nil.", at: child);
            }

            if (child["$class"] is not JsonString name)
            {
                throw item.Fail(Codes.MissingMember, message: "A child needs a class.", at: child);
            }

            if (child["$slot"] is not JsonInteger { Value: >= 0 and <= int.MaxValue } slot)
            {
                throw item.Fail(Codes.MissingMember, message: "A child needs a non-negative integer $slot.", at: child);
            }

            if (!slots.Add((int)slot.Value))
            {
                throw item.Fail(Codes.Slot, message: $"Child slot {slot.Value} is declared twice.", at: slot);
            }

            children.Add(DocumentBackend.ReadObject(classValue: DocumentBackend.RequireClass(item, name: name.Value, family: null, at: name), document: child, site: item, created: true, slotted: true));
        }
        return new ChildListNode(children);
    }
}

/// <summary>A variable-length list. The schema declares whether it stores one. Version 1 refuses the shape.</summary>
internal sealed class ListCodec(PropertyCodec element, bool storesList) : PropertyCodec
{
    internal override string? ImageRefusal => "The direct path does not store a list.";

    internal override bool AcceptsList(int version) => storesList;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
        => throw new InvalidOperationException("A list has no image.");

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
        => throw new InvalidOperationException("A list has no image.");

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (site.Schema.Version == 1)
        {
            throw site.Fail(Codes.Type, message: "Version 1 does not store a list.");
        }

        if (value is not ListNode node)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a list.");
        }

        if (node.Items.Count > site.Context.Limits.CollectionItems)
        {
            throw site.Fail(Codes.Limit, message: "A collection has too many items.", constraint: $"at most {site.Context.Limits.CollectionItems} items");
        }

        JsonArray array = new();
        for (int i = 0; i < node.Items.Count; i++)
        {
            DocumentSite item = site.At(site.Path.Item(i), element: true);
            _ = array.Add(element.WriteTree(node.Items[i], site: item) ?? throw item.Fail(Codes.MissingMember, message: "A list item stores one value."));
        }
        return array;
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (site.Schema.Version == 1)
        {
            throw site.Fail(Codes.Type, message: "Version 1 does not store a list.", at: value);
        }

        if (value is not JsonArray array)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a list.", at: value);
        }

        if (array.Items.Count > site.Context.Limits.CollectionItems)
        {
            throw site.Fail(Codes.Limit, message: "A collection has too many items.", at: array, value: null, constraint: $"at most {site.Context.Limits.CollectionItems} items");
        }

        ListNode node = new();
        node.Items.AddRange(array.Items.Select((value, index) =>
        {
            DocumentSite item = site.At(site.Path.Item(index), element: true);
            return element.ReadTree(value, site: item) ?? throw item.Fail(Codes.MissingMember, message: "A list item stores one value.", at: value);
        }));
        return node;
    }
}

/// <summary>Named fields with no class. Version 1 has no record, and the backend refuses the shape.</summary>
internal sealed class RecordCodec(JsonObject source, FormatSchema schema, string path) : PropertyCodec
{
    internal override string? ImageRefusal => "A record has no image.";

    private readonly (string Name, PropertyCodec Codec)[] _fields = [.. ((JsonArray)source["fields"]!).Items.Cast<JsonObject>().Select(f =>
        {
            string name = ((JsonString)f["name"]!).Value;
            return (name, Create(f, schema, path: $"{path}.{name}"));
        })];

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
        => throw new InvalidOperationException("A record has no image.");

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
        => throw new InvalidOperationException("A record has no image.");

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (site.Schema.Version == 1)
        {
            throw site.Fail(Codes.Type, message: "Version 1 does not store a record.");
        }

        if (value is not RecordNode node)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a record.");
        }

        JsonObject json = new();
        HashSet<string> seen = [with(StringComparer.Ordinal)];
        foreach ((string? name, PropertyCodec? codec) in _fields)
        {
            TreeValue? field = node.Find(name);
            if (field is null)
            {
                continue;
            }

            JsonValue? written = codec.WriteTree(site: site.At(site.Path.Member(name), element: false), value: field);
            if (written is not null)
            {
                _ = json.Add(name, written);
            }

            _ = seen.Add(name);
        }
        foreach ((string? name, _) in node.Members)
        {
            if (!seen.Contains(name))
            {
                throw site.At(site.Path.Member(name), element: false).Fail(Codes.UnknownMember, message: $"The record has no member \"{name}\".", value: name);
            }
        }

        return json;
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (site.Schema.Version == 1)
        {
            throw site.Fail(Codes.Type, message: "Version 1 does not store a record.", at: value);
        }

        if (value is not JsonObject objectValue)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a record.", at: value);
        }

        HashSet<string> known = _fields.Select(f => f.Name).ToHashSet(StringComparer.Ordinal);
        foreach (JsonMember member in objectValue.Members)
        {
            if (member.Name.StartsWith('$') && member.Name != "$text")
            {
                throw site.At(site.Path.Member(member.Name), element: false).Fail(Codes.ReservedMember,
                    message: Reserved.IsKnown(member.Name) ? $"Reserved member \"{member.Name}\" is not allowed here." : $"Unknown reserved member \"{member.Name}\".",
                    at: member.Value, value: member.Name);
            }

            if (!known.Contains(member.Name))
            {
                throw site.At(site.Path.Member(member.Name), element: false).Fail(Codes.UnknownMember, message: $"The record has no member \"{member.Name}\".", at: member.Value, value: member.Name);
            }
        }
        RecordNode node = new();
        foreach ((string? name, PropertyCodec? codec) in _fields)
        {
            JsonMember? member = objectValue.Find(name);
            if (member is null)
            {
                continue;
            }

            TreeValue? read = codec.ReadTree(member.Value, site: site.At(site.Path.Member(name), element: false));
            if (read is not null)
            {
                node.Add(name, read);
            }
        }
        return node;
    }
}
