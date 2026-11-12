using System.Text;
using Aow.Convert;
using Aow.Formats.Json;
using Aow.Formats.Migrations;
using Aow.Formats.Schema;
using Aow.Rtl;

namespace Aow.Formats.Tests;

/// <summary>
/// Issue 200 folded each hex-sprite engine base into its game class. Own documents name the
/// folded classes, and a document that still names a class by its name from before the fold
/// loads and exports the original bytes.
/// </summary>
public sealed class FoldedClassNameTests : FormatsTest
{
    // The names an own document stored before the fold, and the class each one loads as now.
    private static readonly (string Old, string Current)[] s_storedNames =
    [
        ("Hexagon", "Hex"),
        ("AoWHexagon", "Hex"),
        ("AoWHexagon.Resource", "Hex.Resource"),
        ("BorderHexagon", "BorderHex"),
        ("AoWBorderHexagon", "BorderHex"),
        ("BorderHexagon.Resource", "BorderHex.Resource"),
        ("AoWRoad", "Road"),
        ("AoWMapField", "MapField"),
        ("AoWMapLevel", "MapLevel"),
        ("HSMap", "GameMap"),
        ("HSSet", "GamePack"),
        ("AoWHSSet", "GamePack"),
        ("CustomCombatMap", "CombatMap"),
        ("AoWHSMap", "StrategicMap"),
        ("AoWHSMap.MapCampaignSettings", "StrategicMap.MapCampaignSettings"),
        ("THexagon", "Hex"),
        ("TAoWHexagon", "Hex"),
        ("THSMap", "GameMap"),
        ("TAbstractAoWHSMap", "GameMap"),
        ("TAoWHSMap", "StrategicMap"),
        ("THSSet", "GamePack"),
        ("TAoWHSSet", "GamePack"),
        ("TMapLevel", "MapLevel"),
        ("TAoWMapLevel", "MapLevel"),
        ("TAbstractAoWCombatMap", "CombatMap"),
        ("TAoWCombatMap", "CombatMap"),
    ];

    // The game class names that the shipped files stored in own documents before the fold.
    private static readonly (string Old, string Current)[] s_documentNames =
    [
        ("AoWHSMap", "StrategicMap"),
        ("AoWMapLevel", "MapLevel"),
        ("AoWHexagon", "Hex"),
        ("AoWBorderHexagon", "BorderHex"),
        ("AoWRoad", "Road"),
        ("AoWHexagon.Resource", "Hex.Resource"),
        ("BorderHexagon.Resource", "BorderHex.Resource"),
    ];

    [Fact]
    public void AnOldStoredNameFindsTheFoldedClass()
    {
        var schema = FormatSchema.Current;
        foreach (var (old, current) in s_storedNames)
            Assert.True(schema.Find(old)?.Name == current, $"{old} finds {schema.Find(old)?.Name ?? "nothing"}, not {current}");
        // The first row of a current name gives the Delphi name, and that is the game class's.
        foreach (var (current, delphi) in s_delphiNames)
            Assert.Equal(delphi, NameTable.Rows.First(row => row.Kind == "type" && row.Current == current).Delphi);
    }

    private static readonly (string Current, string Delphi)[] s_delphiNames =
    [
        ("Aow.Game.World.Hexes.Hex", "TAoWHexagon"),
        ("Aow.Game.World.Hexes.BorderHex", "TAoWBorderHexagon"),
        ("Aow.Game.World.Hexes.Road", "TAoWRoad"),
        ("Aow.Game.World.Map.MapField", "TAoWMapField"),
        ("Aow.Game.World.Map.MapLevel", "TAoWMapLevel"),
        ("Aow.Game.World.Map.GameMap", "TAbstractAoWHSMap"),
        ("Aow.Game.World.Map.GamePack", "TAoWHSSet"),
        ("Aow.Game.World.Map.StrategicMap", "TAoWHSMap"),
        ("Aow.Game.Combat.CombatMap", "TAoWCombatMap"),
        ("Aow.Game.Draw.DisplayController", "TDisplayCtrl"),
    ];

    [GameTheory]
    [InlineData("Scenario/Custom")]
    [InlineData("TCMaps")]
    [InlineData("Release")]
    public void AnOwnDocumentNamingTheClassesBeforeTheFoldLoads(string folder)
    {
        string directory = Path.Combine(Checker.GameRoot, folder);
        string pattern = folder == "Release" ? "Release.hss" : "*.hsm";
        // The first file in name order whose document names a folded class.
        foreach (string path in Directory.EnumerateFiles(directory, pattern, new EnumerationOptions { MatchCasing = MatchCasing.CaseInsensitive })
            .Order(StringComparer.Ordinal))
        {
            string relative = Path.GetRelativePath(Checker.GameRoot, path);
            byte[] bytes = File.ReadAllBytes(path);
            var classification = GameFiles.Classify(bytes, Path.GetExtension(path));
            byte[] current = GameFiles.Write(GameFiles.Import(bytes, classification.Profile!, classification.Kind!, relative).Document);
            string text = Encoding.UTF8.GetString(current);
            int renamed = 0;
            foreach (var (old, name) in s_documentNames)
            {
                string stored = "\"$class\": \"" + name + "\"";
                if (!text.Contains(stored, StringComparison.Ordinal)) continue;
                text = text.Replace(stored, "\"$class\": \"" + old + "\"", StringComparison.Ordinal);
                renamed++;
            }
            if (renamed == 0) continue;

            // A document written before the fold covered the old names with its layout
            // fingerprints, so they are taken again over the renamed text. The reader keeps the
            // stored names. The export resolves each one to its class and writes the original
            // image, which needs every old name to find the folded class and its id.
            var document = (JsonObject)JsonReader.Parse(text);
            RecordMigration.RefreshFingerprints(document);
            var read = GameFiles.Read(JsonWriter.ToUtf8(document), relative);
            var exported = GameFiles.Export(read.Document, null, relative);
            Assert.True(classification.Profile!.Comparable(bytes, FormatLimits.Default).AsSpan()
                .SequenceEqual(classification.Profile.Comparable(exported.Bytes, FormatLimits.Default)), relative + " exports its original bytes");
            return;
        }

        Assert.Fail(folder + " holds no file that names a folded class");
    }
}
