using System.Globalization;
using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Original;

/// <summary>Where a value sits while it is decoded or encoded.</summary>
internal sealed class ValueSite
{
    public required SchemaProperty Property { get; init; }
    public required int Id { get; init; }
    public required DocumentNodePath Path { get; init; }
    public required JsonObject Owner { get; init; }
}

/// <summary>One encoded value of an object, before the table places it.</summary>
internal readonly record struct EncodedItem(int Id, int Start, int Length, SchemaProperty? Property, int Element)
{
    public bool IsSequential => Property is { IsDynamic: true, SlotBase: null };
}

/// <summary>Collects the encoded values of one object into its data buffer.</summary>
internal sealed class ObjectWriter
{
    public ByteBuffer Data { get; } = new();
    public List<EncodedItem> Items { get; } = [];

    private int _start = -1;
    private int _id;
    private SchemaProperty? _property;
    private int _element;

    public void Begin(int itemId, SchemaProperty? prop, int elementIndex = -1)
    {
        _start = Data.Length;
        _id = itemId;
        _property = prop;
        _element = elementIndex;
    }

    public void End()
    {
        Items.Add(new EncodedItem(_id, _start, Data.Length - _start, _property, _element));
        _start = -1;
    }
}

/// <summary>
/// The image of one stored object: a property table followed by the data of its properties
/// (persistence.md 1.3). Decoding is driven by the class schema, skips ids the schema declares
/// skipped on read, and refuses every other undeclared id. Encoding always writes the canonical
/// table and property order.
/// </summary>
internal static class ObjectImage
{
    private sealed class Slot(TableEntry entry, SchemaProperty property)
    {
        public TableEntry Entry { get; } = entry;
        public SchemaProperty Property { get; } = property;
        public int Start { get; set; }
        public int End { get; set; }
    }

    public static void Decode(DecodeContext context, int start, int end, SchemaClass classValue, DocumentNodePath path, JsonObject target)
    {
        context.Enter(start, path);
        PropertyTable table = PropertyTable.Read(context, start, end, path);
        int db = table.DataBase;
        int length = end - db;
        List<Slot> slots = [with(table.Entries.Count)];
        Dictionary<int, TableEntry> seen = [];
        foreach (TableEntry e in table.Entries)
        {
            if (classValue.IsSkippedId(e.Id))
            {
                if (!seen.TryAdd(e.Id, e))
                {
                    throw context.Fail(Codes.Duplicate, $"Property {e.Id} of class {classValue.Name} is stored twice.", start, path);
                }

                continue;
            }

            SchemaProperty prop = classValue.PropertyById(e.Id)
                ?? throw context.Fail(Codes.UnknownProperty, $"Class {classValue.Name} has no property {e.Id}.", db + e.Offset, path, e.Id.ToString(CultureInfo.InvariantCulture));
            if (!seen.TryAdd(e.Id, e))
            {
                throw context.Fail(Codes.Duplicate, $"{(prop.IsDynamic ? "Dynamic id" : "Property")} {e.Id} of class {classValue.Name} is stored twice.", start, path);
            }

            slots.Add(new Slot(e, prop));
        }

        // Each value runs to the next stored offset, including offsets of skipped ids. Booleans have no bytes.
        int[] boundaries = [.. table.Entries.Select(e => e.Offset).Append(length).Distinct().Order()];
        Dictionary<int, int> nonZeroAt = [];
        foreach (Slot s in slots)
        {
            s.Start = db + s.Entry.Offset;
            if (s.Property.Codec.ZeroSize)
            {
                s.End = s.Start;
            }
            else
            {
                int next = Array.BinarySearch(boundaries, s.Entry.Offset);
                s.End = db + (next + 1 < boundaries.Length ? boundaries[next + 1] : length);
                nonZeroAt[s.Entry.Offset] = nonZeroAt.GetValueOrDefault(s.Entry.Offset) + 1;
                if (nonZeroAt[s.Entry.Offset] > 1)
                {
                    throw context.Fail(Codes.Layout, $"Two valued properties of class {classValue.Name} share offset {s.Entry.Offset}.", s.Start, path);
                }
            }
        }

        List<Slot> order = [.. slots.OrderBy(s => s.Entry.Offset).ThenBy(s => s.End > s.Start ? 1 : 0).ThenBy(s => s.Entry.Index)];
        Dictionary<SchemaProperty, List<Slot>> byProperty = [];
        foreach (Slot s in order)
        {
            if (!byProperty.TryGetValue(s.Property, out List<Slot>? list))
            {
                byProperty[s.Property] = list = [];
            }

            list.Add(s);
        }

        foreach (SchemaProperty prop in classValue.Properties)
        {
            if (!byProperty.TryGetValue(prop, out List<Slot>? list))
            {
                continue;
            }

            DocumentNodePath memberPath = path.Member(prop.Name);
            JsonValue value;
            if (prop.IsDynamic)
            {
                JsonArray array = new();
                context.CheckItems(list.Count, start, memberPath);
                for (int k = 0; k < list.Count; k++)
                {
                    Slot s = list[k];
                    ValueSite site = new() { Property = prop, Id = s.Entry.Id, Path = memberPath.Item(k), Owner = target };
                    _ = array.Add(prop.Codec.DecodeElement(context, s.Start, s.End, site));
                }
                value = array;
            }
            else if (prop.Codec.GroupsEntries)
            {
                SlotImage[] images = new SlotImage[list.Count];
                for (int k = 0; k < list.Count; k++)
                {
                    images[k] = new SlotImage(list[k].Entry.Id, list[k].Start, list[k].End);
                }

                ValueSite site = new() { Property = prop, Id = prop.Id!.Value, Path = memberPath, Owner = target };
                value = prop.Codec.DecodeGroup(context, images, site);
            }
            else if (prop.DataId is int dataId)
            {
                Slot? first = list.FirstOrDefault(s => s.Entry.Id == prop.Id);
                Slot? second = list.FirstOrDefault(s => s.Entry.Id == dataId);
                ValueSite site = new() { Property = prop, Id = prop.Id!.Value, Path = memberPath, Owner = target };
                value = prop.Codec.DecodePair(context, first is null ? null : (first.Start, first.End), second is null ? null : (second.Start, second.End), site);
            }
            else
            {
                Slot s = list[0];
                ValueSite site = new() { Property = prop, Id = s.Entry.Id, Path = memberPath, Owner = target };
                value = prop.Codec.Decode(context, s.Start, s.End, site);
            }
            target.Members.Add(new JsonMember(prop.Name, value));
        }

        // A mapped class shows its case.
        if (classValue.Mapping is { } mapping)
        {
            CaseCodec.ToDocument(context, classValue, mapping, target, path, start);
        }

        context.Leave();
    }

    /// <summary>Checks the members of an object against its class and the allowed reserved names.</summary>
    public static void CheckMembers(EncodeContext context, JsonObject objectValue, SchemaClass classValue, DocumentNodePath path, params string[] reserved)
    {
        foreach (JsonMember m in objectValue.Members)
        {
            if (m.Name.StartsWith('$'))
            {
                if (Array.IndexOf(reserved, m.Name) >= 0)
                {
                    continue;
                }

                throw new ConversionException(new FormatDiagnostic(Codes.ReservedMember,
                    Reserved.IsKnown(m.Name) ? $"Reserved member \"{m.Name}\" is not allowed here." : $"Unknown reserved member \"{m.Name}\".")
                {
                    File = context.File,
                    Span = m.NameSpan,
                    Path = path.Member(m.Name).ToString(),
                    Value = m.Name,
                });
            }
            if (classValue.PropertyByName(m.Name) is null)
            {
                throw new ConversionException(new FormatDiagnostic(Codes.UnknownMember, $"Class {classValue.Name} has no member \"{m.Name}\".")
                {
                    File = context.File,
                    Span = m.NameSpan,
                    Path = path.Member(m.Name).ToString(),
                    Value = m.Name,
                });
            }
        }
    }

    public static void Encode(EncodeContext context, JsonObject objectValue, SchemaClass classValue, ByteBuffer output, DocumentNodePath path, params string[] reserved)
    {
        if (classValue.Mapping is { } mapping)
        {
            objectValue = CaseCodec.FromDocument(context, classValue, mapping, objectValue, path);
        }

        context.Enter(objectValue, path);
        CheckMembers(context, objectValue, classValue, path, reserved);
        ObjectWriter writer = new();
        foreach (SchemaProperty prop in classValue.Properties)
        {
            JsonMember? member = objectValue.Find(prop.Name);
            if (member is null)
            {
                continue;
            }

            DocumentNodePath memberPath = path.Member(prop.Name);
            if (!TransportProfile.Carries(context.Profile, prop))
            {
                throw TransportProfile.Unrepresentable(context, classValue, prop, memberPath);
            }

            if (prop.IsDynamic)
            {
                if (member.Value is not JsonArray array)
                {
                    throw context.Fail(Codes.Type, $"{prop.Name} is an array.", member.Value, memberPath);
                }

                if (array.Items.Count > context.Limits.CollectionItems)
                {
                    throw context.Fail(Codes.Limit, "A collection has too many items.", array, memberPath, null, $"at most {context.Limits.CollectionItems} items");
                }

                HashSet<int> slots = [];
                for (int k = 0; k < array.Items.Count; k++)
                {
                    DocumentNodePath itemPath = memberPath.Item(k);
                    int id = prop.Codec.ElementId(context, array.Items[k], prop, k, itemPath);
                    if (!slots.Add(id))
                    {
                        throw context.Fail(Codes.Slot, $"Slot {id - (prop.SlotBase ?? 0)} of {prop.Name} is declared twice.", array.Items[k], itemPath);
                    }

                    ValueSite site = new() { Property = prop, Id = id, Path = itemPath, Owner = objectValue };
                    writer.Begin(id, prop, k);
                    prop.Codec.EncodeElement(context, array.Items[k], writer.Data, site);
                    writer.End();
                }
            }
            else
            {
                ValueSite site = new() { Property = prop, Id = prop.Id!.Value, Path = memberPath, Owner = objectValue };
                prop.Codec.EncodeMember(context, member.Value, writer, site);
            }
        }
        Place(context, writer: writer, path: path, output: output);
        context.Leave();
    }

    /// <summary>Writes the canonical property table followed by the encoded data.</summary>
    public static void Place(EncodeContext context, ObjectWriter writer, DocumentNodePath path, ByteBuffer output)
    {
        List<(int, int)> entries = [with(writer.Items.Count)];
        foreach (EncodedItem item in writer.Items)
        {
            entries.Add((item.Id, item.Start));
        }

        PropertyTable.WriteCanonical(output, entries, path, context);
        output.Bytes(writer.Data.Span);
    }
}

/// <summary>The reserved members and their total order (own-formats.md 2.4).</summary>
public static class Reserved
{
    public static string[] Order { get; } = ["$format", "$version", "$kind", "$container", "$mods", "$class", "$case", "$slot", "$ext", "$padding", "$bits", "$ilb", "$text"];

    public static bool IsKnown(string name) => Array.IndexOf(Order, name) >= 0;
}
