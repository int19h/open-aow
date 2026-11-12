// Translated from AoWTCUnit.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// components
public partial class CombatUnitInfo : DataModule
{
    public CombatUnitInfo(Component owner) : base(owner)
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
        DialogHosts.General.PlayButtonSound(0);
        MainWindow.Visible = false;
    }

    // AoWTCUnit.pas:123
    public void UAbilLBPreDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        if (DialogHosts.General.CombatUnit != null)
        {
            bool condition = DialogHosts.General.CombatUnit.Unit.GetPlayer() == CombatMap.Current.SeatedPlayer && DialogHosts.General.CombatUnit.Unit.MovePoints > 0 && drawItem < DialogHosts.General.CombatUnit.CombatAbilityCount;
            if (condition)
            {
                var ability = DialogHosts.General.CombatUnit.Abilities[drawItem];
                var combatMode = AoWHSSet.Current.AbilityControl.GetAbility(((CombatAbilityRef)ability).Ability).GetCombatMode(DialogHosts.General.CombatUnit.Unit.Troop);
                bool test = !Set<CombatMode>.Of().WithRange(CombatMode.ShortRange, CombatMode.LongRange).Contains(combatMode) && (DialogHosts.General.CombatUnit.Abilities[drawItem]).Ability != 52;
                if (!test)
                {
                    var moves = DialogHosts.General.CombatUnit.Unit.Troop.GetMoves();
                    test = DialogHosts.General.CombatUnit.Unit.MovePoints == moves;
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
        if (DialogHosts.General.CombatUnit != null)
        {
            DialogHosts.General.CombatUnit.Unit.Troop.Show(CombatUnitPanel.WinLeft - 10, CombatUnitPanel.WinTop - 2, lockedSurface, DialogHosts.General.UnitShowInfo);
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
                var selectAbility = DialogHosts.General.CombatUnit.SelectAbility(ObjectToInt(objectValue));
                UnitAbilityList.Selected = selectAbility;
            }
            else
            {
                if (button == MouseButton.Right && DialogHosts.General.CombatUnit != null)
                {
                    var objectValue = UnitAbilityList.Items.Objects[UnitAbilityList.Selected];
                    ability = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
                    if (ability != null)
                    {
                        var name = ability.GetName(DialogHosts.General.CombatUnit.Unit.Troop);
                        InfoBanner.SetInfoBanner(DialogHosts.InfoBanner, name, ability.Description);
                    }
                }
            }
        }
    }

    // AoWTCUnit.pas:168
    public void UAbilLBMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        if (DialogHosts.General.CombatUnit != null)
        {
            if (DialogHosts.General.CombatUnit.Unit.GetPlayer() == CombatMap.Current.SeatedPlayer && DialogHosts.General.CombatUnit.Unit.MovePoints != 0 && UnitAbilityList.HoverItem < DialogHosts.General.CombatUnit.CombatAbilityCount)
            {
                var ability = DialogHosts.General.CombatUnit.Abilities[UnitAbilityList.HoverItem];
                var combatMode = AoWHSSet.Current.AbilityControl.GetAbility(((CombatAbilityRef)ability).Ability).GetCombatMode(DialogHosts.General.CombatUnit.Unit.Troop);
                if (!Set<CombatMode>.Of().WithRange(CombatMode.ShortRange, CombatMode.LongRange).Contains(combatMode) && (DialogHosts.General.CombatUnit.Abilities[UnitAbilityList.HoverItem]).Ability != 52)
                {
                    return;
                }

                var tacticalBattleUnit = DialogHosts.General.CombatUnit.Unit;
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
        DialogHosts.General.SelectedUnit = DialogHosts.General.CombatUnit.Unit.Troop;
        DialogHosts.UnitWindow.ShowUnit();
    }

    // AoWTCUnit.pas:194
    public void CenterClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var hexLevel = DialogHosts.General.CombatUnit.GetHexLevel();
        var hexY = DialogHosts.General.CombatUnit.GetHexY();
        var hexX = DialogHosts.General.CombatUnit.GetHexX();
        CombatMap.Current.CenterViewTo(hexX, hexY, hexLevel);
        var fCombatUnitHexLevel = DialogHosts.General.CombatUnit.GetHexLevel();
        var fCombatUnitHexY = DialogHosts.General.CombatUnit.GetHexY();
        var fCombatUnitHexX = DialogHosts.General.CombatUnit.GetHexX();
        CombatMap.Current.PlayCenterAnimation(fCombatUnitHexX, fCombatUnitHexY, fCombatUnitHexLevel);
    }

    // AoWTCUnit.pas:195
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        MainWindow.Visible = false;
    }
}
