// Translated from AoWUnitBanner.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWTC;
using Aow.Units.AOWMenu;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWUnitBanner
{
    // published labels, popup and ability-list controls
    public partial class TUnitBanner : TDataModule
    {
        public TUnitBanner(TComponent owner) : base(owner)
        { }

        public TAOWLabel UnitNameLbl;
        public TAOWImage AttackIcon;
        public TAOWImage DefenseIcon;
        public TAOWImage MovesIcon;
        public TAOWImage DamageIcon;
        public TAOWImage ResistanceIcon;
        public TAOWImage HitsIcon;
        public TAOWLabel Attack;
        public TAOWLabel Defense;
        public TAOWLabel Moves;
        public TAOWLabel Damage;
        public TAOWLabel Resistance;
        public TAOWLabel Hits;
        public TAOWMenu BannerPopup;
        public TAOWListBox AbLB;
        public TAOWVScrollBar AbVSB;
        // AoWUnitBanner.pas:53
        public void BannerPopupShow(TObject Sender)
        {
            TTCAbility A;
            if (General.FSelectedUnit != null)
            {
                var name = General.FSelectedUnit.GetName();
                UnitNameLbl.Text = name;
                BannerPopup.FAligned = false;
                var hitPoints = General.FSelectedUnit.GetHitPoints();
                var hits = General.FSelectedUnit.GetHits();
                Hits.Text = IntToStr(hitPoints) + AnsiString.FromBytes(47) + IntToStr(hits);
                var movePoints = General.FSelectedUnit.GetMovePoints();
                var moves = General.FSelectedUnit.GetMoves();
                Moves.Text = IntToStr(movePoints) + AnsiString.FromBytes(47) + IntToStr(moves);
                var attack = General.FSelectedUnit.GetAttack();
                Attack.Text = IntToStr(attack);
                var damage = General.FSelectedUnit.GetDamage();
                Damage.Text = IntToStr(damage);
                var defense = General.FSelectedUnit.GetDefense();
                Defense.Text = IntToStr(defense);
                var resistance = General.FSelectedUnit.GetResistance();
                Resistance.Text = IntToStr(resistance);
                var moveTypes = General.FSelectedUnit.GetMoveTypes();
                var mTtoInt = General.MTtoInt(moveTypes);
                MovesIcon.CurrentImage = mTtoInt;
                AbLB.Items.Clear();
                General.BuildAbilities();
                for (int I = 0, last = General.FAbilities.Count - 1; I <= last; I++)
                {
                    A = (TTCAbility)General.FAbilities[I];
                    var name2 = AoWHSSet.FAbilityControl.GetAbility(A.Ability).GetName(General.FSelectedUnit);
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
        public void AbLBMouseMove(TObject Sender, TSet<TShiftState> Shift, int X, int Y)
        {
            if (General.FSelectedUnit != null && AbLB.F188 >= General.FActiveAbilityCount)
            {
                AbLB.F188 = -1;
            }
        }

        // AoWUnitBanner.pas:90
        public void BannerPopupMouseDown(int A1, byte A2, int A3, int A4, int A5)
        {
            if (A2 == 1)
            {
                AoWUnitBanner.UnitBanner.BannerPopup.Visible = false;
            }
        }

        // AoWUnitBanner.pas:95
        public void AbLBMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            AnsiString Reason = default;
            if (General.FSelectedUnit != null)
            {
                if (AbLB.Selected > -1 && AbLB.Selected < General.FActiveAbilityCount)
                {
                    var ability = General.FAbilities[AbLB.Selected];
                    AoWHSSet.FAbilityControl.GetAbility(((TTCAbility)ability).Ability).Activate(General.FSelectedUnit, ref Reason);
                    BannerPopup.Visible = false;
                }
                else
                {
                    AbLB.Selected = -1;
                }
            }
        }
    }

    public static partial class AoWUnitBanner
    {
        public static TUnitBanner UnitBanner;
    }
}
