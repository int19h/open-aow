// Translated from AoWTCUnit.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWTCUnit.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// components
public partial class CombatUnitInfo : DataModule
{
    public CombatUnitInfo(Component owner) : base(owner)
    { }

    public AOWWindow TCUnitWin;
    public AOWImage SIcon;
    public AOWButton Min;
    public AOWButton Close;
    public AOWLabel IWTitle;
    public AOWImage UAttackIcon;
    public AOWImage UDefenseIcon;
    public AOWImage UMovesIcon;
    public AOWImage UDamageIcon;
    public AOWImage UResistanceIcon;
    public AOWImage UHitsIcon;
    public AOWLabel UAttack;
    public AOWLabel UDefense;
    public AOWLabel UMoves;
    public AOWLabel UDamage;
    public AOWLabel UResistance;
    public AOWLabel UHits;
    public AOWMemo UnitName;
    public AOWPanel UStatPnl;
    public AOWListBox UAbilLB;
    public AOWLabel AbilNameLbl;
    public AOWPanel TCUnitPnl;
    public AOWImage AOWImage1;
    public AOWMemo AUnitName;
    public AOWListBox USelAbilLb;
    public AOWPanel TCAbilPnl;
    public AOWImage AbilAtt;
    public AOWLabel AbilAttLbl;
    public AOWPanel AbilStatBox;
    public AOWListBox AbilStat;
    public AOWImage AbilDam;
    public AOWLabel AbilDamLbl;
    public AOWLabel TargetLbl;
    public AOWLabel TargetULbl;
    public AOWPanel TargetSelPnl;
    public AOWPanel NoTargetPnl;
    public AOWMemo TargetMessage;
    public AoWGauge UnitHits;
    public AoWGauge UnitMana;
    public AOWImage CelticCross;
    public AOWPanel UStatP1;
    public AOWPanel UStatP2;
    public AOWImage AAttackIcon;
    public AOWImage ADefenseIcon;
    public AOWImage AMovesIcon;
    public AOWImage ADamageIcon;
    public AOWImage AResistanceIcon;
    public AOWImage AHitsIcons;
    public AOWLabel AAttack;
    public AOWLabel ADefense;
    public AOWLabel AMoves;
    public AOWLabel ADamage;
    public AOWLabel AResistance;
    public AOWLabel AHits;
    public AOWPanel AStatPnl;
    public AOWPanel AStatP1;
    public AOWPanel AStatP2;
    public AOWButton Center;
    public AOWVScrollBar AbilSB;
    public AOWVScrollBar UAbilSB;
    public AOWVScrollBar AOWVScrollBar1;
    public AOWPanel AbilFr;
    public AOWPanel ANameFr;
    public AOWPanel TargerFr;
    public AOWPanel TNamePnl;
    public AOWPanel UnitFr;
    public TaowUserDrawBox UnitBox;
    public IvTranslator Translator;

    // AoWTCUnit.pas:106
    public void TCUnitWinResize()
    {
        this.TCUnitWin.Alignment.RightOffset = this.TCUnitWin.WinManager.WinWidth - this.TCUnitWin.WinLeft - this.TCUnitWin.WinWidth;
        this.TCUnitWin.Alignment.BottomOffset = this.TCUnitWin.WinManager.WinHeight - this.TCUnitWin.WinTop - this.TCUnitWin.WinHeight;
    }

    // AoWTCUnit.pas:116
    public void MinClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        TCUnitWin.Visible = false;
    }

    // AoWTCUnit.pas:123
    public void UAbilLBPreDrawItem(in Rect DrawRect, int DrawItem, ref SurfaceDesc SD)
    {
        if (DialogHosts.General.FCombatUnit != null)
        {
            bool condition = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.GetPlayer() == AoWCombatMap.Current.FSeatedPlayer && DialogHosts.General.FCombatUnit.FUnit.FMovePoints > 0 && DrawItem < DialogHosts.General.FCombatUnit.FCombatAbilityCount;
            if (condition)
            {
                var ability = DialogHosts.General.FCombatUnit.FAbilities[DrawItem];
                var combatMode = Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(((CombatAbilityRef)ability).Ability).GetCombatMode(DialogHosts.General.FCombatUnit.FUnit.FUnit);
                bool test = !Set<CombatMode>.Of().WithRange(CombatMode.ShortRange, CombatMode.LongRange).Contains(combatMode) && ((CombatAbilityRef)DialogHosts.General.FCombatUnit.FAbilities[DrawItem]).Ability != 52;
                if (!test)
                {
                    var moves = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit.GetMoves();
                    test = DialogHosts.General.FCombatUnit.FUnit.FMovePoints == moves;
                }

                condition = test;
            }

            if (condition)
            {
                UAbilLB.TextColor = 16777215;
            }
            else
            {
                UAbilLB.TextColor = 12632256;
            }
        }
    }

    // AoWTCUnit.pas:138
    public void TCUnitPnlDrawBack(SurfaceDesc SD)
    {
        if (DialogHosts.General.FCombatUnit != null)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit.ShowEx(TCUnitPnl.WinLeft - 10, TCUnitPnl.WinTop - 2, SD, DialogHosts.General.FUnitShowInfo);
        }
    }

    // AoWTCUnit.pas:146
    public void UAbilLBUpdate()
    {
        if (UAbilLB.F188 != UAbilLB.Selected)
        {
            UAbilLB.Selected = UAbilLB.F188;
        }
    }

    // AoWTCUnit.pas:152
    public void UAbilLBMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        Ability Ability;
        if (UAbilLB.Selected > -1)
        {
            if (Button == MouseButton.Left)
            {
                var objectValue = UAbilLB.Items.Objects[UAbilLB.Selected];
                var selectAbility = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.SelectAbility(ObjectToInt(objectValue));
                UAbilLB.Selected = selectAbility;
            }
            else
            {
                if (Button == MouseButton.Right && DialogHosts.General.FCombatUnit != null)
                {
                    var objectValue = UAbilLB.Items.Objects[UAbilLB.Selected];
                    Ability = Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(ObjectToInt(objectValue));
                    if (Ability != null)
                    {
                        var name = Ability.GetName(DialogHosts.General.FCombatUnit.FUnit.FUnit);
                        Aow.Game.Ui.Dialogs.InfoBanner.SetInfoBanner(DialogHosts.InfoBanner, name, Ability.FDescription);
                    }
                }
            }
        }
    }

    // AoWTCUnit.pas:168
    public void UAbilLBMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        if (DialogHosts.General.FCombatUnit != null)
        {
            if (Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.GetPlayer() == AoWCombatMap.Current.FSeatedPlayer && DialogHosts.General.FCombatUnit.FUnit.FMovePoints != 0 && UAbilLB.F188 < DialogHosts.General.FCombatUnit.FCombatAbilityCount)
            {
                var ability = DialogHosts.General.FCombatUnit.FAbilities[UAbilLB.F188];
                var combatMode = Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(((CombatAbilityRef)ability).Ability).GetCombatMode(DialogHosts.General.FCombatUnit.FUnit.FUnit);
                if (!Set<CombatMode>.Of().WithRange(CombatMode.ShortRange, CombatMode.LongRange).Contains(combatMode) && ((CombatAbilityRef)DialogHosts.General.FCombatUnit.FAbilities[UAbilLB.F188]).Ability != 52)
                {
                    return;
                }

                var tacticalCombatUnit = DialogHosts.General.FCombatUnit.FUnit;
                if (tacticalCombatUnit.FUnit.GetMoves() <= tacticalCombatUnit.FMovePoints)
                {
                    return;
                }
            }

            UAbilLB.F188 = -1;
        }
    }

    // AoWTCUnit.pas:187
    public void HeroSMapMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        DialogHosts.General.FSelectedUnit = DialogHosts.General.FCombatUnit.FUnit.FUnit;
        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ShowUnit();
    }

    // AoWTCUnit.pas:194
    public void CenterClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        var lhx = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.GetLhx();
        var yhx = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.GetYhx();
        var xhx = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.GetXhx();
        Aow.Game.Combat.AoWCombatMap.Current.CenterViewTo(xhx, yhx, lhx);
        var fCombatUnitLhx = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.GetLhx();
        var fCombatUnitYhx = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.GetYhx();
        var fCombatUnitXhx = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.GetXhx();
        Aow.Game.Combat.AoWCombatMap.Current.PlayCenterAnimation(fCombatUnitXhx, fCombatUnitYhx, fCombatUnitLhx);
    }

    // AoWTCUnit.pas:195
    public void CloseClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        TCUnitWin.Visible = false;
    }
}
