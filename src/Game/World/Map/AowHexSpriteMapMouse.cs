#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Game.Combat;

using Aow.Game.World.Structures;

using Aow.Rtl;

namespace Aow.Game.World.Map;

public sealed class AowHexSpriteMapMouse : GameMapMouse
{

    public AowHexSpriteMapMouse() : base()
    { }

    // AoWHSMap.PAS:1148
    public override int GetCursor()
    {
        CombatMap combatMap = HexSpriteMap.GetEngine().StrategicMap.CombatMap;
        return combatMap is not null ? combatMap.Mouse.GetCursor() : base.GetCursor();
    }

    // AoWHSMap.PAS:1157
    public override void Down(MouseButton button, ShiftStates shift, int X, int Y)
    {
        StrategicMap strategicMap = HexSpriteMap.GetEngine().StrategicMap;
        if (strategicMap.CombatMap is not null)
        {
            strategicMap.CombatMap.Mouse.Down(button, shift, X, Y);
        }
        else
        {
            base.Down(button, shift, X, Y);
        }

        if (HexSpriteMap.GetEngine().Registry.GetDebugMode())
        {
            int hexX = HexX;
            int hexY = HexY;
            HexSpriteMap.GetEngine().OutputDebugString($"Location ({hexX.ToString(CultureInfo.InvariantCulture)},{hexY.ToString(CultureInfo.InvariantCulture)}) selected");
        }
    }

    // AoWHSMap.PAS:1167
    public override void Up(MouseButton button, ShiftStates shift, int X, int Y)
    {
        StrategicMap strategicMap = HexSpriteMap.GetEngine().StrategicMap;
        if (strategicMap.CombatMap is not null)
        {
            strategicMap.CombatMap.Mouse.Up(button, shift, X, Y);
        }
        else
        {
            base.Up(button, shift, X, Y);
        }
    }

    // AoWHSMap.PAS:1175
    public override void Move(ShiftStates shift, int X, int Y)
    {
        CombatMap combatMap = HexSpriteMap.GetEngine().StrategicMap.CombatMap;
        if (combatMap is not null)
        {
            combatMap.Mouse.Move(shift, X, Y);
        }
        else
        {
            base.Move(shift, X, Y);
        }
    }

    // AoWHSMap.PAS:1184
    public override void DoubleClick()
    {
        CombatMap combatMap = HexSpriteMap.GetEngine().StrategicMap.CombatMap;
        if (combatMap is not null)
        {
            combatMap.Mouse.DoubleClick();
            return;
        }

        bool selectStructure = Capture is null;
        StrategicMap strategicMap = HexSpriteMap.GetEngine().StrategicMap;
        if (Capture == strategicMap.SelectedArmy)
        {
            selectStructure = strategicMap.SelectedArmy.ArmyHexSprite is not null && strategicMap.SelectedArmy.MoveSettingsList.Count == 1 && strategicMap.SelectedArmy.ArmyHexSprite.HexX == HexX && strategicMap.SelectedArmy.ArmyHexSprite.HexY == HexY && strategicMap.SelectedArmy.ArmyHexSprite.HexLevel == strategicMap.ViewMapLevel;
        }

        if (selectStructure && HexX != -1)
        {
            int hexY = HexY;
            int hexX = HexX;
            Structure structure = strategicMap.MapContainer.GetField(hexX, hexY, strategicMap.ViewMapLevel).FindNoneTransparentHexSprite<Structure>();
            if (structure is not null && structure.CanDoubleClick())
            {
                strategicMap.Selection.UnselectLocation();
                HexSpriteMap.GetEngine().StrategicMap.Selection.SelectLocationHexSprite(structure);
            }
        }

        base.DoubleClick();
    }
}
