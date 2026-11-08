using Aow.Formats.Json;
using Aow.Formats.Schema;

namespace Aow.Formats.Original;

/// <summary>rwEObject: an object whose class the schema fixes. The document has no $class.</summary>
internal sealed partial class FixedObjectCodec(string className) : PropertyCodec
{
    public string ClassName { get; } = className;

    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        var obj = new JsonObject();
        ObjectImage.Decode(ctx, start, end, ctx.Schema.Get(ClassName), site.Path, obj);
        return obj;
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonObject obj) throw ctx.Fail(Codes.Type, $"{site.Property.Name} is an object.", value, site.Path, value.KindName);
        ObjectImage.Encode(ctx, obj, ctx.Schema.Get(ClassName), data, site.Path);
    }
}

/// <summary>
/// rwCreateEObject: a 32-bit class id, 0 for nil, then the object. The class resolves through
/// the profile's engine table. Dynamic members (sprites, ability data, items) are created
/// objects too, with $slot where their id carries identity.
/// </summary>
internal sealed partial class CreatedObjectCodec(string? family) : PropertyCodec
{
    internal string? Family => family;

    internal static JsonObject DecodeCreated(DecodeContext ctx, int start, int end, ValueSite site, string? family, int? slot, out bool isNull)
    {
        ctx.Need(start, 4, end, site.Path, "class id");
        int classId = ctx.Int32(start);
        var obj = new JsonObject();
        if (classId == 0)
        {
            if (end != start + 4) throw ctx.Fail(Codes.Corrupt, "Bytes follow a nil object.", start + 4, site.Path);
            isNull = true;
            obj.Add("$class", new JsonNull());
            if (slot is int s0) obj.Add("$slot", new JsonInteger(s0));
            return obj;
        }
        isNull = false;
        var cls = ctx.Schema.Resolve(ctx.Engine, classId)
            ?? throw ctx.Fail(Codes.UnknownClass, $"Class id {classId} is not in the {ctx.Engine} engine table.", start, site.Path, classId.ToString(System.Globalization.CultureInfo.InvariantCulture));
        var shape = site.Property is null ? null : ctx.Schema.LegacyShape(site.Property.Owner.Name + "." + site.Property.Name, cls.Name);
        if (shape is null && family is not null && !cls.InheritsFrom(family))
            throw ctx.Fail(Codes.ClassFamily, $"Class {cls.Name} is not a {family}.", start, site.Path, cls.Name, family);
        obj.Add("$class", new JsonString(cls.Name));
        if (slot is int s) obj.Add("$slot", new JsonInteger(s));
        if (shape is { } legacy)
        {
            ctx.Counts.LegacyRules[legacy.Rule] = ctx.Counts.LegacyRules.GetValueOrDefault(legacy.Rule) + 1;
            ObjectImage.Decode(ctx, start + 4, end, legacy.Shape, site.Path, obj);
        }
        else ObjectImage.Decode(ctx, start + 4, end, cls, site.Path, obj);
        return obj;
    }

    internal static void EncodeCreated(EncodeContext ctx, JsonObject obj, ByteBuffer data, ValueSite site, string? family, bool slotted)
    {
        var classMember = obj.Find("$class") ?? throw ctx.Fail(Codes.MissingMember, "A created object needs $class.", obj, site.Path);
        string[] reserved = slotted ? ["$class", "$slot"] : ["$class"];
        if (classMember.Value is JsonNull)
        {
            foreach (var m in obj.Members)
                if (Array.IndexOf(reserved, m.Name) < 0)
                    throw ctx.Fail(Codes.UnknownMember, "A nil object has no members.", m.Value, site.Path.Member(m.Name));
            data.Int32(0);
            return;
        }
        if (classMember.Value is not JsonString name) throw ctx.Fail(Codes.Type, "$class is a class name or null.", classMember.Value, site.Path.Member("$class"));
        var cls = ctx.Schema.Find(name.Value)
            ?? throw ctx.Fail(Codes.UnknownClass, $"Unknown class {name.Value}.", name, site.Path.Member("$class"), name.Value);
        if (cls.Id is not int id || ctx.Schema.Resolve(ctx.Engine, id) != cls)
            throw ctx.Fail(Codes.UnknownClass, $"Class {cls.Name} is not in the {ctx.Engine} engine table.", name, site.Path.Member("$class"), cls.Name);
        var shape = site.Property is null ? null : ctx.Schema.LegacyShape(site.Property.Owner.Name + "." + site.Property.Name, cls.Name);
        if (shape is null && family is not null && !cls.InheritsFrom(family))
            throw ctx.Fail(Codes.ClassFamily, $"Class {cls.Name} is not a {family}.", name, site.Path.Member("$class"), cls.Name, family);
        data.Int32(id);
        ObjectImage.Encode(ctx, obj, shape?.Shape ?? cls, data, site.Path, reserved);
    }

    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        var obj = DecodeCreated(ctx, start, end, site, family, null, out bool isNull);
        return isNull ? new JsonNull() : obj;
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is JsonNull)
        {
            data.Int32(0);
            return;
        }
        if (value is not JsonObject obj) throw ctx.Fail(Codes.Type, $"{site.Property.Name} is an object or null.", value, site.Path, value.KindName);
        if (obj["$class"] is JsonNull) throw ctx.Fail(Codes.Type, "A stored nil object is null, not $class null.", obj, site.Path);
        EncodeCreated(ctx, obj, data, site, family, false);
    }

    internal override JsonValue DecodeElement(DecodeContext ctx, int start, int end, ValueSite site)
    {
        int? slot = site.Property.SlotBase is int b ? site.Id - b : null;
        return DecodeCreated(ctx, start, end, site, family, slot, out _);
    }

    internal override int ElementId(EncodeContext ctx, JsonValue value, SchemaProperty property, int index, DocPath path)
    {
        if (value is not JsonObject obj) throw ctx.Fail(Codes.Type, $"An item of {property.Name} is an object.", value, path, value.KindName);
        if (property.SlotBase is not int b)
        {
            if (obj.Find("$slot") is { } extra) throw ctx.Fail(Codes.ReservedMember, $"Items of {property.Name} have no $slot.", extra.Value, path.Member("$slot"));
            return property.DynamicMinimum!.Value + index;
        }
        if (obj["$slot"] is not JsonInteger slot) throw ctx.Fail(Codes.MissingMember, $"An item of {property.Name} needs an integer $slot.", obj, path);
        long id = slot.Value + b;
        if (slot.Value < 0 || id < property.DynamicMinimum!.Value || id > int.MaxValue)
            throw ctx.Fail(Codes.Range, "The $slot is outside the stored id range.", slot, path.Member("$slot"));
        return (int)id;
    }

    internal override void EncodeElement(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site) =>
        EncodeCreated(ctx, (JsonObject)value, data, site, family, site.Property.SlotBase is not null);
}

/// <summary>
/// rwChildren: a nested table whose entry ids are the children's slots, each a created object.
/// The document lists them in stored order with an explicit $slot.
/// </summary>
internal sealed partial class ChildrenCodec : PropertyCodec
{
    internal override bool AcceptsDocumentArray(int version) => true;

    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        var table = PropertyTable.Read(ctx, start, end, site.Path);
        int db = table.DataBase;
        int length = end - db;
        var boundaries = table.Entries.Select(e => e.Offset).Append(length).Distinct().Order().ToArray();
        var order = table.Entries.OrderBy(e => e.Offset).ThenBy(e => e.Index).ToList();
        var slots = new HashSet<int>();
        var array = new JsonArray();
        int cursor = 0, k = 0;
        var gaps = new List<(int, int)>();
        for (int i = 0; i < order.Count; i++)
        {
            var e = order[i];
            if (i + 1 < order.Count && order[i + 1].Offset == e.Offset)
                throw ctx.Fail(Codes.Layout, "Two children share one offset.", db + e.Offset, site.Path);
            if (!slots.Add(e.Id)) throw ctx.Fail(Codes.Duplicate, $"Child slot {e.Id} is stored twice.", db + e.Offset, site.Path);
            if (e.Offset > cursor) gaps.Add((cursor, e.Offset - cursor));
            int next = boundaries[Array.BinarySearch(boundaries, e.Offset) + 1];
            var child = new ValueSite { Property = site.Property, Id = e.Id, Path = site.Path.Item(k++), Owner = site.Owner, Builder = site.Builder };
            array.Add(CreatedObjectCodec.DecodeCreated(ctx, db + e.Offset, db + next, child, null, e.Id, out _));
            cursor = next;
        }
        if (cursor < length) gaps.Add((cursor, length - cursor));
        ctx.CheckItems(array.Items.Count, start, site.Path);
        if (gaps.Count != 0 || table.EmptyLongMarker || !ObjectImage.MatchesWriterTable(table.Entries, order))
        {
            site.Builder?.Table(table.Entries, table.EmptyLongMarker, site.Id);
            foreach (var (offset, count) in gaps) site.Builder?.Gap(offset, ctx.Data.AsSpan(db + offset, count), site.Id);
        }
        return array;
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonArray array) throw ctx.Fail(Codes.Type, $"{site.Property.Name} is an array.", value, site.Path, value.KindName);
        if (array.Items.Count > ctx.Limits.CollectionItems)
            throw ctx.Fail(Codes.Limit, "A collection has too many items.", array, site.Path, null, $"at most {ctx.Limits.CollectionItems} items");
        var writer = new ObjectWriter();
        var slots = new Dictionary<int, JsonValue>();
        for (int k = 0; k < array.Items.Count; k++)
        {
            var itemPath = site.Path.Item(k);
            if (array.Items[k] is not JsonObject child) throw ctx.Fail(Codes.Type, "A child is an object.", array.Items[k], itemPath);
            if (child["$slot"] is not JsonInteger { Value: >= 0 and <= int.MaxValue } slot)
                throw ctx.Fail(Codes.MissingMember, "A child needs a non-negative integer $slot.", child, itemPath);
            if (!slots.TryAdd((int)slot.Value, slot))
                throw ctx.Fail(Codes.Slot, $"Child slot {slot.Value} is declared twice, first at {slots[(int)slot.Value].Span.Line}:{slots[(int)slot.Value].Span.Column}.", slot, itemPath.Member("$slot"));
            if (child["$class"] is JsonNull) throw ctx.Fail(Codes.Type, "A stored child is never nil.", child, itemPath);
            var childSite = new ValueSite { Property = site.Property, Id = (int)slot.Value, Path = itemPath, Owner = site.Owner, Reader = site.Reader };
            writer.Begin((int)slot.Value, site.Property, k);
            CreatedObjectCodec.EncodeCreated(ctx, child, writer.Data, childSite, null, true);
            writer.End();
        }
        ObjectImage.Place(ctx, writer, site.Reader, site.Id, null, value, site.Path, data);
    }
}

/// <summary>
/// TMapLevel property 6: Width times Height field images, each a 32-bit length and a field of
/// the level's field class, in engine y then x order. The document holds Fields[y][x], one row
/// per line.
/// </summary>
internal sealed class MapFieldsCodec : PropertyCodec
{
    internal override string? ImageRefusal => "PE-2c still needs map fields. The direct path does not store them.";

    internal override bool AcceptsDocumentArray(int version) => true;

    private static int Dimension(JsonObject owner, string name) => owner[name] is JsonInteger n ? (int)n.Value : 0;

    /// <summary>The level's class, from its $class, decides the field class (MapFieldClass).</summary>
    private static SchemaClass FieldClass(FormatSchema schema, ValueSite site)
    {
        var level = site.Owner["$class"] is JsonString c ? schema.Find(c.Value) : null;
        return schema.Get((level ?? site.Property.Owner).FieldClass ?? "TMapField");
    }

    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site)
    {
        int width = Dimension(site.Owner, "Width"), height = Dimension(site.Owner, "Height");
        long count = (long)width * height;
        if (count > ctx.Limits.CollectionItems) throw ctx.Fail(Codes.Limit, "The map has too many fields.", start, site.Path, count.ToString(System.Globalization.CultureInfo.InvariantCulture));
        var fieldClass = FieldClass(ctx.Schema, site);
        var rows = new JsonArray { Layout = ArrayLayout.RowsOnLines };
        int at = start;
        for (int y = 0; y < height; y++)
        {
            var row = new JsonArray();
            for (int x = 0; x < width; x++)
            {
                if (at + 4 > end) throw ctx.Fail(Codes.Dimensions, $"The field stream ends before field [{y}][{x}] of a {width} by {height} level.", at, site.Path);
                int length = ctx.Int32(at);
                if (length <= 0 || at + 4L + length > end) throw ctx.Fail(Codes.Corrupt, $"Field [{y}][{x}] has an invalid length {length}.", at, site.Path.Item(y).Item(x));
                var field = new JsonObject();
                ObjectImage.Decode(ctx, at + 4, at + 4 + length, fieldClass, site.Path.Item(y).Item(x), field);
                row.Add(field);
                at += 4 + length;
            }
            rows.Add(row);
        }
        if (at != end) throw ctx.Fail(Codes.Dimensions, $"The field stream holds more than {width} by {height} fields.", at, site.Path);
        return rows;
    }

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site)
    {
        int width = Dimension(site.Owner, "Width"), height = Dimension(site.Owner, "Height");
        if (value is not JsonArray rows) throw ctx.Fail(Codes.Type, "Fields is an array of rows.", value, site.Path, value.KindName);
        if (rows.Items.Count != height)
            throw ctx.Fail(Codes.Dimensions, $"Fields has {rows.Items.Count} rows and Height is {height}.", rows, site.Path, rows.Items.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), $"{height} rows");
        var fieldClass = FieldClass(ctx.Schema, site);
        for (int y = 0; y < rows.Items.Count; y++)
        {
            if (rows.Items[y] is not JsonArray row) throw ctx.Fail(Codes.Type, "A row is an array of fields.", rows.Items[y], site.Path.Item(y));
            if (row.Items.Count != width)
                throw ctx.Fail(Codes.Dimensions, $"Row {y} has {row.Items.Count} fields and Width is {width}.", row, site.Path.Item(y), row.Items.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), $"{width} fields");
            for (int x = 0; x < row.Items.Count; x++)
            {
                if (row.Items[x] is not JsonObject field) throw ctx.Fail(Codes.Type, "A field is an object.", row.Items[x], site.Path.Item(y).Item(x));
                int at = data.Length;
                data.Int32(0);
                ObjectImage.Encode(ctx, field, fieldClass, data, site.Path.Item(y).Item(x));
                data.PatchInt32(at, data.Length - at - 4);
            }
        }
    }
}

/// <summary>
/// TAoWCampaign ids 5 and 6: the size and the bytes of a campaign map, an HSM file image as
/// SaveHSMtoStream writes it. The document holds Map, a nested document with its own
/// $container. The exact compressed bytes stay as a nested-compression part, validated against
/// the visible map before export uses them.
/// </summary>
internal sealed class CampaignMapCodec : PropertyCodec
{
    internal override string? ImageRefusal => "PE-2c still needs a campaign map. The direct path does not store it.";

    private static HsmProfile Nested => (HsmProfile)FileProfile.Find("world-hsm")!;

    internal override JsonValue Decode(DecodeContext ctx, int start, int end, ValueSite site) =>
        throw new InvalidOperationException("The campaign map stores two entries.");

    internal override void Encode(EncodeContext ctx, JsonValue value, ByteBuffer data, ValueSite site) =>
        throw new InvalidOperationException("The campaign map stores two entries.");

    internal override JsonValue DecodePair(DecodeContext ctx, (int Start, int End)? first, (int Start, int End)? second, ValueSite site)
    {
        if (first is not { } size || second is not { } bytes)
            throw ctx.Fail(Codes.Corrupt, "A campaign map needs both its size (id 5) and its bytes (id 6).", (first ?? second)!.Value.Start, site.Path);
        if (size.End - size.Start != 4) throw ctx.Fail(Codes.Corrupt, "The campaign map size is not a 32-bit integer.", size.Start, site.Path);
        int length = ctx.Int32(size.Start);
        if (length <= 0 || length != bytes.End - bytes.Start)
            throw ctx.Fail(Codes.Corrupt, $"The campaign map size {length} differs from its {bytes.End - bytes.Start} stored bytes.", size.Start, site.Path);
        var image = ctx.Data.AsSpan(bytes.Start, length).ToArray();
        var session = new ImportSession
        {
            Schema = ctx.Schema,
            Limits = ctx.Limits,
            Counts = ctx.Counts,
            File = ctx.File,
            Nested = site.Path.ToString(),
        };
        var (container, root) = Nested.Import(image, session);
        var map = new JsonObject().Add("$container", container).Add("root", root);
        var builder = new LayoutBuilder();
        if (Cfs.HasMagic(image) && image[4] != 0) builder.Nested(image);
        builder.Attach(map, Nested.Name, ctx.Counts);
        return map;
    }

    internal override void EncodeMember(EncodeContext ctx, JsonValue value, ObjectWriter writer, ValueSite site)
    {
        if (value is not JsonObject map) throw ctx.Fail(Codes.Type, "Map is a nested document.", value, site.Path, value.KindName);
        foreach (var m in map.Members)
            if (m.Name is not ("$container" or "root" or "$layout"))
                throw ctx.Fail(m.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember, $"Map has no member \"{m.Name}\".", m.Value, site.Path.Member(m.Name));
        var container = map["$container"] as JsonObject ?? throw ctx.Fail(Codes.MissingMember, "Map needs $container.", map, site.Path);
        var root = map["root"] ?? throw ctx.Fail(Codes.MissingMember, "Map needs root.", map, site.Path);
        if (container["profile"] is not JsonString { Value: "world-hsm" })
            throw ctx.Fail(Codes.Profile, "A campaign map has profile world-hsm.", container["profile"] ?? container, site.Path.Member("$container").Member("profile"));
        var nested = new EncodeContext(ctx.Schema, Nested.Engine, Nested.Name, ctx.File, ctx.Limits, ctx.Options);
        var layout = LayoutReader.Read(nested, map, site.Path);
        var payload = Nested.ExportPayload(container, root, nested, out string compression, out int level);
        ctx.Invalidated.AddRange(nested.Invalidated);
        byte[] image = compression == "cfs" ? Cfs.Wrap(payload, level) : payload;
        if (layout?.Nested is { } retained)
        {
            // The retained bytes are used only when they inflate to the visible map.
            var ok = Cfs.HasMagic(retained.Bytes) && compression == "cfs" && retained.Bytes[4] == level;
            if (ok)
            {
                try
                {
                    var inflated = Cfs.Inflate(retained.Bytes.AsSpan(5), ctx.Limits.DecompressedBytes, m => new InvalidDataException(m));
                    ok = inflated.AsSpan().SequenceEqual(payload);
                }
                catch (InvalidDataException)
                {
                    ok = false;
                }
            }
            if (!ok) throw ctx.Fail(Codes.Layout, "The retained compressed map does not inflate to the visible map.", retained.At, site.Path.Member("$layout"));
            image = retained.Bytes;
        }
        else if (compression == "cfs" && level != 0 && layout is null && map.Find("$layout") is null && ctx.Options.ExactLayout)
            throw ctx.Fail(Codes.Layout, "--exact-layout needs the retained compressed bytes of a campaign map.", map, site.Path);
        writer.Begin(site.Id, site.Property);
        writer.Data.Int32(image.Length);
        writer.End();
        writer.Begin(site.Property.DataId!.Value, site.Property);
        writer.Data.Bytes(image);
        writer.End();
    }
}
