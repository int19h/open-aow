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
        Assert.Equal([RecordMigration.Name], read.Migrations);
        Assert.Equal(3, ((JsonInteger)read.Document["$version"]!).Value);
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
        var ctx = new EncodeContext(FormatSchema.Current, "game", "world-hsm", "record-migration.json", FormatLimits.Default, ExportOptions.Default);
        Assert.NotNull(LayoutReader.Read(ctx, root, DocPath.Root));
        var fingerprint = ((JsonObject)root["$layout"]!).Find("fingerprint")!;
        fingerprint.Value = new JsonString("stale");
        var error = Assert.Throws<ConversionException>(() => LayoutReader.Read(ctx, root, DocPath.Root));
        Assert.Equal(Codes.Layout, error.Diagnostic.Code);
    }

    [Fact]
    public void TheVersion2StepDoesNotReadTheCurrentSchema()
    {
        JsonObject Document() => Envelope("hero-list", new JsonObject()
            .Add("$class", new JsonString("TSetupSettings"))
            .Add("PlayMode", new JsonString("03")));
        var first = JsonWriter.ToUtf8(RecordMigration.Apply(Document()));
        FormatSchema.Substituted = FormatSchema.Load(Encoding.UTF8.GetBytes("""{"version":3,"classes":[]}"""), "empty.json");
        try
        {
            var second = JsonWriter.ToUtf8(RecordMigration.Apply(Document()));
            Assert.Equal(first, second);
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
        Assert.False(((JsonBool)((JsonObject)absent.Document["root"]!)["Champion"]!).Value);
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

    private static string Fingerprint(JsonObject obj) =>
        ((JsonString)((JsonObject)obj["$layout"]!)["fingerprint"]!).Value;
}
