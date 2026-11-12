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

    private static FormatSchema Frozen(string resource, string file)
    {
        using var stream = typeof(FormatSchema).Assembly.GetManifestResourceStream(resource)!;
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return FormatSchema.Load(memory.ToArray(), file);
    }

    private static SchemaClass SampleClass => Schema.Get(NameTable.Stored(typeof(Sample)));

    [Fact]
    public void EveryValueRoundTripsInSchemaOrder()
    {
        var source = Full();
        var document = Write(source);
        Assert.Equal(SampleClass.Properties.Select(p => p.Name).Where(name => document.Find(name) is not null), document.Members.Select(m => m.Name));
        Assert.Equal("0a0b0c0d", ((JsonString)document["Blob"]!).Value);
        Assert.Equal("Jörg", ((JsonString)document["Title"]!).Value);
        Assert.Equal(4, ((JsonInteger)document["Player"]!).Value);
        var readNode = DocumentBackend.Read(document, SampleClass, Schema, "game");
        Assert.Equal([("Player", 4)], readNode.References("player-slot").ToList());
        var loaded = new Sample();
        TreeReader.Read(readNode, loaded, new TestFactory());
        AssertSame(source, loaded);
    }

    [Fact]
    public void FallbacksAndEmptyValuesAreAbsent()
    {
        var document = Write(new Sample());
        Assert.Equal(["Blob", "Fixed", "Made", "Children", "Pair"], document.Members.Select(m => m.Name).ToList());
        Assert.Equal("00000000", ((JsonString)document["Blob"]!).Value);
        Assert.IsType<JsonNull>(document["Made"]);
        Assert.Empty(((JsonObject)document["Fixed"]!).Members);
        Assert.Empty(((JsonArray)document["Children"]!).Items);
        Assert.Empty(((JsonObject)document["Pair"]!).Members);
        var loaded = new Sample
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
        var source = new Sample
        {
            Number = int.MinValue,
            NumberFallback = 0,
            Small = 255,
            SmallFallback = 0,
            SignedByte = -128,
            Word = 65535,
            WordFallback = 0,
            Flag = true,
        };
        var loaded = RoundTrip(source);
        Assert.Equal(int.MinValue, loaded.Number);
        Assert.Equal(0, loaded.NumberFallback);
        Assert.Equal(255, loaded.Small);
        Assert.Equal(0, loaded.SmallFallback);
        Assert.Equal(-128, loaded.SignedByte);
        Assert.Equal(65535, loaded.Word);
        Assert.Equal(0, loaded.WordFallback);
        Assert.True(loaded.Flag);
        var other = RoundTrip(new Sample { Number = int.MaxValue, NumberFallback = 8, SmallFallback = 255, SignedByte = 127, WordFallback = 65535 });
        Assert.Equal(int.MaxValue, other.Number);
        Assert.Equal(8, other.NumberFallback);
        Assert.Equal(255, other.SmallFallback);
        Assert.Equal(127, other.SignedByte);
        Assert.Equal(65535, other.WordFallback);
    }

    [Fact]
    public void AnEmptyByteSpanIsOmittedAndAnAbsentValueLeavesTheSpan()
    {
        var empty = new BytesProbe();
        var node = TreeWriter.Write(empty, new TestFactory());
        Assert.Null(node.Find("Blob"));
        var kept = new BytesProbe { Data = [9, 9] };
        TreeReader.Read(node, kept, new TestFactory());
        Assert.Equal([9, 9], kept.Data);
        var wrong = new BytesProbe { Data = [1, 2] };
        var mismatch = Assert.Throws<ConversionException>(() => TreeReader.Read(TreeWriter.Write(new BytesProbe { Data = [1, 2, 3, 4] }, new TestFactory()), wrong, new TestFactory()));
        Assert.Equal(Codes.Range, mismatch.Diagnostic.Code);
        Assert.Equal("root.Blob", mismatch.Diagnostic.Path);
        var shortNode = new ObjectNode(null, null);
        shortNode.Add("Blob", new BytesNode([1, 2]));
        var size = Assert.Throws<ConversionException>(() => DocumentBackend.Write(shortNode, SampleClass, Schema, "game"));
        Assert.Equal(Codes.Range, size.Diagnostic.Code);
        Assert.Equal("root.Blob", size.Diagnostic.Path);
    }

    [Fact]
    public void ALineThatHoldsCrIsStoredAsText()
    {
        var source = new Sample();
        source.Message.Add("a\rb");
        var document = Write(source);
        Assert.Equal("a\rb\r\n", ((JsonString)((JsonObject)document["Message"]!)["$text"]!).Value);
    }

    [Fact]
    public void ALineWithObjectsThatHoldsCrIsRefusedAsTheConverterRefusesIt()
    {
        var source = new Sample();
        source.EditorLines.Add("a\rb");
        source.EditorObjects.Add(1);
        var node = TreeWriter.Write(source, new TestFactory());
        var error = Assert.Throws<ConversionException>(() => DocumentBackend.Write(node, SampleClass, Schema, "game"));
        Assert.Equal(Codes.Corrupt, error.Diagnostic.Code);
        Assert.Equal("root.Editors", error.Diagnostic.Path);
    }

    [Fact]
    public void AnUnconsumedMemberIsAnErrorWithItsPath()
    {
        var source = new Sample { SkipNote = true, Note = "kept" };
        var node = TreeWriter.Write(new Sample { Note = "kept" }, new TestFactory());
        var error = Assert.Throws<ConversionException>(() => TreeReader.Read(node, source, new TestFactory()));
        Assert.Equal(Codes.Required, error.Diagnostic.Code);
        Assert.Equal("root.Note", error.Diagnostic.Path);
        Assert.Contains("was not read", error.Diagnostic.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void HasDoesNotConsumeAMember()
    {
        var node = TreeWriter.Write(new Sample { Note = "kept" }, new TestFactory());
        var target = new Sample { SkipNote = true };
        var error = Assert.Throws<ConversionException>(() => TreeReader.Read(node, target, new TestFactory()));
        Assert.Equal("root.Note", error.Diagnostic.Path);
        Assert.True(target.HadNote);
    }

    [Fact]
    public void ANestedRefusalIsSwallowedUnlessTheHandlerStops()
    {
        var made = new ObjectNode(nameof(Inner), null);
        made.Add("N", new IntegerNode(4, true, 2));
        made.Add("Extra", new IntegerNode(4, true, 1));
        var created = new ObjectNode(null, null);
        created.Add("Made", made);
        created.Add("Number", new IntegerNode(4, true, 6));
        var continued = new Sample { Number = 1 };
        TreeReader.Read(created, continued, new TestFactory());
        Assert.Null(continued.Made);
        Assert.Equal(6, continued.Number);
        var stopped = new Sample { Number = 1 };
        var createdError = Assert.Throws<ConversionException>(() =>
            TreeReader.Read(created, stopped, new TestFactory(), static (string _, ref bool cont) => cont = false));
        Assert.Equal(Codes.Required, createdError.Diagnostic.Code);
        Assert.Equal("root.Made.Extra", createdError.Diagnostic.Path);
        Assert.Equal(1, stopped.Number);

        var nested = new ObjectNode(null, null);
        nested.Add("N", new BooleanNode(true));
        var parent = new ObjectNode(null, null);
        parent.Add("Fixed", nested);
        parent.Add("Number", new IntegerNode(4, true, 6));
        var kept = new Sample { Number = 1 };
        kept.Fixed.Number = 4;
        var instance = kept.Fixed;
        TreeReader.Read(parent, kept, new TestFactory());
        Assert.Same(instance, kept.Fixed);
        Assert.Equal(4, kept.Fixed.Number);
        Assert.Equal(6, kept.Number);
        var refused = new Sample { Number = 1 };
        refused.Fixed.Number = 4;
        var refusedFixed = refused.Fixed;
        var fixedError = Assert.Throws<ConversionException>(() =>
            TreeReader.Read(parent, refused, new TestFactory(), static (string _, ref bool cont) => cont = false));
        Assert.Equal(Codes.Type, fixedError.Diagnostic.Code);
        Assert.Equal("root.Fixed.N", fixedError.Diagnostic.Path);
        Assert.Same(refusedFixed, refused.Fixed);
        Assert.Equal(4, refused.Fixed.Number);
        Assert.Equal(1, refused.Number);
    }

    [Fact]
    public void AnUnknownMemberIsAnErrorWithItsPath()
    {
        var document = Write(new Sample());
        document.Add("Nope", new JsonInteger(1));
        var error = Assert.Throws<ConversionException>(() => DocumentBackend.Read(document, SampleClass, Schema, "game"));
        Assert.Equal(Codes.UnknownMember, error.Diagnostic.Code);
        Assert.Equal("root.Nope", error.Diagnostic.Path);
        var nested = Write(new Sample());
        ((JsonObject)nested["Fixed"]!).Add("Nope", new JsonInteger(1));
        var nestedError = Assert.Throws<ConversionException>(() => DocumentBackend.Read(nested, SampleClass, Schema, "game"));
        Assert.Equal("root.Fixed.Nope", nestedError.Diagnostic.Path);
    }

    [Fact]
    public void ADuplicateMemberIsRefused()
    {
        var document = Write(new Sample());
        document.Add("Fixed", new JsonObject());
        var error = Assert.Throws<ConversionException>(() => DocumentBackend.Read(document, SampleClass, Schema, "game"));
        Assert.Equal(Codes.Duplicate, error.Diagnostic.Code);
        Assert.Equal("root.Fixed", error.Diagnostic.Path);
        var twice = Assert.Throws<ConversionException>(() => TreeWriter.Write(new Twice { Number = 1 }, new TestFactory()));
        Assert.Equal(Codes.Duplicate, twice.Diagnostic.Code);
        Assert.Equal("root.Number", twice.Diagnostic.Path);
    }

    [Fact]
    public void AListLongerThanAFixedArrayRoundTrips()
    {
        var source = new Sample();
        for (int i = 0; i < 300; i++) source.Scores.Add(i - 10);
        source.Scores[0] = 0;
        source.Scores[1] = int.MinValue;
        source.Scores[2] = int.MaxValue;
        source.Flags.Add(false);
        source.Flags.Add(true);
        var document = Write(source);
        Assert.Equal(300, ((JsonArray)document["Scores"]!).Items.Count);
        Assert.Equal(0, ((JsonInteger)((JsonArray)document["Scores"]!).Items[0]).Value);
        Assert.False(((JsonBoolean)((JsonArray)document["Flags"]!).Items[0]).Value);
        Assert.True(((JsonBoolean)((JsonArray)document["Flags"]!).Items[1]).Value);
        AssertSame(source, RoundTrip(source));
    }

    [Fact]
    public void ARecordThatGainsAFieldReadsTheOlderDocumentWithTheFallback()
    {
        var older = (JsonObject)JsonReader.Parse("""{"Pair":{"A":2}}""", "older.json");
        var loaded = new Sample();
        TreeReader.Read(DocumentBackend.Read(older, SampleClass, Schema, "game"), loaded, new TestFactory());
        Assert.Equal(2, loaded.PairA);
        Assert.Equal(4, loaded.PairB);
        var absent = new Sample { PairA = 9, PairB = 9 };
        TreeReader.Read(DocumentBackend.Read(new JsonObject(), SampleClass, Schema, "game"), absent, new TestFactory());
        Assert.Equal(0, absent.PairA);
        Assert.Equal(4, absent.PairB);
        var written = Write(new Sample { PairA = 2, PairB = 4 });
        Assert.Equal(["A"], ((JsonObject)written["Pair"]!).Members.Select(m => m.Name).ToList());
    }

    [Fact]
    public void Version1RefusesAListAndARecordWithThePath()
    {
        var current = FormatSchema.Current;
        var enchantment = current.Get("TEnchantment");
        var unknown = new ObjectNode(null, null);
        unknown.Add("Extra", new ListNode());
        var unknownError = Assert.Throws<ConversionException>(() => DocumentBackend.Write(unknown, enchantment, current, "game"));
        Assert.Equal(Codes.UnknownMember, unknownError.Diagnostic.Code);
        Assert.Equal("root.Extra", unknownError.Diagnostic.Path);
        var upkeep = new ObjectNode(null, null);
        upkeep.Add("Upkeep", new ListNode());
        var upkeepError = Assert.Throws<ConversionException>(() => DocumentBackend.Write(upkeep, enchantment, current, "game"));
        Assert.Equal(Codes.Type, upkeepError.Diagnostic.Code);
        Assert.Equal("root.Upkeep", upkeepError.Diagnostic.Path);
        Assert.Equal("Version 3 stores a list only for a fixed integer array.", upkeepError.Diagnostic.Message);
        var record = new ObjectNode(null, null);
        record.Add("Extra", new RecordNode());
        var recordError = Assert.Throws<ConversionException>(() => DocumentBackend.Write(record, enchantment, current, "game"));
        Assert.Equal("root.Extra", recordError.Diagnostic.Path);
        Assert.Equal(Codes.UnknownMember, recordError.Diagnostic.Code);
        Assert.Equal("Class " + enchantment.Name + " has no member \"Extra\".", recordError.Diagnostic.Message);
        var version2 = Frozen("Aow.Formats.Migrations.schema-v2.json", "schema-v2.json");
        var oldRecord = Assert.Throws<ConversionException>(() => DocumentBackend.Write(record, version2.Get("TEnchantment"), version2, "game"));
        Assert.Equal("root.Extra", oldRecord.Diagnostic.Path);
        Assert.Equal("Version 2 does not store a record.", oldRecord.Diagnostic.Message);
        var upkeepDocument = Assert.Throws<ConversionException>(() => DocumentBackend.Read(
            (JsonObject)JsonReader.Parse("""{"Upkeep":[1,2]}""", "upkeep.json"), enchantment, current, "game"));
        Assert.Equal("root.Upkeep", upkeepDocument.Diagnostic.Path);
        Assert.Equal("Version 3 stores a list only for a fixed integer array.", upkeepDocument.Diagnostic.Message);

        var schema = Version1;
        var classValue = schema.Get("TRefuse");
        var scores = Assert.Throws<ConversionException>(() => DocumentBackend.Read((JsonObject)JsonReader.Parse("""{"Scores":[1,2]}""", "list.json"), classValue, schema, "game"));
        Assert.Equal("root.Scores", scores.Diagnostic.Path);
        Assert.Equal("Version 1 does not store a list.", scores.Diagnostic.Message);
        var pair = Assert.Throws<ConversionException>(() => DocumentBackend.Read((JsonObject)JsonReader.Parse("""{"Pair":{"A":1}}""", "record.json"), classValue, schema, "game"));
        Assert.Equal("root.Pair", pair.Diagnostic.Path);
        Assert.Equal("Version 1 does not store a record.", pair.Diagnostic.Message);
        var node = new ObjectNode(null, null);
        node.Add("Scores", new ListNode());
        var written = Assert.Throws<ConversionException>(() => DocumentBackend.Write(node, classValue, schema, "game"));
        Assert.Equal("root.Scores", written.Diagnostic.Path);
        Assert.Equal("Version 1 does not store a list.", written.Diagnostic.Message);
    }

    [Fact]
    public void Version1WritesAReferenceAsAPlainInteger()
    {
        var node = new ObjectNode(null, null);
        node.Add("Upkeep", new ReferenceNode("player-slot", 4));
        var document = DocumentBackend.Write(node, FormatSchema.Current.Get("TEnchantment"), FormatSchema.Current, "game");
        Assert.Equal(4, ((JsonInteger)document["Upkeep"]!).Value);
        var read = DocumentBackend.Read(document, FormatSchema.Current.Get("TEnchantment"), FormatSchema.Current, "game");
        Assert.IsType<IntegerNode>(read.Find("Upkeep"));
        var probe = new ReferenceProbe();
        TreeReader.Read(node, probe, new TestFactory());
        Assert.Equal(4, probe.Key);
        var fromDocument = new ReferenceProbe();
        TreeReader.Read(read, fromDocument, new TestFactory());
        Assert.Equal(4, fromDocument.Key);
    }

    private static JsonObject Write(Sample sample) =>
        DocumentBackend.Write(TreeWriter.Write(sample, new TestFactory()), SampleClass, Schema, "game");

    private static Sample RoundTrip(Sample sample)
    {
        var loaded = new Sample();
        TreeReader.Read(DocumentBackend.Read(Write(sample), SampleClass, Schema, "game"), loaded, new TestFactory());
        return loaded;
    }

    private static Sample Full()
    {
        var sample = new Sample
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
        Assert.Equal(expected.Kids.Select(k => k.Number).ToList(), actual.Kids.Select(k => k.Number).ToList());
        Assert.Equal(expected.Made is Inner made ? made.Number : -1, actual.Made is Inner loaded ? loaded.Number : -1);
        Assert.Equal(expected.Fixed.Number, actual.Fixed.Number);
    }

    private sealed class Inner : IPersistable
    {
        public int Number;

        public void Persist(IPersister persister) => persister.Int32("N", ref Number);
    }

    private sealed class Sample : IPersistable
    {
        public int Number;
        public int NumberFallback = 7;
        public byte Small;
        public byte SmallFallback = 3;
        public sbyte SignedByte;
        public ushort Word;
        public ushort WordFallback = 9;
        public bool Flag;
        public string Title = "";
        public string Note = "";
        public int Player;
        public byte[] Blob = new byte[4];
        public int PairA;
        public int PairB = 4;
        public bool SkipNote;
        public bool HadNote;

        public Inner Fixed { get; } = new();

        public IPersistable? Made;

        public List<string> Message { get; } = [];
        public List<string> EditorLines { get; } = [];
        public List<int> EditorObjects { get; } = [];
        public List<int> Scores { get; } = [];
        public List<bool> Flags { get; } = [];
        public List<Inner> Kids { get; } = [];

        public void Persist(IPersister persister)
        {
            if (persister.Loading) Kids.Clear();
            persister.Reference("Player", "player-slot", ref Player);
            persister.List("Flags", Flags, FlagItem);
            persister.List("Scores", Scores, ScoreItem);
            persister.Record("Pair", WritePair);
            persister.Children("Children", new KidList(Kids));
            persister.Created("Made", ref Made);
            persister.Object("Fixed", Fixed);
            persister.Bytes("Blob", Blob);
            persister.LinesWithObjects("Editors", EditorLines, EditorObjects);
            persister.Lines("Message", Message);
            persister.String("Title", ref Title);
            persister.Boolean("Flag", ref Flag);
            persister.UInt16("WordFallback", ref WordFallback, 9);
            persister.UInt16("Word", ref Word);
            persister.SByte("SignedByte", ref SignedByte);
            persister.Byte("SmallFallback", ref SmallFallback, 3);
            persister.Byte("Small", ref Small);
            persister.Int32("NumberFallback", ref NumberFallback, 7);
            persister.Int32("Number", ref Number);
            if (SkipNote) HadNote = persister.Has("Note");
            else persister.String("Note", ref Note);
        }

        private void WritePair(IPersister record)
        {
            record.Int32("B", ref PairB, 4);
            record.Int32("A", ref PairA);
        }

        private static void ScoreItem(IItemSlot slot, ref int value) => slot.Int32(ref value);

        private static void FlagItem(IItemSlot slot, ref bool value) => slot.Boolean(ref value);
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
        public byte[] Data = [];

        public void Persist(IPersister persister) => persister.Bytes("Blob", Data);
    }

    private sealed class Twice : IPersistable
    {
        public int Number;

        public void Persist(IPersister persister)
        {
            persister.Int32("Number", ref Number);
            persister.Int32("Number", ref Number);
        }
    }

    private sealed class ReferenceProbe : IPersistable
    {
        public int Key;

        public void Persist(IPersister persister) => persister.Reference("Upkeep", "player-slot", ref Key);
    }

    private sealed class TestFactory : IPersistableFactory
    {
        public string ClassName(IPersistable objectValue) => NameTable.Stored(objectValue.GetType());

        public IPersistable Create(string className)
        {
            if (NameTable.Names(typeof(Inner), className)) return new Inner();
            if (NameTable.Names(typeof(Sample), className)) return new Sample();
            throw new InvalidOperationException("The test factory has no class " + className + ".");
        }
    }
}
