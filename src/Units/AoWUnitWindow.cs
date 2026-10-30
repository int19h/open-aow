// Translated from AoWUnitWindow.pas.
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
        public TUnitWindow(TComponent owner) : base(owner)
        { }

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
                        var call__1 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                        var call__2 = call__1.Count;
                        FGroundCount = call__2;
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
                        var call__3 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                        var call__4 = call__3.Count;
                        if (FGroundOffset < call__4)
                        {
                            var call__5 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                            var call__6 = call__5.GetItem(FGroundOffset);
                            Gnd1.Tag = call__6.FItemID;
                        }

                        var call__7 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                        var call__8 = call__7.Count;
                        if ((FGroundOffset + 1) < call__8)
                        {
                            var call__9 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                            var call__10 = call__9.GetItem(FGroundOffset + 1);
                            Gnd2.Tag = call__10.FItemID;
                        }

                        var call__11 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                        var call__12 = call__11.Count;
                        if ((FGroundOffset + 2) < call__12)
                        {
                            var call__13 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                            var call__14 = call__13.GetItem(FGroundOffset + 2);
                            Gnd3.Tag = call__14.FItemID;
                        }

                        var call__15 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                        var call__16 = call__15.Count;
                        if ((FGroundOffset + 3) < call__16)
                        {
                            var call__17 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                            var call__18 = call__17.GetItem(FGroundOffset + 3);
                            Gnd4.Tag = call__18.FItemID;
                        }

                        var call__19 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                        var call__20 = call__19.Count;
                        if ((FGroundOffset + 4) < call__20)
                        {
                            var call__21 = ((Aow.Units.AoWE.THero)U).GetItemsOnGround();
                            var call__22 = call__21.GetItem(FGroundOffset + 4);
                            Gnd5.Tag = call__22.FItemID;
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
            var call__23 = T1AbilityLB.Items.Objects[DrawItem];
            var call__24 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__23));
            A = call__24;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null)
            {
                bool bool__25 = ((A != null) && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit != null)) && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                if (bool__25)
                {
                    var call__26 = A.GetControlType(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                    bool__25 = call__26.Contains(Aow.Units.AoWE.TAbilityControlTypeElement.acAction);
                }

                Enabled = bool__25;
            }
            else
            {
                bool bool__27 = (A != null) && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit != null);
                if (bool__27)
                {
                    var call__28 = T1AbilityLB.Items.Objects[DrawItem];
                    var call__29 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__28));
                    var call__30 = call__29.GetControlType(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit);
                    bool__27 = call__30.Contains(Aow.Units.AoWE.TAbilityControlTypeElement.acCombatAction);
                }

                bool bool__31 = bool__27;
                if (bool__31)
                {
                    var call__32 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.GetPlayer();
                    bool__31 = (call__32 == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                }

                bool bool__33 = bool__31 && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FMovePoints > 0);
                if (bool__33)
                {
                    var call__34 = T1AbilityLB.Items.Objects[DrawItem];
                    var call__35 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__34));
                    var call__36 = call__35.GetCombatMode(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit);
                    bool bool__37 = TSet<Aow.Units.AoWE.TCombatMode>.Of(Aow.Units.AoWE.TCombatMode.cmShortRange, Aow.Units.AoWE.TCombatMode.cmMediumRange, Aow.Units.AoWE.TCombatMode.cmLongRange).Contains(call__36);
                    if (!bool__37)
                    {
                        var call__38 = T1AbilityLB.Items.Objects[DrawItem];
                        bool__37 = (Aow.Rtl.Rtl.ObjectToInt(call__38) == 52);
                    }

                    bool bool__39 = !bool__37;
                    if (!bool__39)
                    {
                        var call__40 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit.GetMoves();
                        bool__39 = (call__40 == Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FMovePoints);
                    }

                    bool__33 = bool__39;
                }

                Enabled = bool__33;
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
                    var call__41 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                    var call__42 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__41));
                    A = call__42;
                    bool bool__43 = (A != null);
                    if (bool__43)
                    {
                        var call__44 = A.GetEnabled(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                        bool__43 = call__44;
                    }

                    if (bool__43)
                    {
                        if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null)
                        {
                            var call__45 = A.GetControlType(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                            T1UseBut.Visible = call__45.Contains(Aow.Units.AoWE.TAbilityControlTypeElement.acAction);
                        }
                        else
                        {
                            var call__46 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                            var call__47 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__46));
                            var call__48 = call__47.GetCombatMode(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                            bool bool__49 = (call__48 != Aow.Units.AoWE.TCombatMode.cmNone);
                            if (bool__49)
                            {
                                var call__50 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                                var call__51 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__50));
                                var call__52 = call__51.GetCombatMode(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                                bool__49 = (call__52 != Aow.Units.AoWE.TCombatMode.cmMelee);
                            }

                            T1UseBut.Visible = bool__49;
                        }

                        if (T1UseBut.Visible)
                        {
                            var call__53 = A.GetName(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                            var call__54 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UseXRStr);
                            var call__55 = Aow.Units.AoWE.AoWE.Format(call__54, [call__53]);
                            T1UseBut.Text = call__55;
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
                bool bool__56 = (T1AbilityLB.Selected > -1) && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit != null);
                if (bool__56)
                {
                    var call__57 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                    var call__58 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__57));
                    var call__59 = call__58.GetControlType(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit);
                    bool__56 = call__59.Contains(Aow.Units.AoWE.TAbilityControlTypeElement.acCombatAction);
                }

                bool bool__60 = bool__56;
                if (bool__60)
                {
                    var call__61 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.GetPlayer();
                    bool__60 = (call__61 == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                }

                bool bool__62 = bool__60 && (Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FMovePoints > 0);
                if (bool__62)
                {
                    var call__63 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                    var call__64 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__63));
                    var call__65 = call__64.GetCombatMode(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit);
                    bool bool__66 = TSet<Aow.Units.AoWE.TCombatMode>.Of(Aow.Units.AoWE.TCombatMode.cmShortRange, Aow.Units.AoWE.TCombatMode.cmMediumRange, Aow.Units.AoWE.TCombatMode.cmLongRange).Contains(call__65);
                    if (!bool__66)
                    {
                        var call__67 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                        bool__66 = (Aow.Rtl.Rtl.ObjectToInt(call__67) == 52);
                    }

                    bool bool__68 = !bool__66;
                    if (!bool__68)
                    {
                        var call__69 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit.GetMoves();
                        bool__68 = (call__69 == Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FMovePoints);
                    }

                    bool__62 = bool__68;
                }

                if (bool__62)
                {
                    T1UseBut.Visible = true;
                    var call__70 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                    var call__71 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__70));
                    var call__72 = call__71.GetName(Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.FUnit.FUnit);
                    var call__73 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UseXRStr);
                    var call__74 = Aow.Units.AoWE.AoWE.Format(call__73, [call__72]);
                    T1UseBut.Text = call__74;
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
            this.UnitWin.Alignment.LeftOffset = this.UnitWin.WinLeft;
            this.UnitWin.Alignment.TopOffset = this.UnitWin.WinTop;
            this.UnitWin.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
            this.UnitWin.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
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
            var call__76 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
            var call__77 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__76));
            A = call__77;
            bool bool__78 = (A != null);
            if (bool__78)
            {
                var call__79 = A.GetEnabled(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                bool__78 = call__79;
            }

            if (bool__78)
            {
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null)
                {
                    if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
                    {
                        var call__80 = A.Activate(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit, ref Reason);
                        if (call__80)
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
                        var call__81 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                        var call__82 = Aow.Units.AoWWinMan.AoWWinMan.General.FCombatUnit.SelectAbility(Aow.Rtl.Rtl.ObjectToInt(call__81));
                        if (call__82 > -1)
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
                var call__83 = ((Aow.Units.AoWE.THero)Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit).CanUseItem(Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem.Tag, ref Reason);
                if (call__83)
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
                var call__84 = U.FInventory.FindItem(Component.Tag);
                Item = call__84;
                if (Item == null)
                {
                    var call__85 = U.FHeroItems.FindItem(Component.Tag);
                    Item = call__85;
                }

                if (Item == null)
                {
                    if (U.GetItemsOnGround() != null)
                    {
                        var call__86 = U.GetItemsOnGround();
                        var call__87 = call__86.FindItem(Component.Tag);
                        Item = call__87;
                    }
                }

                if (Item != null)
                {
                    var panel = T3Panel;
                    var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Component;
                    if (Item.GetImage() != null)
                    {
                        var call__90 = Item.GetImage();
                        call__90.ShowClipped(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc);
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
                var call__91 = Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.GetFace();
                if (call__91 != null)
                {
                    var image = (Aow.Units.AoWImage.TAOWImage)Sender;
                    var call__93 = Aow.Rtl.Rtl.Rect(image.WinLeft + 29, image.WinTop + 28, image.WinLeft + 132, image.WinTop + 156);
                    R = call__93;

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

                var image = (Aow.Units.AoWImage.TAOWImage)Sender;
                Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.ShowEx(image.WinLeft + 49, image.WinTop + 78, SurfDesc, FShowInfo);
            }
        }

        // AoWUnitWindow.pas:855
        public void Gnd3MouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            // { Tried typed checkbox Sender; no exact match. }
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit != null)
            {
                var call__95 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.FPlayer);
                var call__96 = call__95.GetBusy();
                if (!call__96)
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
                var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                var call__99 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin[24];
                call__99.ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[FHighlight]);
            }
        }

        // AoWUnitWindow.pas:881
        public void TorsoDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Gnd1Draw(Sender, SurfDesc);
            if (((Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem != null) && (Sender != Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem)) && (FItemType == ((Aow.Units.AoWE.TItemType)1)))
            {
                var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                var call__102 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin[24];
                call__102.ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[FHighlight]);
            }
        }

        // AoWUnitWindow.pas:891
        public void AttackDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Gnd1Draw(Sender, SurfDesc);
            if (((Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem != null) && (Sender != Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem)) && (FItemType == ((Aow.Units.AoWE.TItemType)2)))
            {
                var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                var call__105 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin[24];
                call__105.ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[FHighlight]);
            }
        }

        // AoWUnitWindow.pas:901
        public void DefendDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Gnd1Draw(Sender, SurfDesc);
            if (((Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem != null) && (Sender != Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem)) && (FItemType == ((Aow.Units.AoWE.TItemType)3)))
            {
                var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                var call__108 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin[24];
                call__108.ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[FHighlight]);
            }
        }

        // AoWUnitWindow.pas:911
        public void RingLDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Gnd1Draw(Sender, SurfDesc);
            if (((Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem != null) && (Sender != Aow.Units.AoWUnitWindow.AoWUnitWindow.SelectedItem)) && (FItemType == ((Aow.Units.AoWE.TItemType)4)))
            {
                var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                var call__111 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin[24];
                call__111.ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[FHighlight]);
            }
        }

        // AoWUnitWindow.pas:921
        public void DisbandBtnClick()
        {
            var call__112 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.DisbandUnitRStr);
            var call__113 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.ConfirmRStr);
            Aow.Units.E2Globals.E2Globals.DialogWin.Confirm(call__113, call__112, 7);
        }

        // AoWUnitWindow.pas:926
        // { draft }
        public void T1AbilityLBMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.AoWE.TAbility A = default;
            if (((Button == Aow.Rtl.TMouseButton.mbRight) && (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit != null)) && (T1AbilityLB.Selected != -1))
            {
                var call__114 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                var call__115 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__114));
                A = call__115;
                if (A != null)
                {
                    var call__116 = A.GetName(null);
                    Aow.Units.AoWExeForms.AoWExeForms.SetInfoBanner(Aow.Units.AoWInfoBanner.AoWInfoBanner.InfoBanner, call__116, A.FDescription);
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
                var call__117 = Aow.Units.AoWWinMan.AoWWinMan.General.IndexOfUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                I = call__117;
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

                    var call__118 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[I]);
                    Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit = call__118;
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
                var call__119 = Aow.Units.AoWWinMan.AoWWinMan.General.IndexOfUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                I = call__119;
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

                    var call__120 = Aow.Units.AoWWinMan.AoWWinMan.General.FUnitList.GetUnit(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnits[I]);
                    Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit = call__120;
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
            var call__121 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(O1));
            A1 = call__121;
            var call__122 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(O2));
            A2 = call__122;
            var call__123 = A1.GetEnabled(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
            bool bool__124 = call__123;
            if (bool__124)
            {
                var call__125 = A1.GetControlType(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                bool__124 = call__125.Contains(Aow.Units.AoWE.TAbilityControlTypeElement.acAction);
            }

            if (bool__124)
            {
                V1 = 10;
            }
            else
            {
                V1 = 0;
            }

            var call__126 = A2.GetEnabled(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
            bool bool__127 = call__126;
            if (bool__127)
            {
                var call__128 = A2.GetControlType(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                bool__127 = call__128.Contains(Aow.Units.AoWE.TAbilityControlTypeElement.acAction);
            }

            if (bool__127)
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
