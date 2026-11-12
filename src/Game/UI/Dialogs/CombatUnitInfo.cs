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

    public Window MainWindow;
    public Image UnitMovesIcon;
    public Label UnitAttackLabel;
    public Label UnitDefenseLabel;
    public Label UnitMovesLabel;
    public Label UnitDamageLabel;
    public Label UnitResistanceLabel;
    public Label UnitHitsLabel;
    public Memo UnitNameMemo;
    public ListBox UnitAbilityList;
    public Label AbilityNameLabel;
    public Panel CombatUnitPanel;
    public ListBox TargetAbilityList;
    public Panel CombatAbilityPanel;
    public Label AbilityAttackLabel;
    public ListBox AbilityStatisticsList;
    public Label AbilityDamageLabel;
    public Label TargetUnitLabel;
    public Panel TargetSelectPanel;
    public Panel NoTargetPanel;
    public Gauge UnitHitsGauge;
    public Gauge UnitManaGauge;
    public Image TargetMovesIcon;
    public Label TargetAttackLabel;
    public Label TargetDefenseLabel;
    public Label TargetMovesLabel;
    public Label TargetDamageLabel;
    public Label TargetResistanceLabel;
    public Label TargetHitsLabel;
    public Panel UnitFramePanel;

    // AoWTCUnit.pas:106
    public void TCUnitWinResize()
    {
        this.MainWindow.Alignment.RightOffset = this.MainWindow.WindowManager.WinWidth - this.MainWindow.WinLeft - this.MainWindow.WinWidth;
        this.MainWindow.Alignment.BottomOffset = this.MainWindow.WindowManager.WinHeight - this.MainWindow.WinTop - this.MainWindow.WinHeight;
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
                var combatMode = GamePack.Current.AbilityControl.GetAbility(((CombatAbilityRef)ability).Ability).GetCombatMode(UI.General.CombatUnit.Unit.Troop);
                bool test = !Set<CombatMode>.Of().WithRange(CombatMode.ShortRange, CombatMode.LongRange).Contains(combatMode) && (UI.General.CombatUnit.Abilities[drawItem]).Ability != 52;
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
                        var name = ability.GetName(UI.General.CombatUnit.Unit.Troop);
                        InfoBanner.SetInfoBanner(UI.InfoBanner, name, ability.Description);
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
                var combatMode = GamePack.Current.AbilityControl.GetAbility(((CombatAbilityRef)ability).Ability).GetCombatMode(UI.General.CombatUnit.Unit.Troop);
                if (!Set<CombatMode>.Of().WithRange(CombatMode.ShortRange, CombatMode.LongRange).Contains(combatMode) && (UI.General.CombatUnit.Abilities[UnitAbilityList.HoverItem]).Ability != 52)
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
