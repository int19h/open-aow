using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;

namespace Aow.Formats.Migrations;

/// <summary>
/// Version 1 stores an enum, a set and a fixed integer array as lowercase hex. Version 2 stores
/// the enum name or its integer, the set as names and the array as a list. The step rewrites
/// those properties and recomputes each $layout fingerprint so it covers the rewritten values.
/// Parts stay. It walks the pinned version 2 schema, so a later document version does not change
/// this step. The walk switches on the codec type.
/// </summary>
public static class SymbolicMigration
{
    public const string Name = "1-symbolic-enum-set-array";

    internal static Migration Step { get; } = new(1, Name, Apply);

    private static readonly Lazy<FormatSchema> version1 = new(() => Load("Aow.Formats.Migrations.schema-v1.json", "schema-v1.json"));
    private static readonly Lazy<FormatSchema> version2 = new(() => Load("Aow.Formats.Migrations.schema-v2.json", "schema-v2.json"));

    /// <summary>The version 1 schema of main a7eb5da, frozen beside the version 2 schema this step walks.</summary>
    internal static FormatSchema Version1 => version1.Value;

    private static FormatSchema Version2 => version2.Value;

    internal static JsonObject Apply(JsonObject document)
    {
        Walk(document, upgrade: true);
        RefreshLayouts(document);
        return document;
    }

    /// <summary>Copies a version 2 document and rewrites the symbolic values back to version 1 hex.</summary>
    internal static JsonObject Downgrade(JsonObject document)
    {
        var copy = (JsonObject)JsonReader.Parse(JsonWriter.ToUtf8(document), "downgrade.json");
        Walk(copy, upgrade: false);
        if (copy.Find("$version") is { } version) version.Value = new JsonInteger(1);
        RefreshLayouts(copy);
        return copy;
    }

    // The fingerprint covers the visible values. A container and a nested campaign map use their
    // own profile. Every other object uses the profile of the document or map that holds it.
    private static void RefreshLayouts(JsonObject document)
    {
        if (document["$container"] is not JsonObject container || container["profile"] is not JsonString profile) return;
        Refresh(document, profile.Value);
    }

    private static void Refresh(JsonValue value, string profile)
    {
        if (value is JsonObject obj)
        {
            string here = ProfileOf(obj, profile);
            if (obj["$layout"] is JsonObject layout && layout.Find("fingerprint") is { } fingerprint)
                fingerprint.Value = new JsonString(LayoutResidue.Fingerprint(obj, here));
            foreach (var member in obj.Members) Refresh(member.Value, here);
        }
        else if (value is JsonArray array)
        {
            foreach (var item in array.Items) Refresh(item, profile);
        }
    }

    private static string ProfileOf(JsonObject obj, string inherited)
    {
        if (obj["profile"] is JsonString direct) return direct.Value;
        if (obj["$container"] is JsonObject box && box["profile"] is JsonString nested) return nested.Value;
        return inherited;
    }

    private static FormatSchema Load(string resource, string file)
    {
        using var stream = typeof(SymbolicMigration).Assembly.GetManifestResourceStream(resource)
            ?? throw new InvalidOperationException("The resource " + resource + " is missing.");
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return FormatSchema.Load(memory.ToArray(), file);
    }

    private static void Walk(JsonObject document, bool upgrade)
    {
        if (document["root"] is not JsonObject root) return;
        if (RootClass(document, root) is not SchemaClass cls) return;
        Visit(root, cls, upgrade, DocumentNodePath.Root);
    }

    private static SchemaClass? RootClass(JsonObject document, JsonObject root)
    {
        var schema = Version2;
        if (root["$class"] is JsonString name) return schema.Find(name.Value);
        if (document["$container"] is JsonObject container && container["profile"] is JsonString profile
            && FileProfile.Find(profile.Value) is { } found)
            return schema.Find(found.RootFamily);
        return null;
    }

    private static void Visit(JsonObject obj, SchemaClass cls, bool upgrade, DocumentNodePath path)
    {
        foreach (var prop in cls.Properties)
        {
            var member = obj.Find(prop.Name);
            if (member is null) continue;
            var at = path.Member(prop.Name);
            if (prop.IsDynamic)
            {
                if (member.Value is JsonArray array)
                    for (int i = 0; i < array.Items.Count; i++) VisitDynamic(array.Items[i], prop, upgrade, at.Item(i));
                continue;
            }
            Convert(member, prop, cls, upgrade, at);
        }
    }

    private static void VisitDynamic(JsonValue item, SchemaProperty prop, bool upgrade, DocumentNodePath path)
    {
        if (item is not JsonObject obj) return;
        if (prop.Codec is FixedObjectCodec fixedObject)
            Visit(obj, Version2.Get(fixedObject.ClassName), upgrade, path);
        else if (prop.Codec is CreatedObjectCodec && obj["$class"] is JsonString name && Version2.Find(name.Value) is { } cls)
            Visit(obj, cls, upgrade, path);
    }

    private static void Convert(JsonMember member, SchemaProperty prop, SchemaClass owner, bool upgrade, DocumentNodePath path)
    {
        var schema = Version2;
        switch (prop.Codec)
        {
            case EnumCodec codec when upgrade:
                member.Value = UpgradeEnum(codec, HexOf(member.Value, prop.Name, path), path);
                break;
            case EnumCodec codec:
                member.Value = DowngradeScalar(codec, member.Value, path);
                break;
            case SetCodec codec when upgrade:
                member.Value = UpgradeSet(codec, HexOf(member.Value, prop.Name, path), path);
                break;
            case SetCodec codec when member.Value is JsonArray:
                member.Value = Hex(codec, codec.Parse(Context(), member.Value, path, prop.Name));
                break;
            case FixedArrayCodec codec when upgrade:
                member.Value = UpgradeArray(codec, HexOf(member.Value, prop.Name, path), path);
                break;
            case FixedArrayCodec codec when member.Value is JsonArray:
                member.Value = DowngradeArray(codec, (JsonArray)member.Value, path);
                break;
            case FixedObjectCodec fixedObject when member.Value is JsonObject nested:
                Visit(nested, schema.Get(fixedObject.ClassName), upgrade, path);
                break;
            case CreatedObjectCodec when member.Value is JsonObject created && created["$class"] is JsonString createdName
                && schema.Find(createdName.Value) is { } createdClass:
                Visit(created, createdClass, upgrade, path);
                break;
            case ChildrenCodec when member.Value is JsonArray children:
                for (int i = 0; i < children.Items.Count; i++)
                    if (children.Items[i] is JsonObject child && child["$class"] is JsonString childName && schema.Find(childName.Value) is { } childClass)
                        Visit(child, childClass, upgrade, path.Item(i));
                break;
            case MapFieldsCodec when member.Value is JsonArray rows:
                var field = schema.Get(owner.FieldClass ?? "TMapField");
                for (int y = 0; y < rows.Items.Count; y++)
                    if (rows.Items[y] is JsonArray cells)
                        for (int x = 0; x < cells.Items.Count; x++)
                            if (cells.Items[x] is JsonObject fieldObject) Visit(fieldObject, field, upgrade, path.Item(y).Item(x));
                break;
            case CampaignMapCodec when member.Value is JsonObject map && map["root"] is JsonObject campaignRoot:
                var campaignClass = campaignRoot["$class"] is JsonString nestedName ? schema.Find(nestedName.Value) : schema.Find("TAoWHSMap");
                if (campaignClass is not null) Visit(campaignRoot, campaignClass, upgrade, path.Member("root"));
                break;
        }
    }

    private static string HexOf(JsonValue value, string name, DocumentNodePath path)
    {
        if (value is JsonString text) return text.Value;
        throw Fail(Codes.Type, name + " is hexadecimal in version 1.", path);
    }

    private static byte[] DecodeHex(string hex, string name, DocumentNodePath path)
    {
        try
        {
            return HexText.Decode(hex);
        }
        catch (FormatException)
        {
            throw Fail(Codes.Type, name + " is hexadecimal in version 1.", path);
        }
    }

    private static JsonValue UpgradeEnum(EnumCodec codec, string hex, DocumentNodePath path)
    {
        var bytes = DecodeHex(hex, codec.TypeName, path);
        if (bytes.Length != codec.Width)
            throw Fail(Codes.Range, "Enum " + codec.TypeName + " holds " + bytes.Length + " hex bytes.", path);
        return codec.MemberText(codec.Read(bytes), null, path.ToString());
    }

    private static JsonValue DowngradeScalar(EnumCodec codec, JsonValue value, DocumentNodePath path)
    {
        long number = codec.Parse(Context(), value, path, codec.TypeName);
        return new JsonString(LayoutResidue.Hex(codec.Raw(number)));
    }

    private static JsonArray UpgradeArray(FixedArrayCodec codec, string hex, DocumentNodePath path)
    {
        var bytes = DecodeHex(hex, "array", path);
        if (bytes.Length != codec.Count * codec.Width)
            throw Fail(Codes.Range, "A fixed array of " + codec.Count + " holds " + bytes.Length + " hex bytes.", path);
        var array = new JsonArray();
        for (int i = 0; i < codec.Count; i++)
            array.Add(new JsonInteger(IntegerBytes.Read(bytes.AsSpan(i * codec.Width, codec.Width), codec.Width, codec.Signed)));
        return array;
    }

    private static JsonString DowngradeArray(FixedArrayCodec codec, JsonArray array, DocumentNodePath path)
    {
        if (array.Items.Count > codec.Count)
            throw Fail(Codes.Range, "A fixed array of " + codec.Count + " holds " + array.Items.Count + " values.", path);
        var data = new ByteBuffer(codec.Count * codec.Width);
        for (int i = 0; i < codec.Count; i++)
        {
            long number = i < array.Items.Count && array.Items[i] is JsonInteger integer ? integer.Value : 0;
            IntegerBytes.Write(data, number, codec.Width);
        }
        return new JsonString(LayoutResidue.Hex(data.ToArray()));
    }

    private static JsonArray UpgradeSet(SetCodec codec, string hex, DocumentNodePath path)
    {
        var bytes = DecodeHex(hex, codec.TypeName, path);
        if (bytes.Length != codec.Width)
            throw Fail(Codes.Range, "Set " + codec.TypeName + " holds " + bytes.Length + " hex bytes.", path);
        return codec.Members(SetCodec.Mask(bytes), null, path.ToString());
    }

    private static JsonString Hex(SetCodec codec, ulong mask)
    {
        var data = new ByteBuffer(codec.Width);
        IntegerBytes.Write(data, (long)mask, codec.Width);
        return new JsonString(LayoutResidue.Hex(data.ToArray()));
    }

    private static ConversionException Fail(string code, string message, DocumentNodePath path) =>
        new(new FormatDiagnostic(code, message) { Path = path.ToString() });

    private static EncodeContext Context() =>
        new(Version2, "game", "migration", null, FormatLimits.Default, ExportOptions.Default);
}
