using System.Text;
using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Tests;

/// <summary>
/// PE-4-b and PE-5-a. The product schema's first port-only member is PrisonersArmy.Items. These
/// tests build a token event through a schema, the way the converter's refusal tests build one.
/// An original image refuses a value it cannot carry and names the path. The own document keeps it.
/// </summary>
public sealed class TransportProfileTests : FormatsTest
{
    // 0x40000000 and 0x50000000. The disk schema has no member in either range.
    private const int PortId = 0x40000000;
    private const int ModId = 0x50000000;

    private const string SampleSchema = """
        {"version": 6, "classes": [
          {"name": "Detail", "id": 2, "properties": [
            {"name": "Note", "id": 1073741824, "codec": "i32"}
          ]},
          {"name": "TokenEvent", "id": 1361, "engines": ["game"], "properties": [
            {"name": "TokenCounter", "id": 10, "codec": "i32"},
            {"name": "PortState", "id": 1073741824, "codec": "i32"},
            {"name": "Detail", "id": 20, "codec": "object", "class": "Detail"},
            {"name": "Scores", "id": 11, "codec": "int-array", "width": 4, "signed": true, "count": 2},
            {"name": "ModState", "id": 1342177280, "codec": "i32"}
          ]}
        ]}
        """;

    private static FormatSchema Sample { get; } = FormatSchema.Load(Encoding.UTF8.GetBytes(SampleSchema), file: "packet.json");

    [Fact]
    public void ATokenEventWithPortStateIsRefusedOnTheLegacyPacketAndKeptOnTheDocument()
    {
        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("TokenEvent");
        ObjectNode node = Event(counter: 4);
        node.Add("PortState", new IntegerNode(4, signed: true, value: 9));

        JsonObject document = DocumentBackend.Write(node, classValue: classValue, schema: schema, engine: "game");
        Assert.Equal(4L, ((JsonInteger)document["TokenCounter"]!).Value);
        Assert.Equal(9L, ((JsonInteger)document["PortState"]!).Value);

        ConversionException error = Assert.Throws<ConversionException>(() => Packet(node, schema));
        Assert.Equal(Codes.Unrepresentable, error.Diagnostic.Code);
        Assert.Equal("root.PortState", error.Diagnostic.Path);
        Assert.Equal(PortId.ToString(System.Globalization.CultureInfo.InvariantCulture), error.Diagnostic.Value);
        Assert.Contains("TokenEvent.PortState", error.Diagnostic.Message);
        Assert.Contains("cannot carry", error.Diagnostic.Message);
    }

    [Fact]
    public void ANestedPortMemberIsRefusedWithItsPath()
    {
        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("TokenEvent");
        ObjectNode detail = new(null, slot: null);
        detail.Add("Note", new IntegerNode(4, signed: true, value: 9));
        ObjectNode node = Event(counter: 1);
        node.Add("Detail", detail);

        JsonObject document = DocumentBackend.Write(node, classValue: classValue, schema: schema, engine: "game");
        JsonObject stored = (JsonObject)document["Detail"]!;
        Assert.Equal(9L, ((JsonInteger)stored["Note"]!).Value);

        ConversionException error = Assert.Throws<ConversionException>(() => Packet(node, schema));
        Assert.Equal(Codes.Unrepresentable, error.Diagnostic.Code);
        Assert.Equal("root.Detail.Note", error.Diagnostic.Path);
        Assert.Contains("Detail.Note", error.Diagnostic.Message);
    }

    [Fact]
    public void AModRangeMemberIsRefusedWithItsPath()
    {
        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("TokenEvent");
        ObjectNode node = Event(counter: 1);
        node.Add("ModState", new IntegerNode(4, signed: true, value: 3));

        JsonObject document = DocumentBackend.Write(node, classValue: classValue, schema: schema, engine: "game");
        Assert.Equal(3L, ((JsonInteger)document["ModState"]!).Value);

        ConversionException error = Assert.Throws<ConversionException>(() => Packet(node, schema));
        Assert.Equal(Codes.Unrepresentable, error.Diagnostic.Code);
        Assert.Equal("root.ModState", error.Diagnostic.Path);
        Assert.Equal(ModId.ToString(System.Globalization.CultureInfo.InvariantCulture), error.Diagnostic.Value);
    }

    [Fact]
    public void AWiderArrayIsRefusedOnThePacketAndKeptOnTheDocument()
    {
        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("TokenEvent");
        ListNode scores = new();
        scores.Items.Add(new IntegerNode(4, signed: true, value: 1));
        scores.Items.Add(new IntegerNode(4, signed: true, value: 2));
        scores.Items.Add(new IntegerNode(4, signed: true, value: 3));
        ObjectNode node = Event(counter: 1);
        node.Add("Scores", scores);

        JsonObject document = DocumentBackend.Write(node, classValue: classValue, schema: schema, engine: "game");
        Assert.Equal(3, ((JsonArray)document["Scores"]!).Items.Count);

        ConversionException error = Assert.Throws<ConversionException>(() => Packet(node, schema));
        Assert.Equal(Codes.Range, error.Diagnostic.Code);
        Assert.Equal("root.Scores", error.Diagnostic.Path);
        Assert.Contains("holds at most 2", error.Diagnostic.Message);
        Assert.Contains("does not truncate", error.Diagnostic.Message);
    }

    [Fact]
    public void ATokenEventWithPortStateIsRefusedOnTheDiskImage()
    {
        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("TokenEvent");
        ObjectNode node = Event(counter: 4);
        node.Add("PortState", new IntegerNode(4, signed: true, value: 9));
        ObjectNode read = DocumentBackend.Read(DocumentBackend.Write(node, classValue: classValue, schema: schema, engine: "game"), classValue: classValue, schema: schema, engine: "game");
        Assert.Equal(9L, ((IntegerNode)read.Find("PortState")!).Value);

        ConversionException error = Assert.Throws<ConversionException>(() => OriginalBackend.Write(node, classValue: classValue, schema: schema, engine: "game"));
        Assert.Equal(Codes.Unrepresentable, error.Diagnostic.Code);
        Assert.Equal("root.PortState", error.Diagnostic.Path);
        Assert.Equal(PortId.ToString(System.Globalization.CultureInfo.InvariantCulture), error.Diagnostic.Value);
        Assert.Contains("cannot carry", error.Diagnostic.Message);
    }

    [Fact]
    public void AWiderArrayReloadsFromTheOwnDocumentAndTheDiskImageRefusesIt()
    {
        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("TokenEvent");
        ListNode scores = new();
        scores.Items.Add(new IntegerNode(4, signed: true, value: 1));
        scores.Items.Add(new IntegerNode(4, signed: true, value: 2));
        scores.Items.Add(new IntegerNode(4, signed: true, value: 3));
        ObjectNode node = Event(counter: 1);
        node.Add("Scores", scores);
        ObjectNode read = DocumentBackend.Read(DocumentBackend.Write(node, classValue: classValue, schema: schema, engine: "game"), classValue: classValue, schema: schema, engine: "game");
        ListNode stored = (ListNode)read.Find("Scores")!;
        Assert.Equal(3, stored.Items.Count);
        Assert.Equal(1L, ((IntegerNode)stored.Items[0]).Value);
        Assert.Equal(2L, ((IntegerNode)stored.Items[1]).Value);
        Assert.Equal(3L, ((IntegerNode)stored.Items[2]).Value);

        ConversionException error = Assert.Throws<ConversionException>(() => OriginalBackend.Write(node, classValue: classValue, schema: schema, engine: "game"));
        Assert.Equal(Codes.Range, error.Diagnostic.Code);
        Assert.Equal("root.Scores", error.Diagnostic.Path);
        Assert.Contains("holds at most 2", error.Diagnostic.Message);
        Assert.Contains("does not truncate", error.Diagnostic.Message);
    }

    [Fact]
    public void ACarriedTokenEventKeepsTheDiskImageBytes()
    {
        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("TokenEvent");
        ObjectNode node = Event(counter: 4);
        byte[] disk = OriginalBackend.Write(node, classValue: classValue, schema: schema, engine: "game");
        Assert.Equal(actual: OriginalBackend.Write(node, classValue: classValue, schema: schema, engine: "game", profile: TransportProfile.Name), expected: disk);

        byte[] packet = Packet(node, schema);
        Assert.Equal(1361, GameBoundary.ReadPacketClassId(packet));
        Assert.True(packet.AsSpan(packet.Length - disk.Length).SequenceEqual(disk));
    }

    private static ObjectNode Event(int counter)
    {
        ObjectNode node = new(null, slot: null);
        node.Add("TokenCounter", new IntegerNode(4, signed: true, value: counter));
        return node;
    }

    private static byte[] Packet(ObjectNode node, FormatSchema schema)
        => GameBoundary.WritePacket(node, classId: 1361, className: "TokenEvent", schema: schema);
}
