// Generated from AoWUnitWindow.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
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
using Aow.Units.AoWTabPanel;
using static Aow.Units.AoWTabPanel.AoWTabPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.E2Globals;
using static Aow.Units.E2Globals.E2Globals;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWInfoBanner;
using static Aow.Units.AoWInfoBanner.AoWInfoBanner;

namespace Aow.Units.AoWUnitWindow
{
    // { TUnitWindow (instance size $248, VMT 0040710C).
//     $21C highlight; $220 dragged item type; $224 highlight step; $228 blink state;
//     $22C blink delay; $230 cursor image; $234/$238 ground offset/count; $23C show info. }
    public partial class TUnitWindow : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow UnitWin;
        // { +0044 }
        public Aow.Units.AoWImage.TAOWImage UnitFrameLeft;
        // { +0048 }
        public Aow.Units.AoWTabPanel.TAOWTabPanel UnitTab;
        // { +004C }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +0050 }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +0054 }
        public Aow.Units.AoWButton.TAOWButton Help;
        // { +0058 }
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +005C }
        public Aow.Units.AoWButton.TAOWButton BCloseBtn;
        // { +0060 }
        public Aow.Units.AoWImage.TAOWImage CeltLineR;
        // { +0064 }
        public Aow.Units.AoWMemo.TAOWMemo T2Memo;
        // { +0068 }
        public Aow.Units.AoWImage.TAOWImage ManaIcon;
        // { +006C }
        public Aow.Units.AoWLabel.TAOWLabel Mana;
        // { +0070 }
        public Aow.Units.AoWListBox.TAOWListBox T1AbilityLB;
        // { +0074 }
        public Aow.Units.AoWButton.TAOWButton T1UseBut;
        // { +0078 }
        public Aow.Units.AoWButton.TAOWButton T1SortName;
        // { +007C }
        public Aow.Units.AoWButton.TAOWButton T1SortType;
        // { +0080 }
        public Aow.Units.AoWButton.TAOWButton T1SortSource;
        // { +0084 }
        public Aow.Units.AoWLabel.TAOWLabel T1UseMsg;
        // { +0088 }
        public Aow.Units.AoWPanel.TAOWPanel T1Panel;
        // { +008C }
        public Aow.Units.AoWPanel.TAOWPanel T2Panel;
        // { +0090 }
        public Aow.Units.AoWListBox.TAOWListBox T1TypeLB;
        // { +0094 }
        public Aow.Units.AoWListBox.TAOWListBox T1SrcLB;
        // { +0098 }
        public Aow.Units.AoWPanel.TAOWPanel T1ListPnl;
        // { +009C }
        public Aow.Units.AoWVScroll.TAOWVScrollBar T1SBar;
        // { +00A0 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar T2SBar;
        // { +00A4 }
        public Aow.Units.AoWPanel.TAOWPanel T2Text;
        // { +00A8 }
        public Aow.Units.AoWPanel.TAOWPanel T2Leader;
        // { +00AC }
        public Aow.Units.AoWButton.TAOWButton Sel4;
        // { +00B0 }
        public Aow.Units.AoWPanel.TAOWPanel SelPnl;
        // { +00B4 }
        public Aow.Units.AoWButton.TAOWButton Sel3;
        // { +00B8 }
        public Aow.Units.AoWButton.TAOWButton Sel2;
        // { +00BC }
        public Aow.Units.AoWButton.TAOWButton Sel1;
        // { +00C0 }
        public Aow.Units.AoWButton.TAOWButton Sel5;
        // { +00C4 }
        public Aow.Units.AoWButton.TAOWButton Sel6;
        // { +00C8 }
        public Aow.Units.AoWButton.TAOWButton Sel7;
        // { +00CC }
        public Aow.Units.AoWImage.TAOWImage AAlignment;
        // { +00D0 }
        public Aow.Units.AoWLabel.TAOWLabel AAlignmentLbl;
        // { +00D4 }
        public Aow.Units.AoWPanel.TAOWPanel T2Alignnl;
        // { +00D8 }
        public Aow.Units.AoWFrame.TAoWFrame AlFr;
        // { +00DC }
        public Aow.Units.AoWImage.TAOWImage TypeIcn;
        // { +00E0 }
        public Aow.Units.AoWLabel.TAOWLabel TypeV;
        // { +00E4 }
        public Aow.Units.AoWPanel.TAOWPanel T2TypePnl;
        // { +00E8 }
        public Aow.Units.AoWFrame.TAoWFrame TypeFr;
        // { +00EC }
        public Aow.Units.AoWFrame.TAoWFrame LdrFr;
        // { +00F0 }
        public Aow.Units.AoWLabel.TAOWLabel SphereLbl;
        // { +00F4 }
        public Aow.Units.AoWPanel.TAOWPanel T3Panel;
        // { +00F8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox BP1;
        // { +00FC }
        public Aow.Units.AoWLabel.TAOWLabel BPLbl1;
        // { +0100 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox BP2;
        // { +0104 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox BP3;
        // { +0108 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox BP4;
        // { +010C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox BP5;
        // { +0110 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox BP6;
        // { +0114 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox BP7;
        // { +0118 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox BP8;
        // { +011C }
        public Aow.Units.AoWLabel.TAOWLabel BPLbl2;
        // { +0120 }
        public Aow.Units.AoWLabel.TAOWLabel BodyLbl;
        // { +0124 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Head;
        // { +0128 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Torso;
        // { +012C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Attack;
        // { +0130 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Defend;
        // { +0134 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox RingL;
        // { +0138 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox RingR;
        // { +013C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Gnd3;
        // { +0140 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Gnd2;
        // { +0144 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Gnd1;
        // { +0148 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Gnd4;
        // { +014C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Gnd5;
        // { +0150 }
        public Aow.Units.AoWLabel.TAOWLabel GndLbl;
        // { +0154 }
        public Aow.Units.AoWImage.TAOWImage SlotBack;
        // { +0158 }
        public Aow.Units.AoWButton.TAOWButton DisbandBtn;
        // { +015C }
        public Aow.Units.AoWPanel.TAOWPanel ItemUsePnl;
        // { +0160 }
        public Aow.Units.AoWButton.TAOWButton ItemUseBtn;
        // { +0164 }
        public Aow.Units.AoWImage.TAOWImage ItemPic;
        // { +0168 }
        public Aow.Units.AoWPanel.TAOWPanel UnitMainPnl;
        // { +016C }
        public Aow.Units.AoWLabel.TAOWLabel UnitName;
        // { +0170 }
        public Aow.Units.AoWImage.TAOWImage UnitFrRight;
        // { +0174 }
        public Aow.Units.AoWImage.TAOWImage UAttackIcon;
        // { +0178 }
        public Aow.Units.AoWImage.TAOWImage UDefenseIcon;
        // { +017C }
        public Aow.Units.AoWImage.TAOWImage UMovesIcon;
        // { +0180 }
        public Aow.Units.AoWImage.TAOWImage UDamageIcon;
        // { +0184 }
        public Aow.Units.AoWImage.TAOWImage UResistanceIcon;
        // { +0188 }
        public Aow.Units.AoWImage.TAOWImage UHitsIcon;
        // { +018C }
        public Aow.Units.AoWLabel.TAOWLabel UAttack;
        // { +0190 }
        public Aow.Units.AoWLabel.TAOWLabel UDefense;
        // { +0194 }
        public Aow.Units.AoWLabel.TAOWLabel UMoves;
        // { +0198 }
        public Aow.Units.AoWLabel.TAOWLabel UDamage;
        // { +019C }
        public Aow.Units.AoWLabel.TAOWLabel UResistance;
        // { +01A0 }
        public Aow.Units.AoWLabel.TAOWLabel UHits;
        // { +01A4 }
        public Aow.Units.AoWPanel.TAOWPanel UStatPnl;
        // { +01A8 }
        public Aow.Units.AoWPanel.TAOWPanel UStatP1;
        // { +01AC }
        public Aow.Units.AoWPanel.TAOWPanel UStatP2;
        // { +01B0 }
        public Aow.Units.AoWPanel.TAOWPanel UnitFr;
        // { +01B4 }
        public Aow.Units.AoWLabel.TAOWLabel Level;
        // { +01B8 }
        public Aow.Units.AoWImage.TAOWImage UnitLine1;
        // { +01BC }
        public Aow.Units.AoWImage.TAOWImage UnitLine2;
        // { +01C0 }
        public Aow.Units.AoWImage.TAOWImage FrLSh;
        // { +01C4 }
        public Aow.Units.AoWImage.TAOWImage FrRSh;
        // { +01C8 }
        public Aow.Units.AoWPanel.TAOWPanel RightFramePnl;
        // { +01CC }
        public Aow.Units.AoWLabel.TAOWLabel MoraleLbl;
        // { +01D0 }
        public Aow.Units.AoWImage.TAOWImage UpkeepIcon;
        // { +01D4 }
        public Aow.Units.AoWLabel.TAOWLabel UpKeep;
        // { +01D8 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +01DC }
        public Aow.Units.AoWImage.TAOWImage CeltLineL;
        // { +01E0 }
        public Aow.Units.AoWButton.TAOWButton GroundL;
        // { +01E4 }
        public Aow.Units.AoWButton.TAOWButton GroundR;
        // { +01E8 }
        public Aow.Units.AoWImage.TAOWImage RelationIcn;
        // { +01EC }
        public Aow.Units.AoWLabel.TAOWLabel RelationV;
        // { +01F0 }
        public Aow.Units.AoWPanel.TAOWPanel T2RelPnl;
        // { +01F4 }
        public Aow.Units.AoWFrame.TAoWFrame RelFr;
        // { +01F8 }
        public Aow.Units.AoWImage.TAOWImage RelRing;
        // { +01FC }
        public Aow.Units.AoWLabel.TAOWLabel RelationLbl;
        // { +0200 }
        public Aow.Units.AoWLabel.TAOWLabel AlignLbl;
        // { +0204 }
        public Aow.Units.AoWLabel.TAOWLabel TypeLbl;
        // { +0208 }
        public Aow.Units.AoWLabel.TAOWLabel XP;
        // { +020C }
        public Aow.Units.AoWButton.TAOWButton PNext;
        // { +0210 }
        public Aow.Units.AoWButton.TAOWButton PPrev;
        // { +0214 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0218 }
        public int FHighlight;
        // { $21C }
        public Aow.Units.AoWE.TItemType FItemType;
        // { $220 }
        public Aow.Units.FixedArray_byte_3 F_UnitWindow_221;
        public int FHighlightStep;
        // { $224 }
        public bool FItemVisible;
        // { $228 }
        public Aow.Units.FixedArray_byte_3 F_UnitWindow_229;
        public int FBlinkDelay;
        // { $22C }
        public Aow.Units.ImageLib.TLibraryImage FCursorImage;
        // { $230 }
        public int FGroundOffset;
        // { $234 }
        public int FGroundCount;
        // { $238 }
        public Aow.Units.AoWE.TP2UnitShowInfo FShowInfo;
        // AoWUnitWindow.pas:235
        // { $23C }
        public void ShowUnit()
        {
            Aow.Units.AoWE.THero H = default;
            Aow.Units.AoWE.TAbstractUnit U = default;
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit != null)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.BuildAbilities();
                U = Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit;
                if ((U != null) && (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == U.FPlayer))
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(64, true, true);
                }

                if (U is Aow.Units.AoWE.THero)
                {
                    H = (Aow.Units.AoWE.THero)U;
                }
                else
                {
                    H = null;
                }

                bool bool__1 = (Aow.Units.AoWTC.AoWTC.AoWCombatMap == null) && (Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList != null);
                if (bool__1)
                {
                    var call__2 = Aow.Units.AoWWinMan.AoWWinMan.General.IndexOfUnit(U);
                    bool__1 = (call__2 > -1);
                }

                if (bool__1 && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[1] > -1))
                {
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.PPrev.Visible = true;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.PNext.Visible = true;
                }
                else
                {
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.PPrev.Visible = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.PNext.Visible = false;
                }

                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Mana.Visible = false;
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ManaIcon.Visible = false;
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Items.Clear();
                var call__3 = Aow.Units.AoWUnitWindow.AoWUnitWindow.AbilitiesRStr;
                var call__4 = Aow.Units.AoWE.AoWE.TranslateRStr(call__3);
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Items.Add(call__4);
                var call__5 = Aow.Units.AoWUnitWindow.AoWUnitWindow.InfoRStr;
                var call__6 = Aow.Units.AoWE.AoWE.TranslateRStr(call__5);
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Items.Add(call__6);
                if (U.GetRace() > -1)
                {
                    var call__7 = U.GetRace();
                    var call__8 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesRStr[call__7]);
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text = call__8;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RelationIcn.Visible = true;
                    var call__9 = U.GetRace();
                    var call__10 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(call__9);
                    var call__11 = call__10.GetPlayerRelation(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RelationIcn.CurrentImage = ((int)call__11) + 11;
                    var call__12 = U.GetRace();
                    var call__13 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(call__12);
                    var call__14 = call__13.GetPlayerRelation(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    var call__15 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TStatusRStr[(int)call__14]);
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RelationV.Text = call__15;
                }
                else
                {
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text = (AnsiString)"";
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RelationIcn.Visible = false;
                    var call__16 = Aow.Units.AoWUnitWindow.AoWUnitWindow.NoneRStr;
                    var call__17 = Aow.Units.AoWE.AoWE.TranslateRStr(call__16);
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RelationV.Text = call__17;
                }

                if (((Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == U.FPlayer) && !(U is Aow.Units.AoWE.TLeader)) && (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null))
                {
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.DisbandBtn.Visible = true;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BCloseBtn.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
                }
                else
                {
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.DisbandBtn.Visible = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BCloseBtn.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter;
                }

                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BCloseBtn.FAligned = false;
                switch (U.GetUnitType())
                {
                    case Aow.Units.AoWE.TUnitType.utCreature:
                        var call__18 = Aow.Units.AoWUnitWindow.AoWUnitWindow.CreatureRStr;
                        var call__19 = Aow.Units.AoWE.AoWE.TranslateRStr(call__18);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text = (Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32)) + call__19;
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeIcn.CurrentImage = 21;
                        break;
                    case Aow.Units.AoWE.TUnitType.utMachine:
                        var call__20 = Aow.Units.AoWUnitWindow.AoWUnitWindow.MachineRStr;
                        var call__21 = Aow.Units.AoWE.AoWE.TranslateRStr(call__20);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text = (Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32)) + call__21;
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeIcn.CurrentImage = 22;
                        break;
                    default:
                        if (H == null)
                        {
                            var call__22 = Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitRStr;
                            var call__23 = Aow.Units.AoWE.AoWE.TranslateRStr(call__22);
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text = (Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32)) + call__23;
                        }
                        else
                        {
                            if (H is Aow.Units.AoWE.TLeader)
                            {
                                var call__24 = Aow.Units.AoWUnitWindow.AoWUnitWindow.LeaderRStr;
                                var call__25 = Aow.Units.AoWE.AoWE.TranslateRStr(call__24);
                                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text = (Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32)) + call__25;
                            }
                            else
                            {
                                var call__26 = Aow.Units.AoWUnitWindow.AoWUnitWindow.HeroRStr;
                                var call__27 = Aow.Units.AoWE.AoWE.TranslateRStr(call__26);
                                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text = (Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeV.Text + AnsiString.FromBytes(32)) + call__27;
                            }
                        }

                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.TypeIcn.CurrentImage = 23;
                        break;
                }

                if (H == null)
                {
                    var call__28 = Aow.Units.AoWUnitWindow.AoWUnitWindow.LevelRStr;
                    var call__29 = Aow.Units.AoWE.AoWE.TranslateRStr(call__28);
                    var call__30 = U.GetUnitLevel();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Level.Text = (call__29 + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(call__30);
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T2Leader.Visible = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T2Text.Visible = true;
                    var call__31 = U.GetDescription();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T2Memo.Items.Assign(call__31);
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T2SBar.Position = 0;
                    var call__32 = U.GetName();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitName.Text = call__32;
                }
                else
                {
                    if (H.FPlayer == 0)
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.SelPnl.Visible = false;
                        var call__33 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWUnitWindow.AoWUnitWindow.NoLeaderRStr);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.SphereLbl.Text = call__33;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.SelPnl.Visible = true;
                        var call__34 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__35 = call__34.FLeader.GetSphereCount();
                        if (call__35 > 0)
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel1.Visible = true;
                            var call__36 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__37 = call__36.FLeader.GetSpheres(0);
                            var call__38 = Aow.Units.AoWWinMan.AoWWinMan.General.SphereToImage(call__37);
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel1.ILIndexUp = call__38;
                        }
                        else
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel1.Visible = false;
                        }

                        var call__39 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__40 = call__39.FLeader.GetSphereCount();
                        if (call__40 > 1)
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel2.Visible = true;
                            var call__41 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__42 = call__41.FLeader.GetSpheres(1);
                            var call__43 = Aow.Units.AoWWinMan.AoWWinMan.General.SphereToImage(call__42);
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel2.ILIndexUp = call__43;
                        }
                        else
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel2.Visible = false;
                        }

                        var call__44 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__45 = call__44.FLeader.GetSphereCount();
                        if (call__45 > 2)
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel3.Visible = true;
                            var call__46 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__47 = call__46.FLeader.GetSpheres(2);
                            var call__48 = Aow.Units.AoWWinMan.AoWWinMan.General.SphereToImage(call__47);
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel3.ILIndexUp = call__48;
                        }
                        else
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel3.Visible = false;
                        }

                        var call__49 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__50 = call__49.FLeader.GetSphereCount();
                        if (call__50 > 3)
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel4.Visible = true;
                            var call__51 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__52 = call__51.FLeader.GetSpheres(3);
                            var call__53 = Aow.Units.AoWWinMan.AoWWinMan.General.SphereToImage(call__52);
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel4.ILIndexUp = call__53;
                        }
                        else
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel4.Visible = false;
                        }

                        var call__54 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__55 = call__54.FLeader.GetSphereCount();
                        if (call__55 > 4)
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel5.Visible = true;
                            var call__56 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__57 = call__56.FLeader.GetSpheres(4);
                            var call__58 = Aow.Units.AoWWinMan.AoWWinMan.General.SphereToImage(call__57);
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel5.ILIndexUp = call__58;
                        }
                        else
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel5.Visible = false;
                        }

                        var call__59 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__60 = call__59.FLeader.GetSphereCount();
                        if (call__60 > 5)
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel6.Visible = true;
                            var call__61 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__62 = call__61.FLeader.GetSpheres(5);
                            var call__63 = Aow.Units.AoWWinMan.AoWWinMan.General.SphereToImage(call__62);
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel6.ILIndexUp = call__63;
                        }
                        else
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel6.Visible = false;
                        }

                        var call__64 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                        var call__65 = call__64.FLeader.GetSphereCount();
                        if (call__65 > 6)
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel7.Visible = true;
                            var call__66 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(H.FPlayer);
                            var call__67 = call__66.FLeader.GetSpheres(6);
                            var call__68 = Aow.Units.AoWWinMan.AoWWinMan.General.SphereToImage(call__67);
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel7.ILIndexUp = call__68;
                        }
                        else
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Sel7.Visible = false;
                        }

                        var call__69 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWUnitWindow.AoWUnitWindow.LeaderSpheresRStr);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.SphereLbl.Text = call__69;
                    }

                    bool bool__70 = ((Aow.Units.AoWE.AoWE.AoWHSMap.FCombat == null) && (H.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    if (!bool__70)
                    {
                        bool bool__71 = (Aow.Units.AoWE.AoWE.AoWHSMap.FCombat != null);
                        if (bool__71)
                        {
                            var call__72 = Aow.Units.AoWE.AoWE.AoWHSMap.FCombat.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FCombat.FCombatData.FCurrentPlayer));
                            bool__71 = (call__72.FPlayer == H.FPlayer);
                        }

                        bool__70 = bool__71;
                    }

                    if (bool__70)
                    {
                        var call__73 = Aow.Units.AoWUnitWindow.AoWUnitWindow.ItemsRStr;
                        var call__74 = Aow.Units.AoWE.AoWE.TranslateRStr(call__73);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Items.Add(call__74);
                    }

                    if (H.GetCastingPointsMax() > 0)
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Mana.Visible = true;
                        var call__75 = H.GetCastingPoints();
                        var call__76 = H.GetCastingPointsMax();
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Mana.Text = (Aow.Rtl.Rtl.IntToStr(call__75) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__76);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ManaIcon.Visible = true;
                    }

                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ItemUseBtn.Visible = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.FGroundOffset = 0;
                    var call__77 = Aow.Units.AoWUnitWindow.AoWUnitWindow.LevelRStr;
                    var call__78 = Aow.Units.AoWE.AoWE.TranslateRStr(call__77);
                    var call__79 = H.GetLevel();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Level.Text = (call__78 + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(call__79);
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T2Leader.Visible = true;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T2Text.Visible = false;
                    var call__80 = H.GetNickname();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitName.Text = (H.FHeroName + AnsiString.FromBytes(32)) + call__80;
                }

                var call__81 = Aow.Units.AoWUnitWindow.AoWUnitWindow.ExpRStr;
                var call__82 = Aow.Units.AoWE.AoWE.TranslateRStr(call__81);
                var call__83 = U.GetExperience();
                var call__84 = U.GetNextLevelExperience();
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.XP.Text = ((((call__82 + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(call__83)) + ((AnsiString)" (")) + Aow.Rtl.Rtl.IntToStr(call__84)) + AnsiString.FromBytes(41);
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Cursor = 0;
                var call__85 = Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Items.Count;
                if ((call__85 <= Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Selected) || (Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Selected < 0))
                {
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Selected = 0;
                }

                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = true;
                bool bool__86 = (H != null);
                if (bool__86)
                {
                    bool bool__87 = ((Aow.Units.AoWE.AoWE.AoWHSMap.FCombat == null) && (H.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    if (!bool__87)
                    {
                        bool bool__88 = (Aow.Units.AoWE.AoWE.AoWHSMap.FCombat != null);
                        if (bool__88)
                        {
                            var call__89 = Aow.Units.AoWE.AoWE.AoWHSMap.FCombat.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FCombat.FCombatData.FCurrentPlayer));
                            bool__88 = (call__89.FPlayer == H.FPlayer);
                        }

                        bool__87 = bool__88;
                    }

                    bool__86 = bool__87;
                }

                if (bool__86)
                {
                    var call__90 = H.FInventory.GetItem(0);
                    if (call__90 != null)
                    {
                        var call__91 = H.FInventory.GetItem(0);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP1.Tag = call__91.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP1.Tag = -1;
                    }

                    var call__92 = H.FInventory.GetItem(1);
                    if (call__92 != null)
                    {
                        var call__93 = H.FInventory.GetItem(1);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP2.Tag = call__93.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP2.Tag = -1;
                    }

                    var call__94 = H.FInventory.GetItem(2);
                    if (call__94 != null)
                    {
                        var call__95 = H.FInventory.GetItem(2);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP3.Tag = call__95.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP3.Tag = -1;
                    }

                    var call__96 = H.FInventory.GetItem(3);
                    if (call__96 != null)
                    {
                        var call__97 = H.FInventory.GetItem(3);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP4.Tag = call__97.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP4.Tag = -1;
                    }

                    var call__98 = H.FInventory.GetItem(4);
                    if (call__98 != null)
                    {
                        var call__99 = H.FInventory.GetItem(4);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP5.Tag = call__99.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP5.Tag = -1;
                    }

                    var call__100 = H.FInventory.GetItem(5);
                    if (call__100 != null)
                    {
                        var call__101 = H.FInventory.GetItem(5);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP6.Tag = call__101.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP6.Tag = -1;
                    }

                    var call__102 = H.FInventory.GetItem(6);
                    if (call__102 != null)
                    {
                        var call__103 = H.FInventory.GetItem(6);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP7.Tag = call__103.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP7.Tag = -1;
                    }

                    var call__104 = H.FInventory.GetItem(7);
                    if (call__104 != null)
                    {
                        var call__105 = H.FInventory.GetItem(7);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP8.Tag = call__105.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP8.Tag = -1;
                    }

                    var call__106 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipAttackSlot);
                    if (call__106 != null)
                    {
                        var call__107 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipAttackSlot);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Attack.Tag = call__107.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Attack.Tag = -1;
                    }

                    var call__108 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipDefenseSlot);
                    if (call__108 != null)
                    {
                        var call__109 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipDefenseSlot);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Defend.Tag = call__109.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Defend.Tag = -1;
                    }

                    var call__110 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipHeadSlot);
                    if (call__110 != null)
                    {
                        var call__111 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipHeadSlot);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Head.Tag = call__111.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Head.Tag = -1;
                    }

                    var call__112 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipTorsoSlot);
                    if (call__112 != null)
                    {
                        var call__113 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipTorsoSlot);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Torso.Tag = call__113.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Torso.Tag = -1;
                    }

                    var call__114 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipLeftRingSlot);
                    if (call__114 != null)
                    {
                        var call__115 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipLeftRingSlot);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RingL.Tag = call__115.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RingL.Tag = -1;
                    }

                    var call__116 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipRightRingSlot);
                    if (call__116 != null)
                    {
                        var call__117 = H.FHeroItems.GetPositionItem(Aow.Units.AoWE.THeroItemPosition.ipRightRingSlot);
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RingR.Tag = call__117.FItemID;
                    }
                    else
                    {
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RingR.Tag = -1;
                    }

                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UpdateGround(0);
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem = null;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP1.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP2.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP3.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP4.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP5.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP6.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP7.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.BP8.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Attack.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Defend.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Head.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.Torso.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RingL.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.RingR.Checked = false;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ItemUseBtn.Visible = false;
                }

                if (U != null)
                {
                    var call__118 = U.GetHitPoints();
                    var call__119 = U.GetHits();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UHits.Text = (Aow.Rtl.Rtl.IntToStr(call__118) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__119);
                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null)
                    {
                        var call__120 = U.GetMovePoints();
                        var call__121 = U.GetMoves();
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UMoves.Text = (Aow.Rtl.Rtl.IntToStr(call__120) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__121);
                    }
                    else
                    {
                        if (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit != null)
                        {
                            var call__122 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.GetMoves();
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UMoves.Text = (Aow.Rtl.Rtl.IntToStr(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FMovePoints) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__122);
                        }
                    }

                    var call__123 = U.GetAttack();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UAttack.Text = Aow.Rtl.Rtl.IntToStr(call__123);
                    var call__124 = U.GetDamage();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UDamage.Text = Aow.Rtl.Rtl.IntToStr(call__124);
                    var call__125 = U.GetDefense();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UDefense.Text = Aow.Rtl.Rtl.IntToStr(call__125);
                    var call__126 = U.GetResistance();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UResistance.Text = Aow.Rtl.Rtl.IntToStr(call__126);
                    var call__127 = U.GetMoveTypes();
                    var call__128 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__127);
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UMovesIcon.CurrentImage = call__128;
                    if (U.GetManaUpkeep() > 0)
                    {
                        var call__129 = U.GetManaUpkeep();
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UpKeep.Text = Aow.Rtl.Rtl.IntToStr(call__129);
                        UpkeepIcon.CurrentImage = 52;
                    }
                    else
                    {
                        var call__130 = U.GetUpkeep();
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UpKeep.Text = Aow.Rtl.Rtl.IntToStr(call__130);
                        UpkeepIcon.CurrentImage = 51;
                    }

                    var call__131 = U.GetAlignment();
                    AAlignment.Visible = call__131 != Aow.Units.AoWE.TAlignment.alNone;
                    switch (U.GetAlignment())
                    {
                        case Aow.Units.AoWE.TAlignment.alPureGood:
                            AAlignment.CurrentImage = 31;
                            break;
                        case Aow.Units.AoWE.TAlignment.alGood:
                            AAlignment.CurrentImage = 32;
                            break;
                        case Aow.Units.AoWE.TAlignment.alPureNeutral:
                        case Aow.Units.AoWE.TAlignment.alNeutral:
                            AAlignment.CurrentImage = 33;
                            break;
                        case Aow.Units.AoWE.TAlignment.alEvil:
                            AAlignment.CurrentImage = 34;
                            break;
                        case Aow.Units.AoWE.TAlignment.alPureEvil:
                            AAlignment.CurrentImage = 35;
                            break;
                    }

                    var call__132 = U.GetAlignment();
                    var call__133 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TAlignmentRStr[(int)call__132]);
                    AAlignmentLbl.Text = call__133;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T1AbilityLB.Items.Clear();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T1TypeLB.Items.Clear();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T1SrcLB.Items.Clear();
                    var call__134 = U.GetMorale();
                    var call__135 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TMoraleRStr[(int)call__134]);
                    var call__136 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MoraleXRStr);
                    var call__137 = Aow.Units.AoWE.AoWE.Format(call__136, [call__135]);
                    MoraleLbl.Text = call__137;
                    var call__138 = U.GetUnitType();
                    MoraleLbl.Visible = call__138 != Aow.Units.AoWE.TUnitType.utMachine;
                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null)
                    {
                        U.ListAbilityInfo(T1AbilityLB.Items, T1TypeLB.Items, T1SrcLB.Items);
                        Aow.Units.Engine.Engine.SortStringsEx(T1AbilityLB.Items, Aow.Units.AoWUnitWindow.AoWUnitWindow.CompareUnitAbilities, [T1TypeLB.Items, T1SrcLB.Items]);
                    }
                    else
                    {
                        if (((Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit != null) && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit != null)) && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit != null))
                        {
                            Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit.ListAbilityInfo(T1AbilityLB.Items, T1TypeLB.Items, T1SrcLB.Items);
                            Aow.Units.Engine.Engine.SortStringsEx(T1AbilityLB.Items, Aow.Units.AoWUnitWindow.AoWUnitWindow.CompareUnitAbilities, [T1TypeLB.Items, T1SrcLB.Items]);
                        }
                    }

                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T1AbilityLB.Selected = -1;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.T1AbilityLBChange(T1AbilityLB);
                    UnitWin.FDrawn = false;
                }
            }
        }

        // AoWUnitWindow.pas:985
        // { draft; 00407DB8 }
        public void UpdateGround(int Offset)
        {
            Aow.Units.AoWE.TAbstractUnit U = default;
            Gnd1.Tag = -1;
            Gnd2.Tag = -1;
            Gnd3.Tag = -1;
            Gnd4.Tag = -1;
            Gnd5.Tag = -1;
            Gnd1.Checked = false;
            Gnd2.Checked = false;
            Gnd3.Checked = false;
            Gnd4.Checked = false;
            Gnd5.Checked = false;
            U = Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit;
            if (U != null)
            {
                if (U is Aow.Units.AoWE.THero)
                {
                    if (((Aow.Units.AoWE.THero)U).GetItemsOnGround() != null)
                    {
                        var call__139 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                        var call__140 = call__139.Count;
                        FGroundCount = call__140;
                        FGroundOffset = Offset;
                        // { Original last-page clamp leaves only four items visible. }
                        if (FGroundCount < (Offset + 5))
                        {
                            FGroundOffset = FGroundCount - 4;
                        }

                        if (FGroundOffset < 0)
                        {
                            FGroundOffset = 0;
                        }

                        GroundL.Visible = FGroundOffset > 0;
                        GroundR.Visible = (FGroundOffset + 5) <= FGroundCount;
                        var call__141 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                        var call__142 = call__141.Count;
                        if (FGroundOffset < call__142)
                        {
                            var call__143 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                            var call__144 = call__143.GetItem(FGroundOffset);
                            Gnd1.Tag = call__144.FItemID;
                        }

                        var call__145 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                        var call__146 = call__145.Count;
                        if ((FGroundOffset + 1) < call__146)
                        {
                            var call__147 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                            var call__148 = call__147.GetItem(FGroundOffset + 1);
                            Gnd2.Tag = call__148.FItemID;
                        }

                        var call__149 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                        var call__150 = call__149.Count;
                        if ((FGroundOffset + 2) < call__150)
                        {
                            var call__151 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                            var call__152 = call__151.GetItem(FGroundOffset + 2);
                            Gnd3.Tag = call__152.FItemID;
                        }

                        var call__153 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                        var call__154 = call__153.Count;
                        if ((FGroundOffset + 3) < call__154)
                        {
                            var call__155 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                            var call__156 = call__155.GetItem(FGroundOffset + 3);
                            Gnd4.Tag = call__156.FItemID;
                        }

                        var call__157 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                        var call__158 = call__157.Count;
                        if ((FGroundOffset + 4) < call__158)
                        {
                            var call__159 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                            var call__160 = call__159.GetItem(FGroundOffset + 4);
                            Gnd5.Tag = call__160.FItemID;
                        }
                    }
                }
            }
        }

        // AoWUnitWindow.pas:472
        public void PreDrawAbilityItem(Aow.Units.AoWComp.TAoWComponent Sender, in Aow.Rtl.TRect DrawRect, int DrawItem, ref Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            bool Enabled = default;
            Aow.Units.AoWE.TAbility A = default;
            var call__161 = T1AbilityLB.Items.Objects[DrawItem];
            var call__162 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__161));
            A = call__162;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null)
            {
                bool bool__163 = ((A != null) && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit != null)) && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                if (bool__163)
                {
                    var call__164 = A.GetControlType(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                    bool__163 = call__164.Contains(Aow.Units.AoWE.TAbilityControlTypeElement.acAction);
                }

                Enabled = bool__163;
            }
            else
            {
                bool bool__165 = (A != null) && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit != null);
                if (bool__165)
                {
                    var call__166 = T1AbilityLB.Items.Objects[DrawItem];
                    var call__167 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__166));
                    var call__168 = call__167.GetControlType(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit);
                    bool__165 = call__168.Contains(Aow.Units.AoWE.TAbilityControlTypeElement.acCombatAction);
                }

                bool bool__169 = bool__165;
                if (bool__169)
                {
                    var call__170 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.GetPlayer();
                    bool__169 = (call__170 == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                }

                bool bool__171 = bool__169 && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FMovePoints > 0);
                if (bool__171)
                {
                    var call__172 = T1AbilityLB.Items.Objects[DrawItem];
                    var call__173 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__172));
                    var call__174 = call__173.GetCombatMode(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit);
                    bool bool__175 = TSet<Aow.Units.AoWE.TCombatMode>.Of(Aow.Units.AoWE.TCombatMode.cmShortRange, Aow.Units.AoWE.TCombatMode.cmMediumRange, Aow.Units.AoWE.TCombatMode.cmLongRange).Contains(call__174);
                    if (!bool__175)
                    {
                        var call__176 = T1AbilityLB.Items.Objects[DrawItem];
                        bool__175 = (Aow.Rtl.Rtl.ObjectToInt(call__176) == 52);
                    }

                    bool bool__177 = !bool__175;
                    if (!bool__177)
                    {
                        var call__178 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit.GetMoves();
                        bool__177 = (call__178 == Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FMovePoints);
                    }

                    bool__171 = bool__177;
                }

                Enabled = bool__171;
            }

            if (Enabled)
            {
                Sender.TextColor = 14679807;
            }
            else
            {
                Sender.TextColor = 9415072;
            }
        }

        // AoWUnitWindow.pas:502
        public void HelpClick(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWUnitWindow.pas:507
        public void T1AbilityLBChange(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TAbility A = default;
            T1AbilityLB.Selected = ((Aow.Units.AoWListBox.TAOWListBox)Sender).Selected;
            T1TypeLB.Selected = ((Aow.Units.AoWListBox.TAOWListBox)Sender).Selected;
            T1SrcLB.Selected = ((Aow.Units.AoWListBox.TAOWListBox)Sender).Selected;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null)
            {
                if ((Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer) && (T1AbilityLB.Selected > -1))
                {
                    var call__179 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                    var call__180 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__179));
                    A = call__180;
                    bool bool__181 = (A != null);
                    if (bool__181)
                    {
                        var call__182 = A.GetEnabled(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                        bool__181 = call__182;
                    }

                    if (bool__181)
                    {
                        if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null)
                        {
                            var call__183 = A.GetControlType(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                            T1UseBut.Visible = call__183.Contains(Aow.Units.AoWE.TAbilityControlTypeElement.acAction);
                        }
                        else
                        {
                            var call__184 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                            var call__185 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__184));
                            var call__186 = call__185.GetCombatMode(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                            bool bool__187 = (call__186 != Aow.Units.AoWE.TCombatMode.cmNone);
                            if (bool__187)
                            {
                                var call__188 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                                var call__189 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__188));
                                var call__190 = call__189.GetCombatMode(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                                bool__187 = (call__190 != Aow.Units.AoWE.TCombatMode.cmMelee);
                            }

                            T1UseBut.Visible = bool__187;
                        }

                        if (T1UseBut.Visible)
                        {
                            var call__191 = A.GetName(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                            var call__192 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UseXRStr);
                            var call__193 = Aow.Units.AoWE.AoWE.Format(call__192, [call__191]);
                            T1UseBut.Text = call__193;
                        }
                    }
                    else
                    {
                        T1UseBut.Visible = false;
                    }
                }
                else
                {
                    T1UseBut.Visible = false;
                }
            }
            else
            {
                bool bool__194 = (T1AbilityLB.Selected > -1) && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit != null);
                if (bool__194)
                {
                    var call__195 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                    var call__196 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__195));
                    var call__197 = call__196.GetControlType(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit);
                    bool__194 = call__197.Contains(Aow.Units.AoWE.TAbilityControlTypeElement.acCombatAction);
                }

                bool bool__198 = bool__194;
                if (bool__198)
                {
                    var call__199 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.GetPlayer();
                    bool__198 = (call__199 == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                }

                bool bool__200 = bool__198 && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FMovePoints > 0);
                if (bool__200)
                {
                    var call__201 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                    var call__202 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__201));
                    var call__203 = call__202.GetCombatMode(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit);
                    bool bool__204 = TSet<Aow.Units.AoWE.TCombatMode>.Of(Aow.Units.AoWE.TCombatMode.cmShortRange, Aow.Units.AoWE.TCombatMode.cmMediumRange, Aow.Units.AoWE.TCombatMode.cmLongRange).Contains(call__203);
                    if (!bool__204)
                    {
                        var call__205 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                        bool__204 = (Aow.Rtl.Rtl.ObjectToInt(call__205) == 52);
                    }

                    bool bool__206 = !bool__204;
                    if (!bool__206)
                    {
                        var call__207 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit.GetMoves();
                        bool__206 = (call__207 == Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FMovePoints);
                    }

                    bool__200 = bool__206;
                }

                if (bool__200)
                {
                    T1UseBut.Visible = true;
                    var call__208 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                    var call__209 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__208));
                    var call__210 = call__209.GetName(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit);
                    var call__211 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UseXRStr);
                    var call__212 = Aow.Units.AoWE.AoWE.Format(call__211, [call__210]);
                    T1UseBut.Text = call__212;
                }
                else
                {
                    T1UseBut.Visible = false;
                }
            }
        }

        // AoWUnitWindow.pas:562
        public void T1SbarChange(Aow.Rtl.TObject Sender)
        {
            T1AbilityLB.ListOffset = T1SBar.Position;
            T1TypeLB.ListOffset = T1SBar.Position;
        }

        // AoWUnitWindow.pas:568
        public void ItemRHMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.AoWCheckBox.TAOWCheckBox Check = default;
            Check = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
            Aow.Units.AoWItemBanner.AoWItemBanner.ItemID = Check.Tag;
        }

        // AoWUnitWindow.pas:575
        public void UnitWinResize(Aow.Rtl.TObject Sender)
        {
            {
                var w__213 = UnitWin;
                w__213.Alignment.LeftOffset = w__213.WinLeft;
                w__213.Alignment.TopOffset = w__213.WinTop;
                w__213.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
                w__213.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
            }
        }

        // AoWUnitWindow.pas:586
        public void UnitWindowCreate(Aow.Rtl.TObject Sender)
        {
            FItemType = (Aow.Units.AoWE.TItemType)6;
        }

        // AoWUnitWindow.pas:591
        public void UnitWinUpdate(Aow.Rtl.TObject Sender)
        {
            if (UnitWin.Visible)
            {
                FHighlight += FHighlightStep;
                if ((FHighlight > 100) || (FHighlight < 50))
                {
                    FHighlightStep = -FHighlightStep;
                    FHighlight += FHighlightStep;
                }

                switch (FItemType)
                {
                    case Aow.Units.AoWE.TItemType.itAttack:
                        Attack.FDrawn = false;
                        break;
                    case Aow.Units.AoWE.TItemType.itDefense:
                        Defend.FDrawn = false;
                        break;
                    case Aow.Units.AoWE.TItemType.itHead:
                        Head.FDrawn = false;
                        break;
                    case Aow.Units.AoWE.TItemType.itTorso:
                        Torso.FDrawn = false;
                        break;
                    case Aow.Units.AoWE.TItemType.itRing:
                        RingL.FDrawn = false;
                        RingR.FDrawn = false;
                        break;
                }

                if (Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem != null)
                {
                    FBlinkDelay -= 1;
                    if (FBlinkDelay < 0)
                    {
                        FBlinkDelay = 2;
                        FItemVisible = !FItemVisible;
                    }
                }
            }
        }

        // AoWUnitWindow.pas:620
        public void T1SortNameClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T1AbilityLB);
            Lists.Add(T1TypeLB);
            Lists.Add(T1SrcLB);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 0);
            T1UseBut.Visible = false;
        }

        // AoWUnitWindow.pas:631
        public void T1SortTypeClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T1TypeLB);
            Lists.Add(T1AbilityLB);
            Lists.Add(T1SrcLB);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 0);
            T1UseBut.Visible = false;
        }

        // AoWUnitWindow.pas:642
        public void T1SortSourceClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T1SrcLB);
            Lists.Add(T1AbilityLB);
            Lists.Add(T1TypeLB);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 0);
            T1UseBut.Visible = false;
        }

        // AoWUnitWindow.pas:653
        public void UseAbility()
        {
            Aow.Units.AoWE.TAbility A = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__214 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
            var call__215 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__214));
            A = call__215;
            bool bool__216 = (A != null);
            if (bool__216)
            {
                var call__217 = A.GetEnabled(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                bool__216 = call__217;
            }

            if (bool__216)
            {
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null)
                {
                    if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
                    {
                        var call__218 = A.Activate(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit, ref Reason);
                        if (call__218)
                        {
                            UnitWin.Visible = false;
                        }
                        else
                        {
                            if (Reason != ((AnsiString)""))
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                            }
                        }
                    }
                }
                else
                {
                    if ((Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit != null) && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer))
                    {
                        var call__219 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                        var call__220 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.SelectAbility(Aow.Rtl.Rtl.ObjectToInt(call__219));
                        if (call__220 > -1)
                        {
                            UnitWin.Visible = false;
                        }
                    }
                }
            }
        }

        // AoWUnitWindow.pas:676
        public void UseItemClick(Aow.Rtl.TObject Sender)
        {
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if ((Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem != null) && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit is Aow.Units.AoWE.THero))
            {
                var call__221 = ((Aow.Units.AoWE.THero)Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit).CanUseItem(Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem.Tag, ref Reason);
                if (call__221)
                {
                    ((Aow.Units.AoWE.THero)Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit).UseItem(Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem.Tag);
                }
                else
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                }
            }
        }

        // AoWUnitWindow.pas:688
        public void UnitTabChange(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWUnitWindow.pas:693
        public void UnitWinMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if ((Button == Aow.Rtl.TMouseButton.mbRight) && (Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem != null))
            {
                Gnd1Change(Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem);
            }
        }

        // AoWUnitWindow.pas:698
        public void UnitWinEnter(Aow.Rtl.TObject Sender)
        {
            /* TODO(T-shim) Platform or editor boundary AoWUnit.FCursorImage */
            throw new System.NotImplementedException();
        }

        // AoWUnitWindow.pas:703
        public void UnitWinExit(Aow.Rtl.TObject Sender)
        {
            /* TODO(T-shim) Platform or editor boundary AoWUnit.FCursorImage */
            throw new System.NotImplementedException();
        }

        // AoWUnitWindow.pas:708
        public void UnitWinShow(Aow.Rtl.TObject Sender)
        {
            /* TODO(T-shim) Platform or editor boundary AoWUnit.FCursorImage */
            throw new System.NotImplementedException();
        }

        // AoWUnitWindow.pas:714
        public void GroundRClick(Aow.Rtl.TObject Sender)
        {
            UpdateGround(FGroundOffset + 1);
        }

        // AoWUnitWindow.pas:719
        public void GroundLClick(Aow.Rtl.TObject Sender)
        {
            UpdateGround(FGroundOffset - 1);
        }

        // AoWUnitWindow.pas:724
        public void CloseClick(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            UnitWin.Visible = false;
        }

        // AoWUnitWindow.pas:730
        public void Gnd1Draw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Aow.Units.AoWE.THero U = default;
            Aow.Units.AoWE.TItem Item = default;
            Aow.Units.AoWComp.TAoWComponent Component = default;
            Component = Sender;
            if (Component.Tag != -1)
            {
                U = (Aow.Units.AoWE.THero)Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit;
                var call__222 = U.FInventory.FindItem(Component.Tag);
                Item = call__222;
                if (Item == null)
                {
                    var call__223 = U.FHeroItems.FindItem(Component.Tag);
                    Item = call__223;
                }

                if (Item == null)
                {
                    if (U.GetItemsOnGround() != null)
                    {
                        var call__224 = U.GetItemsOnGround();
                        var call__225 = call__224.FindItem(Component.Tag);
                        Item = call__225;
                    }
                }

                if (Item != null)
                {
                    {
                        var w__226 = T3Panel;
                        {
                            var w__227 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Component;
                            if (Item.GetImage() != null)
                            {
                                var call__228 = Item.GetImage();
                                call__228.ShowClipped(T3Panel.WinLeft + w__227.WinLeft, T3Panel.WinTop + w__227.WinTop, SurfDesc);
                            }
                        }
                    }
                }
            }
        }

        // AoWUnitWindow.pas:750
        public void FrLShPreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Aow.Rtl.TRect R = default;
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit != null)
            {
                var call__229 = Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.GetFace();
                if (call__229 != null)
                {
                    {
                        var w__230 = (Aow.Units.AoWImage.TAOWImage)Sender;
                        var call__231 = Aow.Rtl.Rtl.Rect(w__230.WinLeft + 29, w__230.WinTop + 28, w__230.WinLeft + 132, w__230.WinTop + 156);
                        R = call__231;
                    }

                    Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.ShowFace(R, SurfDesc);
                }
            }
        }

        // AoWUnitWindow.pas:763
        public void FrRShPreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit != null)
            {
                FShowInfo.Direction = 3;
                if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit is Aow.Units.AoWE.THero)
                {
                    FShowInfo.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of();
                }
                else
                {
                    FShowInfo.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of(Aow.Units.AoWE.TUnitOverlayModesElement.uoRank);
                }

                {
                    var w__232 = (Aow.Units.AoWImage.TAOWImage)Sender;
                    Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.ShowEx(w__232.WinLeft + 49, w__232.WinTop + 78, SurfDesc, FShowInfo);
                }
            }
        }

        // AoWUnitWindow.pas:773
        public void Gnd1Change(Aow.Rtl.TObject Sender)
        {
            /* TODO(T-shim) Platform or editor boundary AoWUnit.FCursorImage */
            throw new System.NotImplementedException();
        }

        // AoWUnitWindow.pas:855
        public void Gnd3MouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            // { Tried typed checkbox Sender; no exact match. }
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit != null)
            {
                var call__233 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.FPlayer);
                var call__234 = call__233.GetBusy();
                if (!call__234)
                {
                    if (Button == Aow.Rtl.TMouseButton.mbRight)
                    {
                        if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag == -1)
                        {
                            ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).PopupMenu = null;
                        }
                        else
                        {
                            ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).PopupMenu = Aow.Units.AoWItemBanner.AoWItemBanner.ItemBanner.IBannerPopup;
                        }

                        Aow.Units.AoWItemBanner.AoWItemBanner.ItemID = ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag;
                    }
                    else
                    {
                        if (Sender == Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem)
                        {
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem.Checked = false;
                        }
                    }
                }
            }
        }

        // AoWUnitWindow.pas:871
        public void HeadDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Gnd1Draw(Sender, SurfDesc);
            if (((Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem != null) && (Sender != Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem)) && (FItemType == ((Aow.Units.AoWE.TItemType)0)))
            {
                {
                    var w__235 = T3Panel;
                    {
                        var w__236 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                        var call__237 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin[24];
                        call__237.ShowBlended(T3Panel.WinLeft + w__236.WinLeft, T3Panel.WinTop + w__236.WinTop, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[FHighlight]);
                    }
                }
            }
        }

        // AoWUnitWindow.pas:881
        public void TorsoDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Gnd1Draw(Sender, SurfDesc);
            if (((Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem != null) && (Sender != Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem)) && (FItemType == ((Aow.Units.AoWE.TItemType)1)))
            {
                {
                    var w__238 = T3Panel;
                    {
                        var w__239 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                        var call__240 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin[24];
                        call__240.ShowBlended(T3Panel.WinLeft + w__239.WinLeft, T3Panel.WinTop + w__239.WinTop, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[FHighlight]);
                    }
                }
            }
        }

        // AoWUnitWindow.pas:891
        public void AttackDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Gnd1Draw(Sender, SurfDesc);
            if (((Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem != null) && (Sender != Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem)) && (FItemType == ((Aow.Units.AoWE.TItemType)2)))
            {
                {
                    var w__241 = T3Panel;
                    {
                        var w__242 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                        var call__243 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin[24];
                        call__243.ShowBlended(T3Panel.WinLeft + w__242.WinLeft, T3Panel.WinTop + w__242.WinTop, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[FHighlight]);
                    }
                }
            }
        }

        // AoWUnitWindow.pas:901
        public void DefendDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Gnd1Draw(Sender, SurfDesc);
            if (((Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem != null) && (Sender != Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem)) && (FItemType == ((Aow.Units.AoWE.TItemType)3)))
            {
                {
                    var w__244 = T3Panel;
                    {
                        var w__245 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                        var call__246 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin[24];
                        call__246.ShowBlended(T3Panel.WinLeft + w__245.WinLeft, T3Panel.WinTop + w__245.WinTop, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[FHighlight]);
                    }
                }
            }
        }

        // AoWUnitWindow.pas:911
        public void RingLDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Gnd1Draw(Sender, SurfDesc);
            if (((Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem != null) && (Sender != Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem)) && (FItemType == ((Aow.Units.AoWE.TItemType)4)))
            {
                {
                    var w__247 = T3Panel;
                    {
                        var w__248 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                        var call__249 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin[24];
                        call__249.ShowBlended(T3Panel.WinLeft + w__248.WinLeft, T3Panel.WinTop + w__248.WinTop, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[FHighlight]);
                    }
                }
            }
        }

        // AoWUnitWindow.pas:921
        public void DisbandBtnClick()
        {
            var call__250 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.DisbandUnitRStr);
            var call__251 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.ConfirmRStr);
            Aow.Units.E2Globals.E2Globals.DialogWin.Confirm(call__251, call__250, 7);
        }

        // AoWUnitWindow.pas:926
        // { draft }
        public void T1AbilityLBMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.AoWE.TAbility A = default;
            if (((Button == Aow.Rtl.TMouseButton.mbRight) && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit != null)) && (T1AbilityLB.Selected != -1))
            {
                var call__252 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                var call__253 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__252));
                A = call__253;
                if (A != null)
                {
                    var call__254 = A.GetName(null);
                    Aow.Units.AoWExeForms.AoWExeForms.SetInfoBanner(Aow.Units.AoWInfoBanner.AoWInfoBanner.InfoBanner, call__254, A.FDescription);
                }
            }
        }

        // AoWUnitWindow.pas:937
        public void UnitWinKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            switch (Key)
            {
                case 27:
                    UnitWin.Visible = false;
                    break;
            }
        }

        // AoWUnitWindow.pas:944
        public void PPrevClick()
        {
            int I = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList != null)
            {
                var call__255 = Aow.Units.AoWWinMan.AoWWinMan.General.IndexOfUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                I = call__255;
                if ((I > -1) && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[0] > -1))
                {
                    I -= 1;
                    if (I < 0)
                    {
                        I = 7;
                        while (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[I] == -1)
                        {
                            I -= 1;
                        }
                    }

                    var call__256 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[I]);
                    Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit = call__256;
                    ShowUnit();
                }
            }
        }

        // AoWUnitWindow.pas:966
        public void PNextClick()
        {
            int I = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList != null)
            {
                var call__257 = Aow.Units.AoWWinMan.AoWWinMan.General.IndexOfUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                I = call__257;
                if (I > -1)
                {
                    I += 1;
                    if (I > 7)
                    {
                        I = 0;
                    }

                    if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[I] == -1)
                    {
                        I = 0;
                    }

                    var call__258 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[I]);
                    Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit = call__258;
                    ShowUnit();
                }
            }
        }
    }

    public static partial class AoWUnitWindow
    {
        public static Aow.Units.AoWUnitWindow.TUnitWindow UnitWindow;
        public static Aow.Units.AoWCheckBox.TAOWCheckBox SelectedItem;
        public static readonly Aow.Rtl.AnsiString AbilitiesRStr = (AnsiString)"Abilities";
        public static readonly Aow.Rtl.AnsiString InfoRStr = (AnsiString)"Info";
        public static readonly Aow.Rtl.AnsiString NoneRStr = (AnsiString)"None";
        public static readonly Aow.Rtl.AnsiString CreatureRStr = (AnsiString)"Creature";
        public static readonly Aow.Rtl.AnsiString MachineRStr = (AnsiString)"Machine";
        public static readonly Aow.Rtl.AnsiString UnitRStr = (AnsiString)"Unit";
        public static readonly Aow.Rtl.AnsiString LeaderRStr = (AnsiString)"Leader";
        public static readonly Aow.Rtl.AnsiString HeroRStr = (AnsiString)"Hero";
        public static readonly Aow.Rtl.AnsiString NoLeaderRStr = (AnsiString)"Hero has no Leader";
        public static readonly Aow.Rtl.AnsiString LeaderSpheresRStr = (AnsiString)"Leaders magic spheres";
        public static readonly Aow.Rtl.AnsiString ItemsRStr = (AnsiString)"Items";
        public static readonly Aow.Rtl.AnsiString LevelRStr = (AnsiString)"Level";
        public static readonly Aow.Rtl.AnsiString ExpRStr = (AnsiString)"Exp";
        // AoWUnitWindow.pas:220
        public static int CompareUnitAbilities(Aow.Rtl.AnsiString S1, Aow.Rtl.AnsiString S2, Aow.Rtl.TObject O1, Aow.Rtl.TObject O2)
        {
            int Result = default;
            Aow.Units.AoWE.TAbility A1 = default;
            Aow.Units.AoWE.TAbility A2 = default;
            int V1 = default;
            int V2 = default;
            var call__259 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(O1));
            A1 = call__259;
            var call__260 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(O2));
            A2 = call__260;
            var call__261 = A1.GetEnabled(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
            bool bool__262 = call__261;
            if (bool__262)
            {
                var call__263 = A1.GetControlType(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                bool__262 = call__263.Contains(Aow.Units.AoWE.TAbilityControlTypeElement.acAction);
            }

            if (bool__262)
            {
                V1 = 10;
            }
            else
            {
                V1 = 0;
            }

            var call__264 = A2.GetEnabled(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
            bool bool__265 = call__264;
            if (bool__265)
            {
                var call__266 = A2.GetControlType(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                bool__265 = call__266.Contains(Aow.Units.AoWE.TAbilityControlTypeElement.acAction);
            }

            if (bool__265)
            {
                V2 = 10;
            }
            else
            {
                V2 = 0;
            }

            Result = V2 - V1;
            if (Result == 0)
            {
                Result = Aow.Rtl.Rtl.CompareStr(S1, S2);
            }

            return Result;
        }
    }
}
