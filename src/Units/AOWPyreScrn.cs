// Translated from AOWPyreScrn.pas.
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
    // game-over event, animation frame
    public partial class TPyreScreen : Aow.Rtl.TDataModule
    {
        public TPyreScreen(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWPanel.TAOWPanel DeadPnl;
        public Aow.Units.AoWImage.TAOWImage PyreTop;
        public Aow.Units.AoWImage.TAOWImage LeaderFaceFr;
        public Aow.Units.aowUserDrawBox.TaowUserDrawBox PyreAnimation;
        public Aow.Rtl.TObject FAnimation;
        public Aow.Units.Sound.TSongStatus FSavedSongStatus;
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        public Aow.Units.EventLog.TGameOverEventLog FGameOverEvent;
        public int FAnimationFrame;
        // AOWPyreScrn.pas:114
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
            FAnimationFrame = 0;
            FGameOverEvent = Event;
            FGameOverEvent.AddRef();
            FDone = Done;
            var call__1 = Aow.Units.Sound.Sound.BackupSongStatus();
            FSavedSongStatus = call__1;
            Aow.Units.Sound.Sound.FadeMusicVolume(0, 1);
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 249;
            var call__2 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Event.FPlayer);
            switch (call__2.FGameOverStatus)
            {
                case Aow.Units.AoWE.TGameOverStatus.goLost:
                    var call__3 = Aow.Units.Sound.Sound.PlaySampleEx((AnsiString)"Sfx\\Attack\\Fireloop.wav", 64, true, 0, Aow.Units.Sound.TSamplePanning.spCenter, null);
                    FAnimation = call__3;
                    Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(68, true, true);
                    break;
                case Aow.Units.AoWE.TGameOverStatus.goWon:
                    Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(67, true, true);
                    break;
            }

            Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PyreFrames.LoadDiscardableImageData();
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitlePnl.Visible = false;
            Aow.Units.AOWPyreScrn.AOWPyreScrn.PyreScreen.DeadPnl.Visible = true;
            Aow.Units.AoWVideoDlg.AoWVideoDlg.VideoDialog.SetSceneSize(640, 480);
            Aow.Units.AoWExeForms.AoWExeForms.SetVideoMode(Aow.Units.AoWVideoDlg.AoWVideoDlg.VideoDialog, Aow.Units.AoWVideoDlg.TScreenMode.scmTitle);
            Aow.Units.AoWVideoDlg.AoWVideoDlg.VideoDialog.ApplyMode();
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.Visible = true;
            var call__4 = Aow.Rtl.Rtl.Application.ExeName;
            var call__5 = Aow.Rtl.Rtl.ExtractFileDir(call__4);
            Aow.Rtl.Rtl.SetCurrentDir(call__5);
        }

        // AOWPyreScrn.pas:48
        public void DeadPnlUpdate()
        {
        }

        // AOWPyreScrn.pas:51
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
                var call__6 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                call__6.FLeader.ShowFace(R, SD);
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
            var call__7 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGameOverEvent.FPlayer);
            switch (call__7.FGameOverStatus)
            {
                case Aow.Units.AoWE.TGameOverStatus.goLost:
                    Frame = (FAnimationFrame / 3) % 9;
                    if (Aow.Units.AoWE.AoWE.AoWEngine.FDemo)
                    {
                        Frame = 0;
                    }

                    var call__8 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PyreFrames[Frame];
                    call__8.Show(DeadPnl.WinLeft + PyreAnimation.WinLeft, DeadPnl.WinTop + PyreAnimation.WinTop, SD);
                    var call__9 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGameOverEvent.FPlayer);
                    var call__10 = call__9.GetNameEx();
                    var call__11 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.YourReignHasEndedRStr);
                    Text = (call__10 + ((AnsiString)", ")) + call__11;
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown.SetTextCentered(X, Y, Text, SD);
                    break;
                case Aow.Units.AoWE.TGameOverStatus.goWon:
                    var call__12 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.VictoryFrames[0];
                    call__12.Show(DeadPnl.WinLeft + PyreAnimation.WinLeft, DeadPnl.WinTop, SD);
                    var call__13 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FGameOverEvent.FPlayer);
                    var call__14 = call__13.GetNameEx();
                    var call__15 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.YouWhereVictoriousRStr);
                    Text = (call__14 + ((AnsiString)", ")) + call__15;
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
