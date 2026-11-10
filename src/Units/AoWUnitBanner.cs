// Translated from AoWUnitBanner.pas.
using Aow.Game.Combat;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// published labels, popup and ability-list controls
public partial class UnitBanner : DataModule
{
    public UnitBanner(Component owner) : base(owner)
    { }

    public AOWLabel UnitNameLbl;
    public AOWImage AttackIcon;
    public AOWImage DefenseIcon;
    public AOWImage MovesIcon;
    public AOWImage DamageIcon;
    public AOWImage ResistanceIcon;
    public AOWImage HitsIcon;
    public AOWLabel Attack;
    public AOWLabel Defense;
    public AOWLabel Moves;
    public AOWLabel Damage;
    public AOWLabel Resistance;
    public AOWLabel Hits;
    public AOWMenu BannerPopup;
    public AOWListBox AbLB;
    public AOWVScrollBar AbVSB;

    // AoWUnitBanner.pas:53
    public void BannerPopupShow()
    {
        CombatAbilityRef A;
        if (DialogHosts.General.FSelectedUnit != null)
        {
            var name = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.GetName();
            UnitNameLbl.Text = name;
            BannerPopup.FAligned = false;
            var hitPoints = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.GetHitPoints();
            var hits = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.GetHits();
            Hits.Text = IntToStr(hitPoints) + "/" + IntToStr(hits);
            var movePoints = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.GetMovePoints();
            var moves = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.GetMoves();
            Moves.Text = IntToStr(movePoints) + "/" + IntToStr(moves);
            var attack = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.GetAttack();
            Attack.Text = IntToStr(attack);
            var damage = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.GetDamage();
            Damage.Text = IntToStr(damage);
            var defense = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.GetDefense();
            Defense.Text = IntToStr(defense);
            var resistance = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.GetResistance();
            Resistance.Text = IntToStr(resistance);
            var moveTypes = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.GetMoveTypes();
            var mTtoInt = Aow.Game.Ui.Dialogs.DialogHosts.General.MTtoInt(moveTypes);
            MovesIcon.CurrentImage = mTtoInt;
            AbLB.Items.Clear();
            Aow.Game.Ui.Dialogs.DialogHosts.General.BuildAbilities();
            for (int I = 0, last = DialogHosts.General.FAbilities.Count - 1; I <= last; I++)
            {
                A = (CombatAbilityRef)DialogHosts.General.FAbilities[I];
                var name2 = Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(A.Ability).GetName(DialogHosts.General.FSelectedUnit);
                AbLB.Items.Add(name2);
            }

            AbLB.Selected = -1;
            this.AbLB.F188 = -1;
            this.AbLB.ListOffset = 0;
        }
    }

    // AoWUnitBanner.pas:80
    public void AbLBUpdate()
    {
        if (AbLB.F188 != AbLB.Selected)
        {
            AbLB.Selected = AbLB.F188;
        }
    }

    // AoWUnitBanner.pas:85
    public void AbLBMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        if (DialogHosts.General.FSelectedUnit != null && AbLB.F188 >= DialogHosts.General.FActiveAbilityCount)
        {
            AbLB.F188 = -1;
        }
    }

    // AoWUnitBanner.pas:90
    public void BannerPopupMouseDown(int Sender, byte Button, int Shift, int X, int Y)
    {
        if (Button == 1)
        {
            BannerPopup.Visible = false;
        }
    }

    // AoWUnitBanner.pas:95
    public void AbLBMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        string Reason = "";
        if (DialogHosts.General.FSelectedUnit != null)
        {
            if (AbLB.Selected > -1 && AbLB.Selected < DialogHosts.General.FActiveAbilityCount)
            {
                var ability = DialogHosts.General.FAbilities[AbLB.Selected];
                Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(((CombatAbilityRef)ability).Ability).Activate(DialogHosts.General.FSelectedUnit, ref Reason);
                BannerPopup.Visible = false;
            }
            else
            {
                AbLB.Selected = -1;
            }
        }
    }
}
