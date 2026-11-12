using System.Globalization;
using Aow.Formats.Json;
using Aow.Formats.Schema;

namespace Aow.Formats.Original;

/// <summary>Where a value sits while it is decoded or encoded.</summary>
internal sealed class ValueSite
{
    public required SchemaProperty Property { get; init; }
    public required int Id { get; init; }
    public required DocumentNodePath Path { get; init; }
    public required JsonObject Owner { get; init; }
    public LayoutBuilder? Builder { get; init; }
    public LayoutReader? Reader { get; init; }
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
/// (persistence.md 1.3). Decoding is driven by the class schema and keeps every byte. A table
/// or data arrangement that the original writer does not produce from the decoded values
/// becomes layout residue.
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
        var table = PropertyTable.Read(context, start, end, path);
        int db = table.DataBase;
        int length = end - db;
        var slots = new List<Slot>(table.Entries.Count);
        var seen = new Dictionary<int, TableEntry>();
        foreach (var e in table.Entries)
        {
            var prop = classValue.PropertyById(e.Id)
                ?? throw context.Fail(Codes.UnknownProperty, $"Class {classValue.Name} has no property {e.Id}.", db + e.Offset, path, e.Id.ToString(CultureInfo.InvariantCulture));
            if (!seen.TryAdd(e.Id, e))
                throw context.Fail(Codes.Duplicate, $"{(prop.IsDynamic ? "Dynamic id" : "Property")} {e.Id} of class {classValue.Name} is stored twice.", start, path);
            slots.Add(new Slot(e, prop));
        }

        // Each value runs to the next stored offset. Booleans have no bytes.
        var boundaries = slots.Select(s => s.Entry.Offset).Append(length).Distinct().Order().ToArray();
        var nonZeroAt = new Dictionary<int, int>();
        foreach (var s in slots)
        {
            s.Start = db + s.Entry.Offset;
            if (s.Property.Codec.ZeroSize) s.End = s.Start;
            else
            {
                int next = Array.BinarySearch(boundaries, s.Entry.Offset);
                s.End = db + (next + 1 < boundaries.Length ? boundaries[next + 1] : length);
                nonZeroAt[s.Entry.Offset] = nonZeroAt.GetValueOrDefault(s.Entry.Offset) + 1;
                if (nonZeroAt[s.Entry.Offset] > 1)
                    throw context.Fail(Codes.Layout, $"Two valued properties of class {classValue.Name} share offset {s.Entry.Offset}.", s.Start, path);
            }
        }

        var builder = new LayoutBuilder();
        // Bytes that belong to no value are gaps.
        var gaps = new List<(int Offset, int Length)>();
        int cursor = 0;
        foreach (var s in slots.Where(s => s.End > s.Start).OrderBy(s => s.Start))
        {
            int offset = s.Start - db;
            if (offset > cursor) gaps.Add((cursor, offset - cursor));
            cursor = Math.Max(cursor, s.End - db);
        }
        if (cursor < length) gaps.Add((cursor, length - cursor));

        var order = slots.OrderBy(s => s.Entry.Offset).ThenBy(s => s.End > s.Start ? 1 : 0).ThenBy(s => s.Entry.Index).ToList();
        var byProperty = new Dictionary<SchemaProperty, List<Slot>>();
        foreach (var s in order)
        {
            if (!byProperty.TryGetValue(s.Property, out var list)) byProperty[s.Property] = list = [];
            list.Add(s);
        }

        foreach (var prop in classValue.Properties)
        {
            if (!byProperty.TryGetValue(prop, out var list)) continue;
            var memberPath = path.Member(prop.Name);
            JsonValue value;
            if (prop.IsDynamic)
            {
                var array = new JsonArray();
                context.CheckItems(list.Count, start, memberPath);
                for (int k = 0; k < list.Count; k++)
                {
                    var s = list[k];
                    var site = new ValueSite { Property = prop, Id = s.Entry.Id, Path = memberPath.Item(k), Owner = target, Builder = builder };
                    array.Add(prop.Codec.DecodeElement(context, s.Start, s.End, site));
                }
                value = array;
            }
            else if (prop.DataId is int dataId)
            {
                var first = list.FirstOrDefault(s => s.Entry.Id == prop.Id);
                var second = list.FirstOrDefault(s => s.Entry.Id == dataId);
                var site = new ValueSite { Property = prop, Id = prop.Id!.Value, Path = memberPath, Owner = target, Builder = builder };
                value = prop.Codec.DecodePair(context, first is null ? null : (first.Start, first.End), second is null ? null : (second.Start, second.End), site);
            }
            else
            {
                var s = list[0];
                var site = new ValueSite { Property = prop, Id = s.Entry.Id, Path = memberPath, Owner = target, Builder = builder };
                value = prop.Codec.Decode(context, s.Start, s.End, site);
            }
            target.Members.Add(new JsonMember(prop.Name, value));
        }

        if (!IsCanonical(classValue, table, order, gaps.Count))
        {
            builder.Table(table.Entries, table.EmptyLongMarker, null);
            foreach (var (offset, count) in gaps) builder.Gap(offset, context.Data.AsSpan(db + offset, count), null);
        }
        builder.Attach(target, context.Profile, context.Counts);
        // A mapped class shows its case. The layout fingerprint covers the stored members, which
        // the export rebuilds from the case before it reads the layout.
        if (classValue.Mapping is { } mapping) CaseCodec.ToDocument(context, classValue, mapping, target, path, start);
        context.Leave();
    }

    private static bool IsCanonical(SchemaClass classValue, PropertyTable table, List<Slot> order, int gapCount)
    {
        if (gapCount != 0 || table.EmptyLongMarker) return false;
        int lastOrdinal = -1;
        int lastId = int.MinValue;
        int element = 0;
        foreach (var s in order)
        {
            int ordinal = classValue.Ordinal(s.Property);
            if (ordinal < lastOrdinal) return false;
            if (ordinal == lastOrdinal)
            {
                if (s.Property.IsDynamic) element++;
                else if (s.Property.DataId is null || s.Entry.Id <= lastId) return false;
            }
            else element = 0;
            if (s.Property.IsDynamic && s.Property.SlotBase is null && s.Entry.Id != s.Property.DynamicMinimum + element) return false;
            lastOrdinal = ordinal;
            lastId = s.Entry.Id;
        }
        return MatchesWriterTable(table.Entries, order.Select(s => s.Entry).ToList());
    }

    /// <summary>Whether the stored entries are what the writer makes from the data order.</summary>
    internal static bool MatchesWriterTable(IReadOnlyList<TableEntry> stored, IReadOnlyList<TableEntry> dataOrder)
    {
        int k = 0;
        foreach (var e in dataOrder.Where(e => PropertyTable.IsShort(e.Id, e.Offset)))
        {
            var s = stored[k++];
            if (s.Id != e.Id || s.Offset != e.Offset || s.Long) return false;
        }
        foreach (var e in dataOrder.Where(e => !PropertyTable.IsShort(e.Id, e.Offset)))
        {
            var s = stored[k++];
            if (s.Id != e.Id || s.Offset != e.Offset || !s.Long) return false;
        }
        return k == stored.Count;
    }

    /// <summary>Checks the members of an object against its class and the allowed reserved names.</summary>
    public static void CheckMembers(EncodeContext context, JsonObject objectValue, SchemaClass classValue, DocumentNodePath path, params string[] reserved)
    {
        foreach (var m in objectValue.Members)
        {
            if (m.Name.StartsWith('$'))
            {
                if (m.Name == "$layout" || Array.IndexOf(reserved, m.Name) >= 0) continue;
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
                throw new ConversionException(new FormatDiagnostic(Codes.UnknownMember, $"Class {classValue.Name} has no member \"{m.Name}\".")
                {
                    File = context.File,
                    Span = m.NameSpan,
                    Path = path.Member(m.Name).ToString(),
                    Value = m.Name,
                });
        }
    }

    public static void Encode(EncodeContext context, JsonObject objectValue, SchemaClass classValue, ByteBuffer output, DocumentNodePath path, params string[] reserved)
    {
        if (classValue.Mapping is { } mapping) objectValue = CaseCodec.FromDocument(context, classValue, mapping, objectValue, path);
        context.Enter(objectValue, path);
        CheckMembers(context, objectValue, classValue, path, reserved);
        var layout = LayoutReader.Read(context, objectValue, path);
        var writer = new ObjectWriter();
        foreach (var prop in classValue.Properties)
        {
            var member = objectValue.Find(prop.Name) ?? prop.OldNames.Select(objectValue.Find).FirstOrDefault(found => found is not null);
            if (member is null) continue;
            var memberPath = path.Member(prop.Name);
            if (prop.IsDynamic)
            {
                if (member.Value is not JsonArray array) throw context.Fail(Codes.Type, $"{prop.Name} is an array.", member.Value, memberPath);
                if (array.Items.Count > context.Limits.CollectionItems)
                    throw context.Fail(Codes.Limit, "A collection has too many items.", array, memberPath, null, $"at most {context.Limits.CollectionItems} items");
                var slots = new HashSet<int>();
                for (int k = 0; k < array.Items.Count; k++)
                {
                    var itemPath = memberPath.Item(k);
                    int id = prop.Codec.ElementId(context, array.Items[k], prop, k, itemPath);
                    if (!slots.Add(id))
                        throw context.Fail(Codes.Slot, $"Slot {id - (prop.SlotBase ?? 0)} of {prop.Name} is declared twice.", array.Items[k], itemPath);
                    var site = new ValueSite { Property = prop, Id = id, Path = itemPath, Owner = objectValue, Reader = layout };
                    writer.Begin(id, prop, k);
                    prop.Codec.EncodeElement(context, array.Items[k], writer.Data, site);
                    writer.End();
                }
            }
            else
            {
                var site = new ValueSite { Property = prop, Id = prop.Id!.Value, Path = memberPath, Owner = objectValue, Reader = layout };
                prop.Codec.EncodeMember(context, member.Value, writer, site);
            }
        }
        Place(context, writer, layout, null, classValue.PropertyById, objectValue, path, output);
        context.Leave();
    }

    /// <summary>
    /// Writes the table and the data. Without a valid table part the writer's canonical form is
    /// used. With one, every value goes to its retained offset and every byte must be covered
    /// once by a value or a gap part, so the result decodes to the same values.
    /// </summary>
    public static void Place(EncodeContext context, ObjectWriter writer, LayoutReader? layout, int? property, Func<int, SchemaProperty?>? resolve,
        JsonValue at, DocumentNodePath path, ByteBuffer output)
    {
        var table = layout?.Table(property);
        if (table is null)
        {
            if (layout is not null && layout.GapsOf(property).Any())
                throw context.Fail(Codes.Layout, "A gap part needs the table part of its object.", at, path);
            var entries = new List<(int, int)>(writer.Items.Count);
            foreach (var item in writer.Items) entries.Add((item.Id, item.Start));
            PropertyTable.WriteCanonical(output, entries, path, context);
            output.Bytes(writer.Data.Span);
            return;
        }
        var (stored, longMarker, tableAt) = table.Value;
        ConversionException Bad(string message) => context.Fail(Codes.Layout, "The retained table does not fit the values: " + message, tableAt, path);

        // Match entries to items: static by id, dynamic members by position in data order.
        var itemsById = new Dictionary<int, EncodedItem>();
        foreach (var item in writer.Items)
            if (!item.IsSequential) itemsById[item.Id] = item;
        var dynamicQueues = writer.Items.Where(i => i.IsSequential)
            .GroupBy(i => i.Property!).ToDictionary(g => g.Key, g => new Queue<EncodedItem>(g));
        var dataOrder = stored.OrderBy(e => e.Offset).ThenBy(e => e.Index).ToList();
        var placed = new List<(TableEntry Entry, EncodedItem Item)>();
        foreach (var e in dataOrder)
        {
            if (itemsById.Remove(e.Id, out var item))
            {
                placed.Add((e, item));
                continue;
            }
            var prop = resolve?.Invoke(e.Id);
            if (prop is null || !dynamicQueues.TryGetValue(prop, out var queue) || queue.Count == 0)
                throw Bad($"no value has id {e.Id}");
            placed.Add((e, queue.Dequeue() with { Id = e.Id }));
        }
        if (placed.Count != writer.Items.Count || itemsById.Count > 0 || dynamicQueues.Values.Any(q => q.Count > 0))
            throw Bad("some values have no entry");
        long size = 0;
        foreach (var (e, item) in placed) size = Math.Max(size, (long)e.Offset + item.Length);
        foreach (var (offset, bytes, _) in layout!.GapsOf(property)) size = Math.Max(size, (long)offset + bytes.Length);
        if (size > context.Limits.DecompressedBytes) throw Bad("it is too large");
        var data = new byte[size];
        var covered = new bool[size];
        void Cover(int offset, ReadOnlySpan<byte> bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                if (covered[offset + i]) throw Bad($"byte {offset + i} is covered twice");
                covered[offset + i] = true;
            }
            bytes.CopyTo(data.AsSpan(offset));
        }
        foreach (var (e, item) in placed) Cover(e.Offset, writer.Data.Span.Slice(item.Start, item.Length));
        foreach (var (offset, bytes, _) in layout.GapsOf(property)) Cover(offset, bytes);
        if (covered.Any(c => !c)) throw Bad("some bytes are not covered");
        var boundaries = placed.Select(p => p.Entry.Offset).Append((int)size).Distinct().Order().ToArray();
        foreach (var (e, item) in placed)
        {
            if (item.Length == 0) continue;
            int next = Array.BinarySearch(boundaries, e.Offset);
            if (boundaries[next + 1] != e.Offset + item.Length) throw Bad($"the value of id {e.Id} decodes with a different length");
        }
        // The table keeps its stored order: short entries first, then long ones.
        int shortCount = 0, longCount = 0;
        bool seenLong = false;
        var entryOf = placed.ToDictionary(p => p.Entry.Index, p => p.Entry);
        foreach (var e in stored)
        {
            var actual = entryOf[e.Index];
            if (actual.Long)
            {
                seenLong = true;
                longCount++;
            }
            else
            {
                if (seenLong) throw Bad("a short entry follows a long one");
                if (!PropertyTable.IsShort(actual.Id, actual.Offset)) throw Bad($"id {actual.Id} at {actual.Offset} cannot be short");
                shortCount++;
            }
        }
        if (shortCount > 127) throw Bad("more than 127 short entries");
        if (longCount != 0 || longMarker)
        {
            output.Byte((byte)(shortCount | 0x80));
            output.Int32(longCount);
        }
        else output.Byte((byte)shortCount);
        foreach (var e in stored)
        {
            var actual = entryOf[e.Index];
            if (actual.Long)
            {
                output.Int32(actual.Id);
                output.Int32(actual.Offset);
            }
            else
            {
                output.Byte((byte)actual.Id);
                output.Byte((byte)actual.Offset);
            }
        }
        output.Bytes(data);
    }
}

/// <summary>The reserved members and their total order (own-formats.md 2.4).</summary>
public static class Reserved
{
    public static readonly string[] Order =
        ["$format", "$version", "$kind", "$container", "$mods", "$class", "$case", "$slot", "$ext", "$residue", "$padding", "$bits", "$text", "$layout"];

    public static bool IsKnown(string name) => Array.IndexOf(Order, name) >= 0;
}
