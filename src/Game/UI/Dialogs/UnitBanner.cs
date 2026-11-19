#nullable disable
// Translated from AoWUnitBanner.pas.
using Aow.Game.Combat;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.UI.Dialogs;

// published labels, popup and ability-list controls
public partial class UnitBanner(UIContext ui) : UIModule(ui)
{
    public Label UnitNameLabel { get; private set; }
    public Image MovesIcon { get; private set; }
    public Label AttackLabel { get; private set; }
    public Label DefenseLabel { get; private set; }
    public Label MovesLabel { get; private set; }
    public Label DamageLabel { get; private set; }
    public Label ResistanceLabel { get; private set; }
    public Label HitsLabel { get; private set; }
    public Menu BannerMenu { get; private set; }
    public ListBox AbilityList { get; private set; }

    // AoWUnitBanner.pas:53
    public void BannerPopupShow()
    {
        CombatAbilityRef a;
        if (UI.General.SelectedUnit != null)
        {
            string name = UI.General.SelectedUnit.GetName();
            UnitNameLabel.Text = name;
            BannerMenu.Aligned = false;
            sbyte hitPoints = UI.General.SelectedUnit.GetHitPoints();
            sbyte hits = UI.General.SelectedUnit.GetHits();
            HitsLabel.Text = $"{IntToStr(hitPoints)}/{IntToStr(hits)}";
            sbyte movePoints = UI.General.SelectedUnit.GetMovePoints();
            sbyte moves = UI.General.SelectedUnit.GetMoves();
            MovesLabel.Text = $"{IntToStr(movePoints)}/{IntToStr(moves)}";
            sbyte attack = UI.General.SelectedUnit.GetAttack();
            AttackLabel.Text = IntToStr(attack);
            sbyte damage = UI.General.SelectedUnit.GetDamage();
            DamageLabel.Text = IntToStr(damage);
            sbyte defense = UI.General.SelectedUnit.GetDefense();
            DefenseLabel.Text = IntToStr(defense);
            sbyte resistance = UI.General.SelectedUnit.GetResistance();
            ResistanceLabel.Text = IntToStr(resistance);
            Set<MoveType> moveTypes = UI.General.SelectedUnit.GetMoveTypes();
            int moveTypesToInt32 = UI.General.MoveTypesToInt32(moveTypes);
            MovesIcon.CurrentImage = moveTypesToInt32;
            AbilityList.Items.Clear();
            UI.General.BuildAbilities();
            for (int i = 0, last = UI.General.Abilities.Count - 1; i <= last; i++)
            {
                a = UI.General.Abilities[i];
                string name2 = GamePack.Current.AbilityControl.GetAbility(a.Ability).GetName(UI.General.SelectedUnit);
                _ = AbilityList.Items.Add(name2);
            }

            AbilityList.Selected = -1;
            AbilityList.HoverItem = -1;
            AbilityList.ListOffset = 0;
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
        if (UI.General.SelectedUnit != null && AbilityList.HoverItem >= UI.General.ActiveAbilityCount)
        {
            AbilityList.HoverItem = -1;
        }
    }

    // AoWUnitBanner.pas:90
    public void BannerPopupMouseDown(int sender, byte button, int shift, int X, int Y)
    {
        if (button == 1)
        {
            UI.UnitBanner.BannerMenu.Visible = false;
        }
    }

    // AoWUnitBanner.pas:95
    public void AbLBMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        string reason = "";
        if (UI.General.SelectedUnit != null)
        {
            if (AbilityList.Selected > -1 && AbilityList.Selected < UI.General.ActiveAbilityCount)
            {
                CombatAbilityRef ability = UI.General.Abilities[AbilityList.Selected];
                _ = GamePack.Current.AbilityControl.GetAbility(ability.Ability).Activate(UI.General.SelectedUnit, ref reason);
                BannerMenu.Visible = false;
            }
            else
            {
                AbilityList.Selected = -1;
            }
        }
    }
}
