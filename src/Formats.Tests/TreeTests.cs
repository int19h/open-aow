using Aow.Formats.Json;

using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Rtl;

namespace Aow.Formats.Tests;

/// <summary>
/// The persistence tree against a version 3 test schema. Version 1 of the game schema has no
/// list and no record, so those shapes are proven here. Version 2 stores a fixed integer array
/// and still refuses a record. Version 3 stores a record, so an unknown record member is an
/// unknown member.
/// </summary>
public sealed class TreeTests : FormatsTest
{
    private static FormatSchema Schema { get; } = FormatSchema.Load(File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TreeSchema.json")), "TreeSchema.json");

    private static FormatSchema Version1 { get; } = FormatSchema.Load(File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TreeSchemaV1.json")), "TreeSchemaV1.json");

    private static SchemaClass SampleClass => Schema.Get(NameTable.Stored(typeof(Sample)));

    [Fact]
    public void EveryValueRoundTripsInSchemaOrder()
    {
        Sample source = Full();
        JsonObject document = Write(source);
        Assert.Equal(SampleClass.Properties.Select(p => p.Name).Where(name => document.Find(name) is not null), document.Members.Select(m => m.Name));
        Assert.Equal("0a0b0c0d", ((JsonString)document["Blob"]!).Value);
        Assert.Equal("Jörg", ((JsonString)document["Title"]!).Value);
        Assert.Equal(4, ((JsonInteger)document["Player"]!).Value);
        ObjectNode readNode = DocumentBackend.Read(document, SampleClass, Schema, "game");
        Assert.Equal([("Player", 4)], readNode.References("player-slot").ToList());
        Sample loaded = new();
        TreeReader.Read(readNode, loaded, new TestFactory());
        AssertSame(source, loaded);
    }

    [Fact]
    public void FallbacksAndEmptyValuesAreAbsent()
    {
        JsonObject document = Write(new Sample());
        Assert.Equal(["Blob", "Fixed", "Made", "Children", "Pair"], document.Members.Select(m => m.Name).ToList());
        Assert.Equal("00000000", ((JsonString)document["Blob"]!).Value);
        _ = Assert.IsType<JsonNull>(document["Made"]);
        Assert.Empty(((JsonObject)document["Fixed"]!).Members);
        Assert.Empty(((JsonArray)document["Children"]!).Items);
        Assert.Empty(((JsonObject)document["Pair"]!).Members);
        Sample loaded = new()
        {
            Number = 1,
            NumberFallback = 1,
            Small = 1,
            SmallFallback = 1,
            SignedByte = 1,
            Word = 1,
            WordFallback = 1,
            Flag = true,
            Title = "x",
            Note = "x",
            Player = 3,
            Blob = [9, 9, 9, 9],
        };
        loaded.Message.Add("x");
        loaded.EditorLines.Add("x");
        loaded.EditorObjects.Add(1);
        loaded.Scores.Add(1);
        loaded.Flags.Add(true);
        loaded.Kids.Add(new Inner { Number = 1 });
        loaded.Made = new Inner { Number = 1 };
        loaded.Fixed.Number = 5;
        TreeReader.Read(DocumentBackend.Read(document, SampleClass, Schema, "game"), loaded, new TestFactory());
        AssertSame(new Sample(), loaded);
    }

    [Fact]
    public void NonZeroFallbacksAndBoundariesRoundTrip()
    {
        Sample loaded = RoundTrip(new Sample
        {
            Number = int.MinValue,
            NumberFallback = 0,
            Small = 255,
            SmallFallback = 0,
            SignedByte = -128,
            Word = 65535,
            WordFallback = 0,
            Flag = true,
        });
        Assert.Equal(int.MinValue, loaded.Number);
        Assert.Equal(0, loaded.NumberFallback);
        Assert.Equal(255, loaded.Small);
        Assert.Equal(0, loaded.SmallFallback);
        Assert.Equal(-128, loaded.SignedByte);
        Assert.Equal(65535, loaded.Word);
        Assert.Equal(0, loaded.WordFallback);
        Assert.True(loaded.Flag);
        Sample other = RoundTrip(new Sample { Number = int.MaxValue, NumberFallback = 8, SmallFallback = 255, SignedByte = 127, WordFallback = 65535 });
        Assert.Equal(int.MaxValue, other.Number);
        Assert.Equal(8, other.NumberFallback);
        Assert.Equal(255, other.SmallFallback);
        Assert.Equal(127, other.SignedByte);
        Assert.Equal(65535, other.WordFallback);
    }

    [Fact]
    public void AnEmptyByteSpanIsOmittedAndAnAbsentValueLeavesTheSpan()
    {
        BytesProbe empty = new();
        ObjectNode node = TreeWriter.Write(empty, new TestFactory());
        Assert.Null(node.Find("Blob"));
        BytesProbe kept = new() { Data = [9, 9] };
        TreeReader.Read(node, kept, new TestFactory());
        Assert.Equal([9, 9], kept.Data);
        BytesProbe wrong = new() { Data = [1, 2] };
        ConversionException mismatch = Assert.Throws<ConversionException>(() => TreeReader.Read(TreeWriter.Write(new BytesProbe { Data = [1, 2, 3, 4] }, new TestFactory()), wrong, new TestFactory()));
        Assert.Equal(Codes.Range, mismatch.Diagnostic.Code);
        Assert.Equal("root.Blob", mismatch.Diagnostic.Path);
        ObjectNode shortNode = new(null, null);
        shortNode.Add("Blob", new BytesNode([1, 2]));
        ConversionException size = Assert.Throws<ConversionException>(() => DocumentBackend.Write(shortNode, SampleClass, Schema, "game"));
        Assert.Equal(Codes.Range, size.Diagnostic.Code);
        Assert.Equal("root.Blob", size.Diagnostic.Path);
    }

    [Fact]
    public void ALineThatHoldsCrIsStoredAsText()
    {
        Sample source = new();
        source.Message.Add("a\rb");
        JsonObject document = Write(source);
        Assert.Equal("a\rb\r\n", ((JsonString)((JsonObject)document["Message"]!)["$text"]!).Value);
    }

    [Fact]
    public void ALineWithObjectsThatHoldsCrIsRefusedAsTheConverterRefusesIt()
    {
        Sample source = new();
        source.EditorLines.Add("a\rb");
        source.EditorObjects.Add(1);
        ObjectNode node = TreeWriter.Write(source, new TestFactory());
        ConversionException error = Assert.Throws<ConversionException>(() => DocumentBackend.Write(node, SampleClass, Schema, "game"));
        Assert.Equal(Codes.Corrupt, error.Diagnostic.Code);
        Assert.Equal("root.Editors", error.Diagnostic.Path);
    }

    [Fact]
    public void AnUnconsumedMemberIsAnErrorWithItsPath()
    {
        Sample source = new() { SkipNote = true, Note = "kept" };
        ObjectNode node = TreeWriter.Write(new Sample { Note = "kept" }, new TestFactory());
        ConversionException error = Assert.Throws<ConversionException>(() => TreeReader.Read(node, source, new TestFactory()));
        Assert.Equal(Codes.Required, error.Diagnostic.Code);
        Assert.Equal("root.Note", error.Diagnostic.Path);
        Assert.Contains("was not read", error.Diagnostic.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void HasDoesNotConsumeAMember()
    {
        ObjectNode node = TreeWriter.Write(new Sample { Note = "kept" }, new TestFactory());
        Sample target = new() { SkipNote = true };
        ConversionException error = Assert.Throws<ConversionException>(() => TreeReader.Read(node, target, new TestFactory()));
        Assert.Equal("root.Note", error.Diagnostic.Path);
        Assert.True(target.HadNote);
    }

    [Fact]
    public void ANestedRefusalIsSwallowedUnlessTheHandlerStops()
    {
        ObjectNode made = new(nameof(Inner), null);
        made.Add("N", new IntegerNode(4, true, 2));
        made.Add("Extra", new IntegerNode(4, true, 1));
        ObjectNode created = new(null, null);
        created.Add("Made", made);
        created.Add("Number", new IntegerNode(4, true, 6));
        Sample continued = new() { Number = 1 };
        TreeReader.Read(created, continued, new TestFactory());
        Assert.Null(continued.Made);
        Assert.Equal(6, continued.Number);
        Sample stopped = new() { Number = 1 };
        ConversionException createdError = Assert.Throws<ConversionException>(() =>
            TreeReader.Read(created, stopped, new TestFactory(), static (_, out cont) => cont = false));
        Assert.Equal(Codes.Required, createdError.Diagnostic.Code);
        Assert.Equal("root.Made.Extra", createdError.Diagnostic.Path);
        Assert.Equal(1, stopped.Number);

        ObjectNode nested = new(null, null);
        nested.Add("N", new BooleanNode(true));
        ObjectNode parent = new(null, null);
        parent.Add("Fixed", nested);
        parent.Add("Number", new IntegerNode(4, true, 6));
        Sample kept = new() { Number = 1 };
        kept.Fixed.Number = 4;
        Inner instance = kept.Fixed;
        TreeReader.Read(parent, kept, new TestFactory());
        Assert.Same(instance, kept.Fixed);
        Assert.Equal(4, kept.Fixed.Number);
        Assert.Equal(6, kept.Number);
        Sample refused = new() { Number = 1 };
        refused.Fixed.Number = 4;
        Inner refusedFixed = refused.Fixed;
        ConversionException fixedError = Assert.Throws<ConversionException>(() =>
            TreeReader.Read(parent, refused, new TestFactory(), static (_, out cont) => cont = false));
        Assert.Equal(Codes.Type, fixedError.Diagnostic.Code);
        Assert.Equal("root.Fixed.N", fixedError.Diagnostic.Path);
        Assert.Same(refusedFixed, refused.Fixed);
        Assert.Equal(4, refused.Fixed.Number);
        Assert.Equal(1, refused.Number);
    }

    [Fact]
    public void AnUnknownMemberIsAnErrorWithItsPath()
    {
        JsonObject document = Write(new Sample());
        _ = document.Add("Nope", new JsonInteger(1));
        ConversionException error = Assert.Throws<ConversionException>(() => DocumentBackend.Read(document, SampleClass, Schema, "game"));
        Assert.Equal(Codes.UnknownMember, error.Diagnostic.Code);
        Assert.Equal("root.Nope", error.Diagnostic.Path);
        JsonObject nested = Write(new Sample());
        _ = ((JsonObject)nested["Fixed"]!).Add("Nope", new JsonInteger(1));
        ConversionException nestedError = Assert.Throws<ConversionException>(() => DocumentBackend.Read(nested, SampleClass, Schema, "game"));
        Assert.Equal("root.Fixed.Nope", nestedError.Diagnostic.Path);
    }

    [Fact]
    public void ADuplicateMemberIsRefused()
    {
        JsonObject document = Write(new Sample());
        _ = document.Add("Fixed", new JsonObject());
        ConversionException error = Assert.Throws<ConversionException>(() => DocumentBackend.Read(document, SampleClass, Schema, "game"));
        Assert.Equal(Codes.Duplicate, error.Diagnostic.Code);
        Assert.Equal("root.Fixed", error.Diagnostic.Path);
        ConversionException twice = Assert.Throws<ConversionException>(() => TreeWriter.Write(new Twice { Number = 1 }, new TestFactory()));
        Assert.Equal(Codes.Duplicate, twice.Diagnostic.Code);
        Assert.Equal("root.Number", twice.Diagnostic.Path);
    }

    [Fact]
    public void AListLongerThanAFixedArrayRoundTrips()
    {
        Sample source = new();
        for (int i = 0; i < 300; i++)
        {
            source.Scores.Add(i - 10);
        }

        source.Scores[0] = 0;
        source.Scores[1] = int.MinValue;
        source.Scores[2] = int.MaxValue;
        source.Flags.Add(false);
        source.Flags.Add(true);
        JsonObject document = Write(source);
        Assert.Equal(300, ((JsonArray)document["Scores"]!).Items.Count);
        Assert.Equal(0, ((JsonInteger)((JsonArray)document["Scores"]!).Items[0]).Value);
        Assert.False(((JsonBoolean)((JsonArray)document["Flags"]!).Items[0]).Value);
        Assert.True(((JsonBoolean)((JsonArray)document["Flags"]!).Items[1]).Value);
        AssertSame(source, RoundTrip(source));
    }

    [Fact]
    public void ARecordThatGainsAFieldReadsTheOlderDocumentWithTheFallback()
    {
        JsonObject older = (JsonObject)JsonReader.Parse("""{"Pair":{"A":2}}""", "older.json");
        Sample loaded = new();
        TreeReader.Read(DocumentBackend.Read(older, SampleClass, Schema, "game"), loaded, new TestFactory());
        Assert.Equal(2, loaded.PairA);
        Assert.Equal(4, loaded.PairB);
        Sample absent = new() { PairA = 9, PairB = 9 };
        TreeReader.Read(DocumentBackend.Read(new JsonObject(), SampleClass, Schema, "game"), absent, new TestFactory());
        Assert.Equal(0, absent.PairA);
        Assert.Equal(4, absent.PairB);
        JsonObject written = Write(new Sample { PairA = 2, PairB = 4 });
        Assert.Equal(["A"], ((JsonObject)written["Pair"]!).Members.Select(m => m.Name).ToList());
    }

    [Fact]
    public void Version1RefusesAListAndARecordWithThePath()
    {
        FormatSchema current = FormatSchema.Current;
        SchemaClass enchantment = current.Get("TEnchantment");
        ObjectNode unknown = new(null, null);
        unknown.Add("Extra", new ListNode());
        ConversionException unknownError = Assert.Throws<ConversionException>(() => DocumentBackend.Write(unknown, enchantment, current, "game"));
        Assert.Equal(Codes.UnknownMember, unknownError.Diagnostic.Code);
        Assert.Equal("root.Extra", unknownError.Diagnostic.Path);
        ObjectNode upkeep = new(null, null);
        upkeep.Add("Upkeep", new ListNode());
        ConversionException upkeepError = Assert.Throws<ConversionException>(() => DocumentBackend.Write(upkeep, enchantment, current, "game"));
        Assert.Equal(Codes.Type, upkeepError.Diagnostic.Code);
        Assert.Equal("root.Upkeep", upkeepError.Diagnostic.Path);
        Assert.Equal($"Version {OwnFormat.CurrentVersion} stores a list only for a fixed integer array.", upkeepError.Diagnostic.Message);
        ObjectNode record = new(null, null);
        record.Add("Extra", new RecordNode());
        ConversionException recordError = Assert.Throws<ConversionException>(() => DocumentBackend.Write(record, enchantment, current, "game"));
        Assert.Equal("root.Extra", recordError.Diagnostic.Path);
        Assert.Equal(Codes.UnknownMember, recordError.Diagnostic.Code);
        Assert.Equal("Class " + enchantment.Name + " has no member \"Extra\".", recordError.Diagnostic.Message);
        ConversionException upkeepDocument = Assert.Throws<ConversionException>(() => DocumentBackend.Read(
            (JsonObject)JsonReader.Parse("""{"Upkeep":[1,2]}""", "upkeep.json"), enchantment, current, "game"));
        Assert.Equal("root.Upkeep", upkeepDocument.Diagnostic.Path);
        Assert.Equal($"Version {OwnFormat.CurrentVersion} stores a list only for a fixed integer array.", upkeepDocument.Diagnostic.Message);

        FormatSchema schema = Version1;
        SchemaClass classValue = schema.Get("TRefuse");
        ConversionException scores = Assert.Throws<ConversionException>(() => DocumentBackend.Read((JsonObject)JsonReader.Parse("""{"Scores":[1,2]}""", "list.json"), classValue, schema, "game"));
        Assert.Equal("root.Scores", scores.Diagnostic.Path);
        Assert.Equal("Version 1 does not store a list.", scores.Diagnostic.Message);
        ConversionException pair = Assert.Throws<ConversionException>(() => DocumentBackend.Read((JsonObject)JsonReader.Parse("""{"Pair":{"A":1}}""", "record.json"), classValue, schema, "game"));
        Assert.Equal("root.Pair", pair.Diagnostic.Path);
        Assert.Equal("Version 1 does not store a record.", pair.Diagnostic.Message);
        ObjectNode node = new(null, null);
        node.Add("Scores", new ListNode());
        ConversionException written = Assert.Throws<ConversionException>(() => DocumentBackend.Write(node, classValue, schema, "game"));
        Assert.Equal("root.Scores", written.Diagnostic.Path);
        Assert.Equal("Version 1 does not store a list.", written.Diagnostic.Message);
    }

    [Fact]
    public void Version1WritesAReferenceAsAPlainInteger()
    {
        ObjectNode node = new(null, null);
        node.Add("Upkeep", new ReferenceNode("player-slot", 4));
        JsonObject document = DocumentBackend.Write(node, FormatSchema.Current.Get("TEnchantment"), FormatSchema.Current, "game");
        Assert.Equal(4, ((JsonInteger)document["Upkeep"]!).Value);
        ObjectNode read = DocumentBackend.Read(document, FormatSchema.Current.Get("TEnchantment"), FormatSchema.Current, "game");
        _ = Assert.IsType<IntegerNode>(read.Find("Upkeep"));
        ReferenceProbe probe = new();
        TreeReader.Read(node, probe, new TestFactory());
        Assert.Equal(4, probe.Key);
        ReferenceProbe fromDocument = new();
        TreeReader.Read(read, fromDocument, new TestFactory());
        Assert.Equal(4, fromDocument.Key);
    }

    private static JsonObject Write(Sample sample) =>
        DocumentBackend.Write(TreeWriter.Write(sample, new TestFactory()), SampleClass, Schema, "game");

    private static Sample RoundTrip(Sample sample)
    {
        Sample loaded = new();
        TreeReader.Read(DocumentBackend.Read(Write(sample), SampleClass, Schema, "game"), loaded, new TestFactory());
        return loaded;
    }

    private static Sample Full()
    {
        Sample sample = new()
        {
            Number = int.MinValue,
            NumberFallback = 8,
            Small = 255,
            SmallFallback = 1,
            SignedByte = 127,
            Word = 65535,
            WordFallback = 10,
            Flag = true,
            Title = "Jörg",
            Note = "seen",
            Player = 4,
            Blob = [0x0A, 0x0B, 0x0C, 0x0D],
            PairA = 1,
            PairB = 5,
            Made = new Inner { Number = 5 },
        };
        sample.Message.Add("one");
        sample.Message.Add("two");
        sample.EditorLines.Add("");
        sample.EditorObjects.Add(0);
        sample.EditorLines.Add("Gate");
        sample.EditorObjects.Add(4);
        sample.Scores.Add(0);
        sample.Scores.Add(int.MaxValue);
        sample.Flags.Add(false);
        sample.Flags.Add(true);
        sample.Kids.Add(new Inner { Number = 1 });
        sample.Kids.Add(new Inner());
        return sample;
    }

    private static void AssertSame(Sample expected, Sample actual)
    {
        Assert.Equal(expected.Number, actual.Number);
        Assert.Equal(expected.NumberFallback, actual.NumberFallback);
        Assert.Equal(expected.Small, actual.Small);
        Assert.Equal(expected.SmallFallback, actual.SmallFallback);
        Assert.Equal(expected.SignedByte, actual.SignedByte);
        Assert.Equal(expected.Word, actual.Word);
        Assert.Equal(expected.WordFallback, actual.WordFallback);
        Assert.Equal(expected.Flag, actual.Flag);
        Assert.Equal(expected.Title, actual.Title);
        Assert.Equal(expected.Note, actual.Note);
        Assert.Equal(expected.Player, actual.Player);
        Assert.Equal(expected.Blob, actual.Blob);
        Assert.Equal(expected.PairA, actual.PairA);
        Assert.Equal(expected.PairB, actual.PairB);
        Assert.Equal(expected.Message, actual.Message);
        Assert.Equal(expected.EditorLines, actual.EditorLines);
        Assert.Equal(expected.EditorObjects, actual.EditorObjects);
        Assert.Equal(expected.Scores, actual.Scores);
        Assert.Equal(expected.Flags, actual.Flags);
        Assert.Equal(expected.Kids.Select(k => k.Number).ToList(), [.. actual.Kids.Select(k => k.Number)]);
        Assert.Equal(expected.Made is Inner made ? made.Number : -1, actual.Made is Inner loaded ? loaded.Number : -1);
        Assert.Equal(expected.Fixed.Number, actual.Fixed.Number);
    }

    private sealed class Inner : IPersistable
    {
        public int Number { get; set; }

        public void Persist(IPersister persister) => Number = persister.Int32("N", Number);
    }

    private sealed class Sample : IPersistable
    {
        public int Number { get; set; }
        public int NumberFallback { get; set; } = 7;
        public byte Small { get; set; }
        public byte SmallFallback { get; set; } = 3;
        public sbyte SignedByte { get; set; }
        public ushort Word { get; set; }
        public ushort WordFallback { get; set; } = 9;
        public bool Flag { get; set; }
        public string Title { get; set; } = "";
        public string Note { get; set; } = "";
        public int Player { get; set; }

        public byte[] Blob { get; init; } = new byte[4];

        public int PairA { get; set; }
        public int PairB { get; set; } = 4;

        public bool SkipNote { get; init; }
        public bool HadNote { get; private set; }

        public Inner Fixed { get; } = new();

        public IPersistable? Made { get; set; }

        public List<string> Message { get; } = [];
        public List<string> EditorLines { get; } = [];
        public List<int> EditorObjects { get; } = [];
        public List<int> Scores { get; } = [];
        public List<bool> Flags { get; } = [];
        public List<Inner> Kids { get; } = [];

        public void Persist(IPersister persister)
        {
            if (persister.Loading)
            {
                Kids.Clear();
            }

            Player = persister.Reference("Player", "player-slot", Player);
            persister.List("Flags", Flags, FlagItem);
            persister.List("Scores", Scores, ScoreItem);
            persister.Record("Pair", WritePair);
            persister.Children("Children", new KidList(Kids));
            Made = persister.Created("Made", Made);
            persister.Object("Fixed", Fixed);
            persister.Bytes("Blob", Blob);
            persister.LinesWithObjects("Editors", EditorLines, EditorObjects);
            persister.Lines("Message", Message);
            Title = persister.String("Title", Title);
            Flag = persister.Boolean("Flag", Flag);
            WordFallback = persister.UInt16("WordFallback", WordFallback, 9);
            Word = persister.UInt16("Word", Word);
            SignedByte = persister.SByte("SignedByte", SignedByte);
            SmallFallback = persister.Byte("SmallFallback", SmallFallback, 3);
            Small = persister.Byte("Small", Small);
            NumberFallback = persister.Int32("NumberFallback", NumberFallback, 7);
            Number = persister.Int32("Number", Number);
            if (SkipNote)
            {
                HadNote = persister.Has("Note");
            }
            else
            {
                Note = persister.String("Note", Note);
            }
        }

        private void WritePair(IPersister record)
        {
            PairB = record.Int32("B", PairB, 4);
            PairA = record.Int32("A", PairA);
        }

        private static int ScoreItem(IItemSlot slot, int value) => slot.Int32(value);

        private static bool FlagItem(IItemSlot slot, bool value) => slot.Boolean(value);
    }

    private sealed class KidList(List<Inner> kids) : IChildCollection
    {
        public int Count => kids.Count;

        public IPersistable Get(int index) => kids[index];

        public bool IsStored(int index) => true;

        public int Slot(int index) => index;

        public bool KeepIndex => false;

        public void Attach(IPersistable child)
        {
        }

        public void Place(int slot, IPersistable child) => kids.Add((Inner)child);
    }

    private sealed class BytesProbe : IPersistable
    {
        public byte[] Data { get; init; } = [];

        public void Persist(IPersister persister) => persister.Bytes("Blob", Data);
    }

    private sealed class Twice : IPersistable
    {
        public int Number { get; set; }

        public void Persist(IPersister persister)
        {
            Number = persister.Int32("Number", Number);
            Number = persister.Int32("Number", Number);
        }
    }

    private sealed class ReferenceProbe : IPersistable
    {
        public int Key { get; set; }

        public void Persist(IPersister persister) => Key = persister.Reference("Upkeep", "player-slot", Key);
    }

    private sealed class TestFactory : IPersistableFactory
    {
        public string ClassName(IPersistable objectValue) => NameTable.Stored(objectValue.GetType());

        public IPersistable Create(string className)
        {
            return NameTable.Names(typeof(Inner), className)
                ? new Inner()
                : NameTable.Names(typeof(Sample), className)
                ? (IPersistable)new Sample()
                : throw new InvalidOperationException("The test factory has no class " + className + ".");
        }
    }
}
