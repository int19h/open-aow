using System.Text;
using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Tests;

/// <summary>
/// An enum stores its C# member name, a set stores names and a fixed integer array stores a list.
/// Export refuses a list longer than the original count.
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
        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("TSampleHolder");
        ObjectNode node = new(null, null);
        node.Add("Rank", new EnumNode("TSampleRank", 1, false, 1));
        node.Add("Marks", new SetNode("TSampleRank", 1, 0x81));
        ListNode scores = new();
        scores.Items.Add(new IntegerNode(4, true, int.MinValue));
        scores.Items.Add(new IntegerNode(4, true, 0));
        node.Add("Scores", scores);
        JsonObject document = DocumentBackend.Write(node, classValue, schema, "game");
        Assert.Equal("Gold", ((JsonString)document["Rank"]!).Value);
        Assert.Equal(["None", 7L], Items(document["Marks"]!));
        Assert.Equal([(long)int.MinValue, 0L], Items(document["Scores"]!));
        JsonObject back = DocumentBackend.Write(OriginalBackend.Read(OriginalBackend.Write(node, classValue, schema, "game"), classValue: classValue, schema: schema, engine: "game"), classValue, schema, "game");
        Assert.Equal(JsonWriter.ToUtf8(document), JsonWriter.ToUtf8(back));
        ObjectNode unnamed = new(null, null);
        unnamed.Add("Rank", new EnumNode("TSampleRank", 1, false, 9));
        Assert.Equal(9, ((JsonInteger)DocumentBackend.Write(unnamed, classValue, schema, "game")["Rank"]!).Value);
    }

    [Fact]
    public void AShorterListFillsTheRestWithZero()
    {
        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("TSampleHolder");
        JsonObject back = DocumentBackend.Write(OriginalBackend.Read(OriginalBackend.Write(DocumentBackend.Read(new JsonObject().Add("Scores", new JsonArray().Add(new JsonInteger(7))), classValue: classValue, schema: schema, engine: "game"), classValue, schema, "game"), classValue: classValue, schema: schema, engine: "game"), classValue, schema, "game");
        Assert.Equal([7L, 0L], Items(back["Scores"]!));
    }

    [Fact]
    public void AnUnknownNameAndAWrongEnumAreRefused()
    {
        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("TSampleHolder");
        ConversionException unknown = Assert.Throws<ConversionException>(() => Pilot(classValue, schema, new JsonObject().Add("Rank", new JsonString("missing"))));
        Assert.Equal(Codes.Type, unknown.Diagnostic.Code);
        Assert.Equal("root.Rank", unknown.Diagnostic.Path);
        Assert.Equal("missing", unknown.Diagnostic.Value);
        ConversionException wrong = Assert.Throws<ConversionException>(() => Pilot(classValue, schema, new JsonObject().Add("Rank", new JsonString("orLow"))));
        Assert.Equal(Codes.Type, wrong.Diagnostic.Code);
        Assert.Equal("root.Rank", wrong.Diagnostic.Path);
        Assert.Equal("orLow", wrong.Diagnostic.Value);
        ConversionException set = Assert.Throws<ConversionException>(() => Pilot(classValue, schema, new JsonObject().Add("Marks", new JsonArray().Add(new JsonString("orLow")))));
        Assert.Equal("root.Marks[0]", set.Diagnostic.Path);
    }

    [Fact]
    public void ANonIntegerListItemIsRefused()
    {
        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("TSampleHolder");
        JsonObject document = new JsonObject().Add("Scores", new JsonArray().Add(new JsonInteger(1)).Add(new JsonString("no")));
        ConversionException error = Assert.Throws<ConversionException>(() => Pilot(classValue, schema, document));
        Assert.Equal(Codes.Type, error.Diagnostic.Code);
        Assert.Equal("root.Scores[1]", error.Diagnostic.Path);
    }

    [Fact]
    public void AListLongerThanTheOriginalCountIsRefusedOnExport()
    {
        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("TSampleHolder");
        ObjectNode node = DocumentBackend.Read(new JsonObject().Add("Scores", new JsonArray().Add(new JsonInteger(1)).Add(new JsonInteger(2)).Add(new JsonInteger(3))), classValue: classValue, schema: schema, engine: "game");
        Assert.Equal(3, ((ListNode)node.Find("Scores")!).Items.Count);
        ConversionException error = Assert.Throws<ConversionException>(() => OriginalBackend.Write(node, classValue, schema, "game"));
        Assert.Equal(Codes.Range, error.Diagnostic.Code);
        Assert.Equal("root.Scores", error.Diagnostic.Path);
        Assert.Contains("holds at most 2", error.Diagnostic.Message);
        Assert.Contains("Export does not truncate", error.Diagnostic.Message);
    }

    [Fact]
    public void AListOnAnotherShapeIsRefused()
    {
        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("TSampleHolder");
        ObjectNode node = new(null, null);
        node.Add("Rank", new ListNode());
        ConversionException error = Assert.Throws<ConversionException>(() => DocumentBackend.Write(node, classValue, schema, "game"));
        Assert.Equal("root.Rank", error.Diagnostic.Path);
        Assert.Equal("Version 2 stores a list only for a fixed integer array.", error.Diagnostic.Message);
    }

    [Fact]
    public void AMissingCatalogEntryCarriesThePath()
    {
        ConversionException direct = Assert.Throws<ConversionException>(() => EnumCatalog.Require("TNoSuchEnum", "sample.json", "root.Rank"));
        Assert.Equal(Codes.Type, direct.Diagnostic.Code);
        Assert.Equal("root.Rank", direct.Diagnostic.Path);
        Assert.Equal("sample.json", direct.Diagnostic.File);
        Assert.Contains("TNoSuchEnum", direct.Diagnostic.Message);

        const string missing = """
            {"version":2,"classes":[{"name":"TSampleHolder","id":7,"engines":["game"],"properties":[
              {"name":"Rank","id":1,"codec":"enum","enum":"TNoSuchEnum","width":1,"signed":false}
            ]}]}
            """;
        FormatSchema schema = FormatSchema.Load(Encoding.UTF8.GetBytes(missing), "missing.json");
        ConversionException error = Assert.Throws<ConversionException>(() => Pilot(schema.Get("TSampleHolder"), schema, new JsonObject().Add("Rank", new JsonInteger(0))));
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

    // A mapped class stores its flat members in the image. The document backend names the case
    // the stored discriminator selects, as the converter does, and reads the case back flat.
    [Fact]
    public void AMappedClassStoresItsCaseOnBothBackends()
    {
        FormatSchema schema = FormatSchema.Current;
        SchemaClass classValue = schema.Get("TProductionItem");
        ObjectNode node = Production(kind: 1, parameter: 5, groupId: 3);
        JsonObject document = DocumentBackend.Write(node, classValue, schema, "game");
        AssertMembers(document, ("$case", "ProduceUnit"), ("Index", 5L), ("GroupId", 3L));
        byte[] image = OriginalBackend.Write(node, classValue, schema, "game");
        Assert.Equal(image, OriginalBackend.Write(OriginalBackend.Read(image, classValue, schema, "game"), classValue: classValue, schema: schema, engine: "game"));
        Assert.Equal(image, OriginalBackend.Write(DocumentBackend.Read(document, classValue, schema, "game"), classValue: classValue, schema: schema, engine: "game"));

        // A key no case registers keeps every stored member in the unknown case.
        JsonObject unknown = DocumentBackend.Write(Production(kind: 99, parameter: 7, groupId: 2), classValue, schema, "game");
        AssertMembers(unknown, ("$case", "Unknown"), ("Kind", 99L), ("Parameter", 7L), ("GroupId", 2L));
        Assert.Equal(OriginalBackend.Write(Production(kind: 99, parameter: 7, groupId: 2), classValue, schema, "game"),
            OriginalBackend.Write(DocumentBackend.Read(unknown, classValue, schema, "game"), classValue, schema, "game"));

        // A case field the case does not keep is refused with its path.
        JsonObject wrong = new JsonObject().Add("$case", new JsonString("ResearchSpell")).Add("Index", new JsonInteger(4)).Add("GroupId", new JsonInteger(1));
        ConversionException error = Assert.Throws<ConversionException>(() => DocumentBackend.Read(wrong, classValue, schema, "game"));
        Assert.Equal(Codes.UnknownMember, error.Diagnostic.Code);
        Assert.Equal("root.GroupId", error.Diagnostic.Path);
    }

    private static void AssertMembers(JsonObject document, params (string Name, object Value)[] expected)
    {
        Assert.Equal(expected.Select(member => member.Name), document.Members.Select(member => member.Name));
        foreach ((string? name, object? value) in expected)
        {
            object actual = document[name] switch
            {
                JsonString text => text.Value,
                JsonInteger number => number.Value,
                var other => other!,
            };
            Assert.Equal(value, actual);
        }
    }

    private static ObjectNode Production(int kind, int parameter, int groupId)
    {
        ObjectNode node = new(null, null);
        node.Add("Kind", new IntegerNode(4, true, kind));
        node.Add("Parameter", new IntegerNode(4, true, parameter));
        node.Add("GroupId", new IntegerNode(4, true, groupId));
        return node;
    }

    private static void AssertWidth(string kind, string property, string path)
    {
        string json = "{\"version\":2,\"classes\":[{\"name\":\"TSampleHolder\",\"properties\":[" + property + "]}]}";
        InvalidOperationException error = Assert.Throws<InvalidOperationException>(() => FormatSchema.Load(Encoding.UTF8.GetBytes(json), "wide.json"));
        Assert.Contains(path, error.Message);
        Assert.Contains(kind, error.Message);
    }

    private static byte[] Pilot(SchemaClass classValue, FormatSchema schema, JsonObject document) =>
        OriginalBackend.Write(DocumentBackend.Read(document, classValue, schema, "game"), classValue, schema, "game");

    private static List<object> Items(JsonValue value)
    {
        JsonArray array = (JsonArray)value;
        List<object> items = new(array.Items.Count);
        foreach (JsonValue item in array.Items)
        {
            if (item is JsonString text)
            {
                items.Add(text.Value);
            }
            else if (item is JsonInteger number)
            {
                items.Add(number.Value);
            }
            else
            {
                items.Add(item.KindName);
            }
        }
        return items;
    }
}
