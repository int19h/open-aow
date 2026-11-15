#nullable disable
// Translated from AoWTCUnit.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// components
public partial class CombatUnitInfo : UIModule
{
    public CombatUnitInfo(UIContext ui) : base(ui)
    { }

    public Window MainWindow { get; private set; }
    public Image UnitMovesIcon { get; private set; }
    public Label UnitAttackLabel { get; private set; }
    public Label UnitDefenseLabel { get; private set; }
    public Label UnitMovesLabel { get; private set; }
    public Label UnitDamageLabel { get; private set; }
    public Label UnitResistanceLabel { get; private set; }
    public Label UnitHitsLabel { get; private set; }
    public Memo UnitNameMemo { get; private set; }
    public ListBox UnitAbilityList { get; private set; }
    public Label AbilityNameLabel { get; private set; }
    public Panel CombatUnitPanel { get; private set; }
    public ListBox TargetAbilityList { get; private set; }
    public Panel CombatAbilityPanel { get; private set; }
    public Label AbilityAttackLabel { get; private set; }
    public ListBox AbilityStatisticsList { get; private set; }
    public Label AbilityDamageLabel { get; private set; }
    public Label TargetUnitLabel { get; private set; }
    public Panel TargetSelectPanel { get; private set; }
    public Panel NoTargetPanel { get; private set; }
    public Gauge UnitHitsGauge { get; private set; }
    public Gauge UnitManaGauge { get; private set; }
    public Image TargetMovesIcon { get; private set; }
    public Label TargetAttackLabel { get; private set; }
    public Label TargetDefenseLabel { get; private set; }
    public Label TargetMovesLabel { get; private set; }
    public Label TargetDamageLabel { get; private set; }
    public Label TargetResistanceLabel { get; private set; }
    public Label TargetHitsLabel { get; private set; }
    public Panel UnitFramePanel { get; private set; }

    // AoWTCUnit.pas:106
    public void TCUnitWinResize()
    {
        MainWindow.Alignment.RightOffset = MainWindow.WindowManager.WinWidth - MainWindow.WinLeft - MainWindow.WinWidth;
        MainWindow.Alignment.BottomOffset = MainWindow.WindowManager.WinHeight - MainWindow.WinTop - MainWindow.WinHeight;
    }

    // AoWTCUnit.pas:116
    public void MinClick()
    {
        UI.General.PlayButtonSound(0);
        MainWindow.Visible = false;
    }

    // AoWTCUnit.pas:123
    public void UAbilLBPreDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        if (UI.General.CombatUnit != null)
        {
            bool condition = UI.General.CombatUnit.Unit.GetPlayer() == CombatMap.Current.SeatedPlayer && UI.General.CombatUnit.Unit.MovePoints > 0 && drawItem < UI.General.CombatUnit.CombatAbilityCount;
            if (condition)
            {
                var ability = UI.General.CombatUnit.Abilities[drawItem];
                var combatMode = GamePack.Current.AbilityControl.GetAbility(ability.Ability).GetCombatMode(UI.General.CombatUnit.Unit.Troop);
                bool test = !Set<CombatMode>.Of().WithRange(CombatMode.ShortRange, CombatMode.LongRange).Contains(combatMode) && UI.General.CombatUnit.Abilities[drawItem].Ability != 52;
                if (!test)
                {
                    var moves = UI.General.CombatUnit.Unit.Troop.GetMoves();
                    test = UI.General.CombatUnit.Unit.MovePoints == moves;
                }

                condition = test;
            }

            if (condition)
            {
                UnitAbilityList.TextColor = 16777215;
            }
            else
            {
                UnitAbilityList.TextColor = 12632256;
            }
        }
    }

    // AoWTCUnit.pas:138
    public void TCUnitPnlDrawBack(LockedSurface lockedSurface)
    {
        if (UI.General.CombatUnit != null)
        {
            UI.General.CombatUnit.Unit.Troop.Show(CombatUnitPanel.WinLeft - 10, CombatUnitPanel.WinTop - 2, lockedSurface, UI.General.UnitShowInfo);
        }
    }

    // AoWTCUnit.pas:146
    public void UAbilLBUpdate()
    {
        if (UnitAbilityList.HoverItem != UnitAbilityList.Selected)
        {
            UnitAbilityList.Selected = UnitAbilityList.HoverItem;
        }
    }

    // AoWTCUnit.pas:152
    public void UAbilLBMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Ability ability;
        if (UnitAbilityList.Selected > -1)
        {
            if (button == MouseButton.Left)
            {
                var objectValue = UnitAbilityList.Items.Objects[UnitAbilityList.Selected];
                var selectAbility = UI.General.CombatUnit.SelectAbility(ObjectToInt(objectValue));
                UnitAbilityList.Selected = selectAbility;
            }
            else
            {
                if (button == MouseButton.Right && UI.General.CombatUnit != null)
                {
                    var objectValue = UnitAbilityList.Items.Objects[UnitAbilityList.Selected];
                    ability = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
                    if (ability != null)
                    {
                        InfoBanner.SetInfoBanner(caption: ability.GetName(UI.General.CombatUnit.Unit.Troop), banner: UI.InfoBanner, description: ability.Description);
                    }
                }
            }
        }
    }

    // AoWTCUnit.pas:168
    public void UAbilLBMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        if (UI.General.CombatUnit != null)
        {
            if (UI.General.CombatUnit.Unit.GetPlayer() == CombatMap.Current.SeatedPlayer && UI.General.CombatUnit.Unit.MovePoints != 0 && UnitAbilityList.HoverItem < UI.General.CombatUnit.CombatAbilityCount)
            {
                var ability = UI.General.CombatUnit.Abilities[UnitAbilityList.HoverItem];
                var combatMode = GamePack.Current.AbilityControl.GetAbility(ability.Ability).GetCombatMode(UI.General.CombatUnit.Unit.Troop);
                if (!Set<CombatMode>.Of().WithRange(CombatMode.ShortRange, CombatMode.LongRange).Contains(combatMode) && UI.General.CombatUnit.Abilities[UnitAbilityList.HoverItem].Ability != 52)
                {
                    return;
                }

                var tacticalBattleUnit = UI.General.CombatUnit.Unit;
                if (tacticalBattleUnit.Troop.GetMoves() <= tacticalBattleUnit.MovePoints)
                {
                    return;
                }
            }

            UnitAbilityList.HoverItem = -1;
        }
    }

    // AoWTCUnit.pas:187
    public void HeroSMapMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        UI.General.SelectedUnit = UI.General.CombatUnit.Unit.Troop;
        UI.UnitWindow.ShowUnit();
    }

    // AoWTCUnit.pas:194
    public void CenterClick()
    {
        UI.General.PlayButtonSound(0);
        var hexLevel = UI.General.CombatUnit.GetHexLevel();
        var hexY = UI.General.CombatUnit.GetHexY();
        var hexX = UI.General.CombatUnit.GetHexX();
        CombatMap.Current.CenterViewTo(hexX, hexY, hexLevel);
        var fCombatUnitHexLevel = UI.General.CombatUnit.GetHexLevel();
        var fCombatUnitHexY = UI.General.CombatUnit.GetHexY();
        var fCombatUnitHexX = UI.General.CombatUnit.GetHexX();
        CombatMap.Current.PlayCenterAnimation(fCombatUnitHexX, fCombatUnitHexY, fCombatUnitHexLevel);
    }

    // AoWTCUnit.pas:195
    public void CloseClick()
    {
        UI.General.PlayButtonSound(0);
        MainWindow.Visible = false;
    }
}
