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
/// tactical engine table. <c>HEROES.PFS</c> and <c>ITEMS.PFS</c> are written objects without CRC
/// words, which the system hero and item libraries load, and <c>General.pfs</c> holds more than the
/// root. This test reads none of the three. Each file passes G1 through the converter, and its root passes
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
        byte[] bytes = File.ReadAllBytes(Path.Combine(Checker.GameRoot, relative));
        WrittenObjectProfile profile = new("pfs", ["pfs"], rootClass, engine);
        byte[] body = bytes[4..^4];
        ImportResult imported = GameFiles.Import(body, profile, "pfs", relative);
        Assert.Equal(OwnFormat.CurrentVersion, ((JsonInteger)imported.Document["$version"]!).Value);
        JsonObject container = (JsonObject)imported.Document["$container"]!;
        JsonObject root = (JsonObject)imported.Document["root"]!;
        byte[] exported = profile.Export(context: new EncodeContext(FormatSchema.Current, engine, profile.Name, relative, FormatLimits.Default), container: container, root: root);
        OriginalComparisonResult comparison = GameFiles.CompareG1(body, exported: exported, profile: profile, document: imported.Document, kind: "pfs");
        Assert.NotEqual(OriginalMatchKind.Mismatch, comparison.Kind);
        Prove(root, FormatSchema.Current.Get(rootClass), engine, relative);
    }

    [GameFact]
    public void TheScenarioSetRoundTripsOnBothBackends()
    {
        const string relative = "Release/Release.hss";
        byte[] bytes = File.ReadAllBytes(Path.Combine(Checker.GameRoot, relative));
        FileProfile profile = FileProfile.Find("scenario-set")!;
        ImportResult imported = GameFiles.Import(bytes, profile, "scenario-set", relative);
        ExportResult exported = GameFiles.Export(GameFiles.Read(GameFiles.Write(imported.Document), relative).Document, relative);
        OriginalComparisonResult comparison = GameFiles.CompareG1(bytes, exported: exported.Bytes, profile: profile, document: imported.Document, kind: "scenario-set");
        Assert.Equal(OriginalMatchKind.Masked, comparison.Kind);
        Prove((JsonObject)imported.Document["root"]!, FormatSchema.Current.Get(profile.RootFamily), profile.Engine, relative);
    }

    private static void Prove(JsonObject root, SchemaClass classValue, string engine, string label)
    {
        FormatSchema schema = FormatSchema.Current;
        byte[] image = PilotImage.Encode(root, classValue.Name, engine);
        ObjectNode tree = OriginalBackend.Read(image, classValue, schema, engine);
        byte[] direct = OriginalBackend.Write(tree, classValue, schema, engine);
        JsonObject expected = PilotImage.Decode(direct, classValue.Name, engine);
        Assert.True(Same(PilotImage.Decode(image, classValue.Name, engine), expected), $"{label} direct read");
        Assert.True(direct.AsSpan().SequenceEqual(PilotImage.Encode(expected, classValue.Name, engine)), $"{label} direct canonical");
        Assert.True(Same(expected, DocumentBackend.Write(tree, classValue, schema, engine)), $"{label} direct document");
        Assert.True(direct.AsSpan().SequenceEqual(OriginalBackend.Write(DocumentBackend.Read(expected, classValue, schema, engine), classValue, schema, engine)), $"{label} direct document read");
    }

    private static bool Same(JsonObject left, JsonObject right) => JsonWriter.ToUtf8(left).AsSpan().SequenceEqual(JsonWriter.ToUtf8(right));
}
