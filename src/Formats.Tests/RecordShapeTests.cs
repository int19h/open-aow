using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Aow.Formats.Json;
using Aow.Formats.Migrations;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Rtl;

namespace Aow.Formats.Tests;

/// <summary>
/// Version 3 stores a record as named fields, a bit array as set-bit indices and a raw primitive
/// as an integer or a boolean. Residue is lowercase hex. The direct image path round-trips each shape.
/// </summary>
public sealed class RecordShapeTests : FormatsTest
{
    private const string SampleSchema = """
        {"version": 3, "classes": [
          {"name": "TEObject", "id": 80, "properties": []},
          {"name": "THolder", "parent": "TEObject", "id": 7, "engines": ["game"], "properties": [
            {"name": "Behavior", "id": 1, "codec": "record", "record": "TSampleBehavior", "size": 8},
            {"name": "Marks", "id": 2, "codec": "record", "record": "TSamplePacked", "size": 1},
            {"name": "AbCount", "id": 3, "codec": "i32"},
            {"name": "Bits", "id": 4, "codec": "bit-array", "count": "AbCount"},
            {"name": "Raw", "id": 5, "codec": "u8", "raw": true},
            {"name": "Signed", "id": 6, "codec": "i8", "raw": true},
            {"name": "Wide", "id": 7, "codec": "i32", "raw": true},
            {"name": "Flag", "id": 8, "codec": "raw-bool"}
          ]},
          {"name": "TPlain", "parent": "TEObject", "id": 8, "engines": ["game"], "properties": [
            {"name": "Pair", "id": 1, "codec": "record", "fields": [{"name": "A", "codec": "i32"}]}
          ]}
        ]}
        """;

    private static FormatSchema Sample => FormatSchema.Load(Encoding.UTF8.GetBytes(SampleSchema), "records.json");

    [Fact]
    public void ARecordABitArrayAndRawValuesRoundTrip()
    {
        var schema = Sample;
        var cls = schema.Get("THolder");
        var source = new Holder
        {
            Behavior = new TSampleBehavior { PlayerType = 2, Flags = 9 },
            Marks = new TSamplePacked { Value = TSet<TSampleRank>.FromMask(0x81) },
            AbCount = 4,
            Bits = [0x81],
            Raw = 0,
            Signed = -1,
            Wide = 0,
            Flag = false,
        };
        Poke(ref source.Behavior, 1, 1);
        Poke(ref source.Behavior, 3, 2);
        var node = TreeWriter.Write(source, Factory);
        var document = DocumentBackend.Write(node, cls, schema, "game");
        var behavior = (JsonObject)document["Behavior"]!;
        Assert.Equal(["PlayerType", "$reserved", "Flags"], behavior.Members.Select(m => m.Name).ToList());
        Assert.Equal(2, ((JsonInteger)behavior["PlayerType"]!).Value);
        Assert.Equal("010002", ((JsonString)behavior["$reserved"]!).Value);
        Assert.Equal(9, ((JsonInteger)behavior["Flags"]!).Value);
        Assert.Equal(["srNone", 7L], Items(document["Marks"]!));
        Assert.Equal(4, ((JsonInteger)document["AbCount"]!).Value);
        var bits = (JsonObject)document["Bits"]!;
        Assert.Equal([0L], Items(bits["values"]!));
        Assert.Equal("80", ((JsonString)bits["$reserved"]!).Value);
        Assert.Equal(0, ((JsonInteger)document["Raw"]!).Value);
        Assert.Equal(-1, ((JsonInteger)document["Signed"]!).Value);
        Assert.Equal(0, ((JsonInteger)document["Wide"]!).Value);
        Assert.False(((JsonBool)document["Flag"]!).Value);

        var image = OriginalBackend.Write(node, cls, schema, "game");
        var back = DocumentBackend.Write(OriginalBackend.Read(image, cls, schema, "game"), cls, schema, "game");
        Assert.Equal(JsonWriter.ToUtf8(document), JsonWriter.ToUtf8(back));
        var loaded = new Holder();
        TreeReader.Read(OriginalBackend.Read(image, cls, schema, "game"), loaded, Factory);
        Assert.Equal(2, loaded.Behavior.PlayerType);
        Assert.Equal(9, loaded.Behavior.Flags);
        Assert.Equal(1, At(loaded.Behavior, 1));
        Assert.Equal(0, At(loaded.Behavior, 2));
        Assert.Equal(2, At(loaded.Behavior, 3));
        Assert.Equal(0x81, loaded.Marks.Value__image);
        Assert.Equal(4, loaded.AbCount);
        Assert.Equal(0x81, loaded.Bits[0]);
        Assert.Equal(0, loaded.Raw);
        Assert.Equal(-1, loaded.Signed);
        Assert.Equal(0, loaded.Wide);
        Assert.False(loaded.Flag);
    }

    [Fact]
    public void ZeroResidueAndAZeroBitArrayStayExact()
    {
        var schema = Sample;
        var cls = schema.Get("THolder");
        var source = new Holder { AbCount = 8, Bits = new byte[1], Marks = new TSamplePacked { Value = TSet<TSampleRank>.FromMask(0) } };
        var document = DocumentBackend.Write(TreeWriter.Write(source, Factory), cls, schema, "game");
        Assert.Empty(((JsonObject)document["Behavior"]!).Members);
        Assert.Empty(((JsonArray)document["Marks"]!).Items);
        Assert.Empty(((JsonArray)document["Bits"]!).Items);
        Assert.False(document["Bits"] is JsonObject);
        var image = OriginalBackend.Write(TreeWriter.Write(source, Factory), cls, schema, "game");
        var loaded = new Holder();
        TreeReader.Read(OriginalBackend.Read(image, cls, schema, "game"), loaded, Factory);
        Assert.Equal(0, loaded.Behavior.PlayerType);
        Assert.Equal(0, loaded.Behavior.Flags);
        Assert.Equal(8, loaded.AbCount);
        Assert.Equal(0, loaded.Bits[0]);
        Assert.Equal(0, loaded.Raw);
        Assert.False(loaded.Flag);
    }

    [Fact]
    public void AMissingRecordFieldReadsAsZero()
    {
        var schema = Sample;
        var document = new JsonObject().Add("Behavior", new JsonObject().Add("PlayerType", new JsonInteger(2)));
        var loaded = new Holder { Behavior = new TSampleBehavior { PlayerType = 9, Flags = 9 } };
        Poke(ref loaded.Behavior, 1, 7);
        TreeReader.Read(DocumentBackend.Read(document, schema.Get("THolder"), schema, "game"), loaded, Factory);
        Assert.Equal(2, loaded.Behavior.PlayerType);
        Assert.Equal(0, loaded.Behavior.Flags);
        Assert.Equal(0, At(loaded.Behavior, 1));
    }

    [Fact]
    public void ARecordFieldOfTheWrongTypeIsRefused()
    {
        var error = Refuse(new JsonObject().Add("Behavior", new JsonObject().Add("PlayerType", new JsonString("no"))));
        Assert.Equal(Codes.Type, error.Diagnostic.Code);
        Assert.Equal("root.Behavior.PlayerType", error.Diagnostic.Path);
    }

    [Fact]
    public void AnUnknownRecordFieldIsRefused()
    {
        var error = Refuse(new JsonObject().Add("Behavior", new JsonObject().Add("Extra", new JsonInteger(1))));
        Assert.Equal(Codes.UnknownMember, error.Diagnostic.Code);
        Assert.Equal("root.Behavior.Extra", error.Diagnostic.Path);
        Assert.Equal("Extra", error.Diagnostic.Value);
    }

    [Fact]
    public void AShortResidueIsRefused()
    {
        var error = Refuse(new JsonObject().Add("Behavior", new JsonObject().Add("$reserved", new JsonString("01"))));
        Assert.Equal(Codes.Range, error.Diagnostic.Code);
        Assert.Equal("root.Behavior.$reserved", error.Diagnostic.Path);
    }

    [Fact]
    public void BitIndicesOutOfOrderPastTheCountOrRepeatedAreRefused()
    {
        var duplicate = Refuse(Bits(4, 1, 1));
        Assert.Equal(Codes.Duplicate, duplicate.Diagnostic.Code);
        Assert.Equal("root.Bits[1]", duplicate.Diagnostic.Path);
        var unsorted = Refuse(Bits(4, 1, 0));
        Assert.Equal(Codes.Range, unsorted.Diagnostic.Code);
        Assert.Equal("root.Bits[1]", unsorted.Diagnostic.Path);
        var atCount = Refuse(Bits(4, 4));
        Assert.Equal(Codes.Range, atCount.Diagnostic.Code);
        Assert.Equal("root.Bits[0]", atCount.Diagnostic.Path);
        var beyond = Refuse(Bits(4, 5));
        Assert.Equal(Codes.Range, beyond.Diagnostic.Code);
        Assert.Equal("root.Bits[0]", beyond.Diagnostic.Path);
    }

    [Fact]
    public void AnOutOfRangePrimitiveIsRefused()
    {
        var raw = Refuse(new JsonObject().Add("Raw", new JsonInteger(256)));
        Assert.Equal(Codes.Range, raw.Diagnostic.Code);
        Assert.Equal("root.Raw", raw.Diagnostic.Path);
        var signed = Refuse(new JsonObject().Add("Signed", new JsonInteger(-129)));
        Assert.Equal(Codes.Range, signed.Diagnostic.Code);
        Assert.Equal("root.Signed", signed.Diagnostic.Path);
        var wide = Refuse(new JsonObject().Add("Wide", new JsonInteger((long)int.MaxValue + 1)));
        Assert.Equal(Codes.Range, wide.Diagnostic.Code);
        Assert.Equal("root.Wide", wide.Diagnostic.Path);
        var flag = Refuse(new JsonObject().Add("Flag", new JsonInteger(256)));
        Assert.Equal(Codes.Range, flag.Diagnostic.Code);
        Assert.Equal("root.Flag", flag.Diagnostic.Path);
    }

    [Fact]
    public void TheConverterRefusesTheSameShapes()
    {
        var wrong = Converter(new JsonObject().Add("Behavior", new JsonObject().Add("PlayerType", new JsonString("no"))));
        Assert.Equal(Codes.Type, wrong.Diagnostic.Code);
        Assert.Equal("root.Behavior.PlayerType", wrong.Diagnostic.Path);
        var extra = Converter(new JsonObject().Add("Behavior", new JsonObject().Add("Extra", new JsonInteger(1))));
        Assert.Equal(Codes.UnknownMember, extra.Diagnostic.Code);
        Assert.Equal("root.Behavior.Extra", extra.Diagnostic.Path);
        var unsorted = Converter(Bits(4, 1, 0));
        Assert.Equal(Codes.Range, unsorted.Diagnostic.Code);
        Assert.Equal("root.Bits[1]", unsorted.Diagnostic.Path);
        var duplicate = Converter(Bits(4, 1, 1));
        Assert.Equal(Codes.Duplicate, duplicate.Diagnostic.Code);
        Assert.Equal("root.Bits[1]", duplicate.Diagnostic.Path);
        var atCount = Converter(Bits(4, 4));
        Assert.Equal(Codes.Range, atCount.Diagnostic.Code);
        var beyond = Converter(Bits(4, 5));
        Assert.Equal(Codes.Range, beyond.Diagnostic.Code);
        var raw = Converter(new JsonObject().Add("Raw", new JsonInteger(256)));
        Assert.Equal(Codes.Range, raw.Diagnostic.Code);
        Assert.Equal("root.Raw", raw.Diagnostic.Path);

        var schema = Sample;
        var cls = schema.Get("THolder");
        var behavior = cls.PropertyByName("Behavior")!;
        var shortRecord = new DecodeContext(new byte[7], schema, "game", "game", null, FormatLimits.Default, new LayoutCounts());
        var recordSite = new ValueSite { Property = behavior, Id = behavior.Id ?? 0, Path = DocPath.Root.Member("Behavior"), Owner = new JsonObject() };
        var size = Assert.Throws<ConversionException>(() => behavior.Codec.Decode(shortRecord, 0, 7, recordSite));
        Assert.Equal(Codes.Corrupt, size.Diagnostic.Code);
        var bits = cls.PropertyByName("Bits")!;
        var owner = new JsonObject().Add("AbCount", new JsonInteger(8));
        var shortBits = new DecodeContext(new byte[2], schema, "game", "game", null, FormatLimits.Default, new LayoutCounts());
        var bitSite = new ValueSite { Property = bits, Id = bits.Id ?? 0, Path = DocPath.Root.Member("Bits"), Owner = owner };
        var length = Assert.Throws<ConversionException>(() => bits.Codec.Decode(shortBits, 0, 2, bitSite));
        Assert.Equal(Codes.Range, length.Diagnostic.Code);
    }

    [Fact]
    public void AMalformedReservedIsRefused()
    {
        var number = Refuse(new JsonObject().Add("Behavior", new JsonObject().Add("$reserved", new JsonInteger(1))));
        Assert.Equal(Codes.Type, number.Diagnostic.Code);
        Assert.Equal("root.Behavior.$reserved", number.Diagnostic.Path);
        var upper = Refuse(new JsonObject().Add("Behavior", new JsonObject().Add("$reserved", new JsonString("01000A"))));
        Assert.Equal(Codes.Type, upper.Diagnostic.Code);
        Assert.Equal("root.Behavior.$reserved", upper.Diagnostic.Path);
        var packed = Converter(new JsonObject().Add("Behavior", new JsonObject().Add("$reserved", new JsonInteger(1))));
        Assert.Equal(Codes.Type, packed.Diagnostic.Code);
        Assert.Equal("root.Behavior.$reserved", packed.Diagnostic.Path);

        var schema = Sample;
        var node = new ObjectNode(null, null);
        var behavior = new RecordNode();
        behavior.Add("$reserved", new IntegerNode(1, false, 1));
        node.Add("Behavior", behavior);
        var image = Assert.Throws<ConversionException>(() => OriginalBackend.Write(node, schema.Get("THolder"), schema, "game"));
        Assert.Equal(Codes.Type, image.Diagnostic.Code);
        Assert.Equal("root.Behavior.$reserved", image.Diagnostic.Path);
        var document = Assert.Throws<ConversionException>(() => DocumentBackend.Write(node, schema.Get("THolder"), schema, "game"));
        Assert.Equal(Codes.Type, document.Diagnostic.Code);
        Assert.Equal("root.Behavior.$reserved", document.Diagnostic.Path);

        var shortNode = new ObjectNode(null, null);
        var shortRecord = new RecordNode();
        shortRecord.Add("PlayerType", new IntegerNode(1, false, 2));
        shortRecord.Add("$reserved", new BytesNode([1]));
        shortNode.Add("Behavior", shortRecord);
        var shortImage = Assert.Throws<ConversionException>(() => OriginalBackend.Write(shortNode, schema.Get("THolder"), schema, "game"));
        Assert.Equal(Codes.Range, shortImage.Diagnostic.Code);
        Assert.Equal("root.Behavior.$reserved", shortImage.Diagnostic.Path);
        var shortDocument = Assert.Throws<ConversionException>(() => DocumentBackend.Write(shortNode, schema.Get("THolder"), schema, "game"));
        Assert.Equal(Codes.Range, shortDocument.Diagnostic.Code);
        Assert.Equal("root.Behavior.$reserved", shortDocument.Diagnostic.Path);
    }

    [Fact]
    public void ARawBooleanKeepsAByteOtherThanZeroOrOne()
    {
        var schema = Sample;
        var cls = schema.Get("THolder");
        var node = new ObjectNode(null, null);
        node.Add("Flag", new IntegerNode(1, false, 2));
        var document = DocumentBackend.Write(node, cls, schema, "game");
        Assert.Equal(2, ((JsonInteger)document["Flag"]!).Value);
        var image = OriginalBackend.Write(node, cls, schema, "game");
        var back = OriginalBackend.Read(image, cls, schema, "game");
        Assert.Equal(2, ((IntegerNode)back.Find("Flag")!).Value);

        var source = new Holder();
        Unsafe.As<bool, byte>(ref source.Flag) = 2;
        var stored = TreeWriter.Write(source, Factory);
        Assert.Equal(2, ((IntegerNode)stored.Find("Flag")!).Value);
        var loaded = new Holder();
        TreeReader.Read(stored, loaded, Factory);
        Assert.Equal(2, Unsafe.As<bool, byte>(ref loaded.Flag));
        var round = OriginalBackend.Read(OriginalBackend.Write(stored, cls, schema, "game"), cls, schema, "game");
        var again = new Holder();
        TreeReader.Read(round, again, Factory);
        Assert.Equal(2, Unsafe.As<bool, byte>(ref again.Flag));
    }

    [Fact]
    public void EveryIntegerWidthTransfers()
    {
        var source = new WidthHolder
        {
            Value = new TEveryWidth { U8 = 255, I8 = sbyte.MinValue, U16 = ushort.MaxValue, I16 = short.MinValue, U32 = uint.MaxValue, I32 = int.MinValue },
        };
        var node = TreeWriter.Write(source, Factory);
        var record = (RecordNode)node.Find("Value")!;
        Assert.Equal(255, ((IntegerNode)record.Find("U8")!).Value);
        Assert.Equal(sbyte.MinValue, ((IntegerNode)record.Find("I8")!).Value);
        Assert.Equal(ushort.MaxValue, ((IntegerNode)record.Find("U16")!).Value);
        Assert.Equal(short.MinValue, ((IntegerNode)record.Find("I16")!).Value);
        Assert.Equal(uint.MaxValue, ((IntegerNode)record.Find("U32")!).Value);
        Assert.Equal(int.MinValue, ((IntegerNode)record.Find("I32")!).Value);
        var loaded = new WidthHolder();
        TreeReader.Read(node, loaded, Factory);
        Assert.Equal(source.Value.U8, loaded.Value.U8);
        Assert.Equal(source.Value.I8, loaded.Value.I8);
        Assert.Equal(source.Value.U16, loaded.Value.U16);
        Assert.Equal(source.Value.I16, loaded.Value.I16);
        Assert.Equal(source.Value.U32, loaded.Value.U32);
        Assert.Equal(source.Value.I32, loaded.Value.I32);
    }

    [Fact]
    public void AnExplicitRecordHasNoImage()
    {
        var schema = Sample;
        var node = new ObjectNode(null, null);
        var pair = new RecordNode();
        pair.Add("A", new IntegerNode(4, true, 1));
        node.Add("Pair", pair);
        var error = Assert.Throws<ConversionException>(() => OriginalBackend.Write(node, schema.Get("TPlain"), schema, "game"));
        Assert.Equal(Codes.Type, error.Diagnostic.Code);
        Assert.Equal("root.Pair", error.Diagnostic.Path);
        Assert.Contains("A record has no image.", error.Diagnostic.Message);
    }

    [Fact]
    public void ProductRecordsAndTheAbilityBitsRoundTrip()
    {
        var schema = FormatSchema.Current;
        var rect = new RecordNode();
        rect.Add("Left", new IntegerNode(4, true, int.MinValue));
        rect.Add("Bottom", new IntegerNode(4, true, 4));
        var animation = new ObjectNode(null, null);
        animation.Add("Dimensions", rect);
        var animationDocument = RoundTrip(schema.Get("TAnimation"), schema, animation);
        var dimensions = (JsonObject)animationDocument["Dimensions"]!;
        Assert.Equal(["Left", "Bottom"], dimensions.Members.Select(m => m.Name).ToList());
        Assert.Equal(int.MinValue, ((JsonInteger)dimensions["Left"]!).Value);

        var effects = new SetNode("TDamageType", 2, (1UL << 9) | (1UL << 15));
        var damage = new ObjectNode(null, null);
        damage.Add("Effects", effects);
        var damageDocument = RoundTrip(schema.Get("TDamageCA"), schema, damage);
        Assert.Equal(["dtNone", 15L], Items(damageDocument["Effects"]!));

        var behavior = new RecordNode();
        behavior.Add("PlayerType", new IntegerNode(1, false, 2));
        behavior.Add("$reserved", new BytesNode([1, 0, 2]));
        behavior.Add("Flags", new IntegerNode(4, true, 9));
        var player = new ObjectNode(null, null);
        player.Add("Behavior", behavior);
        var playerDocument = RoundTrip(schema.Get("TPlayerSetupSettings"), schema, player);
        var stored = (JsonObject)playerDocument["Behavior"]!;
        Assert.Equal(["PlayerType", "$reserved", "Flags"], stored.Members.Select(m => m.Name).ToList());
        Assert.Equal("010002", ((JsonString)stored["$reserved"]!).Value);

        var list = new ObjectNode(null, null);
        list.Add("AbCount", new IntegerNode(4, true, 1));
        list.Add("AbilityCount", new BitArrayNode(1, [], 0x80));
        var listDocument = RoundTrip(schema.Get("TCustomAbilityList"), schema, list);
        var ability = (JsonObject)listDocument["AbilityCount"]!;
        Assert.Empty(((JsonArray)ability["values"]!).Items);
        Assert.Equal("80", ((JsonString)ability["$reserved"]!).Value);

        var champion = new ObjectNode(null, null);
        champion.Add("Champion", new IntegerNode(1, false, 2));
        var championDocument = RoundTrip(schema.Get("TUnitResource"), schema, champion);
        Assert.Equal(2, ((JsonInteger)championDocument["Champion"]!).Value);
    }

    private static JsonObject RoundTrip(SchemaClass cls, FormatSchema schema, ObjectNode node)
    {
        var document = DocumentBackend.Write(node, cls, schema, "game");
        var image = OriginalBackend.Write(node, cls, schema, "game");
        var back = DocumentBackend.Write(OriginalBackend.Read(image, cls, schema, "game"), cls, schema, "game");
        Assert.Equal(JsonWriter.ToUtf8(document), JsonWriter.ToUtf8(back));
        return document;
    }

    private static ConversionException Converter(JsonObject document)
    {
        var schema = Sample;
        var ctx = new EncodeContext(schema, "game", "game", null, FormatLimits.Default, ExportOptions.Default);
        return Assert.Throws<ConversionException>(() => ObjectImage.Encode(ctx, document, schema.Get("THolder"), new ByteBuffer(), DocPath.Root));
    }

    private static ConversionException Refuse(JsonObject document)
    {
        var schema = Sample;
        return Assert.Throws<ConversionException>(() => OriginalBackend.Write(
            DocumentBackend.Read(document, schema.Get("THolder"), schema, "game"), schema.Get("THolder"), schema, "game"));
    }

    private static JsonObject Bits(int count, params int[] indices)
    {
        var array = new JsonArray();
        foreach (var index in indices) array.Add(new JsonInteger(index));
        return new JsonObject().Add("AbCount", new JsonInteger(count)).Add("Bits", array);
    }

    private static void Poke(ref TSampleBehavior value, int offset, byte stored) =>
        MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref value, 1))[offset] = stored;

    private static byte At(TSampleBehavior value, int offset) =>
        MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref value, 1))[offset];

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

    private static readonly NamedFactory Factory = new();

    private sealed class NamedFactory : IPersistableFactory
    {
        public string ClassName(IPersistable obj) => obj.GetType().Name;
        public IPersistable Create(string className) => throw new InvalidOperationException(className);
    }

    private sealed class Holder : IPersistable
    {
        public TSampleBehavior Behavior;
        public TSamplePacked Marks;
        public int AbCount;
        public byte[] Bits = [];
        public byte Raw;
        public sbyte Signed;
        public int Wide;
        public bool Flag;

        public void Persist(IPersister persister)
        {
            RecordValues.Transfer(persister, "Behavior", ref Behavior);
            RecordValues.Transfer(persister, "Marks", ref Marks);
            persister.Int("AbCount", ref AbCount);
            if (persister.Loading) Array.Resize(ref Bits, AbCount < 0 ? 0 : (AbCount + 7) >> 3);
            int length = AbCount < 0 ? 0 : (AbCount + 7) >> 3;
            persister.BitArray("Bits", AbCount, Bits.AsSpan(0, Math.Max(length, 0)));
            persister.RawByte("Raw", ref Raw);
            persister.RawSByte("Signed", ref Signed);
            persister.RawInt("Wide", ref Wide);
            persister.RawBool("Flag", ref Flag);
        }
    }

    private sealed class WidthHolder : IPersistable
    {
        public TEveryWidth Value;

        public void Persist(IPersister persister) => RecordValues.Transfer(persister, "Value", ref Value);
    }
}
