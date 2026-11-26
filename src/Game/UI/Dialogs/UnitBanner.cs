#nullable disable warnings
// Translated from AoWUnitBanner.pas.
using System.Globalization;
using Aow.Game.Combat;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// published labels, popup and ability-list controls
public sealed partial class UnitBanner(UIContext ui) : UIModule(ui)
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
        if (UI.General.SelectedUnit is not null)
        {
            string name = UI.General.SelectedUnit.GetName();
            UnitNameLabel.Text = name;
            BannerMenu.Aligned = false;
            int hitPoints = UI.General.SelectedUnit.GetHitPoints();
            int hits = UI.General.SelectedUnit.GetHits();
            HitsLabel.Text = $"{hitPoints.ToString(CultureInfo.InvariantCulture)}/{hits.ToString(CultureInfo.InvariantCulture)}";
            int movePoints = UI.General.SelectedUnit.GetMovePoints();
            int moves = UI.General.SelectedUnit.Moves;
            MovesLabel.Text = $"{movePoints.ToString(CultureInfo.InvariantCulture)}/{moves.ToString(CultureInfo.InvariantCulture)}";
            int attack = UI.General.SelectedUnit.GetAttack();
            AttackLabel.Text = attack.ToString(CultureInfo.InvariantCulture);
            int damage = UI.General.SelectedUnit.GetDamage();
            DamageLabel.Text = damage.ToString(CultureInfo.InvariantCulture);
            int defense = UI.General.SelectedUnit.GetDefense();
            DefenseLabel.Text = defense.ToString(CultureInfo.InvariantCulture);
            int resistance = UI.General.SelectedUnit.GetResistance();
            ResistanceLabel.Text = resistance.ToString(CultureInfo.InvariantCulture);
            MoveTypes moveTypes = UI.General.SelectedUnit.GetMoveTypes();
            int moveTypesToInt32 = UI.General.MoveTypesToInt32(moveTypes);
            MovesIcon.CurrentImage = moveTypesToInt32;
            AbilityList.Items.Clear();
            UI.General.BuildAbilities();
            for (int i = 0, last = UI.General.Abilities.Count - 1; i <= last; i++)
            {
                CombatAbilityRef a = UI.General.Abilities[i];
                string name2 = UI.Engine.GamePack.AbilityControl[a.Ability].GetName(UI.General.SelectedUnit);
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
    public void AbLBMouseMove(ShiftStates shift, int X, int Y)
    {
        if (UI.General.SelectedUnit is not null && AbilityList.HoverItem >= UI.General.ActiveAbilityCount)
        {
            AbilityList.HoverItem = -1;
        }
    }

    // AoWUnitBanner.pas:90
    public void BannerPopupMouseDown(int sender, int button, int shift, int X, int Y)
    {
        if (button == 1)
        {
            UI.UnitBanner.BannerMenu.Visible = false;
        }
    }

    // AoWUnitBanner.pas:95
    public void AbLBMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (UI.General.SelectedUnit is not null)
        {
            if (AbilityList.Selected > -1 && AbilityList.Selected < UI.General.ActiveAbilityCount)
            {
                CombatAbilityRef ability = UI.General.Abilities[AbilityList.Selected];
                string reason = "";
                _ = UI.Engine.GamePack.AbilityControl[ability.Ability].Activate(UI.General.SelectedUnit, ref reason);
                BannerMenu.Visible = false;
            }
            else
            {
                AbilityList.Selected = -1;
            }
        }
    }
}
