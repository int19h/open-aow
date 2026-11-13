using System.Buffers.Binary;
using System.IO.Compression;
using System.Text;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Tests;

/// <summary>
/// The map shapes on the real schema: the fields of a map level as a grid, the sprites of a field
/// as a created sequence at ids 50 and on, and the campaign map as a nested document. Each shape
/// round-trips through the document backend and the original backend and matches the converter.
/// A malformed value is refused with its path.
/// </summary>
public sealed class MapShapeTests : FormatsTest
{
    private static readonly ProbeFactory s_factory = new();

    private static FormatSchema Schema => FormatSchema.Current;

    [Theory]
    [InlineData("MapLevel", "game")]
    [InlineData("CombatMapLevel", "tactical")]
    public void ALevelRoundTripsThroughBothBackendsAndMatchesTheConverter(string className, string engine)
    {
        var classValue = Schema.Get(className);
        var source = Level(width: 3, height: 2, spritesPerField: [0, 1, 2, 0, 3, 1]);
        source.Fields[1].Sprites.Add(new SpriteProbe { DaysRemaining = 9, Shared = true });
        var tree = TreeWriter.Write(source, s_factory);
        Assert.Equal(6, source.Visited);
        var document = DocumentBackend.Write(tree, classValue, Schema, engine);
        var image = OriginalBackend.Write(tree, classValue, Schema, engine);
        Assert.Equal(Compact(PilotImage.Decode(image, className, engine)), Compact(document));
        Assert.Equal(image, PilotImage.Encode(document, className, engine));
        var rows = (JsonArray)document["Fields"]!;
        Assert.Equal(2, rows.Items.Count);
        Assert.Equal(3, ((JsonArray)rows.Items[0]).Items.Count);
        // A shared sprite is not stored, and a stored sprite carries its class and no slot.
        Assert.Equal("""{"HexSpriteList":[{"$class":"FrozenWaterHexSprite","DaysRemaining":1,"CasterPlayer":-1}],"FieldFlags":1007}""",
            Compact(((JsonArray)rows.Items[0]).Items[1]));
        Assert.Equal("""{"FieldFlags":7}""", Compact(((JsonArray)rows.Items[0]).Items[0]));
        foreach (var read in new[] { OriginalBackend.Read(image, classValue, Schema, engine), DocumentBackend.Read(document, classValue, Schema, engine) })
        {
            var loaded = new LevelProbe();
            TreeReader.Read(read, loaded, s_factory);
            Assert.Equal(6, loaded.Visited);
            Assert.Equal(Describe(source, storedOnly: true), Describe(loaded, storedOnly: true));
        }
    }

    [Fact]
    public void TheSpritesOfAFieldAreStoredAtIdsFromFifty()
    {
        var classValue = Schema.Get("MapLevel");
        var tree = TreeWriter.Write(Level(width: 1, height: 1, spritesPerField: [3]), s_factory);
        var image = OriginalBackend.Write(tree, classValue, Schema, "game");
        // The field image is the value of id 6: its 32-bit length, then its table.
        var field = FieldImage(image);
        Assert.Equal(4, field[0]);
        Assert.Equal([50, 51, 52, 4], new[] { field[1], field[3], field[5], field[7] });
    }

    [Fact]
    public void AnEmptyGridIsStoredAndAnAbsentGridLeavesTheFields()
    {
        var classValue = Schema.Get("MapLevel");
        var tree = TreeWriter.Write(new LevelProbe(), s_factory);
        var document = DocumentBackend.Write(tree, classValue, Schema, "game");
        Assert.Equal("""{"Fields":[]}""", Compact(document));
        var image = OriginalBackend.Write(tree, classValue, Schema, "game");
        Assert.Equal(image, PilotImage.Encode(document, "MapLevel"));
        var loaded = new LevelProbe();
        TreeReader.Read(OriginalBackend.Read(image, classValue, Schema, "game"), loaded, s_factory);
        Assert.Empty(loaded.Fields);
        var untouched = Level(width: 1, height: 1, spritesPerField: [1]);
        var absent = (JsonObject)JsonReader.Parse("""{"Width":1,"Height":1}"""u8, "map.json");
        TreeReader.Read(DocumentBackend.Read(absent, classValue, Schema, "game"), untouched, s_factory);
        Assert.Single(untouched.Fields[0].Sprites);
        Assert.Equal(0, untouched.Visited);
    }

    [Fact]
    public void AStoredNullSpriteRoundTripsAndLoadsNothing()
    {
        var classValue = Schema.Get("MapLevel");
        var document = (JsonObject)JsonReader.Parse("""{"Width":1,"Height":1,"Fields":[[{"HexSpriteList":[{"$class":null},{"$class":"FrozenWaterHexSprite","DaysRemaining":2}]}]]}"""u8, "map.json");
        var tree = DocumentBackend.Read(document, classValue, Schema, "game");
        Assert.Equal(Compact(document), Compact(DocumentBackend.Write(tree, classValue, Schema, "game")));
        var image = OriginalBackend.Write(tree, classValue, Schema, "game");
        Assert.Equal(image, PilotImage.Encode(document, "MapLevel"));
        Assert.Equal(Compact(document), Compact(DocumentBackend.Write(OriginalBackend.Read(image, classValue, Schema, "game"), classValue, Schema, "game")));
        var loaded = new LevelProbe();
        TreeReader.Read(tree, loaded, s_factory);
        Assert.Equal(2, Assert.Single(loaded.Fields[0].Sprites).DaysRemaining);
    }

    [Theory]
    [InlineData("""{"Width":1,"Height":2,"Fields":[[{}]]}""", Codes.Dimensions, "root.Fields")]
    [InlineData("""{"Width":2,"Height":1,"Fields":[[{}]]}""", Codes.Dimensions, "root.Fields[0]")]
    [InlineData("""{"Width":1,"Height":1,"Fields":[{}]}""", Codes.Type, "root.Fields[0]")]
    [InlineData("""{"Width":1,"Height":1,"Fields":[[1]]}""", Codes.Type, "root.Fields[0][0]")]
    [InlineData("""{"Width":1,"Height":1,"Fields":{}}""", Codes.Type, "root.Fields")]
    [InlineData("""{"Width":1,"Height":1,"Fields":[[{"HexSpriteList":{}}]]}""", Codes.Type, "root.Fields[0][0].HexSpriteList")]
    [InlineData("""{"Width":1,"Height":1,"Fields":[[{"HexSpriteList":[1]}]]}""", Codes.Type, "root.Fields[0][0].HexSpriteList[0]")]
    [InlineData("""{"Width":1,"Height":1,"Fields":[[{"HexSpriteList":[{"$class":"FrozenWaterHexSprite","$slot":0}]}]]}""", Codes.ReservedMember, "root.Fields[0][0].HexSpriteList[0].$slot")]
    [InlineData("""{"Width":1,"Height":1,"Fields":[[{"HexSpriteList":[{"$class":null,"DaysRemaining":1}]}]]}""", Codes.UnknownMember, "root.Fields[0][0].HexSpriteList[0].DaysRemaining")]
    [InlineData("""{"Width":1,"Height":1,"Fields":[[{"HexSpriteList":[{"DaysRemaining":1}]}]]}""", Codes.MissingMember, "root.Fields[0][0].HexSpriteList[0]")]
    [InlineData("""{"Width":1,"Height":1,"Fields":[[{"HexSpriteList":[{"$class":"NoSuchSprite"}]}]]}""", Codes.UnknownClass, "root.Fields[0][0].HexSpriteList[0]")]
    [InlineData("""{"Width":1,"Height":1,"Fields":[[{"Height":1}]]}""", Codes.UnknownMember, "root.Fields[0][0].Height")]
    public void AMalformedGridIsRefusedWithItsPath(string text, string code, string path)
    {
        var document = (JsonObject)JsonReader.Parse(Encoding.UTF8.GetBytes(text), "map.json");
        var error = Assert.Throws<ConversionException>(() => DocumentBackend.Read(document, Schema.Get("MapLevel"), Schema, "game"));
        Assert.Equal(code, error.Diagnostic.Code);
        Assert.Equal(path, error.Diagnostic.Path);
    }

    [Fact]
    public void AGridThatDisagreesWithTheLevelIsRefusedOnBothBackends()
    {
        var classValue = Schema.Get("MapLevel");
        var tree = TreeWriter.Write(new MismatchedLevel(), s_factory);
        var document = Assert.Throws<ConversionException>(() => DocumentBackend.Write(tree, classValue, Schema, "game"));
        Assert.Equal((Codes.Dimensions, "root.Fields"), (document.Diagnostic.Code, document.Diagnostic.Path));
        var image = Assert.Throws<ConversionException>(() => OriginalBackend.Write(tree, classValue, Schema, "game"));
        Assert.Equal((Codes.Dimensions, "root.Fields"), (image.Diagnostic.Code, image.Diagnostic.Path));
        var count = Assert.Throws<ConversionException>(() => TreeWriter.Write(new LevelProbe { Width = 2, Height = 2 }, s_factory));
        Assert.Equal((Codes.Dimensions, "root.Fields"), (count.Diagnostic.Code, count.Diagnostic.Path));
        var stored = TreeWriter.Write(Level(width: 2, height: 1, spritesPerField: [0, 0]), s_factory);
        var load = Assert.Throws<ConversionException>(() => TreeReader.Read(stored, new MismatchedLevel(), s_factory));
        Assert.Equal((Codes.Dimensions, "root.Fields"), (load.Diagnostic.Code, load.Diagnostic.Path));
    }

    [Fact]
    public void AMalformedFieldStreamIsRefusedOnTheImage()
    {
        var classValue = Schema.Get("MapLevel");
        var level = new LevelProbe { Width = 1, Height = 1, Fields = [new FieldProbe()] };
        var image = OriginalBackend.Write(TreeWriter.Write(level, s_factory), classValue, Schema, "game");
        // The last value is the field: a length of 1 and an empty table.
        Assert.Equal([1, 0, 0, 0, 0], image[^5..]);
        var empty = image.ToArray();
        empty[^5] = 0;
        var zero = Assert.Throws<ConversionException>(() => OriginalBackend.Read(empty, classValue, Schema, "game"));
        Assert.Equal((Codes.Corrupt, "root.Fields[0][0]"), (zero.Diagnostic.Code, zero.Diagnostic.Path));
        var longer = Assert.Throws<ConversionException>(() => OriginalBackend.Read([.. image, 0], classValue, Schema, "game"));
        Assert.Equal((Codes.Dimensions, "root.Fields"), (longer.Diagnostic.Code, longer.Diagnostic.Path));
    }

    [Theory]
    [InlineData("none", 0)]
    [InlineData("cfs", 0)]
    [InlineData("cfs", 1)]
    [InlineData("cfs", 2)]
    [InlineData("cfs", 3)]
    public void ACampaignSaveWithANestedMapPassesBothBackends(string compression, int level)
    {
        var classValue = Schema.Get("Campaign");
        var nested = NestedMap(compression, level);
        var file = CampaignSave(nested);
        var imported = (JsonObject)GameFiles.Import(file, FileProfile.Find("campaign")!, "campaign-save").Document["root"]!;
        var map = (JsonObject)imported["Map"]!;
        Assert.Equal(compression == "cfs" && level != 0, map.Find("$layout") is not null);
        imported.Members.RemoveAll(member => member.Name == "$class");
        var image = PilotImage.Encode(imported, "Campaign", "campaign");
        var read = OriginalBackend.Read(image, classValue, Schema, "campaign");
        var loaded = new CampaignProbe();
        TreeReader.Read(read, loaded, s_factory);
        Assert.Equal(nested, loaded.Map);
        Assert.Equal("Fixture campaign", loaded.Name);
        var tree = TreeWriter.Write(loaded, s_factory);
        Assert.Equal(image, OriginalBackend.Write(tree, classValue, Schema, "campaign"));
        // The direct document holds the nested document without the converter's retained bytes.
        map.Members.RemoveAll(member => member.Name == "$layout");
        var document = DocumentBackend.Write(tree, classValue, Schema, "campaign");
        Assert.Equal(Compact(imported), Compact(document));
        var again = new CampaignProbe();
        TreeReader.Read(DocumentBackend.Read(document, classValue, Schema, "campaign"), again, s_factory);
        Assert.Equal(nested, again.Map);
        Assert.Equal(image, OriginalBackend.Write(TreeWriter.Write(again, s_factory), classValue, Schema, "campaign"));
    }

    [Fact]
    public void TheRetainedCompressedBytesStayTheConvertersAlone()
    {
        var classValue = Schema.Get("Campaign");
        // A level 2 map compressed by another deflater: the bytes differ from a fresh compression.
        var payload = Payload(NestedMap("none", 0));
        using var buffer = new MemoryStream();
        using (var zlib = new ZLibStream(buffer, CompressionLevel.NoCompression, leaveOpen: true)) zlib.Write(payload);
        byte[] nested = [.. Cfs.Magic, 2, .. buffer.ToArray()];
        Assert.NotEqual(nested, Cfs.Wrap(payload, 2));
        var file = CampaignSave(nested);
        var document = GameFiles.Import(file, FileProfile.Find("campaign")!, "campaign-save").Document;
        Assert.Equal(file, GameFiles.Export(document).Bytes);
        var root = (JsonObject)document["root"]!;
        root.Members.RemoveAll(member => member.Name == "$class");
        var image = PilotImage.Encode(root, "Campaign", "campaign");
        var tree = OriginalBackend.Read(image, classValue, Schema, "campaign");
        Assert.Equal(image, OriginalBackend.Write(tree, classValue, Schema, "campaign"));
        var direct = DocumentBackend.Read(DocumentBackend.Write(tree, classValue, Schema, "campaign"), classValue, Schema, "campaign");
        var loaded = new CampaignProbe();
        TreeReader.Read(direct, loaded, s_factory);
        Assert.Equal(Cfs.Wrap(payload, 2), loaded.Map);
    }

    [Fact]
    public void AnEmptyCampaignMapIsOmittedAndAnAbsentOneIsEmpty()
    {
        var classValue = Schema.Get("Campaign");
        var tree = TreeWriter.Write(new CampaignProbe { Name = "Empty" }, s_factory);
        Assert.Null(tree.Find("Map"));
        Assert.Equal("""{"Name":"Empty"}""", Compact(DocumentBackend.Write(tree, classValue, Schema, "campaign")));
        var loaded = new CampaignProbe { Map = [1, 2, 3] };
        TreeReader.Read(OriginalBackend.Read(OriginalBackend.Write(tree, classValue, Schema, "campaign"), classValue, Schema, "campaign"), loaded, s_factory);
        Assert.Empty(loaded.Map);
    }

    [Theory]
    [InlineData("""{"Map":{"root":{"$class":"StrategicMap"}}}""", Codes.MissingMember, "root.Map")]
    [InlineData("""{"Map":{"$container":{"profile":"world-hsm","compression":"none"}}}""", Codes.MissingMember, "root.Map")]
    [InlineData("""{"Map":{"$container":{"profile":"tactical-hsm","compression":"none"},"root":{"$class":"StrategicMap"}}}""", Codes.Profile, "root.Map.$container.profile")]
    [InlineData("""{"Map":{"$container":{"profile":"world-hsm","compression":"none"},"root":{"$class":"StrategicMap"},"Extra":1}}""", Codes.UnknownMember, "root.Map.Extra")]
    [InlineData("""{"Map":{"$container":{"profile":"world-hsm","compression":"none"},"root":{"$class":"StrategicMap"},"$slot":1}}""", Codes.ReservedMember, "root.Map.$slot")]
    [InlineData("""{"Map":"00"}""", Codes.Type, "root.Map")]
    public void AMalformedCampaignMapIsRefusedWithItsPath(string text, string code, string path)
    {
        var document = (JsonObject)JsonReader.Parse(Encoding.UTF8.GetBytes(text), "campaign.json");
        var error = Assert.Throws<ConversionException>(() => DocumentBackend.Read(document, Schema.Get("Campaign"), Schema, "campaign"));
        Assert.Equal(code, error.Diagnostic.Code);
        Assert.Equal(path, error.Diagnostic.Path);
    }

    [Fact]
    public void AMalformedCampaignMapIsRefusedOnTheImage()
    {
        var classValue = Schema.Get("Campaign");
        var image = OriginalBackend.Write(TreeWriter.Write(new CampaignProbe { Map = NestedMap("none", 0) }, s_factory), classValue, Schema, "campaign");
        // The table holds the size (id 5) at offset 0 and the bytes (id 6) at offset 4.
        Assert.Equal([2, 5, 0, 6, 4], image[..5]);
        var bigger = image.ToArray();
        BinaryPrimitives.WriteInt32LittleEndian(bigger.AsSpan(5), BinaryPrimitives.ReadInt32LittleEndian(bigger.AsSpan(5)) + 1);
        var size = Assert.Throws<ConversionException>(() => OriginalBackend.Read(bigger, classValue, Schema, "campaign"));
        Assert.Equal((Codes.Corrupt, "root.Map"), (size.Diagnostic.Code, size.Diagnostic.Path));
        byte[] half = [1, 5, 0, .. image[5..9]];
        var alone = Assert.Throws<ConversionException>(() => OriginalBackend.Read(half, classValue, Schema, "campaign"));
        Assert.Equal((Codes.Corrupt, "root.Map"), (alone.Diagnostic.Code, alone.Diagnostic.Path));
        // The document backend imports the stored file through its profile, which refuses a file that is not a map.
        var stored = TreeWriter.Write(new CampaignProbe { Map = [9] }, s_factory);
        var junk = Assert.Throws<ConversionException>(() => DocumentBackend.Write(stored, classValue, Schema, "campaign"));
        Assert.Equal(Codes.Truncated, junk.Diagnostic.Code);
    }

    // A sign is a sprite of a field. Its bit list of the text seen by each player is on the direct path too.
    [Theory]
    [InlineData("""{"TextSeen":{"default":false,"count":10,"bits":[1,9],"$reserved":"fc"}}""")]
    [InlineData("""{"TextSeen":{"default":2,"count":0,"bits":[]}}""")]
    [InlineData("""{"TextSeen":{"default":true,"count":16,"bits":[0,15]}}""")]
    public void ABitListRoundTripsThroughBothBackends(string text)
    {
        var classValue = Schema.Get("Sign");
        var document = (JsonObject)JsonReader.Parse(Encoding.UTF8.GetBytes(text), "sign.json");
        var tree = DocumentBackend.Read(document, classValue, Schema, "game");
        Assert.Equal(Compact(document), Compact(DocumentBackend.Write(tree, classValue, Schema, "game")));
        var image = OriginalBackend.Write(tree, classValue, Schema, "game");
        Assert.Equal(image, PilotImage.Encode(document, "Sign"));
        Assert.Equal(Compact(document), Compact(DocumentBackend.Write(OriginalBackend.Read(image, classValue, Schema, "game"), classValue, Schema, "game")));
    }

    [Theory]
    [InlineData("""{"TextSeen":{"default":false,"count":10,"bits":[10]}}""", Codes.Range, "root.TextSeen.bits[0]")]
    [InlineData("""{"TextSeen":{"default":false,"count":10,"bits":[],"$reserved":"01"}}""", Codes.Range, "root.TextSeen.$reserved")]
    [InlineData("""{"TextSeen":{"default":false,"bits":[]}}""", Codes.Range, "root.TextSeen.count")]
    [InlineData("""{"TextSeen":{"default":false,"count":1,"bits":[],"extra":1}}""", Codes.UnknownMember, "root.TextSeen.extra")]
    public void AMalformedBitListIsRefusedWithItsPath(string text, string code, string path)
    {
        var document = (JsonObject)JsonReader.Parse(Encoding.UTF8.GetBytes(text), "sign.json");
        var error = Assert.Throws<ConversionException>(() => DocumentBackend.Read(document, Schema.Get("Sign"), Schema, "game"));
        Assert.Equal(code, error.Diagnostic.Code);
        Assert.Equal(path, error.Diagnostic.Path);
    }

    // A small world map: an author and a description, in the container the test names.
    private static byte[] NestedMap(string compression, int level)
    {
        var container = new JsonObject().Add("profile", new JsonString("world-hsm")).Add("compression", new JsonString(compression));
        if (compression == "cfs") container.Add("level", new JsonInteger(level));
        var root = new JsonObject()
            .Add("$class", new JsonString("StrategicMap"))
            .Add("Author", new JsonString("Fixture author"))
            .Add("Description", new JsonArray().Add(new JsonString("A nested map")).Add(new JsonString("of a campaign save")));
        return GameFiles.Export(Envelope("map", container, root)).Bytes;
    }

    private static byte[] Payload(byte[] file) => Cfs.HasMagic(file) ? file[5..] : file;

    private static byte[] CampaignSave(byte[] nested)
    {
        var map = GameFiles.Import(nested, FileProfile.Find("world-hsm")!, "map").Document;
        var nestedMap = new JsonObject().Add("$container", map["$container"]!).Add("root", map["root"]!);
        var container = new JsonObject().Add("profile", new JsonString("campaign")).Add("compression", new JsonString("none"));
        var root = new JsonObject()
            .Add("$class", new JsonString("Campaign"))
            .Add("Map", nestedMap)
            .Add("Name", new JsonString("Fixture campaign"));
        if (Cfs.HasMagic(nested) && nested[4] != 0)
        {
            var layout = new JsonObject()
                .Add("codec", new JsonString(LayoutResidue.Codec))
                .Add("fingerprint", new JsonString(LayoutResidue.Fingerprint(nestedMap, "world-hsm")))
                .Add("parts", new JsonArray().Add(new JsonObject()
                    .Add("category", new JsonString(LayoutResidue.NestedCompression))
                    .Add("bytes", new JsonString(System.Convert.ToHexStringLower(nested)))));
            nestedMap.Members.Insert(1, new JsonMember("$layout", layout));
        }
        return GameFiles.Export(Envelope("campaign-save", container, root)).Bytes;
    }

    private static JsonObject Envelope(string kind, JsonObject container, JsonObject root) => new JsonObject()
        .Add("$format", new JsonString(OwnFormat.FormatName))
        .Add("$version", new JsonInteger(OwnFormat.CurrentVersion))
        .Add("$kind", new JsonString(kind))
        .Add("$container", container)
        .Add("root", root);

    // The first field of a level image: the value of id 6 in the level's short table, less its length.
    private static byte[] FieldImage(byte[] image)
    {
        int count = image[0];
        for (int k = 0; k < count; k++)
            if (image[1 + 2 * k] == 6)
                return image[(1 + 2 * count + image[2 + 2 * k] + 4)..];
        throw new InvalidOperationException("The level stores no fields.");
    }

    private static LevelProbe Level(int width, int height, int[] spritesPerField)
    {
        var level = new LevelProbe { Width = (byte)width, Height = (byte)height, HexLevel = 1, Fields = new FieldProbe[width * height] };
        for (int i = 0; i < level.Fields.Length; i++)
        {
            var field = new FieldProbe { Flags = (ushort)(i * 1000 + 7) };
            for (int k = 0; k < spritesPerField[i]; k++)
                field.Sprites.Add(new SpriteProbe { DaysRemaining = (sbyte)(k + i), CasterPlayer = (sbyte)(k - 1) });
            level.Fields[i] = field;
        }
        return level;
    }

    private static string Describe(LevelProbe level, bool storedOnly) =>
        level.Width + "x" + level.Height + "@" + level.HexLevel + ":" + string.Join("|", level.Fields.Select(field =>
            field.Flags + "[" + string.Join(",", field.Sprites.Where(sprite => !storedOnly || !sprite.Shared).Select(sprite => sprite.DaysRemaining + "/" + sprite.CasterPlayer)) + "]"));

    private static string Compact(JsonValue value) => Encoding.UTF8.GetString(JsonWriter.ToUtf8(value)).Replace(" ", "").Replace("\n", "");

    private sealed class LevelProbe : IPersistable
    {
        public byte Width { get; set; }
        public byte Height { get; set; }
        public byte HexLevel { get; set; }
        public FieldProbe[] Fields { get; set; } = [];
        public int Visited { get; private set; }

        public void Persist(IPersister persister)
        {
            Width = persister.Byte("Width", Width);
            Height = persister.Byte("Height", Height);
            HexLevel = persister.Byte("HexLevel", HexLevel);
            if (persister.Loading)
            {
                if (!persister.Has("Fields")) return;
                Fields = new FieldProbe[Width * Height];
                for (int i = 0; i < Fields.Length; i++) Fields[i] = new FieldProbe();
            }
            persister.Grid("Fields", width: Width, height: Height, fields: Fields, visited: _ => Visited++);
        }
    }

    // A level that states a width of 3 and holds a grid of 1 by 1, on save and on load.
    private sealed class MismatchedLevel : IPersistable
    {
        private readonly FieldProbe[] _fields = [new FieldProbe()];

        public void Persist(IPersister persister)
        {
            persister.Byte("Width", 3);
            persister.Byte("Height", 1);
            persister.Grid("Fields", width: 1, height: 1, fields: _fields);
        }
    }

    private sealed class FieldProbe : IPersistable
    {
        public List<SpriteProbe> Sprites { get; } = [];
        public ushort Flags { get; set; }

        public void Persist(IPersister persister)
        {
            persister.CreatedSequence("HexSpriteList", new SpriteList(Sprites));
            Flags = persister.UInt16("FieldFlags", Flags);
        }
    }

    // The stored sprites of a field: a shared sprite is not stored, and a load adds each sprite in order.
    private sealed class SpriteList(List<SpriteProbe> sprites) : IChildCollection
    {
        public int Count => sprites.Count;

        public bool KeepIndex => false;

        public IPersistable? Get(int index) => sprites[index];

        public bool IsStored(int index) => !sprites[index].Shared;

        public int Slot(int index) => index;

        public void Attach(IPersistable child)
        {
        }

        public void Place(int slot, IPersistable child) => sprites.Add((SpriteProbe)child);
    }

    private sealed class SpriteProbe : IPersistable
    {
        public sbyte DaysRemaining { get; set; }
        public sbyte CasterPlayer { get; set; }
        public bool Shared { get; init; }

        public void Persist(IPersister persister)
        {
            DaysRemaining = persister.SByte("DaysRemaining", DaysRemaining);
            CasterPlayer = persister.SByte("CasterPlayer", CasterPlayer);
        }
    }

    private sealed class CampaignProbe : IPersistable
    {
        public byte[] Map { get; set; } = [];
        public string Name { get; set; } = "";

        public void Persist(IPersister persister)
        {
            Map = persister.NestedDocument("Map", profile: "world-hsm", image: Map);
            Name = persister.String("Name", Name);
        }
    }

    private sealed class ProbeFactory : IPersistableFactory
    {
        public string ClassName(IPersistable objectValue) => objectValue switch
        {
            SpriteProbe => "FrozenWaterHexSprite",
            _ => throw new InvalidOperationException("The test factory names no class for " + objectValue.GetType().Name + "."),
        };

        public IPersistable Create(string className) => className switch
        {
            "FrozenWaterHexSprite" => new SpriteProbe(),
            _ => throw new InvalidOperationException("The test factory has no class " + className + "."),
        };
    }
}
