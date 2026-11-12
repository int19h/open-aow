using System.Text;
using Aow.Formats.Json;
using Aow.Formats.Migrations;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Tests;

/// <summary>
/// Version 2 stores an enum as its C# member name, a set as names and a fixed integer array as
/// a list. Version 1 hex migrates to those values. Export refuses a list longer than the original count.
/// </summary>
public sealed class SymbolicTests : FormatsTest
{
    private const string SampleSchema = """
        {"version": 2, "classes": [
          {"name": "TEObject", "id": 80, "properties": []},
          {"name": "TSampleHolder", "parent": "TEObject", "id": 7, "engines": ["game"], "properties": [
            {"name": "Rank", "id": 1, "codec": "enum", "enum": "TSampleRank", "width": 1, "signed": false},
            {"name": "Marks", "id": 2, "codec": "set", "set": "TSampleRank", "width": 1},
            {"name": "Scores", "id": 3, "codec": "int-array", "width": 4, "signed": true, "count": 2}
          ]}
        ]}
        """;

    private static FormatSchema Sample => FormatSchema.Load(Encoding.UTF8.GetBytes(SampleSchema), "sample.json");

    [Fact]
    public void NamesAndListsRoundTripThroughTheOriginalBackend()
    {
        var schema = Sample;
        var classValue = schema.Get("TSampleHolder");
        var node = new ObjectNode(null, null);
        node.Add("Rank", new EnumNode("TSampleRank", 1, false, 1));
        node.Add("Marks", new SetNode("TSampleRank", 1, 0x81));
        var scores = new ListNode();
        scores.Items.Add(new IntegerNode(4, true, int.MinValue));
        scores.Items.Add(new IntegerNode(4, true, 0));
        node.Add("Scores", scores);
        var document = DocumentBackend.Write(node, classValue, schema, "game");
        Assert.Equal("Gold", ((JsonString)document["Rank"]!).Value);
        Assert.Equal(["None", 7L], Items(document["Marks"]!));
        Assert.Equal([(long)int.MinValue, 0L], Items(document["Scores"]!));
        var image = OriginalBackend.Write(node, classValue, schema, "game");
        var back = DocumentBackend.Write(OriginalBackend.Read(image, classValue, schema, "game"), classValue, schema, "game");
        Assert.Equal(JsonWriter.ToUtf8(document), JsonWriter.ToUtf8(back));
        var unnamed = new ObjectNode(null, null);
        unnamed.Add("Rank", new EnumNode("TSampleRank", 1, false, 9));
        Assert.Equal(9, ((JsonInteger)DocumentBackend.Write(unnamed, classValue, schema, "game")["Rank"]!).Value);
    }

    [Fact]
    public void AShorterListFillsTheRestWithZero()
    {
        var schema = Sample;
        var classValue = schema.Get("TSampleHolder");
        var document = new JsonObject().Add("Scores", new JsonArray().Add(new JsonInteger(7)));
        var image = OriginalBackend.Write(DocumentBackend.Read(document, classValue, schema, "game"), classValue, schema, "game");
        var back = DocumentBackend.Write(OriginalBackend.Read(image, classValue, schema, "game"), classValue, schema, "game");
        Assert.Equal([7L, 0L], Items(back["Scores"]!));
    }

    [Fact]
    public void AnUnknownNameAndAWrongEnumAreRefused()
    {
        var schema = Sample;
        var classValue = schema.Get("TSampleHolder");
        var unknown = Assert.Throws<ConversionException>(() => Pilot(classValue, schema, new JsonObject().Add("Rank", new JsonString("missing"))));
        Assert.Equal(Codes.Type, unknown.Diagnostic.Code);
        Assert.Equal("root.Rank", unknown.Diagnostic.Path);
        Assert.Equal("missing", unknown.Diagnostic.Value);
        var wrong = Assert.Throws<ConversionException>(() => Pilot(classValue, schema, new JsonObject().Add("Rank", new JsonString("orLow"))));
        Assert.Equal(Codes.Type, wrong.Diagnostic.Code);
        Assert.Equal("root.Rank", wrong.Diagnostic.Path);
        Assert.Equal("orLow", wrong.Diagnostic.Value);
        var set = Assert.Throws<ConversionException>(() => Pilot(classValue, schema, new JsonObject().Add("Marks", new JsonArray().Add(new JsonString("orLow")))));
        Assert.Equal("root.Marks[0]", set.Diagnostic.Path);
    }

    [Fact]
    public void ANonIntegerListItemIsRefused()
    {
        var schema = Sample;
        var classValue = schema.Get("TSampleHolder");
        var document = new JsonObject().Add("Scores", new JsonArray().Add(new JsonInteger(1)).Add(new JsonString("no")));
        var error = Assert.Throws<ConversionException>(() => Pilot(classValue, schema, document));
        Assert.Equal(Codes.Type, error.Diagnostic.Code);
        Assert.Equal("root.Scores[1]", error.Diagnostic.Path);
    }

    [Fact]
    public void AListLongerThanTheOriginalCountIsRefusedOnExport()
    {
        var schema = Sample;
        var classValue = schema.Get("TSampleHolder");
        var document = new JsonObject().Add("Scores", new JsonArray().Add(new JsonInteger(1)).Add(new JsonInteger(2)).Add(new JsonInteger(3)));
        var node = DocumentBackend.Read(document, classValue, schema, "game");
        Assert.Equal(3, ((ListNode)node.Find("Scores")!).Items.Count);
        var error = Assert.Throws<ConversionException>(() => OriginalBackend.Write(node, classValue, schema, "game"));
        Assert.Equal(Codes.Range, error.Diagnostic.Code);
        Assert.Equal("root.Scores", error.Diagnostic.Path);
        Assert.Contains("holds at most 2", error.Diagnostic.Message);
        Assert.Contains("Export does not truncate", error.Diagnostic.Message);
    }

    [Fact]
    public void AListOnAnotherShapeIsRefused()
    {
        var schema = Sample;
        var classValue = schema.Get("TSampleHolder");
        var node = new ObjectNode(null, null);
        node.Add("Rank", new ListNode());
        var error = Assert.Throws<ConversionException>(() => DocumentBackend.Write(node, classValue, schema, "game"));
        Assert.Equal("root.Rank", error.Diagnostic.Path);
        Assert.Equal("Version 2 stores a list only for a fixed integer array.", error.Diagnostic.Message);
    }

    [Fact]
    public void Version1HexMigratesToNamesAndLists()
    {
        var relation = Migrate("""
            {"$format":"OpenAoW","$version":1,"$kind":"hero-list","$container":{"profile":"hero-list","compression":"none"},
             "root":{"$class":"TDiplomaticRelationHistoryItem","Day":4,"Relation":"02"}}
            """);
        var root = (JsonObject)relation.Document["root"]!;
        Assert.Equal([SymbolicMigration.Name, RecordMigration.Name, ListMigration.Name], relation.Migrations);
        Assert.Equal(4, ((JsonInteger)relation.Document["$version"]!).Value);
        Assert.Equal("Alliance", ((JsonString)root["Relation"]!).Value);
        Assert.Equal(4, ((JsonInteger)root["Day"]!).Value);

        var unnamed = Migrate("""
            {"$format":"OpenAoW","$version":1,"$kind":"hero-list","$container":{"profile":"hero-list","compression":"none"},
             "root":{"$class":"TDiplomaticRelationHistoryItem","Relation":"ff"}}
            """);
        Assert.Equal(255, ((JsonInteger)((JsonObject)unnamed.Document["root"]!)["Relation"]!).Value);

        var style = Migrate("""
            {"$format":"OpenAoW","$version":1,"$kind":"hero-list","$container":{"profile":"hero-list","compression":"none"},
             "root":{"$class":"TEControlStyleObject","ControlStyle":"a5"}}
            """);
        Assert.Equal(["SaveLoad", "VerifyFixedChildren", 5L, 7L], Items(((JsonObject)style.Document["root"]!)["ControlStyle"]!));

        var spell = Migrate("""
            {"$format":"OpenAoW","$version":1,"$kind":"hero-list","$container":{"profile":"hero-list","compression":"none"},
             "root":{"$class":"TUnitSpellCA","SpellData":"07000000ffffffff"}}
            """);
        Assert.Equal([7L, -1L], Items(((JsonObject)spell.Document["root"]!)["SpellData"]!));
    }

    [Fact]
    public void Version1HexInsideNestedValuesMigrates()
    {
        var child = MigrateRoot("""{"$class":"TGlobalEnchantmentList","Children":[{"$class":"TGlobalEnchantment","$slot":0,"EnchantmentType":"03"}]}""");
        var enchantment = (JsonObject)((JsonArray)((JsonObject)child.Document["root"]!)["Children"]!).Items[0];
        Assert.Equal("PowerLeak", ((JsonString)enchantment["EnchantmentType"]!).Value);

        var created = MigrateRoot("""{"$class":"TGlobalEnchantmentSpellTE","Enchantment":{"$class":"TGlobalEnchantment","EnchantmentType":"01"}}""");
        var made = (JsonObject)((JsonObject)created.Document["root"]!)["Enchantment"]!;
        Assert.Equal("Hatred", ((JsonString)made["EnchantmentType"]!).Value);

        var dynamicItem = MigrateRoot("""{"$class":"TAbilityControl","Abilities":[{"SelectionTypes":"0100"}]}""");
        var ability = (JsonObject)((JsonArray)((JsonObject)dynamicItem.Document["root"]!)["Abilities"]!).Items[0];
        Assert.Equal(new List<object> { "Unit" }, Items(ability["SelectionTypes"]!));

        var field = MigrateRoot("""{"$class":"TMapLevel","Fields":[[{"Sprites":[{"$class":"TDiplomaticRelationHistoryItem","Relation":"01"}]}]]}""");
        var row = (JsonArray)((JsonArray)((JsonObject)field.Document["root"]!)["Fields"]!).Items[0];
        var sprites = (JsonArray)((JsonObject)row.Items[0])["Sprites"]!;
        Assert.Equal("War", ((JsonString)((JsonObject)sprites.Items[0])["Relation"]!).Value);

        var campaign = MigrateRoot("""{"$class":"TAoWCampaign","Map":{"root":{"$class":"TDiplomaticRelationHistoryItem","Relation":"03"}}}""");
        var nested = (JsonObject)((JsonObject)((JsonObject)campaign.Document["root"]!)["Map"]!)["root"]!;
        Assert.Equal("Unknown", ((JsonString)nested["Relation"]!).Value);
    }

    [Fact]
    public void MalformedVersion1HexAndAnIntegerAreRefused()
    {
        var integer = Assert.Throws<ConversionException>(() => MigrateRoot("""{"$class":"TDiplomaticRelationHistoryItem","Relation":2}"""));
        Assert.Equal(Codes.Type, integer.Diagnostic.Code);
        Assert.Equal("root.Relation", integer.Diagnostic.Path);
        Assert.Contains("hexadecimal", integer.Diagnostic.Message);

        var broken = Assert.Throws<ConversionException>(() => MigrateRoot("""{"$class":"TDiplomaticRelationHistoryItem","Relation":"zz"}"""));
        Assert.Equal(Codes.Type, broken.Diagnostic.Code);
        Assert.Equal("root.Relation", broken.Diagnostic.Path);
    }

    [Fact]
    public void AMissingCatalogEntryCarriesThePath()
    {
        var direct = Assert.Throws<ConversionException>(() => EnumCatalog.Require("TNoSuchEnum", "sample.json", "root.Rank"));
        Assert.Equal(Codes.Type, direct.Diagnostic.Code);
        Assert.Equal("root.Rank", direct.Diagnostic.Path);
        Assert.Equal("sample.json", direct.Diagnostic.File);
        Assert.Contains("TNoSuchEnum", direct.Diagnostic.Message);

        const string Missing = """
            {"version":2,"classes":[{"name":"TSampleHolder","id":7,"engines":["game"],"properties":[
              {"name":"Rank","id":1,"codec":"enum","enum":"TNoSuchEnum","width":1,"signed":false}
            ]}]}
            """;
        var schema = FormatSchema.Load(Encoding.UTF8.GetBytes(Missing), "missing.json");
        var error = Assert.Throws<ConversionException>(() => Pilot(schema.Get("TSampleHolder"), schema, new JsonObject().Add("Rank", new JsonInteger(0))));
        Assert.Equal(Codes.Type, error.Diagnostic.Code);
        Assert.Equal("root.Rank", error.Diagnostic.Path);
    }

    [Fact]
    public void AnUnsupportedSymbolicWidthIsRefusedWithTheSchemaPath()
    {
        AssertWidth("enum", """{"name":"Rank","codec":"enum","enum":"TSampleRank","width":8,"signed":false}""", "TSampleHolder.Rank");
        AssertWidth("set", """{"name":"Marks","codec":"set","set":"TSampleRank","width":3}""", "TSampleHolder.Marks");
        AssertWidth("int-array", """{"name":"Scores","codec":"int-array","width":8,"signed":true,"count":1}""", "TSampleHolder.Scores");
    }

    [Fact]
    public void MigrationRefreshesLayoutFingerprints()
    {
        var root = new JsonObject()
            .Add("$class", new JsonString("TDiplomaticRelationHistoryItem"))
            .Add("Day", new JsonInteger(4))
            .Add("Relation", new JsonString("02"));
        AttachLayout(root, "hero-list");
        var container = new JsonObject()
            .Add("profile", new JsonString("hero-list"))
            .Add("compression", new JsonString("none"));
        AttachLayout(container, "hero-list");
        var read = GameFiles.Read(GameFiles.Write(Envelope("hero-list", container, root)), "layout.json");
        var migrated = (JsonObject)read.Document["root"]!;
        Assert.Equal("Alliance", ((JsonString)migrated["Relation"]!).Value);
        Assert.Equal(LayoutResidue.Fingerprint(migrated, "hero-list"), FingerprintOf(migrated));
        Assert.Equal(LayoutResidue.TableFormOrder, PartCategory(migrated));
        var migratedContainer = (JsonObject)read.Document["$container"]!;
        Assert.Equal(LayoutResidue.Fingerprint(migratedContainer, "hero-list"), FingerprintOf(migratedContainer));

        var nested = new JsonObject()
            .Add("$class", new JsonString("TDiplomaticRelationHistoryItem"))
            .Add("Relation", new JsonString("03"));
        var map = new JsonObject()
            .Add("$container", new JsonObject()
                .Add("profile", new JsonString("world-hsm"))
                .Add("compression", new JsonString("cfs"))
                .Add("level", new JsonInteger(1)))
            .Add("root", nested);
        AttachLayout(map, "world-hsm");
        var campaign = new JsonObject().Add("$class", new JsonString("TAoWCampaign")).Add("Map", map);
        var outer = new JsonObject().Add("profile", new JsonString("campaign")).Add("compression", new JsonString("none"));
        var migratedCampaign = GameFiles.Read(GameFiles.Write(Envelope("campaign", outer, campaign)), "campaign.json").Document;
        var migratedMap = (JsonObject)((JsonObject)migratedCampaign["root"]!)["Map"]!;
        Assert.Equal("Unknown", ((JsonString)((JsonObject)migratedMap["root"]!)["Relation"]!).Value);
        Assert.Equal(LayoutResidue.Fingerprint(migratedMap, "world-hsm"), FingerprintOf(migratedMap));
        Assert.NotEqual(LayoutResidue.Fingerprint(migratedMap, "campaign"), FingerprintOf(migratedMap));
        Assert.Equal(LayoutResidue.TableFormOrder, PartCategory(migratedMap));
    }

    [Fact]
    public void TheHeroListGoldenKeepsItsBody()
    {
        var golden = Golden("hero-list-v1.json");
        var read = GameFiles.Read(golden, "hero-list-v1.json");
        var original = (JsonObject)JsonReader.Parse(golden, "hero-list-v1.json");
        Assert.Equal([SymbolicMigration.Name, RecordMigration.Name, ListMigration.Name], read.Migrations);
        Assert.Equal(4, ((JsonInteger)read.Document["$version"]!).Value);
        Assert.Equal(JsonWriter.ToUtf8((JsonObject)original["root"]!), JsonWriter.ToUtf8((JsonObject)read.Document["root"]!));
    }

    [Fact]
    public void AMappedClassIsRefusedByBothBackends()
    {
        var schema = FormatSchema.Current;
        var classValue = schema.Get("TDiplomaticAction");
        var node = new ObjectNode(null, null);
        var document = Assert.Throws<ConversionException>(() => DocumentBackend.Write(node, classValue, schema, "game"));
        Assert.Equal(Codes.Mapping, document.Diagnostic.Code);
        Assert.Equal("root", document.Diagnostic.Path);
        var original = Assert.Throws<ConversionException>(() => OriginalBackend.Write(node, classValue, schema, "game"));
        Assert.Equal(Codes.Mapping, original.Diagnostic.Code);
        Assert.Equal("root", original.Diagnostic.Path);
        var read = Assert.Throws<ConversionException>(() => OriginalBackend.Read([], classValue, schema, "game"));
        Assert.Equal("root", read.Diagnostic.Path);
        Assert.Contains(classValue.Name, read.Diagnostic.Message);
        var production = Assert.Throws<ConversionException>(() => DocumentBackend.Read(new JsonObject(), schema.Get("TProductionItem"), schema, "game"));
        Assert.Equal(Codes.Mapping, production.Diagnostic.Code);
        Assert.Equal("root", production.Diagnostic.Path);
    }

    private static void AttachLayout(JsonObject target, string profile)
    {
        var layout = new JsonObject()
            .Add("codec", new JsonString(LayoutResidue.Codec))
            .Add("fingerprint", new JsonString(LayoutResidue.Fingerprint(target, profile)))
            .Add("parts", new JsonArray().Add(new JsonObject()
                .Add("category", new JsonString(LayoutResidue.TableFormOrder))
                .Add("entries", new JsonArray())));
        int at = 0;
        while (at < target.Members.Count && target.Members[at].Name.StartsWith('$')) at++;
        target.Members.Insert(at, new JsonMember("$layout", layout));
    }

    private static JsonObject Envelope(string profile, JsonObject container, JsonObject root) => new JsonObject()
        .Add("$format", new JsonString(OwnFormat.FormatName))
        .Add("$version", new JsonInteger(1))
        .Add("$kind", new JsonString(profile))
        .Add("$container", container)
        .Add("root", root);

    private static string FingerprintOf(JsonObject objectValue) =>
        ((JsonString)((JsonObject)objectValue["$layout"]!)["fingerprint"]!).Value;

    private static string PartCategory(JsonObject objectValue)
    {
        var parts = (JsonArray)((JsonObject)objectValue["$layout"]!)["parts"]!;
        return ((JsonString)((JsonObject)parts.Items[0])["category"]!).Value;
    }

    private static ReadResult Migrate(string json) => GameFiles.Read(Encoding.UTF8.GetBytes(json), "symbolic.json");

    private static ReadResult MigrateRoot(string root) =>
        Migrate("{\"$format\":\"OpenAoW\",\"$version\":1,\"root\":" + root + "}");

    private static void AssertWidth(string kind, string property, string path)
    {
        string json = "{\"version\":2,\"classes\":[{\"name\":\"TSampleHolder\",\"properties\":[" + property + "]}]}";
        var error = Assert.Throws<InvalidOperationException>(() => FormatSchema.Load(Encoding.UTF8.GetBytes(json), "wide.json"));
        Assert.Contains(path, error.Message);
        Assert.Contains(kind, error.Message);
    }

    private static byte[] Golden(string name)
    {
        using var stream = typeof(SymbolicTests).Assembly.GetManifestResourceStream("Aow.Formats.Tests.Golden." + name)!;
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return memory.ToArray();
    }

    private static byte[] Pilot(SchemaClass classValue, FormatSchema schema, JsonObject document) =>
        OriginalBackend.Write(DocumentBackend.Read(document, classValue, schema, "game"), classValue, schema, "game");

    private static List<object> Items(JsonValue value)
    {
        var array = (JsonArray)value;
        var items = new List<object>(array.Items.Count);
        foreach (var item in array.Items)
        {
            if (item is JsonString text) items.Add(text.Value);
            else if (item is JsonInteger number) items.Add(number.Value);
            else items.Add(item.KindName);
        }
        return items;
    }
}
