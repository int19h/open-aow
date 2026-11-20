using Aow.Formats.Json;
using Aow.Formats.Schema;

namespace Aow.Formats.Original;

/// <summary>rwEObject: an object whose class the schema fixes. The document has no $class.</summary>
internal sealed partial class FixedObjectCodec(string className) : PropertyCodec
{
    public string ClassName { get; } = className;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        JsonObject objectValue = new();
        ObjectImage.Decode(context, start, end, classValue: context.Schema.Get(ClassName), site.Path, target: objectValue);
        return objectValue;
    }

    // A dynamic slot stores the same image as a fixed object, with no class id. From version 5 the
    // item states its slot, the stored id less the slot base. Version 4 and older keep no slot and
    // cannot write the member back.
    internal override JsonValue DecodeElement(DecodeContext context, int start, int end, ValueSite site)
    {
        JsonObject objectValue = new();
        if (context.Schema.Version >= SlotVersion && site.Property.SlotBase is int slotBase)
        {
            _ = objectValue.Add("$slot", new JsonInteger(site.Id - slotBase));
        }

        ObjectImage.Decode(context, start, end, classValue: context.Schema.Get(ClassName), site.Path, target: objectValue);
        return objectValue;
    }

    internal override int ElementId(EncodeContext context, JsonValue value, SchemaProperty property, int index, DocumentNodePath path)
    {
        if (context.Schema.Version < SlotVersion || property.SlotBase is not int slotBase)
        {
            throw context.Fail(Codes.Type, message: $"Version {context.Schema.Version} stores no slot of {property.Name}.", at: value, path);
        }

        if (value is not JsonObject objectValue)
        {
            throw context.Fail(Codes.Type, message: $"An item of {property.Name} is an object.", at: value, path, value: value.KindName);
        }

        if (objectValue["$slot"] is not JsonInteger slot)
        {
            throw context.Fail(Codes.MissingMember, message: $"An item of {property.Name} needs an integer $slot.", at: objectValue, path);
        }

        long id = slot.Value + slotBase;
        return slot.Value < 0 || id < property.DynamicMinimum!.Value || id > int.MaxValue
            ? throw context.Fail(Codes.Range, message: "The $slot is outside the stored id range.", at: slot, path: path.Member("$slot"))
            : (int)id;
    }

    internal override void EncodeElement(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
        => ObjectImage.Encode(context, objectValue: (JsonObject)value, classValue: context.Schema.Get(ClassName), output: data, site.Path, "$slot");

    /// <summary>The first version whose items of a fixed dynamic member state their slot.</summary>
    internal const int SlotVersion = 5;

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonObject objectValue)
        {
            throw context.Fail(Codes.Type, message: $"{site.Property.Name} is an object.", at: value, site.Path, value: value.KindName);
        }

        ObjectImage.Encode(context, objectValue, classValue: context.Schema.Get(ClassName), output: data, site.Path);
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

    internal static JsonObject DecodeCreated(DecodeContext context, int start, int end, ValueSite site, string? family, int? slot, out bool isNull)
    {
        context.Need(start, count: 4, end, site.Path, what: "class id");
        int classId = context.Int32(start);
        JsonObject objectValue = new();
        if (classId == 0)
        {
            if (end != start + 4)
            {
                throw context.Fail(Codes.Corrupt, message: "Bytes follow a nil object.", offset: start + 4, site.Path);
            }

            isNull = true;
            _ = objectValue.Add("$class", new JsonNull());
            if (slot is int s0)
            {
                _ = objectValue.Add("$slot", new JsonInteger(s0));
            }

            return objectValue;
        }
        isNull = false;
        SchemaClass classValue = context.Schema.Resolve(context.Engine, classId)
            ?? throw context.Fail(Codes.UnknownClass, message: $"Class id {classId} is not in the {context.Engine} engine table.", offset: start, site.Path, value: classId.ToString(System.Globalization.CultureInfo.InvariantCulture));
        (string Rule, SchemaClass Shape)? shape = site.Property is null ? null : context.Schema.LegacyShape($"{site.Property.Owner.Name}.{site.Property.Name}", className: classValue.Name);
        if (shape is null && family is not null && !classValue.InheritsFrom(family))
        {
            throw context.Fail(Codes.ClassFamily, message: $"Class {classValue.Name} is not a {family}.", offset: start, site.Path, value: classValue.Name, constraint: family);
        }

        _ = objectValue.Add("$class", new JsonString(classValue.Name));
        if (slot is int s)
        {
            _ = objectValue.Add("$slot", new JsonInteger(s));
        }

        if (shape is { } legacy)
        {
            context.LegacyRules[legacy.Rule] = context.LegacyRules.GetValueOrDefault(legacy.Rule) + 1;
            ObjectImage.Decode(context, start: start + 4, end, classValue: legacy.Shape, site.Path, target: objectValue);
        }
        else
        {
            ObjectImage.Decode(context, start: start + 4, end, classValue, site.Path, target: objectValue);
        }

        return objectValue;
    }

    internal static void EncodeCreated(EncodeContext context, JsonObject objectValue, ByteBuffer data, ValueSite site, string? family, bool slotted)
    {
        JsonMember classMember = objectValue.Find("$class") ?? throw context.Fail(Codes.MissingMember, message: "A created object needs $class.", at: objectValue, site.Path);
        string[] reserved = slotted ? ["$class", "$slot"] : ["$class"];
        if (classMember.Value is JsonNull)
        {
            foreach (JsonMember m in objectValue.Members)
            {
                if (Array.IndexOf(reserved, value: m.Name) < 0)
                {
                    throw context.Fail(Codes.UnknownMember, message: "A nil object has no members.", at: m.Value, path: site.Path.Member(m.Name));
                }
            }

            data.Int32(0);
            return;
        }
        if (classMember.Value is not JsonString name)
        {
            throw context.Fail(Codes.Type, message: "$class is a class name or null.", at: classMember.Value, path: site.Path.Member("$class"));
        }

        SchemaClass classValue = context.Schema.Find(name.Value)
            ?? throw context.Fail(Codes.UnknownClass, message: $"Unknown class {name.Value}.", at: name, path: site.Path.Member("$class"), name.Value);
        if (classValue.Id is not int id || context.Schema.Resolve(context.Engine, classId: id) != classValue)
        {
            throw context.Fail(Codes.UnknownClass, message: $"Class {classValue.Name} is not in the {context.Engine} engine table.", at: name, path: site.Path.Member("$class"), value: classValue.Name);
        }

        (string Rule, SchemaClass Shape)? shape = site.Property is null ? null : context.Schema.LegacyShape($"{site.Property.Owner.Name}.{site.Property.Name}", className: classValue.Name);
        if (shape is null && family is not null && !classValue.InheritsFrom(family))
        {
            throw context.Fail(Codes.ClassFamily, message: $"Class {classValue.Name} is not a {family}.", at: name, path: site.Path.Member("$class"), value: classValue.Name, constraint: family);
        }

        data.Int32(id);
        ObjectImage.Encode(context, objectValue, classValue: shape?.Shape ?? classValue, output: data, site.Path, reserved);
    }

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        JsonObject objectValue = DecodeCreated(context, start, end, site, family, slot: null, out bool isNull);
        return isNull ? new JsonNull() : objectValue;
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is JsonNull)
        {
            data.Int32(0);
            return;
        }
        if (value is not JsonObject objectValue)
        {
            throw context.Fail(Codes.Type, message: $"{site.Property.Name} is an object or null.", at: value, site.Path, value: value.KindName);
        }

        if (objectValue["$class"] is JsonNull)
        {
            throw context.Fail(Codes.Type, message: "A stored nil object is null, not $class null.", at: objectValue, site.Path);
        }

        EncodeCreated(context, objectValue, data, site, family, slotted: false);
    }

    internal override JsonValue DecodeElement(DecodeContext context, int start, int end, ValueSite site)
    {
        int? slot = site.Property.SlotBase is int b ? site.Id - b : null;
        return DecodeCreated(context, start, end, site, family, slot, isNull: out _);
    }

    internal override int ElementId(EncodeContext context, JsonValue value, SchemaProperty property, int index, DocumentNodePath path)
    {
        if (value is not JsonObject objectValue)
        {
            throw context.Fail(Codes.Type, message: $"An item of {property.Name} is an object.", at: value, path, value: value.KindName);
        }

        if (property.SlotBase is not int b)
        {
            return objectValue.Find("$slot") is { } extra
                ? throw context.Fail(Codes.ReservedMember, message: $"Items of {property.Name} have no $slot.", at: extra.Value, path: path.Member("$slot"))
                : property.DynamicMinimum!.Value + index;
        }
        if (objectValue["$slot"] is not JsonInteger slot)
        {
            throw context.Fail(Codes.MissingMember, message: $"An item of {property.Name} needs an integer $slot.", at: objectValue, path);
        }

        long id = slot.Value + b;
        return slot.Value < 0 || id < property.DynamicMinimum!.Value || id > int.MaxValue
            ? throw context.Fail(Codes.Range, message: "The $slot is outside the stored id range.", at: slot, path: path.Member("$slot"))
            : (int)id;
    }

    internal override void EncodeElement(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
        => EncodeCreated(context, objectValue: (JsonObject)value, data, site, family, slotted: site.Property.SlotBase is not null);
}

/// <summary>
/// rwChildren: a nested table whose entry ids are the children's slots, each a created object.
/// The document lists them in stored order with an explicit $slot.
/// </summary>
internal sealed partial class ChildrenCodec : PropertyCodec
{
    internal override bool AcceptsDocumentArray(int version) => true;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        PropertyTable table = PropertyTable.Read(context, start, end, site.Path);
        int db = table.DataBase;
        int length = end - db;
        int[] boundaries = [.. table.Entries.Select(e => e.Offset).Append(length).Distinct().Order()];
        List<TableEntry> order = [.. table.Entries.OrderBy(e => e.Offset).ThenBy(e => e.Index)];
        HashSet<int> slots = [];
        JsonArray array = new();
        int k = 0;
        for (int i = 0; i < order.Count; i++)
        {
            TableEntry e = order[i];
            if (i + 1 < order.Count && order[i + 1].Offset == e.Offset)
            {
                throw context.Fail(Codes.Layout, message: "Two children share one offset.", offset: db + e.Offset, site.Path);
            }

            if (!slots.Add(e.Id))
            {
                throw context.Fail(Codes.Duplicate, message: $"Child slot {e.Id} is stored twice.", offset: db + e.Offset, site.Path);
            }

            int next = boundaries[Array.BinarySearch(boundaries, value: e.Offset) + 1];
            _ = array.Add(CreatedObjectCodec.DecodeCreated(site: new ValueSite { Property = site.Property, Id = e.Id, Path = site.Path.Item(k++), Owner = site.Owner }, context: context, start: db + e.Offset, end: db + next, family: null, slot: e.Id, isNull: out _));
        }

        context.CheckItems(array.Items.Count, offset: start, site.Path);
        return array;
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        if (value is not JsonArray array)
        {
            throw context.Fail(Codes.Type, message: $"{site.Property.Name} is an array.", at: value, site.Path, value: value.KindName);
        }

        if (array.Items.Count > context.Limits.CollectionItems)
        {
            throw context.Fail(Codes.Limit, message: "A collection has too many items.", at: array, site.Path, value: null, constraint: $"at most {context.Limits.CollectionItems} items");
        }

        ObjectWriter writer = new();
        Dictionary<int, JsonValue> slots = [];
        for (int k = 0; k < array.Items.Count; k++)
        {
            DocumentNodePath itemPath = site.Path.Item(k);
            if (array.Items[k] is not JsonObject child)
            {
                throw context.Fail(Codes.Type, message: "A child is an object.", at: array.Items[k], path: itemPath);
            }

            if (child["$slot"] is not JsonInteger { Value: >= 0 and <= int.MaxValue } slot)
            {
                throw context.Fail(Codes.MissingMember, message: "A child needs a non-negative integer $slot.", at: child, path: itemPath);
            }

            if (!slots.TryAdd((int)slot.Value, slot))
            {
                throw context.Fail(Codes.Slot, message: $"Child slot {slot.Value} is declared twice, first at {slots[(int)slot.Value].Span.Line}:{slots[(int)slot.Value].Span.Column}.", at: slot, path: itemPath.Member("$slot"));
            }

            if (child["$class"] is JsonNull)
            {
                throw context.Fail(Codes.Type, message: "A stored child is never nil.", at: child, path: itemPath);
            }

            ValueSite childSite = new() { Property = site.Property, Id = (int)slot.Value, Path = itemPath, Owner = site.Owner };
            writer.Begin((int)slot.Value, prop: site.Property, elementIndex: k);
            CreatedObjectCodec.EncodeCreated(context, objectValue: child, writer.Data, site: childSite, family: null, slotted: true);
            writer.End();
        }
        ObjectImage.Place(context, writer: writer, path: site.Path, output: data);
    }
}

/// <summary>
/// TMapLevel property 6: Width times Height field images, each a 32-bit length and a field of
/// the level's field class, in engine y then x order. The document holds Fields[y][x], one row
/// per line.
/// </summary>
internal sealed partial class MapFieldsCodec : PropertyCodec
{
    internal override bool AcceptsDocumentArray(int version) => true;

    private static int Dimension(JsonObject owner, string name) => owner[name] is JsonInteger n ? (int)n.Value : 0;

    /// <summary>The level's class, from its $class, decides the field class (MapFieldClass).</summary>
    private static SchemaClass FieldClass(FormatSchema schema, ValueSite site)
    {
        SchemaClass? level = site.Owner["$class"] is JsonString c ? schema.Find(c.Value) : null;
        return schema.Get((level ?? site.Property.Owner).FieldClass ?? "TMapField");
    }

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
    {
        int width = Dimension(site.Owner, name: "Width"), height = Dimension(site.Owner, name: "Height");
        long count = (long)width * height;
        if (count > context.Limits.CollectionItems)
        {
            throw context.Fail(Codes.Limit, message: "The map has too many fields.", offset: start, site.Path, value: count.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        SchemaClass fieldClass = FieldClass(context.Schema, site);
        JsonArray rows = new() { Layout = ArrayLayout.RowsOnLines };
        int at = start;
        for (int y = 0; y < height; y++)
        {
            JsonArray row = new();
            for (int x = 0; x < width; x++)
            {
                if (at + 4 > end)
                {
                    throw context.Fail(Codes.Dimensions, message: $"The field stream ends before field [{y}][{x}] of a {width} by {height} level.", offset: at, site.Path);
                }

                int length = context.Int32(at);
                if (length <= 0 || at + 4L + length > end)
                {
                    throw context.Fail(Codes.Corrupt, message: $"Field [{y}][{x}] has an invalid length {length}.", offset: at, path: site.Path.Item(y).Item(x));
                }

                JsonObject field = new();
                ObjectImage.Decode(context, start: at + 4, end: at + 4 + length, classValue: fieldClass, path: site.Path.Item(y).Item(x), target: field);
                _ = row.Add(field);
                at += 4 + length;
            }
            _ = rows.Add(row);
        }
        return at != end
            ? throw context.Fail(Codes.Dimensions, message: $"The field stream holds more than {width} by {height} fields.", offset: at, site.Path)
            : (JsonValue)rows;
    }

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
    {
        int width = Dimension(site.Owner, name: "Width"), height = Dimension(site.Owner, name: "Height");
        if (value is not JsonArray rows)
        {
            throw context.Fail(Codes.Type, message: "Fields is an array of rows.", at: value, site.Path, value: value.KindName);
        }

        if (rows.Items.Count != height)
        {
            throw context.Fail(Codes.Dimensions, message: $"Fields has {rows.Items.Count} rows and Height is {height}.", at: rows, site.Path, value: rows.Items.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), constraint: $"{height} rows");
        }

        SchemaClass fieldClass = FieldClass(context.Schema, site);
        for (int y = 0; y < rows.Items.Count; y++)
        {
            if (rows.Items[y] is not JsonArray row)
            {
                throw context.Fail(Codes.Type, message: "A row is an array of fields.", at: rows.Items[y], path: site.Path.Item(y));
            }

            if (row.Items.Count != width)
            {
                throw context.Fail(Codes.Dimensions, message: $"Row {y} has {row.Items.Count} fields and Width is {width}.", at: row, path: site.Path.Item(y), value: row.Items.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), constraint: $"{width} fields");
            }

            for (int x = 0; x < row.Items.Count; x++)
            {
                if (row.Items[x] is not JsonObject field)
                {
                    throw context.Fail(Codes.Type, message: "A field is an object.", at: row.Items[x], path: site.Path.Item(y).Item(x));
                }

                int at = data.Length;
                data.Int32(0);
                ObjectImage.Encode(context, objectValue: field, classValue: fieldClass, output: data, path: site.Path.Item(y).Item(x));
                data.PatchInt32(at, value: data.Length - at - 4);
            }
        }
    }
}

/// <summary>
/// TAoWCampaign ids 5 and 6: the size and the bytes of a campaign map, an HSM file image as
/// SaveHSMtoStream writes it. The document holds Map, a nested document with its own
/// $container. Export compresses the payload again at the container's level.
/// </summary>
internal sealed partial class CampaignMapCodec : PropertyCodec
{
    private static HsmProfile Nested => (HsmProfile)FileProfile.Find("world-hsm")!;

    internal override JsonValue Decode(DecodeContext context, int start, int end, ValueSite site)
        => throw new InvalidOperationException("The campaign map stores two entries.");

    internal override void Encode(EncodeContext context, JsonValue value, ByteBuffer data, ValueSite site)
        => throw new InvalidOperationException("The campaign map stores two entries.");

    internal override JsonValue DecodePair(DecodeContext context, (int Start, int End)? first, (int Start, int End)? second, ValueSite site)
    {
        if (first is not { } size || second is not { } bytes)
        {
            throw context.Fail(Codes.Corrupt, message: "A campaign map needs both its size (id 5) and its bytes (id 6).", offset: (first ?? second)!.Value.Start, site.Path);
        }

        if (size.End - size.Start != 4)
        {
            throw context.Fail(Codes.Corrupt, message: "The campaign map size is not a 32-bit integer.", offset: size.Start, site.Path);
        }

        int length = context.Int32(size.Start);
        if (length <= 0 || length != bytes.End - bytes.Start)
        {
            throw context.Fail(Codes.Corrupt, message: $"The campaign map size {length} differs from its {bytes.End - bytes.Start} stored bytes.", offset: size.Start, site.Path);
        }

        byte[] image = context.Data.AsSpan(bytes.Start, length).ToArray();
        ImportSession session = new()
        {
            Schema = context.Schema,
            Limits = context.Limits,
            LegacyRules = context.LegacyRules,
            File = context.File,
            Nested = site.Path.ToString(),
        };
        (JsonObject? container, JsonObject? root) = Nested.Import(image, session);
        return new JsonObject().Add("$container", container).Add("root", root);
    }

    internal override void EncodeMember(EncodeContext context, JsonValue value, ObjectWriter writer, ValueSite site)
    {
        if (value is not JsonObject map)
        {
            throw context.Fail(Codes.Type, message: "Map is a nested document.", at: value, site.Path, value: value.KindName);
        }

        foreach (JsonMember m in map.Members)
        {
            if (m.Name is not ("$container" or "root"))
            {
                throw context.Fail(m.Name.StartsWith('$') ? Codes.ReservedMember : Codes.UnknownMember, message: $"Map has no member \"{m.Name}\".", at: m.Value, path: site.Path.Member(m.Name));
            }
        }

        JsonObject container = map["$container"] as JsonObject ?? throw context.Fail(Codes.MissingMember, message: "Map needs $container.", at: map, site.Path);
        JsonValue root = map["root"] ?? throw context.Fail(Codes.MissingMember, message: "Map needs root.", at: map, site.Path);
        if (container["profile"] is not JsonString { Value: "world-hsm" })
        {
            throw context.Fail(Codes.Profile, message: "A campaign map has profile world-hsm.", at: container["profile"] ?? container, path: site.Path.Member("$container").Member("profile"));
        }

        EncodeContext nested = new(context.Schema, Nested.Engine, profile: Nested.Name, context.File, context.Limits);
        byte[] payload = Nested.ExportPayload(container, root, context: nested, out string compression, out int level);
        byte[] image = compression == "cfs" ? Cfs.Wrap(payload, level) : payload;
        writer.Begin(site.Id, prop: site.Property);
        writer.Data.Int32(image.Length);
        writer.End();
        writer.Begin(site.Property.DataId!.Value, prop: site.Property);
        writer.Data.Bytes(image);
        writer.End();
    }
}
