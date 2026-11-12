using System.Text;
using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Original;

/// <summary>Lowercase hexadecimal, shared by the converter and the document backend.</summary>
internal static class HexText
{
    public static bool IsLowerHex(string text)
    {
        if (text.Length % 2 != 0) return false;
        foreach (var c in text)
            if (c is not (>= '0' and <= '9' or >= 'a' and <= 'f')) return false;
        return true;
    }

    public static byte[] Decode(string text) => Convert.FromHexString(text);
}

internal sealed partial class IntegerCodec
{
    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        long number;
        if (value is IntegerNode integer)
        {
            if (integer.Width != width || integer.Signed != signed) throw site.Mismatch($"{Name(site)} is an integer.");
            number = integer.Value;
        }
        else if (value is ReferenceNode reference && width == 4 && signed)
        {
            if (domain is not null && reference.Domain != domain)
                throw site.Fail(Codes.Reference, $"{Name(site)} is in domain {reference.Domain}.", value: reference.Domain, constraint: domain);
            number = reference.Key;
        }
        else throw site.Mismatch($"{Name(site)} is an integer.");
        if (number < Min || number > Max)
            throw site.Fail(Codes.Range, $"{Name(site)} is outside its storage range.", value: Text(number), constraint: $"{Min} to {Max}");
        return new JsonInteger(number);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is not JsonInteger number) throw site.Mismatch($"{Name(site)} is an integer.", value);
        if (number.Value < Min || number.Value > Max)
            throw site.Fail(Codes.Range, $"{Name(site)} is outside its storage range.", value, Text(number.Value), $"{Min} to {Max}");
        if (domain is not null) return new ReferenceNode(domain, (int)number.Value);
        return new IntegerNode(width, signed, number.Value);
    }

    private static string Name(DocumentSite site) => site.Property?.Name ?? "The value";
}

internal sealed partial class BooleanCodec
{
    internal override bool StoresImage(TreeValue value) => value is not BooleanNode { Value: false };

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not BooleanNode node) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a boolean.");
        if (!node.Value && !site.Element) return null;
        return new JsonBoolean(node.Value);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is not JsonBoolean node) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a boolean.", value);
        if (!node.Value && !site.Element) return null;
        return new BooleanNode(node.Value);
    }
}

internal sealed partial class StringCodec
{
    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not StringNode node) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a string.");
        site.RequireText(node.Value);
        return new JsonString(node.Value);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is not JsonString text) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a string.", value);
        site.RequireText(text.Value, text);
        return new StringNode(text.Value);
    }
}

internal partial class StringsCodec
{
    internal override bool AcceptsDocumentArray(int version) => true;

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not LinesNode node) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is lines.");
        string text = node.RawText ?? Join(node.Lines ?? []);
        site.RequireText(text);
        return TextValue(text);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is JsonObject objectValue) CheckOnly(site.Context, objectValue, site.Path, "$text");
        string text = TextOf(site.Context, value, site.Path, out _);
        site.RequireText(text, value);
        return LinesNode.FromText(text);
    }

    internal static string Join(IReadOnlyList<string> lines)
    {
        var builder = new StringBuilder();
        foreach (var line in lines) builder.Append(line).Append("\r\n");
        return builder.ToString();
    }
}

internal sealed partial class StringsWithValuesCodec
{
    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not LinesWithValuesNode node) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is lines with objects.");
        string text = node.RawText ?? StringsCodec.Join(node.Lines ?? []);
        int lines = LineCount(text);
        if (lines != node.Objects.Count)
            throw site.Fail(Codes.Corrupt, $"The text has {lines} lines and {node.Objects.Count} object values.");
        site.RequireText(text);
        var objects = new JsonArray();
        foreach (var item in node.Objects) objects.Add(new JsonInteger(item));
        // The converter stores an array only when the block rule reproduces the bytes. A line that
        // holds CR, LF or NUL stays $text, the same choice Decode makes for the engine image.
        var canonical = node.RawText is null ? StringsCodec.Lines(text) : null;
        if (canonical is null) return new JsonObject().Add("$text", new JsonString(text)).Add("objects", objects);
        var array = new JsonArray();
        foreach (var line in canonical) array.Add(new JsonString(line));
        return new JsonObject().Add("lines", array).Add("objects", objects);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is not JsonObject objectValue) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an object with lines or $text, and objects.", value);
        var (text, ids) = ReadMembers(site.Context, objectValue, site.Path);
        site.RequireText(text, value);
        return LinesWithValuesNode.FromText(text, ids);
    }
}

internal sealed partial class DataCodec
{
    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not BytesNode node) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is lowercase hexadecimal bytes.");
        if (Size is int n && node.Data.Length != n)
            throw site.Fail(Codes.Range, $"{site.Property?.Name ?? "The value"} holds exactly {n} bytes.", value: Text(node.Data.Length), constraint: $"{n} bytes");
        if (node.Data.Length > site.Context.Limits.DecompressedBytes)
            throw site.Fail(Codes.Limit, "A payload is too large.");
        return new JsonString(LayoutResidue.Hex(node.Data));
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is not JsonString text || !HexText.IsLowerHex(text.Value))
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is lowercase hexadecimal bytes.", value);
        if (text.Value.Length / 2 > site.Context.Limits.DecompressedBytes)
            throw site.Fail(Codes.Limit, "A payload is too large.", text);
        var bytes = HexText.Decode(text.Value);
        if (Size is int n && bytes.Length != n)
            throw site.Fail(Codes.Range, $"{site.Property?.Name ?? "The value"} holds exactly {n} bytes.", text, Text(bytes.Length), $"{n} bytes");
        return new BytesNode(bytes);
    }
}

internal sealed partial class FixedObjectCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not ObjectNode node) throw context.Fail(Codes.Type, (frame.Property.Name) + " is an object.", null, frame.Path);
        ImageTree.WriteBody(node, context.Schema.Get(ClassName), context, data, frame.Path, false);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame) =>
        ImageTree.ReadBody(context, start, end, context.Schema.Get(ClassName), frame.Path, null, null);

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not ObjectNode node) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an object.");
        return DocumentBackend.WriteObject(node, site.Schema.Get(ClassName), site, created: false, slotted: false);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is not JsonObject objectValue) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an object.", value);
        return DocumentBackend.ReadObject(objectValue, site.Schema.Get(ClassName), site, created: false, slotted: false);
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
            throw context.Fail(Codes.Type, frame.Property.Name + " is an object or null.", null, frame.Path);
        var classValue = Resolve(context, name, frame);
        var shape = context.Schema.LegacyShape(frame.Property.Owner.Name + "." + frame.Property.Name, classValue.Name);
        data.Int32(classValue.Id!.Value);
        ImageTree.WriteBody(node, shape?.Shape ?? classValue, context, data, frame.Path, true);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame) =>
        ReadCreated(context, start, end, frame, null);

    // A keyed member's entry stores the same image as a created object. Its id less the slot base is the slot.
    internal override void WriteSlotsImage(TreeValue value, EncodeContext context, ObjectWriter writer, ImageFrame frame)
    {
        if (frame.Property.SlotBase is not int slotBase)
        {
            base.WriteSlotsImage(value, context, writer, frame);
            return;
        }
        string name = frame.Property.Name;
        if (value is not SlotListNode node) throw context.Fail(Codes.Type, name + " is created objects at slots.", null, frame.Path);
        if (node.Entries.Count > context.Limits.CollectionItems)
            throw context.Fail(Codes.Limit, "A collection has too many items.", null, frame.Path, null, "at most " + context.Limits.CollectionItems + " items");
        var used = new HashSet<int>();
        for (int i = 0; i < node.Entries.Count; i++)
        {
            var (slot, item) = node.Entries[i];
            var itemPath = frame.Path.Item(i);
            long id = (long)slot + slotBase;
            if (slot < 0 || id < frame.Property.DynamicMinimum!.Value || id > int.MaxValue)
                throw context.Fail(Codes.Range, "The $slot is outside the stored id range.", null, itemPath.Member("$slot"), Text(slot));
            if (!used.Add(slot))
                throw context.Fail(Codes.Slot, "Slot " + Text(slot) + " of " + name + " is declared twice.", null, itemPath);
            if (item is ObjectNode { Slot: int stated } && stated != slot)
                throw context.Fail(Codes.Slot, "An item of " + name + " states slot " + Text(stated) + " at slot " + Text(slot) + ".", null, itemPath);
            writer.Begin((int)id, frame.Property, i);
            WriteImage(item, context, writer.Data, new ImageFrame { Property = frame.Property, Id = (int)id, Path = itemPath, Parent = frame.Parent });
            writer.End();
        }
    }

    internal override TreeValue ReadSlotsImage(DecodeContext context, IReadOnlyList<SlotImage> entries, ImageFrame frame)
    {
        if (frame.Property.SlotBase is not int slotBase) return base.ReadSlotsImage(context, entries, frame);
        context.CheckItems(entries.Count, entries[0].Start, frame.Path);
        var node = new SlotListNode();
        for (int i = 0; i < entries.Count; i++)
        {
            var (id, start, end) = entries[i];
            var itemFrame = new ImageFrame { Property = frame.Property, Id = id, Path = frame.Path.Item(i), Parent = frame.Parent };
            node.Add(id - slotBase, ReadCreated(context, start, end, itemFrame, id - slotBase));
        }
        return node;
    }

    internal override JsonValue WriteSlotsTree(TreeValue value, DocumentSite site)
    {
        if (site.Property?.SlotBase is not int slotBase) return base.WriteSlotsTree(value, site);
        string name = site.Property.Name;
        if (value is not SlotListNode node) throw site.Mismatch(name + " is created objects at slots.");
        if (node.Entries.Count > site.Context.Limits.CollectionItems)
            throw site.Fail(Codes.Limit, "A collection has too many items.", constraint: $"at most {site.Context.Limits.CollectionItems} items");
        var array = new JsonArray();
        var used = new HashSet<int>();
        for (int i = 0; i < node.Entries.Count; i++)
        {
            var (slot, item) = node.Entries[i];
            var at = site.At(site.Path.Item(i), element: false);
            long id = (long)slot + slotBase;
            if (slot < 0 || id < site.Property.DynamicMinimum!.Value || id > int.MaxValue)
                throw at.At(at.Path.Member("$slot"), element: false).Fail(Codes.Range, "The $slot is outside the stored id range.", value: Text(slot));
            if (!used.Add(slot)) throw at.Fail(Codes.Slot, $"Slot {slot} of {name} is declared twice.");
            if (item is NullNode)
                array.Add(new JsonObject().Add("$class", new JsonNull()).Add("$slot", new JsonInteger(slot)));
            else if (item is ObjectNode { ClassName: string className } objectNode && objectNode.Slot == slot)
                array.Add(DocumentBackend.WriteObject(objectNode, DocumentBackend.RequireClass(at, className, Family, null), at, created: true, slotted: true));
            else throw at.Mismatch($"An item of {name} is a created object or null at its slot.");
        }
        return array;
    }

    internal override TreeValue ReadSlotsTree(JsonValue value, DocumentSite site)
    {
        if (site.Property?.SlotBase is not int slotBase) return base.ReadSlotsTree(value, site);
        string name = site.Property.Name;
        if (value is not JsonArray array) throw site.Mismatch($"{name} is an array.", value);
        if (array.Items.Count > site.Context.Limits.CollectionItems)
            throw site.Fail(Codes.Limit, "A collection has too many items.", array, null, $"at most {site.Context.Limits.CollectionItems} items");
        var node = new SlotListNode();
        var used = new HashSet<long>();
        for (int i = 0; i < array.Items.Count; i++)
        {
            var at = site.At(site.Path.Item(i), element: false);
            if (array.Items[i] is not JsonObject item) throw at.Mismatch($"An item of {name} is an object.", array.Items[i]);
            if (item["$slot"] is not JsonInteger slot) throw at.Fail(Codes.MissingMember, $"An item of {name} needs an integer $slot.", item);
            long id = slot.Value + slotBase;
            if (slot.Value < 0 || id < site.Property.DynamicMinimum!.Value || id > int.MaxValue)
                throw at.At(at.Path.Member("$slot"), element: false).Fail(Codes.Range, "The $slot is outside the stored id range.", slot, Text(slot.Value));
            if (!used.Add(slot.Value)) throw at.Fail(Codes.Slot, $"Slot {slot.Value} of {name} is declared twice.", item);
            if (item["$class"] is JsonNull)
            {
                foreach (var member in item.Members)
                    if (member.Name is not ("$class" or "$slot"))
                        throw at.At(at.Path.Member(member.Name), element: false).Fail(Codes.UnknownMember, "A nil object has no members.", member.Value, member.Name);
                node.Add((int)slot.Value, NullNode.Instance);
                continue;
            }
            if (item["$class"] is not JsonString className) throw at.Fail(Codes.MissingMember, "A created object needs $class.", item);
            var classValue = DocumentBackend.RequireClass(at, className.Value, Family, className);
            node.Add((int)slot.Value, DocumentBackend.ReadObject(item, classValue, at, created: true, slotted: true));
        }
        return node;
    }

    private TreeValue ReadCreated(DecodeContext context, int start, int end, ImageFrame frame, int? slot)
    {
        context.Need(start, 4, end, frame.Path, "class id");
        int classId = context.Int32(start);
        if (classId == 0)
        {
            if (end != start + 4) throw context.Fail(Codes.Corrupt, "Bytes follow a nil object.", start + 4, frame.Path);
            return NullNode.Instance;
        }
        var classValue = context.Schema.Resolve(context.Engine, classId)
            ?? throw context.Fail(Codes.UnknownClass, "Class id " + classId.ToString(System.Globalization.CultureInfo.InvariantCulture) + " is not in the " + context.Engine + " engine table.", start, frame.Path, classId.ToString(System.Globalization.CultureInfo.InvariantCulture));
        var shape = context.Schema.LegacyShape(frame.Property.Owner.Name + "." + frame.Property.Name, classValue.Name);
        if (shape is null && Family is not null && !classValue.InheritsFrom(Family))
            throw context.Fail(Codes.ClassFamily, "Class " + classValue.Name + " is not a " + Family + ".", start, frame.Path, classValue.Name, Family);
        if (shape is { } legacy)
            context.Counts.LegacyRules[legacy.Rule] = context.Counts.LegacyRules.GetValueOrDefault(legacy.Rule) + 1;
        return ImageTree.ReadBody(context, start + 4, end, shape?.Shape ?? classValue, frame.Path, classValue.Name, slot);
    }

    private SchemaClass Resolve(EncodeContext context, string name, ImageFrame frame)
    {
        var classValue = context.Schema.Find(name)
            ?? throw context.Fail(Codes.UnknownClass, "Unknown class " + name + ".", null, frame.Path, name);
        if (classValue.Id is not int id || context.Schema.Resolve(context.Engine, id) != classValue)
            throw context.Fail(Codes.UnknownClass, "Class " + classValue.Name + " is not in the " + context.Engine + " engine table.", null, frame.Path, classValue.Name);
        if (Family is not null && context.Schema.LegacyShape(frame.Property.Owner.Name + "." + frame.Property.Name, classValue.Name) is null && !classValue.InheritsFrom(Family))
            throw context.Fail(Codes.ClassFamily, "Class " + classValue.Name + " is not a " + Family + ".", null, frame.Path, classValue.Name, Family);
        return classValue;
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is NullNode) return new JsonNull();
        if (value is not ObjectNode node || node.ClassName is not string name)
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an object or null.");
        var classValue = DocumentBackend.RequireClass(site, name, Family, null);
        return DocumentBackend.WriteObject(node, classValue, site, created: true, slotted: false);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is JsonNull) return NullNode.Instance;
        if (value is not JsonObject objectValue) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an object or null.", value);
        if (objectValue["$class"] is not JsonString name) throw site.Fail(Codes.MissingMember, "A created object needs $class.", objectValue);
        var classValue = DocumentBackend.RequireClass(site, name.Value, Family, name);
        return DocumentBackend.ReadObject(objectValue, classValue, site, created: true, slotted: false);
    }
}

internal sealed partial class ChildrenCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not ChildListNode node) throw context.Fail(Codes.Type, frame.Property.Name + " is an array.", null, frame.Path);
        if (node.Children.Count > context.Limits.CollectionItems)
            throw context.Fail(Codes.Limit, "A collection has too many items.", null, frame.Path, null, "at most " + context.Limits.CollectionItems + " items");
        var writer = new ObjectWriter();
        var slots = new HashSet<int>();
        for (int i = 0; i < node.Children.Count; i++)
        {
            var child = node.Children[i];
            var itemPath = frame.Path.Item(i);
            if (child.Slot is not int slot || slot < 0)
                throw context.Fail(Codes.MissingMember, "A child needs a non-negative integer $slot.", null, itemPath);
            if (!slots.Add(slot))
                throw context.Fail(Codes.Slot, "Child slot " + slot + " is declared twice.", null, itemPath.Member("$slot"));
            if (child.ClassName is not string name)
                throw context.Fail(Codes.MissingMember, "A child needs a class.", null, itemPath);
            var classValue = context.Schema.Find(name)
                ?? throw context.Fail(Codes.UnknownClass, "Unknown class " + name + ".", null, itemPath, name);
            if (classValue.Id is not int id || context.Schema.Resolve(context.Engine, id) != classValue)
                throw context.Fail(Codes.UnknownClass, "Class " + classValue.Name + " is not in the " + context.Engine + " engine table.", null, itemPath, classValue.Name);
            var shape = context.Schema.LegacyShape(frame.Property.Owner.Name + "." + frame.Property.Name, classValue.Name);
            writer.Begin(slot, frame.Property, i);
            writer.Data.Int32(id);
            ImageTree.WriteBody(child, shape?.Shape ?? classValue, context, writer.Data, itemPath, true);
            writer.End();
        }
        ObjectImage.Place(context, writer, null, frame.Id, null, new JsonObject(), frame.Path, data);
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        var table = PropertyTable.Read(context, start, end, frame.Path);
        int db = table.DataBase;
        int length = end - db;
        var boundaries = table.Entries.Select(entry => entry.Offset).Append(length).Distinct().Order().ToArray();
        var order = table.Entries.OrderBy(entry => entry.Offset).ThenBy(entry => entry.Index).ToList();
        var slots = new HashSet<int>();
        var children = new List<ObjectNode>();
        for (int i = 0; i < order.Count; i++)
        {
            var entry = order[i];
            if (i + 1 < order.Count && order[i + 1].Offset == entry.Offset)
                throw context.Fail(Codes.Layout, "Two children share one offset.", db + entry.Offset, frame.Path);
            if (!slots.Add(entry.Id)) throw context.Fail(Codes.Duplicate, "Child slot " + entry.Id + " is stored twice.", db + entry.Offset, frame.Path);
            int next = boundaries[Array.BinarySearch(boundaries, entry.Offset) + 1];
            int childStart = db + entry.Offset;
            int childEnd = db + next;
            context.Need(childStart, 4, childEnd, frame.Path.Item(children.Count), "class id");
            int classId = context.Int32(childStart);
            if (classId == 0) throw context.Fail(Codes.Type, "A stored child is never nil.", childStart, frame.Path.Item(children.Count));
            var classValue = context.Schema.Resolve(context.Engine, classId)
                ?? throw context.Fail(Codes.UnknownClass, "Class id " + classId.ToString(System.Globalization.CultureInfo.InvariantCulture) + " is not in the " + context.Engine + " engine table.", childStart, frame.Path.Item(children.Count), classId.ToString(System.Globalization.CultureInfo.InvariantCulture));
            var shape = context.Schema.LegacyShape(frame.Property.Owner.Name + "." + frame.Property.Name, classValue.Name);
            if (shape is { } legacy)
                context.Counts.LegacyRules[legacy.Rule] = context.Counts.LegacyRules.GetValueOrDefault(legacy.Rule) + 1;
            var itemPath = frame.Path.Item(children.Count);
            children.Add(ImageTree.ReadBody(context, childStart + 4, childEnd, shape?.Shape ?? classValue, itemPath, classValue.Name, entry.Id));
        }
        if (children.Count > context.Limits.CollectionItems)
            throw context.Fail(Codes.Limit, "A collection has too many items.", start, frame.Path, null, "at most " + context.Limits.CollectionItems + " items");
        return new ChildListNode(children);
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not ChildListNode node) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an array.");
        if (node.Children.Count > site.Context.Limits.CollectionItems)
            throw site.Fail(Codes.Limit, "A collection has too many items.", constraint: $"at most {site.Context.Limits.CollectionItems} items");
        var array = new JsonArray();
        var slots = new HashSet<int>();
        for (int i = 0; i < node.Children.Count; i++)
        {
            var child = node.Children[i];
            var item = site.At(site.Path.Item(i), false);
            if (child.Slot is not int slot || !slots.Add(slot))
                throw item.Fail(Codes.Slot, child.Slot is int used ? $"Child slot {used} is declared twice." : "A child needs a non-negative integer $slot.");
            if (child.ClassName is not string name) throw item.Fail(Codes.MissingMember, "A child needs a class.");
            var classValue = DocumentBackend.RequireClass(item, name, null, null);
            array.Add(DocumentBackend.WriteObject(child, classValue, item, created: true, slotted: true));
        }
        return array;
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is not JsonArray array) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an array.", value);
        if (array.Items.Count > site.Context.Limits.CollectionItems)
            throw site.Fail(Codes.Limit, "A collection has too many items.", array, null, $"at most {site.Context.Limits.CollectionItems} items");
        var children = new List<ObjectNode>(array.Items.Count);
        var slots = new HashSet<int>();
        for (int i = 0; i < array.Items.Count; i++)
        {
            var item = site.At(site.Path.Item(i), false);
            if (array.Items[i] is not JsonObject child) throw item.Mismatch("A child is an object.", array.Items[i]);
            if (child["$class"] is JsonNull) throw item.Fail(Codes.Type, "A stored child is never nil.", child);
            if (child["$class"] is not JsonString name) throw item.Fail(Codes.MissingMember, "A child needs a class.", child);
            if (child["$slot"] is not JsonInteger { Value: >= 0 and <= int.MaxValue } slot)
                throw item.Fail(Codes.MissingMember, "A child needs a non-negative integer $slot.", child);
            if (!slots.Add((int)slot.Value))
                throw item.Fail(Codes.Slot, $"Child slot {slot.Value} is declared twice.", slot);
            var classValue = DocumentBackend.RequireClass(item, name.Value, null, name);
            children.Add(DocumentBackend.ReadObject(child, classValue, item, created: true, slotted: true));
        }
        return new ChildListNode(children);
    }
}

/// <summary>A variable-length list. The schema declares whether it stores one. Version 1 refuses the shape.</summary>
internal sealed class ListCodec(PropertyCodec element, bool storesList) : PropertyCodec
{
    internal override string? ImageRefusal => "The direct path does not store a list.";

    internal override bool AcceptsList(int version) => storesList;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site) =>
        throw new InvalidOperationException("A list has no image.");

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site) =>
        throw new InvalidOperationException("A list has no image.");

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (site.Schema.Version == 1) throw site.Fail(Codes.Type, "Version 1 does not store a list.");
        if (value is not ListNode node) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a list.");
        if (node.Items.Count > site.Context.Limits.CollectionItems)
            throw site.Fail(Codes.Limit, "A collection has too many items.", constraint: $"at most {site.Context.Limits.CollectionItems} items");
        var array = new JsonArray();
        for (int i = 0; i < node.Items.Count; i++)
        {
            var item = site.At(site.Path.Item(i), true);
            var written = element.WriteTree(node.Items[i], item) ?? throw item.Fail(Codes.MissingMember, "A list item stores one value.");
            array.Add(written);
        }
        return array;
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (site.Schema.Version == 1) throw site.Fail(Codes.Type, "Version 1 does not store a list.", value);
        if (value is not JsonArray array) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a list.", value);
        if (array.Items.Count > site.Context.Limits.CollectionItems)
            throw site.Fail(Codes.Limit, "A collection has too many items.", array, null, $"at most {site.Context.Limits.CollectionItems} items");
        var node = new ListNode();
        for (int i = 0; i < array.Items.Count; i++)
        {
            var item = site.At(site.Path.Item(i), true);
            node.Items.Add(element.ReadTree(array.Items[i], item) ?? throw item.Fail(Codes.MissingMember, "A list item stores one value.", array.Items[i]));
        }
        return node;
    }
}

/// <summary>Named fields with no class. Version 1 has no record, and the backend refuses the shape.</summary>
internal sealed class RecordCodec : PropertyCodec
{
    internal override string? ImageRefusal => "A record has no image.";

    private readonly (string Name, PropertyCodec Codec)[] _fields;

    public RecordCodec(JsonObject source, FormatSchema schema, string path)
    {
        _fields = ((JsonArray)source["fields"]!).Items.Cast<JsonObject>().Select(f =>
        {
            string name = ((JsonString)f["name"]!).Value;
            return (name, Create(f, schema, path + "." + name));
        }).ToArray();
    }

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site) =>
        throw new InvalidOperationException("A record has no image.");

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site) =>
        throw new InvalidOperationException("A record has no image.");

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (site.Schema.Version == 1) throw site.Fail(Codes.Type, "Version 1 does not store a record.");
        if (value is not RecordNode node) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a record.");
        var json = new JsonObject();
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var (name, codec) in _fields)
        {
            var field = node.Find(name);
            if (field is null) continue;
            var at = site.At(site.Path.Member(name), false);
            var written = codec.WriteTree(field, at);
            if (written is not null) json.Add(name, written);
            seen.Add(name);
        }
        foreach (var (name, _) in node.Members)
            if (!seen.Contains(name))
                throw site.At(site.Path.Member(name), false).Fail(Codes.UnknownMember, $"The record has no member \"{name}\".", value: name);
        return json;
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (site.Schema.Version == 1) throw site.Fail(Codes.Type, "Version 1 does not store a record.", value);
        if (value is not JsonObject objectValue) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a record.", value);
        var known = _fields.Select(f => f.Name).ToHashSet(StringComparer.Ordinal);
        foreach (var member in objectValue.Members)
        {
            if (member.Name.StartsWith('$') && member.Name != "$text")
                throw site.At(site.Path.Member(member.Name), false).Fail(Codes.ReservedMember,
                    Reserved.IsKnown(member.Name) ? $"Reserved member \"{member.Name}\" is not allowed here." : $"Unknown reserved member \"{member.Name}\".",
                    member.Value, member.Name);
            if (!known.Contains(member.Name))
                throw site.At(site.Path.Member(member.Name), false).Fail(Codes.UnknownMember, $"The record has no member \"{member.Name}\".", member.Value, member.Name);
        }
        var node = new RecordNode();
        foreach (var (name, codec) in _fields)
        {
            var member = objectValue.Find(name);
            if (member is null) continue;
            var read = codec.ReadTree(member.Value, site.At(site.Path.Member(name), false));
            if (read is not null) node.Add(name, read);
        }
        return node;
    }
}
