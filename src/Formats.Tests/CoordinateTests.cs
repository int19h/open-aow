using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Tests;

/// <summary>
/// A map position is one document member and up to three original image entries.
/// Width 1 keeps the unsigned byte, so 255 round-trips. Width 4 refuses a value
/// outside -128 to 127 and names the component.
/// </summary>
public sealed class CoordinateTests : FormatsTest
{
    private const string SampleSchema = """
        {"version": 6, "classes": [
          {"name": "Marker", "id": 1, "engines": ["game"], "properties": [
            {"name": "Location", "id": 5, "codec": "coordinate", "width": 1, "parts": [
              {"name": "X", "id": 5}, {"name": "Y", "id": 6}, {"name": "MapLevel", "id": 7}]}
          ]},
          {"name": "Spot", "id": 2, "engines": ["game"], "properties": [
            {"name": "Location", "id": 10, "codec": "coordinate", "width": 4, "parts": [
              {"name": "X", "id": 10}, {"name": "Y", "id": 11}, {"name": "MapLevel", "id": 12}]}
          ]}
        ]}
        """;

    private static FormatSchema Sample { get; } = FormatSchema.Load(System.Text.Encoding.UTF8.GetBytes(SampleSchema), "coordinate.json");

    [Fact]
    public void AnEmptyCoordinateIsOmitted()
    {
        var node = new ObjectNode(null, slot: null);
        node.Add("Location", new RecordNode());
        var document = WriteDocument("Marker", node);
        Assert.Null(document.Find("Location"));
        var read = ReadImage("Marker", WriteImage("Marker", node));
        Assert.Null(read.Find("Location"));
    }

    [Fact]
    public void APresentZeroIsKeptAndAnAbsentComponentStaysAbsent()
    {
        var location = new RecordNode();
        location.Add("X", new IntegerNode(1, signed: false, value: 0));
        location.Add("MapLevel", new IntegerNode(1, signed: false, value: 2));
        var node = new ObjectNode(null, slot: null);
        node.Add("Location", location);

        var document = WriteDocument("Marker", node);
        var stored = (JsonObject)document["Location"]!;
        Assert.Equal(0L, ((JsonInteger)stored["X"]!).Value);
        Assert.Null(stored.Find("Y"));
        Assert.Equal(2L, ((JsonInteger)stored["MapLevel"]!).Value);

        var read = (RecordNode)ReadImage("Marker", WriteImage("Marker", node)).Find("Location")!;
        Assert.Equal(0L, ((IntegerNode)read.Find("X")!).Value);
        Assert.Null(read.Find("Y"));
        Assert.Equal(2L, ((IntegerNode)read.Find("MapLevel")!).Value);
    }

    [Fact]
    public void AnUnsignedByteOf255RoundTrips()
    {
        var location = new RecordNode();
        location.Add("X", new IntegerNode(1, signed: false, value: 255));
        location.Add("Y", new IntegerNode(1, signed: false, value: 255));
        var node = new ObjectNode(null, slot: null);
        node.Add("Location", location);

        var stored = (JsonObject)WriteDocument("Marker", node)["Location"]!;
        Assert.Equal(255L, ((JsonInteger)stored["X"]!).Value);
        Assert.Equal(255L, ((JsonInteger)stored["Y"]!).Value);
        Assert.Null(stored.Find("MapLevel"));

        var read = (RecordNode)ReadImage("Marker", WriteImage("Marker", node)).Find("Location")!;
        Assert.Equal(255L, ((IntegerNode)read.Find("X")!).Value);
        Assert.Equal(255L, ((IntegerNode)read.Find("Y")!).Value);
    }

    [Fact]
    public void AWideCoordinateRefusesAValueOutsideTheMapRange()
    {
        var location = new RecordNode();
        location.Add("X", new IntegerNode(4, signed: true, value: 200));
        var node = new ObjectNode(null, slot: null);
        node.Add("Location", location);

        var written = Assert.Throws<ConversionException>(() => WriteDocument("Spot", node));
        Assert.Equal(Codes.Range, written.Diagnostic.Code);
        Assert.Equal("root.Location.X", written.Diagnostic.Path);

        var image = Assert.Throws<ConversionException>(() => WriteImage("Spot", node));
        Assert.Equal(Codes.Range, image.Diagnostic.Code);
        Assert.Equal("root.Location.X", image.Diagnostic.Path);

        var document = Assert.Throws<ConversionException>(() => ReadDocument("Spot", """{"Location":{"Y":128}}"""));
        Assert.Equal(Codes.Range, document.Diagnostic.Code);
        Assert.Equal("root.Location.Y", document.Diagnostic.Path);

        byte[] stored = WriteImage("Spot", Spot(x: 127));
        int at = IndexOf(stored, [0x7F, 0, 0, 0]);
        stored[at] = 200;
        var read = Assert.Throws<ConversionException>(() => ReadImage("Spot", stored));
        Assert.Equal(Codes.Range, read.Diagnostic.Code);
        Assert.Equal("root.Location.X", read.Diagnostic.Path);
    }

    private static ObjectNode Spot(int x)
    {
        var location = new RecordNode();
        location.Add("X", new IntegerNode(4, signed: true, value: x));
        var node = new ObjectNode(null, slot: null);
        node.Add("Location", location);
        return node;
    }

    private static JsonObject WriteDocument(string className, ObjectNode node) =>
        DocumentBackend.Write(node, classValue: Sample.Get(className), schema: Sample, engine: "game");

    private static ObjectNode ReadDocument(string className, string json) =>
        DocumentBackend.Read((JsonObject)JsonReader.Parse(System.Text.Encoding.UTF8.GetBytes(json), "coordinate.json"), classValue: Sample.Get(className), schema: Sample, engine: "game");

    private static byte[] WriteImage(string className, ObjectNode node) =>
        OriginalBackend.Write(node, classValue: Sample.Get(className), schema: Sample, engine: "game");

    private static ObjectNode ReadImage(string className, byte[] image) =>
        OriginalBackend.Read(image, classValue: Sample.Get(className), schema: Sample, engine: "game");

    private static int IndexOf(byte[] image, byte[] pattern)
    {
        int found = -1;
        for (int i = 0; i <= image.Length - pattern.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < pattern.Length; j++)
            {
                if (image[i + j] != pattern[j]) { match = false; break; }
            }

            if (!match)
            {
                continue;
            }

            if (found >= 0)
            {
                throw new InvalidOperationException("The stored 127 is not unique.");
            }

            found = i;
        }
        if (found < 0)
        {
            throw new InvalidOperationException("The stored 127 is absent.");
        }

        return found;
    }
}
