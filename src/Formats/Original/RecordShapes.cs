using Aow.Formats.Json;

using Aow.Formats.Tree;

namespace Aow.Formats.Original;

/// <summary>
/// A record named by the C# struct in the record catalog. A one-field set wrapper is that set.
/// Other records are objects of named fields. Declared padding bytes are ignored on read,
/// written as 0 and never stored in a document.
/// </summary>
internal sealed partial class CatalogRecordCodec(string typeName, int size) : PropertyCodec
{
    public string TypeName { get; } = typeName;

    /// <summary>The version 2 to 3 step builds its records from this layout and does not consult the live catalog.</summary>
    internal CatalogRecordCodec(RecordTypeInfo pinned) : this(pinned.Name, pinned.Size) => _known = pinned;

    private RecordTypeInfo? _known;
    private readonly Dictionary<string, PropertyCodec> _codecs = [];

    internal override bool AcceptsDocumentArray(int version) => version >= 3 && Info(null, path: "").BareSet;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        RecordTypeInfo info = Info(context.File, path: site.Path.ToString());
        return end - start != info.Size
            ? throw Mismatch(context, start, site, message: $"A record of {info.Size} bytes is stored in {end - start} bytes.")
            : info.BareSet
            ? FieldCodec(info.Fields[0]).Decode(context, start, end, site)
            : Document(info, bytes: context.Data.AsSpan(start, length: info.Size), context, site);
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        RecordTypeInfo info = Info(context.File, path: site.Path.ToString());
        if (info.BareSet)
        {
            FieldCodec(info.Fields[0]).Encode(context, value, data, site);
            return;
        }
        data.Bytes(Pack(info, value, context, parent: site));
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        RefuseOld(site, value: null);
        RecordTypeInfo info = Info(site.Context.File, path: site.Path.ToString());
        if (info.BareSet)
        {
            return FieldCodec(info.Fields[0]).WriteTree(value, site);
        }

        if (value is not RecordNode node)
        {
            throw site.Mismatch($"{site.Property?.Name ?? TypeName} is a record.");
        }

        JsonObject json = new();
        HashSet<string> seen = [];
        foreach (RecordFieldInfo field in info.Fields)
        {
            TreeValue? found = node.Find(field.Name);
            if (found is null)
            {
                continue;
            }

            DocumentSite at = site.At(site.Path.Member(field.Name), element: false);
            JsonValue? written = FieldCodec(field).WriteTree(found, site: at);
            if (written is not null && !field.OmitsDocument(written))
            {
                _ = json.Add(field.Name, written);
            }

            _ = seen.Add(field.Name);
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
        RefuseOld(site, value);
        RecordTypeInfo info = Info(site.Context.File, path: site.Path.ToString());
        if (info.BareSet)
        {
            return FieldCodec(info.Fields[0]).ReadTree(value, site);
        }

        if (value is not JsonObject objectValue)
        {
            throw site.Mismatch($"{site.Property?.Name ?? TypeName} is a record.", at: value);
        }

        CheckNames(objectValue, info, site);
        RecordNode node = new();
        foreach (RecordFieldInfo field in info.Fields)
        {
            JsonMember? member = objectValue.Find(field.Name);
            if (member is null)
            {
                continue;
            }

            DocumentSite at = site.At(site.Path.Member(field.Name), element: false);
            TreeValue? read = FieldCodec(field).ReadTree(member.Value, site: at);
            if (read is not null && !field.OmitsImage(read))
            {
                node.Add(field.Name, read);
            }
        }
        return node;
    }

    private JsonObject Document(RecordTypeInfo info, ReadOnlySpan<byte> bytes, DecodeContext context, ValueSite site)
    {
        JsonObject json = new();
        foreach (RecordFieldInfo field in info.Fields)
        {
            ValueSite fieldSite = new() { Property = site.Property, Id = site.Id, Path = site.Path.Member(field.Name), Owner = site.Owner };
            JsonValue? value = FieldValue(field, bytes: bytes.Slice(field.Offset, length: field.Size), context, site: fieldSite);
            if (value is not null)
            {
                _ = json.Add(field.Name, value);
            }
        }

        return json;
    }

    private JsonValue? FieldValue(RecordFieldInfo field, ReadOnlySpan<byte> bytes, DecodeContext context, ValueSite site)
    {
        PropertyCodec codec = FieldCodec(field);
        byte[] copy = bytes.ToArray();
        JsonValue value = codec.Decode(new DecodeContext(copy, context.Schema, context.Engine, context.Profile, context.File, context.Limits, context.LegacyRules) { Nested = site.Path.ToString() }, start: 0, end: copy.Length, site: site);
        return field.OmitsDocument(value) ? null : value;
    }

    private byte[] Pack(RecordTypeInfo info, JsonValue value, EncodeContext context, ValueSite parent)
    {
        string name = parent.Property?.Name ?? TypeName;
        if (value is not JsonObject objectValue)
        {
            throw context.Fail(Codes.Type, message: $"{name} is a record.", at: value, parent.Path, value: value.KindName);
        }

        CheckNames(objectValue, info, context, parent.Path);
        byte[] bytes = new byte[info.Size];
        foreach (RecordFieldInfo field in info.Fields)
        {
            JsonMember? member = objectValue.Find(field.Name);
            if (member is null)
            {
                continue;
            }

            DocumentNodePath fieldPath = parent.Path.Member(field.Name);
            ValueSite site = new() { Property = parent.Property!, Id = 0, Path = fieldPath, Owner = objectValue };
            WriteField(field, member.Value, context, site, target: bytes.AsSpan(field.Offset, length: field.Size));
        }
        return bytes;
    }

    private void WriteField(RecordFieldInfo field, JsonValue value, EncodeContext context, ValueSite site, Span<byte> target)
    {
        ByteBuffer buffer = new(field.Size);
        FieldCodec(field).Encode(context, value, data: buffer, site);
        if (buffer.Length != field.Size)
        {
            throw context.Fail(Codes.Range, message: $"{field.Name} holds exactly {field.Size} bytes.", at: value, site.Path, value: Text(buffer.Length), constraint: $"{field.Size} bytes");
        }

        buffer.Span.CopyTo(target);
    }

    private PropertyCodec FieldCodec(RecordFieldInfo field)
    {
        if (_codecs.TryGetValue(field.Name, out PropertyCodec? codec))
        {
            return codec;
        }

        codec = field.CreateCodec();
        _codecs.Add(field.Name, codec);
        return codec;
    }

    private RecordTypeInfo Info(string? file, string path)
    {
        if (_known is not null)
        {
            return _known;
        }

        RecordTypeInfo info = RecordCatalog.Require(TypeName, file, path);
        if (info.Size != size)
        {
            throw new InvalidOperationException($"Record {TypeName} is {info.Size} bytes in the catalog and {size} bytes in the schema.");
        }

        _known = info;
        return info;
    }

    private static void RefuseOld(DocumentSite site, JsonValue? value)
    {
        if (site.Schema.Version >= 3)
        {
            return;
        }

        throw site.Fail(Codes.Type, message: $"Version {site.Schema.Version} does not store a record.", at: value);
    }

    private static void CheckNames(JsonObject objectValue, RecordTypeInfo info, DocumentSite site)
    {
        HashSet<string> known = [.. info.Fields.Select(field => field.Name)];
        foreach (JsonMember member in objectValue.Members)
        {
            if (member.Name.StartsWith('$'))
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
    }

    private static void CheckNames(JsonObject objectValue, RecordTypeInfo info, EncodeContext context, DocumentNodePath path)
    {
        HashSet<string> known = [.. info.Fields.Select(field => field.Name)];
        foreach (JsonMember member in objectValue.Members)
        {
            if (member.Name.StartsWith('$'))
            {
                throw context.Fail(Codes.ReservedMember,
                    message: Reserved.IsKnown(member.Name) ? $"Reserved member \"{member.Name}\" is not allowed here." : $"Unknown reserved member \"{member.Name}\".",
                    at: member.Value, path: path.Member(member.Name), value: member.Name);
            }

            if (!known.Contains(member.Name))
            {
                throw context.Fail(Codes.UnknownMember, message: $"The record has no member \"{member.Name}\".", at: member.Value, path: path.Member(member.Name), value: member.Name);
            }
        }
    }
}

/// <summary>
/// A counted bit array stored by rwData. The document is the ascending indices of every set bit
/// in the stored bytes (bits 0 to ((count + 7) &amp; ~7) - 1, including bits past the count that
/// 1.36 keeps across shrinks, compares and revives on growth). The count is the sibling integer
/// named by the schema.
/// </summary>
internal sealed class BitArrayCodec(string countName) : PropertyCodec
{
    public string CountName { get; } = countName;

    internal override bool AcceptsDocumentArray(int version) => version >= 3;

    internal override bool StoresImage(TreeValue value) => value is not BitArrayNode node || ((node.Count + 7) >> 3) != 0;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        int[] indices = Unpack(CountOf(site.Owner, parent: null, name: CountName, context, site.Path), bytes: context.Data.AsSpan(start, length: end - start), context: context, path: site.Path);
        return Document(indices);
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site) => data.Bytes(Pack(CountOf(site.Owner, parent: null, name: CountName, context, site.Path), value: value, context: context, path: site.Path, name: site.Property?.Name ?? "The value"));

    internal override void WriteImage(TreeValue value, EncodeContext context, ByteBuffer data, ImageFrame frame)
    {
        if (value is not BitArrayNode node)
        {
            throw FrameFail(context, frame, code: Codes.Type, message: $"{frame.Property?.Name ?? "The value"} is a bit array.");
        }

        data.Bytes(Pack(node.Count, value: Document(node.Indices), context, frame.Path, name: frame.Property?.Name ?? "The value"));
    }

    internal override TreeValue ReadImage(DecodeContext context, int start, int end, ImageFrame frame)
    {
        int count = CountOf(frame.Owner, frame.Parent, name: CountName, context, frame.Path);
        int[] indices = Unpack(count, bytes: context.Data.AsSpan(start, length: end - start), context, frame.Path);
        return new BitArrayNode(count, indices);
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (site.Schema.Version < 3)
        {
            throw site.Fail(Codes.Type, message: $"Version {site.Schema.Version} does not store a bit array.");
        }

        if (value is not BitArrayNode node)
        {
            throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a bit array.");
        }

        JsonValue json = Document(node.Indices);
        _ = Pack(node.Count, value: json, site.Context, site.Path, name: site.Property?.Name ?? "The value");
        return json;
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (site.Schema.Version < 3)
        {
            throw site.Fail(Codes.Type, message: $"Version {site.Schema.Version} does not store a bit array.", at: value);
        }

        int count = CountOf(site.Owner, parent: null, name: CountName, site.Context, site.Path);
        byte[] bytes = Pack(count, value, site.Context, site.Path, name: site.Property?.Name ?? "The value");
        int[] indices = Unpack(count, bytes, site);
        return new BitArrayNode(count, indices);
    }

    internal static JsonArray Document(IReadOnlyList<int> indices)
    {
        JsonArray array = new();
        array.Items.AddRange(indices.Select(static index => new JsonInteger(index)));
        return array;
    }

    internal static byte[] Pack(int count, JsonValue value, EncodeContext context, DocumentNodePath path, string name)
    {
        if (count < 0)
        {
            throw context.Fail(Codes.Range, message: $"{name} has a negative count.", at: value, path, value: Text(count));
        }

        int length = (count + 7) >> 3;
        if (length == 0)
        {
            throw context.Fail(Codes.Range, message: $"{name} of 0 bits is omitted.", at: value, path);
        }

        if (value is not JsonArray array)
        {
            throw context.Fail(Codes.Type, message: $"{name} is an array of bit indices.", at: value, path, value: value.KindName);
        }

        int[] indices = ReadIndices(array, limit: length * 8, context, path, name);
        byte[] bytes = new byte[length];
        foreach (int index in indices)
        {
            bytes[index >> 3] |= (byte)(1 << (index & 7));
        }

        return bytes;
    }

    internal static int[] Unpack(int count, ReadOnlySpan<byte> bytes, DecodeContext context, DocumentNodePath path)
    {
        int length = count < 0 ? -1 : (count + 7) >> 3;
        return count < 0 || bytes.Length != length
            ? throw context.Fail(Codes.Range, message: $"A bit array of {count} bits is stored in {bytes.Length} bytes.", offset: 0, path, value: Text(bytes.Length), constraint: $"{length} bytes")
            : Split(bytes);
    }

    private static int[] Unpack(int count, ReadOnlySpan<byte> bytes, DocumentSite site)
    {
        int length = count < 0 ? -1 : (count + 7) >> 3;
        return count < 0 || bytes.Length != length
            ? throw site.Fail(Codes.Range, message: $"A bit array of {count} bits is stored in {bytes.Length} bytes.", value: Text(bytes.Length), constraint: $"{length} bytes")
            : Split(bytes);
    }

    private static int[] Split(ReadOnlySpan<byte> bytes)
    {
        int storedBits = bytes.Length * 8;
        List<int> indices = [];
        for (int i = 0; i < storedBits; i++)
        {
            if ((bytes[i >> 3] & (1 << (i & 7))) != 0)
            {
                indices.Add(i);
            }
        }

        return [.. indices];
    }

    /// <summary>Set bit indices: ascending, each once, each below <paramref name="limit"/>.</summary>
    internal static int[] ReadIndices(JsonArray array, int limit, EncodeContext context, DocumentNodePath path, string name)
    {
        if (array.Items.Count > context.Limits.CollectionItems)
        {
            throw context.Fail(Codes.Limit, message: "A collection has too many items.", at: array, path, value: null, constraint: $"at most {context.Limits.CollectionItems} items");
        }

        int[] indices = new int[array.Items.Count];
        int previous = -1;
        for (int i = 0; i < array.Items.Count; i++)
        {
            if (array.Items[i] is not JsonInteger number)
            {
                throw context.Fail(Codes.Type, message: $"{name} is an integer.", at: array.Items[i], path: path.Item(i), value: array.Items[i].KindName);
            }

            if (number.Value < 0 || number.Value >= limit)
            {
                throw context.Fail(Codes.Range, message: $"{name} is outside its storage range.", at: array.Items[i], path: path.Item(i), value: Text(number.Value), constraint: $"0 to {limit - 1}");
            }

            int index = (int)number.Value;
            if (index == previous)
            {
                throw context.Fail(Codes.Duplicate, message: $"{name} lists bit {index} twice.", at: array.Items[i], path: path.Item(i));
            }

            if (index < previous)
            {
                throw context.Fail(Codes.Range, message: $"{name} lists bits out of order.", at: array.Items[i], path: path.Item(i), value: Text(index));
            }

            previous = index;
            indices[i] = index;
        }
        return indices;
    }

    private static int CountOf(JsonObject? owner, ObjectNode? parent, string name, DecodeContext context, DocumentNodePath path)
    {
        return owner?[name] is JsonInteger json
            ? json.Value is < 0 or > int.MaxValue
                ? throw context.Fail(Codes.Range, message: $"{name} is a non-negative 32-bit integer.", offset: 0, path, value: Text(json.Value))
                : (int)json.Value
            : parent?.Find(name) is IntegerNode node
            ? node.Value is < 0 or > int.MaxValue
                ? throw context.Fail(Codes.Range, message: $"{name} is a non-negative 32-bit integer.", offset: 0, path, value: Text(node.Value))
                : (int)node.Value
            : 0;
    }

    private static int CountOf(JsonObject? owner, ObjectNode? parent, string name, EncodeContext context, DocumentNodePath path)
    {
        return owner?[name] is JsonInteger json
            ? json.Value is < 0 or > int.MaxValue
                ? throw context.Fail(Codes.Range, message: $"{name} is a non-negative 32-bit integer.", at: json, path, value: Text(json.Value))
                : (int)json.Value
            : parent?.Find(name) is IntegerNode node
            ? node.Value is < 0 or > int.MaxValue
                ? throw context.Fail(Codes.Range, message: $"{name} is a non-negative 32-bit integer.", at: null, path, value: Text(node.Value))
                : (int)node.Value
            : 0;
    }

    private static ConversionException FrameFail(EncodeContext context, ImageFrame frame, string code, string message)
        => context.Fail(code, message, at: null, frame.Path);
}

/// <summary>
/// A bool stored through rwData. The byte is present even when false.
/// 0 and 1 are booleans. Any other byte is that integer, so the image round-trips.
/// </summary>
internal sealed partial class RawBooleanCodec : PropertyCodec
{
    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        return end - start != 1
            ? throw Mismatch(context, start, site, message: $"A raw boolean is stored in {end - start} bytes.")
            : Value(context.Data[start]);
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site) => data.Byte(ByteOf(context, value, site.Path, name: site.Property?.Name ?? "The value"));

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        return value is BooleanNode bit
            ? new JsonBoolean(bit.Value)
            : value is IntegerNode { Width: 1, Signed: false } number
            ? number.Value is < 0 or > 255
                ? throw site.Fail(Codes.Range, message: $"{site.Property?.Name ?? "The value"} is outside its storage range.", value: Text(number.Value), constraint: "0 to 255")
                : (JsonValue)new JsonInteger(number.Value)
            : throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a boolean.");
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        return value is JsonBoolean bit
            ? new BooleanNode(bit.Value)
            : value is JsonInteger number
            ? number.Value is < 0 or > 255
                ? throw site.Fail(Codes.Range, message: $"{site.Property?.Name ?? "The value"} is outside its storage range.", at: value, value: Text(number.Value), constraint: "0 to 255")
                : number.Value is 0 or 1 ? new BooleanNode(number.Value != 0) : new IntegerNode(1, signed: false, number.Value)
            : throw site.Mismatch($"{site.Property?.Name ?? "The value"} is a boolean.", at: value);
    }

    /// <summary>The document value of a raw boolean byte: false, true, or the integer of any other byte.</summary>
    internal static JsonValue Value(byte value) => value switch
    {
        0 => new JsonBoolean(false),
        1 => new JsonBoolean(true),
        _ => new JsonInteger(value),
    };

    internal static byte ByteOf(EncodeContext context, JsonValue value, DocumentNodePath path, string name)
    {
        if (value is JsonBoolean bit)
        {
            return bit.Value ? (byte)1 : (byte)0;
        }

        if (value is JsonInteger { Value: >= 0 and <= 255 } number)
        {
            return (byte)number.Value;
        }

        if (value is JsonInteger integer)
        {
            throw context.Fail(Codes.Range, message: $"{name} is outside its storage range.", at: integer, path, value: Text(integer.Value), constraint: "0 to 255");
        }

        throw context.Fail(Codes.Type, message: $"{name} is a boolean.", at: value, path, value: value.KindName);
    }
}
