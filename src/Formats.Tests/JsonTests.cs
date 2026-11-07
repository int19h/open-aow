using System.Text;
using Aow.Formats.Json;

namespace Aow.Formats.Tests;

public sealed class JsonTests : FormatsTest
{
    private static ConversionException Fails(string text) =>
        Assert.Throws<ConversionException>(() => JsonReader.Parse(Encoding.UTF8.GetBytes(text), "t.json"));

    [Fact]
    public void ParsesMembersInOrderWithSpans()
    {
        var value = (JsonObject)JsonReader.Parse("{\n  \"b\": 1,\n  \"a\": [true, null, \"x\"]\n}");
        Assert.Equal(["b", "a"], value.Members.Select(m => m.Name));
        Assert.Equal(new TextSpan(9, 2, 8), value.Members[0].Value.Span);
        var array = (JsonArray)value["a"]!;
        Assert.Equal(3, array.Items.Count);
        Assert.Equal(3, array.Items[2].Span.Line);
        Assert.Equal(21, array.Items[2].Span.Column);
    }

    [Fact]
    public void AcceptsOneLeadingByteOrderMark()
    {
        var bytes = Encoding.UTF8.GetPreamble().Concat("{}"u8.ToArray()).ToArray();
        Assert.IsType<JsonObject>(JsonReader.Parse(bytes));
    }

    [Theory]
    [InlineData("{\"a\": 1,}", Codes.Syntax, 1, 9)]
    [InlineData("[1, 2,]", Codes.Syntax, 1, 7)]
    [InlineData("{\"a\": 1} x", Codes.Syntax, 1, 10)]
    [InlineData("{\"a\": 1 // c\n}", Codes.Syntax, 1, 9)]
    [InlineData("{\"a\": 1.5}", Codes.Number, 1, 7)]
    [InlineData("{\"a\": 1e3}", Codes.Number, 1, 7)]
    [InlineData("{\"a\": 99999999999999999999}", Codes.Range, 1, 7)]
    [InlineData("{\"a\": \"\\ud800\"}", Codes.Surrogate, 1, 7)]
    [InlineData("{\"a\": \"x\ty\"}", Codes.Syntax, 1, 9)]
    [InlineData("{\n  \"a\": 1,\n  \"a\": 2\n}", Codes.Duplicate, 3, 3)]
    [InlineData("{\"a\": tru}", Codes.Syntax, 1, 7)]
    [InlineData("{\"a\": 01}", Codes.Syntax, 1, 8)]
    public void RejectsInvalidInputWithLocation(string text, string code, int line, int column)
    {
        var error = Fails(text);
        Assert.Equal(code, error.Diagnostic.Code);
        Assert.Equal(line, error.Diagnostic.Span.Line);
        Assert.Equal(column, error.Diagnostic.Span.Column);
        Assert.Equal("t.json", error.Diagnostic.File);
    }

    [Fact]
    public void DuplicateMemberNamesBothDeclarations()
    {
        var error = Fails("{\n  \"a\": 1,\n  \"a\": 2\n}");
        Assert.Contains("first at 2:3", error.Diagnostic.Message);
    }

    [Fact]
    public void RejectsInvalidUtf8WithByteOffset()
    {
        var error = Assert.Throws<ConversionException>(() => JsonReader.Parse(new byte[] { (byte)'{', 0xFF, (byte)'}' }));
        Assert.Equal(Codes.Utf8, error.Diagnostic.Code);
        Assert.Equal(1, error.Diagnostic.Offset);
    }

    [Fact]
    public void LimitsAreLocated()
    {
        var limits = new FormatLimits { Depth = 2 };
        var error = Assert.Throws<ConversionException>(() => JsonReader.Parse("{\"a\": {\"b\": {}}}", null, false, limits));
        Assert.Equal(Codes.Limit, error.Diagnostic.Code);
        Assert.Equal(13, error.Diagnostic.Span.Column);
    }

    [Fact]
    public void WritesCanonicalForm()
    {
        var value = new JsonObject()
            .Add("a", new JsonInteger(-5))
            .Add("e", new JsonObject())
            .Add("l", new JsonArray())
            .Add("s", new JsonString("q\"\\\b\t\n\f\r\u0001\u007f\u0081\u009f\u00a0\u00e9\u20ac"))
            .Add("n", new JsonArray().Add(new JsonBool(true)).Add(new JsonNull()));
        Assert.Equal(
            "{\n  \"a\": -5,\n  \"e\": {},\n  \"l\": [],\n  \"s\": \"q\\\"\\\\\\b\\t\\n\\f\\r\\u0001\\u007f\\u0081\\u009f\u00a0\u00e9\u20ac\",\n  \"n\": [\n    true,\n    null\n  ]\n}\n",
            JsonWriter.ToText(value));
    }

    [Fact]
    public void WritesFieldRowsOnSingleLines()
    {
        var rows = new JsonArray { Layout = ArrayLayout.RowsOnLines };
        rows.Add(new JsonArray().Add(new JsonObject().Add("Terrain", new JsonInteger(1))).Add(new JsonObject().Add("Terrain", new JsonInteger(2))));
        rows.Add(new JsonArray().Add(new JsonObject().Add("Sprites", new JsonArray().Add(new JsonObject()))).Add(new JsonObject()));
        var text = JsonWriter.ToText(new JsonObject().Add("Fields", rows));
        Assert.Equal(
            "{\n  \"Fields\": [\n    [{\"Terrain\": 1}, {\"Terrain\": 2}],\n    [{\"Sprites\": [{}]}, {}]\n  ]\n}\n",
            text);
    }

    [Fact]
    public void AParsedDocumentWritesItsFieldRowsBackOnSingleLines()
    {
        var rows = new JsonArray { Layout = ArrayLayout.RowsOnLines };
        rows.Add(new JsonArray().Add(new JsonObject().Add("Sprites", new JsonArray().Add(new JsonObject().Add("$class", new JsonString("T"))))));
        var text = JsonWriter.ToText(new JsonObject().Add("Level", new JsonObject().Add("Fields", rows)));
        Assert.Equal(text, JsonWriter.ToText(JsonReader.Parse(text)));
    }

    [Fact]
    public void WriterRefusesUnpairedSurrogates()
    {
        var error = Assert.Throws<ConversionException>(() => JsonWriter.ToText(new JsonString("\ud800")));
        Assert.Equal(Codes.Surrogate, error.Diagnostic.Code);
    }

    [Fact]
    public void WriterOutputHasNoByteOrderMarkAndOneFinalLineFeed()
    {
        var bytes = JsonWriter.ToUtf8(new JsonObject().Add("a", new JsonString("\u00e9")));
        Assert.NotEqual(0xEF, bytes[0]);
        Assert.Equal((byte)'\n', bytes[^1]);
        Assert.NotEqual((byte)'\n', bytes[^2]);
        Assert.DoesNotContain((byte)'\r', bytes);
    }

    [Fact]
    public void DiagnosticTextHasFileLineColumnCodeAndPath()
    {
        var d = new FormatDiagnostic(Codes.Reference, "Leader id 17 is absent from root.Leaders.")
        {
            File = "Save/edited.asg",
            Span = new TextSpan(0, 42, 19),
            Path = "root.Players[1].LeaderId",
        };
        Assert.Equal("Save/edited.asg:42:19 E_REFERENCE root.Players[1].LeaderId\nLeader id 17 is absent from root.Leaders.", d.ToString());
        var binary = new FormatDiagnostic(Codes.Corrupt, "Bad.") { File = "a.hsm", Offset = 0x41565, Profile = "world-hsm", Path = "root" };
        Assert.Equal("a.hsm:0x41565 E_CORRUPT root [world-hsm]\nBad.", binary.ToString());
    }
}
