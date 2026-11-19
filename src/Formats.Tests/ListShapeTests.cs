using System.Text;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;

namespace Aow.Formats.Tests;

/// <summary>
/// Lists on the real schema: a bit list as set indices with default-filled tail bits, enum names in a
/// byte list, the groups of a unit index list collection and the map coordinates of a move path.
/// Each shape refuses a malformed value with its path.
/// </summary>
public sealed class ListShapeTests : FormatsTest
{
    // Count 13, default false: bits 0, 2 and 12 set, tail bits 13..15 equal 0.
    private static readonly byte[] s_bits = [0, 13, 0, 0, 0, 0x05, 0x10];

    // Count 13, default true: bits 0, 2 and 12 set, tail bits 13..15 equal 1 (0xE0 | 0x10).
    private static readonly byte[] s_trueBits = [1, 13, 0, 0, 0, 0x05, 0xF0];

    [Fact]
    public void ABitListStoresItsSetIndicesAndDefaultTail()
    {
        JsonValue value = Decode("Player", "AIValidAttackPlayers", s_bits);
        Assert.Equal("""{"default":false,"count":13,"bits":[0,2,12]}""", Compact(value));
        Assert.Equal(s_bits, Encode("Player", "AIValidAttackPlayers", value));
        JsonValue trueValue = Decode("Player", "AIValidAttackPlayers", s_trueBits);
        Assert.Equal("""{"default":true,"count":13,"bits":[0,2,12]}""", Compact(trueValue));
        Assert.Equal(s_trueBits, Encode("Player", "AIValidAttackPlayers", trueValue));
        byte[] odd = [2, 0, 0, 0, 0];
        JsonValue other = Decode("Sign", "TextSeen", odd);
        Assert.Equal("""{"default":2,"count":0,"bits":[]}""", Compact(other));
        Assert.Equal(odd, Encode("Sign", "TextSeen", other));
    }

    [Fact]
    public void ABitListWithWrongTailBitsIsRefused()
    {
        byte[] falseBadTail = [0, 13, 0, 0, 0, 0x05, 0x90];
        ConversionException falseError = Assert.Throws<ConversionException>(() => Decode("Player", "AIValidAttackPlayers", falseBadTail));
        Assert.Equal(Codes.Range, falseError.Diagnostic.Code);
        Assert.Equal("root.AIValidAttackPlayers.bits", falseError.Diagnostic.Path);

        byte[] trueBadTail = [1, 13, 0, 0, 0, 0x05, 0x10];
        ConversionException trueError = Assert.Throws<ConversionException>(() => Decode("Player", "AIValidAttackPlayers", trueBadTail));
        Assert.Equal(Codes.Range, trueError.Diagnostic.Code);
        Assert.Equal("root.AIValidAttackPlayers.bits", trueError.Diagnostic.Path);
    }

    [Theory]
    [InlineData("""{"default":false,"count":13,"bits":[0,13]}""", Codes.Range, "root.VictoryLocations.bits[1]")]
    [InlineData("""{"default":false,"count":13,"bits":[2,0]}""", Codes.Range, "root.VictoryLocations.bits[1]")]
    [InlineData("""{"default":false,"count":13,"bits":[2,2]}""", Codes.Duplicate, "root.VictoryLocations.bits[1]")]
    [InlineData("""{"default":false,"count":16,"bits":[],"$reserved":"80"}""", Codes.ReservedMember, "root.VictoryLocations.$reserved")]
    [InlineData("""{"default":false,"count":13,"bits":[],"$reserved":"01"}""", Codes.ReservedMember, "root.VictoryLocations.$reserved")]
    [InlineData("""{"default":false,"count":13,"bits":"0590"}""", Codes.Type, "root.VictoryLocations.bits")]
    [InlineData("""{"default":false,"count":13,"bits":[],"extra":1}""", Codes.UnknownMember, "root.VictoryLocations.extra")]
    public void AMalformedBitListIsRefused(string json, string code, string path) =>
        Refused("PlayerStructure", "VictoryLocations", json, code, path);

    [Fact]
    public void EnumListsStoreNames()
    {
        byte[] relations = [3, 4, 0, 0, 0, 1, 0, 2, 7];
        JsonValue value = Decode("PlayerDiplomaticRelations", "Relations", relations);
        Assert.Equal("""{"default":"Unknown","items":["War","Peace","Alliance",7]}""", Compact(value));
        Assert.Equal(relations, Encode("PlayerDiplomaticRelations", "Relations", value));
        byte[] spheres = [0, 3, 0, 0, 0, 3, 7, 0, 1];
        JsonValue picks = Decode("Leader", "Spheres", spheres);
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
        byte[] bytes = RunList(-1, flat);
        JsonValue value = Decode("StructureResource", "Defenders", bytes);
        Assert.Equal("""{"groups":[[198,201,199],[203,200,200],[]],"count":19}""", Compact(value));
        Assert.Equal(bytes, Encode("StructureResource", "Defenders", value));
        long[] full = [1, -1, 2, -1, -1, -1, -1, -1, 4, 4, 4, 4, 4, 4, 4, 4];
        JsonValue other = Decode("ExplorationSiteResource", "DefenderCollections", RunList(5, full));
        Assert.Equal("""{"default":5,"groups":[[1,-1,2],[4,4,4,4,4,4,4,4]]}""", Compact(other));
        Assert.Equal(RunList(5, full), Encode("ExplorationSiteResource", "DefenderCollections", other));
        Assert.Equal("""{"groups":[]}""", Compact(Decode("StructureResource", "Defenders", RunList(-1, []))));
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
        byte[] bytes = RunList(0, [0x03000201, 0, 0, unchecked((int)0xFF0001FE)]);
        JsonValue value = Decode("MoveTokenEvent", "Path", bytes);
        Assert.Equal("""{"default":{},"items":[{"X":1,"Y":2,"Tag":3},{},{},{"X":-2,"Y":1,"Tag":-1}]}""", Compact(value));
        Assert.Equal(bytes, Encode("MoveTokenEvent", "Path", value));
        Refused("MoveTokenEvent", "Path", """{"default":{},"items":[{"Z":1}]}""", Codes.UnknownMember, "root.Path.items[0].Z");
    }

    [Fact]
    public void AUnitIndexListIsAListWithADefault()
    {
        byte[] bytes = RunList(-1, [12, -1, -1, 40]);
        JsonValue value = Decode("RaceResource", "UnitIndexes", bytes);
        Assert.Equal("""{"default":-1,"items":[12,-1,-1,40]}""", Compact(value));
        Assert.Equal(bytes, Encode("RaceResource", "UnitIndexes", value));
        Assert.Equal("""{"default":"Unknown","items":["Alliance","Unknown"]}""", Compact(Decode("Player", "LegacyNewRelations", [3, 2, 0, 0, 0, 2, 3, 1])));
    }

    private static SchemaProperty Property(string className, string property) =>
        FormatSchema.Current.Get(className).PropertyByName(property) ?? throw new InvalidOperationException(className + " has no " + property + ".");

    private static JsonValue Decode(string className, string property, byte[] bytes)
    {
        SchemaProperty prop = Property(className, property);
        DecodeContext context = new(bytes, FormatSchema.Current, "game", "world-hsm", "lists.json", FormatLimits.Default, new LayoutCounts());
        LayoutBuilder builder = new();
        ValueSite site = new() { Property = prop, Id = prop.Id ?? 0, Path = DocumentNodePath.Root.Member(property), Owner = new JsonObject(), Builder = builder };
        JsonValue value = prop.Codec.Decode(context, 0, bytes.Length, site);
        Assert.Empty(builder.Parts);
        return value;
    }

    private static byte[] Encode(string className, string property, JsonValue value)
    {
        SchemaProperty prop = Property(className, property);
        EncodeContext context = new(FormatSchema.Current, "game", "world-hsm", "lists.json", FormatLimits.Default, ExportOptions.Default);
        ByteBuffer data = new();
        prop.Codec.Encode(context, value, data, new ValueSite { Property = prop, Id = prop.Id ?? 0, Path = DocumentNodePath.Root.Member(property), Owner = new JsonObject() });
        return data.ToArray();
    }

    private static void Refused(string className, string property, string json, string code, string path)
    {
        ConversionException error = Assert.Throws<ConversionException>(() => Encode(className, property, JsonReader.Parse(json)));
        Assert.Equal(code, error.Diagnostic.Code);
        Assert.Equal(path, error.Diagnostic.Path);
    }

    // The compact writer separates members with a space. None of these values holds text with one.
    private static string Compact(JsonValue value)
    {
        StringBuilder text = new();
        JsonWriter.WriteCompact(text, value);
        return text.ToString().Replace(": ", ":", StringComparison.Ordinal).Replace(", ", ",", StringComparison.Ordinal);
    }

    private static byte[] RunList(int defaultValue, long[] items)
    {
        ByteBuffer data = new();
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
}
