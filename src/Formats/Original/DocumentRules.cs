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

internal sealed partial class BoolCodec
{
    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not BoolNode node) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a boolean.");
        if (!node.Value && !site.Element) return null;
        return new JsonBool(node.Value);
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is not JsonBool node) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a boolean.", value);
        if (!node.Value && !site.Element) return null;
        return new BoolNode(node.Value);
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

internal sealed partial class StringsExCodec
{
    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is not LinesExNode node) throw site.Mismatch($"{site.Property?.Name ?? "The value"} is lines with objects.");
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
        return LinesExNode.FromText(text, ids);
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
