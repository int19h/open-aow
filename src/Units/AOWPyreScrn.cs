// Translated from AOWPyreScrn.pas.
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using static Aow.Game.Audio.SoundUnit;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.Ui.Dialogs.AoWExeFormsUnit;
using static Aow.Game.Ui.Dialogs.AoWResultsWinUnit;
using static Aow.Game.Ui.Dialogs.AoWVideoDlgUnit;
using static Aow.Game.Ui.Dialogs.AoWWinManUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// game-over event, animation frame
public partial class TPyreScreen : TDataModule
{
    public TPyreScreen(TComponent owner) : base(owner)
    { }

    public TAOWPanel DeadPnl;
    public TAOWImage PyreTop;
    public TAOWImage LeaderFaceFr;
    public TaowUserDrawBox PyreAnimation;
    public object FAnimation;
    public TSongStatus FSavedSongStatus;
    public TExecuteDoneEvent FDone;
    public TGameOverEventLog FGameOverEvent;
    public int FAnimationFrame;

    // AOWPyreScrn.pas:114
    public void ClearFire()
    {
        if (FAnimation != null)
        {
            (FAnimation as IDisposable)?.Dispose();
            FAnimation = null;
        }
    }

    // AOWPyreScrn.pas:119
    public void FinishScreen()
    {
        ClearFire();
        aowIntGfxModUnit.IntGfxMod.PyreFrames.ReleaseDiscardableImageData();
        AOWPyreScrnUnit.PyreScreen.DeadPnl.Visible = false;
        ShowResults(ResultsWin);
    }

    // AOWPyreScrn.pas:127
    public void ShowGameOver(TGameOverEventLog Event, TExecuteDoneEvent Done)
    {
        FAnimationFrame = 0;
        FGameOverEvent = Event;
        FGameOverEvent.AddRef();
        FDone = Done;
        FSavedSongStatus = BackupSongStatus();
        FadeMusicVolume(0, 1);
        General.Manager.OverlayPriority = 249;
        switch (AoWHSMap.FPlayers.GetPlayers(Event.FPlayer).FGameOverStatus)
        {
            case TGameOverStatus.goLost:
                FAnimation = PlaySampleEx("Sfx\\Attack\\Fireloop.wav", 64, true, 0, TSamplePanning.spCenter, null);
                AoWHSMap.FTutorialControl.ShowMessage(68, true, true);
                break;
            case TGameOverStatus.goWon:
                AoWHSMap.FTutorialControl.ShowMessage(67, true, true);
                break;
        }

        aowIntGfxModUnit.IntGfxMod.PyreFrames.LoadDiscardableImageData();
        E2GlobalsUnit.TitleWindow.TitlePnl.Visible = false;
        AOWPyreScrnUnit.PyreScreen.DeadPnl.Visible = true;
        VideoDialog.SetSceneSize(640, 480);
        SetVideoMode(VideoDialog, TScreenMode.scmTitle);
        VideoDialog.ApplyMode();
        E2GlobalsUnit.TitleWindow.TitleWin.Visible = true;
        SetCurrentDir(ExtractFileDir(Application.ExeName));
    }

    // AOWPyreScrn.pas:48
    public void DeadPnlUpdate()
    {
    }

    // AOWPyreScrn.pas:51
    public void DeadPnlMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FinishScreen();
    }

    // AOWPyreScrn.pas:54
    public void LeaderFaceFrPreDraw(TSurfaceDesc SD)
    {
        TRect R = default;
        if (AoWHSMap != null)
        {
            R.Left = DeadPnl.WinLeft + LeaderFaceFr.WinLeft + 45;
            R.Right = R.Left + 103;
            R.Top = DeadPnl.WinTop + LeaderFaceFr.WinTop + 35;
            R.Bottom = R.Top + 128;
            AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FLeader.ShowFace(R, SD);
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
    public void PyreAnimationDraw(TSurfaceDesc SD)
    {
        int X;
        int Y;
        int Frame;
        string Text;
        X = PyreAnimation.WinWidth / 2;
        Y = PyreAnimation.WinHeight + 20 - FAnimationFrame / 2;
        if (Y < 400)
        {
            Y = 400;
        }

        Y += DeadPnl.WinTop;
        X += DeadPnl.WinLeft;
        switch (AoWHSMap.FPlayers.GetPlayers(FGameOverEvent.FPlayer).FGameOverStatus)
        {
            case TGameOverStatus.goLost:
                Frame = FAnimationFrame / 3 % 9;
                if (AoWEngine.FDemo)
                {
                    Frame = 0;
                }

                aowIntGfxModUnit.IntGfxMod.PyreFrames[Frame].Show(DeadPnl.WinLeft + PyreAnimation.WinLeft, DeadPnl.WinTop + PyreAnimation.WinTop, SD);
                Text = AoWHSMap.FPlayers.GetPlayers(FGameOverEvent.FPlayer).GetNameEx() + ", " + TranslateRStr(YourReignHasEndedRStr);
                FontModule.AoW15BlackBrown.SetTextCentered(X, Y, Text, SD);
                break;
            case TGameOverStatus.goWon:
                aowIntGfxModUnit.IntGfxMod.VictoryFrames[0].Show(DeadPnl.WinLeft + PyreAnimation.WinLeft, DeadPnl.WinTop, SD);
                Text = AoWHSMap.FPlayers.GetPlayers(FGameOverEvent.FPlayer).GetNameEx() + ", " + TranslateRStr(YouWhereVictoriousRStr);
                FontModule.AoW15WhiteGrey.SetTextCentered(X, Y, Text, SD);
                break;
        }
    }

    // AOWPyreScrn.pas:110
    public void DeadPnlKeyDown(ref ushort Key, TSet<TShiftState> Shift)
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

public static partial class AOWPyreScrnUnit
{
    public static TPyreScreen PyreScreen;
}
