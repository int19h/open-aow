using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Migrations;

/// <summary>
/// Version 3 stores a unit index list, a unit index list collection, a diplomatic relation list
/// and a move path as lowercase hex, a bit list with hex bits, and the sphere and relation byte
/// lists as integers. Version 4 stores the lists with a stored default, the groups of a
/// collection, the set bit indices of a bit list and enum names (own-formats.md 2.3). The step
/// rewrites those properties and recomputes each $layout fingerprint. A list payload whose runs
/// the writer does not make gains a value-encoding part, so the original bytes stay exact.
/// It walks the pinned version 3 schema. Each pinned property encodes its version 3 value through
/// the version 3 codec and decodes the bytes through an embedded version 4 codec spec. The map
/// coordinate layout is an embedded field list. The codec specs and the layout are pinned. The
/// codec implementations are live code. Enum names come from the enum catalog, as the earlier
/// steps do. The walk switches on the codec type.
/// </summary>
internal static class ListMigration
{
    public const string Name = "3-lists";

    internal static Migration Step { get; } = new(3, Name, Apply);

    private static readonly Lazy<FormatSchema> s_version3 = new(() => Load("Aow.Formats.Migrations.schema-v3.json", "schema-v3.json"));

    /// <summary>The version 3 schema this step walks. A version 3 import uses it.</summary>
    internal static FormatSchema Version3 => s_version3.Value;

    private static readonly Dictionary<string, PropertyCodec> s_targets = BuildTargets();

    /// <summary>The declaring class and property of each pinned property, as the version 3 schema names them.</summary>
    internal static IReadOnlyCollection<string> Pinned => s_targets.Keys;

    internal static JsonObject Apply(JsonObject document)
    {
        if (document["root"] is JsonObject root && RootClass(document, root) is SchemaClass classValue)
            Visit(root, classValue, DocumentNodePath.Root);
        RecordMigration.RefreshFingerprints(document);
        return document;
    }

    private static FormatSchema Load(string resource, string file)
    {
        using var stream = typeof(ListMigration).Assembly.GetManifestResourceStream(resource)
            ?? throw new InvalidOperationException("The resource " + resource + " is missing.");
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return FormatSchema.Load(memory.ToArray(), file);
    }

    private static SchemaClass? RootClass(JsonObject document, JsonObject root)
    {
        if (root["$class"] is JsonString name) return Version3.Find(name.Value);
        if (document["$container"] is JsonObject container && container["profile"] is JsonString profile
            && FileProfile.Find(profile.Value) is { } found)
            return Version3.Find(found.RootFamily);
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
                if (member.Value is JsonArray array)
                    for (int i = 0; i < array.Items.Count; i++) VisitDynamic(array.Items[i], prop, at.Item(i));
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
            Visit(objectValue, Version3.Get(fixedObject.ClassName), path);
        else if (prop.Codec is CreatedObjectCodec && objectValue["$class"] is JsonString name && Version3.Find(name.Value) is { } classValue)
            Visit(objectValue, classValue, path);
    }

    private static void Convert(JsonMember member, SchemaProperty prop, JsonObject owner, SchemaClass ownerClass, DocumentNodePath path)
    {
        if (s_targets.TryGetValue(prop.Owner.Name + "." + prop.Name, out var target))
        {
            member.Value = Rewrite(member.Value, prop, target, owner, path);
            return;
        }
        var schema = Version3;
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

    // The version 3 codec writes the stored bytes, so a hex payload passes through as it is.
    private static JsonValue Rewrite(JsonValue value, SchemaProperty prop, PropertyCodec target, JsonObject owner, DocumentNodePath path)
    {
        var encoding = new EncodeContext(Version3, "game", "migration", null, FormatLimits.Default, ExportOptions.Default);
        var data = new ByteBuffer();
        prop.Codec.Encode(encoding, value, data, new ValueSite { Property = prop, Id = prop.Id ?? 0, Path = path, Owner = owner });
        var bytes = data.ToArray();
        var builder = new LayoutBuilder();
        var decoding = new DecodeContext(bytes, Version3, "game", "migration", null, FormatLimits.Default, new LayoutCounts());
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
        var unitIndexes = new RunListCodec(4);
        var groups = new GroupedListCodec(8);
        var relations = new RunListCodec(1, new EnumListElement("DiplomaticRelation", 1));
        var bits = new BitListCodec(indices: true);
        return new Dictionary<string, PropertyCodec>(StringComparer.Ordinal)
        {
            ["ProductionPlaceResource.Units"] = unitIndexes,
            ["RaceResource.UnitIndexes"] = unitIndexes,
            ["RaceResource.SpecialUnitIndexes"] = unitIndexes,
            ["SummonSpellTokenEvent.UnitIndices"] = unitIndexes,
            ["StructureResource.Defenders"] = groups,
            ["ExplorationSiteResource.DefenderCollections"] = groups,
            ["Dungeon.Resource.PrisonerCollections"] = groups,
            ["Player.LegacyNewRelations"] = relations,
            ["Player.LegacyRelations"] = relations,
            ["MoveTokenEvent.Path"] = new RunListCodec(4, new RecordListElement(MapCoordinateLayout())),
            ["Player.AIValidAttackPlayers"] = bits,
            ["PlayerStructure.VictoryLocations"] = bits,
            ["Sign.TextSeen"] = bits,
            ["Leader.Spheres"] = new RunListCodec(1, new EnumListElement("MagicSphere", 1)),
            ["PlayerDiplomaticRelations.Relations"] = relations,
            ["Race.DefaultRelations"] = new RunListCodec(1, new EnumListElement("Status", 1)),
        };
    }

    private static RecordTypeInfo MapCoordinateLayout() => new()
    {
        Name = "MapCoordinate",
        Size = 4,
        Fields =
        [
            new IntegerRecordField { Name = "X", Offset = 0, Size = 1, ElementWidth = 1, ElementSigned = true },
            new IntegerRecordField { Name = "Y", Offset = 1, Size = 1, ElementWidth = 1, ElementSigned = true },
            new IntegerRecordField { Name = "MapLevel", Offset = 2, Size = 1, ElementWidth = 1, ElementSigned = true },
            new IntegerRecordField { Name = "Tag", Offset = 3, Size = 1, ElementWidth = 1, ElementSigned = true },
        ],
        Residue = [],
    };
}
