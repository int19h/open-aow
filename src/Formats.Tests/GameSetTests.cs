using Aow.Convert;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Tests;

/// <summary>
/// The scenario set and the game set files hold every resource payload of the current version. A
/// game set file (<c>Release\*.pfs</c>) stays outside the own formats (own-formats.md 1.1). It is a written
/// object between two CRC words (TEngine.WriteToFileCRC), read here through a written-object
/// profile of its root class. <c>comb_res.pfs</c> holds tactical classes and reads through the
/// tactical engine table. <c>HEROES.PFS</c>, <c>ITEMS.PFS</c> and <c>General.pfs</c> hold more
/// than the root and are not read. Each file passes G1 through the converter, and its root passes
/// both backends: the image read and the canonical write, the document write equal to the
/// converter's document without its layout, and the document read.
/// </summary>
public sealed class GameSetTests : FormatsTest
{
    public static TheoryData<string, string, string> Files() => new()
    {
        { "Release/Ability.pfs", "AbilityControl", "game" },
        { "Release/Spells.pfs", "SpellControl", "game" },
        { "Release/Unitgfx.pfs", "UnitGFXResourceList", "game" },
        { "Release/Unitres.pfs", "UnitResourceList", "game" },
        { "Release/HERORES.PFS", "HeroResourceList", "game" },
        { "Release/ITEMGFX.PFS", "ItemGFXList", "game" },
        { "Release/FACERES.PFS", "FaceResourceList", "game" },
        { "Release/TEXT.pfs", "TextControl", "game" },
        { "Release/comb_res.pfs", "MapObjectResourceList", "tactical" },
    };

    [GameTheory]
    [MemberData(nameof(Files))]
    public void AGameSetFileRoundTripsOnBothBackends(string relative, string rootClass, string engine)
    {
        var bytes = File.ReadAllBytes(Path.Combine(Checker.GameRoot, relative));
        var profile = new WrittenObjectProfile("pfs", ["pfs"], rootClass, engine);
        var body = bytes[4..^4];
        var imported = GameFiles.Import(body, profile, "pfs", relative);
        Assert.Equal(OwnFormat.CurrentVersion, ((JsonInteger)imported.Document["$version"]!).Value);
        var container = (JsonObject)imported.Document["$container"]!;
        var root = (JsonObject)imported.Document["root"]!;
        var context = new EncodeContext(FormatSchema.Current, engine, profile.Name, relative, FormatLimits.Default, ExportOptions.Default);
        Assert.Equal(body, profile.Export(container, root, context));
        Prove(root, FormatSchema.Current.Get(rootClass), engine, relative);
    }

    [GameFact]
    public void TheScenarioSetRoundTripsOnBothBackends()
    {
        const string relative = "Release/Release.hss";
        var bytes = File.ReadAllBytes(Path.Combine(Checker.GameRoot, relative));
        var profile = FileProfile.Find("scenario-set")!;
        var imported = GameFiles.Import(bytes, profile, "scenario-set", relative);
        var exported = GameFiles.Export(GameFiles.Read(GameFiles.Write(imported.Document), relative).Document, null, relative);
        Assert.Equal(bytes, exported.Bytes);
        Prove((JsonObject)imported.Document["root"]!, FormatSchema.Current.Get(profile.RootFamily), profile.Engine, relative);
    }

    private static void Prove(JsonObject root, SchemaClass classValue, string engine, string label)
    {
        var schema = FormatSchema.Current;
        var image = PilotImage.Encode(root, classValue.Name, engine);
        var tree = OriginalBackend.Read(image, classValue, schema, engine);
        var direct = OriginalBackend.Write(tree, classValue, schema, engine);
        var expected = PilotImage.Decode(direct, classValue.Name, engine);
        Assert.True(Same(WithoutLayout(PilotImage.Decode(image, classValue.Name, engine)), expected), label + " direct read");
        Assert.True(direct.AsSpan().SequenceEqual(PilotImage.Encode(expected, classValue.Name, engine)), label + " direct canonical");
        Assert.True(Same(expected, DocumentBackend.Write(tree, classValue, schema, engine)), label + " direct document");
        Assert.True(direct.AsSpan().SequenceEqual(OriginalBackend.Write(DocumentBackend.Read(expected, classValue, schema, engine), classValue, schema, engine)), label + " direct document read");
    }

    private static bool Same(JsonObject left, JsonObject right) => JsonWriter.ToUtf8(left).AsSpan().SequenceEqual(JsonWriter.ToUtf8(right));

    private static JsonObject WithoutLayout(JsonObject objectValue)
    {
        objectValue.Members.RemoveAll(member => member.Name == "$layout");
        foreach (var member in objectValue.Members) WithoutLayout(member.Value);
        return objectValue;
    }

    private static void WithoutLayout(JsonValue value)
    {
        if (value is JsonObject nested) WithoutLayout(nested);
        else if (value is JsonArray array)
            foreach (var item in array.Items) WithoutLayout(item);
    }
}
