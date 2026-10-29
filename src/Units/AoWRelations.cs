// Translated from AoWRelations.pas.
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
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWHScroll;
using static Aow.Units.AoWHScroll.AoWHScroll;
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
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.aowIntGfxMod;
using static Aow.Units.aowIntGfxMod.aowIntGfxMod;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;

namespace Aow.Units.AoWRelations
{
    // { TRelWindow (size $2A8, VMT 00423988): $280 diplomatic actions, $288 player indices, $28C race indices, $290 controls. }
    public partial class TRelWindow : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow RelWin;
        // { +0044 }
        public Aow.Units.AoWImage.TAOWImage RShield;
        // { +0048 }
        public Aow.Units.AoWImage.TAOWImage LShield;
        // { +004C }
        public Aow.Units.AoWButton.TAOWButton MExitBtn;
        // { +0050 }
        public Aow.Units.AoWPanel.TAOWPanel RelPnl;
        // { +0054 }
        public Aow.Units.AoWPanel.TAOWPanel RIncomingPnl;
        // { +0058 }
        public Aow.Units.AoWButton.TAOWButton MAcceptBtn;
        // { +005C }
        public Aow.Units.AoWButton.TAOWButton MRejectBtn;
        // { +0060 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar MScroll;
        // { +0064 }
        public Aow.Units.AoWImage.TAOWImage P1I;
        // { +0068 }
        public Aow.Units.AoWImage.TAOWImage P2I;
        // { +006C }
        public Aow.Units.AoWImage.TAOWImage P3I;
        // { +0070 }
        public Aow.Units.AoWImage.TAOWImage P4I;
        // { +0074 }
        public Aow.Units.AoWImage.TAOWImage P5I;
        // { +0078 }
        public Aow.Units.AoWImage.TAOWImage P6I;
        // { +007C }
        public Aow.Units.AoWImage.TAOWImage P7I;
        // { +0080 }
        public Aow.Units.AoWImage.TAOWImage P8I;
        // { +0084 }
        public Aow.Units.AoWImage.TAOWImage P9I;
        // { +0088 }
        public Aow.Units.AoWImage.TAOWImage P10I;
        // { +008C }
        public Aow.Units.AoWImage.TAOWImage P11I;
        // { +0090 }
        public Aow.Units.AoWImage.TAOWImage P12I;
        // { +0094 }
        public Aow.Units.AoWImage.TAOWImage R1I;
        // { +0098 }
        public Aow.Units.AoWImage.TAOWImage R2I;
        // { +009C }
        public Aow.Units.AoWImage.TAOWImage R3I;
        // { +00A0 }
        public Aow.Units.AoWImage.TAOWImage R4I;
        // { +00A4 }
        public Aow.Units.AoWImage.TAOWImage R5I;
        // { +00A8 }
        public Aow.Units.AoWImage.TAOWImage R6I;
        // { +00AC }
        public Aow.Units.AoWImage.TAOWImage R7I;
        // { +00B0 }
        public Aow.Units.AoWImage.TAOWImage R8I;
        // { +00B4 }
        public Aow.Units.AoWImage.TAOWImage R9I;
        // { +00B8 }
        public Aow.Units.AoWImage.TAOWImage R10I;
        // { +00BC }
        public Aow.Units.AoWImage.TAOWImage R11I;
        // { +00C0 }
        public Aow.Units.AoWImage.TAOWImage R12I;
        // { +00C4 }
        public Aow.Units.AoWImage.TAOWImage FaceShadow;
        // { +00C8 }
        public Aow.Units.AoWMemo.TAOWMemo MMemo;
        // { +00CC }
        public Aow.Units.AoWLabel.TAOWLabel MBGoldLbl;
        // { +00D0 }
        public Aow.Units.AoWImage.TAOWImage MIcon;
        // { +00D4 }
        public Aow.Units.AoWPanel.TAOWPanel RMsgEditPnl;
        // { +00D8 }
        public Aow.Units.AoWPanel.TAOWPanel RSBribePnl;
        // { +00DC }
        public Aow.Units.AoWVScroll.TAOWVScrollBar SScroll;
        // { +00E0 }
        public Aow.Units.AoWMemo.TAOWMemo SMemo;
        // { +00E4 }
        public Aow.Units.AoWHScroll.TAOWHScrollBar SBGoldSlid;
        // { +00E8 }
        public Aow.Units.AoWPanel.TAOWPanel ActionPnl;
        // { +00EC }
        public Aow.Units.AoWPanel.TAOWPanel LInfoPlayerPnl;
        // { +00F0 }
        public Aow.Units.AoWPanel.TAOWPanel RNewActionPnl;
        // { +00F4 }
        public Aow.Units.AoWButton.TAOWButton AExit;
        // { +00F8 }
        public Aow.Units.AoWLabel.TAOWLabel APlayer;
        // { +00FC }
        public Aow.Units.AoWPanel.TAOWPanel LInfoRacePnl;
        // { +0100 }
        public Aow.Units.AoWLabel.TAOWLabel ARace2;
        // { +0104 }
        public Aow.Units.AoWImage.TAOWImage AAlignment;
        // { +0108 }
        public Aow.Units.AoWLabel.TAOWLabel AAlignmentLbl;
        // { +010C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P1CB;
        // { +0110 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P2CB;
        // { +0114 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P3CB;
        // { +0118 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P4CB;
        // { +011C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P5CB;
        // { +0120 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P6CB;
        // { +0124 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P7CB;
        // { +0128 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P8CB;
        // { +012C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P9CB;
        // { +0130 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P10CB;
        // { +0134 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P11CB;
        // { +0138 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox P12CB;
        // { +013C }
        public Aow.Units.AoWImage.TAOWImage FaceFr;
        // { +0140 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox R1CB;
        // { +0144 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox R2CB;
        // { +0148 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox R3CB;
        // { +014C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox R4CB;
        // { +0150 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox R5CB;
        // { +0154 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox R6CB;
        // { +0158 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox R7CB;
        // { +015C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox R8CB;
        // { +0160 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox R9CB;
        // { +0164 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox R10CB;
        // { +0168 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox R11CB;
        // { +016C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox R12CB;
        // { +0170 }
        public Aow.Units.AoWImage.TAOWImage PlyrGlow;
        // { +0174 }
        public Aow.Units.AoWLabel.TAOWLabel NoContact;
        // { +0178 }
        public Aow.Units.AoWButton.TAOWButton ASelect;
        // { +017C }
        public Aow.Units.AoWListBox.TAOWListBox ActionsLB;
        // { +0180 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar ActionSB;
        // { +0184 }
        public Aow.Units.AoWPanel.TAOWPanel LBActionsPnl;
        // { +0188 }
        public Aow.Units.AoWLabel.TAOWLabel ActionLbl;
        // { +018C }
        public Aow.Units.AoWImage.TAOWImage PIRelIcn;
        // { +0190 }
        public Aow.Units.AoWLabel.TAOWLabel PIRelLbl;
        // { +0194 }
        public Aow.Units.AoWPanel.TAOWPanel PIRelPnl;
        // { +0198 }
        public Aow.Units.AoWLabel.TAOWLabel PIRelV;
        // { +019C }
        public Aow.Units.AoWImage.TAOWImage APlyrIcon;
        // { +01A0 }
        public Aow.Units.AoWLabel.TAOWLabel APlyrType;
        // { +01A4 }
        public Aow.Units.AoWImage.TAOWImage PlStatIcn;
        // { +01A8 }
        public Aow.Units.AoWLabel.TAOWLabel PlStatLbl;
        // { +01AC }
        public Aow.Units.AoWPanel.TAOWPanel PIStatPnl;
        // { +01B0 }
        public Aow.Units.AoWLabel.TAOWLabel PlStatV;
        // { +01B4 }
        public Aow.Units.AoWPanel.TAOWPanel PTypePnl;
        // { +01B8 }
        public Aow.Units.AoWPanel.TAOWPanel PIFrame;
        // { +01BC }
        public Aow.Units.AoWPanel.TAOWPanel RIFrame;
        // { +01C0 }
        public Aow.Units.AoWPanel.TAOWPanel RIRelPnl;
        // { +01C4 }
        public Aow.Units.AoWPanel.TAOWPanel RAlignPnl;
        // { +01C8 }
        public Aow.Units.AoWImage.TAOWImage RlRelIcn;
        // { +01CC }
        public Aow.Units.AoWLabel.TAOWLabel RIRelLbl;
        // { +01D0 }
        public Aow.Units.AoWLabel.TAOWLabel RlRelV;
        // { +01D4 }
        public Aow.Units.AoWLabel.TAOWLabel MBGoldBribeV;
        // { +01D8 }
        public Aow.Units.AoWImage.TAOWImage MBGoldIcn;
        // { +01DC }
        public Aow.Units.AoWPanel.TAOWPanel MBGoldPnl;
        // { +01E0 }
        public Aow.Units.AoWPanel.TAOWPanel MBManaPnl;
        // { +01E4 }
        public Aow.Units.AoWLabel.TAOWLabel MBManaLbl;
        // { +01E8 }
        public Aow.Units.AoWLabel.TAOWLabel MBManaBribeV;
        // { +01EC }
        public Aow.Units.AoWImage.TAOWImage MBManaIcn;
        // { +01F0 }
        public Aow.Units.AoWLabel.TAOWLabel MsgLbl;
        // { +01F4 }
        public Aow.Units.AoWButton.TAOWButton MsgSendBtn;
        // { +01F8 }
        public Aow.Units.AoWButton.TAOWButton MsgCancelBtn;
        // { +01FC }
        public Aow.Units.AoWFrame.TAoWFrame RSBribe2;
        // { +0200 }
        public Aow.Units.AoWLabel.TAOWLabel SBGoldV;
        // { +0204 }
        public Aow.Units.AoWImage.TAOWImage SBGoldIcn;
        // { +0208 }
        public Aow.Units.AoWPanel.TAOWPanel SBGold;
        // { +020C }
        public Aow.Units.AoWPanel.TAOWPanel SBMana;
        // { +0210 }
        public Aow.Units.AoWLabel.TAOWLabel SBManaV;
        // { +0214 }
        public Aow.Units.AoWImage.TAOWImage SBManaIcn;
        // { +0218 }
        public Aow.Units.AoWFrame.TAoWFrame RSBSep1;
        // { +021C }
        public Aow.Units.AoWLabel.TAOWLabel SBribeBtn;
        // { +0220 }
        public Aow.Units.AoWHScroll.TAOWHScrollBar SBManaSlid;
        // { +0224 }
        public Aow.Units.AoWImage.TAOWImage SBIcon;
        // { +0228 }
        public Aow.Units.AoWButton.TAOWButton RSBSendBtn;
        // { +022C }
        public Aow.Units.AoWButton.TAOWButton RBSCanlBtn;
        // { +0230 }
        public Aow.Units.AoWImage.TAOWImage StatusGlow;
        // { +0234 }
        public Aow.Units.AoWLabel.TAOWLabel BribeHeaderLbl;
        // { +0238 }
        public Aow.Units.AoWPanel.TAOWPanel RMsgRecPnl;
        // { +023C }
        public Aow.Units.AoWMemo.TAOWMemo RMMemo;
        // { +0240 }
        public Aow.Units.AoWLabel.TAOWLabel RMHeaderLbl;
        // { +0244 }
        public Aow.Units.AoWButton.TAOWButton RMReplyBtn;
        // { +0248 }
        public Aow.Units.AoWButton.TAOWButton RMCancelBtn;
        // { +024C }
        public Aow.Units.AoWButton.TAOWButton RMExitBtn;
        // { +0250 }
        public Aow.Units.AoWPanel.TAOWPanel IncommingInfoPnl;
        // { +0254 }
        public Aow.Units.AoWButton.TAOWButton IIOkBtn;
        // { +0258 }
        public Aow.Units.AoWButton.TAOWButton IIExitBtn;
        // { +025C }
        public Aow.Units.AoWVScroll.TAOWVScrollBar IRScroll;
        // { +0260 }
        public Aow.Units.AoWMemo.TAOWMemo IIMemo;
        // { +0264 }
        public Aow.Units.AoWImage.TAOWImage IIIcon;
        // { +0268 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +026C }
        public Aow.Units.AoWLabel.TAOWLabel OfTheLbl;
        // { +0270 }
        public Aow.Units.AoWLabel.TAOWLabel PRaceLbl;
        // { +0274 }
        public int FSelectedIndex;
        // { $278 }
        public Aow.Units.AoWE.TDiplomaticAction FAction;
        // { $27C }
        public Aow.Units.AoWE.TDiplomaticActionList FActions;
        // { $280 }
        public sbyte FInfoPlayer;
        public sbyte FInfoRace;
        // { $284, $285 }
        public sbyte FSelectedPlayer;
        // { $286 }
        public sbyte FSelectedRace;
        // { $287 }
        public Aow.Units.Engine.TIntegerList FPlayers;
        // { $288 }
        public Aow.Units.Engine.TIntegerList FRaces;
        // { $28C }
        public Aow.Rtl.TList FControls;
        // { $290 }
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        // { $294 }
        public Aow.Units.AoWE.TDiplomaticActionEventLog FEvent;
        // { $29C }
        public int FGlowIntensity;
        // { $2A0 }
        public bool FGlowIncreasing;
        // { $2A4 }
        public Aow.Units.FixedArray_byte_3 F_RelWindow_2A5;
        // AoWRelations.pas:221
        public void FillPlayers()
        {
            int I = default;
            FPlayers.Clear();
            {
                I = 1;
                int end__1 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetCount() - 1;
                for (; I <= end__1; I += 1)
                {
                    var call__2 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    var call__3 = call__2.GetFirstDaySeen(unchecked((sbyte)I));
                    bool bool__4 = (call__3 != -1);
                    if (bool__4)
                    {
                        var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                        bool__4 = (call__5.FStatus == Aow.Units.AoWE.TPlayerStatus.psPlaying);
                    }

                    if (bool__4)
                    {
                        FPlayers.Add(I);
                    }
                }
            }

            {
                I = 1;
                byte end__6 = 12;
                for (; I <= end__6; I += 1)
                {
                    if (I <= FPlayers.Count)
                    {
                        var call__7 = FControls[I + 23];
                        ((Aow.Units.AoWCheckBox.TAOWCheckBox)call__7).Visible = true;
                        var call__8 = FControls[I + 23];
                        ((Aow.Units.AoWCheckBox.TAOWCheckBox)call__8).ToolTipText.Clear();
                        var call__9 = FPlayers[I - 1];
                        var call__10 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__9);
                        var call__11 = call__10.GetName();
                        var call__12 = FControls[I + 23];
                        ((Aow.Units.AoWCheckBox.TAOWCheckBox)call__12).ToolTipText.Add(call__11);
                    }
                    else
                    {
                        var call__13 = FControls[I + 23];
                        ((Aow.Units.AoWCheckBox.TAOWCheckBox)call__13).Visible = false;
                    }
                }
            }

            var call__14 = FPlayers.IndexOf(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            PlyrGlow.Alignment.LeftOffset = ((call__14 * 51) - 37) + 51;
            PlyrGlow.FAligned = false;
        }

        // AoWRelations.pas:241
        public void ShowRelations(int Player)
        {
            int I = default;
            int Index = default;
            ClearActions();
            ResetChecks();
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 49;
            FRaces.Clear();
            {
                I = 0;
                int end__15 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetCount() - 1;
                for (; I <= end__15; I += 1)
                {
                    var call__16 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(I);
                    if (call__16.FEnabled)
                    {
                        FRaces.Add(I);
                    }
                }
            }

            {
                I = 0;
                byte end__17 = 11;
                for (; I <= end__17; I += 1)
                {
                    if (I < FRaces.Count)
                    {
                        var call__18 = FControls[I + 36];
                        ((Aow.Units.AoWCheckBox.TAOWCheckBox)call__18).Visible = true;
                        var call__19 = FControls[I + 36];
                        ((Aow.Units.AoWCheckBox.TAOWCheckBox)call__19).ToolTipText.Clear();
                        var call__20 = FRaces[I];
                        var call__21 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(call__20);
                        var call__22 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[call__21.FRace]);
                        var call__23 = FControls[I + 36];
                        ((Aow.Units.AoWCheckBox.TAOWCheckBox)call__23).ToolTipText.Add(call__22);
                    }
                    else
                    {
                        var call__24 = FControls[I + 36];
                        ((Aow.Units.AoWCheckBox.TAOWCheckBox)call__24).Visible = false;
                    }
                }
            }

            FillPlayers();
            FGlowIntensity = 50;
            FGlowIncreasing = false;
            RelWin.Visible = true;
            var call__25 = FControls[Player + 23];
            ((Aow.Units.AoWCheckBox.TAOWCheckBox)call__25).Checked = true;
            var call__26 = FPlayers.IndexOf(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            Index = call__26;
            var call__27 = FControls[Index + 24];
            ((Aow.Units.AoWCheckBox.TAOWCheckBox)call__27).Checked = true;
            var call__28 = FControls[Index + 24];
            P1CBChange((Aow.Units.AoWCheckBox.TAOWCheckBox)call__28);
            Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(65, true, true);
        }

        // AoWRelations.pas:270
        public void ResetChecks()
        {
            int I = default;
            Aow.Units.AoWCheckBox.TAOWCheckBox Check = default;
            FSelectedIndex = -1;
            RelPnl.FAligned = false;
            RelWin.FDrawn = false;
            PlyrGlow.Alignment.TopOffset = 10;
            {
                I = 24;
                byte end__29 = 35;
                for (; I <= end__29; I += 1)
                {
                    var call__30 = FControls[I];
                    Check = (Aow.Units.AoWCheckBox.TAOWCheckBox)call__30;
                    Check.Checked = false;
                    Check.Alignment.TopOffset = 10;
                }
            }

            {
                I = 36;
                byte end__31 = 47;
                for (; I <= end__31; I += 1)
                {
                    var call__32 = FControls[I];
                    Check = (Aow.Units.AoWCheckBox.TAOWCheckBox)call__32;
                    Check.Checked = false;
                    Check.Alignment.TopOffset = 185;
                }
            }
        }

        // AoWRelations.pas:293
        public void ShowPlayerActions(sbyte Player)
        {
            int Selected = default;
            int I = default;
            ClearActions();
            HighlightPlayer(unchecked((sbyte)Player));
            ShowPlayerInformation(unchecked((sbyte)Player));
            Selected = ActionsLB.Selected;
            var call__33 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            call__33.ListDiplomaticActions(unchecked((sbyte)Player), ref FActions);
            {
                I = 0;
                int end__34 = FActions.GetCount() - 1;
                for (; I <= end__34; I += 1)
                {
                    var call__35 = FActions.GetDiplomaticActions(I);
                    var call__36 = call__35.GetName(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    var call__37 = FActions.GetDiplomaticActions(I);
                    ActionsLB.Items.AddObject(call__36, call__37);
                }
            }

            ActionsLB.Selected = Selected;
            ActionsLBChange(this);
            ActionPnl.Visible = true;
        }

        // AoWRelations.pas:309
        public void ShowPlayerInformation(sbyte Player)
        {
            Aow.Units.AoWE.TDiplomaticRelation Relation = default;
            FInfoPlayer = unchecked((sbyte)Player);
            FInfoRace = unchecked((sbyte)-1);
            FaceShadow.Visible = true;
            NoContact.Visible = false;
            PlStatV.Visible = true;
            PlStatIcn.Visible = true;
            PlStatLbl.Visible = true;
            LInfoPlayerPnl.Visible = true;
            LInfoRacePnl.Visible = false;
            var call__38 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            LShield.CurrentImage = call__38.FPlayerRace + 26;
            LShield.Visible = true;
            RShield.CurrentImage = LShield.CurrentImage;
            RShield.Visible = true;
            var call__39 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__40 = call__39.GetFirstDaySeen(unchecked((sbyte)Player));
            NoContact.Visible = call__40 == -1;
            APlayer.Visible = !NoContact.Visible;
            OfTheLbl.Visible = APlayer.Visible;
            PRaceLbl.Visible = APlayer.Visible;
            var call__41 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            var call__42 = call__41.GetFirstDaySeen(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            if (call__42 != -1)
            {
                if (Player != Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
                {
                    var call__43 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetDiplomaticRelation(unchecked((sbyte)Player), unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                    Relation = call__43;
                    var call__44 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TDiplomaticRelationRStr[(int)Relation]);
                    PlStatV.Text = call__44;
                    if (Relation != Aow.Units.AoWE.TDiplomaticRelation.drUnknown)
                    {
                        PlStatIcn.CurrentImage = ((int)Relation) + 8;
                    }
                    else
                    {
                        PlStatIcn.CurrentImage = 50;
                    }
                }
                else
                {
                    PlStatIcn.Visible = false;
                    PlStatLbl.Visible = false;
                    PlStatV.Visible = false;
                }
            }
            else
            {
                PlStatIcn.Visible = false;
                var call__45 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoContactRStr);
                PlStatV.Text = call__45;
            }

            var call__46 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            var call__47 = call__46.GetName();
            APlayer.Text = call__47;
            OfTheLbl.Text = Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.LeadL.Text;
            var call__48 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[call__48.FPlayerRace]);
            PRaceLbl.Text = call__49;
            var call__50 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            if (TSet<Aow.Units.AoWE.TPlayerType>.Of(Aow.Units.AoWE.TPlayerType.ptHuman, Aow.Units.AoWE.TPlayerType.ptCPU1, Aow.Units.AoWE.TPlayerType.ptCPU2, Aow.Units.AoWE.TPlayerType.ptCPU3, Aow.Units.AoWE.TPlayerType.ptCPU4, Aow.Units.AoWE.TPlayerType.ptCPU5).Contains(call__50.FPlayerType))
            {
                var call__51 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                switch (call__51.FPlayerType)
                {
                    case Aow.Units.AoWE.TPlayerType.ptCPU1:
                        APlyrIcon.CurrentImage = 1;
                        break;
                    case Aow.Units.AoWE.TPlayerType.ptCPU2:
                        APlyrIcon.CurrentImage = 2;
                        break;
                    case Aow.Units.AoWE.TPlayerType.ptCPU3:
                        APlyrIcon.CurrentImage = 3;
                        break;
                    case Aow.Units.AoWE.TPlayerType.ptCPU4:
                        APlyrIcon.CurrentImage = 4;
                        break;
                    case Aow.Units.AoWE.TPlayerType.ptCPU5:
                        APlyrIcon.CurrentImage = 5;
                        break;
                    default:
                        APlyrIcon.CurrentImage = 0;
                        break;
                }

                APlyrIcon.Visible = true;
            }
            else
            {
                APlyrIcon.Visible = false;
            }

            var call__52 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            var call__53 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPlayerTypeRStr[(int)call__52.FPlayerType]);
            APlyrType.Text = call__53;
        }

        // AoWRelations.pas:370
        public void HighlightPlayer(sbyte Player)
        {
            int I = default;
            Aow.Units.AoWCheckBox.TAOWCheckBox Check = default;
            ResetChecks();
            FSelectedPlayer = unchecked((sbyte)Player);
            FSelectedRace = unchecked((sbyte)-1);
            var call__54 = FPlayers.IndexOf(Player);
            FSelectedIndex = call__54;
            if (FSelectedIndex != -1)
            {
                var call__55 = FControls[FSelectedIndex + 24];
                Check = (Aow.Units.AoWCheckBox.TAOWCheckBox)call__55;
                Check.Alignment.TopOffset = 20;
                Check.Checked = true;
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Player)
                {
                    PlyrGlow.Alignment.TopOffset = Check.Alignment.TopOffset;
                }
            }

            {
                I = 0;
                byte end__56 = 11;
                for (; I <= end__56; I += 1)
                {
                    if (I < FPlayers.Count)
                    {
                        var call__57 = FControls[I];
                        ((Aow.Units.AoWImage.TAOWImage)call__57).Visible = true;
                        var call__58 = FPlayers[I];
                        if (call__58 == Player)
                        {
                            var call__59 = FControls[I];
                            ((Aow.Units.AoWImage.TAOWImage)call__59).CurrentImage = 2;
                        }
                        else
                        {
                            if (I > FRaces.Count)
                            {
                                var call__60 = FControls[I];
                                ((Aow.Units.AoWImage.TAOWImage)call__60).CurrentImage = 0;
                            }
                            else
                            {
                                var call__61 = FControls[I];
                                ((Aow.Units.AoWImage.TAOWImage)call__61).CurrentImage = 1;
                            }
                        }
                    }
                    else
                    {
                        var call__62 = FControls[I];
                        ((Aow.Units.AoWImage.TAOWImage)call__62).CurrentImage = 2;
                    }

                    var call__63 = FControls[I];
                    switch (((Aow.Units.AoWImage.TAOWImage)call__63).CurrentImage)
                    {
                        case 1:
                            var call__64 = FControls[I];
                            ((Aow.Units.AoWImage.TAOWImage)call__64).ToolTipText.Clear();
                            if (FSelectedPlayer != -1)
                            {
                                var call__65 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                                var call__66 = call__65.GetFirstDaySeen(unchecked((sbyte)FSelectedPlayer));
                                if (call__66 != -1)
                                {
                                    var call__67 = FPlayers[I];
                                    var call__68 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                                    var call__69 = call__68.GetFirstDaySeen(unchecked((sbyte)call__67));
                                    if (call__69 != -1)
                                    {
                                        var call__70 = FPlayers[I];
                                        var call__71 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FSelectedPlayer);
                                        var call__72 = call__71.GetFirstDaySeen(unchecked((sbyte)call__70));
                                        if (call__72 != -1)
                                        {
                                            var call__73 = FPlayers[I];
                                            var call__74 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetDiplomaticRelation(unchecked((sbyte)FSelectedPlayer), unchecked((sbyte)call__73));
                                            var call__75 = FControls[I];
                                            ((Aow.Units.AoWImage.TAOWImage)call__75).ToolTipText.Add(Aow.Units.AoWE.AoWE.TDiplomaticRelationRStr[(int)call__74]);
                                        }
                                    }
                                }
                            }

                            break;
                        default:
                            var call__76 = FControls[I];
                            ((Aow.Units.AoWImage.TAOWImage)call__76).ToolTipText.Clear();
                            break;
                    }
                }
            }

            {
                I = 0;
                byte end__77 = 11;
                for (; I <= end__77; I += 1)
                {
                    if (I < FRaces.Count)
                    {
                        var call__78 = FControls[I + 12];
                        ((Aow.Units.AoWImage.TAOWImage)call__78).CurrentImage = 4;
                    }
                    else
                    {
                        var call__79 = FControls[I + 12];
                        ((Aow.Units.AoWImage.TAOWImage)call__79).CurrentImage = 5;
                    }

                    var call__80 = FControls[I + 12];
                    switch (((Aow.Units.AoWImage.TAOWImage)call__80).CurrentImage)
                    {
                        case 4:
                            var call__81 = FControls[I + 12];
                            ((Aow.Units.AoWImage.TAOWImage)call__81).ToolTipText.Clear();
                            if (FSelectedPlayer != -1)
                            {
                                var call__82 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                                var call__83 = call__82.GetFirstDaySeen(unchecked((sbyte)FSelectedPlayer));
                                if (call__83 != -1)
                                {
                                    var call__84 = FRaces[I];
                                    var call__85 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(call__84);
                                    var call__86 = call__85.GetPlayerRelation(unchecked((sbyte)FSelectedPlayer));
                                    var call__87 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TStatusRStr[(int)call__86]);
                                    var call__88 = FRaces[I];
                                    var call__89 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(call__88);
                                    var call__90 = call__89.GetPlayerRelationValue(unchecked((sbyte)FSelectedPlayer));
                                    var call__91 = FControls[12 + I];
                                    ((Aow.Units.AoWImage.TAOWImage)call__91).ToolTipText.Add(((call__87 + ((AnsiString)" (")) + Aow.Rtl.Rtl.IntToStr(call__90)) + AnsiString.FromBytes(41));
                                }
                            }

                            break;
                        default:
                            var call__92 = FControls[I + 12];
                            ((Aow.Units.AoWImage.TAOWImage)call__92).ToolTipText.Clear();
                            break;
                    }
                }
            }
        }

        // AoWRelations.pas:427
        public void ShowRaceInformation(byte Race)
        {
            FInfoPlayer = unchecked((sbyte)-1);
            FInfoRace = unchecked((sbyte)Race);
            LShield.Visible = false;
            RShield.Visible = false;
            FaceShadow.Visible = true;
            ActionPnl.Visible = true;
            LInfoPlayerPnl.Visible = false;
            LInfoRacePnl.Visible = true;
            var call__93 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(unchecked((sbyte)Race));
            var call__94 = call__93.GetPlayerRelation(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            RlRelIcn.CurrentImage = ((int)call__94) + 11;
            var call__95 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(unchecked((sbyte)Race));
            var call__96 = call__95.GetPlayerRelation(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            var call__97 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TStatusRStr[(int)call__96]);
            RlRelV.Text = call__97;
            var call__98 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[unchecked((sbyte)Race)]);
            ARace2.Text = call__98;
            switch (Race)
            {
                case >= 0 and <= 3:
                    var call__99 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NeutralRStr);
                    var call__100 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AlignmentXRStr);
                    var call__101 = Aow.Units.AoWE.AoWE.Format(call__100, [call__99]);
                    AAlignmentLbl.Text = call__101;
                    AAlignment.CurrentImage = 33;
                    break;
                case >= 4 and <= 7:
                    var call__102 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GoodRStr);
                    var call__103 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AlignmentXRStr);
                    var call__104 = Aow.Units.AoWE.AoWE.Format(call__103, [call__102]);
                    AAlignmentLbl.Text = call__104;
                    AAlignment.CurrentImage = 31;
                    break;
                case >= 8 and <= 11:
                    var call__105 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EvilRStr);
                    var call__106 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AlignmentXRStr);
                    var call__107 = Aow.Units.AoWE.AoWE.Format(call__106, [call__105]);
                    AAlignmentLbl.Text = call__107;
                    AAlignment.CurrentImage = 35;
                    break;
            }
        }

        // AoWRelations.pas:456
        public void HighlightRace(sbyte Race)
        {
            int I = default;
            int Index = default;
            Aow.Units.AoWCheckBox.TAOWCheckBox Check = default;
            ResetChecks();
            FSelectedPlayer = unchecked((sbyte)-1);
            FSelectedRace = unchecked((sbyte)Race);
            var call__108 = FRaces.IndexOf(Race);
            Index = call__108;
            var call__109 = FControls[Index + 36];
            Check = (Aow.Units.AoWCheckBox.TAOWCheckBox)call__109;
            Check.Alignment.TopOffset = 175;
            Check.Checked = true;
            FSelectedIndex = Check.Tag;
            {
                I = 0;
                byte end__110 = 11;
                for (; I <= end__110; I += 1)
                {
                    if (I < FPlayers.Count)
                    {
                        var call__111 = FControls[I];
                        ((Aow.Units.AoWImage.TAOWImage)call__111).CurrentImage = 1;
                    }
                    else
                    {
                        var call__112 = FControls[I];
                        ((Aow.Units.AoWImage.TAOWImage)call__112).CurrentImage = 2;
                    }

                    var call__113 = FControls[I];
                    switch (((Aow.Units.AoWImage.TAOWImage)call__113).CurrentImage)
                    {
                        case 1:
                            var call__114 = FControls[I];
                            ((Aow.Units.AoWImage.TAOWImage)call__114).ToolTipText.Clear();
                            var call__115 = FPlayers[I];
                            var call__116 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                            var call__117 = call__116.GetFirstDaySeen(unchecked((sbyte)call__115));
                            if (call__117 != -1)
                            {
                                var call__118 = FPlayers[I];
                                var call__119 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FSelectedRace);
                                var call__120 = call__119.GetPlayerRelation(unchecked((sbyte)call__118));
                                var call__121 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TStatusRStr[(int)call__120]);
                                var call__122 = FPlayers[I];
                                var call__123 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FSelectedRace);
                                var call__124 = call__123.GetPlayerRelationValue(unchecked((sbyte)call__122));
                                var call__125 = FControls[I];
                                ((Aow.Units.AoWImage.TAOWImage)call__125).ToolTipText.Add(((call__121 + ((AnsiString)" (")) + Aow.Rtl.Rtl.IntToStr(call__124)) + AnsiString.FromBytes(41));
                            }

                            break;
                        default:
                            var call__126 = FControls[I];
                            ((Aow.Units.AoWImage.TAOWImage)call__126).ToolTipText.Clear();
                            break;
                    }
                }
            }

            {
                I = 0;
                byte end__127 = 11;
                for (; I <= end__127; I += 1)
                {
                    var call__128 = FControls[I + 12];
                    ((Aow.Units.AoWImage.TAOWImage)call__128).CurrentImage = 5;
                    var call__129 = FControls[I + 12];
                    ((Aow.Units.AoWImage.TAOWImage)call__129).ToolTipText.Clear();
                }
            }
        }

        // AoWRelations.pas:490
        public void SetAction(Aow.Units.AoWE.TDiplomaticAction Action)
        {
            if (Action != FAction)
            {
                if (FAction != null)
                {
                    FAction.Release();
                }

                FAction = Action;
                if (FAction != null)
                {
                    FAction.AddRef();
                }
            }
        }

        // AoWRelations.pas:500
        public void FinishEvent()
        {
            Aow.Units.EventLog.TExecuteDoneEvent Done = default;
            Aow.Units.AoWE.TDiplomaticActionEventLog Pending = default;
            Aow.Units.AoWE.TDiplomaticActionEventLog Event = default;
            Pending = FEvent;
            if (Pending != null)
            {
                Done = FDone;
                Event = Pending;
                FDone = null;
                FEvent = null;
                if (Done != null)
                {
                    Done(Event);
                }

                if (Event != null)
                {
                    Event.Release();
                }
            }
        }

        // AoWRelations.pas:516
        public void FinishDialog()
        {
            SetAction(null);
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
            FinishEvent();
            Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(71, true, true);
        }

        // AoWRelations.pas:524
        public void ClearActions()
        {
            SetAction(null);
            RNewActionPnl.Visible = true;
            RIncomingPnl.Visible = false;
            RMsgEditPnl.Visible = false;
            RSBribePnl.Visible = false;
            RMsgRecPnl.Visible = false;
            IncommingInfoPnl.Visible = false;
            FActions.Clear();
            ActionsLB.Items.Clear();
            SMemo.Items.Clear();
            MMemo.Items.Clear();
            ActionsLBChange(this);
        }

        // AoWRelations.pas:540
        public void RelWindowCreate()
        {
            var call__130 = Aow.Units.AoWE.TDiplomaticActionList.Create(null);
            FActions = call__130;
            var call__131 = new Aow.Units.Engine.TIntegerList();
            FRaces = call__131;
            var call__132 = new Aow.Units.Engine.TIntegerList();
            FPlayers = call__132;
            var call__133 = new Aow.Rtl.TList();
            FControls = call__133;
            FControls.Add(P1I);
            FControls.Add(P2I);
            FControls.Add(P3I);
            FControls.Add(P4I);
            FControls.Add(P5I);
            FControls.Add(P6I);
            FControls.Add(P7I);
            FControls.Add(P8I);
            FControls.Add(P9I);
            FControls.Add(P10I);
            FControls.Add(P11I);
            FControls.Add(P12I);
            FControls.Add(R1I);
            FControls.Add(R2I);
            FControls.Add(R3I);
            FControls.Add(R4I);
            FControls.Add(R5I);
            FControls.Add(R6I);
            FControls.Add(R7I);
            FControls.Add(R8I);
            FControls.Add(R9I);
            FControls.Add(R10I);
            FControls.Add(R11I);
            FControls.Add(R12I);
            FControls.Add(P1CB);
            FControls.Add(P2CB);
            FControls.Add(P3CB);
            FControls.Add(P4CB);
            FControls.Add(P5CB);
            FControls.Add(P6CB);
            FControls.Add(P7CB);
            FControls.Add(P8CB);
            FControls.Add(P9CB);
            FControls.Add(P10CB);
            FControls.Add(P11CB);
            FControls.Add(P12CB);
            FControls.Add(R1CB);
            FControls.Add(R2CB);
            FControls.Add(R3CB);
            FControls.Add(R4CB);
            FControls.Add(R5CB);
            FControls.Add(R6CB);
            FControls.Add(R7CB);
            FControls.Add(R8CB);
            FControls.Add(R9CB);
            FControls.Add(R10CB);
            FControls.Add(R11CB);
            FControls.Add(R12CB);
        }

        // AoWRelations.pas:596
        // { draft }
        public void RelWindowDestroy()
        {
            FActions.Free();
            FControls.Free();
            FPlayers.Free();
            FRaces.Free();
        }

        // AoWRelations.pas:604
        // { draft }
        public void ExitClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            RelWin.Visible = false;
        }

        // AoWRelations.pas:610
        // { draft }
        public void P1CBChange(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWCheckBox.TAOWCheckBox Check = default;
            Check = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
            if (Check.Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }

            RelPnl.FAligned = false;
            RelWin.FDrawn = false;
            if (Check.Checked)
            {
                if (FAction == null)
                {
                    var call__134 = FPlayers[Check.Tag];
                    ShowPlayerActions(unchecked((sbyte)call__134));
                }
                else
                {
                    var call__135 = FPlayers[Check.Tag];
                    HighlightPlayer(unchecked((sbyte)call__135));
                }
            }
        }

        // AoWRelations.pas:623
        public void R1CBChange(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWCheckBox.TAOWCheckBox Check = default;
            Check = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
            if (Check.Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }

            RelPnl.FAligned = false;
            if (FAction == null)
            {
                ClearActions();
                var call__136 = FRaces[Check.Tag - 20];
                HighlightRace(unchecked((sbyte)call__136));
                var call__137 = FRaces[Check.Tag - 20];
                ShowRaceInformation(unchecked((byte)call__137));
            }
            else
            {
                var call__138 = FRaces[Check.Tag - 20];
                HighlightRace(unchecked((sbyte)call__138));
            }
        }

        // AoWRelations.pas:639
        public void P1CBPreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            sbyte Player = default;
            Aow.Rtl.TRect R = default;
            Aow.Units.AoWComp.TAoWComponent Component = default;
            Component = Sender;
            if (Component.Tag < FPlayers.Count)
            {
                var call__140 = FPlayers[Component.Tag];
                Player = unchecked((sbyte)call__140);
                var call__141 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__142 = call__141.GetFirstDaySeen(unchecked((sbyte)Player));
                if (call__142 == -1)
                {
                    var call__143 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin[28];
                    call__143.ShowClipped(Component.WinLeft, Component.WinTop, Surface);
                }
                else
                {
                    var call__144 = Aow.Rtl.Rtl.Rect(Component.WinLeft + 1, Component.WinTop + 1, (Component.WinLeft + Component.WinWidth) - 1, (Component.WinTop + Component.WinHeight) - 1);
                    R = call__144;
                    var call__145 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    var call__146 = call__145.GetFace();
                    if (call__146 != null)
                    {
                        var call__147 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                        var call__148 = call__147.GetFace();
                        call__148.ShowStretched(R, Surface);
                    }
                }

                var call__149 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                var call__150 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin[call__149.FPlayerRace + 16];
                call__150.ShowClipped(Component.WinLeft, Component.WinTop, Surface);
            }
        }

        // AoWRelations.pas:659
        public void R1CBPreDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Units.AoWComp.TAoWComponent Control = default;
            sbyte Race = default;
            Aow.Rtl.TRect R = default;
            Aow.Units.AoWE.TDiplomaticAction Action = default;
            int Modifier = default;
            Control = (Aow.Units.AoWComp.TAoWComponent)Sender;
            if ((Control.Tag - 20) < FRaces.Count)
            {
                var call__152 = FRaces[Control.Tag - 20];
                Race = unchecked((sbyte)call__152);
                var call__153 = Aow.Rtl.Rtl.Rect(Control.WinLeft + 1, Control.WinTop + 1, (Control.WinLeft + Control.WinWidth) - 1, (Control.WinTop + Control.WinHeight) - 1);
                R = call__153;
                var call__154 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(Race);
                var call__155 = call__154.GetFace();
                call__155.ShowStretched(R, Surface);
                Action = FAction;
                if (((Action == null) && RNewActionPnl.Visible) && (ActionsLB.Selected != -1))
                {
                    var call__156 = ActionsLB.Items.Objects[ActionsLB.Selected];
                    Action = (Aow.Units.AoWE.TDiplomaticAction)call__156;
                }

                if (Action != null)
                {
                    var call__157 = Action.GetRelationModifier(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer), unchecked((sbyte)Race));
                    Modifier = call__157;
                    if (Modifier != 0)
                    {
                        Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10.Color = 16777215;
                        Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10.SetTextCentered(R.Left + ((R.Right - R.Left) / 2), R.Top + 50, Aow.Rtl.Rtl.IntToStr(Modifier), Surface);
                    }
                }
            }
        }

        // AoWRelations.pas:689
        public void P1IDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            int Player = default;
            Aow.Units.AoWE.TDiplomaticRelation Relation = default;
            int Image = default;
            var component = (Aow.Units.AoWComp.TAoWComponent)Sender;
            if ((FPlayers.Count > component.Tag) && (component.Tag != FSelectedIndex))
            {
                var call__159 = FPlayers[component.Tag];
                Player = call__159;
                if (FSelectedPlayer != -1)
                {
                    var call__160 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    var call__161 = call__160.GetFirstDaySeen(unchecked((sbyte)FSelectedPlayer));
                    if (call__161 != -1)
                    {
                        var call__162 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                        var call__163 = call__162.GetFirstDaySeen(unchecked((sbyte)Player));
                        if (call__163 != -1)
                        {
                            var call__164 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FSelectedPlayer);
                            var call__165 = call__164.GetFirstDaySeen(unchecked((sbyte)Player));
                            if (call__165 != -1)
                            {
                                var call__166 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetDiplomaticRelation(unchecked((sbyte)FSelectedPlayer), unchecked((sbyte)Player));
                                Relation = call__166;
                                if (Relation != Aow.Units.AoWE.TDiplomaticRelation.drUnknown)
                                {
                                    Image = ((int)Relation) + 8;
                                }
                                else
                                {
                                    Image = 50;
                                }

                                var call__167 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin[Image];
                                call__167.Show(component.WinLeft, component.WinTop + 8, Surface);
                            }
                        }
                    }
                }
                else
                {
                    var call__168 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    var call__169 = call__168.GetFirstDaySeen(unchecked((sbyte)Player));
                    if (call__169 != -1)
                    {
                        var call__170 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FSelectedRace);
                        var call__171 = call__170.GetPlayerRelation(unchecked((sbyte)Player));
                        var call__172 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin[((int)call__171) + 11];
                        call__172.ShowClipped(component.WinLeft, component.WinTop + 8, Surface);
                    }
                }
            }
        }

        // AoWRelations.pas:715
        public void R1IDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            int RaceIndex = default;
            Aow.Units.AoWComp.TAoWComponent S = default;
            S = Sender;
            if ((FSelectedPlayer != -1) && ((S.Tag - 20) < FRaces.Count))
            {
                var call__174 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__175 = call__174.GetFirstDaySeen(unchecked((sbyte)FSelectedPlayer));
                if (call__175 != -1)
                {
                    var call__176 = FRaces[S.Tag - 20];
                    RaceIndex = call__176;
                    var call__177 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(RaceIndex);
                    var call__178 = call__177.GetPlayerRelation(unchecked((sbyte)FSelectedPlayer));
                    var call__179 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin[((int)call__178) + 11];
                    call__179.ShowClipped(S.WinLeft, S.WinTop + 15, Surface);
                }
            }
        }

        // AoWRelations.pas:723
        public void MAcceptBtnClick()
        {
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWE.TDiplomaticAction Action = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Action = FAction;
            if (Action != null)
            {
                if (Action.Accept(ref Reason))
                {
                    FinishEvent();
                    ShowPlayerActions(unchecked((sbyte)FAction.FSource));
                }
                else
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(Reason);
                }
            }
        }

        // AoWRelations.pas:739
        // { draft }
        public void MRejectBtnClick()
        {
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWE.TDiplomaticAction Action = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Action = FAction;
            if (Action != null)
            {
                if (Action.Reject(ref Reason))
                {
                    FinishEvent();
                    ShowPlayerActions(unchecked((sbyte)FAction.FSource));
                }
                else
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(Reason);
                }
            }
        }

        // AoWRelations.pas:755
        // { draft }
        public void PlyrGlowUpdate()
        {
            if (FGlowIncreasing)
            {
                FGlowIntensity += 1;
            }
            else
            {
                FGlowIntensity -= 1;
            }

            if (FGlowIntensity == 20)
            {
                FGlowIncreasing = true;
            }
            else
            {
                if (FGlowIntensity == 50)
                {
                    FGlowIncreasing = false;
                }
            }

            PlyrGlow.FDrawn = false;
        }

        // AoWRelations.pas:763
        // { draft }
        public void PlyrGlowDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            var call__180 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin[44];
            call__180.ShowBlended(PlyrGlow.WinLeft, PlyrGlow.WinTop, Surface, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[FGlowIntensity]);
        }

        // AoWRelations.pas:768
        public void FaceFrPreDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Rtl.TRect R = default;
            var call__181 = Aow.Rtl.Rtl.Rect(FaceFr.WinLeft + 1, FaceFr.WinTop, FaceFr.WinLeft + FaceFr.WinWidth, FaceFr.WinTop + FaceFr.WinHeight);
            R = call__181;
            if (FAction == null)
            {
                if (FSelectedPlayer != -1)
                {
                    var call__182 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    var call__183 = call__182.GetFirstDaySeen(unchecked((sbyte)FSelectedPlayer));
                    if (call__183 != -1)
                    {
                        var call__184 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FSelectedPlayer);
                        call__184.FLeader.ShowFace(R, Surface);
                    }
                }
                else
                {
                    var call__185 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FSelectedRace);
                    var call__186 = call__185.GetFace();
                    call__186.Show(FaceFr.WinLeft + 1, FaceFr.WinTop + 3, Surface);
                }
            }
            else
            {
                if ((FEvent != null) && (FEvent.FAction.FActionType == Aow.Units.AoWE.TDiplomaticActionType.daSendMessageAsY))
                {
                    var call__187 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FEvent.FAction.FOtherPlayer);
                    call__187.FLeader.ShowFace(R, Surface);
                }
                else
                {
                    if (FAction.FSource == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
                    {
                        var call__188 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FAction.FTarget);
                        call__188.FLeader.ShowFace(R, Surface);
                    }
                    else
                    {
                        if (FAction.FReaction == Aow.Units.AoWE.TDiplomaticActionReaction.darNone)
                        {
                            var call__189 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FAction.FSource);
                            call__189.FLeader.ShowFace(R, Surface);
                        }
                        else
                        {
                            var call__190 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FAction.FTarget);
                            call__190.FLeader.ShowFace(R, Surface);
                        }
                    }
                }
            }
        }

        // AoWRelations.pas:794
        public void SBGoldSlidChange(Aow.Rtl.TObject Sender)
        {
            if (FAction != null)
            {
                FAction.FGold = SBGoldSlid.Position;
            }

            SBGoldV.Text = Aow.Rtl.Rtl.IntToStr(SBGoldSlid.Position);
        }

        // AoWRelations.pas:800
        // { draft }
        public void SBManaSlidChange(Aow.Rtl.TObject Sender)
        {
            if (FAction != null)
            {
                FAction.FMana = SBManaSlid.Position;
            }

            SBManaV.Text = Aow.Rtl.Rtl.IntToStr(SBManaSlid.Position);
        }

        // AoWRelations.pas:806
        // { draft }
        public void RelWinHide()
        {
            FinishDialog();
        }

        // AoWRelations.pas:811
        // { draft }
        public void ASelectClick()
        {
            Aow.Rtl.AnsiString Reason = default;
            if (ActionsLB.Selected != -1)
            {
                var call__191 = ActionsLB.Items.Objects[ActionsLB.Selected];
                SetAction((Aow.Units.AoWE.TDiplomaticAction)call__191);
                switch (FAction.FActionType)
                {
                    case Aow.Units.AoWE.TDiplomaticActionType.daDeclareWar:
                    case Aow.Units.AoWE.TDiplomaticActionType.daThreatenToAttack:
                    case Aow.Units.AoWE.TDiplomaticActionType.daBreakAlliance:
                        var call__192 = FAction.Send(ref Reason);
                        if (!call__192)
                        {
                            Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(Reason);
                        }

                        break;
                    case Aow.Units.AoWE.TDiplomaticActionType.daOfferPeace:
                    case Aow.Units.AoWE.TDiplomaticActionType.daProposeAlliance:
                    case Aow.Units.AoWE.TDiplomaticActionType.daMakeTribute:
                    case Aow.Units.AoWE.TDiplomaticActionType.daBreakAllianceWithY:
                    case Aow.Units.AoWE.TDiplomaticActionType.daDeclareWarToY:
                        RNewActionPnl.Visible = false;
                        SBIcon.Visible = true;
                        RSBribePnl.Visible = true;
                        SBGoldSlid.Position = 0;
                        SBManaSlid.Position = 0;
                        var call__193 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                        SBGoldSlid.Max = call__193.FGems;
                        var call__194 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                        SBManaSlid.Max = call__194.FMagicControl.FMana;
                        SBManaSlidChange(null);
                        SBGoldSlidChange(null);
                        var call__195 = FAction.GetName(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                        BribeHeaderLbl.Text = call__195;
                        switch (FAction.FActionType)
                        {
                            case Aow.Units.AoWE.TDiplomaticActionType.daOfferPeace:
                                SBIcon.CurrentImage = 8;
                                break;
                            case Aow.Units.AoWE.TDiplomaticActionType.daProposeAlliance:
                                SBIcon.CurrentImage = 10;
                                break;
                            case Aow.Units.AoWE.TDiplomaticActionType.daMakeTribute:
                                SBIcon.CurrentImage = 48;
                                break;
                            case Aow.Units.AoWE.TDiplomaticActionType.daDeclareWarToY:
                                SBIcon.CurrentImage = 47;
                                break;
                            case Aow.Units.AoWE.TDiplomaticActionType.daBreakAllianceWithY:
                                SBIcon.CurrentImage = 47;
                                break;
                            default:
                                SBIcon.Visible = false;
                                break;
                        }

                        break;
                    case Aow.Units.AoWE.TDiplomaticActionType.daSendMessage:
                    case Aow.Units.AoWE.TDiplomaticActionType.daSendMessageAsY:
                        RNewActionPnl.Visible = false;
                        RMsgEditPnl.Visible = true;
                        break;
                }
            }
        }

        // AoWRelations.pas:851
        // { draft }
        public void ActionsLBChange(Aow.Rtl.TObject Sender)
        {
            RelPnl.FDrawn = false;
            if (ActionsLB.Selected != -1)
            {
                ASelect.ILIndexDown = 37;
                ASelect.ILIndexLit = 40;
                ASelect.ILIndexUp = 34;
            }
            else
            {
                ASelect.ILIndexDown = 46;
                ASelect.ILIndexLit = 49;
                ASelect.ILIndexUp = 43;
            }
        }

        // AoWRelations.pas:868
        // { draft }
        public void CancelSelectedAction()
        {
            if (FEvent != null)
            {
                FinishEvent();
            }

            ShowPlayerActions(unchecked((sbyte)FAction.FTarget));
            RNewActionPnl.Visible = true;
            RSBribePnl.Visible = false;
            RMsgEditPnl.Visible = false;
        }

        // AoWRelations.pas:877
        // { draft }
        public void ChangeActionMessage(Aow.Units.AoWMemo.TAOWMemo Sender)
        {
            if (FAction != null)
            {
                var call__196 = Sender.Items.Text;
                FAction.FMessage.Text = call__196;
            }
        }

        // AoWRelations.pas:882
        public void MsgSendActionBtn()
        {
            Aow.Rtl.AnsiString Reason = default;
            sbyte Player = default;
            Aow.Units.AoWE.TDiplomaticAction Action = default;
            Action = FAction;
            if (Action != null)
            {
                if (Action.Send(ref Reason))
                {
                    Player = unchecked((sbyte)FAction.FTarget);
                    if ((FEvent != null) && (FEvent.FAction.FActionType == Aow.Units.AoWE.TDiplomaticActionType.daSendMessageAsY))
                    {
                        Player = unchecked((sbyte)FEvent.FAction.FOtherPlayer);
                    }

                    FinishEvent();
                    ShowPlayerActions(unchecked((sbyte)Player));
                }
                else
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(Reason);
                }
            }
        }

        // AoWRelations.pas:901
        // { draft }
        public void ReplyBtnClick()
        {
            Aow.Units.AoWE.TDiplomaticAction Action = default;
            var call__197 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FAction.FTarget);
            var call__198 = call__197.CreateDiplomaticAction(Aow.Units.AoWE.TDiplomaticActionType.daSendMessage, unchecked((sbyte)FAction.FSource), unchecked((sbyte)-1));
            Action = call__198;
            ClearActions();
            SetAction(Action);
            RNewActionPnl.Visible = false;
            RMsgEditPnl.Visible = true;
        }

        // AoWRelations.pas:912
        // { draft }
        public void RMCancelBtnClick()
        {
            if (FEvent != null)
            {
                FinishEvent();
            }

            ShowPlayerActions(unchecked((sbyte)FAction.FSource));
        }

        // AoWRelations.pas:918
        // { draft }
        public void IIOkBtnClick()
        {
            FinishEvent();
            if (FAction.FSource == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
            {
                ShowPlayerActions(unchecked((sbyte)FAction.FTarget));
            }
            else
            {
                ShowPlayerActions(unchecked((sbyte)FAction.FSource));
            }
        }

        // AoWRelations.pas:925
        // { draft }
        public void RelWinKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                RelWin.Visible = false;
            }
        }
    }

    public static partial class AoWRelations
    {
    }
}
