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
            int I = default;
            TTCAbility A = default;
            if (General.FSelectedUnit != null)
            {
                var call__1 = General.FSelectedUnit.GetName();
                UnitNameLbl.Text = call__1;
                BannerPopup.FAligned = false;
                var call__2 = General.FSelectedUnit.GetHitPoints();
                var call__3 = General.FSelectedUnit.GetHits();
                Hits.Text = IntToStr(call__2) + AnsiString.FromBytes(47) + IntToStr(call__3);
                var call__4 = General.FSelectedUnit.GetMovePoints();
                var call__5 = General.FSelectedUnit.GetMoves();
                Moves.Text = IntToStr(call__4) + AnsiString.FromBytes(47) + IntToStr(call__5);
                var call__6 = General.FSelectedUnit.GetAttack();
                Attack.Text = IntToStr(call__6);
                var call__7 = General.FSelectedUnit.GetDamage();
                Damage.Text = IntToStr(call__7);
                var call__8 = General.FSelectedUnit.GetDefense();
                Defense.Text = IntToStr(call__8);
                var call__9 = General.FSelectedUnit.GetResistance();
                Resistance.Text = IntToStr(call__9);
                var call__10 = General.FSelectedUnit.GetMoveTypes();
                var call__11 = General.MTtoInt(call__10);
                MovesIcon.CurrentImage = call__11;
                AbLB.Items.Clear();
                General.BuildAbilities();
                {
                    I = 0;
                    int end__12 = General.FAbilities.Count - 1;
                    for (; I <= end__12; I += 1)
                    {
                        var call__13 = General.FAbilities[I];
                        A = (TTCAbility)call__13;
                        var call__14 = AoWHSSet.FAbilityControl.GetAbility(A.Ability);
                        var call__15 = call__14.GetName(General.FSelectedUnit);
                        AbLB.Items.Add(call__15);
                    }
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
                    var call__17 = General.FAbilities[AbLB.Selected];
                    var call__18 = AoWHSSet.FAbilityControl.GetAbility(((TTCAbility)call__17).Ability);
                    call__18.Activate(General.FSelectedUnit, ref Reason);
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
