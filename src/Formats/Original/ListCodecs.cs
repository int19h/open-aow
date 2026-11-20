using System.Buffers.Binary;
using Aow.Formats.Json;

namespace Aow.Formats.Original;

/// <summary>
/// TIntegerList and TByteList streams: the default value, the 32-bit count, then the values
/// where a default value is followed by a run length. The document holds the default and the
/// decoded items. Export writes the canonical runs (Engine.pas 3499).
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
        {
            throw context.Fail(Codes.Type, message: $"{name} is a list with a default.", at: null, frame.Path);
        }

        if (node.Items.Count > context.Limits.CollectionItems)
        {
            throw context.Fail(Codes.Limit, message: "A collection has too many items.", at: null, frame.Path, value: null, constraint: $"at most {context.Limits.CollectionItems} items");
        }

        if (node.Default < Min || node.Default > Max)
        {
            throw context.Fail(Codes.Range, message: "A list value is outside its storage range.", at: null, path: frame.Path.Member("default"), value: Text(node.Default), constraint: $"{Min} to {Max}");
        }

        List<long> items = [with(node.Items.Count)];
        for (int i = 0; i < node.Items.Count; i++)
        {
            long item = node.Items[i];
            if (item < Min || item > Max)
            {
                throw context.Fail(Codes.Range, message: "A list value is outside its storage range.", at: null, path: frame.Path.Member("items").Item(i), value: Text(item), constraint: $"{Min} to {Max}");
            }

            items.Add(item);
        }
        data.Bytes(Canonical(node.Default, items));
    }

    internal override Tree.TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        (List<long> items, long defense) = Parse(context, start, end, site: frame.Value(context));
        return new Tree.DefaultListNode(width, defaultValue: defense, items);
    }

    internal override JsonValue? WriteTree(Tree.TreeValue value, Tree.DocumentSite site)
    {
        if (value is not Tree.DefaultListNode node || node.Width != width)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is {{ \"default\": ..., \"items\": [...] }}.");
        }

        if (node.Items.Count > site.Context.Limits.CollectionItems)
        {
            throw site.Fail(Codes.Limit, message: "A collection has too many items.", constraint: $"at most {site.Context.Limits.CollectionItems} items");
        }

        if (node.Default < Min || node.Default > Max)
        {
            throw site.At(site.Path.Member("default"), element: false).Fail(Codes.Range, message: "A list value is outside its storage range.", value: Text(node.Default), constraint: $"{Min} to {Max}");
        }

        DecodeContext? decoding = Element is null ? null : ListElement.Decoding(site.Context);
        JsonArray array = new();
        for (int i = 0; i < node.Items.Count; i++)
        {
            long item = node.Items[i];
            if (item < Min || item > Max)
            {
                throw site.At(site.Path.Member("items").Item(i), element: true).Fail(Codes.Range, message: "A list value is outside its storage range.", value: Text(item), constraint: $"{Min} to {Max}");
            }

            _ = array.Add(Value(item, context: decoding, path: site.Path.Member("items").Item(i)));
        }
        return new JsonObject().Add("default", Value(node.Default, context: decoding, path: site.Path.Member("default"))).Add("items", array);
    }

    internal override Tree.TreeValue? ReadTree(JsonValue value, Tree.DocumentSite site)
    {
        if (value is not JsonObject objectValue)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is {{ \"default\": ..., \"items\": [...] }}.", at: value);
        }

        foreach (JsonMember member in objectValue.Members)
        {
            if (member.Name is not ("default" or "items"))
            {
                throw site.At(site.Path.Member(member.Name), element: false).Fail(Codes.UnknownMember, message: $"Member \"{member.Name}\" is not allowed in a list.", at: member.Value, value: member.Name);
            }
        }

        long Item(JsonValue? item, Tree.DocumentSite at)
        {
            return Element is not null && item is not null
                ? Element.Read(item, site.Context, at.Path)
                : item is JsonInteger n && n.Value >= Min && n.Value <= Max ? n.Value
                : throw at.Fail(item is JsonInteger ? Codes.Range : Codes.Type, message: "A list value is outside its storage range.", at: item ?? objectValue, value: null, constraint: $"{Min} to {Max}");
        }
        long defense = Item(objectValue["default"], at: site.At(site.Path.Member("default"), element: false));
        JsonArray array = objectValue["items"] as JsonArray ?? throw site.Fail(Codes.MissingMember, message: "items is an array.", at: objectValue);
        if (array.Items.Count > site.Context.Limits.CollectionItems)
        {
            throw site.Fail(Codes.Limit, message: "A collection has too many items.", at: array, value: null, constraint: $"at most {site.Context.Limits.CollectionItems} items");
        }

        long[] items = new long[array.Items.Count];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = Item(array.Items[i], at: site.At(site.Path.Member("items").Item(i), element: true));
        }

        return new Tree.DefaultListNode(width, defaultValue: defense, items);
    }

    private JsonValue Value(long value, DecodeContext? context, DocumentNodePath path)
        => Element is null ? new JsonInteger(value) : Element.Write(value, context!, path);

    private long Read(byte[] data, int at) => width == 1 ? data[at] : BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(at, length: 4));

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        (List<long> items, long defense) = DecodeRuns(context, start, end, site);
        JsonArray array = new();
        for (int i = 0; i < items.Count; i++)
        {
            _ = array.Add(Value(items[i], context, path: site.Path.Member("items").Item(i)));
        }

        return new JsonObject().Add("default", Value(defense, context, path: site.Path.Member("default"))).Add("items", array);
    }

    /// <summary>The default and the items of a stored list.</summary>
    internal (List<long>, long Defense) DecodeRuns(DecodeContext context, int start, int end, ValueSite site)
        => Parse(context, start, end, site);

    internal (List<long> Items, long Defense) Parse(DecodeContext context, int start, int end, ValueSite site)
    {
        context.Need(start, count: width + 4, end, site.Path, what: "list header");
        long defense = Read(context.Data, at: start);
        int count = context.Int32(start + width);
        if (count < 0)
        {
            throw Mismatch(context, start, site, message: "A list has a negative count.");
        }

        context.CheckItems(count, offset: start, site.Path);
        List<long> items = [with(count)];
        int at = start + width + 4;
        while (items.Count < count)
        {
            context.Need(at, count: width, end, site.Path, what: "list item");
            long v = Read(context.Data, at);
            at += width;
            if (v == defense)
            {
                context.Need(at, count: width, end, site.Path, what: "list run");
                long run = Read(context.Data, at);
                at += width;
                if (run < 0 || items.Count + run > count)
                {
                    throw Mismatch(context, start: at, site, message: "A list run passes the stored count.");
                }

                for (long k = 0; k < run; k++)
                {
                    items.Add(defense);
                }
            }
            else
            {
                items.Add(v);
            }
        }
        return (at != end ? throw Mismatch(context, start: at, site, message: "Bytes follow the list items.") : items, defense);
    }

    private void Put(ByteBuffer b, long v)
    {
        if (width == 1)
        {
            b.Byte((byte)v);
        }
        else
        {
            b.Int32((int)v);
        }
    }

    private byte[] Canonical(long defense, List<long> items)
    {
        ByteBuffer b = new((items.Count * width) + 8);
        Put(b, v: defense);
        b.Int32(items.Count);
        int i = 0;
        while (i < items.Count)
        {
            if (items[i] == defense)
            {
                int j = i;
                while (j < items.Count && items[j] == defense)
                {
                    j++;
                }

                int run = j - i;
                while (width == 1 && run > 255)
                {
                    Put(b, v: defense);
                    Put(b, v: 255);
                    run -= 255;
                }
                Put(b, v: defense);
                Put(b, v: run);
                i = j;
            }
            else
            {
                Put(b, v: items[i++]);
            }
        }
        return b.ToArray();
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonObject objectValue)
        {
            throw context.Fail(Codes.Type, message: $"{site.Property.Name} is {{ \"default\": ..., \"items\": [...] }}.", at: value, site.Path, value: value.KindName);
        }

        foreach (JsonMember m in objectValue.Members)
        {
            if (m.Name is not ("default" or "items"))
            {
                throw new ConversionException(new FormatDiagnostic(Codes.UnknownMember, Message: $"Member \"{m.Name}\" is not allowed in a list.")
                { File = context.File, Span = m.NameSpan, Path = site.Path.Member(m.Name).ToString() });
            }
        }

        long Item(JsonValue? v, DocumentNodePath p)
        {
            return Element is not null && v is not null
                ? Element.Read(v, context, path: p)
                : v is JsonInteger n && n.Value >= Min && n.Value <= Max ? n.Value
                : throw context.Fail(v is JsonInteger ? Codes.Range : Codes.Type, message: "A list value is outside its storage range.", at: v ?? objectValue, path: p, value: null, constraint: $"{Min} to {Max}");
        }
        long defense = Item(objectValue["default"], p: site.Path.Member("default"));
        JsonArray array = objectValue["items"] as JsonArray ?? throw context.Fail(Codes.MissingMember, message: "items is an array.", at: objectValue, site.Path);
        if (array.Items.Count > context.Limits.CollectionItems)
        {
            throw context.Fail(Codes.Limit, message: "A collection has too many items.", at: array, site.Path, value: null, constraint: $"at most {context.Limits.CollectionItems} items");
        }

        List<long> items = [with(array.Items.Count)];
        for (int i = 0; i < array.Items.Count; i++)
        {
            items.Add(Item(array.Items[i], p: site.Path.Member("items").Item(i)));
        }

        EncodeRuns(defense, items, data);
    }

    /// <summary>Writes the canonical runs.</summary>
    internal void EncodeRuns(long defense, List<long> items, ByteBuffer data)
        => data.Bytes(Canonical(defense, items));
}

/// <summary>
/// A TUnitIndexListCollection stream (Unit.PAS 2413): one TUnitIndexList cut into groups of a
/// fixed size, where -1 is an empty entry. The image is the flat run list. From version 4 the
/// document is <c>{"default": d, "groups": [[...], ...], "count": n}</c>. Each group drops its
/// trailing -1 entries. <c>count</c> is present only when the stored length is not the group
/// size times the number of groups, and <c>default</c> only when it is not -1, the constructor's.
/// Since PE-2c-5 the tree holds the flat list as a <see cref="Tree.DefaultListNode"/> of width 4,
/// and the direct path writes its canonical runs.
/// </summary>
internal sealed class GroupedListCodec(int size, string? domain = null) : PropertyCodec
{
    private const long Empty = -1;

    private readonly RunListCodec _flat = new(4);

    public int Size { get; } = size;

    /// <summary>The reference domain of the values, an annotation only.</summary>
    public string? Domain { get; } = domain;

    internal override void WriteImage(Tree.TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
        => _flat.WriteImage(value, context, data, frame);

    internal override Tree.TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
        => _flat.ReadImage(context, start, end, frame);

    internal override JsonValue? WriteTree(Tree.TreeValue value, Tree.DocumentSite site)
    {
        string name = site.Property?.Name ?? "The value";
        if (value is not Tree.DefaultListNode { Width: 4 } node)
        {
            throw site.Mismatch($"{name} is a list with a default.");
        }

        if (node.Items.Count > site.Context.Limits.CollectionItems)
        {
            throw site.Fail(Codes.Limit, message: "A collection has too many items.", constraint: $"at most {site.Context.Limits.CollectionItems} items");
        }

        if (node.Default is < int.MinValue or > int.MaxValue)
        {
            throw site.At(site.Path.Member("default"), element: false).Fail(Codes.Range, message: "A list value is a signed 32-bit integer.", value: Text(node.Default));
        }

        for (int i = 0; i < node.Items.Count; i++)
        {
            if (node.Items[i] is < int.MinValue or > int.MaxValue)
            {
                throw site.Fail(Codes.Range, message: "A list value is a signed 32-bit integer.", value: Text(node.Items[i]));
            }
        }

        return Document(node.Default, node.Items);
    }

    internal override Tree.TreeValue? ReadTree(JsonValue value, Tree.DocumentSite site)
    {
        (long defense, List<long>? items) = Flat(site.Context, value, site.Path, name: site.Property?.Name ?? "The value");
        return new Tree.DefaultListNode(4, defaultValue: defense, items);
    }

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        (List<long> items, long defense) = _flat.DecodeRuns(context, start, end, site);
        return Document(defense, items);
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        (long defense, List<long>? items) = Flat(context, value, site.Path, name: site.Property?.Name ?? "The value");
        _flat.EncodeRuns(defense, items, data);
    }

    private JsonObject Document(long defense, IReadOnlyList<long> items)
    {
        int groups = (items.Count + Size - 1) / Size;
        JsonArray array = new();
        for (int g = 0; g < groups; g++)
        {
            int from = g * Size;
            int last = Math.Min(from + Size, items.Count);
            while (last > from && items[last - 1] == Empty)
            {
                last--;
            }

            JsonArray group = new();
            for (int k = from; k < last; k++)
            {
                _ = group.Add(new JsonInteger(items[k]));
            }

            _ = array.Add(group);
        }
        JsonObject document = new();
        if (defense != Empty)
        {
            _ = document.Add("default", new JsonInteger(defense));
        }

        _ = document.Add("groups", array);
        if (items.Count != groups * Size)
        {
            _ = document.Add("count", new JsonInteger(items.Count));
        }

        return document;
    }

    private (long Default, List<long> Items) Flat(EncodeContext context, JsonValue value, DocumentNodePath path, string name)
    {
        if (value is not JsonObject objectValue)
        {
            throw context.Fail(Codes.Type, message: $"{name} is {{ \"groups\": [[...]] }}.", at: value, path, value: value.KindName);
        }

        foreach (JsonMember m in objectValue.Members)
        {
            if (m.Name is not ("default" or "groups" or "count"))
            {
                throw new ConversionException(new FormatDiagnostic(m.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember, Message: $"Member \"{m.Name}\" is not allowed in a grouped list.")
                { File = context.File, Span = m.NameSpan, Path = path.Member(m.Name).ToString() });
            }
        }

        long Integer(JsonValue item, DocumentNodePath at) => item is JsonInteger n && n.Value is >= int.MinValue and <= int.MaxValue ? n.Value
            : throw context.Fail(item is JsonInteger ? Codes.Range : Codes.Type, message: "A list value is a signed 32-bit integer.", at: item, path: at, value: null, constraint: $"{int.MinValue} to {int.MaxValue}");
        long defense = objectValue["default"] is { } stored ? Integer(stored, at: path.Member("default")) : Empty;
        JsonArray groups = objectValue["groups"] as JsonArray ?? throw context.Fail(Codes.MissingMember, message: "groups is an array of groups.", at: objectValue, path);
        long full = (long)groups.Items.Count * Size;
        long count = full;
        if (objectValue["count"] is { } countValue)
        {
            if (countValue is not JsonInteger n)
            {
                throw context.Fail(Codes.Type, message: "count is an integer.", at: countValue, path: path.Member("count"), value: countValue.KindName);
            }

            count = n.Value;
            if (count < 0 || count > full || count <= full - Size)
            {
                throw context.Fail(Codes.Range, message: $"count does not fit {groups.Items.Count} groups of {Size}.", at: countValue, path: path.Member("count"), value: Text(count),
                    constraint: groups.Items.Count == 0 ? "0" : $"{full - Size + 1} to {full}");
            }
        }
        if (count > context.Limits.CollectionItems)
        {
            throw context.Fail(Codes.Limit, message: "A collection has too many items.", at: groups, path, value: Text(count), constraint: $"at most {context.Limits.CollectionItems} items");
        }

        List<long> items = [with((int)count)];
        for (int g = 0; g < groups.Items.Count; g++)
        {
            DocumentNodePath at = path.Member("groups").Item(g);
            if (groups.Items[g] is not JsonArray group)
            {
                throw context.Fail(Codes.Type, message: "A group is an array of values.", at: groups.Items[g], path: at, value: groups.Items[g].KindName);
            }

            long room = Math.Min(Size, count - ((long)g * Size));
            if (group.Items.Count > room)
            {
                throw context.Fail(Codes.Range, message: room == Size ? $"A group holds at most {Size} values." : $"The last group holds at most {room} values under count {count}.",
                    at: group, path: at, value: Text(group.Items.Count), constraint: $"at most {room} values");
            }

            for (int k = 0; k < group.Items.Count; k++)
            {
                items.Add(Integer(group.Items[k], at: at.Item(k)));
            }

            for (long k = group.Items.Count; k < room; k++)
            {
                items.Add(Empty);
            }
        }
        return (defense, items);
    }
}

/// <summary>
/// TBitList streams: a default byte, the stored count and the bit bytes, bit i in byte i >> 3 at
/// mask 1 &lt;&lt; (i and 7). Versions 1 to 3 store the exact bit bytes as hex. From version 4 the
/// document is <c>{"default": false, "count": n, "bits": [set indices]}</c>, the default by the
/// raw boolean rule. Bits past the count in the last byte equal the default bit: load refuses any
/// other tail bit and save writes the default bit.
/// </summary>
internal sealed class BitListCodec(bool indices = false) : PropertyCodec
{
    /// <summary>True from version 4: the set bits are indices, not hex.</summary>
    public bool Indices { get; } = indices;

    /// <summary>The tail bits past <paramref name="count"/> in the last byte, filled with the default bit.</summary>
    internal static byte ExpectedTail(byte defaultValue, int count)
    {
        int used = count & 7;
        return used == 0 || defaultValue == 0 ? (byte)0 : (byte)(0xFF << used);
    }

    // The tree and the direct path store the default byte, the count and the set bits (PE-2c-4).
    internal override void WriteImage(Tree.TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not Tree.BitListNode node)
        {
            throw context.Fail(Codes.Type, message: $"{frame.Property.Name} is a bit list.", at: null, frame.Path);
        }

        data.Byte(node.Default);
        data.Int32(node.Count);
        data.Bytes(Bits(node));
    }

    internal override Tree.TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        context.Need(start, count: 5, end, frame.Path, what: "bit list header");
        int count = context.Int32(start + 1);
        if (count < 0 || start + 5L + ((count + 7L) >> 3) != end)
        {
            throw context.Fail(Codes.Corrupt, message: $"A bit list of {count} bits does not fill its {end - start} stored bytes.", offset: start, frame.Path);
        }

        byte defaultValue = context.Data[start];
        int[] set = Unpack(defaultValue, count, bytes: context.Data.AsSpan(start + 5, length: end - start - 5), context, tailOffset: end - 1, path: frame.Path.Member("bits"));
        return new Tree.BitListNode(defaultValue, count, indices: set);
    }

    internal override JsonValue? WriteTree(Tree.TreeValue value, Tree.DocumentSite site)
    {
        if (value is not Tree.BitListNode node)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a bit list.");
        }

        if (!Indices)
        {
            return new JsonObject().Add("default", new JsonInteger(node.Default)).Add("count", new JsonInteger(node.Count))
                .Add("bits", new JsonString(HexText.Encode(Bits(node))));
        }

        JsonArray bits = new();
        foreach (int index in node.Indices)
        {
            _ = bits.Add(new JsonInteger(index));
        }

        return new JsonObject().Add("default", RawBooleanCodec.Value(node.Default)).Add("count", new JsonInteger(node.Count)).Add("bits", bits);
    }

    internal override Tree.TreeValue? ReadTree(JsonValue value, Tree.DocumentSite site)
    {
        // The converter's rules check the document. The bytes they write are the node.
        ByteBuffer data = new();
        Encode(site.Context, value, data, site: new ValueSite { Property = site.Property!, Id = site.Property!.Id ?? 0, Path = site.Path, Owner = site.Owner ?? new JsonObject() });
        byte[] bytes = data.ToArray();
        int count = BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(1));
        List<int> set = [];
        for (int i = 0; i < count; i++)
        {
            if ((bytes[5 + (i >> 3)] & (1 << (i & 7))) != 0)
            {
                set.Add(i);
            }
        }

        return new Tree.BitListNode(defaultValue: bytes[0], count: count, indices: [.. set]);
    }

    private static byte[] Bits(Tree.BitListNode node)
    {
        byte[] bits = new byte[(node.Count + 7) >> 3];
        foreach (int index in node.Indices)
        {
            bits[index >> 3] |= (byte)(1 << (index & 7));
        }

        if ((node.Count & 7) != 0 && bits.Length != 0)
        {
            bits[^1] |= ExpectedTail(node.Default, node.Count);
        }

        return bits;
    }

    private static int[] Unpack(byte defaultValue, int count, ReadOnlySpan<byte> bytes, DecodeContext context, int tailOffset, DocumentNodePath path)
    {
        int used = count & 7;
        if (used != 0 && bytes.Length != 0)
        {
            byte tail = (byte)(bytes[^1] & (byte)(0xFF << used));
            byte expected = ExpectedTail(defaultValue, count);
            if (tail != expected)
            {
                throw context.Fail(Codes.Range, message: "Bits past the bit list count do not match the default bit.", offset: tailOffset, path, value: HexText.Encode([tail]), constraint: HexText.Encode([expected]));
            }
        }

        List<int> indices = [];
        for (int i = 0; i < count; i++)
        {
            if ((bytes[i >> 3] & (1 << (i & 7))) != 0)
            {
                indices.Add(i);
            }
        }

        return [.. indices];
    }

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        context.Need(start, count: 5, end, site.Path, what: "bit list header");
        int count = context.Int32(start + 1);
        if (count < 0 || start + 5L + ((count + 7L) >> 3) != end)
        {
            throw Mismatch(context, start, site, message: $"A bit list of {count} bits does not fill its {end - start} stored bytes.");
        }

        byte defaultValue = context.Data[start];
        ReadOnlySpan<byte> bitBytes = context.Data.AsSpan(start + 5, length: end - start - 5);
        int[] set = Unpack(defaultValue, count, bytes: bitBytes, context, tailOffset: end - 1, path: site.Path.Member("bits"));
        if (!Indices)
        {
            return new JsonObject().Add("default", new JsonInteger(defaultValue)).Add("count", new JsonInteger(count))
                .Add("bits", new JsonString(HexText.Encode(bitBytes)));
        }

        JsonArray bits = new();
        foreach (int index in set)
        {
            _ = bits.Add(new JsonInteger(index));
        }

        return new JsonObject().Add("default", RawBooleanCodec.Value(defaultValue)).Add("count", new JsonInteger(count)).Add("bits", bits);
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonObject objectValue)
        {
            throw context.Fail(Codes.Type, message: $"{site.Property.Name} is {{ \"default\", \"count\", \"bits\" }}.", at: value, site.Path, value: value.KindName);
        }

        foreach (JsonMember m in objectValue.Members)
        {
            if (m.Name is not ("default" or "count" or "bits"))
            {
                throw new ConversionException(new FormatDiagnostic(m.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember, Message: $"Member \"{m.Name}\" is not allowed in a bit list.")
                { File = context.File, Span = m.NameSpan, Path = site.Path.Member(m.Name).ToString() });
            }
        }

        byte defense = Indices
            ? RawBooleanCodec.ByteOf(context, value: objectValue["default"] ?? objectValue, path: site.Path.Member("default"), name: "default")
            : objectValue["default"] is JsonInteger { Value: >= 0 and <= 255 } stored
                ? (byte)stored.Value
                : throw context.Fail(Codes.Range, message: "default is a byte.", at: objectValue["default"] ?? objectValue, path: site.Path.Member("default"));
        if (objectValue["count"] is not JsonInteger { Value: >= 0 and <= int.MaxValue } count)
        {
            throw context.Fail(Codes.Range, message: "count is a non-negative 32-bit integer.", at: objectValue["count"] ?? objectValue, path: site.Path.Member("count"));
        }

        byte[] bits;
        if (Indices)
        {
            JsonArray array = objectValue["bits"] as JsonArray
                ?? throw context.Fail(Codes.Type, message: "bits is an array of set bit indices.", at: objectValue["bits"] ?? objectValue, path: site.Path.Member("bits"), value: (objectValue["bits"] ?? objectValue).KindName);
            int length = (int)((count.Value + 7) >> 3);
            bits = new byte[length];
            foreach (int index in BitArrayCodec.ReadIndices(array, limit: (int)count.Value, context, path: site.Path.Member("bits"), name: site.Property?.Name ?? "The value"))
            {
                bits[index >> 3] |= (byte)(1 << (index & 7));
            }

            if ((count.Value & 7) != 0 && bits.Length != 0)
            {
                bits[^1] |= ExpectedTail(defense, count: (int)count.Value);
            }
        }
        else
        {
            bits = DataCodec.HexValue(context, value: objectValue["bits"] ?? new JsonNull(), site);
            if (bits.Length != (count.Value + 7) >> 3)
            {
                throw context.Fail(Codes.Range, message: $"A bit list of {count.Value} bits holds {(count.Value + 7) >> 3} bytes.", at: objectValue["bits"], path: site.Path.Member("bits"));
            }

            int used = (int)(count.Value & 7);
            if (used != 0 && bits.Length != 0)
            {
                byte tail = (byte)(bits[^1] & (byte)(0xFF << used));
                byte expected = ExpectedTail(defense, count: (int)count.Value);
                if (tail != expected)
                {
                    throw context.Fail(Codes.Range, message: "Bits past the bit list count do not match the default bit.", at: objectValue["bits"], path: site.Path.Member("bits"), value: HexText.Encode([tail]), constraint: HexText.Encode([expected]));
                }
            }
        }
        data.Byte(defense);
        data.Int32((int)count.Value);
        data.Bytes(bits);
    }
}
