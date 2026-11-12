using System.Text;
using Aow.Formats.Json;
using Aow.Formats.Migrations;
using Aow.Formats.Original;
using Aow.Formats.Schema;

namespace Aow.Formats.Tests;

/// <summary>
/// Version 4 lists on the real schema: a bit list as set indices with residue, enum names in a
/// byte list, the groups of a unit index list collection and the map coordinates of a move path.
/// Each shape refuses a malformed value with its path. The version 3 step rewrites the version 3
/// values and keeps a list encoding the writer does not make.
/// </summary>
public sealed class ListShapeTests : FormatsTest
{
    // Count 13: bits 0, 2 and 12 set, bit 15 is residue past the count.
    private static readonly byte[] s_bits = [0, 13, 0, 0, 0, 0x05, 0x90];

    [Fact]
    public void ABitListStoresItsSetIndicesAndResidue()
    {
        var value = Decode("Player", "AIValidAttackPlayers", s_bits);
        Assert.Equal("""{"default":false,"count":13,"bits":[0,2,12],"$reserved":"80"}""", Compact(value));
        Assert.Equal(s_bits, Encode("Player", "AIValidAttackPlayers", value));
        byte[] odd = [2, 0, 0, 0, 0];
        var other = Decode("Sign", "TextSeen", odd);
        Assert.Equal("""{"default":2,"count":0,"bits":[]}""", Compact(other));
        Assert.Equal(odd, Encode("Sign", "TextSeen", other));
    }

    [Theory]
    [InlineData("""{"default":false,"count":13,"bits":[0,13]}""", Codes.Range, "root.VictoryLocations.bits[1]")]
    [InlineData("""{"default":false,"count":13,"bits":[2,0]}""", Codes.Range, "root.VictoryLocations.bits[1]")]
    [InlineData("""{"default":false,"count":13,"bits":[2,2]}""", Codes.Duplicate, "root.VictoryLocations.bits[1]")]
    [InlineData("""{"default":false,"count":16,"bits":[],"$reserved":"80"}""", Codes.Range, "root.VictoryLocations.$reserved")]
    [InlineData("""{"default":false,"count":13,"bits":[],"$reserved":"01"}""", Codes.Range, "root.VictoryLocations.$reserved")]
    [InlineData("""{"default":false,"count":13,"bits":"0590"}""", Codes.Type, "root.VictoryLocations.bits")]
    [InlineData("""{"default":false,"count":13,"bits":[],"extra":1}""", Codes.UnknownMember, "root.VictoryLocations.extra")]
    public void AMalformedBitListIsRefused(string json, string code, string path) =>
        Refused("PlayerStructure", "VictoryLocations", json, code, path);

    [Fact]
    public void EnumListsStoreNames()
    {
        byte[] relations = [3, 4, 0, 0, 0, 1, 0, 2, 7];
        var value = Decode("PlayerDiplomaticRelations", "Relations", relations);
        Assert.Equal("""{"default":"Unknown","items":["War","Peace","Alliance",7]}""", Compact(value));
        Assert.Equal(relations, Encode("PlayerDiplomaticRelations", "Relations", value));
        byte[] spheres = [0, 3, 0, 0, 0, 3, 7, 0, 1];
        var picks = Decode("Leader", "Spheres", spheres);
        Assert.Equal("""{"default":"Cosmos","items":["Earth","Secret","Cosmos"]}""", Compact(picks));
        Assert.Equal(spheres, Encode("Leader", "Spheres", picks));
    }

    [Fact]
    public void AnUnknownEnumNameIsRefused()
    {
        Refused("PlayerDiplomaticRelations", "Relations", """{"default":"Unknown","items":["Friendly"]}""", Codes.Type, "root.Relations.items[0]");
        Refused("Race", "DefaultRelations", """{"default":"Nobody","items":[]}""", Codes.Type, "root.DefaultRelations.default");
    }

    [Fact]
    public void ACollectionStoresTrimmedGroups()
    {
        long[] flat = [198, 201, 199, -1, -1, -1, -1, -1, 203, 200, 200, -1, -1, -1, -1, -1, -1, -1, -1];
        var bytes = RunList(-1, flat);
        var value = Decode("StructureResource", "Defenders", bytes);
        Assert.Equal("""{"groups":[[198,201,199],[203,200,200],[]],"count":19}""", Compact(value));
        Assert.Equal(bytes, Encode("StructureResource", "Defenders", value));
        long[] full = [1, -1, 2, -1, -1, -1, -1, -1, 4, 4, 4, 4, 4, 4, 4, 4];
        var other = Decode("ExplorationSiteResource", "DefenderCollections", RunList(5, full));
        Assert.Equal("""{"default":5,"groups":[[1,-1,2],[4,4,4,4,4,4,4,4]]}""", Compact(other));
        Assert.Equal(RunList(5, full), Encode("ExplorationSiteResource", "DefenderCollections", other));
        var empty = Decode("StructureResource", "Defenders", RunList(-1, []));
        Assert.Equal("""{"groups":[]}""", Compact(empty));
    }

    [Theory]
    [InlineData("""{"groups":[[1,2,3,4,5,6,7,8,9]]}""", Codes.Range, "root.Defenders.groups[0]")]
    [InlineData("""{"groups":[[1],[2]],"count":17}""", Codes.Range, "root.Defenders.count")]
    [InlineData("""{"groups":[[1],[2]],"count":8}""", Codes.Range, "root.Defenders.count")]
    [InlineData("""{"groups":[[1],[1,2,3]],"count":10}""", Codes.Range, "root.Defenders.groups[1]")]
    [InlineData("""{"groups":[["a"]]}""", Codes.Type, "root.Defenders.groups[0][0]")]
    [InlineData("""{"groups":[3]}""", Codes.Type, "root.Defenders.groups[0]")]
    [InlineData("""{"count":0}""", Codes.MissingMember, "root.Defenders")]
    [InlineData("""{"groups":[],"$slot":1}""", Codes.ReservedMember, "root.Defenders.$slot")]
    public void AMalformedCollectionIsRefused(string json, string code, string path) =>
        Refused("StructureResource", "Defenders", json, code, path);

    [Fact]
    public void AMovePathStoresMapCoordinates()
    {
        var bytes = RunList(0, [0x03000201, 0, 0, unchecked((int)0xFF0001FE)]);
        var value = Decode("MoveTokenEvent", "Path", bytes);
        Assert.Equal("""{"default":{},"items":[{"X":1,"Y":2,"Tag":3},{},{},{"X":-2,"Y":1,"Tag":-1}]}""", Compact(value));
        Assert.Equal(bytes, Encode("MoveTokenEvent", "Path", value));
        Refused("MoveTokenEvent", "Path", """{"default":{},"items":[{"Z":1}]}""", Codes.UnknownMember, "root.Path.items[0].Z");
    }

    [Fact]
    public void AUnitIndexListIsAListWithADefault()
    {
        var bytes = RunList(-1, [12, -1, -1, 40]);
        var value = Decode("RaceResource", "UnitIndexes", bytes);
        Assert.Equal("""{"default":-1,"items":[12,-1,-1,40]}""", Compact(value));
        Assert.Equal(bytes, Encode("RaceResource", "UnitIndexes", value));
        var relations = Decode("Player", "LegacyNewRelations", [3, 2, 0, 0, 0, 2, 3, 1]);
        Assert.Equal("""{"default":"Unknown","items":["Alliance","Unknown"]}""", Compact(relations));
    }

    [Fact]
    public void EveryPinnedPropertyIsInTheVersion3Schema()
    {
        Assert.Equal(16, ListMigration.Pinned.Count);
        foreach (string key in ListMigration.Pinned)
        {
            int dot = key.LastIndexOf('.');
            var classValue = ListMigration.Version3.Get(key[..dot]);
            var property = classValue.PropertyByName(key[(dot + 1)..]);
            Assert.NotNull(property);
            Assert.Equal(classValue.Name, property.Owner.Name);
            Assert.Equal(3, ListMigration.Version3.Version);
        }
    }

    [Fact]
    public void Version3ValuesBecomeTheVersion4Shapes()
    {
        var read = Read(Envelope(new JsonObject()
            .Add("$class", new JsonString("Player"))
            .Add("LegacyNewRelations", new JsonString("0302000000" + "020301"))
            .Add("AIValidAttackPlayers", new JsonObject()
                .Add("default", new JsonInteger(0))
                .Add("count", new JsonInteger(13))
                .Add("bits", new JsonString("0590")))));
        Assert.Equal([ListMigration.Name, ResourceMigration.Name], read.Migrations);
        Assert.Equal(5, ((JsonInteger)read.Document["$version"]!).Value);
        var root = (JsonObject)read.Document["root"]!;
        Assert.Equal("""{"default":"Unknown","items":["Alliance","Unknown"]}""", Compact(root["LegacyNewRelations"]!));
        Assert.Equal("""{"default":false,"count":13,"bits":[0,2,12],"$reserved":"80"}""", Compact(root["AIValidAttackPlayers"]!));
        var leader = Read(Envelope(new JsonObject()
            .Add("$class", new JsonString("Leader"))
            .Add("Spheres", new JsonObject()
                .Add("default", new JsonInteger(0))
                .Add("items", new JsonArray().Add(new JsonInteger(3)).Add(new JsonInteger(9))))));
        Assert.Equal("""{"default":"Cosmos","items":["Earth",9]}""", Compact(((JsonObject)leader.Document["root"]!)["Spheres"]!));
    }

    [Fact]
    public void AListEncodingTheWriterDoesNotMakeBecomesALayoutPart()
    {
        // Items 5, -1, -1, 7 with the two defaults stored as two runs of one.
        string split = "ffffffff" + "04000000" + "05000000" + "ffffffff01000000" + "ffffffff01000000" + "07000000";
        var read = Read(Envelope(new JsonObject()
            .Add("$class", new JsonString("RaceResource"))
            .Add("UnitIndexes", new JsonString(split))));
        var root = (JsonObject)read.Document["root"]!;
        Assert.Equal("""{"default":-1,"items":[5,-1,-1,7]}""", Compact(root["UnitIndexes"]!));
        var context = new EncodeContext(FormatSchema.Current, "game", "hero-list", "lists.json", FormatLimits.Default, ExportOptions.Default);
        var reader = LayoutReader.Read(context, root, DocumentNodePath.Root)!;
        Assert.Equal(split, System.Convert.ToHexStringLower(reader.Encodings[10].Bytes));
        var property = FormatSchema.Current.Get("RaceResource").PropertyByName("UnitIndexes")!;
        var data = new ByteBuffer();
        property.Codec.Encode(context, root["UnitIndexes"]!, data, new ValueSite { Property = property, Id = 10, Path = DocumentNodePath.Root.Member("UnitIndexes"), Owner = root, Reader = reader });
        Assert.Equal(split, System.Convert.ToHexStringLower(data.ToArray()));
    }

    [Fact]
    public void Version3HexThatIsNotHexIsRefused()
    {
        var error = Assert.Throws<ConversionException>(() => Read(Envelope(new JsonObject()
            .Add("$class", new JsonString("RaceResource"))
            .Add("UnitIndexes", new JsonInteger(5)))));
        Assert.Equal(Codes.Type, error.Diagnostic.Code);
        Assert.Equal("root.UnitIndexes", error.Diagnostic.Path);
    }

    [Fact]
    public void TheVersion3StepDoesNotReadTheCurrentSchema()
    {
        JsonObject Document() => Envelope(new JsonObject()
            .Add("$class", new JsonString("RaceResource"))
            .Add("UnitIndexes", new JsonString("ffffffff0100000009000000")));
        var first = ListMigration.Apply(Document());
        FormatSchema.Substituted = FormatSchema.Load(Encoding.UTF8.GetBytes("""{"version":4,"classes":[]}"""), "empty.json");
        try
        {
            Assert.Equal(JsonWriter.ToUtf8(first), JsonWriter.ToUtf8(ListMigration.Apply(Document())));
        }
        finally
        {
            FormatSchema.Substituted = null;
        }
        Assert.Equal("""{"default":-1,"items":[9]}""", Compact(((JsonObject)first["root"]!)["UnitIndexes"]!));
    }

    private static SchemaProperty Property(string className, string property) =>
        FormatSchema.Current.Get(className).PropertyByName(property) ?? throw new InvalidOperationException(className + " has no " + property + ".");

    private static JsonValue Decode(string className, string property, byte[] bytes)
    {
        var prop = Property(className, property);
        var context = new DecodeContext(bytes, FormatSchema.Current, "game", "world-hsm", "lists.json", FormatLimits.Default, new LayoutCounts());
        var builder = new LayoutBuilder();
        var site = new ValueSite { Property = prop, Id = prop.Id ?? 0, Path = DocumentNodePath.Root.Member(property), Owner = new JsonObject(), Builder = builder };
        var value = prop.Codec.Decode(context, 0, bytes.Length, site);
        Assert.Empty(builder.Parts);
        return value;
    }

    private static byte[] Encode(string className, string property, JsonValue value)
    {
        var prop = Property(className, property);
        var context = new EncodeContext(FormatSchema.Current, "game", "world-hsm", "lists.json", FormatLimits.Default, ExportOptions.Default);
        var data = new ByteBuffer();
        prop.Codec.Encode(context, value, data, new ValueSite { Property = prop, Id = prop.Id ?? 0, Path = DocumentNodePath.Root.Member(property), Owner = new JsonObject() });
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

    private static byte[] RunList(int defaultValue, long[] items)
    {
        var data = new ByteBuffer();
        data.Int32(defaultValue);
        data.Int32(items.Length);
        int i = 0;
        while (i < items.Length)
        {
            if (items[i] != defaultValue)
            {
                data.Int32((int)items[i++]);
                continue;
            }
            int run = 0;
            while (i < items.Length && items[i] == defaultValue)
            {
                run++;
                i++;
            }
            data.Int32(defaultValue);
            data.Int32(run);
        }
        return data.ToArray();
    }

    private static ReadResult Read(JsonObject document) =>
        GameFiles.Read(JsonWriter.ToUtf8(document), "lists.json");

    private static JsonObject Envelope(JsonObject root) => new JsonObject()
        .Add("$format", new JsonString(OwnFormat.FormatName))
        .Add("$version", new JsonInteger(3))
        .Add("$kind", new JsonString("hero-list"))
        .Add("$container", new JsonObject()
            .Add("profile", new JsonString("hero-list"))
            .Add("compression", new JsonString("none")))
        .Add("root", root);
}
