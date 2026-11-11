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

    public Window TCUnitWin;
    public Image SIcon;
    public Button Min;
    public Button Close;
    public Label IWTitle;
    public Image UAttackIcon;
    public Image UDefenseIcon;
    public Image UMovesIcon;
    public Image UDamageIcon;
    public Image UResistanceIcon;
    public Image UHitsIcon;
    public Label UAttack;
    public Label UDefense;
    public Label UMoves;
    public Label UDamage;
    public Label UResistance;
    public Label UHits;
    public Memo UnitName;
    public Panel UStatPnl;
    public ListBox UAbilLB;
    public Label AbilNameLbl;
    public Panel TCUnitPnl;
    public Image AOWImage1;
    public Memo AUnitName;
    public ListBox USelAbilLb;
    public Panel TCAbilPnl;
    public Image AbilAtt;
    public Label AbilAttLbl;
    public Panel AbilStatBox;
    public ListBox AbilStat;
    public Image AbilDam;
    public Label AbilDamLbl;
    public Label TargetLbl;
    public Label TargetULbl;
    public Panel TargetSelPnl;
    public Panel NoTargetPnl;
    public Memo TargetMessage;
    public Gauge UnitHits;
    public Gauge UnitMana;
    public Image CelticCross;
    public Panel UStatP1;
    public Panel UStatP2;
    public Image AAttackIcon;
    public Image ADefenseIcon;
    public Image AMovesIcon;
    public Image ADamageIcon;
    public Image AResistanceIcon;
    public Image AHitsIcons;
    public Label AAttack;
    public Label ADefense;
    public Label AMoves;
    public Label ADamage;
    public Label AResistance;
    public Label AHits;
    public Panel AStatPnl;
    public Panel AStatP1;
    public Panel AStatP2;
    public Button Center;
    public VerticalScrollBar AbilSB;
    public VerticalScrollBar UAbilSB;
    public VerticalScrollBar AOWVScrollBar1;
    public Panel AbilFr;
    public Panel ANameFr;
    public Panel TargerFr;
    public Panel TNamePnl;
    public Panel UnitFr;
    public UserDrawBox UnitBox;
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
        DialogHosts.General.PlayButtonSound(0);
        TCUnitWin.Visible = false;
    }

    // AoWTCUnit.pas:123
    public void UAbilLBPreDrawItem(in Rect DrawRect, int DrawItem, ref LockedSurface SD)
    {
        if (DialogHosts.General.FCombatUnit != null)
        {
            bool condition = DialogHosts.General.FCombatUnit.FUnit.GetPlayer() == CombatMap.Current.FSeatedPlayer && DialogHosts.General.FCombatUnit.FUnit.FMovePoints > 0 && DrawItem < DialogHosts.General.FCombatUnit.FCombatAbilityCount;
            if (condition)
            {
                var ability = DialogHosts.General.FCombatUnit.FAbilities[DrawItem];
                var combatMode = AoWHSSet.Current.FAbilityControl.GetAbility(((CombatAbilityRef)ability).Ability).GetCombatMode(DialogHosts.General.FCombatUnit.FUnit.FUnit);
                bool test = !Set<CombatMode>.Of().WithRange(CombatMode.ShortRange, CombatMode.LongRange).Contains(combatMode) && (DialogHosts.General.FCombatUnit.FAbilities[DrawItem]).Ability != 52;
                if (!test)
                {
                    var moves = DialogHosts.General.FCombatUnit.FUnit.FUnit.GetMoves();
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
    public void TCUnitPnlDrawBack(LockedSurface SD)
    {
        if (DialogHosts.General.FCombatUnit != null)
        {
            DialogHosts.General.FCombatUnit.FUnit.FUnit.ShowEx(TCUnitPnl.WinLeft - 10, TCUnitPnl.WinTop - 2, SD, DialogHosts.General.FUnitShowInfo);
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
                var selectAbility = DialogHosts.General.FCombatUnit.SelectAbility(ObjectToInt(objectValue));
                UAbilLB.Selected = selectAbility;
            }
            else
            {
                if (Button == MouseButton.Right && DialogHosts.General.FCombatUnit != null)
                {
                    var objectValue = UAbilLB.Items.Objects[UAbilLB.Selected];
                    Ability = AoWHSSet.Current.FAbilityControl.GetAbility(ObjectToInt(objectValue));
                    if (Ability != null)
                    {
                        var name = Ability.GetName(DialogHosts.General.FCombatUnit.FUnit.FUnit);
                        InfoBanner.SetInfoBanner(DialogHosts.InfoBanner, name, Ability.FDescription);
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
            if (DialogHosts.General.FCombatUnit.FUnit.GetPlayer() == CombatMap.Current.FSeatedPlayer && DialogHosts.General.FCombatUnit.FUnit.FMovePoints != 0 && UAbilLB.F188 < DialogHosts.General.FCombatUnit.FCombatAbilityCount)
            {
                var ability = DialogHosts.General.FCombatUnit.FAbilities[UAbilLB.F188];
                var combatMode = AoWHSSet.Current.FAbilityControl.GetAbility(((CombatAbilityRef)ability).Ability).GetCombatMode(DialogHosts.General.FCombatUnit.FUnit.FUnit);
                if (!Set<CombatMode>.Of().WithRange(CombatMode.ShortRange, CombatMode.LongRange).Contains(combatMode) && (DialogHosts.General.FCombatUnit.FAbilities[UAbilLB.F188]).Ability != 52)
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
        DialogHosts.UnitWindow.ShowUnit();
    }

    // AoWTCUnit.pas:194
    public void CenterClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var lhx = DialogHosts.General.FCombatUnit.GetLhx();
        var yhx = DialogHosts.General.FCombatUnit.GetYhx();
        var xhx = DialogHosts.General.FCombatUnit.GetXhx();
        CombatMap.Current.CenterViewTo(xhx, yhx, lhx);
        var fCombatUnitLhx = DialogHosts.General.FCombatUnit.GetLhx();
        var fCombatUnitYhx = DialogHosts.General.FCombatUnit.GetYhx();
        var fCombatUnitXhx = DialogHosts.General.FCombatUnit.GetXhx();
        CombatMap.Current.PlayCenterAnimation(fCombatUnitXhx, fCombatUnitYhx, fCombatUnitLhx);
    }

    // AoWTCUnit.pas:195
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        TCUnitWin.Visible = false;
    }
}
