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
        if (value is JsonObject obj) CheckOnly(site.Context, obj, site.Path, "$text");
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
        if (value is not JsonObject obj) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an object with lines or $text, and objects.", value);
        var (text, ids) = ReadMembers(site.Context, obj, site.Path);
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
    internal override void WriteImage(TreeValue value, EncodeContext ctx, ByteBuffer data, ImageFrame frame)
    {
        if (value is not ObjectNode node) throw ctx.Fail(Codes.Type, (frame.Property.Name) + " is an object.", null, frame.Path);
        ImageTree.WriteBody(node, ctx.Schema.Get(ClassName), ctx, data, frame.Path, false);
    }

    internal override TreeValue ReadImage(DecodeContext ctx, int start, int end, ImageFrame frame) =>
        ImageTree.ReadBody(ctx, start, end, ctx.Schema.Get(ClassName), frame.Path, null, null);

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not ObjectNode node) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an object.");
        return DocumentBackend.WriteObject(node, site.Schema.Get(ClassName), site, created: false, slotted: false);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is not JsonObject obj) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an object.", value);
        return DocumentBackend.ReadObject(obj, site.Schema.Get(ClassName), site, created: false, slotted: false);
    }
}

internal sealed partial class CreatedObjectCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext ctx, ByteBuffer data, ImageFrame frame)
    {
        if (value is NullNode)
        {
            data.Int32(0);
            return;
        }
        if (value is not ObjectNode node || node.ClassName is not string name)
            throw ctx.Fail(Codes.Type, frame.Property.Name + " is an object or null.", null, frame.Path);
        var cls = Resolve(ctx, name, frame);
        var shape = ctx.Schema.LegacyShape(frame.Property.Owner.Name + "." + frame.Property.Name, cls.Name);
        data.Int32(cls.Id!.Value);
        ImageTree.WriteBody(node, shape?.Shape ?? cls, ctx, data, frame.Path, true);
    }

    internal override TreeValue ReadImage(DecodeContext ctx, int start, int end, ImageFrame frame)
    {
        ctx.Need(start, 4, end, frame.Path, "class id");
        int classId = ctx.Int32(start);
        if (classId == 0)
        {
            if (end != start + 4) throw ctx.Fail(Codes.Corrupt, "Bytes follow a nil object.", start + 4, frame.Path);
            return NullNode.Instance;
        }
        var cls = ctx.Schema.Resolve(ctx.Engine, classId)
            ?? throw ctx.Fail(Codes.UnknownClass, "Class id " + classId.ToString(System.Globalization.CultureInfo.InvariantCulture) + " is not in the " + ctx.Engine + " engine table.", start, frame.Path, classId.ToString(System.Globalization.CultureInfo.InvariantCulture));
        var shape = ctx.Schema.LegacyShape(frame.Property.Owner.Name + "." + frame.Property.Name, cls.Name);
        if (shape is null && Family is not null && !cls.InheritsFrom(Family))
            throw ctx.Fail(Codes.ClassFamily, "Class " + cls.Name + " is not a " + Family + ".", start, frame.Path, cls.Name, Family);
        if (shape is { } legacy)
            ctx.Counts.LegacyRules[legacy.Rule] = ctx.Counts.LegacyRules.GetValueOrDefault(legacy.Rule) + 1;
        return ImageTree.ReadBody(ctx, start + 4, end, shape?.Shape ?? cls, frame.Path, cls.Name, null);
    }

    private SchemaClass Resolve(EncodeContext ctx, string name, ImageFrame frame)
    {
        var cls = ctx.Schema.Find(name)
            ?? throw ctx.Fail(Codes.UnknownClass, "Unknown class " + name + ".", null, frame.Path, name);
        if (cls.Id is not int id || ctx.Schema.Resolve(ctx.Engine, id) != cls)
            throw ctx.Fail(Codes.UnknownClass, "Class " + cls.Name + " is not in the " + ctx.Engine + " engine table.", null, frame.Path, cls.Name);
        if (Family is not null && ctx.Schema.LegacyShape(frame.Property.Owner.Name + "." + frame.Property.Name, cls.Name) is null && !cls.InheritsFrom(Family))
            throw ctx.Fail(Codes.ClassFamily, "Class " + cls.Name + " is not a " + Family + ".", null, frame.Path, cls.Name, Family);
        return cls;
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is NullNode) return new JsonNull();
        if (value is not ObjectNode node || node.ClassName is not string name)
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an object or null.");
        var cls = DocumentBackend.RequireClass(site, name, Family, null);
        return DocumentBackend.WriteObject(node, cls, site, created: true, slotted: false);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is JsonNull) return NullNode.Instance;
        if (value is not JsonObject obj) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is an object or null.", value);
        if (obj["$class"] is not JsonString name) throw site.Fail(Codes.MissingMember, "A created object needs $class.", obj);
        var cls = DocumentBackend.RequireClass(site, name.Value, Family, name);
        return DocumentBackend.ReadObject(obj, cls, site, created: true, slotted: false);
    }
}

internal sealed partial class ChildrenCodec
{
    internal override void WriteImage(TreeValue value, EncodeContext ctx, ByteBuffer data, ImageFrame frame)
    {
        if (value is not ChildListNode node) throw ctx.Fail(Codes.Type, frame.Property.Name + " is an array.", null, frame.Path);
        if (node.Children.Count > ctx.Limits.CollectionItems)
            throw ctx.Fail(Codes.Limit, "A collection has too many items.", null, frame.Path, null, "at most " + ctx.Limits.CollectionItems + " items");
        var writer = new ObjectWriter();
        var slots = new HashSet<int>();
        for (int i = 0; i < node.Children.Count; i++)
        {
            var child = node.Children[i];
            var itemPath = frame.Path.Item(i);
            if (child.Slot is not int slot || slot < 0)
                throw ctx.Fail(Codes.MissingMember, "A child needs a non-negative integer $slot.", null, itemPath);
            if (!slots.Add(slot))
                throw ctx.Fail(Codes.Slot, "Child slot " + slot + " is declared twice.", null, itemPath.Member("$slot"));
            if (child.ClassName is not string name)
                throw ctx.Fail(Codes.MissingMember, "A child needs a class.", null, itemPath);
            var cls = ctx.Schema.Find(name)
                ?? throw ctx.Fail(Codes.UnknownClass, "Unknown class " + name + ".", null, itemPath, name);
            if (cls.Id is not int id || ctx.Schema.Resolve(ctx.Engine, id) != cls)
                throw ctx.Fail(Codes.UnknownClass, "Class " + cls.Name + " is not in the " + ctx.Engine + " engine table.", null, itemPath, cls.Name);
            var shape = ctx.Schema.LegacyShape(frame.Property.Owner.Name + "." + frame.Property.Name, cls.Name);
            writer.Begin(slot, frame.Property, i);
            writer.Data.Int32(id);
            ImageTree.WriteBody(child, shape?.Shape ?? cls, ctx, writer.Data, itemPath, true);
            writer.End();
        }
        ObjectImage.Place(ctx, writer, null, frame.Id, null, new JsonObject(), frame.Path, data);
    }

    internal override TreeValue ReadImage(DecodeContext ctx, int start, int end, ImageFrame frame)
    {
        var table = PropertyTable.Read(ctx, start, end, frame.Path);
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
                throw ctx.Fail(Codes.Layout, "Two children share one offset.", db + entry.Offset, frame.Path);
            if (!slots.Add(entry.Id)) throw ctx.Fail(Codes.Duplicate, "Child slot " + entry.Id + " is stored twice.", db + entry.Offset, frame.Path);
            int next = boundaries[Array.BinarySearch(boundaries, entry.Offset) + 1];
            int childStart = db + entry.Offset;
            int childEnd = db + next;
            ctx.Need(childStart, 4, childEnd, frame.Path.Item(children.Count), "class id");
            int classId = ctx.Int32(childStart);
            if (classId == 0) throw ctx.Fail(Codes.Type, "A stored child is never nil.", childStart, frame.Path.Item(children.Count));
            var cls = ctx.Schema.Resolve(ctx.Engine, classId)
                ?? throw ctx.Fail(Codes.UnknownClass, "Class id " + classId.ToString(System.Globalization.CultureInfo.InvariantCulture) + " is not in the " + ctx.Engine + " engine table.", childStart, frame.Path.Item(children.Count), classId.ToString(System.Globalization.CultureInfo.InvariantCulture));
            var shape = ctx.Schema.LegacyShape(frame.Property.Owner.Name + "." + frame.Property.Name, cls.Name);
            if (shape is { } legacy)
                ctx.Counts.LegacyRules[legacy.Rule] = ctx.Counts.LegacyRules.GetValueOrDefault(legacy.Rule) + 1;
            var itemPath = frame.Path.Item(children.Count);
            children.Add(ImageTree.ReadBody(ctx, childStart + 4, childEnd, shape?.Shape ?? cls, itemPath, cls.Name, entry.Id));
        }
        if (children.Count > ctx.Limits.CollectionItems)
            throw ctx.Fail(Codes.Limit, "A collection has too many items.", start, frame.Path, null, "at most " + ctx.Limits.CollectionItems + " items");
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
            var cls = DocumentBackend.RequireClass(item, name, null, null);
            array.Add(DocumentBackend.WriteObject(child, cls, item, created: true, slotted: true));
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
            var cls = DocumentBackend.RequireClass(item, name.Value, null, name);
            children.Add(DocumentBackend.ReadObject(child, cls, item, created: true, slotted: true));
        }
        return new ChildListNode(children);
    }
}

/// <summary>A variable-length list. The schema declares whether it stores one. Version 1 refuses the shape.</summary>
internal sealed class ListCodec(PropertyCodec element, bool storesList) : PropertyCodec
{
    internal override string? ImageRefusal => "The direct path does not store a list.";

    internal override bool AcceptsList(int version) => storesList;

    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site) =>
        throw new InvalidOperationException("A list has no image.");

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site) =>
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

    private readonly (string Name, PropertyCodec Codec)[] fields;

    public RecordCodec(JsonObject source, FormatSchema schema, string path)
    {
        fields = ((JsonArray)source["fields"]!).Items.Cast<JsonObject>().Select(f =>
        {
            string name = ((JsonString)f["name"]!).Value;
            return (name, Create(f, schema, path + "." + name));
        }).ToArray();
    }

    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site) =>
        throw new InvalidOperationException("A record has no image.");

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site) =>
        throw new InvalidOperationException("A record has no image.");

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (site.Schema.Version == 1) throw site.Fail(Codes.Type, "Version 1 does not store a record.");
        if (value is not RecordNode node) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a record.");
        var json = new JsonObject();
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var (name, codec) in fields)
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
        if (value is not JsonObject obj) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a record.", value);
        var known = fields.Select(f => f.Name).ToHashSet(StringComparer.Ordinal);
        foreach (var member in obj.Members)
        {
            if (member.Name.StartsWith('$') && member.Name != "$text")
                throw site.At(site.Path.Member(member.Name), false).Fail(Codes.ReservedMember,
                    Reserved.IsKnown(member.Name) ? $"Reserved member \"{member.Name}\" is not allowed here." : $"Unknown reserved member \"{member.Name}\".",
                    member.Value, member.Name);
            if (!known.Contains(member.Name))
                throw site.At(site.Path.Member(member.Name), false).Fail(Codes.UnknownMember, $"The record has no member \"{member.Name}\".", member.Value, member.Name);
        }
        var node = new RecordNode();
        foreach (var (name, codec) in fields)
        {
            var member = obj.Find(name);
            if (member is null) continue;
            var read = codec.ReadTree(member.Value, site.At(site.Path.Member(name), false));
            if (read is not null) node.Add(name, read);
        }
        return node;
    }
}
