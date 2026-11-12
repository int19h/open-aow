using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;

namespace Aow.Formats.Migrations;

/// <summary>
/// Version 4 stores an image sequence list, a terrain list, an ILB library and the flood's terrain
/// grid as lowercase hex, the single of the AI as <c>{ "$bits": ... }</c>, and the items of a fixed
/// dynamic member without their slot. Version 5 stores the sequences as layers, the terrain list
/// as pairs, the library as base64, the changed hexagons of the grid, the single as a number and
/// each fixed item with its <c>$slot</c> (own-formats.md 2.3). The step rewrites the nine pinned
/// properties and recomputes each $layout fingerprint. A sequence list whose nil or has-next bytes
/// the writer does not make gains a value-encoding part, so the original bytes stay exact.
/// Version 4 cannot tell the slot of a fixed item, so the step refuses a document that holds one
/// without <c>$slot</c>. The version 4 export refused such a document too.
/// It walks the pinned version 4 schema. Each pinned property encodes its version 4 value through
/// the version 4 codec, so a hex payload passes through as its bytes, and decodes those bytes
/// through an embedded version 5 codec spec. The codec specs are pinned. The codec implementations
/// are live code. Enum names come from the enum catalog, as the earlier steps do. The walk
/// switches on the codec type.
/// </summary>
internal static class ResourceMigration
{
    public const string Name = "4-resources";

    internal static Migration Step { get; } = new(4, Name, Apply);

    private static readonly Lazy<FormatSchema> s_version4 = new(() => Load("Aow.Formats.Migrations.schema-v4.json", "schema-v4.json"));

    /// <summary>The version 4 schema this step walks. A version 4 import uses it.</summary>
    internal static FormatSchema Version4 => s_version4.Value;

    private static readonly Dictionary<string, PropertyCodec> s_targets = BuildTargets();

    /// <summary>The declaring class and property of each pinned property, as the version 4 schema names them.</summary>
    internal static IReadOnlyCollection<string> Pinned => s_targets.Keys;

    /// <summary>The fixed dynamic members whose items gain <c>$slot</c>, as the version 4 schema names them.</summary>
    internal static IReadOnlyList<string> Slotted { get; } = ["AbilityControl.Abilities", "SpellControl.Spells"];

    internal static JsonObject Apply(JsonObject document)
    {
        if (document["root"] is JsonObject root && RootClass(document, root) is SchemaClass classValue)
            Visit(root, classValue, DocumentNodePath.Root);
        RecordMigration.RefreshFingerprints(document);
        return document;
    }

    private static FormatSchema Load(string resource, string file)
    {
        using var stream = typeof(ResourceMigration).Assembly.GetManifestResourceStream(resource)
            ?? throw new InvalidOperationException("The resource " + resource + " is missing.");
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return FormatSchema.Load(memory.ToArray(), file);
    }

    private static SchemaClass? RootClass(JsonObject document, JsonObject root)
    {
        if (root["$class"] is JsonString name) return Version4.Find(name.Value);
        if (document["$container"] is JsonObject container && container["profile"] is JsonString profile
            && FileProfile.Find(profile.Value) is { } found)
            return Version4.Find(found.RootFamily);
        return null;
    }

    private static void Visit(JsonObject objectValue, SchemaClass classValue, DocumentNodePath path)
    {
        foreach (var prop in classValue.Properties)
        {
            var member = Member(objectValue, prop);
            if (member is null) continue;
            var at = path.Member(member.Name);
            if (prop.IsDynamic)
            {
                if (member.Value is not JsonArray array) continue;
                bool slotted = prop.Codec is FixedObjectCodec;
                for (int i = 0; i < array.Items.Count; i++)
                {
                    if (slotted && array.Items[i] is JsonObject item && item.Find("$slot") is null)
                        throw new ConversionException(new FormatDiagnostic(Codes.Slot,
                            "Version 4 stores no slot for the items of " + prop.Name + ". Import the original file again.")
                        { Path = at.Item(i).ToString(), Migration = Name });
                    VisitDynamic(array.Items[i], prop, at.Item(i));
                }
                continue;
            }
            Convert(member, prop, objectValue, classValue, at);
        }
    }

    // A document from before a property rename stores the old name. The rename pass runs after the steps.
    private static JsonMember? Member(JsonObject objectValue, SchemaProperty prop)
    {
        var member = objectValue.Find(prop.Name);
        if (member is not null) return member;
        foreach (string old in prop.OldNames)
            if (objectValue.Find(old) is { } renamed) return renamed;
        return null;
    }

    private static void VisitDynamic(JsonValue item, SchemaProperty prop, DocumentNodePath path)
    {
        if (item is not JsonObject objectValue) return;
        if (prop.Codec is FixedObjectCodec fixedObject)
            Visit(objectValue, Version4.Get(fixedObject.ClassName), path);
        else if (prop.Codec is CreatedObjectCodec && objectValue["$class"] is JsonString name && Version4.Find(name.Value) is { } classValue)
            Visit(objectValue, classValue, path);
    }

    private static void Convert(JsonMember member, SchemaProperty prop, JsonObject owner, SchemaClass ownerClass, DocumentNodePath path)
    {
        if (s_targets.TryGetValue(prop.Owner.Name + "." + prop.Name, out var target))
        {
            member.Value = Rewrite(member.Value, prop, target, owner, path);
            return;
        }
        var schema = Version4;
        switch (prop.Codec)
        {
            case FixedObjectCodec fixedObject when member.Value is JsonObject nested:
                Visit(nested, schema.Get(fixedObject.ClassName), path);
                break;
            case CreatedObjectCodec when member.Value is JsonObject created && created["$class"] is JsonString createdName
                && schema.Find(createdName.Value) is { } createdClass:
                Visit(created, createdClass, path);
                break;
            case ChildrenCodec when member.Value is JsonArray children:
                for (int i = 0; i < children.Items.Count; i++)
                    if (children.Items[i] is JsonObject child && child["$class"] is JsonString childName && schema.Find(childName.Value) is { } childClass)
                        Visit(child, childClass, path.Item(i));
                break;
            case MapFieldsCodec when member.Value is JsonArray rows:
                var field = schema.Get(ownerClass.FieldClass ?? "TMapField");
                for (int y = 0; y < rows.Items.Count; y++)
                    if (rows.Items[y] is JsonArray cells)
                        for (int x = 0; x < cells.Items.Count; x++)
                            if (cells.Items[x] is JsonObject fieldObject) Visit(fieldObject, field, path.Item(y).Item(x));
                break;
            case CampaignMapCodec when member.Value is JsonObject map && map["root"] is JsonObject campaignRoot:
                var campaignClass = campaignRoot["$class"] is JsonString nestedName ? schema.Find(nestedName.Value) : schema.Find("TAoWHSMap");
                if (campaignClass is not null) Visit(campaignRoot, campaignClass, path.Member("root"));
                break;
        }
    }

    // The version 4 codec writes the stored bytes, so a hex payload passes through as it is.
    private static JsonValue Rewrite(JsonValue value, SchemaProperty prop, PropertyCodec target, JsonObject owner, DocumentNodePath path)
    {
        var encoding = new EncodeContext(Version4, "game", "migration", null, FormatLimits.Default, ExportOptions.Default);
        var data = new ByteBuffer();
        prop.Codec.Encode(encoding, value, data, new ValueSite { Property = prop, Id = prop.Id ?? 0, Path = path, Owner = owner });
        var bytes = data.ToArray();
        var builder = new LayoutBuilder();
        var decoding = new DecodeContext(bytes, Version4, "game", "migration", null, FormatLimits.Default, new LayoutCounts());
        var site = new ValueSite { Property = prop, Id = prop.Id ?? 0, Path = path, Owner = owner, Builder = builder };
        var result = target.Decode(decoding, 0, bytes.Length, site);
        if (builder.Parts.Count != 0) AddParts(owner, builder.Parts);
        return result;
    }

    // The fingerprint is recomputed after the walk.
    private static void AddParts(JsonObject owner, List<JsonObject> parts)
    {
        if (owner["$layout"] is JsonObject layout && layout["parts"] is JsonArray existing)
        {
            foreach (var part in parts) existing.Add(part);
            return;
        }
        var array = new JsonArray();
        foreach (var part in parts) array.Add(part);
        var created = new JsonObject()
            .Add("codec", new JsonString(LayoutResidue.Codec))
            .Add("fingerprint", new JsonString(""))
            .Add("parts", array);
        int at = 0;
        while (at < owner.Members.Count && owner.Members[at].Name.StartsWith('$')) at++;
        owner.Members.Insert(at, new JsonMember("$layout", created));
    }

    private static Dictionary<string, PropertyCodec> BuildTargets()
    {
        var sequences = new ImageSequencesCodec();
        return new Dictionary<string, PropertyCodec>(StringComparer.Ordinal)
        {
            ["Ability.ImageSequences"] = sequences,
            ["GamePack.ImageSequences"] = sequences,
            ["MapObjectResource.ImageSequenceList"] = sequences,
            ["Spell.CastImages"] = sequences,
            ["UnitGraphicsResource.Sequences"] = sequences,
            ["MapObjectResource.TerrainList"] = new TerrainListCodec(),
            ["MapObjectResource.ImageLibrary"] = new ImageLibraryCodec(),
            ["FloodControl.OriginalTerrain"] = new TerrainGridCodec(widthName: "Width", heightName: "Height"),
            ["AIPlayerControl.DefenseStrengthMultiplier"] = new SingleCodec(),
        };
    }
}
