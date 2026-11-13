using System.Buffers.Binary;
using System.Text;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Tests;

/// <summary>
/// Resource payloads on the real schema: the single of the AI as a number, a terrain
/// list as pairs, the flood's terrain grid as changed hexagons, an ILB library as base64 with its
/// header checked, an image sequence list as layers, and the fixed objects of the ability and
/// spell controls at their slots. Each shape round-trips through the converter and through the tree
/// on both backends, and refuses a malformed value with its path.
/// </summary>
public sealed class ResourceShapeTests : FormatsTest
{
    private static readonly FormatSchema s_schema = FormatSchema.Current;

    [Theory]
    [InlineData("00000000", "0")]
    [InlineData("0000803f", "1")]
    [InlineData("cdcccc3d", "0.1")]
    [InlineData("0000c0bf", "-1.5")]
    [InlineData("acc52737", "1e-5")]
    [InlineData("ffff7f7f", "3.4028235e38")]
    [InlineData("01000000", "1e-45")]
    [InlineData("00000080", """{"$bits":"00000080"}""")]
    [InlineData("0000807f", """{"$bits":"0000807f"}""")]
    [InlineData("000080ff", """{"$bits":"000080ff"}""")]
    [InlineData("0100c07f", """{"$bits":"0100c07f"}""")]
    public void ASingleIsItsShortestNumber(string bits, string document)
    {
        var bytes = System.Convert.FromHexString(bits);
        var value = Decode("AIPlayerControl", "DefenseStrengthMultiplier", bytes);
        Assert.Equal(document, Compact(value));
        Assert.Equal(bytes, Encode("AIPlayerControl", "DefenseStrengthMultiplier", value));
        Assert.Equal(bytes, Encode("AIPlayerControl", "DefenseStrengthMultiplier", JsonReader.Parse(document)));
    }

    [Theory]
    [InlineData("0.10", "cdcccc3d")]
    [InlineData("1E-5", "acc52737")]
    [InlineData("2", "00000040")]
    [InlineData("0.100000001", "cdcccc3d")]
    [InlineData("""{"$bits":"0000803f"}""", "0000803f")]
    public void ASingleReadsAnyNumberText(string document, string bits) =>
        Assert.Equal(bits, System.Convert.ToHexStringLower(Encode("AIPlayerControl", "DefenseStrengthMultiplier", JsonReader.Parse(document))));

    [Theory]
    [InlineData("\"0.5\"", Codes.Type, "root.DefenseStrengthMultiplier")]
    [InlineData("1e39", Codes.Range, "root.DefenseStrengthMultiplier")]
    [InlineData("""{"$bits":"00"}""", Codes.Type, "root.DefenseStrengthMultiplier.$bits")]
    [InlineData("""{"$bits":"00000000","more":1}""", Codes.UnknownMember, "root.DefenseStrengthMultiplier.more")]
    public void AMalformedSingleIsRefused(string json, string code, string path) =>
        Refused("AIPlayerControl", "DefenseStrengthMultiplier", json, code, path);

    [Fact]
    public void ATerrainListIsPairs()
    {
        byte[] bytes = [3, 0, 0, 0, 1, 0xFE, 0xFE, 4, 0x7F, 0x80];
        var value = Decode("MapObjectResource", "TerrainList", bytes);
        Assert.Equal("[[1,-2],[-2,4],[127,-128]]", Compact(value));
        Assert.Equal(bytes, Encode("MapObjectResource", "TerrainList", value));
        Assert.Equal("[]", Compact(Decode("MapObjectResource", "TerrainList", [0, 0, 0, 0])));
    }

    [Theory]
    [InlineData("[[1,2,3]]", Codes.Type, "root.TerrainList[0]")]
    [InlineData("[[128,0]]", Codes.Range, "root.TerrainList[0][0]")]
    [InlineData("[[0,\"a\"]]", Codes.Type, "root.TerrainList[0][1]")]
    [InlineData("""{"pairs":[]}""", Codes.Type, "root.TerrainList")]
    public void AMalformedTerrainListIsRefused(string json, string code, string path) =>
        Refused("MapObjectResource", "TerrainList", json, code, path);

    [Fact]
    public void AFloodTerrainGridListsTheChangedHexagons()
    {
        // Three by two hexagons: [1][0] holds 0 -1, [0][2] holds 5 0, the others are unchanged.
        byte[] bytes = [0, 0, 0, 0, 5, 0, 0, 0xFF, 0, 0, 0, 0];
        var owner = Grid(width: 3, height: 2);
        var value = Decode("FloodControl", "OriginalTerrain", bytes, owner);
        Assert.Equal("""[{"X":2,"Y":0,"Terrain":5,"Overlay":0},{"X":0,"Y":1,"Terrain":0,"Overlay":-1}]""", Compact(value));
        Assert.Equal(bytes, Encode("FloodControl", "OriginalTerrain", value, owner));
        Assert.Equal(new byte[12], Encode("FloodControl", "OriginalTerrain", new JsonArray(), owner));
        var truncated = Assert.Throws<ConversionException>(() => Decode("FloodControl", "OriginalTerrain", [0, 0], owner));
        Assert.Equal(Codes.Dimensions, truncated.Diagnostic.Code);
    }

    [Theory]
    [InlineData("""[{"X":2,"Y":0,"Terrain":5,"Overlay":0},{"X":1,"Y":0,"Terrain":5,"Overlay":0}]""", Codes.Range, "root.OriginalTerrain[1]")]
    [InlineData("""[{"X":1,"Y":0,"Terrain":5,"Overlay":0},{"X":1,"Y":0,"Terrain":6,"Overlay":0}]""", Codes.Duplicate, "root.OriginalTerrain[1]")]
    [InlineData("""[{"X":1,"Y":0,"Terrain":0,"Overlay":0}]""", Codes.Range, "root.OriginalTerrain[0]")]
    [InlineData("""[{"X":3,"Y":0,"Terrain":1,"Overlay":0}]""", Codes.Range, "root.OriginalTerrain[0].X")]
    [InlineData("""[{"X":0,"Y":2,"Terrain":1,"Overlay":0}]""", Codes.Range, "root.OriginalTerrain[0].Y")]
    [InlineData("""[{"X":0,"Y":0,"Terrain":1}]""", Codes.MissingMember, "root.OriginalTerrain[0]")]
    [InlineData("""[{"X":0,"Y":0,"Terrain":1,"Overlay":0,"Sprite":1}]""", Codes.UnknownMember, "root.OriginalTerrain[0].Sprite")]
    [InlineData("""[{"X":0,"Y":0,"Terrain":-129,"Overlay":0}]""", Codes.Range, "root.OriginalTerrain[0].Terrain")]
    [InlineData("""[3]""", Codes.Type, "root.OriginalTerrain[0]")]
    public void AMalformedTerrainGridIsRefused(string json, string code, string path)
    {
        var error = Assert.Throws<ConversionException>(() => Encode("FloodControl", "OriginalTerrain", JsonReader.Parse(json), Grid(width: 3, height: 2)));
        Assert.Equal(code, error.Diagnostic.Code);
        Assert.Equal(path, error.Diagnostic.Path);
    }

    [Fact]
    public void AnImageLibraryIsBase64()
    {
        var bytes = Library();
        var value = Decode("MapObjectResource", "ImageLibrary", bytes);
        Assert.Equal("""{"$ilb":"BElMQgAWUFUAAIBAGAAAACAAAAAgAAAAAAAAAP////8="}""", Compact(value));
        Assert.Equal(bytes, Encode("MapObjectResource", "ImageLibrary", value));
    }

    [Theory]
    [InlineData(0, 5, Codes.Corrupt)]
    [InlineData(11, 0x3F, Codes.Corrupt)]
    [InlineData(12, 25, Codes.Corrupt)]
    [InlineData(16, 40, Codes.Corrupt)]
    [InlineData(16, 20, Codes.Corrupt)]
    [InlineData(20, 31, Codes.Corrupt)]
    public void AnImageLibraryWithAFalseHeaderIsRefused(int offset, byte value, string code)
    {
        var bytes = Library();
        bytes[offset] = value;
        var decoded = Assert.Throws<ConversionException>(() => Decode("MapObjectResource", "ImageLibrary", bytes));
        Assert.Equal(code, decoded.Diagnostic.Code);
        var encoded = Assert.Throws<ConversionException>(() => Encode("MapObjectResource", "ImageLibrary",
            new JsonObject().Add("$ilb", new JsonString(System.Convert.ToBase64String(bytes)))));
        Assert.Equal(code, encoded.Diagnostic.Code);
        Assert.Equal("root.ImageLibrary.$ilb", encoded.Diagnostic.Path);
    }

    [Theory]
    [InlineData("""{"$ilb":"BElMQgAWUFUAAIBAGAAAACAAAAAgAAAAAAAAAP////8"}""", Codes.Type, "root.ImageLibrary.$ilb")]
    [InlineData("""{"$ilb":"BElMQgAWUFUAAIBAGAAAACAA AAAgAAAAAAAAAP////8="}""", Codes.Type, "root.ImageLibrary.$ilb")]
    [InlineData("""{"$ilb":"BElMQgAWUFUAAIBAGAAAACAAAAAgAAAAAAAAAP////9="}""", Codes.Type, "root.ImageLibrary.$ilb")]
    [InlineData("""{"$ilb":"BElMQg==","name":"x"}""", Codes.UnknownMember, "root.ImageLibrary.name")]
    [InlineData("""{}""", Codes.MissingMember, "root.ImageLibrary")]
    [InlineData("""{"$ilb":"BElMQg=="}""", Codes.Corrupt, "root.ImageLibrary.$ilb")]
    [InlineData("\"04494c42\"", Codes.Type, "root.ImageLibrary")]
    public void AMalformedImageLibraryIsRefused(string json, string code, string path) =>
        Refused("MapObjectResource", "ImageLibrary", json, code, path);

    [Fact]
    public void AnImageSequenceListStoresLayers()
    {
        var bytes = Sequences(nil: 1, next: 1);
        var value = Decode("MapObjectResource", "ImageSequenceList", bytes);
        Assert.Equal(SequencesText, Compact(value));
        Assert.Equal(bytes, Encode("MapObjectResource", "ImageSequenceList", value));
        foreach (var (className, property) in new[] { ("Ability", "ImageSequences"), ("GamePack", "ImageSequences"), ("Spell", "CastImages"), ("UnitGraphicsResource", "Sequences") })
            Assert.Equal(bytes, Encode(className, property, Decode(className, property, bytes)));
        Assert.Equal("[]", Compact(Decode("MapObjectResource", "ImageSequenceList", [0, 0, 0, 0])));
    }

    [Fact]
    public void ABooleanByteTheWriterDoesNotMakeBecomesALayoutPart()
    {
        var bytes = Sequences(nil: 2, next: 5);
        var prop = Property("MapObjectResource", "ImageSequenceList");
        var context = new DecodeContext(bytes, s_schema, "game", "world-hsm", "resources.json", FormatLimits.Default, new LayoutCounts());
        var builder = new LayoutBuilder();
        var owner = new JsonObject();
        var value = prop.Codec.Decode(context, 0, bytes.Length, new ValueSite { Property = prop, Id = 10, Path = DocumentNodePath.Root.Member("ImageSequenceList"), Owner = owner, Builder = builder });
        Assert.Equal(SequencesText, Compact(value));
        owner.Add("ImageSequenceList", value);
        builder.Attach(owner, "world-hsm", new LayoutCounts());
        var encode = new EncodeContext(s_schema, "game", "world-hsm", "resources.json", FormatLimits.Default, ExportOptions.Default);
        var reader = LayoutReader.Read(encode, owner, DocumentNodePath.Root)!;
        var data = new ByteBuffer();
        prop.Codec.Encode(encode, value, data, new ValueSite { Property = prop, Id = 10, Path = DocumentNodePath.Root.Member("ImageSequenceList"), Owner = owner, Reader = reader });
        Assert.Equal(bytes, data.ToArray());
        Assert.Equal(Sequences(nil: 1, next: 1), Encode("MapObjectResource", "ImageSequenceList", value));
    }

    [Theory]
    [InlineData("""[[{"Mode":"None","Frames":{"default":0,"items":[]}}]]""", Codes.UnknownMember, "root.ImageSequenceList[0][0].Frames")]
    [InlineData("""[[{"Mode":"FrameTable"}]]""", Codes.MissingMember, "root.ImageSequenceList[0][0]")]
    [InlineData("""[[]]""", Codes.Type, "root.ImageSequenceList[0]")]
    [InlineData("""[[{"Layer":256}]]""", Codes.Range, "root.ImageSequenceList[0][0].Layer")]
    [InlineData("""[[{"Mode":"Sideways"}]]""", Codes.Type, "root.ImageSequenceList[0][0].Mode")]
    [InlineData("""[[{"Speed":1,"Image":3}]]""", Codes.UnknownMember, "root.ImageSequenceList[0][0].Image")]
    [InlineData("""[[{"Mode":"RandomFrameTable","Frames":{"default":0,"items":["a"]}}]]""", Codes.Type, "root.ImageSequenceList[0][0].Frames.items[0]")]
    [InlineData("""[5]""", Codes.Type, "root.ImageSequenceList[0]")]
    [InlineData("""[[{"Library":3}]]""", Codes.Type, "root.ImageSequenceList[0][0].Library")]
    public void AMalformedImageSequenceListIsRefused(string json, string code, string path) =>
        Refused("MapObjectResource", "ImageSequenceList", json, code, path);

    [Fact]
    public void TheTreeStoresEachResourceShapeOnBothBackends()
    {
        var resource = new ObjectNode(null, null);
        resource.Add("TerrainList", new PairListNode([(1, -2), (-128, 127)]));
        resource.Add("ImageLibrary", new BytesNode(Library()));
        resource.Add("ImageSequenceList", Node("MapObjectResource", "ImageSequenceList", Sequences(nil: 1, next: 1)));
        var document = BothBackends("MapObjectResource", resource);
        Assert.Equal("[[1,-2],[-128,127]]", Compact(document["TerrainList"]!));
        Assert.Equal(SequencesText, Compact(document["ImageSequenceList"]!));

        var flood = new ObjectNode(null, null);
        flood.Add("Width", new IntegerNode(4, true, 2));
        flood.Add("Height", new IntegerNode(4, true, 1));
        flood.Add("OriginalTerrain", new BytesNode([0, 0, 3, 0xFE]));
        Assert.Equal("""[{"X":1,"Y":0,"Terrain":3,"Overlay":-2}]""", Compact(BothBackends("FloodControl", flood)["OriginalTerrain"]!));

        foreach (var (bits, text) in new[] { (0x3DCCCCCD, "0.1"), (int.MinValue, """{"$bits":"00000080"}"""), (0x7FC00001, """{"$bits":"0100c07f"}""") })
        {
            var control = new ObjectNode(null, null);
            control.Add("DefenseStrengthMultiplier", new SingleNode(bits));
            Assert.Equal(text, Compact(BothBackends("AIPlayerControl", control)["DefenseStrengthMultiplier"]!));
        }
    }

    [Fact]
    public void TheTreeRefusesAResourceThatBreaksItsShape()
    {
        var library = new ObjectNode(null, null);
        library.Add("ImageLibrary", new BytesNode([1, 2, 3]));
        var classValue = s_schema.Get("MapObjectResource");
        Assert.Equal(Codes.Corrupt, Assert.Throws<ConversionException>(() => OriginalBackend.Write(library, classValue, s_schema, "game")).Diagnostic.Code);
        Assert.Equal(Codes.Corrupt, Assert.Throws<ConversionException>(() => DocumentBackend.Write(library, classValue, s_schema, "game")).Diagnostic.Code);
        var flood = new ObjectNode(null, null);
        flood.Add("Width", new IntegerNode(4, true, 2));
        flood.Add("Height", new IntegerNode(4, true, 2));
        flood.Add("OriginalTerrain", new BytesNode([0, 0, 3, 0xFE]));
        var floodClass = s_schema.Get("FloodControl");
        Assert.Equal(Codes.Dimensions, Assert.Throws<ConversionException>(() => OriginalBackend.Write(flood, floodClass, s_schema, "game")).Diagnostic.Code);
        Assert.Equal(Codes.Dimensions, Assert.Throws<ConversionException>(() => DocumentBackend.Write(flood, floodClass, s_schema, "game")).Diagnostic.Code);
        var layers = new ObjectNode(null, null);
        layers.Add("ImageSequenceList", new ImageSequenceListNode([[new SequenceLayer { Mode = 4 }]]));
        var error = Assert.Throws<ConversionException>(() => OriginalBackend.Write(layers, classValue, s_schema, "game"));
        Assert.Equal("root.ImageSequenceList[0][0]", error.Diagnostic.Path);
    }

    // A unit index list collection is the flat list on the tree. The document cuts it into groups.
    [Fact]
    public void AGroupedListIsItsFlatListOnTheTree()
    {
        var resource = new ObjectNode(null, null);
        resource.Add("Defenders", new DefaultListNode(4, -1, [198, 201, 199, -1, -1, -1, -1, -1, 203, 200]));
        Assert.Equal("""{"groups":[[198,201,199],[203,200]],"count":10}""", Compact(BothBackends("StructureResource", resource)["Defenders"]!));
        var bytes = new ObjectNode(null, null);
        bytes.Add("Defenders", new DefaultListNode(1, 0, []));
        var error = Assert.Throws<ConversionException>(() => DocumentBackend.Write(bytes, s_schema.Get("StructureResource"), s_schema, "game"));
        Assert.Equal(Codes.Type, error.Diagnostic.Code);
        Assert.Equal("root.Defenders", error.Diagnostic.Path);
    }

    [Fact]
    public void FixedObjectsKeepTheirSlots()
    {
        var holder = new AbilitySlots();
        holder.Abilities[0].SkillPointCost = 5;
        holder.Abilities[3].SkillPointCost = -1;
        var tree = TreeWriter.Write(holder, s_factory);
        var document = BothBackends("AbilityControl", tree);
        Assert.Equal("""[{"$slot":0,"SkillPointCost":5},{"$slot":3,"SkillPointCost":-1}]""", Compact(document["Abilities"]!));

        // A slot no object asks for stays unread, and an object whose slot is absent keeps its state.
        var loaded = new AbilitySlots(slots: [3, 7]);
        loaded.Abilities[7].SkillPointCost = 9;
        TreeReader.Read(DocumentBackend.Read(document, s_schema.Get("AbilityControl"), s_schema, "game"), loaded, s_factory);
        Assert.Equal(-1, loaded.Abilities[3].SkillPointCost);
        Assert.Equal(9, loaded.Abilities[7].SkillPointCost);

        var spells = new ObjectNode(null, null);
        var list = new SlotListNode();
        var spell = new ObjectNode(null, 2);
        spell.Add("BaseManaCost", new IntegerNode(4, true, 30));
        list.Add(2, spell);
        spells.Add("RegisteredSpells", list);
        Assert.Equal("""[{"$slot":2,"BaseManaCost":30}]""", Compact(BothBackends("SpellControl", spells)["RegisteredSpells"]!));
    }

    [Fact]
    public void AMalformedFixedSlotIsRefused()
    {
        var classValue = s_schema.Get("AbilityControl");
        JsonObject Root(string abilities) => new JsonObject().Add("Abilities", JsonReader.Parse(abilities));
        var missing = Assert.Throws<ConversionException>(() => PilotImage.Encode(Root("""[{"SkillPointCost":1}]"""), "AbilityControl"));
        Assert.Equal(Codes.MissingMember, missing.Diagnostic.Code);
        Assert.Equal("root.Abilities[0]", missing.Diagnostic.Path);
        var twice = Assert.Throws<ConversionException>(() => PilotImage.Encode(Root("""[{"$slot":1},{"$slot":1}]"""), "AbilityControl"));
        Assert.Equal(Codes.Slot, twice.Diagnostic.Code);
        var negative = Assert.Throws<ConversionException>(() => DocumentBackend.Read(Root("""[{"$slot":-1}]"""), classValue, s_schema, "game"));
        Assert.Equal(Codes.Range, negative.Diagnostic.Code);
        Assert.Equal("root.Abilities[0].$slot", negative.Diagnostic.Path);
        var classed = Assert.Throws<ConversionException>(() => DocumentBackend.Read(Root("""[{"$slot":0,"$class":"Ability"}]"""), classValue, s_schema, "game"));
        Assert.Equal(Codes.ReservedMember, classed.Diagnostic.Code);
    }

    // The first entry chains a FrameTable layer to a plain layer, the second is nil, and the third
    // holds a layer of the undeclared mode 9.
    private const string SequencesText =
        """[[{"Library":"Units","Layer":1,"Speed":2,"Mode":"FrameTable","ImageIndex":7,"OffsetX":-3,"OffsetY":5,"Frames":{"default":0,"items":[3,5,7]}},{}],null,[{"Library":"fx","Layer":2,"Mode":9,"ImageIndex":-1}]]""";

    private static byte[] Sequences(byte nil, byte next)
    {
        var data = new ByteBuffer();
        data.Int32(3);
        data.Byte(0);
        Layer(data, library: "Units", layer: 1, speed: 2, mode: 4, imageIndex: 7, offsetX: -3, offsetY: 5);
        data.Int32(0);
        data.Int32(3);
        data.Int32(3);
        data.Int32(5);
        data.Int32(7);
        data.Byte(next);
        Layer(data, library: "", layer: 0, speed: 0, mode: 0, imageIndex: 0, offsetX: 0, offsetY: 0);
        data.Byte(0);
        data.Byte(nil);
        data.Byte(0);
        Layer(data, library: "fx", layer: 2, speed: 0, mode: 9, imageIndex: -1, offsetX: 0, offsetY: 0);
        data.Byte(0);
        return data.ToArray();
    }

    private static void Layer(ByteBuffer data, string library, byte layer, byte speed, byte mode, int imageIndex, int offsetX, int offsetY)
    {
        data.Int32(library.Length);
        data.Bytes(Encoding.ASCII.GetBytes(library));
        data.Byte(layer);
        data.Byte(speed);
        data.Byte(mode);
        data.Int32(imageIndex);
        data.Int32(offsetX);
        data.Int32(offsetY);
    }

    // The smallest library the writer makes: the header with three bytes of stale memory, no palette
    // and the index terminator.
    private static byte[] Library()
    {
        var bytes = new byte[32];
        "\u0004ILB\0\u0016PU"u8.CopyTo(bytes);
        BinaryPrimitives.WriteSingleLittleEndian(bytes.AsSpan(8), 4f);
        BinaryPrimitives.WriteInt32LittleEndian(bytes.AsSpan(12), 24);
        BinaryPrimitives.WriteInt32LittleEndian(bytes.AsSpan(16), 32);
        BinaryPrimitives.WriteInt32LittleEndian(bytes.AsSpan(20), 32);
        BinaryPrimitives.WriteInt32LittleEndian(bytes.AsSpan(28), -1);
        return bytes;
    }

    // The tree value the direct path reads from the bytes of one property.
    private static TreeValue Node(string className, string property, byte[] value)
    {
        var prop = Property(className, property);
        var context = new DecodeContext(value, s_schema, "game", "game", null, FormatLimits.Default, new LayoutCounts());
        return prop.Codec.ReadImage(context, 0, value.Length, new ImageFrame { Property = prop, Id = prop.Id ?? 0, Path = DocumentNodePath.Root.Member(property) });
    }

    // The tree written through both backends: the document is the converter's decode of the image,
    // and the image comes back from the image read and from the document read.
    private static JsonObject BothBackends(string className, ObjectNode node)
    {
        var classValue = s_schema.Get(className);
        var image = OriginalBackend.Write(node, classValue, s_schema, "game");
        var document = DocumentBackend.Write(node, classValue, s_schema, "game");
        Assert.Equal(JsonWriter.ToUtf8(PilotImage.Decode(image, className)), JsonWriter.ToUtf8(document));
        Assert.Equal(image, PilotImage.Encode(document, className));
        Assert.Equal(image, OriginalBackend.Write(OriginalBackend.Read(image, classValue, s_schema, "game"), classValue, s_schema, "game"));
        Assert.Equal(image, OriginalBackend.Write(DocumentBackend.Read(document, classValue, s_schema, "game"), classValue, s_schema, "game"));
        return document;
    }

    private static JsonObject Grid(int width, int height) =>
        new JsonObject().Add("Width", new JsonInteger(width)).Add("Height", new JsonInteger(height));

    private static SchemaProperty Property(string className, string property) =>
        s_schema.Get(className).PropertyByName(property) ?? throw new InvalidOperationException(className + " has no " + property + ".");

    private static JsonValue Decode(string className, string property, byte[] bytes, JsonObject? owner = null)
    {
        var prop = Property(className, property);
        var context = new DecodeContext(bytes, s_schema, "game", "world-hsm", "resources.json", FormatLimits.Default, new LayoutCounts());
        var builder = new LayoutBuilder();
        var site = new ValueSite { Property = prop, Id = prop.Id ?? 0, Path = DocumentNodePath.Root.Member(property), Owner = owner ?? new JsonObject(), Builder = builder };
        var value = prop.Codec.Decode(context, 0, bytes.Length, site);
        Assert.Empty(builder.Parts);
        return value;
    }

    private static byte[] Encode(string className, string property, JsonValue value, JsonObject? owner = null)
    {
        var prop = Property(className, property);
        var context = new EncodeContext(s_schema, "game", "world-hsm", "resources.json", FormatLimits.Default, ExportOptions.Default);
        var data = new ByteBuffer();
        prop.Codec.Encode(context, value, data, new ValueSite { Property = prop, Id = prop.Id ?? 0, Path = DocumentNodePath.Root.Member(property), Owner = owner ?? new JsonObject() });
        return data.ToArray();
    }

    private static void Refused(string className, string property, string json, string code, string path)
    {
        var error = Assert.Throws<ConversionException>(() => Encode(className, property, JsonReader.Parse(json)));
        Assert.Equal(code, error.Diagnostic.Code);
        Assert.Equal(path, error.Diagnostic.Path);
    }

    // The compact writer separates members with a space. None of these values holds text with one.
    private static string Compact(JsonValue value)
    {
        var text = new StringBuilder();
        JsonWriter.WriteCompact(text, value);
        return text.ToString().Replace(": ", ":", StringComparison.Ordinal).Replace(", ", ",", StringComparison.Ordinal);
    }

    private static readonly NoFactory s_factory = new();

    private sealed class NoFactory : IPersistableFactory
    {
        public string ClassName(IPersistable objectValue) => throw new InvalidOperationException("A fixed object has no class name.");

        public IPersistable Create(string className) => throw new InvalidOperationException("The test creates no object.");
    }

    // Abilities registered at the given slots. Each persists the one member it stores.
    private sealed class AbilitySlots : IPersistable
    {
        public AbilitySlots(int[]? slots = null)
        {
            foreach (int slot in slots ?? [0, 3]) Abilities[slot] = new Ability();
        }

        public Dictionary<int, Ability> Abilities { get; } = [];

        public void Persist(IPersister persister)
        {
            foreach (var (slot, ability) in Abilities.OrderBy(pair => pair.Key))
                persister.ObjectSlot("Abilities", slot: slot, objectValue: ability);
            persister.Skip("Abilities");
        }

        public sealed class Ability : IPersistable
        {
            public int SkillPointCost { get; set; }

            public void Persist(IPersister persister) => SkillPointCost = persister.Int32("SkillPointCost", SkillPointCost);
        }
    }
}
