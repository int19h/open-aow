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
public partial class TTCUnit : TDataModule
{
    public TTCUnit(TComponent owner) : base(owner)
    { }

    public TAOWWindow TCUnitWin;
    public TAOWImage SIcon;
    public TAOWButton Min;
    public TAOWButton Close;
    public TAOWLabel IWTitle;
    public TAOWImage UAttackIcon;
    public TAOWImage UDefenseIcon;
    public TAOWImage UMovesIcon;
    public TAOWImage UDamageIcon;
    public TAOWImage UResistanceIcon;
    public TAOWImage UHitsIcon;
    public TAOWLabel UAttack;
    public TAOWLabel UDefense;
    public TAOWLabel UMoves;
    public TAOWLabel UDamage;
    public TAOWLabel UResistance;
    public TAOWLabel UHits;
    public TAOWMemo UnitName;
    public TAOWPanel UStatPnl;
    public TAOWListBox UAbilLB;
    public TAOWLabel AbilNameLbl;
    public TAOWPanel TCUnitPnl;
    public TAOWImage AOWImage1;
    public TAOWMemo AUnitName;
    public TAOWListBox USelAbilLb;
    public TAOWPanel TCAbilPnl;
    public TAOWImage AbilAtt;
    public TAOWLabel AbilAttLbl;
    public TAOWPanel AbilStatBox;
    public TAOWListBox AbilStat;
    public TAOWImage AbilDam;
    public TAOWLabel AbilDamLbl;
    public TAOWLabel TargetLbl;
    public TAOWLabel TargetULbl;
    public TAOWPanel TargetSelPnl;
    public TAOWPanel NoTargetPnl;
    public TAOWMemo TargetMessage;
    public TAoWGauge UnitHits;
    public TAoWGauge UnitMana;
    public TAOWImage CelticCross;
    public TAOWPanel UStatP1;
    public TAOWPanel UStatP2;
    public TAOWImage AAttackIcon;
    public TAOWImage ADefenseIcon;
    public TAOWImage AMovesIcon;
    public TAOWImage ADamageIcon;
    public TAOWImage AResistanceIcon;
    public TAOWImage AHitsIcons;
    public TAOWLabel AAttack;
    public TAOWLabel ADefense;
    public TAOWLabel AMoves;
    public TAOWLabel ADamage;
    public TAOWLabel AResistance;
    public TAOWLabel AHits;
    public TAOWPanel AStatPnl;
    public TAOWPanel AStatP1;
    public TAOWPanel AStatP2;
    public TAOWButton Center;
    public TAOWVScrollBar AbilSB;
    public TAOWVScrollBar UAbilSB;
    public TAOWVScrollBar AOWVScrollBar1;
    public TAOWPanel AbilFr;
    public TAOWPanel ANameFr;
    public TAOWPanel TargerFr;
    public TAOWPanel TNamePnl;
    public TAOWPanel UnitFr;
    public TaowUserDrawBox UnitBox;
    public TIvTranslator Translator;

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
    public void UAbilLBPreDrawItem(in TRect DrawRect, int DrawItem, ref TSurfaceDesc SD)
    {
        if (Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit != null)
        {
            bool condition = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.GetPlayer() == Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FSeatedPlayer && Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FMovePoints > 0 && DrawItem < Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FCombatAbilityCount;
            if (condition)
            {
                var ability = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FAbilities[DrawItem];
                var combatMode = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(((TTCAbility)ability).Ability).GetCombatMode(Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit);
                bool test = !TSet<TCombatMode>.Of().WithRange(TCombatMode.cmShortRange, TCombatMode.cmLongRange).Contains(combatMode) && ((TTCAbility)Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FAbilities[DrawItem]).Ability != 52;
                if (!test)
                {
                    var moves = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit.GetMoves();
                    test = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FMovePoints == moves;
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
    public void TCUnitPnlDrawBack(TSurfaceDesc SD)
    {
        if (Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit != null)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit.ShowEx(TCUnitPnl.WinLeft - 10, TCUnitPnl.WinTop - 2, SD, Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitShowInfo);
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
    public void UAbilLBMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        TAbility Ability;
        if (UAbilLB.Selected > -1)
        {
            if (Button == TMouseButton.mbLeft)
            {
                var objectValue = UAbilLB.Items.Objects[UAbilLB.Selected];
                var selectAbility = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.SelectAbility(ObjectToInt(objectValue));
                UAbilLB.Selected = selectAbility;
            }
            else
            {
                if (Button == TMouseButton.mbRight && Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit != null)
                {
                    var objectValue = UAbilLB.Items.Objects[UAbilLB.Selected];
                    Ability = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue));
                    if (Ability != null)
                    {
                        var name = Ability.GetName(Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit);
                        Aow.Game.Ui.Dialogs.TInfoBanner.SetInfoBanner(Aow.Game.Ui.Dialogs.DialogHosts.InfoBanner, name, Ability.FDescription);
                    }
                }
            }
        }
    }

    // AoWTCUnit.pas:168
    public void UAbilLBMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        if (Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit != null)
        {
            if (Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.GetPlayer() == Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FSeatedPlayer && Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FMovePoints != 0 && UAbilLB.F188 < Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FCombatAbilityCount)
            {
                var ability = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FAbilities[UAbilLB.F188];
                var combatMode = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(((TTCAbility)ability).Ability).GetCombatMode(Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit);
                if (!TSet<TCombatMode>.Of().WithRange(TCombatMode.cmShortRange, TCombatMode.cmLongRange).Contains(combatMode) && ((TTCAbility)Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FAbilities[UAbilLB.F188]).Ability != 52)
                {
                    return;
                }

                var tacticalCombatUnit = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit;
                if (tacticalCombatUnit.FUnit.GetMoves() <= tacticalCombatUnit.FMovePoints)
                {
                    return;
                }
            }

            UAbilLB.F188 = -1;
        }
    }

    // AoWTCUnit.pas:187
    public void HeroSMapMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit;
        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ShowUnit();
    }

    // AoWTCUnit.pas:194
    public void CenterClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        var lhx = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.GetLhx();
        var yhx = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.GetYhx();
        var xhx = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.GetXhx();
        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.CenterViewTo(xhx, yhx, lhx);
        var fCombatUnitLhx = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.GetLhx();
        var fCombatUnitYhx = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.GetYhx();
        var fCombatUnitXhx = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.GetXhx();
        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.PlayCenterAnimation(fCombatUnitXhx, fCombatUnitYhx, fCombatUnitLhx);
    }

    // AoWTCUnit.pas:195
    public void CloseClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        TCUnitWin.Visible = false;
    }
}
