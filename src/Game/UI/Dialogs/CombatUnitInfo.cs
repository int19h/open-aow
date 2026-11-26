#nullable disable warnings
// Translated from AoWTCUnit.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Graphics;
using Aow.Rtl;

using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.UI.Dialogs;

// components
public sealed partial class CombatUnitInfo(UIContext ui) : UIModule(ui)
{
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
    public void UAbilLBPreDrawItem(in Rect drawRectangle, int drawItem, ref readonly LockedSurface lockedSurface)
    {
        if (UI.General.CombatUnit is not null)
        {
            bool condition = UI.General.CombatUnit.Unit.GetPlayer() == UI.Engine.CombatMap.SeatedPlayer && UI.General.CombatUnit.Unit.MovePoints > 0 && drawItem < UI.General.CombatUnit.CombatAbilityCount;
            if (condition)
            {
                CombatAbilityRef ability = UI.General.CombatUnit.Abilities[drawItem];
                CombatMode combatMode = UI.Engine.GamePack.AbilityControl[ability.Ability].GetCombatMode(UI.General.CombatUnit.Unit.Troop);
                bool test = combatMode is not (>= CombatMode.ShortRange and <= CombatMode.LongRange) && UI.General.CombatUnit.Abilities[drawItem].Ability != 52;
                if (!test)
                {
                    int moves = UI.General.CombatUnit.Unit.Troop.Moves;
                    test = UI.General.CombatUnit.Unit.MovePoints == moves;
                }

                condition = test;
            }

            UnitAbilityList.TextColor = condition ? Color.White : Color.Silver;
        }
    }

    // AoWTCUnit.pas:138
    public void TCUnitPnlDrawBack(LockedSurface lockedSurface) => UI.General.CombatUnit?.Unit.Troop.Show(CombatUnitPanel.AbsoluteLeft - 10, CombatUnitPanel.AbsoluteTop - 2, lockedSurface, info: UI.General.UnitShowInfo);

    // AoWTCUnit.pas:146
    public void UAbilLBUpdate()
    {
        if (UnitAbilityList.HoverItem != UnitAbilityList.Selected)
        {
            UnitAbilityList.Selected = UnitAbilityList.HoverItem;
        }
    }

    // AoWTCUnit.pas:152
    public void UAbilLBMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (UnitAbilityList.Selected > -1)
        {
            if (button == MouseButton.Left)
            {
                object? objectValue = UnitAbilityList.Items.Objects[UnitAbilityList.Selected];
                int selectAbility = UI.General.CombatUnit.SelectAbility(ObjectToInt(objectValue));
                UnitAbilityList.Selected = selectAbility;
            }
            else
            {
                if (button == MouseButton.Right && UI.General.CombatUnit is not null)
                {
                    object? objectValue = UnitAbilityList.Items.Objects[UnitAbilityList.Selected];
                    Ability? ability = UI.Engine.GamePack.AbilityControl[ObjectToInt(objectValue)];
                    if (ability is not null)
                    {
                        InfoBanner.SetInfoBanner(caption: ability.GetName(UI.General.CombatUnit.Unit.Troop), banner: UI.InfoBanner, description: ability.Description);
                    }
                }
            }
        }
    }

    // AoWTCUnit.pas:168
    public void UAbilLBMouseMove(ShiftStates shift, int X, int Y)
    {
        if (UI.General.CombatUnit is not null)
        {
            if (UI.General.CombatUnit.Unit.GetPlayer() == UI.Engine.CombatMap.SeatedPlayer && UI.General.CombatUnit.Unit.MovePoints != 0 && UnitAbilityList.HoverItem < UI.General.CombatUnit.CombatAbilityCount)
            {
                CombatAbilityRef ability = UI.General.CombatUnit.Abilities[UnitAbilityList.HoverItem];
                CombatMode combatMode = UI.Engine.GamePack.AbilityControl[ability.Ability].GetCombatMode(UI.General.CombatUnit.Unit.Troop);
                if (combatMode is not (>= CombatMode.ShortRange and <= CombatMode.LongRange) && UI.General.CombatUnit.Abilities[UnitAbilityList.HoverItem].Ability != 52)
                {
                    return;
                }

                TacticalBattleUnit tacticalBattleUnit = UI.General.CombatUnit.Unit;
                if (tacticalBattleUnit.Troop.Moves <= tacticalBattleUnit.MovePoints)
                {
                    return;
                }
            }

            UnitAbilityList.HoverItem = -1;
        }
    }

    // AoWTCUnit.pas:187
    public void HeroSMapMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        UI.General.SelectedUnit = UI.General.CombatUnit.Unit.Troop;
        UI.UnitWindow.ShowUnit();
    }

    // AoWTCUnit.pas:194
    public void CenterClick()
    {
        UI.General.PlayButtonSound(0);
        int hexLevel = UI.General.CombatUnit.HexLevel;
        int hexY = UI.General.CombatUnit.HexY;
        int hexX = UI.General.CombatUnit.HexX;
        UI.Engine.CombatMap.CenterViewTo(hexX, hexY, hexLevel);
        int fCombatUnitHexLevel = UI.General.CombatUnit.HexLevel;
        int fCombatUnitHexY = UI.General.CombatUnit.HexY;
        int fCombatUnitHexX = UI.General.CombatUnit.HexX;
        UI.Engine.CombatMap.PlayCenterAnimation(fCombatUnitHexX, fCombatUnitHexY, L: fCombatUnitHexLevel);
    }

    // AoWTCUnit.pas:195
    public void CloseClick()
    {
        UI.General.PlayButtonSound(0);
        MainWindow.Visible = false;
    }
}
