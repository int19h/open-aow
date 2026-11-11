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
        int Result;
        CustomCombatMap CombatMap;
        CombatMap = AoWHSMap.Current.FCombatMap;
        if (CombatMap != null)
        {
            Result = CombatMap.Mouse.GetCursor();
        }
        else
        {
            Result = base.GetCursor();
        }

        return Result;
    }

    // AoWHSMap.PAS:1157
    public override void Down(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (AoWHSMap.Current.FCombatMap != null)
        {
            AoWHSMap.Current.FCombatMap.Mouse.Down(Button, Shift, X, Y);
        }
        else
        {
            base.Down(Button, Shift, X, Y);
        }

        if (GameTables.AoWEngine.FRegistry.GetDebugMode())
        {
            var xhx = Xhx;
            var yhx = Yhx;
            GameTables.AoWEngine.OutputDebugStr("Location (" + IntToStr(xhx) + "," + IntToStr(yhx) + ") selected");
        }
    }

    // AoWHSMap.PAS:1167
    public override void Up(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (AoWHSMap.Current.FCombatMap != null)
        {
            AoWHSMap.Current.FCombatMap.Mouse.Up(Button, Shift, X, Y);
        }
        else
        {
            base.Up(Button, Shift, X, Y);
        }
    }

    // AoWHSMap.PAS:1175
    public override void Move(Set<ShiftState> Shift, int X, int Y)
    {
        CustomCombatMap CombatMap;
        CombatMap = AoWHSMap.Current.FCombatMap;
        if (CombatMap != null)
        {
            CombatMap.Mouse.Move(Shift, X, Y);
        }
        else
        {
            base.Move(Shift, X, Y);
        }
    }

    // AoWHSMap.PAS:1184
    public override void DblClick()
    {
        CustomCombatMap CombatMap;
        bool SelectStructure;
        Structure Structure;
        CombatMap = AoWHSMap.Current.FCombatMap;
        if (CombatMap != null)
        {
            CombatMap.Mouse.DblClick();
            return;
        }

        SelectStructure = Capture == null;
        if (Capture == AoWHSMap.Current.FSelectedArmy)
        {
            SelectStructure = AoWHSMap.Current.FSelectedArmy.GetArmyHS() != null && AoWHSMap.Current.FSelectedArmy.FMoveSettingsList.Count == 1 && AoWHSMap.Current.FSelectedArmy.GetArmyHS().Xhx == Xhx && AoWHSMap.Current.FSelectedArmy.GetArmyHS().Yhx == Yhx && AoWHSMap.Current.FSelectedArmy.GetArmyHS().Lhx == AoWHSMap.Current.ViewL;
        }

        if (SelectStructure && Xhx != -1)
        {
            var yhx = Yhx;
            var xhx = Xhx;
            Structure = (Structure)AoWHSMap.Current.MapContainer.GetField(xhx, yhx, AoWHSMap.Current.ViewL).FindNoneTransparentHS(typeof(Structure));
            if (Structure != null && Structure.CanDblClick())
            {
                AoWHSMap.Current.Selection.UnselectLocation();
                AoWHSMap.Current.Selection.SelectLocationHS(Structure);
            }
        }

        base.DblClick();
    }

    // intf_P1_05.inc:68

}
