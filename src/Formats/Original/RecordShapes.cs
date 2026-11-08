using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Original;

/// <summary>
/// A record named by the C# struct in the record catalog. A one-field set wrapper is that set.
/// Other records are objects of named fields. Reserved bytes and padding are <c>$reserved</c> when any byte is nonzero.
/// </summary>
internal sealed partial class CatalogRecordCodec(string typeName, int size) : PropertyCodec
{
    public string TypeName { get; } = typeName;

    /// <summary>The version 2 to 3 step builds its records from this layout and does not consult the live catalog.</summary>
    internal CatalogRecordCodec(RecordTypeInfo pinned) : this(pinned.Name, pinned.Size) => known = pinned;

    private RecordTypeInfo? known;
    private readonly Dictionary<string, PropertyCodec> codecs = new();

    internal override bool AcceptsDocumentArray(int version) => version >= 3 && Info(null, "").BareSet;

    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        var info = Info(ctx.File, site.Path.ToString());
        if (end - start != info.Size)
            throw Mismatch(ctx, start, site, "A record of " + info.Size + " bytes is stored in " + (end - start) + " bytes.");
        if (info.BareSet) return FieldCodec(info.Fields[0]).Decode(ctx, start, end, site);
        return Document(info, ctx.Data.AsSpan(start, info.Size), ctx, site);
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        var info = Info(ctx.File, site.Path.ToString());
        if (info.BareSet)
        {
            FieldCodec(info.Fields[0]).Encode(ctx, value, data, site);
            return;
        }
        data.Bytes(Pack(info, value, ctx, site));
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        RefuseOld(site, null);
        var info = Info(site.Context.File, site.Path.ToString());
        if (info.BareSet) return FieldCodec(info.Fields[0]).WriteTree(value, site);
        if (value is not RecordNode node) throw site.Mismatch((site.Property?.Name ?? TypeName) + " is a record.");
        var json = new JsonObject();
        var seen = new HashSet<string>();
        int residueAt = info.Residue.Count == 0 ? -1 : info.Residue[0].Offset;
        bool residue = false;
        foreach (var field in info.Fields)
        {
            if (!residue && residueAt >= 0 && residueAt < field.Offset)
            {
                AddResidue(node, json, site);
                residue = true;
            }
            var found = node.Find(field.Name);
            if (found is null) continue;
            var at = site.At(site.Path.Member(field.Name), false);
            JsonValue? written = FieldCodec(field).WriteTree(found, at);
            if (written is not null && !field.OmitsDocument(written)) json.Add(field.Name, written);
            seen.Add(field.Name);
        }
        if (!residue && node.Find("$reserved") is not null) AddResidue(node, json, site);
        seen.Add("$reserved");
        foreach (var (name, _) in node.Members)
            if (!seen.Contains(name))
                throw site.At(site.Path.Member(name), false).Fail(Codes.UnknownMember, "The record has no member \"" + name + "\".", value: name);
        return json;
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        RefuseOld(site, value);
        var info = Info(site.Context.File, site.Path.ToString());
        if (info.BareSet) return FieldCodec(info.Fields[0]).ReadTree(value, site);
        if (value is not JsonObject obj) throw site.Mismatch((site.Property?.Name ?? TypeName) + " is a record.", value);
        CheckNames(obj, info, site);
        var node = new RecordNode();
        foreach (var field in info.Fields)
        {
            var member = obj.Find(field.Name);
            if (member is null) continue;
            var at = site.At(site.Path.Member(field.Name), false);
            TreeValue? read = FieldCodec(field).ReadTree(member.Value, at);
            if (read is not null && !field.OmitsImage(read)) node.Add(field.Name, read);
        }
        if (obj.Find("$reserved") is { } reserved)
        {
            var at = site.At(site.Path.Member("$reserved"), false);
            if (reserved.Value is not JsonString hex)
                throw at.Fail(Codes.Type, "$reserved is lowercase hexadecimal.", reserved.Value);
            node.Add("$reserved", new BytesNode(ResidueBytes(hex.Value, info, at)));
        }
        return node;
    }

    private JsonObject Document(RecordTypeInfo info, ReadOnlySpan<byte> bytes, DecodeContext ctx, ValueSite site)
    {
        var json = new JsonObject();
        int residueAt = info.Residue.Count == 0 ? -1 : info.Residue[0].Offset;
        bool residue = false;
        foreach (var field in info.Fields)
        {
            if (!residue && residueAt >= 0 && residueAt < field.Offset)
            {
                AddResidue(bytes, info, json);
                residue = true;
            }
            var fieldSite = new ValueSite { Property = site.Property, Id = site.Id, Path = site.Path.Member(field.Name), Owner = site.Owner, Builder = site.Builder };
            var value = FieldValue(field, bytes.Slice(field.Offset, field.Size), ctx, fieldSite);
            if (value is not null) json.Add(field.Name, value);
        }
        if (!residue) AddResidue(bytes, info, json);
        return json;
    }

    private JsonValue? FieldValue(RecordFieldInfo field, ReadOnlySpan<byte> bytes, DecodeContext ctx, ValueSite site)
    {
        var codec = FieldCodec(field);
        var copy = bytes.ToArray();
        var nested = new DecodeContext(copy, ctx.Schema, ctx.Engine, ctx.Profile, ctx.File, ctx.Limits, ctx.Counts) { Nested = site.Path.ToString() };
        var value = codec.Decode(nested, 0, copy.Length, site);
        if (field.OmitsDocument(value)) return null;
        return value;
    }

    private byte[] Pack(RecordTypeInfo info, JsonValue value, EncodeContext ctx, ValueSite parent)
    {
        string name = parent.Property?.Name ?? TypeName;
        if (value is not JsonObject obj) throw ctx.Fail(Codes.Type, name + " is a record.", value, parent.Path, value.KindName);
        CheckNames(obj, info, ctx, parent.Path);
        var bytes = new byte[info.Size];
        foreach (var field in info.Fields)
        {
            var member = obj.Find(field.Name);
            if (member is null) continue;
            var fieldPath = parent.Path.Member(field.Name);
            var site = new ValueSite { Property = parent.Property!, Id = 0, Path = fieldPath, Owner = obj };
            WriteField(field, member.Value, ctx, site, bytes.AsSpan(field.Offset, field.Size));
        }
        if (obj.Find("$reserved") is { } reserved)
        {
            if (reserved.Value is not JsonString hex)
                throw ctx.Fail(Codes.Type, "$reserved is lowercase hexadecimal.", reserved.Value, parent.Path.Member("$reserved"), reserved.Value.KindName);
            Scatter(bytes, ResidueBytes(hex.Value, info, ctx, parent.Path), info);
        }
        return bytes;
    }

    private void WriteField(RecordFieldInfo field, JsonValue value, EncodeContext ctx, ValueSite site, Span<byte> target)
    {
        var buffer = new ByteBuffer(field.Size);
        FieldCodec(field).Encode(ctx, value, buffer, site);
        if (buffer.Length != field.Size)
            throw ctx.Fail(Codes.Range, field.Name + " holds exactly " + field.Size + " bytes.", value, site.Path, Text(buffer.Length), field.Size + " bytes");
        buffer.Span.CopyTo(target);
    }

    private void AddResidue(RecordNode node, JsonObject json, DocumentSite site)
    {
        var found = node.Find("$reserved");
        if (found is null) return;
        if (found is not BytesNode bytes)
            throw site.At(site.Path.Member("$reserved"), false).Fail(Codes.Type, "$reserved is lowercase hexadecimal.");
        if (bytes.Data.Length != Info(site.Context.File, site.Path.ToString()).ResidueLength)
            throw site.At(site.Path.Member("$reserved"), false).Fail(Codes.Range, "$reserved holds the record residue.", value: Text(bytes.Data.Length), constraint: Info(site.Context.File, site.Path.ToString()).ResidueLength + " bytes");
        if (AllZero(bytes.Data)) return;
        json.Add("$reserved", new JsonString(LayoutResidue.Hex(bytes.Data)));
    }

    private static void AddResidue(ReadOnlySpan<byte> image, RecordTypeInfo info, JsonObject json)
    {
        if (info.ResidueLength == 0) return;
        var residue = Gather(image, info);
        if (AllZero(residue)) return;
        json.Add("$reserved", new JsonString(LayoutResidue.Hex(residue)));
    }

    private PropertyCodec FieldCodec(RecordFieldInfo field)
    {
        if (codecs.TryGetValue(field.Name, out var codec)) return codec;
        codec = field.CreateCodec();
        codecs.Add(field.Name, codec);
        return codec;
    }

    private RecordTypeInfo Info(string? file, string path)
    {
        if (known is not null) return known;
        var info = RecordCatalog.Require(TypeName, file, path);
        if (info.Size != size)
            throw new InvalidOperationException("Record " + TypeName + " is " + info.Size + " bytes in the catalog and " + size + " bytes in the schema.");
        known = info;
        return info;
    }

    private static void RefuseOld(DocumentSite site, JsonValue? value)
    {
        if (site.Schema.Version >= 3) return;
        throw site.Fail(Codes.Type, "Version " + site.Schema.Version + " does not store a record.", value);
    }

    private static void CheckNames(JsonObject obj, RecordTypeInfo info, DocumentSite site)
    {
        var known = info.Fields.Select(field => field.Name).ToHashSet();
        foreach (var member in obj.Members)
        {
            if (member.Name == "$reserved") continue;
            if (member.Name.StartsWith('$'))
                throw site.At(site.Path.Member(member.Name), false).Fail(Codes.ReservedMember,
                    Reserved.IsKnown(member.Name) ? "Reserved member \"" + member.Name + "\" is not allowed here." : "Unknown reserved member \"" + member.Name + "\".",
                    member.Value, member.Name);
            if (!known.Contains(member.Name))
                throw site.At(site.Path.Member(member.Name), false).Fail(Codes.UnknownMember, "The record has no member \"" + member.Name + "\".", member.Value, member.Name);
        }
    }

    private static void CheckNames(JsonObject obj, RecordTypeInfo info, EncodeContext ctx, DocPath path)
    {
        var known = info.Fields.Select(field => field.Name).ToHashSet();
        foreach (var member in obj.Members)
        {
            if (member.Name == "$reserved") continue;
            if (member.Name.StartsWith('$'))
                throw ctx.Fail(Reserved.IsKnown(member.Name) ? Codes.ReservedMember : Codes.ReservedMember,
                    Reserved.IsKnown(member.Name) ? "Reserved member \"" + member.Name + "\" is not allowed here." : "Unknown reserved member \"" + member.Name + "\".",
                    member.Value, path.Member(member.Name), member.Name);
            if (!known.Contains(member.Name))
                throw ctx.Fail(Codes.UnknownMember, "The record has no member \"" + member.Name + "\".", member.Value, path.Member(member.Name), member.Name);
        }
    }

    private static byte[] ResidueBytes(string hex, RecordTypeInfo info, DocumentSite site)
    {
        if (!HexText.IsLowerHex(hex)) throw site.Fail(Codes.Type, "$reserved is lowercase hexadecimal.", value: hex);
        var bytes = HexText.Decode(hex);
        if (bytes.Length != info.ResidueLength)
            throw site.Fail(Codes.Range, "$reserved holds the record residue.", value: Text(bytes.Length), constraint: info.ResidueLength + " bytes");
        return bytes;
    }

    private static byte[] ResidueBytes(string hex, RecordTypeInfo info, EncodeContext ctx, DocPath path)
    {
        if (!HexText.IsLowerHex(hex)) throw ctx.Fail(Codes.Type, "$reserved is lowercase hexadecimal.", null, path.Member("$reserved"), hex);
        var bytes = HexText.Decode(hex);
        if (bytes.Length != info.ResidueLength)
            throw ctx.Fail(Codes.Range, "$reserved holds the record residue.", null, path.Member("$reserved"), Text(bytes.Length), info.ResidueLength + " bytes");
        return bytes;
    }

    internal static byte[] Gather(ReadOnlySpan<byte> image, RecordTypeInfo info)
    {
        var residue = new byte[info.ResidueLength];
        int at = 0;
        foreach (var (offset, length) in info.Residue)
        {
            image.Slice(offset, length).CopyTo(residue.AsSpan(at));
            at += length;
        }
        return residue;
    }

    private static void Scatter(byte[] image, ReadOnlySpan<byte> residue, RecordTypeInfo info)
    {
        int at = 0;
        foreach (var (offset, length) in info.Residue)
        {
            residue.Slice(at, length).CopyTo(image.AsSpan(offset));
            at += length;
        }
    }

    private static bool AllZero(ReadOnlySpan<byte> bytes)
    {
        foreach (var b in bytes)
            if (b != 0) return false;
        return true;
    }
}

/// <summary>
/// A counted bit array stored by rwData. The document is the ascending indices of the set bits.
/// Bits past the count in the last byte are <c>$reserved</c> on the value when any of them is set.
/// The count is the sibling integer named by the schema.
/// </summary>
internal sealed class BitArrayCodec(string countName) : PropertyCodec
{
    public string CountName { get; } = countName;

    internal override bool AcceptsDocumentArray(int version) => version >= 3;

    internal override bool StoresImage(TreeValue value) => value is not BitArrayNode node || ((node.Count + 7) >> 3) != 0;

    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        int count = CountOf(site.Owner, null, CountName, ctx, site.Path);
        var (indices, residue) = Unpack(count, ctx.Data.AsSpan(start, end - start), ctx, site.Path);
        return Document(indices, residue);
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        int count = CountOf(site.Owner, null, CountName, ctx, site.Path);
        data.Bytes(Pack(count, value, ctx, site.Path, site.Property?.Name ?? "The value"));
    }

    internal override void WriteImage(TreeValue value, EncodeContext ctx, ByteBuffer data, ImageFrame frame)
    {
        if (value is not BitArrayNode node) throw FrameFail(ctx, frame, Codes.Type, (frame.Property?.Name ?? "The value") + " is a bit array.");
        data.Bytes(Pack(node.Count, Document(node.Indices, node.Residue), ctx, frame.Path, frame.Property?.Name ?? "The value"));
    }

    internal override TreeValue ReadImage(DecodeContext ctx, int start, int end, ImageFrame frame)
    {
        int count = CountOf(frame.Owner, frame.Parent, CountName, ctx, frame.Path);
        var (indices, residue) = Unpack(count, ctx.Data.AsSpan(start, end - start), ctx, frame.Path);
        return new BitArrayNode(count, indices, residue);
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (site.Schema.Version < 3)
            throw site.Fail(Codes.Type, "Version " + site.Schema.Version + " does not store a bit array.");
        if (value is not BitArrayNode node) throw site.Mismatch((site.Property?.Name ?? "The value") + " is a bit array.");
        var json = Document(node.Indices, node.Residue);
        Pack(node.Count, json, site.Context, site.Path, site.Property?.Name ?? "The value");
        return json;
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (site.Schema.Version < 3)
            throw site.Fail(Codes.Type, "Version " + site.Schema.Version + " does not store a bit array.", value);
        int count = CountOf(site.Owner, null, CountName, site.Context, site.Path);
        var bytes = Pack(count, value, site.Context, site.Path, site.Property?.Name ?? "The value");
        var (indices, residue) = Unpack(count, bytes, site);
        return new BitArrayNode(count, indices, residue);
    }

    internal static JsonValue Document(IReadOnlyList<int> indices, byte residue)
    {
        var array = new JsonArray();
        foreach (var index in indices) array.Add(new JsonInteger(index));
        if (residue == 0) return array;
        return new JsonObject().Add("values", array).Add("$reserved", new JsonString(LayoutResidue.Hex([residue])));
    }

    internal static byte[] Pack(int count, JsonValue value, EncodeContext ctx, DocPath path, string name)
    {
        if (count < 0) throw ctx.Fail(Codes.Range, name + " has a negative count.", value, path, Text(count));
        int length = (count + 7) >> 3;
        if (length == 0) throw ctx.Fail(Codes.Range, name + " of 0 bits is omitted.", value, path);
        var (indices, residue) = Read(value, count, ctx, path, name);
        var bytes = new byte[length];
        foreach (var index in indices) bytes[index >> 3] |= (byte)(1 << (index & 7));
        if ((count & 7) != 0) bytes[length - 1] |= residue;
        return bytes;
    }

    internal static (int[] Indices, byte Residue) Unpack(int count, ReadOnlySpan<byte> bytes, DecodeContext ctx, DocPath path)
    {
        int length = count < 0 ? -1 : (count + 7) >> 3;
        if (count < 0 || bytes.Length != length)
            throw ctx.Fail(Codes.Range, "A bit array of " + count + " bits is stored in " + bytes.Length + " bytes.", 0, path, Text(bytes.Length), length + " bytes");
        return Split(count, bytes);
    }

    private static (int[] Indices, byte Residue) Unpack(int count, ReadOnlySpan<byte> bytes, DocumentSite site)
    {
        int length = count < 0 ? -1 : (count + 7) >> 3;
        if (count < 0 || bytes.Length != length)
            throw site.Fail(Codes.Range, "A bit array of " + count + " bits is stored in " + bytes.Length + " bytes.", value: Text(bytes.Length), constraint: length + " bytes");
        return Split(count, bytes);
    }

    private static (int[] Indices, byte Residue) Split(int count, ReadOnlySpan<byte> bytes)
    {
        int used = count & 7;
        byte residue = 0;
        if (used != 0 && bytes.Length != 0) residue = (byte)(bytes[^1] & (byte)(0xFF << used));
        var indices = new List<int>();
        for (int i = 0; i < count; i++)
            if ((bytes[i >> 3] & (1 << (i & 7))) != 0) indices.Add(i);
        return (indices.ToArray(), residue);
    }

    private static (int[] Indices, byte Residue) Read(JsonValue value, int count, EncodeContext ctx, DocPath path, string name)
    {
        JsonArray array;
        byte residue = 0;
        if (value is JsonArray plain) array = plain;
        else if (value is JsonObject obj)
        {
            foreach (var member in obj.Members)
                if (member.Name is not ("values" or "$reserved"))
                    throw ctx.Fail(member.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember,
                        "The bit array has no member \"" + member.Name + "\".", member.Value, path.Member(member.Name), member.Name);
            if (obj["values"] is not JsonArray values) throw ctx.Fail(Codes.MissingMember, name + " needs values.", obj, path);
            array = values;
            if (obj["$reserved"] is JsonValue reserved)
            {
                if (reserved is not JsonString hex || !HexText.IsLowerHex(hex.Value) || hex.Value.Length != 2)
                    throw ctx.Fail(Codes.Type, "$reserved is one lowercase hexadecimal byte.", reserved, path.Member("$reserved"));
                residue = HexText.Decode(hex.Value)[0];
                int used = count & 7;
                if (used == 0 || (residue & (byte)~(0xFF << used)) != 0)
                    throw ctx.Fail(Codes.Range, "$reserved keeps only the bits past the count.", reserved, path.Member("$reserved"));
            }
        }
        else throw ctx.Fail(Codes.Type, name + " is an array of bit indices.", value, path, value.KindName);
        if (array.Items.Count > ctx.Limits.CollectionItems)
            throw ctx.Fail(Codes.Limit, "A collection has too many items.", array, path, null, "at most " + ctx.Limits.CollectionItems + " items");
        var indices = new int[array.Items.Count];
        int previous = -1;
        for (int i = 0; i < array.Items.Count; i++)
        {
            if (array.Items[i] is not JsonInteger number)
                throw ctx.Fail(Codes.Type, name + " is an integer.", array.Items[i], path.Item(i), array.Items[i].KindName);
            if (number.Value < 0 || number.Value >= count)
                throw ctx.Fail(Codes.Range, name + " is outside its storage range.", array.Items[i], path.Item(i), Text(number.Value), "0 to " + (count - 1));
            int index = (int)number.Value;
            if (index == previous)
                throw ctx.Fail(Codes.Duplicate, name + " lists bit " + index + " twice.", array.Items[i], path.Item(i));
            if (index < previous)
                throw ctx.Fail(Codes.Range, name + " lists bits out of order.", array.Items[i], path.Item(i), Text(index));
            previous = index;
            indices[i] = index;
        }
        return (indices, residue);
    }

    private static int CountOf(JsonObject? owner, ObjectNode? parent, string name, DecodeContext ctx, DocPath path)
    {
        if (owner?[name] is JsonInteger json)
        {
            if (json.Value < 0 || json.Value > int.MaxValue)
                throw ctx.Fail(Codes.Range, name + " is a non-negative 32-bit integer.", 0, path, Text(json.Value));
            return (int)json.Value;
        }
        if (parent?.Find(name) is IntegerNode node)
        {
            if (node.Value < 0 || node.Value > int.MaxValue)
                throw ctx.Fail(Codes.Range, name + " is a non-negative 32-bit integer.", 0, path, Text(node.Value));
            return (int)node.Value;
        }
        return 0;
    }

    private static int CountOf(JsonObject? owner, ObjectNode? parent, string name, EncodeContext ctx, DocPath path)
    {
        if (owner?[name] is JsonInteger json)
        {
            if (json.Value < 0 || json.Value > int.MaxValue)
                throw ctx.Fail(Codes.Range, name + " is a non-negative 32-bit integer.", json, path, Text(json.Value));
            return (int)json.Value;
        }
        if (parent?.Find(name) is IntegerNode node)
        {
            if (node.Value < 0 || node.Value > int.MaxValue)
                throw ctx.Fail(Codes.Range, name + " is a non-negative 32-bit integer.", null, path, Text(node.Value));
            return (int)node.Value;
        }
        return 0;
    }

    private static ConversionException FrameFail(EncodeContext ctx, ImageFrame frame, string code, string message) =>
        ctx.Fail(code, message, null, frame.Path);
}

/// <summary>
/// A bool stored through rwData. The byte is present even when false.
/// 0 and 1 are booleans. Any other byte is that integer, so the image round-trips.
/// </summary>
internal sealed partial class RawBoolCodec : PropertyCodec
{
    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        if (end - start != 1) throw Mismatch(ctx, start, site, "A raw boolean is stored in " + (end - start) + " bytes.");
        byte value = ctx.Data[start];
        if (value == 0) return new JsonBool(false);
        if (value == 1) return new JsonBool(true);
        return new JsonInteger(value);
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        data.Byte(ByteOf(ctx, value, site.Path, site.Property?.Name ?? "The value"));
    }

    internal override JsonValue? WriteTree(TreeValue value, DocumentSite site)
    {
        if (value is BoolNode bit) return new JsonBool(bit.Value);
        if (value is IntegerNode { Width: 1, Signed: false } number)
        {
            if (number.Value is < 0 or > 255)
                throw site.Fail(Codes.Range, (site.Property?.Name ?? "The value") + " is outside its storage range.", value: Text(number.Value), constraint: "0 to 255");
            return new JsonInteger(number.Value);
        }
        throw site.Mismatch((site.Property?.Name ?? "The value") + " is a boolean.");
    }

    internal override TreeValue? ReadTree(JsonValue value, DocumentSite site)
    {
        if (value is JsonBool bit) return new BoolNode(bit.Value);
        if (value is JsonInteger number)
        {
            if (number.Value is < 0 or > 255)
                throw site.Fail(Codes.Range, (site.Property?.Name ?? "The value") + " is outside its storage range.", value, Text(number.Value), "0 to 255");
            if (number.Value is 0 or 1) return new BoolNode(number.Value != 0);
            return new IntegerNode(1, false, number.Value);
        }
        throw site.Mismatch((site.Property?.Name ?? "The value") + " is a boolean.", value);
    }

    internal static byte ByteOf(EncodeContext ctx, JsonValue value, DocPath path, string name)
    {
        if (value is JsonBool bit) return bit.Value ? (byte)1 : (byte)0;
        if (value is JsonInteger { Value: >= 0 and <= 255 } number) return (byte)number.Value;
        if (value is JsonInteger integer)
            throw ctx.Fail(Codes.Range, name + " is outside its storage range.", integer, path, Text(integer.Value), "0 to 255");
        throw ctx.Fail(Codes.Type, name + " is a boolean.", value, path, value.KindName);
    }
}
