using System.Buffers.Binary;
using Aow.Formats.Json;
using Aow.Formats.Schema;

namespace Aow.Formats.Original;

/// <summary>
/// TIntegerList and TByteList streams: the default value, the 32-bit count, then the values
/// where a default value is followed by a run length. The document holds the default and the
/// decoded items. A run split the writer does not make is kept as a value-encoding part.
/// The tree holds a <see cref="Tree.DefaultListNode"/>. The direct path writes the canonical runs,
/// splitting a byte run longer than 255, and reads any run split the stream decodes.
/// From version 4 an element names the enum or the record each value is, and the schema
/// annotates the reference domain of the values. The domain changes no value.
/// </summary>
internal sealed class RunListCodec(int width, ListElement? element = null, string? domain = null) : PropertyCodec
{
    public int Width => width;

    /// <summary>The enum or record of each value, or null for an integer.</summary>
    public ListElement? Element { get; } = element;

    /// <summary>The reference domain of the values, an annotation only.</summary>
    public string? Domain { get; } = domain;

    private long Min => width == 1 ? 0 : int.MinValue;
    private long Max => width == 1 ? 255 : int.MaxValue;

    internal override void WriteImage(Tree.TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        string name = frame.Property?.Name ?? "The value";
        if (value is not Tree.DefaultListNode node || node.Width != width)
            throw context.Fail(Codes.Type, name + " is a list with a default.", null, frame.Path);
        if (node.Items.Count > context.Limits.CollectionItems)
            throw context.Fail(Codes.Limit, "A collection has too many items.", null, frame.Path, null, "at most " + context.Limits.CollectionItems + " items");
        if (node.Default < Min || node.Default > Max)
            throw context.Fail(Codes.Range, "A list value is outside its storage range.", null, frame.Path.Member("default"), Text(node.Default), Min + " to " + Max);
        var items = new List<long>(node.Items.Count);
        for (int i = 0; i < node.Items.Count; i++)
        {
            long item = node.Items[i];
            if (item < Min || item > Max)
                throw context.Fail(Codes.Range, "A list value is outside its storage range.", null, frame.Path.Member("items").Item(i), Text(item), Min + " to " + Max);
            items.Add(item);
        }
        data.Bytes(Canonical(node.Default, items));
    }

    internal override Tree.TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        var items = Parse(context, start, end, frame.Value(context), out long defense);
        return new Tree.DefaultListNode(width, defense, items);
    }

    internal override JsonValue? WriteTree(Tree.TreeValue value, Tree.DocumentSite site)
    {
        if (value is not Tree.DefaultListNode node || node.Width != width)
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is {{ \"default\": ..., \"items\": [...] }}.");
        if (node.Items.Count > site.Context.Limits.CollectionItems)
            throw site.Fail(Codes.Limit, "A collection has too many items.", constraint: $"at most {site.Context.Limits.CollectionItems} items");
        if (node.Default < Min || node.Default > Max)
            throw site.At(site.Path.Member("default"), element: false).Fail(Codes.Range, "A list value is outside its storage range.", value: Text(node.Default), constraint: $"{Min} to {Max}");
        var decoding = Element is null ? null : ListElement.Decoding(site.Context);
        var array = new JsonArray();
        for (int i = 0; i < node.Items.Count; i++)
        {
            long item = node.Items[i];
            if (item < Min || item > Max)
                throw site.At(site.Path.Member("items").Item(i), element: true).Fail(Codes.Range, "A list value is outside its storage range.", value: Text(item), constraint: $"{Min} to {Max}");
            array.Add(Value(item, decoding, site.Path.Member("items").Item(i)));
        }
        return new JsonObject().Add("default", Value(node.Default, decoding, site.Path.Member("default"))).Add("items", array);
    }

    internal override Tree.TreeValue? ReadTree(JsonValue value, Tree.DocumentSite site)
    {
        if (value is not JsonObject objectValue)
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is {{ \"default\": ..., \"items\": [...] }}.", value);
        foreach (var member in objectValue.Members)
            if (member.Name is not ("default" or "items"))
                throw site.At(site.Path.Member(member.Name), element: false).Fail(Codes.UnknownMember, $"Member \"{member.Name}\" is not allowed in a list.", member.Value, member.Name);
        long Item(JsonValue? item, Tree.DocumentSite at)
        {
            if (Element is not null && item is not null) return Element.Read(item, site.Context, at.Path);
            return item is JsonInteger n && n.Value >= Min && n.Value <= Max ? n.Value
                : throw at.Fail(item is JsonInteger ? Codes.Range : Codes.Type, "A list value is outside its storage range.", item ?? objectValue, null, $"{Min} to {Max}");
        }
        long defense = Item(objectValue["default"], site.At(site.Path.Member("default"), element: false));
        var array = objectValue["items"] as JsonArray ?? throw site.Fail(Codes.MissingMember, "items is an array.", objectValue);
        if (array.Items.Count > site.Context.Limits.CollectionItems)
            throw site.Fail(Codes.Limit, "A collection has too many items.", array, null, $"at most {site.Context.Limits.CollectionItems} items");
        var items = new long[array.Items.Count];
        for (int i = 0; i < items.Length; i++) items[i] = Item(array.Items[i], site.At(site.Path.Member("items").Item(i), element: true));
        return new Tree.DefaultListNode(width, defense, items);
    }

    private JsonValue Value(long value, DecodeContext? context, DocumentNodePath path) =>
        Element is null ? new JsonInteger(value) : Element.Write(value, context!, path);

    private long Read(byte[] data, int at) => width == 1 ? data[at] : BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(at, 4));

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        var items = DecodeRuns(context, start, end, site, out long defense);
        var array = new JsonArray();
        for (int i = 0; i < items.Count; i++) array.Add(Value(items[i], context, site.Path.Member("items").Item(i)));
        return new JsonObject().Add("default", Value(defense, context, site.Path.Member("default"))).Add("items", array);
    }

    /// <summary>The default and the items of a stored list. A run split the writer does not make becomes a value-encoding part.</summary>
    internal List<long> DecodeRuns(DecodeContext context, int start, int end, ValueSite site, out long defense)
    {
        var items = Parse(context, start, end, site, out defense);
        var bytes = Canonical(defense, items);
        if (!bytes.AsSpan().SequenceEqual(context.Data.AsSpan(start, end - start)))
            site.Builder?.ValueEncoding(site.Id, context.Data.AsSpan(start, end - start));
        return items;
    }

    private List<long> Parse(DecodeContext context, int start, int end, ValueSite site, out long defense)
    {
        context.Need(start, width + 4, end, site.Path, "list header");
        defense = Read(context.Data, start);
        int count = context.Int32(start + width);
        if (count < 0) throw Mismatch(context, start, site, "A list has a negative count.");
        context.CheckItems(count, start, site.Path);
        var items = new List<long>(count);
        int at = start + width + 4;
        while (items.Count < count)
        {
            context.Need(at, width, end, site.Path, "list item");
            long v = Read(context.Data, at);
            at += width;
            if (v == defense)
            {
                context.Need(at, width, end, site.Path, "list run");
                long run = Read(context.Data, at);
                at += width;
                if (run < 0 || items.Count + run > count) throw Mismatch(context, at, site, "A list run passes the stored count.");
                for (long k = 0; k < run; k++) items.Add(defense);
            }
            else items.Add(v);
        }
        if (at != end) throw Mismatch(context, at, site, "Bytes follow the list items.");
        return items;
    }

    private void Put(ByteBuffer b, long v)
    {
        if (width == 1) b.Byte((byte)v);
        else b.Int32((int)v);
    }

    private byte[] Canonical(long defense, List<long> items)
    {
        var b = new ByteBuffer(items.Count * width + 8);
        Put(b, defense);
        b.Int32(items.Count);
        int i = 0;
        while (i < items.Count)
        {
            if (items[i] == defense)
            {
                int j = i;
                while (j < items.Count && items[j] == defense) j++;
                int run = j - i;
                while (width == 1 && run > 255)
                {
                    Put(b, defense);
                    Put(b, 255);
                    run -= 255;
                }
                Put(b, defense);
                Put(b, run);
                i = j;
            }
            else Put(b, items[i++]);
        }
        return b.ToArray();
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonObject objectValue) throw context.Fail(Codes.Type, $"{site.Property.Name} is {{ \"default\": ..., \"items\": [...] }}.", value, site.Path, value.KindName);
        foreach (var m in objectValue.Members)
            if (m.Name is not ("default" or "items"))
                throw new ConversionException(new FormatDiagnostic(Codes.UnknownMember, $"Member \"{m.Name}\" is not allowed in a list.")
                { File = context.File, Span = m.NameSpan, Path = site.Path.Member(m.Name).ToString() });
        long Item(JsonValue? v, DocumentNodePath p)
        {
            if (Element is not null && v is not null) return Element.Read(v, context, p);
            return v is JsonInteger n && n.Value >= Min && n.Value <= Max ? n.Value
                : throw context.Fail(v is JsonInteger ? Codes.Range : Codes.Type, "A list value is outside its storage range.", v ?? objectValue, p, null, $"{Min} to {Max}");
        }
        long defense = Item(objectValue["default"], site.Path.Member("default"));
        var array = objectValue["items"] as JsonArray ?? throw context.Fail(Codes.MissingMember, "items is an array.", objectValue, site.Path);
        if (array.Items.Count > context.Limits.CollectionItems)
            throw context.Fail(Codes.Limit, "A collection has too many items.", array, site.Path, null, $"at most {context.Limits.CollectionItems} items");
        var items = new List<long>(array.Items.Count);
        for (int i = 0; i < array.Items.Count; i++) items.Add(Item(array.Items[i], site.Path.Member("items").Item(i)));
        EncodeRuns(context, defense, items, data, site);
    }

    /// <summary>Writes the canonical runs, or the retained encoding when it decodes to the same list.</summary>
    internal void EncodeRuns(EncodeContext context, long defense, List<long> items, ByteBuffer data, ValueSite site)
    {
        var bytes = Canonical(defense, items);
        if (site.Reader?.Encodings.TryGetValue(site.Id, out var retained) == true)
        {
            // The retained encoding is used only if it decodes to the visible list.
            if (!SameList(retained.Bytes, defense, items))
                throw context.Fail(Codes.Layout, "A retained list encoding does not decode to the visible list.", retained.At, site.Path);
            bytes = retained.Bytes;
        }
        data.Bytes(bytes);
    }

    private bool SameList(byte[] bytes, long defense, List<long> items)
    {
        try
        {
            var probe = new DecodeContext(bytes, FormatSchema.Current, "game", "", null, FormatLimits.Default, new LayoutCounts());
            var site = new ValueSite { Property = null!, Id = 0, Path = DocumentNodePath.Root, Owner = new JsonObject() };
            var parsed = Parse(probe, 0, bytes.Length, site, out long d);
            return d == defense && parsed.SequenceEqual(items);
        }
        catch (ConversionException)
        {
            return false;
        }
    }
}

/// <summary>
/// A TUnitIndexListCollection stream (Unit.PAS 2413): one TUnitIndexList cut into groups of a
/// fixed size, where -1 is an empty entry. The image is the flat run list. From version 4 the
/// document is <c>{"default": d, "groups": [[...], ...], "count": n}</c>. Each group drops its
/// trailing -1 entries. <c>count</c> is present only when the stored length is not the group
/// size times the number of groups, and <c>default</c> only when it is not -1, the constructor's.
/// The tree has no member for it yet.
/// </summary>
internal sealed class GroupedListCodec(int size, string? domain = null) : PropertyCodec
{
    private const long Empty = -1;

    private readonly RunListCodec _flat = new(4);

    public int Size { get; } = size;

    /// <summary>The reference domain of the values, an annotation only.</summary>
    public string? Domain { get; } = domain;

    internal override string? ImageRefusal => "PE-3 still needs a grouped list. The direct path does not store it.";

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        var items = _flat.DecodeRuns(context, start, end, site, out long defense);
        return Document(defense, items);
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        var (defense, items) = Flat(context, value, site.Path, site.Property?.Name ?? "The value");
        _flat.EncodeRuns(context, defense, items, data, site);
    }

    private JsonObject Document(long defense, IReadOnlyList<long> items)
    {
        int groups = (items.Count + Size - 1) / Size;
        var array = new JsonArray();
        for (int g = 0; g < groups; g++)
        {
            int from = g * Size;
            int last = Math.Min(from + Size, items.Count);
            while (last > from && items[last - 1] == Empty) last--;
            var group = new JsonArray();
            for (int k = from; k < last; k++) group.Add(new JsonInteger(items[k]));
            array.Add(group);
        }
        var document = new JsonObject();
        if (defense != Empty) document.Add("default", new JsonInteger(defense));
        document.Add("groups", array);
        if (items.Count != groups * Size) document.Add("count", new JsonInteger(items.Count));
        return document;
    }

    private (long Default, List<long> Items) Flat(EncodeContext context, JsonValue value, DocumentNodePath path, string name)
    {
        if (value is not JsonObject objectValue)
            throw context.Fail(Codes.Type, name + " is { \"groups\": [[...]] }.", value, path, value.KindName);
        foreach (var m in objectValue.Members)
            if (m.Name is not ("default" or "groups" or "count"))
                throw new ConversionException(new FormatDiagnostic(m.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember, $"Member \"{m.Name}\" is not allowed in a grouped list.")
                { File = context.File, Span = m.NameSpan, Path = path.Member(m.Name).ToString() });
        long Integer(JsonValue item, DocumentNodePath at) => item is JsonInteger n && n.Value >= int.MinValue && n.Value <= int.MaxValue ? n.Value
            : throw context.Fail(item is JsonInteger ? Codes.Range : Codes.Type, "A list value is a signed 32-bit integer.", item, at, null, $"{int.MinValue} to {int.MaxValue}");
        long defense = objectValue["default"] is { } stored ? Integer(stored, path.Member("default")) : Empty;
        var groups = objectValue["groups"] as JsonArray ?? throw context.Fail(Codes.MissingMember, "groups is an array of groups.", objectValue, path);
        long full = (long)groups.Items.Count * Size;
        long count = full;
        if (objectValue["count"] is { } countValue)
        {
            if (countValue is not JsonInteger n) throw context.Fail(Codes.Type, "count is an integer.", countValue, path.Member("count"), countValue.KindName);
            count = n.Value;
            if (count < 0 || count > full || count <= full - Size)
                throw context.Fail(Codes.Range, $"count does not fit {groups.Items.Count} groups of {Size}.", countValue, path.Member("count"), Text(count),
                    groups.Items.Count == 0 ? "0" : $"{full - Size + 1} to {full}");
        }
        if (count > context.Limits.CollectionItems)
            throw context.Fail(Codes.Limit, "A collection has too many items.", groups, path, Text(count), $"at most {context.Limits.CollectionItems} items");
        var items = new List<long>((int)count);
        for (int g = 0; g < groups.Items.Count; g++)
        {
            var at = path.Member("groups").Item(g);
            if (groups.Items[g] is not JsonArray group) throw context.Fail(Codes.Type, "A group is an array of values.", groups.Items[g], at, groups.Items[g].KindName);
            long room = Math.Min(Size, count - (long)g * Size);
            if (group.Items.Count > room)
                throw context.Fail(Codes.Range, room == Size ? $"A group holds at most {Size} values." : $"The last group holds at most {room} values under count {count}.",
                    group, at, Text(group.Items.Count), $"at most {room} values");
            for (int k = 0; k < group.Items.Count; k++) items.Add(Integer(group.Items[k], at.Item(k)));
            for (long k = group.Items.Count; k < room; k++) items.Add(Empty);
        }
        return (defense, items);
    }
}

/// <summary>
/// TBitList streams: a default byte, the stored count and the bit bytes, bit i in byte i >> 3 at
/// mask 1 &lt;&lt; (i and 7). Versions 1 to 3 store the exact bit bytes as hex. From version 4 the
/// document is <c>{"default": false, "count": n, "bits": [set indices]}</c>, the default by the
/// raw boolean rule, and bits past the count in the last byte as <c>$reserved</c>, the rule of the
/// ability bit array.
/// </summary>
internal sealed class BitListCodec(bool indices = false) : PropertyCodec
{
    /// <summary>True from version 4: the set bits are indices, not hex.</summary>
    public bool Indices { get; } = indices;

    internal override string? ImageRefusal => "PE-2c still needs a bit list. The direct path does not store it.";

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        context.Need(start, 5, end, site.Path, "bit list header");
        int count = context.Int32(start + 1);
        if (count < 0 || start + 5L + ((count + 7L) >> 3) != end)
            throw Mismatch(context, start, site, $"A bit list of {count} bits does not fill its {end - start} stored bytes.");
        if (!Indices)
            return new JsonObject().Add("default", new JsonInteger(context.Data[start])).Add("count", new JsonInteger(count))
                .Add("bits", new JsonString(LayoutResidue.Hex(context.Data.AsSpan(start + 5, end - start - 5))));
        var (set, residue) = BitArrayCodec.Unpack(count, context.Data.AsSpan(start + 5, end - start - 5), context, site.Path.Member("bits"));
        var bits = new JsonArray();
        foreach (int index in set) bits.Add(new JsonInteger(index));
        var document = new JsonObject().Add("default", RawBooleanCodec.Value(context.Data[start])).Add("count", new JsonInteger(count)).Add("bits", bits);
        if (residue != 0) document.Add("$reserved", new JsonString(LayoutResidue.Hex([residue])));
        return document;
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonObject objectValue) throw context.Fail(Codes.Type, $"{site.Property.Name} is {{ \"default\", \"count\", \"bits\" }}.", value, site.Path, value.KindName);
        foreach (var m in objectValue.Members)
            if (m.Name is not ("default" or "count" or "bits") && !(Indices && m.Name == "$reserved"))
                throw new ConversionException(new FormatDiagnostic(m.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember, $"Member \"{m.Name}\" is not allowed in a bit list.")
                { File = context.File, Span = m.NameSpan, Path = site.Path.Member(m.Name).ToString() });
        byte defense;
        if (Indices)
            defense = RawBooleanCodec.ByteOf(context, objectValue["default"] ?? objectValue, site.Path.Member("default"), "default");
        else if (objectValue["default"] is JsonInteger { Value: >= 0 and <= 255 } stored)
            defense = (byte)stored.Value;
        else
            throw context.Fail(Codes.Range, "default is a byte.", objectValue["default"] ?? objectValue, site.Path.Member("default"));
        if (objectValue["count"] is not JsonInteger { Value: >= 0 and <= int.MaxValue } count)
            throw context.Fail(Codes.Range, "count is a non-negative 32-bit integer.", objectValue["count"] ?? objectValue, site.Path.Member("count"));
        byte[] bits;
        if (Indices)
        {
            var array = objectValue["bits"] as JsonArray
                ?? throw context.Fail(Codes.Type, "bits is an array of set bit indices.", objectValue["bits"] ?? objectValue, site.Path.Member("bits"), (objectValue["bits"] ?? objectValue).KindName);
            int length = (int)((count.Value + 7) >> 3);
            bits = new byte[length];
            foreach (int index in BitArrayCodec.ReadIndices(array, (int)count.Value, context, site.Path.Member("bits"), site.Property?.Name ?? "The value"))
                bits[index >> 3] |= (byte)(1 << (index & 7));
            if (objectValue["$reserved"] is { } reserved)
                bits[length - 1] |= BitArrayCodec.ReadResidue(reserved, (int)count.Value, context, site.Path.Member("$reserved"));
        }
        else
        {
            bits = DataCodec.HexValue(context, objectValue["bits"] ?? new JsonNull(), site);
            if (bits.Length != (count.Value + 7) >> 3)
                throw context.Fail(Codes.Range, $"A bit list of {count.Value} bits holds {(count.Value + 7) >> 3} bytes.", objectValue["bits"], site.Path.Member("bits"));
        }
        data.Byte(defense);
        data.Int32((int)count.Value);
        data.Bytes(bits);
    }
}
