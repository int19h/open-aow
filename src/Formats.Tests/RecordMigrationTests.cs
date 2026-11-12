using System.Text;
using Aow.Formats.Json;
using Aow.Formats.Migrations;
using Aow.Formats.Original;
using Aow.Formats.Schema;

namespace Aow.Formats.Tests;

/// <summary>
/// Version 2 hex of a record, a bit array and a raw primitive migrates to the version 3 value.
/// The step reaches a child, a created object, a dynamic item, a map-field row and a nested campaign map.
/// </summary>
public sealed class RecordMigrationTests : FormatsTest
{
    [Fact]
    public void NestedVersion2HexBecomesTheVersion3Value()
    {
        var root = new JsonObject()
            .Add("$class", new JsonString("TAoWCampaign"))
            .Add("Map", Map());
        var read = Read(Envelope("campaign", root));
        Assert.Equal([RecordMigration.Name, ListMigration.Name], read.Migrations);
        Assert.Equal(4, ((JsonInteger)read.Document["$version"]!).Value);
        var map = (JsonObject)((JsonObject)read.Document["root"]!)["Map"]!;
        var campaignRoot = (JsonObject)map["root"]!;
        Assert.Equal(3, ((JsonInteger)campaignRoot["VictoryMode"]!).Value);
        var children = (JsonArray)campaignRoot["Children"]!;
        var child = (JsonObject)children.Items[0];
        Assert.Equal(7, ((JsonInteger)child["BehaviorMode"]!).Value);
        var behavior = (JsonObject)child["Behavior"]!;
        Assert.Equal(["PlayerType", "$reserved", "Flags"], behavior.Members.Select(m => m.Name).ToList());
        Assert.Equal(2, ((JsonInteger)behavior["PlayerType"]!).Value);
        Assert.Equal("010002", ((JsonString)behavior["$reserved"]!).Value);
        Assert.Equal(9, ((JsonInteger)behavior["Flags"]!).Value);
        var made = (JsonObject)((JsonObject)children.Items[1])["Animation"]!;
        Assert.Empty(((JsonObject)made["Dimensions"]!).Members);
    }

    [Fact]
    public void AMigratedLayoutLoadsAndAStaleFingerprintIsRefused()
    {
        var read = Read(Envelope("world-hsm", new JsonObject()
            .Add("$class", new JsonString("TAoWHSMap"))
            .Add("VictoryMode", new JsonString("03"))));
        var again = GameFiles.Read(GameFiles.Write(read.Document), "record-migration.json");
        Assert.Equal(3, ((JsonInteger)((JsonObject)again.Document["root"]!)["VictoryMode"]!).Value);
        var root = (JsonObject)read.Document["root"]!;
        var context = new EncodeContext(FormatSchema.Current, "game", "world-hsm", "record-migration.json", FormatLimits.Default, ExportOptions.Default);
        Assert.NotNull(LayoutReader.Read(context, root, DocumentNodePath.Root));
        var fingerprint = ((JsonObject)root["$layout"]!).Find("fingerprint")!;
        string current = ((JsonString)fingerprint.Value).Value;
        string wrong = current[0] == '0' ? new string('f', 64) : new string('0', 64);
        fingerprint.Value = new JsonString(wrong);
        var exact = new EncodeContext(FormatSchema.Current, "game", "world-hsm", "record-migration.json", FormatLimits.Default, new ExportOptions { ExactLayout = true });
        var compared = Assert.Throws<ConversionException>(() => LayoutReader.Read(exact, root, DocumentNodePath.Root));
        Assert.Equal(Codes.Layout, compared.Diagnostic.Code);
        Assert.Equal("The retained exact layout is stale after an edit. --exact-layout refuses to rebuild it.", compared.Diagnostic.Message);
        fingerprint.Value = new JsonString("stale");
        var malformed = Assert.Throws<ConversionException>(() => LayoutReader.Read(context, root, DocumentNodePath.Root));
        Assert.Equal(Codes.Layout, malformed.Diagnostic.Code);
        Assert.Equal("The $layout fingerprint is not 64 lowercase hexadecimal digits.", malformed.Diagnostic.Message);
    }

    [Fact]
    public void TheVersion2StepDoesNotReadTheCurrentSchema()
    {
        JsonObject[] Documents() =>
        [
            Envelope("hero-list", new JsonObject()
                .Add("$class", new JsonString("TSetupSettings"))
                .Add("PlayMode", new JsonString("03"))),
            Envelope("world-hsm", new JsonObject()
                .Add("$class", new JsonString("TPlayerSetupSettings"))
                .Add("Behavior", new JsonString("0201000209000000"))),
            Envelope("hero-list", new JsonObject()
                .Add("$class", new JsonString("TCustomAbilityList"))
                .Add("AbCount", new JsonInteger(1))
                .Add("AbilityCount", new JsonString("81"))),
            Envelope("hero-list", new JsonObject()
                .Add("$class", new JsonString("TUnitResource"))
                .Add("Champion", new JsonString("02"))),
            Envelope("hero-list", new JsonObject()
                .Add("$class", new JsonString("TUnitResource"))
                .Add("Champion", new JsonString("00"))),
        ];
        var applied = Documents().Select(RecordMigration.Apply).ToArray();
        Assert.Equal(3, ((JsonInteger)((JsonObject)applied[0]["root"]!)["PlayMode"]!).Value);
        var behavior = (JsonObject)((JsonObject)applied[1]["root"]!)["Behavior"]!;
        Assert.Equal(2, ((JsonInteger)behavior["PlayerType"]!).Value);
        Assert.Equal("010002", ((JsonString)behavior["$reserved"]!).Value);
        Assert.Equal(9, ((JsonInteger)behavior["Flags"]!).Value);
        var ability = (JsonObject)((JsonObject)applied[2]["root"]!)["AbilityCount"]!;
        Assert.Equal([0L], ((JsonArray)ability["values"]!).Items.Select(item => ((JsonInteger)item).Value).ToList());
        Assert.Equal("80", ((JsonString)ability["$reserved"]!).Value);
        Assert.Equal(2, ((JsonInteger)((JsonObject)applied[3]["root"]!)["Champion"]!).Value);
        Assert.False(((JsonBoolean)((JsonObject)applied[4]["root"]!)["Champion"]!).Value);
        var first = applied.Select(JsonWriter.ToUtf8).ToArray();
        FormatSchema.Substituted = FormatSchema.Load(Encoding.UTF8.GetBytes("""{"version":3,"classes":[]}"""), "empty.json");
        try
        {
            var second = Documents().Select(doc => JsonWriter.ToUtf8(RecordMigration.Apply(doc))).ToArray();
            Assert.Equal(first.Length, second.Length);
            for (int i = 0; i < first.Length; i++) Assert.Equal(first[i], second[i]);
        }
        finally
        {
            FormatSchema.Substituted = null;
        }
    }

    [Fact]
    public void AMapFieldSpriteAndABitArrayMigrate()
    {
        var sprites = new JsonArray().Add(new JsonObject()
            .Add("$class", new JsonString("TAnimation"))
            .Add("Dimensions", new JsonString("01000000000000000000000000000000")));
        var cell = new JsonObject().Add("Sprites", sprites);
        var root = new JsonObject()
            .Add("$class", new JsonString("TMapLevel"))
            .Add("Fields", new JsonArray().Add(new JsonArray().Add(cell)));
        var read = Read(Envelope("world-hsm", root));
        var row = (JsonArray)((JsonArray)((JsonObject)read.Document["root"]!)["Fields"]!).Items[0];
        var dimensions = (JsonObject)((JsonObject)((JsonArray)((JsonObject)row.Items[0])["Sprites"]!).Items[0])["Dimensions"]!;
        Assert.Equal(1, ((JsonInteger)dimensions["Left"]!).Value);
        Assert.Single(dimensions.Members);

        var bits = Read(Envelope("hero-list", new JsonObject()
            .Add("$class", new JsonString("TCustomAbilityList"))
            .Add("AbCount", new JsonInteger(1))
            .Add("AbilityCount", new JsonString("81"))));
        var ability = (JsonObject)((JsonObject)bits.Document["root"]!)["AbilityCount"]!;
        Assert.Equal([0L], ((JsonArray)ability["values"]!).Items.Select(item => ((JsonInteger)item).Value).ToList());
        Assert.Equal("80", ((JsonString)ability["$reserved"]!).Value);

        var champion = Read(Envelope("hero-list", new JsonObject()
            .Add("$class", new JsonString("TUnitResource"))
            .Add("Champion", new JsonString("02"))));
        Assert.Equal(2, ((JsonInteger)((JsonObject)champion.Document["root"]!)["Champion"]!).Value);
        var absent = Read(Envelope("hero-list", new JsonObject()
            .Add("$class", new JsonString("TUnitResource"))
            .Add("Champion", new JsonString("00"))));
        Assert.False(((JsonBoolean)((JsonObject)absent.Document["root"]!)["Champion"]!).Value);
    }

    [Fact]
    public void Version2HexThatIsNotHexIsRefused()
    {
        var integer = Assert.Throws<ConversionException>(() => Read(Envelope("hero-list", new JsonObject()
            .Add("$class", new JsonString("TSetupSettings"))
            .Add("PlayMode", new JsonInteger(2)))));
        Assert.Equal(Codes.Type, integer.Diagnostic.Code);
        Assert.Equal("root.PlayMode", integer.Diagnostic.Path);
        var broken = Assert.Throws<ConversionException>(() => Read(Envelope("hero-list", new JsonObject()
            .Add("$class", new JsonString("TSetupSettings"))
            .Add("PlayMode", new JsonString("zz")))));
        Assert.Equal(Codes.Type, broken.Diagnostic.Code);
        Assert.Equal("root.PlayMode", broken.Diagnostic.Path);
    }

    private static JsonObject Map()
    {
        var child = new JsonObject()
            .Add("$class", new JsonString("TPlayerSetupSettings"))
            .Add("$slot", new JsonInteger(0))
            .Add("BehaviorMode", new JsonString("07"))
            .Add("Behavior", new JsonString("0201000209000000"));
        var made = new JsonObject()
            .Add("$class", new JsonString("TAnimationTE"))
            .Add("$slot", new JsonInteger(1))
            .Add("Animation", new JsonObject()
                .Add("$class", new JsonString("TAnimation"))
                .Add("Dimensions", new JsonString("00000000000000000000000000000000")));
        var campaignRoot = new JsonObject()
            .Add("$class", new JsonString("TAoWHSMap"))
            .Add("VictoryMode", new JsonString("03"))
            .Add("Children", new JsonArray().Add(child).Add(made));
        var map = new JsonObject()
            .Add("$container", new JsonObject()
                .Add("profile", new JsonString("world-hsm"))
                .Add("compression", new JsonString("cfs"))
                .Add("level", new JsonInteger(1)))
            .Add("root", campaignRoot);
        AttachLayout(map, "world-hsm");
        return map;
    }

    private static ReadResult Read(JsonObject document) =>
        GameFiles.Read(JsonWriter.ToUtf8(document), "record-migration.json");

    private static JsonObject Envelope(string profile, JsonObject root)
    {
        AttachLayout(root, profile);
        return new JsonObject()
            .Add("$format", new JsonString(OwnFormat.FormatName))
            .Add("$version", new JsonInteger(2))
            .Add("$kind", new JsonString(profile))
            .Add("$container", new JsonObject()
                .Add("profile", new JsonString(profile))
                .Add("compression", new JsonString("none")))
            .Add("root", root);
    }

    private static void AttachLayout(JsonObject target, string profile)
    {
        var layout = new JsonObject()
            .Add("codec", new JsonString(LayoutResidue.Codec))
            .Add("fingerprint", new JsonString("stale"))
            .Add("parts", new JsonArray().Add(new JsonObject()
                .Add("category", new JsonString(LayoutResidue.TableFormOrder))
                .Add("entries", new JsonArray())));
        int at = 0;
        while (at < target.Members.Count && target.Members[at].Name.StartsWith('$')) at++;
        target.Members.Insert(at, new JsonMember("$layout", layout));
    }
}
