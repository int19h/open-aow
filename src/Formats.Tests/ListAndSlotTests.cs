using System.Text;
using Aow.Formats.Json;

using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Tests;

/// <summary>
/// A list with a stored default round-trips through the document and the canonical runs of the
/// original image at widths 1 and 4. A keyed dynamic member stores its created objects at their
/// slots. Each shape refuses a malformed value with its path.
/// </summary>
public sealed class ListAndSlotTests : FormatsTest
{
    private const string SampleSchema = """
        {"version": 3, "classes": [
          {"name": "TEObject", "id": 80, "properties": []},
          {"name": "TLists", "parent": "TEObject", "id": 7, "engines": ["game"], "properties": [
            {"name": "Numbers", "id": 1, "codec": "int-list"},
            {"name": "Small", "id": 2, "codec": "byte-list"}
          ]},
          {"name": "TSlotted", "parent": "TEObject", "id": 8, "engines": ["game"], "properties": [
            {"name": "Last", "id": 9, "codec": "i32"},
            {"name": "Parts", "dynamic": 10, "codec": "created", "family": "TPart", "slotBase": 10}
          ]},
          {"name": "TPart", "parent": "TEObject", "id": 9, "engines": ["game"], "properties": [
            {"name": "Value", "id": 1, "codec": "i32"}
          ]},
          {"name": "TSprites", "parent": "TEObject", "id": 10, "engines": ["game"], "properties": [
            {"name": "Sprites", "dynamic": 50, "codec": "created"}
          ]},
          {"name": "TFixedSlots", "parent": "TEObject", "id": 11, "engines": ["game"], "properties": [
            {"name": "Fixed", "dynamic": 10, "codec": "object", "class": "TPart", "slotBase": 10}
          ]}
        ]}
        """;

    private static FormatSchema Sample { get; } = FormatSchema.Load(Encoding.UTF8.GetBytes(SampleSchema), "lists.json");

    private static readonly PartFactory s_factory = new();

    [Fact]
    public void BothWidthsRoundTripThroughTheDocumentAndTheImage()
    {
        var schema = Sample;
        var classValue = schema.Get("TLists");
        var source = new Lists { NumbersDefault = -1, SmallDefault = 0 };
        source.Numbers.AddRange([int.MinValue, -1, -1, 5, int.MaxValue, -1]);
        source.Small.AddRange([.. Enumerable.Repeat((byte)0, 300), 255, .. Enumerable.Repeat((byte)0, 600)]);
        var tree = TreeWriter.Write(source, s_factory);
        var document = DocumentBackend.Write(tree, classValue, schema, "game");
        var numbers = (JsonObject)document["Numbers"]!;
        Assert.Equal(-1, ((JsonInteger)numbers["default"]!).Value);
        Assert.Equal(6, ((JsonArray)numbers["items"]!).Items.Count);
        Assert.Equal(901, ((JsonArray)((JsonObject)document["Small"]!)["items"]!).Items.Count);
        var image = OriginalBackend.Write(tree, classValue, schema, "game");
        byte[] numberImage =
        [
            0xFF, 0xFF, 0xFF, 0xFF, 6, 0, 0, 0,
            0, 0, 0, 0x80, 0xFF, 0xFF, 0xFF, 0xFF, 2, 0, 0, 0,
            5, 0, 0, 0, 0xFF, 0xFF, 0xFF, 0x7F, 0xFF, 0xFF, 0xFF, 0xFF, 1, 0, 0, 0,
        ];
        Assert.True(image.AsSpan().IndexOf(numberImage) >= 0);
        // A byte run longer than 255 splits: 300 is 255 and 45, 600 is 255, 255 and 90.
        byte[] smallImage = [0, 0x85, 3, 0, 0, 0, 255, 0, 45, 255, 0, 255, 0, 255, 0, 90];
        Assert.True(image.AsSpan().IndexOf(smallImage) >= 0);
        foreach (var read in new[] { OriginalBackend.Read(image, classValue, schema, "game"), DocumentBackend.Read(document, classValue, schema, "game") })
        {
            var loaded = new Lists();
            TreeReader.Read(read, loaded, s_factory);
            Assert.Equal(-1, loaded.NumbersDefault);
            Assert.Equal(source.Numbers, loaded.Numbers);
            Assert.Equal(0, loaded.SmallDefault);
            Assert.Equal(source.Small, loaded.Small);
        }
    }

    [Fact]
    public void AnEmptyListIsStoredAndAnAbsentListIsLeft()
    {
        var schema = Sample;
        var classValue = schema.Get("TLists");
        var empty = new Lists { NumbersDefault = 7, SmallDefault = 3 };
        var tree = TreeWriter.Write(empty, s_factory);
        var document = DocumentBackend.Write(tree, classValue, schema, "game");
        Assert.Equal("{\"Numbers\":{\"default\":7,\"items\":[]},\"Small\":{\"default\":3,\"items\":[]}}", Encoding.UTF8.GetString(JsonWriter.ToUtf8(document)).Replace(" ", "").Replace("\n", ""));
        var image = OriginalBackend.Write(tree, classValue, schema, "game");
        var loaded = new Lists { NumbersDefault = 1, SmallDefault = 1 };
        loaded.Numbers.Add(9);
        TreeReader.Read(OriginalBackend.Read(image, classValue, schema, "game"), loaded, s_factory);
        Assert.Equal(7, loaded.NumbersDefault);
        Assert.Empty(loaded.Numbers);
        Assert.Equal(3, loaded.SmallDefault);
        var untouched = new Lists { NumbersDefault = 1, SmallDefault = 2 };
        untouched.Numbers.Add(9);
        untouched.Small.Add(4);
        TreeReader.Read(DocumentBackend.Read(new JsonObject(), classValue, schema, "game"), untouched, s_factory);
        Assert.Equal(1, untouched.NumbersDefault);
        Assert.Equal([9], untouched.Numbers);
        Assert.Equal(2, untouched.SmallDefault);
        Assert.Equal([(byte)4], untouched.Small);
    }

    [Theory]
    [InlineData("{\"Small\":{\"default\":0,\"items\":[256]}}", Codes.Range, "root.Small.items[0]")]
    [InlineData("{\"Small\":{\"default\":-1,\"items\":[]}}", Codes.Range, "root.Small.default")]
    [InlineData("{\"Numbers\":{\"default\":0,\"items\":[\"1\"]}}", Codes.Type, "root.Numbers.items[0]")]
    [InlineData("{\"Numbers\":{\"default\":0,\"items\":[2147483648]}}", Codes.Range, "root.Numbers.items[0]")]
    [InlineData("{\"Numbers\":{\"default\":0}}", Codes.MissingMember, "root.Numbers")]
    [InlineData("{\"Numbers\":{\"default\":0,\"items\":[],\"count\":0}}", Codes.UnknownMember, "root.Numbers.count")]
    [InlineData("{\"Numbers\":[1,2]}", Codes.Type, "root.Numbers")]
    public void AMalformedListIsRefusedWithItsPath(string text, string code, string path)
    {
        var document = (JsonObject)JsonReader.Parse(Encoding.UTF8.GetBytes(text), "lists.json");
        var error = Assert.Throws<ConversionException>(() => DocumentBackend.Read(document, Sample.Get("TLists"), Sample, "game"));
        Assert.Equal(code, error.Diagnostic.Code);
        Assert.Equal(path, error.Diagnostic.Path);
    }

    [Fact]
    public void ARunPastTheCountIsRefusedOnTheImage()
    {
        var schema = Sample;
        var classValue = schema.Get("TLists");
        var image = OriginalBackend.Write(TreeWriter.Write(new Lists { Numbers = { 0 } }, s_factory), classValue, schema, "game");
        // The single item 0 is the default, stored as the run (0, 1). A run of 2 passes the count.
        int run = image.Length - 5 - 4;
        Assert.Equal(1, image[run]);
        image[run] = 2;
        var error = Assert.Throws<ConversionException>(() => OriginalBackend.Read(image, classValue, schema, "game"));
        Assert.Equal(Codes.Corrupt, error.Diagnostic.Code);
    }

    [Fact]
    public void KeyedSlotsRoundTripInStoredOrder()
    {
        var schema = Sample;
        var classValue = schema.Get("TSlotted");
        var source = new Slotted { Last = 4 };
        source.Stored.Add((3, new Part { Value = 30 }));
        source.Stored.Add((0, null));
        source.Stored.Add((1, new Part { Value = 10 }));
        var tree = TreeWriter.Write(source, s_factory);
        var document = DocumentBackend.Write(tree, classValue, schema, "game");
        var parts = (JsonArray)document["Parts"]!;
        Assert.Equal(3, parts.Items.Count);
        Assert.Equal("{\"$class\":\"TPart\",\"$slot\":3,\"Value\":30}", Compact(parts.Items[0]));
        Assert.Equal("{\"$class\":null,\"$slot\":0}", Compact(parts.Items[1]));
        var image = OriginalBackend.Write(tree, classValue, schema, "game");
        foreach (var read in new[] { OriginalBackend.Read(image, classValue, schema, "game"), DocumentBackend.Read(document, classValue, schema, "game") })
        {
            Assert.Equal(Compact(document), Compact(DocumentBackend.Write(read, classValue, schema, "game")));
            var loaded = new Slotted { Asked = [1, 3, 0, 7, 1] };
            TreeReader.Read(read, loaded, s_factory);
            Assert.Equal(4, loaded.Last);
            Assert.Equal([10, 30, -1, -1, 10], loaded.Loaded.Select(part => part?.Value ?? -1));
            Assert.NotSame(loaded.Loaded[0], loaded.Loaded[4]);
        }
    }

    [Fact]
    public void ASlotTheLoadDoesNotAskForStaysUnread()
    {
        var schema = Sample;
        var classValue = schema.Get("TSlotted");
        var source = new Slotted();
        source.Stored.Add((2, new Part { Value = 1 }));
        var tree = TreeWriter.Write(source, s_factory);
        var loaded = new Slotted { Asked = [], SkipParts = true };
        TreeReader.Read(tree, loaded, s_factory);
        Assert.Empty(loaded.Loaded);
        var strict = new Slotted { Asked = [] };
        var error = Assert.Throws<ConversionException>(() => TreeReader.Read(tree, strict, s_factory));
        Assert.Equal(Codes.Required, error.Diagnostic.Code);
        Assert.Equal("root.Parts", error.Diagnostic.Path);
    }

    [Fact]
    public void ASlotStoredTwiceOrBelowZeroIsRefused()
    {
        var twice = new Slotted();
        twice.Stored.Add((1, new Part()));
        twice.Stored.Add((1, new Part()));
        var error = Assert.Throws<ConversionException>(() => TreeWriter.Write(twice, s_factory));
        Assert.Equal(Codes.Slot, error.Diagnostic.Code);
        Assert.Equal("root.Parts[1]", error.Diagnostic.Path);
        var negative = new Slotted();
        negative.Stored.Add((-1, new Part()));
        error = Assert.Throws<ConversionException>(() => TreeWriter.Write(negative, s_factory));
        Assert.Equal(Codes.Range, error.Diagnostic.Code);
    }

    [Theory]
    [InlineData("{\"Parts\":[{\"$class\":\"TPart\",\"$slot\":1},{\"$class\":\"TPart\",\"$slot\":1}]}", Codes.Slot, "root.Parts[1]")]
    [InlineData("{\"Parts\":[{\"$class\":\"TPart\",\"$slot\":-1}]}", Codes.Range, "root.Parts[0].$slot")]
    [InlineData("{\"Parts\":[{\"$class\":\"TPart\"}]}", Codes.MissingMember, "root.Parts[0]")]
    [InlineData("{\"Parts\":[{\"$class\":null,\"$slot\":0,\"Value\":1}]}", Codes.UnknownMember, "root.Parts[0].Value")]
    [InlineData("{\"Parts\":[{\"$class\":\"TSlotted\",\"$slot\":0}]}", Codes.ClassFamily, "root.Parts[0]")]
    [InlineData("{\"Parts\":{\"$class\":\"TPart\",\"$slot\":0}}", Codes.Type, "root.Parts")]
    public void AMalformedSlotIsRefusedWithItsPath(string text, string code, string path)
    {
        var document = (JsonObject)JsonReader.Parse(Encoding.UTF8.GetBytes(text), "slots.json");
        var error = Assert.Throws<ConversionException>(() => DocumentBackend.Read(document, Sample.Get("TSlotted"), Sample, "game"));
        Assert.Equal(code, error.Diagnostic.Code);
        Assert.Equal(path, error.Diagnostic.Path);
    }

    [Fact]
    public void KeyedSlotsOnASequenceAreRefused()
    {
        var schema = Sample;
        var tree = TreeWriter.Write(new SpriteHolder(), s_factory);
        var classValue = schema.Get("TSprites");
        var document = Assert.Throws<ConversionException>(() => DocumentBackend.Write(tree, classValue, schema, "game"));
        Assert.Equal(Codes.Type, document.Diagnostic.Code);
        Assert.Equal("root.Sprites", document.Diagnostic.Path);
        var image = Assert.Throws<ConversionException>(() => OriginalBackend.Write(tree, classValue, schema, "game"));
        Assert.Equal(Codes.Type, image.Diagnostic.Code);
        Assert.Equal("root.Sprites", image.Diagnostic.Path);
    }

    // A fixed dynamic member stores fixed objects, so a created object at its slot is refused.
    [Fact]
    public void ACreatedObjectInAFixedSlotIsRefused()
    {
        var schema = Sample;
        var tree = TreeWriter.Write(new FixedSlots(), s_factory);
        var classValue = schema.Get("TFixedSlots");
        var document = Assert.Throws<ConversionException>(() => DocumentBackend.Write(tree, classValue, schema, "game"));
        Assert.Equal("Version 3 stores no slot of Fixed.", document.Diagnostic.Message);
        var image = Assert.Throws<ConversionException>(() => OriginalBackend.Write(tree, classValue, schema, "game"));
        Assert.Equal("An item of Fixed is a fixed object at its slot.", image.Diagnostic.Message);
        Assert.Equal("root.Fixed[0]", image.Diagnostic.Path);
    }

    private static string Compact(JsonValue value) => Encoding.UTF8.GetString(JsonWriter.ToUtf8(value)).Replace(" ", "").Replace("\n", "");

    private sealed class Lists : IPersistable
    {
        public List<int> Numbers { get; } = [];
        public int NumbersDefault { get; set; }
        public List<byte> Small { get; } = [];
        public byte SmallDefault { get; set; }

        public void Persist(IPersister persister)
        {
            NumbersDefault = persister.Int32List("Numbers", Numbers, NumbersDefault);
            SmallDefault = persister.ByteList("Small", Small, SmallDefault);
        }
    }

    private sealed class Part : IPersistable
    {
        public int Value { get; set; }

        public void Persist(IPersister persister) => Value = persister.Int32("Value", Value);
    }

    // Save stores Stored in its order. Load asks for the slots in Asked, in that order.
    private sealed class Slotted : IPersistable
    {
        public int Last { get; set; }
        public List<(int Slot, Part? Part)> Stored { get; } = [];
        public int[] Asked { get; init; } = [];
        public bool SkipParts { get; init; }
        public List<Part?> Loaded { get; } = [];

        public void Persist(IPersister persister)
        {
            Last = persister.Int32("Last", Last);
            if (persister.Loading)
            {
                foreach (int slot in Asked) Loaded.Add(persister.CreatedSlot<Part>("Parts", slot: slot, objectValue: null));
                if (SkipParts) persister.Skip("Parts");
                return;
            }
            foreach (var (slot, part) in Stored) persister.CreatedSlot("Parts", slot: slot, objectValue: part);
        }
    }

    private sealed class SpriteHolder : IPersistable
    {
        public void Persist(IPersister persister) => persister.CreatedSlot("Sprites", slot: 0, objectValue: new Part());
    }

    private sealed class FixedSlots : IPersistable
    {
        public void Persist(IPersister persister) => persister.CreatedSlot("Fixed", slot: 0, objectValue: new Part());
    }

    private sealed class PartFactory : IPersistableFactory
    {
        public string ClassName(IPersistable objectValue) => objectValue is Part ? "TPart" : "TSlotted";

        public IPersistable Create(string className) => className switch
        {
            "TPart" => new Part(),
            "TSlotted" => new Slotted(),
            _ => throw new InvalidOperationException("The test factory has no class " + className + "."),
        };
    }
}
