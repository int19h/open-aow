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

    public Label UnitNameLabel;
    public Image MovesIcon;
    public Label AttackLabel;
    public Label DefenseLabel;
    public Label MovesLabel;
    public Label DamageLabel;
    public Label ResistanceLabel;
    public Label HitsLabel;
    public Menu BannerMenu;
    public ListBox AbilityList;

    // AoWUnitBanner.pas:53
    public void BannerPopupShow()
    {
        CombatAbilityRef A;
        if (DialogHosts.General.SelectedUnit != null)
        {
            var name = DialogHosts.General.SelectedUnit.GetName();
            UnitNameLabel.Text = name;
            BannerMenu.Aligned = false;
            var hitPoints = DialogHosts.General.SelectedUnit.GetHitPoints();
            var hits = DialogHosts.General.SelectedUnit.GetHits();
            HitsLabel.Text = IntToStr(hitPoints) + "/" + IntToStr(hits);
            var movePoints = DialogHosts.General.SelectedUnit.GetMovePoints();
            var moves = DialogHosts.General.SelectedUnit.GetMoves();
            MovesLabel.Text = IntToStr(movePoints) + "/" + IntToStr(moves);
            var attack = DialogHosts.General.SelectedUnit.GetAttack();
            AttackLabel.Text = IntToStr(attack);
            var damage = DialogHosts.General.SelectedUnit.GetDamage();
            DamageLabel.Text = IntToStr(damage);
            var defense = DialogHosts.General.SelectedUnit.GetDefense();
            DefenseLabel.Text = IntToStr(defense);
            var resistance = DialogHosts.General.SelectedUnit.GetResistance();
            ResistanceLabel.Text = IntToStr(resistance);
            var moveTypes = DialogHosts.General.SelectedUnit.GetMoveTypes();
            var moveTypesToInt32 = DialogHosts.General.MoveTypesToInt32(moveTypes);
            MovesIcon.CurrentImage = moveTypesToInt32;
            AbilityList.Items.Clear();
            DialogHosts.General.BuildAbilities();
            for (int I = 0, last = DialogHosts.General.Abilities.Count - 1; I <= last; I++)
            {
                A = DialogHosts.General.Abilities[I];
                var name2 = GamePack.Current.AbilityControl.GetAbility(A.Ability).GetName(DialogHosts.General.SelectedUnit);
                AbilityList.Items.Add(name2);
            }

            AbilityList.Selected = -1;
            this.AbilityList.HoverItem = -1;
            this.AbilityList.ListOffset = 0;
        }
    }

    // AoWUnitBanner.pas:80
    public void AbLBUpdate()
    {
        if (AbilityList.HoverItem != AbilityList.Selected)
        {
            AbilityList.Selected = AbilityList.HoverItem;
        }
    }

    // AoWUnitBanner.pas:85
    public void AbLBMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        if (DialogHosts.General.SelectedUnit != null && AbilityList.HoverItem >= DialogHosts.General.ActiveAbilityCount)
        {
            AbilityList.HoverItem = -1;
        }
    }

    // AoWUnitBanner.pas:90
    public void BannerPopupMouseDown(int sender, byte button, int shift, int X, int Y)
    {
        if (button == 1)
        {
            DialogHosts.UnitBanner.BannerMenu.Visible = false;
        }
    }

    // AoWUnitBanner.pas:95
    public void AbLBMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        string reason = "";
        if (DialogHosts.General.SelectedUnit != null)
        {
            if (AbilityList.Selected > -1 && AbilityList.Selected < DialogHosts.General.ActiveAbilityCount)
            {
                var ability = DialogHosts.General.Abilities[AbilityList.Selected];
                GamePack.Current.AbilityControl.GetAbility(((CombatAbilityRef)ability).Ability).Activate(DialogHosts.General.SelectedUnit, ref reason);
                BannerMenu.Visible = false;
            }
            else
            {
                AbilityList.Selected = -1;
            }
        }
    }
}
