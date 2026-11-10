// Translated from AOWPyreScrn.pas.
using Aow.Game.Audio;

using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.Ui.Controls;

using Aow.Game.Ui.Windows;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// game-over event, animation frame
public partial class PyreScreen : DataModule
{
    public PyreScreen(Component owner) : base(owner)
    { }

    public AOWPanel DeadPnl;
    public AOWImage PyreTop;
    public AOWImage LeaderFaceFr;
    public TaowUserDrawBox PyreAnimation;
    public object FAnimation;
    public SongStatus FSavedSongStatus;
    public ExecuteDoneEvent FDone;
    public GameOverEventLog FGameOverEvent;
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
        DialogHosts.IntGfxMod.PyreFrames.ReleaseDiscardableImageData();
        DialogHosts.PyreScreen.DeadPnl.Visible = false;
        ShowResults(DialogHosts.ResultsWin);
    }

    // AOWPyreScrn.pas:127
    public void ShowGameOver(GameOverEventLog Event, ExecuteDoneEvent Done)
    {
        FAnimationFrame = 0;
        FGameOverEvent = Event;
        FGameOverEvent.AddRef();
        FDone = Done;
        FSavedSongStatus = AudioPlayback.BackupSongStatus();
        AudioPlayback.FadeMusicVolume(0, 1);
        DialogHosts.General.Manager.OverlayPriority = 249;
        switch (AoWHSMap.Current.FPlayers.GetPlayers(Event.FPlayer).FGameOverStatus)
        {
            case GameOverStatus.Lost:
                FAnimation = AudioPlayback.PlaySampleEx("Sfx\\Attack\\Fireloop.wav", 64, true, 0, SamplePanning.Center, null);
                AoWHSMap.Current.FTutorialControl.ShowMessage(68, true, true);
                break;
            case GameOverStatus.Won:
                AoWHSMap.Current.FTutorialControl.ShowMessage(67, true, true);
                break;
        }

        DialogHosts.IntGfxMod.PyreFrames.LoadDiscardableImageData();
        DialogHosts.TitleWindow.TitlePnl.Visible = false;
        DialogHosts.PyreScreen.DeadPnl.Visible = true;
        DialogHosts.VideoDialog.SetSceneSize(640, 480);
        VideoDialog.SetVideoMode(DialogHosts.VideoDialog, VideoDialog.ScreenMode.Title);
        DialogHosts.VideoDialog.ApplyMode();
        DialogHosts.TitleWindow.TitleWin.Visible = true;
        SetCurrentDir(ExtractFileDir(RtlUnit.Application.ExeName));
    }

    // AOWPyreScrn.pas:48
    public void DeadPnlUpdate()
    {
    }

    // AOWPyreScrn.pas:51
    public void DeadPnlMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        FinishScreen();
    }

    // AOWPyreScrn.pas:54
    public void LeaderFaceFrPreDraw(SurfaceDesc SD)
    {
        Rect R = default;
        if (AoWHSMap.Current != null)
        {
            R.Left = DeadPnl.WinLeft + LeaderFaceFr.WinLeft + 45;
            R.Right = R.Left + 103;
            R.Top = DeadPnl.WinTop + LeaderFaceFr.WinTop + 35;
            R.Bottom = R.Top + 128;
            AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FLeader.ShowFace(R, SD);
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
    public void PyreAnimationDraw(SurfaceDesc SD)
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
        switch (AoWHSMap.Current.FPlayers.GetPlayers(FGameOverEvent.FPlayer).FGameOverStatus)
        {
            case GameOverStatus.Lost:
                Frame = FAnimationFrame / 3 % 9;
                if (GameTables.AoWEngine.FDemo)
                {
                    Frame = 0;
                }

                DialogHosts.IntGfxMod.PyreFrames[Frame].Show(DeadPnl.WinLeft + PyreAnimation.WinLeft, DeadPnl.WinTop + PyreAnimation.WinTop, SD);
                Text = AoWHSMap.Current.FPlayers.GetPlayers(FGameOverEvent.FPlayer).GetNameEx() + ", " + AbilityRules.TranslateRStr(YourReignHasEndedRStr);
                DialogHosts.FontModule.AoW15BlackBrown.SetTextCentered(X, Y, Text, SD);
                break;
            case GameOverStatus.Won:
                DialogHosts.IntGfxMod.VictoryFrames[0].Show(DeadPnl.WinLeft + PyreAnimation.WinLeft, DeadPnl.WinTop, SD);
                Text = AoWHSMap.Current.FPlayers.GetPlayers(FGameOverEvent.FPlayer).GetNameEx() + ", " + AbilityRules.TranslateRStr(YouWhereVictoriousRStr);
                DialogHosts.FontModule.AoW15WhiteGrey.SetTextCentered(X, Y, Text, SD);
                break;
        }
    }

    // AOWPyreScrn.pas:110
    public void DeadPnlKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        switch (Key)
        {
            case 13:
            case 27:
                FinishScreen();
                break;
        }
    }

    private static readonly string ContinueRStr = "Continue";

    private static readonly string ReplayRStr = "Replay";

    private static readonly string YouWhereVictoriousRStr = "you were victorious!";

    private static readonly string YourReignHasEndedRStr = "your reign has ended!";

    private static void ShowResults(ResultsWin Window)
    {
        if (GameTables.AoWEngine.FDemo)
        {
            AudioPlayback.PlaySongEx("Songs\\Special\\end2.s3m", 2, 2, true);
        }
        else
        {
            AudioPlayback.PlaySongEx("Songs\\Special\\Sunset.it", 2, 2, true);
        }

        AudioPlayback.SetMusicVolume(0);
        AudioPlayback.FadeMusicVolume(DialogHosts.SoundDlg.MusicVolume.Position, 1);
        Window.FPlayers.Clear();
        for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType != PlayerType.Independent)
            {
                Window.FPlayers.Add((byte)I);
            }
        }

        for (int I = 0; I <= 11; I++)
        {
            ((AoWComponent)Window.FControls[I]).Visible = I < Window.FPlayers.Count;
            bool condition = I < Window.FPlayers.Count && Window.FPlayers[I] == AoWHSMap.Current.FSeatedPlayer;
            ((AOWCheckBox)Window.FControls[I]).Checked = condition;
        }

        if (GameTables.AoWEngine.FCampaign != null && AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FGameOverStatus == GameOverStatus.Lost)
        {
            var replay = AbilityRules.TranslateRStr(ReplayRStr);
            Window.ContinueBtn.Text = replay;
            Window.ExitBtn.Visible = true;
        }
        else
        {
            var continueText = AbilityRules.TranslateRStr(ContinueRStr);
            Window.ContinueBtn.Text = continueText;
            Window.ExitBtn.Visible = false;
        }

        DialogHosts.ResultsWin.ResultsPnl.Visible = true;
    }

    internal static void ClearPyre(PyreScreen Dialog)
    {
        if (Dialog.FAnimation != null)
        {
            (Dialog.FAnimation as IDisposable)?.Dispose();
            Dialog.FAnimation = null;
        }
    }

}
