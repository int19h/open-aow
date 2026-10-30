// Translated from AoWTBWindow.pas.
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
using Aow.Units.AOWMenuBar;
using static Aow.Units.AOWMenuBar.AOWMenuBar;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWEdit;
using static Aow.Units.AoWEdit.AoWEdit;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.TurnButton;
using static Aow.Units.TurnButton.TurnButton;
using Aow.Units.AoWPInfoWin;
using static Aow.Units.AoWPInfoWin.AoWPInfoWin;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWTCUnit;
using static Aow.Units.AoWTCUnit.AoWTCUnit;
using Aow.Units.AoWRealm;
using static Aow.Units.AoWRealm.AoWRealm;
using Aow.Units.aowMapEvents;
using static Aow.Units.aowMapEvents.aowMapEvents;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.E1Globals;
using static Aow.Units.E1Globals.E1Globals;
using Aow.Units.E1GameFlow;
using static Aow.Units.E1GameFlow.E1GameFlow;

namespace Aow.Units.AoWTBWindow
{
    // { TTBWindow (size $144, VMT 00456CAC): $134/$138 indicator frame/step, $13C/$13E world/combat clock delays, $140 chat flash countdown. }
    public partial class TTBWindow : Aow.Rtl.TDataModule
    {
        public TTBWindow(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow TaskWindow;
        // { +0044 }
        public Aow.Units.AoWButton.TAOWButton TBNextButton;
        // { +0048 }
        public Aow.Units.AOWMenuBar.TAOWMenuBar StartMenuBar;
        // { +004C }
        public Aow.Units.AoWEdit.TAOWEdit TBMessEdit;
        // { +0050 }
        public Aow.Units.AoWLabel.TAOWLabel WGILbl;
        // { +0054 }
        public Aow.Units.AoWLabel.TAOWLabel WGTLbl;
        // { +0058 }
        public Aow.Units.TurnButton.TAOWTurnButton TurnButton;
        // { +005C }
        public Aow.Units.AoWButton.TAOWButton WMButton;
        // { +0060 }
        public Aow.Units.AoWButton.TAOWButton OVButton;
        // { +0064 }
        public Aow.Units.AoWButton.TAOWButton ConButton;
        // { +0068 }
        public Aow.Units.AoWButton.TAOWButton StructButton;
        // { +006C }
        public Aow.Units.AoWButton.TAOWButton OV2Btn;
        // { +0070 }
        public Aow.Units.AoWButton.TAOWButton InfoBtn;
        // { +0074 }
        public Aow.Units.AoWButton.TAOWButton ChatBtn;
        // { +0078 }
        public Aow.Units.AoWPanel.TAOWPanel TaskPanel;
        // { +007C }
        public Aow.Units.AoWPanel.TAOWPanel WGIPnl;
        // { +0080 }
        public Aow.Units.AoWFrame.TAoWFrame TBbk;
        // { +0084 }
        public Aow.Units.AoWImage.TAOWImage TimerImg;
        // { +0088 }
        public Aow.Units.AoWImage.TAOWImage TimeIcn;
        // { +008C }
        public Aow.Units.AoWLabel.TAOWLabel TimeLbl;
        // { +0090 }
        public Aow.Units.AoWFrame.TAoWFrame PInd;
        // { +0094 }
        public Aow.Units.AoWImage.TAOWImage CurP;
        // { +0098 }
        public Aow.Units.AoWPanel.TAOWPanel TBWorld;
        // { +009C }
        public Aow.Units.AoWButton.TAOWButton TCNextBtn;
        // { +00A0 }
        public Aow.Units.AOWMenuBar.TAOWMenuBar TCStartBar;
        // { +00A4 }
        public Aow.Units.AoWEdit.TAOWEdit TCMsg;
        // { +00A8 }
        public Aow.Units.TurnButton.TAOWTurnButton TCTurnButton;
        // { +00AC }
        public Aow.Units.AoWButton.TAOWButton TCScannerBtn;
        // { +00B0 }
        public Aow.Units.AoWButton.TAOWButton TCUnitBtn;
        // { +00B4 }
        public Aow.Units.AoWFrame.TAoWFrame AoWFrame1;
        // { +00B8 }
        public Aow.Units.AoWImage.TAOWImage TCTimerImg;
        // { +00BC }
        public Aow.Units.AoWImage.TAOWImage TCTimeImg;
        // { +00C0 }
        public Aow.Units.AoWLabel.TAOWLabel TCTimeLbl;
        // { +00C4 }
        public Aow.Units.AoWFrame.TAoWFrame TCPInd;
        // { +00C8 }
        public Aow.Units.AoWImage.TAOWImage TCCurP;
        // { +00CC }
        public Aow.Units.AoWPanel.TAOWPanel TBCombat;
        // { +00D0 }
        public Aow.Units.AoWPanel.TAOWPanel TCPnl;
        // { +00D4 }
        public Aow.Units.AoWButton.TAOWButton TCChatBtn;
        // { +00D8 }
        public Aow.Units.AoWPanel.TAOWPanel WGoldPnl;
        // { +00DC }
        public Aow.Units.AoWLabel.TAOWLabel WGSign;
        // { +00E0 }
        public Aow.Units.AoWLabel.TAOWLabel WMILbl;
        // { +00E4 }
        public Aow.Units.AoWLabel.TAOWLabel WMTLbl;
        // { +00E8 }
        public Aow.Units.AoWPanel.TAOWPanel WMIPnl;
        // { +00EC }
        public Aow.Units.AoWPanel.TAOWPanel WManaPnl;
        // { +00F0 }
        public Aow.Units.AoWLabel.TAOWLabel WMTSign;
        // { +00F4 }
        public Aow.Units.AoWLabel.TAOWLabel TGILbl;
        // { +00F8 }
        public Aow.Units.AoWLabel.TAOWLabel TGTLbl;
        // { +00FC }
        public Aow.Units.AoWImage.TAOWImage TGIcn;
        // { +0100 }
        public Aow.Units.AoWPanel.TAOWPanel TGIPnl;
        // { +0104 }
        public Aow.Units.AoWPanel.TAOWPanel TGoldPnl;
        // { +0108 }
        public Aow.Units.AoWLabel.TAOWLabel TGSign;
        // { +010C }
        public Aow.Units.AoWLabel.TAOWLabel TMILbl;
        // { +0110 }
        public Aow.Units.AoWLabel.TAOWLabel TMTLbl;
        // { +0114 }
        public Aow.Units.AoWImage.TAOWImage TWMIcn;
        // { +0118 }
        public Aow.Units.AoWPanel.TAOWPanel TMIPnl;
        // { +011C }
        public Aow.Units.AoWPanel.TAOWPanel TManaPnl;
        // { +0120 }
        public Aow.Units.AoWLabel.TAOWLabel TMTSign;
        // { +0124 }
        public Aow.Units.AoWButton.TAOWButton RelationsBtn;
        // { +0128 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox AutoBtn;
        // { +012C }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0130 }
        public int FIndicatorFrame;
        // { $134 }
        public int FIndicatorStep;
        // { $138 }
        public short FWorldClockDelay;
        // { $13C }
        public short FCombatClockDelay;
        // { $13E }
        public short FChatFlash;
        // { $140 }
        public Aow.Units.FixedArray_byte_2 F_TBWindow_142;
        // AoWTBWindow.pas:122
        // { draft }
        public void TurnButtonAnimDone()
        {
            TurnButton.Checked = false;
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap != null)
            {
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FEndTurnPending = true;
            }
        }

        // AoWTBWindow.pas:128
        // { draft }
        public void WMButtonClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Visible = !Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Visible;
        }

        // AoWTBWindow.pas:134
        // { draft }
        public void OVButtonClick(int A1)
        {
            if (A1 != 0)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            }

            Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible = !Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible;
            if (Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible)
            {
                Aow.Units.AoWRealm.AoWRealm.RealmWin.UpdateOverview();
            }
        }

        // AoWTBWindow.pas:141
        // { draft }
        public void ConButtonClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Visible = !Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Visible;
        }

        // AoWTBWindow.pas:147
        // { draft }
        public void StructButtonClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Visible = !Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Visible;
        }

        // AoWTBWindow.pas:153
        // { draft }
        public void TBNextButtonClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Aow.Units.AoWE.AoWE.AoWHSMap.SelectNextArmy();
        }

        // AoWTBWindow.pas:159
        // { draft }
        public void UnitBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = true;
        }

        // AoWTBWindow.pas:165
        // { draft }
        public void InfoBtnClick(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Visible)
            {
                Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Visible = false;
            }
            else
            {
                Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.UpdatePlayer();
            }
        }

        // AoWTBWindow.pas:172
        public void ProdBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWTBWindow.pas:177
        // { draft }
        public void COBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWTBWindow.pas:182
        // { draft }
        public void ChatBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Visible = !Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Visible;
        }

        // AoWTBWindow.pas:188
        // { draft }
        public void TurnButtonChange()
        {
            var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (!call__1.FSingleProcessCycle)
            {
                var call__2 = Aow.Units.AoWE.AoWE.AoWHSMap.EndTurn();
                TurnButton.Checked = call__2;
            }
            else
            {
                var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.CancelEndTurn();
                TurnButton.Checked = !call__3;
            }
        }

        // AoWTBWindow.pas:195
        // { draft }
        public void PIndDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            int I = default;
            int Position = default;
            int Race = default;
            Position = 0;
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                {
                    I = 0;
                    int end__4 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetCount() - 1;
                    for (; I <= end__4; I += 1)
                    {
                        var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                        if (call__5.FStatus == Aow.Units.AoWE.TPlayerStatus.psPlaying)
                        {
                            var call__6 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                            Race = call__6.FPlayerRace;
                            if (Race == -1)
                            {
                                Race = 12;
                            }

                            var call__7 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin[Race + 32];
                            call__7.ShowClipped((PInd.WinLeft + 3) + (Position * 5), PInd.WinTop + 2, Surface);
                            if (Aow.Units.AoWE.AoWE.AoWHSMap.FPlayMode == Aow.Units.AoWE.TPlayMode.pmTurn)
                            {
                                if (I == Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayer)
                                {
                                    CurP.WinLeft = (PInd.WinLeft + 2) + (Position * 5);
                                    CurP.WinTop = PInd.WinTop + 11;
                                }
                            }
                            else
                            {
                                var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                                if (call__8.FSingleProcessCycle)
                                {
                                    var call__9 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin[50];
                                    call__9.ShowClipped((PInd.WinLeft + 2) + (Position * 5), PInd.WinTop + 11, Surface);
                                }
                            }

                            Position += 1;
                        }
                    }
                }
            }
        }

        // AoWTBWindow.pas:222
        public void CurPUpdate()
        {
            FIndicatorFrame += FIndicatorStep;
            if (FIndicatorFrame > 4)
            {
                FIndicatorStep = -1;
            }

            if (FIndicatorFrame < 1)
            {
                FIndicatorStep = 1;
            }

            CurP.CurrentImage = FIndicatorFrame + 45;
            PInd.FDrawn = false;
        }

        // AoWTBWindow.pas:231
        // { draft }
        public void TCTurnButtonChange()
        {
            if (TCTurnButton.Checked)
            {
                var call__10 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.EndTurn(false);
                TCTurnButton.Checked = call__10;
            }
            else
            {
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.CancelEndTurn();
            }
        }

        // AoWTBWindow.pas:237
        // { draft }
        public void TCTurnButtonAnimDone()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
            TCTurnButton.Checked = false;
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FEndTurnPending = true;
        }

        // AoWTBWindow.pas:244
        // { draft }
        public void TCPIndDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            int I = default;
            int Position = default;
            int Race = default;
            Position = 0;
            {
                I = 0;
                int end__11 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FTurnOrder.Count - 1;
                for (; I <= end__11; I += 1)
                {
                    var call__12 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FTurnOrder[I];
                    var call__13 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)call__12));
                    var call__14 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__13.FPlayer);
                    Race = call__14.FPlayerRace;
                    if (Race == -1)
                    {
                        Race = 12;
                    }

                    var call__15 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin[Race + 32];
                    call__15.ShowClipped((TCPInd.WinLeft + 3) + (Position * 5), TCPInd.WinTop + 2, Surface);
                    var call__16 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FTurnOrder[I];
                    if (call__16 == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer)
                    {
                        TCCurP.WinLeft = (TCPInd.WinLeft + 2) + (Position * 5);
                        TCCurP.WinTop = TCPInd.WinTop + 11;
                    }

                    Position += 1;
                }
            }
        }

        // AoWTBWindow.pas:264
        public void TCCurPUpdate()
        {
            FIndicatorFrame += FIndicatorStep;
            if (FIndicatorFrame > 4)
            {
                FIndicatorStep = -1;
            }

            if (FIndicatorFrame < 1)
            {
                FIndicatorStep = 1;
            }

            TCCurP.CurrentImage = FIndicatorFrame + 45;
            TCPInd.FDrawn = false;
        }

        // AoWTBWindow.pas:273
        // { draft }
        public void TCScannerBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.Visible = !Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.Visible;
        }

        // AoWTBWindow.pas:279
        // { draft }
        public void TCNextBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.SelectNextUnit(true);
        }

        // AoWTBWindow.pas:285
        // { draft }
        public void TBWorldUpdate(Aow.Rtl.TObject Sender)
        {
            ushort Hour = default;
            ushort Minute = default;
            ushort Second = default;
            ushort Millisecond = default;
            if (TBWorld.Visible && (Aow.Units.AoWE.AoWE.AoWHSMap != null))
            {
                FWorldClockDelay = (short)(((int)FWorldClockDelay) - 1);
                if (FWorldClockDelay < 0)
                {
                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FTurnTime != 0)
                    {
                        FWorldClockDelay = 10;
                        var call__17 = Aow.Units.AoWE.AoWE.AoWHSMap.GetTurnTimeLeft();
                        Minute = unchecked((ushort)(call__17 / 60000));
                        var call__18 = Aow.Units.AoWE.AoWE.AoWHSMap.GetTurnTimeLeft();
                        Second = unchecked((ushort)((call__18 % 60000) / 1000));
                        if (((Minute == 0) && (Second > 0)) && (Second <= 10))
                        {
                            TimeLbl.TextColor = 255;
                        }
                        else
                        {
                            TimeLbl.TextColor = 0;
                        }

                        TimeLbl.Text = Aow.Rtl.Rtl.IntToStr(Minute) + AnsiString.FromBytes(58);
                        if (Second < 10)
                        {
                            TimeLbl.Text = TimeLbl.Text + AnsiString.FromBytes(48);
                        }

                        TimeLbl.Text = TimeLbl.Text + Aow.Rtl.Rtl.IntToStr(Second);
                    }
                    else
                    {
                        FWorldClockDelay = 900;
                        var call__19 = Aow.Rtl.Rtl.Time();
                        Aow.Rtl.Rtl.DecodeTime(call__19, ref Hour, ref Minute, ref Second, ref Millisecond);
                        if (Hour > 12)
                        {
                            Hour = (ushort)(((int)Hour) - 12);
                        }

                        if (Hour == 0)
                        {
                            Hour = 12;
                        }

                        TimeLbl.Text = Aow.Rtl.Rtl.IntToStr(Hour) + AnsiString.FromBytes(58);
                        if (Minute < 10)
                        {
                            TimeLbl.Text = TimeLbl.Text + AnsiString.FromBytes(48);
                        }

                        TimeLbl.Text = TimeLbl.Text + Aow.Rtl.Rtl.IntToStr(Minute);
                    }
                }
            }
        }

        // AoWTBWindow.pas:317
        public void TBCombatUpdate(Aow.Rtl.TObject Sender)
        {
            ushort Hour = default;
            ushort Minute = default;
            ushort Second = default;
            ushort Millisecond = default;
            if (TBCombat.Visible && (Aow.Units.AoWTC.AoWTC.AoWCombatMap != null))
            {
                FCombatClockDelay = (short)(((int)FCombatClockDelay) - 1);
                if (FCombatClockDelay < 0)
                {
                    FCombatClockDelay = 900;
                    var call__20 = Aow.Rtl.Rtl.Time();
                    Aow.Rtl.Rtl.DecodeTime(call__20, ref Hour, ref Minute, ref Second, ref Millisecond);
                    if (Hour > 12)
                    {
                        Hour = (ushort)(((int)Hour) - 12);
                    }

                    if (Hour == 0)
                    {
                        Hour = 12;
                    }

                    TCTimeLbl.Text = Aow.Rtl.Rtl.IntToStr(Hour) + AnsiString.FromBytes(58);
                    if (Minute < 10)
                    {
                        TCTimeLbl.Text = TCTimeLbl.Text + AnsiString.FromBytes(48);
                    }

                    TCTimeLbl.Text = TCTimeLbl.Text + Aow.Rtl.Rtl.IntToStr(Minute);
                }
            }
        }

        // AoWTBWindow.pas:337
        public void OV2BtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Visible)
            {
                Aow.Units.E1GameFlow.E1GameFlow.HideMagic(Aow.Units.AoWExeForms.AoWExeForms.MagicWin);
            }
            else
            {
                Aow.Units.aowMapEvents.aowMapEvents.ShowMagicWindow(Aow.Units.AoWExeForms.AoWExeForms.MagicWin);
            }
        }

        // AoWTBWindow.pas:344
        // { draft }
        public void TCUnitBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Visible = !Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Visible;
        }

        // AoWTBWindow.pas:350
        // { draft }
        public void StartMenuBarChange()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(3);
        }

        // AoWTBWindow.pas:355
        // { draft }
        public void RelationsBtnClick(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var relWindow = Aow.Units.E1Globals.E1Globals.RelWindow;
            if (relWindow.RelWin.Visible)
            {
                relWindow.RelWin.Visible = false;
            }
            else
            {
                Aow.Units.E1Globals.E1Globals.RelWindow.ShowRelations(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            }
        }

        // AoWTBWindow.pas:363
        public void ChatBtnUpdate(Aow.Rtl.TObject Sender)
        {
            if (FChatFlash > 0)
            {
                switch (FChatFlash)
                {
                    case 1:
                    case 31:
                    case 61:
                    case 91:
                    case 121:
                    {
                        var button = (Aow.Units.AoWButton.TAOWButton)Sender;
                        button.FDrawn = false;
                        button.ILIndexUp = 14;
                    }

                        break;
                    case 16:
                    case 46:
                    case 76:
                    case 106:
                    case 136:
                    {
                        var button2 = (Aow.Units.AoWButton.TAOWButton)Sender;
                        button2.FDrawn = false;
                        button2.ILIndexUp = 20;
                    }

                        break;
                }

                FChatFlash = (short)(((int)FChatFlash) - 1);
            }
        }

        // AoWTBWindow.pas:377
        public void AutoBtn2Click()
        {
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer > -1)
            {
                var call__24 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer));
                call__24.SetAuto(AutoBtn.Checked);
            }
        }
    }

    public static partial class AoWTBWindow
    {
    }
}
