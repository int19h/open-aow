// Translated from AoWNewTurnDialog.pas.
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
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;

namespace Aow.Units.AoWNewTurnDialog
{
    // { TNewTurnDlg: done callback $78, new-turn event $80.
//     (instance size 132, VMT 004391B0) }
    public partial class TNewTurnDlg : Aow.Rtl.TDataModule
    {
        public TNewTurnDlg(Aow.Rtl.TComponent AOwner) : base(AOwner) { }

        public Aow.Units.AoWWindow.TAOWWindow NewTDlg;
        // { +0044 }
        public Aow.Units.AoWFrame.TAoWFrame TCeltB;
        // { +0048 }
        public Aow.Units.AoWLabel.TAOWLabel ClickMsg;
        // { +004C }
        public Aow.Units.AoWImage.TAOWImage LShield;
        // { +0050 }
        public Aow.Units.AoWLabel.TAOWLabel PlayerName;
        // { +0054 }
        public Aow.Units.AoWLabel.TAOWLabel TurnMsg;
        // { +0058 }
        public Aow.Units.AoWLabel.TAOWLabel PlayerRace;
        // { +005C }
        public Aow.Units.AoWLabel.TAOWLabel Turn;
        // { +0060 }
        public Aow.Units.AoWImage.TAOWImage RShield;
        // { +0064 }
        public Aow.Units.AoWFrame.TAoWFrame BlueFr;
        // { +0068 }
        public Aow.Units.AoWImage.TAOWImage TCeltLineL;
        // { +006C }
        public Aow.Units.AoWImage.TAOWImage TCeltLineR;
        // { +0070 }
        public Aow.Units.AoWImage.TAOWImage FaceFrame;
        // { +0074 }
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        // { $78 }
        public Aow.Units.AoWE.TNewTurnEventLog FNewTurn;
        // AoWNewTurnDialog.pas:50
        // { draft }
        // { $80 }
        public void FinishDialog()
        {
            Aow.Units.EventLog.TExecuteDoneEvent Done = default;
            Aow.Units.AoWE.TNewTurnEventLog Event = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
            if (FNewTurn == null)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.SetSeatedPlayer(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayer));
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer > -1)
                {
                    Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.UpdateEvents();
                }
            }

            Done = FDone;
            Event = FNewTurn;
            FDone = null;
            FNewTurn = null;
            if (Done != null)
            {
                Done(Event);
            }

            if (Event != null)
            {
                Event.Release();
            }
        }

        // AoWNewTurnDialog.pas:69
        public void ShowNewTurn(Aow.Units.AoWE.TNewTurnEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            sbyte Race = default;
            int Day = default;
            switch (Event.FMode)
            {
                case Aow.Units.AoWE.TNewTurnEventLogMode.ntDialog:
                    if (FNewTurn != null)
                    {
                        FNewTurn.Release();
                        FNewTurn = null;
                    }

                    FDone = Done;
                    FNewTurn = Event;
                    FNewTurn.AddRef();
                    TurnMsg.Text = Event.FText;
                    var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Event.FTurnPlayer);
                    var call__2 = call__1.GetName();
                    PlayerName.Text = call__2;
                    if (Event.FTurnPlayer != 0)
                    {
                        var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Event.FTurnPlayer);
                        var call__4 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[call__3.FPlayerRace]);
                        PlayerRace.Text = call__4;
                    }
                    else
                    {
                        PlayerRace.Text = (AnsiString)"";
                    }

                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FDay > 0)
                    {
                        Day = Aow.Units.AoWE.AoWE.AoWHSMap.FDay;
                    }
                    else
                    {
                        Day = 1;
                    }

                    var call__5 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DayRStr);
                    Turn.Text = (call__5 + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(Day);
                    var call__6 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Event.FTurnPlayer);
                    Race = unchecked((sbyte)call__6.FPlayerRace);
                    if (Race == -1)
                    {
                        Race = 12;
                    }

                    LShield.CurrentImage = Race + 26;
                    RShield.CurrentImage = Race + 26;
                    NewTDlg.FDrawn = false;
                    NewTDlg.Visible = true;
                    if (FNewTurn.FDialogStyle.Contains(Aow.Units.AoWE.TNewTurnEventLogDialogStyleElement.nsCloseable))
                    {
                        var call__7 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ClickToBeginRStr);
                        ClickMsg.Text = call__7;
                    }
                    else
                    {
                        var call__8 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HiddenmovementPleaseWaitRStr);
                        ClickMsg.Text = call__8;
                    }

                    break;
                case Aow.Units.AoWE.TNewTurnEventLogMode.ntClose:
                    NewTDlg.Visible = false;
                    break;
            }
        }

        // AoWNewTurnDialog.pas:106
        public void NewTDlgMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if ((Aow.Units.AoWTC.AoWTC.AoWCombatMap == null) && FNewTurn.FDialogStyle.Contains(Aow.Units.AoWE.TNewTurnEventLogDialogStyleElement.nsCloseable))
            {
                NewTDlg.Visible = false;
            }
        }

        // AoWNewTurnDialog.pas:111
        public void NewTDlgKeyPress(Aow.Rtl.TObject Sender, ref byte Key)
        {
            if (FNewTurn.FDialogStyle.Contains(Aow.Units.AoWE.TNewTurnEventLogDialogStyleElement.nsCloseable) && (((Key == 32) || (Key == 13)) || (Key == 27)))
            {
                NewTDlg.Visible = false;
            }
        }

        // AoWNewTurnDialog.pas:116
        public void FaceFramePreDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Rtl.TRect R = default;
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap != null)
            {
                var call__9 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                if (call__9.FLeader != null)
                {
                    var call__10 = Aow.Rtl.Rtl.Rect(NewTDlg.WinLeft + 188, NewTDlg.WinTop + 40, (NewTDlg.WinLeft + 188) + 103, (NewTDlg.WinTop + 40) + 128);
                    R = call__10;
                    var call__11 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                    call__11.FLeader.ShowFace(R, Surface);
                }
            }
            else
            {
                var call__12 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FNewTurn.FTurnPlayer);
                if (call__12.FLeader != null)
                {
                    var call__13 = Aow.Rtl.Rtl.Rect(NewTDlg.WinLeft + 188, NewTDlg.WinTop + 40, (NewTDlg.WinLeft + 188) + 103, (NewTDlg.WinTop + 40) + 128);
                    R = call__13;
                    var call__14 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FNewTurn.FTurnPlayer);
                    call__14.FLeader.ShowFace(R, Surface);
                }
            }
        }

        // AoWNewTurnDialog.pas:136
        public void NewTDlgHide()
        {
            FinishDialog();
        }
    }

    public static partial class AoWNewTurnDialog
    {
    }
}
