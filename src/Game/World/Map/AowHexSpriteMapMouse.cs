// Translated from AoWE.pas.
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Structures;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

public partial class AowHexSpriteMapMouse : GameMapMouse
{

    public AowHexSpriteMapMouse() : base()
    { }

    // AoWHSMap.PAS:1148
    public override int GetCursor()
    {
        int result;
        CustomCombatMap combatMap;
        combatMap = AoWHSMap.Current.CombatMap;
        if (combatMap != null)
        {
            result = combatMap.Mouse.GetCursor();
        }
        else
        {
            result = base.GetCursor();
        }

        return result;
    }

    // AoWHSMap.PAS:1157
    public override void Down(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (AoWHSMap.Current.CombatMap != null)
        {
            AoWHSMap.Current.CombatMap.Mouse.Down(button, shift, X, Y);
        }
        else
        {
            base.Down(button, shift, X, Y);
        }

        if (GameTables.GameEngine.Registry.GetDebugMode())
        {
            var hexX = HexX;
            var hexY = HexY;
            GameTables.GameEngine.OutputDebugString("Location (" + IntToStr(hexX) + "," + IntToStr(hexY) + ") selected");
        }
    }

    // AoWHSMap.PAS:1167
    public override void Up(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (AoWHSMap.Current.CombatMap != null)
        {
            AoWHSMap.Current.CombatMap.Mouse.Up(button, shift, X, Y);
        }
        else
        {
            base.Up(button, shift, X, Y);
        }
    }

    // AoWHSMap.PAS:1175
    public override void Move(Set<ShiftState> shift, int X, int Y)
    {
        CustomCombatMap combatMap;
        combatMap = AoWHSMap.Current.CombatMap;
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
        CustomCombatMap combatMap;
        bool selectStructure;
        Structure structure;
        combatMap = AoWHSMap.Current.CombatMap;
        if (combatMap != null)
        {
            combatMap.Mouse.DoubleClick();
            return;
        }

        selectStructure = Capture == null;
        if (Capture == AoWHSMap.Current.SelectedArmy)
        {
            selectStructure = AoWHSMap.Current.SelectedArmy.GetArmyHexSprite() != null && AoWHSMap.Current.SelectedArmy.MoveSettingsList.Count == 1 && AoWHSMap.Current.SelectedArmy.GetArmyHexSprite().HexX == HexX && AoWHSMap.Current.SelectedArmy.GetArmyHexSprite().HexY == HexY && AoWHSMap.Current.SelectedArmy.GetArmyHexSprite().HexLevel == AoWHSMap.Current.ViewL;
        }

        if (selectStructure && HexX != -1)
        {
            var hexY = HexY;
            var hexX = HexX;
            structure = (Structure)AoWHSMap.Current.MapContainer.GetField(hexX, hexY, AoWHSMap.Current.ViewL).FindNoneTransparentHexSprite(typeof(Structure));
            if (structure != null && structure.CanDoubleClick())
            {
                AoWHSMap.Current.Selection.UnselectLocation();
                AoWHSMap.Current.Selection.SelectLocationHexSprite(structure);
            }
        }

        base.DoubleClick();
    }

    // intf_P1_05.inc:68

}
