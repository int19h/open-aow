using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;

namespace Aow.Formats.Tests;

public sealed class SchemaTests : FormatsTest
{
    private static FormatSchema Schema => FormatSchema.Current;

    [Fact]
    public void VersionsPinTheSchemaHash()
    {
        using var stream = typeof(FormatSchema).Assembly.GetManifestResourceStream("Aow.Formats.versions.json")!;
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        var versions = (JsonObject)JsonReader.Parse(memory.ToArray());
        Assert.Equal(OwnFormat.CurrentVersion, ((JsonInteger)versions["current"]!).Value);
        var current = ((JsonArray)versions["versions"]!).Items.Cast<JsonObject>()
            .Single(v => ((JsonInteger)v["version"]!).Value == OwnFormat.CurrentVersion);
        string pinned = ((JsonString)current["schemaSha256"]!).Value;
        Assert.True(pinned == Schema.Hash, $"versions.json pins {pinned}, the schema hashes to {Schema.Hash}. A schema change needs a version review.");
        Assert.Equal(OwnFormat.CurrentVersion, Schema.Version);
    }

    [Fact]
    public void EngineTablesHaveNoAmbiguity()
    {
        foreach (var engine in new[] { "game", "tactical", "campaign" })
        {
            var table = Schema.EngineTable(engine);
            Assert.NotEmpty(table);
            foreach (var (id, cls) in table) Assert.Equal(id, cls.Id);
        }
        Assert.Equal("TAoWHSMap", Schema.Resolve("game", 131089)!.Name);
        Assert.Equal("TCity", Schema.Resolve("game", 131336)!.Name);
        Assert.Null(Schema.Resolve("game", 2228482));
        Assert.Equal("TAoWCombatMapLevel", Schema.Resolve("tactical", 2228482)!.Name);
        Assert.Equal("TWMCampaign", Schema.Resolve("campaign", 2248704)!.Name);
        Assert.Null(Schema.Resolve("game", 2248704));
    }

    [Fact]
    public void RegistrationAmbiguitiesAreMeasuredAndRefused()
    {
        Assert.Empty(Schema.RegistrationAmbiguities);
        var text = """
            {"version": 1, "classes": [
              {"name": "TEObject", "id": 80, "properties": []},
              {"name": "TA", "parent": "TEObject", "id": 7, "engines": ["game"], "properties": []},
              {"name": "TB", "parent": "TEObject", "id": 7, "engines": ["game"], "properties": []},
              {"name": "TC", "parent": "TEObject", "id": 8, "engines": ["game"], "properties": []}]}
            """;
        var schema = FormatSchema.Load(System.Text.Encoding.UTF8.GetBytes(text), "ambiguous.json");
        Assert.Equal(["engine game class id 7: TA and TB"], schema.RegistrationAmbiguities);
        Assert.Null(schema.Resolve("game", 7));
        Assert.Equal("TC", schema.Resolve("game", 8)!.Name);
    }

    [Fact]
    public void EveryClassHasUniqueMemberNamesAndIds()
    {
        foreach (var cls in Schema.Classes)
        {
            var names = cls.Properties.Select(p => p.Name).ToList();
            Assert.Equal(names.Count, names.Distinct(StringComparer.Ordinal).Count());
            foreach (var p in cls.Properties)
            {
                Assert.False(p.Name.StartsWith('$'), cls.Name + "." + p.Name);
                if (p.Id is int id) Assert.Same(p, cls.PropertyById(id));
            }
        }
    }

    [Fact]
    public void ChildrenComeLastAndBaseFieldsFirst()
    {
        var level = Schema.Get("TAoWMapLevel");
        Assert.Equal(["Width", "Height", "Lhx", "Fields", "Children"], level.Properties.Select(p => p.Name));
        Assert.Equal("TAoWMapField", level.FieldClass);
        var map = Schema.Get("TAoWHSMap");
        Assert.Equal("ConnectionType", map.Properties[0].Name);
        Assert.Equal("Children", map.Properties[^1].Name);
        var field = Schema.Get("TAoWMapField");
        Assert.Equal(["Sprites", "FieldFlags"], field.Properties.Select(p => p.Name));
    }

    [Fact]
    public void SubclassIdsShadowInheritedIds()
    {
        Assert.Equal("TargetUnitIDs", Schema.Get("THuntAG").PropertyById(30)!.Name);
        Assert.Equal("ManaBudget", Schema.Get("TAIGroup").PropertyById(30)!.Name);
        Assert.Equal("MapFieldResponse", Schema.Get("TAoWTerrainResource").PropertyById(20)!.Name);
    }

    [Fact]
    public void TheValleyLegacyShapeAppliesOnlyToSprites()
    {
        var shape = Schema.LegacyShape("TMapField.Sprites", "TFlagMessageEventResource");
        Assert.NotNull(shape);
        Assert.Equal("LEGACY_VALLEY01_SPRITE", shape.Value.Rule);
        Assert.Equal(["ResourceID", "LegacyXhx", "LegacyYhx", "LegacyProperty10", "LegacyProperty12"], shape.Value.Shape.Properties.Select(p => p.Name));
        Assert.Null(Schema.LegacyShape("TECustomNode.Children", "TFlagMessageEventResource"));
    }

    [Fact]
    public void ReservedMembersHaveTheirTotalOrder()
    {
        Assert.Equal(
            ["$format", "$version", "$kind", "$container", "$mods", "$class", "$case", "$slot", "$ext", "$residue", "$padding", "$bits", "$text", "$layout"],
            Reserved.Order);
    }

    [Fact]
    public void ProfilesCoverEveryInScopeKind()
    {
        Assert.Equal(["world-hsm", "tactical-hsm", "campaign", "scenario-set", "hero-list", "item-list"], FileProfile.All.Select(p => p.Name));
        foreach (var kind in FileProfile.AllKinds) Assert.Contains(FileProfile.All, p => p.Kinds.Contains(kind));
        foreach (var p in FileProfile.All) Assert.NotNull(Schema.Find(p.RootFamily));
    }
}
