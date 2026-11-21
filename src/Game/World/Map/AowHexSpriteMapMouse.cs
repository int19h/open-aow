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
        CombatMap combatMap = StrategicMap.Current.CombatMap;
        return combatMap is not null ? combatMap.Mouse.GetCursor() : base.GetCursor();
    }

    // AoWHSMap.PAS:1157
    public override void Down(MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (StrategicMap.Current.CombatMap is not null)
        {
            StrategicMap.Current.CombatMap.Mouse.Down(button, shift, X, Y);
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
        if (StrategicMap.Current.CombatMap is not null)
        {
            StrategicMap.Current.CombatMap.Mouse.Up(button, shift, X, Y);
        }
        else
        {
            base.Up(button, shift, X, Y);
        }
    }

    // AoWHSMap.PAS:1175
    public override void Move(ShiftStates shift, int X, int Y)
    {
        CombatMap combatMap = StrategicMap.Current.CombatMap;
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
        CombatMap combatMap = StrategicMap.Current.CombatMap;
        if (combatMap is not null)
        {
            combatMap.Mouse.DoubleClick();
            return;
        }

        bool selectStructure = Capture is null;
        if (Capture == StrategicMap.Current.SelectedArmy)
        {
            selectStructure = StrategicMap.Current.SelectedArmy.ArmyHexSprite is not null && StrategicMap.Current.SelectedArmy.MoveSettingsList.Count == 1 && StrategicMap.Current.SelectedArmy.ArmyHexSprite.HexX == HexX && StrategicMap.Current.SelectedArmy.ArmyHexSprite.HexY == HexY && StrategicMap.Current.SelectedArmy.ArmyHexSprite.HexLevel == StrategicMap.Current.ViewMapLevel;
        }

        if (selectStructure && HexX != -1)
        {
            int hexY = HexY;
            int hexX = HexX;
            Structure structure = StrategicMap.Current.MapContainer.GetField(hexX, hexY, StrategicMap.Current.ViewMapLevel).FindNoneTransparentHexSprite<Structure>();
            if (structure is not null && structure.CanDoubleClick())
            {
                StrategicMap.Current.Selection.UnselectLocation();
                StrategicMap.Current.Selection.SelectLocationHexSprite(structure);
            }
        }

        base.DoubleClick();
    }
}
