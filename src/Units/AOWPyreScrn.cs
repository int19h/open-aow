// Translated from AOWPyreScrn.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.AoWImage;
using Aow.Units.AoWPanel;
using Aow.Units.aowUserDrawBox;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using static Aow.Units.AoWResultsWin.AoWResultsWin;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;

namespace Aow.Units.AOWPyreScrn
{
    // game-over event, animation frame
    public partial class TPyreScreen : TDataModule
    {
        public TPyreScreen(TComponent owner) : base(owner)
        { }

        public TAOWPanel DeadPnl;
        public TAOWImage PyreTop;
        public TAOWImage LeaderFaceFr;
        public TaowUserDrawBox PyreAnimation;
        public TObject FAnimation;
        public TSongStatus FSavedSongStatus;
        public TExecuteDoneEvent FDone;
        public TGameOverEventLog FGameOverEvent;
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
            aowIntGfxMod.aowIntGfxMod.IntGfxMod.PyreFrames.ReleaseDiscardableImageData();
            AOWPyreScrn.PyreScreen.DeadPnl.Visible = false;
            ShowResults(ResultsWin);
        }

        // AOWPyreScrn.pas:127
        public void ShowGameOver(TGameOverEventLog Event, TExecuteDoneEvent Done)
        {
            FAnimationFrame = 0;
            FGameOverEvent = Event;
            FGameOverEvent.AddRef();
            FDone = Done;
            var call__1 = BackupSongStatus();
            FSavedSongStatus = call__1;
            FadeMusicVolume(0, 1);
            General.Manager.OverlayPriority = 249;
            var call__2 = AoWHSMap.FPlayers.GetPlayers(Event.FPlayer);
            switch (call__2.FGameOverStatus)
            {
                case TGameOverStatus.goLost:
                    var call__3 = PlaySampleEx((AnsiString)"Sfx\\Attack\\Fireloop.wav", 64, true, 0, TSamplePanning.spCenter, null);
                    FAnimation = call__3;
                    AoWHSMap.FTutorialControl.ShowMessage(68, true, true);
                    break;
                case TGameOverStatus.goWon:
                    AoWHSMap.FTutorialControl.ShowMessage(67, true, true);
                    break;
            }

            aowIntGfxMod.aowIntGfxMod.IntGfxMod.PyreFrames.LoadDiscardableImageData();
            E2Globals.E2Globals.TitleWindow.TitlePnl.Visible = false;
            AOWPyreScrn.PyreScreen.DeadPnl.Visible = true;
            VideoDialog.SetSceneSize(640, 480);
            SetVideoMode(VideoDialog, TScreenMode.scmTitle);
            VideoDialog.ApplyMode();
            E2Globals.E2Globals.TitleWindow.TitleWin.Visible = true;
            var call__4 = Application.ExeName;
            var call__5 = ExtractFileDir(call__4);
            SetCurrentDir(call__5);
        }

        // AOWPyreScrn.pas:48
        public void DeadPnlUpdate()
        {
        }

        // AOWPyreScrn.pas:51
        public void DeadPnlMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            FinishScreen();
        }

        // AOWPyreScrn.pas:54
        public void LeaderFaceFrPreDraw(TObject Sender, TSurfaceDesc SD)
        {
            TRect R = default;
            if (AoWHSMap != null)
            {
                R.Left = DeadPnl.WinLeft + LeaderFaceFr.WinLeft + 45;
                R.Right = R.Left + 103;
                R.Top = DeadPnl.WinTop + LeaderFaceFr.WinTop + 35;
                R.Bottom = R.Top + 128;
                var call__6 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
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
        public void PyreAnimationDraw(TObject Sender, TSurfaceDesc SD)
        {
            int X = default;
            int Y = default;
            int Frame = default;
            AnsiString Text = default;
            X = PyreAnimation.WinWidth / 2;
            Y = PyreAnimation.WinHeight + 20 - FAnimationFrame / 2;
            if (Y < 400)
            {
                Y = 400;
            }

            Y += DeadPnl.WinTop;
            X += DeadPnl.WinLeft;
            var call__7 = AoWHSMap.FPlayers.GetPlayers(FGameOverEvent.FPlayer);
            switch (call__7.FGameOverStatus)
            {
                case TGameOverStatus.goLost:
                    Frame = FAnimationFrame / 3 % 9;
                    if (AoWEngine.FDemo)
                    {
                        Frame = 0;
                    }

                    var call__8 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.PyreFrames[Frame];
                    call__8.Show(DeadPnl.WinLeft + PyreAnimation.WinLeft, DeadPnl.WinTop + PyreAnimation.WinTop, SD);
                    var call__9 = AoWHSMap.FPlayers.GetPlayers(FGameOverEvent.FPlayer);
                    var call__10 = call__9.GetNameEx();
                    var call__11 = TranslateRStr(YourReignHasEndedRStr);
                    Text = call__10 + (AnsiString)", " + call__11;
                    FontModule.AoW15BlackBrown.SetTextCentered(X, Y, Text, SD);
                    break;
                case TGameOverStatus.goWon:
                    var call__12 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.VictoryFrames[0];
                    call__12.Show(DeadPnl.WinLeft + PyreAnimation.WinLeft, DeadPnl.WinTop, SD);
                    var call__13 = AoWHSMap.FPlayers.GetPlayers(FGameOverEvent.FPlayer);
                    var call__14 = call__13.GetNameEx();
                    var call__15 = TranslateRStr(YouWhereVictoriousRStr);
                    Text = call__14 + (AnsiString)", " + call__15;
                    FontModule.AoW15WhiteGrey.SetTextCentered(X, Y, Text, SD);
                    break;
            }
        }

        // AOWPyreScrn.pas:110
        public void DeadPnlKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
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
        public static TPyreScreen PyreScreen;
    }
}
