using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Migrations;

/// <summary>
/// Version 2 stores a padded record, the ability bit array and a raw primitive as lowercase hex.
/// Version 3 stores the record fields, the set bit indices and the integer or boolean.
/// The step rewrites those properties and recomputes each $layout fingerprint so it covers the
/// rewritten values. Parts stay. It walks the pinned version 2 schema. Each pinned property
/// decodes through an embedded version 3 codec spec, and the three record layouts are embedded
/// field lists. The codec specs and the record layouts are pinned. The codec implementations are
/// live code. A later schema reannotation or a C# field rename does not change this step. A change
/// to a live codec implementation does.
/// Enum and set names still come from the enum catalog, as the version 1 step does.
/// The walk switches on the codec type.
/// </summary>
internal static class RecordMigration
{
    public const string Name = "2-record-bit-primitive";

    internal static Migration Step { get; } = new(2, Name, Apply);

    private static readonly Lazy<FormatSchema> s_version2 = new(() => Load("Aow.Formats.Migrations.schema-v2.json", "schema-v2.json"));

    /// <summary>The version 2 schema this step walks. A version 2 import uses it.</summary>
    internal static FormatSchema Version2 => s_version2.Value;

    // Declaring class and property. An inherited property keeps the class that declared it.
    private static readonly HashSet<string> s_pinned =
    [
        "TAIGroup.IndependentRelation",
        "TAIGroup.TargetMode",
        "TAnimation.Dimensions",
        "TAoWHSMap.VictoryMode",
        "TArmyInfoEventLog.Selection",
        "TCity.WallType",
        "TCombatData._CombatData_30",
        "TCombatData.WallType",
        "TCombatObject.ID",
        "TCombatObjectTurnDoneCA._CombatObjectTurnDoneCA_0C",
        "TCombatParty.Wall",
        "TCombatPlayer.SideIndex",
        "TCombatWall.WallType",
        "TCustomAbilityList.AbilityCount",
        "TDamageCA.Effects",
        "TDamageCA.DamageTypes",
        "TFleeCA.ObjectID",
        "TItemExplorationSite.ItemCount",
        "TItemTE.Action",
        "TManaCrystals.Size",
        "TMine.State",
        "TMoveWallCA._MoveWallCA_0C",
        "TMoveWallCA._MoveWallCA_0D",
        "TPlayerSetupSettings.Behavior",
        "TPlayerSetupSettings.BehaviorMode",
        "TPlayerSetupSettings.OriginalBehavior",
        "TProductionPlace.DefenderLevel",
        "TProductionPlace.RazedState",
        "TRangedAttackCA.AbilityID",
        "TRemotePlayer.RemoteID",
        "TSetupSettings.PlayMode",
        "TSetupSettings.VictoryCondition",
        "TSetupSettings.IndependentAIMode",
        "TSetupSettings.ConnectionType",
        "TSetupSettings.TacticalCombatAvailability",
        "TUnitGFXResource._UnitGFXResource_30",
        "TUnitResource.Champion",
        "TWMCampaignScenario.MapKind",
        "TWizardsTower.SpellID",
    ];

    internal static JsonObject Apply(JsonObject document)
    {
        Walk(document);
        RefreshLayouts(document);
        return document;
    }

    // The class-name step rewrites $class after this migration, so the chain asks for a second pass.
    internal static void RefreshFingerprints(JsonObject document) => RefreshLayouts(document);

    /// <summary>
    /// Runs an edit of member names and recomputes only the fingerprints that held before it, so a
    /// stale fingerprint stays stale.
    /// </summary>
    internal static void KeepFingerprints(JsonObject document, Func<bool> edit)
    {
        if (document["$container"] is not JsonObject container || container["profile"] is not JsonString profile)
        {
            edit();
            return;
        }
        var valid = new HashSet<JsonObject>(ReferenceEqualityComparer.Instance);
        Collect(document, profile.Value, valid);
        if (!edit()) return;
        Refresh(document, profile.Value, valid);
    }

    private static void Collect(JsonValue value, string profile, HashSet<JsonObject> valid)
    {
        if (value is JsonObject objectValue)
        {
            string here = ProfileOf(objectValue, profile);
            if (objectValue["$layout"] is JsonObject layout && layout["fingerprint"] is JsonString stored
                && stored.Value == LayoutResidue.Fingerprint(objectValue, here))
                valid.Add(objectValue);
            foreach (var member in objectValue.Members) Collect(member.Value, here, valid);
        }
        else if (value is JsonArray array)
        {
            foreach (var item in array.Items) Collect(item, profile, valid);
        }
    }

    // The fingerprint covers the visible values. A container and a nested campaign map use their
    // own profile. Every other object uses the profile of the document or map that holds it.
    private static void RefreshLayouts(JsonObject document)
    {
        if (document["$container"] is not JsonObject container || container["profile"] is not JsonString profile) return;
        Refresh(document, profile.Value);
    }

    private static void Refresh(JsonValue value, string profile, HashSet<JsonObject>? only = null)
    {
        if (value is JsonObject objectValue)
        {
            string here = ProfileOf(objectValue, profile);
            if (objectValue["$layout"] is JsonObject layout && layout.Find("fingerprint") is { } fingerprint
                && (only is null || only.Contains(objectValue)))
                fingerprint.Value = new JsonString(LayoutResidue.Fingerprint(objectValue, here));
            foreach (var member in objectValue.Members) Refresh(member.Value, here, only);
        }
        else if (value is JsonArray array)
        {
            foreach (var item in array.Items) Refresh(item, profile, only);
        }
    }

    private static string ProfileOf(JsonObject objectValue, string inherited)
    {
        if (objectValue["profile"] is JsonString direct) return direct.Value;
        if (objectValue["$container"] is JsonObject box && box["profile"] is JsonString nested) return nested.Value;
        return inherited;
    }

    private static FormatSchema Load(string resource, string file)
    {
        using var stream = typeof(RecordMigration).Assembly.GetManifestResourceStream(resource)
            ?? throw new InvalidOperationException("The resource " + resource + " is missing.");
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return FormatSchema.Load(memory.ToArray(), file);
    }

    private static void Walk(JsonObject document)
    {
        if (document["root"] is not JsonObject root) return;
        if (RootClass(document, root) is not SchemaClass classValue) return;
        Visit(root, classValue, DocumentNodePath.Root);
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

    private static void Visit(JsonObject objectValue, SchemaClass classValue, DocumentNodePath path)
    {
        foreach (var prop in classValue.Properties)
        {
            var member = objectValue.Find(prop.Name);
            if (member is null) continue;
            var at = path.Member(prop.Name);
            if (prop.IsDynamic)
            {
                if (member.Value is JsonArray array)
                    for (int i = 0; i < array.Items.Count; i++) VisitDynamic(array.Items[i], prop, at.Item(i));
                continue;
            }
            Convert(member, prop, objectValue, classValue, at);
        }
    }

    private static void VisitDynamic(JsonValue item, SchemaProperty prop, DocumentNodePath path)
    {
        if (item is not JsonObject objectValue) return;
        if (prop.Codec is FixedObjectCodec fixedObject)
            Visit(objectValue, Version2.Get(fixedObject.ClassName), path);
        else if (prop.Codec is CreatedObjectCodec && objectValue["$class"] is JsonString name && Version2.Find(name.Value) is { } classValue)
            Visit(objectValue, classValue, path);
    }

    private static void Convert(JsonMember member, SchemaProperty prop, JsonObject owner, SchemaClass ownerClass, DocumentNodePath path)
    {
        if (s_pinned.Contains(prop.Owner.Name + "." + prop.Name))
        {
            member.Value = Rewrite(member.Value, prop, owner, path);
            return;
        }
        var schema = Version2;
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

    private static readonly Dictionary<string, PropertyCodec> s_targets = BuildTargets();

    private static JsonValue Rewrite(JsonValue value, SchemaProperty prop, JsonObject owner, DocumentNodePath path)
    {
        if (value is not JsonString text)
            throw Fail(Codes.Type, prop.Name + " is hexadecimal in version 2.", path);
        byte[] bytes;
        try
        {
            bytes = HexText.Decode(text.Value);
        }
        catch (FormatException)
        {
            throw Fail(Codes.Type, prop.Name + " is hexadecimal in version 2.", path);
        }
        string key = prop.Owner.Name + "." + prop.Name;
        if (!s_targets.TryGetValue(key, out var codec))
            throw new InvalidOperationException("Version 3 has no pinned codec for " + key + ".");
        var context = new DecodeContext(bytes, Version2, "game", "migration", null, FormatLimits.Default, new LayoutCounts());
        var site = new ValueSite { Property = prop, Id = prop.Id ?? 0, Path = path, Owner = owner };
        return codec.Decode(context, 0, bytes.Length, site);
    }

    private static Dictionary<string, PropertyCodec> BuildTargets()
    {
        var rawByte = new IntegerCodec(1, false, raw: true);
        var rawInteger = new IntegerCodec(4, true, raw: true);
        var targets = new Dictionary<string, PropertyCodec>(StringComparer.Ordinal)
        {
            ["TAIGroup.IndependentRelation"] = new IntegerCodec(1, true, raw: true),
            ["TAIGroup.TargetMode"] = rawByte,
            ["TAnimation.Dimensions"] = new CatalogRecordCodec(RectangleLayout()),
            ["TAoWHSMap.VictoryMode"] = rawByte,
            ["TArmyInfoEventLog.Selection"] = rawByte,
            ["TCity.WallType"] = rawByte,
            ["TCombatData._CombatData_30"] = rawByte,
            ["TCombatData.WallType"] = rawByte,
            ["TCombatObject.ID"] = rawByte,
            ["TCombatObjectTurnDoneCA._CombatObjectTurnDoneCA_0C"] = rawByte,
            ["TCombatParty.Wall"] = rawByte,
            ["TCombatPlayer.SideIndex"] = rawByte,
            ["TCombatWall.WallType"] = rawByte,
            ["TCustomAbilityList.AbilityCount"] = new BitArrayCodec("AbCount"),
            ["TDamageCA.Effects"] = new CatalogRecordCodec(DamageLayout()),
            ["TDamageCA.DamageTypes"] = new CatalogRecordCodec(DamageLayout()),
            ["TFleeCA.ObjectID"] = rawByte,
            ["TItemExplorationSite.ItemCount"] = rawByte,
            ["TItemTE.Action"] = rawByte,
            ["TManaCrystals.Size"] = rawByte,
            ["TMine.State"] = rawByte,
            ["TMoveWallCA._MoveWallCA_0C"] = rawByte,
            ["TMoveWallCA._MoveWallCA_0D"] = rawByte,
            ["TPlayerSetupSettings.Behavior"] = new CatalogRecordCodec(BehaviorLayout()),
            ["TPlayerSetupSettings.BehaviorMode"] = rawByte,
            ["TPlayerSetupSettings.OriginalBehavior"] = new CatalogRecordCodec(BehaviorLayout()),
            ["TProductionPlace.DefenderLevel"] = rawByte,
            ["TProductionPlace.RazedState"] = rawByte,
            ["TRangedAttackCA.AbilityID"] = rawInteger,
            ["TRemotePlayer.RemoteID"] = rawInteger,
            ["TSetupSettings.PlayMode"] = rawByte,
            ["TSetupSettings.VictoryCondition"] = rawByte,
            ["TSetupSettings.IndependentAIMode"] = rawByte,
            ["TSetupSettings.ConnectionType"] = rawByte,
            ["TSetupSettings.TacticalCombatAvailability"] = rawByte,
            ["TUnitGFXResource._UnitGFXResource_30"] = rawByte,
            ["TUnitResource.Champion"] = new RawBooleanCodec(),
            ["TWMCampaignScenario.MapKind"] = rawByte,
            ["TWizardsTower.SpellID"] = rawInteger,
        };
        foreach (var name in s_pinned)
            if (!targets.ContainsKey(name))
                throw new InvalidOperationException("Version 3 has no pinned codec for " + name + ".");
        if (targets.Count != s_pinned.Count)
            throw new InvalidOperationException("A pinned codec is not in the version 2 walk.");
        return targets;
    }

    private static RecordTypeInfo RectangleLayout() => new()
    {
        Name = "TRect",
        Size = 16,
        Fields =
        [
            new IntegerRecordField { Name = "Left", Offset = 0, Size = 4, ElementWidth = 4, ElementSigned = true },
            new IntegerRecordField { Name = "Top", Offset = 4, Size = 4, ElementWidth = 4, ElementSigned = true },
            new IntegerRecordField { Name = "Right", Offset = 8, Size = 4, ElementWidth = 4, ElementSigned = true },
            new IntegerRecordField { Name = "Bottom", Offset = 12, Size = 4, ElementWidth = 4, ElementSigned = true },
        ],
        Residue = [],
    };

    private static RecordTypeInfo DamageLayout() => new()
    {
        Name = "TP3PackedDamageTypes",
        Size = 2,
        BareSet = true,
        Fields = [new SetRecordField { Name = "Value", Offset = 0, Size = 2, ElementType = "TDamageType", ElementWidth = 2 }],
        Residue = [],
    };

    private static RecordTypeInfo BehaviorLayout() => new()
    {
        Name = "TSetupPlayerBehavior",
        Size = 8,
        Fields =
        [
            new IntegerRecordField { Name = "PlayerType", Offset = 0, Size = 1, ElementWidth = 1, ElementSigned = false },
            new IntegerRecordField { Name = "Flags", Offset = 4, Size = 4, ElementWidth = 4, ElementSigned = true },
        ],
        Residue = [(1, 3)],
    };

    private static ConversionException Fail(string code, string message, DocumentNodePath path) =>
        new(new FormatDiagnostic(code, message) { Path = path.ToString() });
}
