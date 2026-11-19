using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Rtl;

namespace Aow.Formats.Tests;

public sealed class SchemaTests : FormatsTest
{
    private static FormatSchema Schema => FormatSchema.Current;

    [Fact]
    public void VersionsPinTheSchemaHash()
    {
        using Stream stream = typeof(FormatSchema).Assembly.GetManifestResourceStream("Aow.Formats.versions.json")!;
        using MemoryStream memory = new();
        stream.CopyTo(memory);
        JsonObject current = (JsonObject)JsonReader.Parse(memory.ToArray());
        Assert.Equal(OwnFormat.CurrentVersion, ((JsonInteger)current["version"]!).Value);
        Assert.Null(current.Find("versions"));
        string pinned = ((JsonString)current["schemaSha256"]!).Value;
        Assert.True(pinned == Schema.Hash, $"versions.json pins {pinned}, the schema hashes to {Schema.Hash}. A schema change needs a version review.");
        Assert.Equal(OwnFormat.CurrentVersion, Schema.Version);
    }

    [Fact]
    public void EngineTablesHaveNoAmbiguity()
    {
        foreach (string? engine in new[] { "game", "tactical", "campaign" })
        {
            IReadOnlyDictionary<int, SchemaClass> table = Schema.EngineTable(engine);
            Assert.NotEmpty(table);
            foreach ((int id, SchemaClass? classValue) in table)
            {
                Assert.Equal(id, classValue.Id);
            }
        }
        Assert.Equal(NameTable.ResolveStored("TAoWHSMap"), Schema.Resolve("game", 131089)!.Name);
        Assert.Equal(NameTable.ResolveStored("TCity"), Schema.Resolve("game", 131336)!.Name);
        Assert.Null(Schema.Resolve("game", 2228482));
        Assert.Equal(NameTable.ResolveStored("TAoWCombatMapLevel"), Schema.Resolve("tactical", 2228482)!.Name);
        Assert.Equal(NameTable.ResolveStored("TWMCampaign"), Schema.Resolve("campaign", 2248704)!.Name);
        Assert.Null(Schema.Resolve("game", 2248704));
    }

    [Fact]
    public void RegistrationAmbiguitiesAreMeasuredAndRefused()
    {
        Assert.Empty(Schema.RegistrationAmbiguities);
        FormatSchema schema = FormatSchema.Load(System.Text.Encoding.UTF8.GetBytes("""
            {"version": 1, "classes": [
              {"name": "TEObject", "id": 80, "properties": []},
              {"name": "TA", "parent": "TEObject", "id": 7, "engines": ["game"], "properties": []},
              {"name": "TB", "parent": "TEObject", "id": 7, "engines": ["game"], "properties": []},
              {"name": "TC", "parent": "TEObject", "id": 8, "engines": ["game"], "properties": []}]}
            """), "ambiguous.json");
        Assert.Equal(["engine game class id 7: TA and TB"], schema.RegistrationAmbiguities);
        Assert.Null(schema.Resolve("game", 7));
        Assert.Equal("TC", schema.Resolve("game", 8)!.Name);
    }

    [Fact]
    public void EveryClassHasUniqueMemberNamesAndIds()
    {
        foreach (SchemaClass classValue in Schema.Classes)
        {
            List<string> names = [.. classValue.Properties.Select(p => p.Name)];
            Assert.Equal(names.Count, names.Distinct(StringComparer.Ordinal).Count());
            foreach (SchemaProperty p in classValue.Properties)
            {
                Assert.False(p.Name.StartsWith('$'), classValue.Name + "." + p.Name);
                if (p.Id is int id)
                {
                    Assert.Same(p, classValue.PropertyById(id));
                }
            }
        }
    }

    [Fact]
    public void ChildrenComeLastAndBaseFieldsFirst()
    {
        SchemaClass level = Schema.Get("TAoWMapLevel");
        Assert.Equal(["Width", "Height", "HexLevel", "Fields", "Children"], level.Properties.Select(p => p.Name));
        Assert.Equal(NameTable.ResolveStored("TAoWMapField"), level.FieldClass);
        SchemaClass map = Schema.Get("TAoWHSMap");
        Assert.Equal("ConnectionType", map.Properties[0].Name);
        Assert.Equal("Children", map.Properties[^1].Name);
        SchemaClass field = Schema.Get("TAoWMapField");
        Assert.Equal(["HexSpriteList", "FieldFlags"], field.Properties.Select(p => p.Name));
    }

    [Fact]
    public void SubclassIdsShadowInheritedIds()
    {
        Assert.Equal("TargetUnitIds", Schema.Get("THuntAG").PropertyById(30)!.Name);
        Assert.Equal("ManaBudget", Schema.Get("TAIGroup").PropertyById(30)!.Name);
        Assert.Equal("MapFieldResponse", Schema.Get("TAoWTerrainResource").PropertyById(20)!.Name);
    }

    [Fact]
    public void TheValleyLegacyShapeAppliesOnlyToSprites()
    {
        (string Rule, SchemaClass Shape)? shape = Schema.LegacyShape("TMapField.HexSpriteList", "TFlagMessageEventResource");
        _ = Assert.NotNull(shape);
        Assert.Equal("LEGACY_VALLEY01_SPRITE", shape.Value.Rule);
        Assert.Equal(["ResourceId", "LegacyHexX", "LegacyHexY", "UnknownProperty10", "UnknownProperty12"], shape.Value.Shape.Properties.Select(p => p.Name));
        Assert.Null(Schema.LegacyShape("TECustomNode.Children", "TFlagMessageEventResource"));
    }

    [Fact]
    public void ReservedMembersHaveTheirTotalOrder()
    {
        Assert.Equal(
            ["$format", "$version", "$kind", "$container", "$mods", "$class", "$case", "$slot", "$ext", "$padding", "$bits", "$ilb", "$text"],
            Reserved.Order);
    }

    [Fact]
    public void ProfilesCoverEveryInScopeKind()
    {
        Assert.Equal(["world-hsm", "tactical-hsm", "campaign", "scenario-set", "hero-list", "item-list"], FileProfile.All.Select(p => p.Name));
        foreach (string kind in FileProfile.AllKinds)
        {
            Assert.Contains(FileProfile.All, p => p.Kinds.Contains(kind));
        }

        foreach (FileProfile p in FileProfile.All)
        {
            Assert.NotNull(Schema.Find(p.RootFamily));
        }
    }
}
