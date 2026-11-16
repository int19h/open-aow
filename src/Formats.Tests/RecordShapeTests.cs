using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Aow.Formats.Json;

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
          ]},
          {"name": "TWide", "parent": "TEObject", "id": 9, "engines": ["game"], "properties": [
            {"name": "Value", "id": 1, "codec": "record", "record": "TEveryWidth", "size": 14}
          ]}
        ]}
        """;

    private static FormatSchema Sample => FormatSchema.Load(Encoding.UTF8.GetBytes(SampleSchema), "records.json");

    [Fact]
    public void ARecordABitArrayAndRawValuesRoundTrip()
    {
        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("THolder");
        Holder source = new()
        {
            Behavior = new SampleBehavior { PlayerType = 2, Flags = 9 },
            Marks = new SamplePacked { Value = Set<SampleRank>.FromMask(0x81) },
            AbilityCount = 4,
            Bits = [0x81],
            Raw = 0,
            Signed = -1,
            Wide = 0,
            Flag = false,
        };
        SampleBehavior behavior2 = source.Behavior;
        Poke(ref behavior2, 1, 1);
        source.Behavior = behavior2;
        SampleBehavior behavior3 = source.Behavior;
        Poke(ref behavior3, 3, 2);
        source.Behavior = behavior3;
        ObjectNode node = TreeWriter.Write(source, s_factory);
        JsonObject document = DocumentBackend.Write(node, classValue, schema, "game");
        JsonObject behavior = (JsonObject)document["Behavior"]!;
        Assert.Equal(["PlayerType", "$reserved", "Flags"], behavior.Members.Select(m => m.Name).ToList());
        Assert.Equal(2, ((JsonInteger)behavior["PlayerType"]!).Value);
        Assert.Equal("010002", ((JsonString)behavior["$reserved"]!).Value);
        Assert.Equal(9, ((JsonInteger)behavior["Flags"]!).Value);
        Assert.Equal(["None", 7L], Items(document["Marks"]!));
        Assert.Equal(4, ((JsonInteger)document["AbCount"]!).Value);
        JsonObject bits = (JsonObject)document["Bits"]!;
        Assert.Equal([0L], Items(bits["values"]!));
        Assert.Equal("80", ((JsonString)bits["$reserved"]!).Value);
        Assert.Equal(0, ((JsonInteger)document["Raw"]!).Value);
        Assert.Equal(-1, ((JsonInteger)document["Signed"]!).Value);
        Assert.Equal(0, ((JsonInteger)document["Wide"]!).Value);
        Assert.False(((JsonBoolean)document["Flag"]!).Value);

        byte[] image = OriginalBackend.Write(node, classValue, schema, "game");
        JsonObject back = DocumentBackend.Write(OriginalBackend.Read(image, classValue, schema, "game"), classValue, schema, "game");
        Assert.Equal(JsonWriter.ToUtf8(document), JsonWriter.ToUtf8(back));
        Holder loaded = new();
        TreeReader.Read(OriginalBackend.Read(image, classValue, schema, "game"), loaded, s_factory);
        Assert.Equal(2, loaded.Behavior.PlayerType);
        Assert.Equal(9, loaded.Behavior.Flags);
        Assert.Equal(1, At(loaded.Behavior, 1));
        Assert.Equal(0, At(loaded.Behavior, 2));
        Assert.Equal(2, At(loaded.Behavior, 3));
        Assert.Equal(0x81, loaded.Marks.ValueImage);
        Assert.Equal(4, loaded.AbilityCount);
        Assert.Equal(0x81, loaded.Bits[0]);
        Assert.Equal(0, loaded.Raw);
        Assert.Equal(-1, loaded.Signed);
        Assert.Equal(0, loaded.Wide);
        Assert.False(loaded.Flag);
    }

    [Fact]
    public void ZeroResidueAndAZeroBitArrayStayExact()
    {
        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("THolder");
        Holder source = new() { AbilityCount = 8, Bits = new byte[1], Marks = new SamplePacked { Value = Set<SampleRank>.FromMask(0) } };
        JsonObject document = DocumentBackend.Write(TreeWriter.Write(source, s_factory), classValue, schema, "game");
        Assert.Empty(((JsonObject)document["Behavior"]!).Members);
        Assert.Empty(((JsonArray)document["Marks"]!).Items);
        Assert.Empty(((JsonArray)document["Bits"]!).Items);
        Assert.False(document["Bits"] is JsonObject);
        byte[] image = OriginalBackend.Write(TreeWriter.Write(source, s_factory), classValue, schema, "game");
        Holder loaded = new();
        TreeReader.Read(OriginalBackend.Read(image, classValue, schema, "game"), loaded, s_factory);
        Assert.Equal(0, loaded.Behavior.PlayerType);
        Assert.Equal(0, loaded.Behavior.Flags);
        Assert.Equal(8, loaded.AbilityCount);
        Assert.Equal(0, loaded.Bits[0]);
        Assert.Equal(0, loaded.Raw);
        Assert.False(loaded.Flag);
    }

    [Fact]
    public void AMissingRecordFieldReadsAsZero()
    {
        FormatSchema schema = Sample;
        JsonObject document = new JsonObject().Add("Behavior", new JsonObject().Add("PlayerType", new JsonInteger(2)));
        Holder loaded = new() { Behavior = new SampleBehavior { PlayerType = 9, Flags = 9 } };
        SampleBehavior behavior = loaded.Behavior;
        Poke(ref behavior, 1, 7);
        loaded.Behavior = behavior;
        TreeReader.Read(DocumentBackend.Read(document, schema.Get("THolder"), schema, "game"), loaded, s_factory);
        Assert.Equal(2, loaded.Behavior.PlayerType);
        Assert.Equal(0, loaded.Behavior.Flags);
        Assert.Equal(0, At(loaded.Behavior, 1));
    }

    [Fact]
    public void ARecordFieldOfTheWrongTypeIsRefused()
    {
        ConversionException error = Refuse(new JsonObject().Add("Behavior", new JsonObject().Add("PlayerType", new JsonString("no"))));
        Assert.Equal(Codes.Type, error.Diagnostic.Code);
        Assert.Equal("root.Behavior.PlayerType", error.Diagnostic.Path);
    }

    [Fact]
    public void AnUnknownRecordFieldIsRefused()
    {
        ConversionException error = Refuse(new JsonObject().Add("Behavior", new JsonObject().Add("Extra", new JsonInteger(1))));
        Assert.Equal(Codes.UnknownMember, error.Diagnostic.Code);
        Assert.Equal("root.Behavior.Extra", error.Diagnostic.Path);
        Assert.Equal("Extra", error.Diagnostic.Value);
    }

    [Fact]
    public void AShortResidueIsRefused()
    {
        ConversionException error = Refuse(new JsonObject().Add("Behavior", new JsonObject().Add("$reserved", new JsonString("01"))));
        Assert.Equal(Codes.Range, error.Diagnostic.Code);
        Assert.Equal("root.Behavior.$reserved", error.Diagnostic.Path);
    }

    [Fact]
    public void BitIndicesOutOfOrderPastTheCountOrRepeatedAreRefused()
    {
        ConversionException duplicate = Refuse(Bits(4, 1, 1));
        Assert.Equal(Codes.Duplicate, duplicate.Diagnostic.Code);
        Assert.Equal("root.Bits[1]", duplicate.Diagnostic.Path);
        ConversionException unsorted = Refuse(Bits(4, 1, 0));
        Assert.Equal(Codes.Range, unsorted.Diagnostic.Code);
        Assert.Equal("root.Bits[1]", unsorted.Diagnostic.Path);
        ConversionException atCount = Refuse(Bits(4, 4));
        Assert.Equal(Codes.Range, atCount.Diagnostic.Code);
        Assert.Equal("root.Bits[0]", atCount.Diagnostic.Path);
        ConversionException beyond = Refuse(Bits(4, 5));
        Assert.Equal(Codes.Range, beyond.Diagnostic.Code);
        Assert.Equal("root.Bits[0]", beyond.Diagnostic.Path);
    }

    [Fact]
    public void AnOutOfRangePrimitiveIsRefused()
    {
        ConversionException raw = Refuse(new JsonObject().Add("Raw", new JsonInteger(256)));
        Assert.Equal(Codes.Range, raw.Diagnostic.Code);
        Assert.Equal("root.Raw", raw.Diagnostic.Path);
        ConversionException signed = Refuse(new JsonObject().Add("Signed", new JsonInteger(-129)));
        Assert.Equal(Codes.Range, signed.Diagnostic.Code);
        Assert.Equal("root.Signed", signed.Diagnostic.Path);
        ConversionException wide = Refuse(new JsonObject().Add("Wide", new JsonInteger((long)int.MaxValue + 1)));
        Assert.Equal(Codes.Range, wide.Diagnostic.Code);
        Assert.Equal("root.Wide", wide.Diagnostic.Path);
        ConversionException flag = Refuse(new JsonObject().Add("Flag", new JsonInteger(256)));
        Assert.Equal(Codes.Range, flag.Diagnostic.Code);
        Assert.Equal("root.Flag", flag.Diagnostic.Path);
    }

    [Fact]
    public void TheConverterRefusesTheSameShapes()
    {
        ConversionException wrong = Converter(new JsonObject().Add("Behavior", new JsonObject().Add("PlayerType", new JsonString("no"))));
        Assert.Equal(Codes.Type, wrong.Diagnostic.Code);
        Assert.Equal("root.Behavior.PlayerType", wrong.Diagnostic.Path);
        ConversionException extra = Converter(new JsonObject().Add("Behavior", new JsonObject().Add("Extra", new JsonInteger(1))));
        Assert.Equal(Codes.UnknownMember, extra.Diagnostic.Code);
        Assert.Equal("root.Behavior.Extra", extra.Diagnostic.Path);
        ConversionException unsorted = Converter(Bits(4, 1, 0));
        Assert.Equal(Codes.Range, unsorted.Diagnostic.Code);
        Assert.Equal("root.Bits[1]", unsorted.Diagnostic.Path);
        ConversionException duplicate = Converter(Bits(4, 1, 1));
        Assert.Equal(Codes.Duplicate, duplicate.Diagnostic.Code);
        Assert.Equal("root.Bits[1]", duplicate.Diagnostic.Path);
        ConversionException atCount = Converter(Bits(4, 4));
        Assert.Equal(Codes.Range, atCount.Diagnostic.Code);
        ConversionException beyond = Converter(Bits(4, 5));
        Assert.Equal(Codes.Range, beyond.Diagnostic.Code);
        ConversionException raw = Converter(new JsonObject().Add("Raw", new JsonInteger(256)));
        Assert.Equal(Codes.Range, raw.Diagnostic.Code);
        Assert.Equal("root.Raw", raw.Diagnostic.Path);

        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("THolder");
        SchemaProperty behavior = classValue.PropertyByName("Behavior")!;
        DecodeContext shortRecord = new(new byte[7], schema, "game", "game", null, FormatLimits.Default, new LayoutCounts());
        ValueSite recordSite = new() { Property = behavior, Id = behavior.Id ?? 0, Path = DocumentNodePath.Root.Member("Behavior"), Owner = new JsonObject() };
        ConversionException size = Assert.Throws<ConversionException>(() => behavior.Codec.Decode(shortRecord, 0, 7, recordSite));
        Assert.Equal(Codes.Corrupt, size.Diagnostic.Code);
        SchemaProperty bits = classValue.PropertyByName("Bits")!;
        JsonObject owner = new JsonObject().Add("AbCount", new JsonInteger(8));
        DecodeContext shortBits = new(new byte[2], schema, "game", "game", null, FormatLimits.Default, new LayoutCounts());
        ValueSite bitSite = new() { Property = bits, Id = bits.Id ?? 0, Path = DocumentNodePath.Root.Member("Bits"), Owner = owner };
        ConversionException length = Assert.Throws<ConversionException>(() => bits.Codec.Decode(shortBits, 0, 2, bitSite));
        Assert.Equal(Codes.Range, length.Diagnostic.Code);
    }

    [Fact]
    public void AMalformedReservedIsRefused()
    {
        ConversionException number = Refuse(new JsonObject().Add("Behavior", new JsonObject().Add("$reserved", new JsonInteger(1))));
        Assert.Equal(Codes.Type, number.Diagnostic.Code);
        Assert.Equal("root.Behavior.$reserved", number.Diagnostic.Path);
        ConversionException upper = Refuse(new JsonObject().Add("Behavior", new JsonObject().Add("$reserved", new JsonString("01000A"))));
        Assert.Equal(Codes.Type, upper.Diagnostic.Code);
        Assert.Equal("root.Behavior.$reserved", upper.Diagnostic.Path);
        ConversionException packed = Converter(new JsonObject().Add("Behavior", new JsonObject().Add("$reserved", new JsonInteger(1))));
        Assert.Equal(Codes.Type, packed.Diagnostic.Code);
        Assert.Equal("root.Behavior.$reserved", packed.Diagnostic.Path);

        FormatSchema schema = Sample;
        ObjectNode node = new(null, null);
        RecordNode behavior = new();
        behavior.Add("$reserved", new IntegerNode(1, false, 1));
        node.Add("Behavior", behavior);
        ConversionException image = Assert.Throws<ConversionException>(() => OriginalBackend.Write(node, schema.Get("THolder"), schema, "game"));
        Assert.Equal(Codes.Type, image.Diagnostic.Code);
        Assert.Equal("root.Behavior.$reserved", image.Diagnostic.Path);
        ConversionException document = Assert.Throws<ConversionException>(() => DocumentBackend.Write(node, schema.Get("THolder"), schema, "game"));
        Assert.Equal(Codes.Type, document.Diagnostic.Code);
        Assert.Equal("root.Behavior.$reserved", document.Diagnostic.Path);

        ObjectNode shortNode = new(null, null);
        RecordNode shortRecord = new();
        shortRecord.Add("PlayerType", new IntegerNode(1, false, 2));
        shortRecord.Add("$reserved", new BytesNode([1]));
        shortNode.Add("Behavior", shortRecord);
        ConversionException shortImage = Assert.Throws<ConversionException>(() => OriginalBackend.Write(shortNode, schema.Get("THolder"), schema, "game"));
        Assert.Equal(Codes.Range, shortImage.Diagnostic.Code);
        Assert.Equal("root.Behavior.$reserved", shortImage.Diagnostic.Path);
        ConversionException shortDocument = Assert.Throws<ConversionException>(() => DocumentBackend.Write(shortNode, schema.Get("THolder"), schema, "game"));
        Assert.Equal(Codes.Range, shortDocument.Diagnostic.Code);
        Assert.Equal("root.Behavior.$reserved", shortDocument.Diagnostic.Path);
    }

    [Fact]
    public void ARawBooleanKeepsAByteOtherThanZeroOrOne()
    {
        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("THolder");
        ObjectNode node = new(null, null);
        node.Add("Flag", new IntegerNode(1, false, 2));
        JsonObject document = DocumentBackend.Write(node, classValue, schema, "game");
        Assert.Equal(2, ((JsonInteger)document["Flag"]!).Value);
        ObjectNode back = OriginalBackend.Read(OriginalBackend.Write(node, classValue, schema, "game"), classValue: classValue, schema: schema, engine: "game");
        Assert.Equal(2, ((IntegerNode)back.Find("Flag")!).Value);

        Holder source = new();
        bool flag = source.Flag;
        Unsafe.As<bool, byte>(ref flag) = 2;
        source.Flag = flag;
        ObjectNode stored = TreeWriter.Write(source, s_factory);
        Assert.Equal(2, ((IntegerNode)stored.Find("Flag")!).Value);
        Holder loaded = new();
        TreeReader.Read(stored, loaded, s_factory);
        bool flag2 = loaded.Flag;
        Assert.Equal(2, Unsafe.As<bool, byte>(ref flag2));
        loaded.Flag = flag2;
        ObjectNode round = OriginalBackend.Read(OriginalBackend.Write(stored, classValue, schema, "game"), classValue, schema, "game");
        Holder again = new();
        TreeReader.Read(round, again, s_factory);
        bool flag3 = again.Flag;
        Assert.Equal(2, Unsafe.As<bool, byte>(ref flag3));
        again.Flag = flag3;
    }

    [Fact]
    public void AnUnsigned32BitRecordFieldRoundTripsAboveTheSignedRange()
    {
        FormatSchema schema = Sample;
        SchemaClass classValue = schema.Get("TWide");
        SchemaProperty property = classValue.PropertyByName("Value")!;
        IntegerCodec codec = new(4, false);
        foreach (uint number in new uint[] { 0x80000000, 0xFFFFFFFF })
        {
            byte[] bytes = new byte[4];
            BinaryPrimitives.WriteUInt32LittleEndian(bytes, number);
            ValueSite site = new() { Property = property, Id = property.Id ?? 0, Path = DocumentNodePath.Root.Member("Value").Member("U32"), Owner = new JsonObject() };
            JsonValue decoded = codec.Decode(new DecodeContext(bytes, schema, "game", "game", null, FormatLimits.Default, new LayoutCounts()), 0, 4, site);
            Assert.Equal(number, ((JsonInteger)decoded).Value);
            EncodeContext encode = new(schema, "game", "game", null, FormatLimits.Default, ExportOptions.Default);
            ByteBuffer buffer = new();
            codec.Encode(encode, decoded, buffer, site);
            Assert.Equal(bytes, buffer.ToArray());
            ImageFrame frame = new() { Property = property, Id = property.Id ?? 0, Path = site.Path };
            IntegerNode read = (IntegerNode)codec.ReadImage(new DecodeContext(bytes, schema, "game", "game", null, FormatLimits.Default, new LayoutCounts()), 0, 4, frame);
            Assert.Equal(4, read.Width);
            Assert.False(read.Signed);
            Assert.Equal(number, read.Value);
            ByteBuffer written = new();
            codec.WriteImage(read, encode, written, frame);
            Assert.Equal(bytes, written.ToArray());

            RecordNode record = new();
            record.Add("U32", new IntegerNode(4, false, number));
            ObjectNode node = new(null, null);
            node.Add("Value", record);
            byte[] image = OriginalBackend.Write(node, classValue, schema, "game");
            ObjectNode back = OriginalBackend.Read(image, classValue, schema, "game");
            Assert.Equal(number, ((IntegerNode)((RecordNode)back.Find("Value")!).Find("U32")!).Value);
            JsonObject document = DocumentBackend.Write(back, classValue, schema, "game");
            Assert.Equal(number, ((JsonInteger)((JsonObject)document["Value"]!)["U32"]!).Value);
            Assert.Equal(image, OriginalBackend.Write(DocumentBackend.Read(document, classValue, schema, "game"), classValue: classValue, schema: schema, engine: "game"));
        }
    }

    [Fact]
    public void EveryIntegerWidthTransfers()
    {
        WidthHolder source = new()
        {
            Value = new EveryWidth { U8 = 255, I8 = sbyte.MinValue, U16 = ushort.MaxValue, I16 = short.MinValue, U32 = uint.MaxValue, I32 = int.MinValue },
        };
        ObjectNode node = TreeWriter.Write(source, s_factory);
        RecordNode record = (RecordNode)node.Find("Value")!;
        Assert.Equal(255, ((IntegerNode)record.Find("U8")!).Value);
        Assert.Equal(sbyte.MinValue, ((IntegerNode)record.Find("I8")!).Value);
        Assert.Equal(ushort.MaxValue, ((IntegerNode)record.Find("U16")!).Value);
        Assert.Equal(short.MinValue, ((IntegerNode)record.Find("I16")!).Value);
        Assert.Equal(uint.MaxValue, ((IntegerNode)record.Find("U32")!).Value);
        Assert.Equal(int.MinValue, ((IntegerNode)record.Find("I32")!).Value);
        WidthHolder loaded = new();
        TreeReader.Read(node, loaded, s_factory);
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
        FormatSchema schema = Sample;
        ObjectNode node = new(null, null);
        RecordNode pair = new();
        pair.Add("A", new IntegerNode(4, true, 1));
        node.Add("Pair", pair);
        ConversionException error = Assert.Throws<ConversionException>(() => OriginalBackend.Write(node, schema.Get("TPlain"), schema, "game"));
        Assert.Equal(Codes.Type, error.Diagnostic.Code);
        Assert.Equal("root.Pair", error.Diagnostic.Path);
        Assert.Contains("A record has no image.", error.Diagnostic.Message);
    }

    [Fact]
    public void ProductRecordsAndTheAbilityBitsRoundTrip()
    {
        FormatSchema schema = FormatSchema.Current;
        RecordNode rectangle = new();
        rectangle.Add("Left", new IntegerNode(4, true, int.MinValue));
        rectangle.Add("Bottom", new IntegerNode(4, true, 4));
        ObjectNode animation = new(null, null);
        animation.Add("Dimensions", rectangle);
        JsonObject animationDocument = RoundTrip(schema.Get("TAnimation"), schema, animation);
        JsonObject dimensions = (JsonObject)animationDocument["Dimensions"]!;
        Assert.Equal(["Left", "Bottom"], dimensions.Members.Select(m => m.Name).ToList());
        Assert.Equal(int.MinValue, ((JsonInteger)dimensions["Left"]!).Value);

        SetNode effects = new("DamageType", 2, (1UL << 9) | (1UL << 15));
        ObjectNode damage = new(null, null);
        damage.Add("Effects", effects);
        JsonObject damageDocument = RoundTrip(schema.Get("TDamageCA"), schema, damage);
        Assert.Equal(["None", 15L], Items(damageDocument["Effects"]!));

        RecordNode behavior = new();
        behavior.Add("PlayerType", new IntegerNode(1, false, 2));
        behavior.Add("$reserved", new BytesNode([1, 0, 2]));
        behavior.Add("Flags", new IntegerNode(4, true, 9));
        ObjectNode player = new(null, null);
        player.Add("Behavior", behavior);
        JsonObject playerDocument = RoundTrip(schema.Get("TPlayerSetupSettings"), schema, player);
        JsonObject stored = (JsonObject)playerDocument["Behavior"]!;
        Assert.Equal(["PlayerType", "$reserved", "Flags"], stored.Members.Select(m => m.Name).ToList());
        Assert.Equal("010002", ((JsonString)stored["$reserved"]!).Value);

        ObjectNode list = new(null, null);
        list.Add("AbilityCount", new IntegerNode(4, true, 1));
        list.Add("AbilityBits", new BitArrayNode(1, [], 0x80));
        JsonObject listDocument = RoundTrip(schema.Get("TCustomAbilityList"), schema, list);
        JsonObject ability = (JsonObject)listDocument["AbilityBits"]!;
        Assert.Empty(((JsonArray)ability["values"]!).Items);
        Assert.Equal("80", ((JsonString)ability["$reserved"]!).Value);

        ObjectNode champion = new(null, null);
        champion.Add("Champion", new IntegerNode(1, false, 2));
        JsonObject championDocument = RoundTrip(schema.Get("TUnitResource"), schema, champion);
        Assert.Equal(2, ((JsonInteger)championDocument["Champion"]!).Value);
    }

    private static JsonObject RoundTrip(SchemaClass classValue, FormatSchema schema, ObjectNode node)
    {
        JsonObject document = DocumentBackend.Write(node, classValue, schema, "game");
        JsonObject back = DocumentBackend.Write(OriginalBackend.Read(OriginalBackend.Write(node, classValue, schema, "game"), classValue: classValue, schema: schema, engine: "game"), classValue, schema, "game");
        Assert.Equal(JsonWriter.ToUtf8(document), JsonWriter.ToUtf8(back));
        return document;
    }

    private static ConversionException Converter(JsonObject document)
    {
        FormatSchema schema = Sample;
        EncodeContext context = new(schema, "game", "game", null, FormatLimits.Default, ExportOptions.Default);
        return Assert.Throws<ConversionException>(() => ObjectImage.Encode(context, document, schema.Get("THolder"), new ByteBuffer(), DocumentNodePath.Root));
    }

    private static ConversionException Refuse(JsonObject document)
    {
        FormatSchema schema = Sample;
        return Assert.Throws<ConversionException>(() => OriginalBackend.Write(
            DocumentBackend.Read(document, schema.Get("THolder"), schema, "game"), schema.Get("THolder"), schema, "game"));
    }

    private static JsonObject Bits(int count, params int[] indices)
    {
        JsonArray array = new();
        foreach (int index in indices)
        {
            array.Add(new JsonInteger(index));
        }

        return new JsonObject().Add("AbCount", new JsonInteger(count)).Add("Bits", array);
    }

    private static void Poke(ref SampleBehavior value, int offset, byte stored) =>
        MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref value, 1))[offset] = stored;

    private static byte At(SampleBehavior value, int offset) =>
        MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref value, 1))[offset];

    private static List<object> Items(JsonValue value)
    {
        JsonArray array = (JsonArray)value;
        List<object> items = [with(array.Items.Count)];
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

    private static readonly NamedFactory s_factory = new();

    private sealed class NamedFactory : IPersistableFactory
    {
        public string ClassName(IPersistable objectValue) => NameTable.Stored(objectValue.GetType());
        public IPersistable Create(string className) => throw new InvalidOperationException(className);
    }

    private sealed class Holder : IPersistable
    {
        public SampleBehavior Behavior { get; internal set; }

        public SamplePacked Marks { get; set; }
        public int AbilityCount { get; set; }
        public byte[] Bits { get; set; } = [];
        public byte Raw { get; set; }
        public sbyte Signed { get; set; }
        public int Wide { get; set; }

        public bool Flag { get; internal set; }

        public void Persist(IPersister persister)
        {
            Behavior = RecordValues.Transfer(persister, "Behavior", Behavior);
            Marks = RecordValues.Transfer(persister, "Marks", Marks);
            AbilityCount = persister.Int32("AbCount", AbilityCount);
            int length = AbilityCount < 0 ? 0 : (AbilityCount + 7) >> 3;
            if (persister.Loading)
            {
                byte[] bits = Bits;
                Array.Resize(ref bits, length);
                Bits = bits;
            }
            persister.BitArray("Bits", AbilityCount, Bits.AsSpan(0, Math.Max(length, 0)));
            Raw = persister.RawByte("Raw", Raw);
            Signed = persister.RawSByte("Signed", Signed);
            Wide = persister.RawInt32("Wide", Wide);
            Flag = persister.RawBool("Flag", Flag);
        }
    }

    private sealed class WidthHolder : IPersistable
    {
        public EveryWidth Value { get; set; }

        public void Persist(IPersister persister) => Value = RecordValues.Transfer(persister, "Value", Value);
    }
}
