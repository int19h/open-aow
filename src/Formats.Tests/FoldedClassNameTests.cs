using Aow.Formats.Schema;
using Aow.Rtl;

namespace Aow.Formats.Tests;

/// <summary>
/// Issue 200 folded each hex-sprite engine base into its game class. Own documents name the
/// folded classes, and the schema still finds each class by its Delphi name and its name from
/// before the fold.
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
}
