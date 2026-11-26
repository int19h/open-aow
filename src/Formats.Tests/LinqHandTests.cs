using System.Text;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Tests;

// Issue 317 package 5 with the review fold. The facts pin search order, duplicates,
// empty input and end-of-list runs, whichever form the code has.
public sealed class LinqHandTests : FormatsTest
{
    private const string LookupSchema = """
        {"version": 6, "classes": [
          {"name": "Widget", "id": 1, "engines": ["game"], "properties": [
            {"name": "Fixed", "id": 25, "codec": "i32"},
            {"name": "High", "dynamic": 20, "codec": "i32"},
            {"name": "Low", "dynamic": 10, "codec": "i32"}
          ]}
        ]}
        """;

    [Fact]
    public void SlotFindReturnsTheFirstDuplicateAndAStoredNull()
    {
        SlotListNode empty = new();
        Assert.Null(empty.Find(1));

        SlotListNode slots = new();
        slots.Add(slot: 1, value: new IntegerNode(4, signed: true, value: 10));
        slots.Add(slot: 0, value: NullNode.Instance);
        slots.Add(slot: 1, value: new IntegerNode(4, signed: true, value: 20));
        Assert.Equal(10L, Assert.IsType<IntegerNode>(slots.Find(1)).Value);
        Assert.Same(NullNode.Instance, slots.Find(0));
        Assert.Null(slots.Find(2));
    }

    [Fact]
    public void LowerHexAcceptsAnEmptyStringAndRejectsOneBadCharacter()
    {
        Assert.True(HexText.IsLowerHex(""));
        Assert.True(HexText.IsLowerHex("aa"));
        Assert.False(HexText.IsLowerHex("a"));
        Assert.False(HexText.IsLowerHex("0g"));
        Assert.False(HexText.IsLowerHex("0A"));
    }

    [Fact]
    public void PropertyByIdPrefersTheDictionaryAndWalksDynamicsDown()
    {
        SchemaClass classValue = FormatSchema.Load(Encoding.UTF8.GetBytes(LookupSchema), file: "lookup.json").Get("Widget");
        Assert.Equal("Fixed", classValue.PropertyById(25)!.Name);
        Assert.Equal("High", classValue.PropertyById(30)!.Name);
        Assert.Equal("Low", classValue.PropertyById(15)!.Name);
        Assert.Null(classValue.PropertyById(9));
    }

    [Fact]
    public void CanonicalEndsARunAtTheEndOfTheList()
    {
        RunListCodec codec = new(4);
        ByteBuffer buffer = new();
        codec.EncodeRuns(7, items: [5, 7, 7], data: buffer);
        byte[] expected = [7, 0, 0, 0, 3, 0, 0, 0, 5, 0, 0, 0, 7, 0, 0, 0, 2, 0, 0, 0];
        Assert.Equal(expected, buffer.ToArray());
    }

    [Fact]
    public void CaseFallsBetweenReservedNamesOnBothSidesOfItsRank()
    {
        FormatSchema schema = FormatSchema.Current;
        SchemaClass classValue = schema.Get("ProductionItem");
        DocumentSite site = new()
        {
            Path = DocumentNodePath.Root,
            Schema = schema,
            Engine = "game",
            Context = new EncodeContext(schema, engine: "game", profile: "world-hsm", file: "case.json", limits: FormatLimits.Default),
        };
        JsonObject target = new();
        target.Members.Add(new JsonMember("Kind", new JsonInteger(11)));
        target.Members.Add(new JsonMember("$class", new JsonString("ProductionItem")));
        target.Members.Add(new JsonMember("$slot", new JsonInteger(2)));
        CaseCodec.ToDocument(classValue, mapping: classValue.Mapping!, target: target, site: site);
        Assert.Equal(["$class", "$case", "$slot"], target.Members.Select(member => member.Name));
        Assert.Equal("Upgrade", ((JsonString)target["$case"]!).Value);

        JsonObject appended = new();
        appended.Members.Add(new JsonMember("Kind", new JsonInteger(11)));
        CaseCodec.ToDocument(classValue, mapping: classValue.Mapping!, target: appended, site: site);
        Assert.Equal(["$case"], appended.Members.Select(member => member.Name));
    }

    [Fact]
    public void LinesKeepDuplicateAndEmptyLinesAndRefuseAControlCharacter()
    {
        List<string>? empty = StringsCodec.Lines("");
        List<string>? blank = StringsCodec.Lines("\r\n");
        List<string>? repeated = StringsCodec.Lines("aa\r\naa\r\n");
        Assert.NotNull(empty);
        Assert.NotNull(blank);
        Assert.NotNull(repeated);
        Assert.Empty(empty);
        Assert.Equal([""], blank);
        Assert.Equal(["aa", "aa"], repeated);
        Assert.Null(StringsCodec.Lines("aa\0bb\r\n"));
    }
}
