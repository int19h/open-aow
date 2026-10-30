// Translated from AoWUnitBanner.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AOWMenu;
using static Aow.Units.AOWMenu.AOWMenu;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWUnitBanner
{
    // { TUnitBanner (instance size $84, VMT 00403D84).
//     $44..80 published labels, popup and ability-list controls. }
    public partial class TUnitBanner : Aow.Rtl.TDataModule
    {
        public TUnitBanner(Aow.Rtl.TComponent AOwner) : base(AOwner) { }

        public Aow.Units.AoWLabel.TAOWLabel UnitNameLbl;
        // { +0044 }
        public Aow.Units.AoWImage.TAOWImage AttackIcon;
        // { +0048 }
        public Aow.Units.AoWImage.TAOWImage DefenseIcon;
        // { +004C }
        public Aow.Units.AoWImage.TAOWImage MovesIcon;
        // { +0050 }
        public Aow.Units.AoWImage.TAOWImage DamageIcon;
        // { +0054 }
        public Aow.Units.AoWImage.TAOWImage ResistanceIcon;
        // { +0058 }
        public Aow.Units.AoWImage.TAOWImage HitsIcon;
        // { +005C }
        public Aow.Units.AoWLabel.TAOWLabel Attack;
        // { +0060 }
        public Aow.Units.AoWLabel.TAOWLabel Defense;
        // { +0064 }
        public Aow.Units.AoWLabel.TAOWLabel Moves;
        // { +0068 }
        public Aow.Units.AoWLabel.TAOWLabel Damage;
        // { +006C }
        public Aow.Units.AoWLabel.TAOWLabel Resistance;
        // { +0070 }
        public Aow.Units.AoWLabel.TAOWLabel Hits;
        // { +0074 }
        public Aow.Units.AOWMenu.TAOWMenu BannerPopup;
        // { +0078 }
        public Aow.Units.AoWListBox.TAOWListBox AbLB;
        // { +007C }
        public Aow.Units.AoWVScroll.TAOWVScrollBar AbVSB;
        // AoWUnitBanner.pas:53
        // { +0080 }
        public void BannerPopupShow(Aow.Rtl.TObject Sender)
        {
            int I = default;
            Aow.Units.AoWTC.TTCAbility A = default;
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit != null)
            {
                var call__1 = Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.GetName();
                UnitNameLbl.Text = call__1;
                BannerPopup.FAligned = false;
                var call__2 = Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.GetHitPoints();
                var call__3 = Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.GetHits();
                Hits.Text = (Aow.Rtl.Rtl.IntToStr(call__2) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__3);
                var call__4 = Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.GetMovePoints();
                var call__5 = Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.GetMoves();
                Moves.Text = (Aow.Rtl.Rtl.IntToStr(call__4) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__5);
                var call__6 = Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.GetAttack();
                Attack.Text = Aow.Rtl.Rtl.IntToStr(call__6);
                var call__7 = Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.GetDamage();
                Damage.Text = Aow.Rtl.Rtl.IntToStr(call__7);
                var call__8 = Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.GetDefense();
                Defense.Text = Aow.Rtl.Rtl.IntToStr(call__8);
                var call__9 = Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.GetResistance();
                Resistance.Text = Aow.Rtl.Rtl.IntToStr(call__9);
                var call__10 = Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.GetMoveTypes();
                var call__11 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__10);
                MovesIcon.CurrentImage = call__11;
                AbLB.Items.Clear();
                Aow.Units.AoWWinMan.AoWWinMan.General.BuildAbilities();
                {
                    I = 0;
                    int end__12 = Aow.Units.AoWWinMan.AoWWinMan.General.FAbilities.Count - 1;
                    for (; I <= end__12; I += 1)
                    {
                        var call__13 = Aow.Units.AoWWinMan.AoWWinMan.General.FAbilities[I];
                        A = (Aow.Units.AoWTC.TTCAbility)call__13;
                        var call__14 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(A.Ability);
                        var call__15 = call__14.GetName(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                        AbLB.Items.Add(call__15);
                    }
                }

                AbLB.Selected = -1;
                this.AbLB.F188 = -1;
                this.AbLB.ListOffset = 0;
            }
        }

        // AoWUnitBanner.pas:80
        // { draft }
        public void AbLBUpdate()
        {
            if (AbLB.F188 != AbLB.Selected)
            {
                AbLB.Selected = AbLB.F188;
            }
        }

        // AoWUnitBanner.pas:85
        // { draft }
        public void AbLBMouseMove(Aow.Rtl.TObject Sender, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            // { Tried with/local receiver; no exact match. }
            if ((Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit != null) && (AbLB.F188 >= Aow.Units.AoWWinMan.AoWWinMan.General.FActiveAbilityCount))
            {
                AbLB.F188 = -1;
            }
        }

        // AoWUnitBanner.pas:90
        public void BannerPopupMouseDown(int A1, byte A2, int A3, int A4, int A5)
        {
            if (A2 == 1)
            {
                Aow.Units.AoWUnitBanner.AoWUnitBanner.UnitBanner.BannerPopup.Visible = false;
            }
        }

        // AoWUnitBanner.pas:95
        // { draft }
        public void AbLBMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Rtl.AnsiString Reason = default;
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit != null)
            {
                if ((AbLB.Selected > -1) && (AbLB.Selected < Aow.Units.AoWWinMan.AoWWinMan.General.FActiveAbilityCount))
                {
                    var call__17 = Aow.Units.AoWWinMan.AoWWinMan.General.FAbilities[AbLB.Selected];
                    var call__18 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TTCAbility)call__17).Ability);
                    call__18.Activate(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit, ref Reason);
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
        public static Aow.Units.AoWUnitBanner.TUnitBanner UnitBanner;
    }
}
