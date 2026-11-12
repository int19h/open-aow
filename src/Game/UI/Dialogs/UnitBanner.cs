// Translated from AoWUnitBanner.pas.
using Aow.Game.Combat;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// published labels, popup and ability-list controls
public partial class UnitBanner : DataModule
{
    public UnitBanner(Component owner) : base(owner)
    { }

    public Label UnitNameLbl;
    public Image AttackIcon;
    public Image DefenseIcon;
    public Image MovesIcon;
    public Image DamageIcon;
    public Image ResistanceIcon;
    public Image HitsIcon;
    public Label Attack;
    public Label Defense;
    public Label Moves;
    public Label Damage;
    public Label Resistance;
    public Label Hits;
    public Menu BannerPopup;
    public ListBox AbLB;
    public VerticalScrollBar AbVSB;

    // AoWUnitBanner.pas:53
    public void BannerPopupShow()
    {
        CombatAbilityRef A;
        if (DialogHosts.General.SelectedUnit != null)
        {
            var name = DialogHosts.General.SelectedUnit.GetName();
            UnitNameLbl.Text = name;
            BannerPopup.Aligned = false;
            var hitPoints = DialogHosts.General.SelectedUnit.GetHitPoints();
            var hits = DialogHosts.General.SelectedUnit.GetHits();
            Hits.Text = IntToStr(hitPoints) + "/" + IntToStr(hits);
            var movePoints = DialogHosts.General.SelectedUnit.GetMovePoints();
            var moves = DialogHosts.General.SelectedUnit.GetMoves();
            Moves.Text = IntToStr(movePoints) + "/" + IntToStr(moves);
            var attack = DialogHosts.General.SelectedUnit.GetAttack();
            Attack.Text = IntToStr(attack);
            var damage = DialogHosts.General.SelectedUnit.GetDamage();
            Damage.Text = IntToStr(damage);
            var defense = DialogHosts.General.SelectedUnit.GetDefense();
            Defense.Text = IntToStr(defense);
            var resistance = DialogHosts.General.SelectedUnit.GetResistance();
            Resistance.Text = IntToStr(resistance);
            var moveTypes = DialogHosts.General.SelectedUnit.GetMoveTypes();
            var moveTypesToInt32 = DialogHosts.General.MoveTypesToInt32(moveTypes);
            MovesIcon.CurrentImage = moveTypesToInt32;
            AbLB.Items.Clear();
            DialogHosts.General.BuildAbilities();
            for (int I = 0, last = DialogHosts.General.Abilities.Count - 1; I <= last; I++)
            {
                A = DialogHosts.General.Abilities[I];
                var name2 = AoWHSSet.Current.AbilityControl.GetAbility(A.Ability).GetName(DialogHosts.General.SelectedUnit);
                AbLB.Items.Add(name2);
            }

            AbLB.Selected = -1;
            this.AbLB.HoverItem = -1;
            this.AbLB.ListOffset = 0;
        }
    }

    // AoWUnitBanner.pas:80
    public void AbLBUpdate()
    {
        if (AbLB.HoverItem != AbLB.Selected)
        {
            AbLB.Selected = AbLB.HoverItem;
        }
    }

    // AoWUnitBanner.pas:85
    public void AbLBMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        if (DialogHosts.General.SelectedUnit != null && AbLB.HoverItem >= DialogHosts.General.ActiveAbilityCount)
        {
            AbLB.HoverItem = -1;
        }
    }

    // AoWUnitBanner.pas:90
    public void BannerPopupMouseDown(int sender, byte button, int shift, int X, int Y)
    {
        if (button == 1)
        {
            DialogHosts.UnitBanner.BannerPopup.Visible = false;
        }
    }

    // AoWUnitBanner.pas:95
    public void AbLBMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        string reason = "";
        if (DialogHosts.General.SelectedUnit != null)
        {
            if (AbLB.Selected > -1 && AbLB.Selected < DialogHosts.General.ActiveAbilityCount)
            {
                var ability = DialogHosts.General.Abilities[AbLB.Selected];
                AoWHSSet.Current.AbilityControl.GetAbility(((CombatAbilityRef)ability).Ability).Activate(DialogHosts.General.SelectedUnit, ref reason);
                BannerPopup.Visible = false;
            }
            else
            {
                AbLB.Selected = -1;
            }
        }
    }
}
