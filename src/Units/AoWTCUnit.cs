// Translated from AoWTCUnit.pas.
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
using Aow.Units.AOWGauge;
using static Aow.Units.AOWGauge.AOWGauge;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.aowUserDrawBox;
using static Aow.Units.aowUserDrawBox.aowUserDrawBox;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.E1Resources;
using static Aow.Units.E1Resources.E1Resources;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWInfoBanner;
using static Aow.Units.AoWInfoBanner.AoWInfoBanner;

namespace Aow.Units.AoWTCUnit
{
    // { TTCUnit (instance size $154, VMT 00433494); components $44..$150. }
    public partial class TTCUnit : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow TCUnitWin;
        // { +0044 }
        public Aow.Units.AoWImage.TAOWImage SIcon;
        // { +0048 }
        public Aow.Units.AoWButton.TAOWButton Min;
        // { +004C }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +0050 }
        public Aow.Units.AoWLabel.TAOWLabel IWTitle;
        // { +0054 }
        public Aow.Units.AoWImage.TAOWImage UAttackIcon;
        // { +0058 }
        public Aow.Units.AoWImage.TAOWImage UDefenseIcon;
        // { +005C }
        public Aow.Units.AoWImage.TAOWImage UMovesIcon;
        // { +0060 }
        public Aow.Units.AoWImage.TAOWImage UDamageIcon;
        // { +0064 }
        public Aow.Units.AoWImage.TAOWImage UResistanceIcon;
        // { +0068 }
        public Aow.Units.AoWImage.TAOWImage UHitsIcon;
        // { +006C }
        public Aow.Units.AoWLabel.TAOWLabel UAttack;
        // { +0070 }
        public Aow.Units.AoWLabel.TAOWLabel UDefense;
        // { +0074 }
        public Aow.Units.AoWLabel.TAOWLabel UMoves;
        // { +0078 }
        public Aow.Units.AoWLabel.TAOWLabel UDamage;
        // { +007C }
        public Aow.Units.AoWLabel.TAOWLabel UResistance;
        // { +0080 }
        public Aow.Units.AoWLabel.TAOWLabel UHits;
        // { +0084 }
        public Aow.Units.AoWMemo.TAOWMemo UnitName;
        // { +0088 }
        public Aow.Units.AoWPanel.TAOWPanel UStatPnl;
        // { +008C }
        public Aow.Units.AoWListBox.TAOWListBox UAbilLB;
        // { +0090 }
        public Aow.Units.AoWLabel.TAOWLabel AbilNameLbl;
        // { +0094 }
        public Aow.Units.AoWPanel.TAOWPanel TCUnitPnl;
        // { +0098 }
        public Aow.Units.AoWImage.TAOWImage AOWImage1;
        // { +009C }
        public Aow.Units.AoWMemo.TAOWMemo AUnitName;
        // { +00A0 }
        public Aow.Units.AoWListBox.TAOWListBox USelAbilLb;
        // { +00A4 }
        public Aow.Units.AoWPanel.TAOWPanel TCAbilPnl;
        // { +00A8 }
        public Aow.Units.AoWImage.TAOWImage AbilAtt;
        // { +00AC }
        public Aow.Units.AoWLabel.TAOWLabel AbilAttLbl;
        // { +00B0 }
        public Aow.Units.AoWPanel.TAOWPanel AbilStatBox;
        // { +00B4 }
        public Aow.Units.AoWListBox.TAOWListBox AbilStat;
        // { +00B8 }
        public Aow.Units.AoWImage.TAOWImage AbilDam;
        // { +00BC }
        public Aow.Units.AoWLabel.TAOWLabel AbilDamLbl;
        // { +00C0 }
        public Aow.Units.AoWLabel.TAOWLabel TargetLbl;
        // { +00C4 }
        public Aow.Units.AoWLabel.TAOWLabel TargetULbl;
        // { +00C8 }
        public Aow.Units.AoWPanel.TAOWPanel TargetSelPnl;
        // { +00CC }
        public Aow.Units.AoWPanel.TAOWPanel NoTargetPnl;
        // { +00D0 }
        public Aow.Units.AoWMemo.TAOWMemo TargetMessage;
        // { +00D4 }
        public Aow.Units.AOWGauge.TAoWGauge UnitHits;
        // { +00D8 }
        public Aow.Units.AOWGauge.TAoWGauge UnitMana;
        // { +00DC }
        public Aow.Units.AoWImage.TAOWImage CelticCross;
        // { +00E0 }
        public Aow.Units.AoWPanel.TAOWPanel UStatP1;
        // { +00E4 }
        public Aow.Units.AoWPanel.TAOWPanel UStatP2;
        // { +00E8 }
        public Aow.Units.AoWImage.TAOWImage AAttackIcon;
        // { +00EC }
        public Aow.Units.AoWImage.TAOWImage ADefenseIcon;
        // { +00F0 }
        public Aow.Units.AoWImage.TAOWImage AMovesIcon;
        // { +00F4 }
        public Aow.Units.AoWImage.TAOWImage ADamageIcon;
        // { +00F8 }
        public Aow.Units.AoWImage.TAOWImage AResistanceIcon;
        // { +00FC }
        public Aow.Units.AoWImage.TAOWImage AHitsIcons;
        // { +0100 }
        public Aow.Units.AoWLabel.TAOWLabel AAttack;
        // { +0104 }
        public Aow.Units.AoWLabel.TAOWLabel ADefense;
        // { +0108 }
        public Aow.Units.AoWLabel.TAOWLabel AMoves;
        // { +010C }
        public Aow.Units.AoWLabel.TAOWLabel ADamage;
        // { +0110 }
        public Aow.Units.AoWLabel.TAOWLabel AResistance;
        // { +0114 }
        public Aow.Units.AoWLabel.TAOWLabel AHits;
        // { +0118 }
        public Aow.Units.AoWPanel.TAOWPanel AStatPnl;
        // { +011C }
        public Aow.Units.AoWPanel.TAOWPanel AStatP1;
        // { +0120 }
        public Aow.Units.AoWPanel.TAOWPanel AStatP2;
        // { +0124 }
        public Aow.Units.AoWButton.TAOWButton Center;
        // { +0128 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar AbilSB;
        // { +012C }
        public Aow.Units.AoWVScroll.TAOWVScrollBar UAbilSB;
        // { +0130 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar AOWVScrollBar1;
        // { +0134 }
        public Aow.Units.AoWPanel.TAOWPanel AbilFr;
        // { +0138 }
        public Aow.Units.AoWPanel.TAOWPanel ANameFr;
        // { +013C }
        public Aow.Units.AoWPanel.TAOWPanel TargerFr;
        // { +0140 }
        public Aow.Units.AoWPanel.TAOWPanel TNamePnl;
        // { +0144 }
        public Aow.Units.AoWPanel.TAOWPanel UnitFr;
        // { +0148 }
        public Aow.Units.aowUserDrawBox.TaowUserDrawBox UnitBox;
        // { +014C }
        public Aow.Rtl.TIvTranslator Translator;
        // AoWTCUnit.pas:106
        // { +0150 }
        public void TCUnitWinResize()
        {
            this.TCUnitWin.Alignment.RightOffset = (this.TCUnitWin.WinManager.WinWidth - this.TCUnitWin.WinLeft) - this.TCUnitWin.WinWidth;
            this.TCUnitWin.Alignment.BottomOffset = (this.TCUnitWin.WinManager.WinHeight - this.TCUnitWin.WinTop) - this.TCUnitWin.WinHeight;
        }

        // AoWTCUnit.pas:116
        public void MinClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            TCUnitWin.Visible = false;
        }

        // AoWTCUnit.pas:123
        public void UAbilLBPreDrawItem(Aow.Rtl.TObject Sender, in Aow.Rtl.TRect DrawRect, int DrawItem, ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit != null)
            {
                var call__2 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.GetPlayer();
                bool bool__3 = ((call__2 == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer) && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FMovePoints > 0)) && (DrawItem < Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FCombatAbilityCount);
                if (bool__3)
                {
                    var call__4 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FAbilities[DrawItem];
                    var call__5 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TTCAbility)call__4).Ability);
                    var call__6 = call__5.GetCombatMode(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit);
                    bool bool__7 = !TSet<Aow.Units.AoWE.TCombatMode>.Of().WithRange(Aow.Units.AoWE.TCombatMode.cmShortRange, Aow.Units.AoWE.TCombatMode.cmLongRange).Contains(call__6);
                    if (bool__7)
                    {
                        var call__8 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FAbilities[DrawItem];
                        bool__7 = (((Aow.Units.AoWTC.TTCAbility)call__8).Ability != 52);
                    }

                    bool bool__9 = bool__7;
                    if (!bool__9)
                    {
                        var call__10 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit.GetMoves();
                        bool__9 = (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FMovePoints == call__10);
                    }

                    bool__3 = bool__9;
                }

                if (bool__3)
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
        public void TCUnitPnlDrawBack(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit != null)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit.ShowEx(TCUnitPnl.WinLeft - 10, TCUnitPnl.WinTop - 2, SD, Aow.Units.AoWWinMan.AoWWinMan.General.FUnitShowInfo);
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
        public void UAbilLBMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.AoWE.TAbility Ability = default;
            if (UAbilLB.Selected > -1)
            {
                if (Button == Aow.Rtl.TMouseButton.mbLeft)
                {
                    var call__11 = UAbilLB.Items.Objects[UAbilLB.Selected];
                    var call__12 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.SelectAbility(Aow.Rtl.Rtl.ObjectToInt(call__11));
                    UAbilLB.Selected = call__12;
                }
                else
                {
                    if ((Button == Aow.Rtl.TMouseButton.mbRight) && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit != null))
                    {
                        var call__13 = UAbilLB.Items.Objects[UAbilLB.Selected];
                        var call__14 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__13));
                        Ability = call__14;
                        if (Ability != null)
                        {
                            var call__15 = Ability.GetName(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit);
                            Aow.Units.AoWExeForms.AoWExeForms.SetInfoBanner(Aow.Units.AoWInfoBanner.AoWInfoBanner.InfoBanner, call__15, Ability.FDescription);
                        }
                    }
                }
            }
        }

        // AoWTCUnit.pas:168
        public void UAbilLBMouseMove(Aow.Rtl.TObject Sender, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            // { Tried reload final movement operand through global; no exact match. }
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit != null)
            {
                var call__16 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.GetPlayer();
                if (((call__16 == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer) && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FMovePoints != 0)) && (UAbilLB.F188 < Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FCombatAbilityCount))
                {
                    var call__17 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FAbilities[UAbilLB.F188];
                    var call__18 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TTCAbility)call__17).Ability);
                    var call__19 = call__18.GetCombatMode(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit);
                    bool bool__20 = !TSet<Aow.Units.AoWE.TCombatMode>.Of().WithRange(Aow.Units.AoWE.TCombatMode.cmShortRange, Aow.Units.AoWE.TCombatMode.cmLongRange).Contains(call__19);
                    if (bool__20)
                    {
                        var call__21 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FAbilities[UAbilLB.F188];
                        bool__20 = (((Aow.Units.AoWTC.TTCAbility)call__21).Ability != 52);
                    }

                    if (bool__20)
                    {
                        return;
                    }

                    var tacticalCombatUnit = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit;
                    var call__23 = tacticalCombatUnit.FUnit.GetMoves();
                    if (call__23 <= tacticalCombatUnit.FMovePoints)
                    {
                        return;
                    }
                }

                UAbilLB.F188 = -1;
            }
        }

        // AoWTCUnit.pas:187
        public void HeroSMapMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit;
            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
        }

        // AoWTCUnit.pas:194
        public void CenterClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__24 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.GetLhx();
            var call__25 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.GetYhx();
            var call__26 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.GetXhx();
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(call__26, call__25, call__24);
            var call__27 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.GetLhx();
            var call__28 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.GetYhx();
            var call__29 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.GetXhx();
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlayCenterAnimation(unchecked((sbyte)call__29), unchecked((sbyte)call__28), unchecked((sbyte)call__27));
        }

        // AoWTCUnit.pas:195
        public void CloseClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            TCUnitWin.Visible = false;
        }
    }

    public static partial class AoWTCUnit
    {
        public static Aow.Units.AoWTCUnit.TTCUnit TCUnit;
        // AoWTCUnit.pas:197
        public static void RefreshCombatUnit(Aow.Units.AoWTCUnit.TTCUnit Window)
        {
            Aow.Units.AoWE.TAbilityCombatInfo AbilityInfo = default;
            Aow.Units.AoWE.TSpellCombatInfo SpellInfo = default;
            Aow.Units.AoWE.TAbstractUnit U = default;
            Aow.Units.AoWE.THero Hero = default;
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit != null)
            {
                U = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit;
                if (U is Aow.Units.AoWE.THero)
                {
                    Hero = (Aow.Units.AoWE.THero)U;
                }
                else
                {
                    Hero = null;
                }

                if (!Window.TCUnitWin.Visible)
                {
                    Window.UnitMana.Visible = false;
                    Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TCUnitBtn.Visible = true;
                }

                if (Hero != null)
                {
                    if (Hero.GetCastingPointsMax() > 0)
                    {
                        Window.UnitMana.Visible = true;
                        var call__30 = Hero.GetCastingPointsMax();
                        Window.UnitMana.Max = call__30;
                        var call__31 = Hero.GetCastingPoints();
                        Window.UnitMana.Position = call__31;
                    }

                    var panel = Window.UnitFr;
                    panel.Alignment.TopOffset = 7;
                    panel.WinHeight = 54;
                }
                else
                {
                    var panel2 = Window.UnitFr;
                    panel2.Alignment.TopOffset = 0;
                    panel2.WinHeight = 61;
                }

                Window.UnitFr.FAligned = false;
                if (U != null)
                {
                    if (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FSelectedAbility == -1)
                    {
                        Window.TCUnitPnl.Visible = true;
                        Window.TCAbilPnl.Visible = false;
                        Window.TargetSelPnl.Visible = false;
                        var call__34 = U.GetHitPoints();
                        var call__35 = U.GetHits();
                        Window.UHits.Text = (Aow.Rtl.Rtl.IntToStr(call__34) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__35);
                        var call__36 = U.GetMoves();
                        Window.UMoves.Text = (Aow.Rtl.Rtl.IntToStr(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FMovePoints) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__36);
                        var call__37 = U.GetAttack();
                        Window.UAttack.Text = Aow.Rtl.Rtl.IntToStr(call__37);
                        var call__38 = U.GetDamage();
                        Window.UDamage.Text = Aow.Rtl.Rtl.IntToStr(call__38);
                        var call__39 = U.GetDefense();
                        Window.UDefense.Text = Aow.Rtl.Rtl.IntToStr(call__39);
                        var call__40 = U.GetResistance();
                        Window.UResistance.Text = Aow.Rtl.Rtl.IntToStr(call__40);
                        var call__41 = U.GetMoveTypes();
                        var call__42 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__41);
                        Window.UMovesIcon.SetCurrent(call__42);
                        Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.ListAbilities(Window.UAbilLB.Items, null, null);
                        Window.UAbilLB.Selected = -1;
                        Window.UAbilLB.F188 = -1;
                        Window.UAbilLB.ListOffset = 0;
                    }
                    else
                    {
                        if ((Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FSelectedAbility == 52) && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FSelectedSpell > -1))
                        {
                            Window.TCUnitPnl.Visible = false;
                            Window.TCAbilPnl.Visible = true;
                            Window.TargetSelPnl.Visible = false;
                            var call__43 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FSelectedSpell);
                            call__43.GetCombatInfo(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit, ref SpellInfo);
                            var call__44 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FSelectedSpell);
                            Window.AbilNameLbl.Text = call__44.FSpellName;
                            if (SpellInfo.Attack > 0)
                            {
                                Window.AbilAttLbl.Text = Aow.Rtl.Rtl.IntToStr(SpellInfo.Attack);
                            }
                            else
                            {
                                Window.AbilAttLbl.Text = AnsiString.FromBytes(45);
                            }

                            if (SpellInfo.Damage > 0)
                            {
                                Window.AbilDamLbl.Text = Aow.Rtl.Rtl.IntToStr(SpellInfo.Damage);
                            }
                            else
                            {
                                Window.AbilDamLbl.Text = AnsiString.FromBytes(45);
                            }

                            Window.AbilStat.Items.Clear();
                            if (SpellInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtFire))
                            {
                                var call__45 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FireRStr);
                                var call__46 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__45 + AnsiString.FromBytes(32)) + call__46);
                            }

                            if (SpellInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtCold))
                            {
                                var call__47 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ColdRStr);
                                var call__48 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__47 + AnsiString.FromBytes(32)) + call__48);
                            }

                            if (SpellInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtLightning))
                            {
                                var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LightningRStr);
                                var call__50 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__49 + AnsiString.FromBytes(32)) + call__50);
                            }

                            if (SpellInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtMagic))
                            {
                                var call__51 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MagicRStr);
                                var call__52 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__51 + AnsiString.FromBytes(32)) + call__52);
                            }

                            if (SpellInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtPoison))
                            {
                                var call__53 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PoisonRStr);
                                var call__54 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__53 + AnsiString.FromBytes(32)) + call__54);
                            }

                            if (SpellInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtDeath))
                            {
                                var call__55 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DeathRStr);
                                var call__56 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__55 + AnsiString.FromBytes(32)) + call__56);
                            }

                            if (SpellInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtHoly))
                            {
                                var call__57 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HolyRStr);
                                var call__58 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__57 + AnsiString.FromBytes(32)) + call__58);
                            }

                            if (SpellInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtPhysical))
                            {
                                var call__59 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PhysicalRStr);
                                var call__60 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__59 + AnsiString.FromBytes(32)) + call__60);
                            }

                            if (SpellInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtWall))
                            {
                                var call__61 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WallRStr);
                                var call__62 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__61 + AnsiString.FromBytes(32)) + call__62);
                            }
                        }
                        else
                        {
                            Window.TCUnitPnl.Visible = false;
                            Window.TCAbilPnl.Visible = true;
                            Window.TargetSelPnl.Visible = false;
                            var call__63 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FSelectedAbility);
                            call__63.GetCombatInfo(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit, ref AbilityInfo);
                            var call__64 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FSelectedAbility);
                            var call__65 = call__64.GetName(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit);
                            Window.AbilNameLbl.Text = call__65;
                            if (AbilityInfo.Attack > 0)
                            {
                                Window.AbilAttLbl.Text = Aow.Rtl.Rtl.IntToStr(AbilityInfo.Attack);
                            }
                            else
                            {
                                Window.AbilAttLbl.Text = AnsiString.FromBytes(45);
                            }

                            if (AbilityInfo.Damage > 0)
                            {
                                Window.AbilDamLbl.Text = Aow.Rtl.Rtl.IntToStr(AbilityInfo.Damage);
                            }
                            else
                            {
                                Window.AbilDamLbl.Text = AnsiString.FromBytes(45);
                            }

                            Window.AbilStat.Items.Clear();
                            if (AbilityInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtFire))
                            {
                                var call__66 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FireRStr);
                                var call__67 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__66 + AnsiString.FromBytes(32)) + call__67);
                            }

                            if (AbilityInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtCold))
                            {
                                var call__68 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ColdRStr);
                                var call__69 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__68 + AnsiString.FromBytes(32)) + call__69);
                            }

                            if (AbilityInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtLightning))
                            {
                                var call__70 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LightningRStr);
                                var call__71 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__70 + AnsiString.FromBytes(32)) + call__71);
                            }

                            if (AbilityInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtMagic))
                            {
                                var call__72 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MagicRStr);
                                var call__73 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__72 + AnsiString.FromBytes(32)) + call__73);
                            }

                            if (AbilityInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtPoison))
                            {
                                var call__74 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PoisonRStr);
                                var call__75 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__74 + AnsiString.FromBytes(32)) + call__75);
                            }

                            if (AbilityInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtDeath))
                            {
                                var call__76 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DeathRStr);
                                var call__77 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__76 + AnsiString.FromBytes(32)) + call__77);
                            }

                            if (AbilityInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtHoly))
                            {
                                var call__78 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HolyRStr);
                                var call__79 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__78 + AnsiString.FromBytes(32)) + call__79);
                            }

                            if (AbilityInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtPhysical))
                            {
                                var call__80 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PhysicalRStr);
                                var call__81 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__80 + AnsiString.FromBytes(32)) + call__81);
                            }

                            if (AbilityInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtWall))
                            {
                                var call__82 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WallRStr);
                                var call__83 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1DamageRStr);
                                Window.AbilStat.Items.Add((call__82 + AnsiString.FromBytes(32)) + call__83);
                            }
                        }
                    }

                    Window.UnitName.Items.Clear();
                    var call__84 = U.GetName();
                    Window.UnitName.Items.Add(call__84);
                    var call__85 = U.GetHits();
                    Window.UnitHits.Max = call__85;
                    var call__86 = U.GetHitPoints();
                    Window.UnitHits.Position = call__86;
                }
            }

            Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Visible = true;
        }
    }
}
