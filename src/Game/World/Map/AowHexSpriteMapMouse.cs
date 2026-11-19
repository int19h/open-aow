// Translated from AoWE.pas.
using Aow.Game.Combat;

using Aow.Game.World.Structures;

using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.World.Map;

public partial class AowHexSpriteMapMouse : GameMapMouse
{

    public AowHexSpriteMapMouse() : base()
    { }

    // AoWHSMap.PAS:1148
    public override int GetCursor()
    {
        int result;
        CombatMap combatMap;
        combatMap = StrategicMap.Current.CombatMap;
        result = combatMap != null ? combatMap.Mouse.GetCursor() : base.GetCursor();

        return result;
    }

    // AoWHSMap.PAS:1157
    public override void Down(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (StrategicMap.Current.CombatMap != null)
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
            HexSpriteMap.GetEngine().OutputDebugString($"Location ({IntToStr(hexX)},{IntToStr(hexY)}) selected");
        }
    }

    // AoWHSMap.PAS:1167
    public override void Up(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (StrategicMap.Current.CombatMap != null)
        {
            StrategicMap.Current.CombatMap.Mouse.Up(button, shift, X, Y);
        }
        else
        {
            base.Up(button, shift, X, Y);
        }
    }

    // AoWHSMap.PAS:1175
    public override void Move(Set<ShiftState> shift, int X, int Y)
    {
        CombatMap combatMap;
        combatMap = StrategicMap.Current.CombatMap;
        if (combatMap != null)
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
        CombatMap combatMap;
        bool selectStructure;
        Structure structure;
        combatMap = StrategicMap.Current.CombatMap;
        if (combatMap != null)
        {
            combatMap.Mouse.DoubleClick();
            return;
        }

        selectStructure = Capture == null;
        if (Capture == StrategicMap.Current.SelectedArmy)
        {
            selectStructure = StrategicMap.Current.SelectedArmy.GetArmyHexSprite() != null && StrategicMap.Current.SelectedArmy.MoveSettingsList.Count == 1 && StrategicMap.Current.SelectedArmy.GetArmyHexSprite().HexX == HexX && StrategicMap.Current.SelectedArmy.GetArmyHexSprite().HexY == HexY && StrategicMap.Current.SelectedArmy.GetArmyHexSprite().HexLevel == StrategicMap.Current.ViewMapLevel;
        }

        if (selectStructure && HexX != -1)
        {
            int hexY = HexY;
            int hexX = HexX;
            structure = StrategicMap.Current.MapContainer.GetField(hexX, hexY, StrategicMap.Current.ViewMapLevel).FindNoneTransparentHexSprite<Structure>();
            if (structure != null && structure.CanDoubleClick())
            {
                StrategicMap.Current.Selection.UnselectLocation();
                StrategicMap.Current.Selection.SelectLocationHexSprite(structure);
            }
        }

        base.DoubleClick();
    }

    // intf_P1_05.inc:68

}
