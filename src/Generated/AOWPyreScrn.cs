// Generated from AOWPyreScrn.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.aowUserDrawBox;
using static Aow.Units.aowUserDrawBox.aowUserDrawBox;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWResultsWin;
using static Aow.Units.AoWResultsWin.AoWResultsWin;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E1Globals;
using static Aow.Units.E1Globals.E1Globals;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;

namespace Aow.Units.AOWPyreScrn
{
    // { TPyreScreen (instance size $6C, VMT 004290B0): $64 game-over event, $68 animation frame. }
    public partial class TPyreScreen : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWPanel.TAOWPanel DeadPnl;
        // { +0044 }
        public Aow.Units.AoWImage.TAOWImage PyreTop;
        // { +0048 }
        public Aow.Units.AoWImage.TAOWImage LeaderFaceFr;
        // { +004C }
        public Aow.Units.aowUserDrawBox.TaowUserDrawBox PyreAnimation;
        // { +0050 }
        public Aow.Rtl.TObject FAnimation;
        public Aow.Units.Sound.TSongStatus FSavedSongStatus;
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        // { $54, $58, $5C; 004293AC }
        // { conflict: FFire: TSample; FSongStatus: TSongStatus; }
        public Aow.Units.EventLog.TGameOverEventLog FGameOverEvent;
        // { $64 }
        public int FAnimationFrame;
        // AOWPyreScrn.pas:114
        // { $68 }
        public void ClearFire()
        {
            if (FAnimation != null)
            {
                FAnimation.Free();
                FAnimation = null;
            }
        }

        // AOWPyreScrn.pas:119
        public void FinishScreen()
        {
            ClearFire();
            Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PyreFrames.ReleaseDiscardableImageData();
            Aow.Units.AOWPyreScrn.AOWPyreScrn.PyreScreen.DeadPnl.Visible = false;
            Aow.Units.AoWResultsWin.AoWResultsWin.ShowResults(Aow.Units.AoWResultsWin.AoWResultsWin.ResultsWin);
        }

        // AOWPyreScrn.pas:127
        public void ShowGameOver(Aow.Units.EventLog.TGameOverEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            /* TODO(T-shim) Platform or editor boundary SysUtils.SetCurrentDir */
            throw new System.NotImplementedException();
        }

        // AOWPyreScrn.pas:48
        public void DeadPnlUpdate()
        {
        }

        // AOWPyreScrn.pas:51
        // { draft }
        public void DeadPnlMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            FinishScreen();
        }

        // AOWPyreScrn.pas:54
        public void LeaderFaceFrPreDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Rtl.TRect R = default;
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                R.Left = (DeadPnl.WinLeft + LeaderFaceFr.WinLeft) + 45;
                R.Right = R.Left + 103;
                R.Top = (DeadPnl.WinTop + LeaderFaceFr.WinTop) + 35;
                R.Bottom = R.Top + 128;
                var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                call__1.FLeader.ShowFace(R, SD);
            }
        }

        // AOWPyreScrn.pas:69
        public void PyreAnimationUpdate()
        {
            PyreAnimation.FDrawn = false;
            DeadPnl.FDrawn = false;
            FAnimationFrame += 1;
            if (FAnimationFrame > 450)
            {
                FinishScreen();
            }

            if (FAnimationFrame > 16777215)
            {
                FAnimationFrame = 0;
            }
        }

        // AOWPyreScrn.pas:77
        // { draft }
        public void PyreAnimationDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int X = default;
            int Y = default;
            int Frame = default;
            Aow.Rtl.AnsiString Text = default;
            X = PyreAnimation.WinWidth / 2;
            Y = (PyreAnimation.WinHeight + 20) - (FAnimationFrame / 2);
            if (Y < 400)
            {
                Y = 400;
            }

            Y += DeadPnl.WinTop;
            X += DeadPnl.WinLeft;
            var call__2 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGameOverEvent.FPlayer);
            switch (call__2.FGameOverStatus)
            {
                case Aow.Units.AoWE.TGameOverStatus.goLost:
                    Frame = (FAnimationFrame / 3) % 9;
                    if (Aow.Units.AoWE.AoWE.AoWEngine.FDemo)
                    {
                        Frame = 0;
                    }

                    var call__3 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PyreFrames[Frame];
                    call__3.Show(DeadPnl.WinLeft + PyreAnimation.WinLeft, DeadPnl.WinTop + PyreAnimation.WinTop, SD);
                    var call__4 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGameOverEvent.FPlayer);
                    var call__5 = call__4.GetNameEx();
                    var call__6 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.YourReignHasEndedRStr);
                    Text = (call__5 + ((AnsiString)", ")) + call__6;
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown.SetTextCentered(X, Y, Text, SD);
                    break;
                case Aow.Units.AoWE.TGameOverStatus.goWon:
                    var call__7 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.VictoryFrames[0];
                    call__7.Show(DeadPnl.WinLeft + PyreAnimation.WinLeft, DeadPnl.WinTop, SD);
                    var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGameOverEvent.FPlayer);
                    var call__9 = call__8.GetNameEx();
                    var call__10 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.YouWhereVictoriousRStr);
                    Text = (call__9 + ((AnsiString)", ")) + call__10;
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey.SetTextCentered(X, Y, Text, SD);
                    break;
            }
        }

        // AOWPyreScrn.pas:110
        public void DeadPnlKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            switch (Key)
            {
                case 13:
                case 27:
                    FinishScreen();
                    break;
            }
        }
    }

    public static partial class AOWPyreScrn
    {
        public static Aow.Units.AOWPyreScrn.TPyreScreen PyreScreen;
    }
}
