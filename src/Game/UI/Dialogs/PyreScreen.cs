// Translated from AOWPyreScrn.pas.
using Aow.Game.Audio;

using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// game-over event, animation frame
public partial class PyreScreen : DataModule
{
    public PyreScreen(Component owner) : base(owner)
    { }

    public Panel DeadPnl;
    public Image PyreTop;
    public Image LeaderFaceFr;
    public UserDrawBox PyreAnimation;
    private object _animation;

    public SongStatus SavedSongStatus { get; set; }
    public ExecuteDoneEvent Done { get; set; }
    public GameOverEventLog GameOverEvent { get; set; }

    private int _animationFrame;

    // AOWPyreScrn.pas:114
    public void ClearFire()
    {
        if (_animation != null)
        {
            (_animation as IDisposable)?.Dispose();
            _animation = null;
        }
    }

    // AOWPyreScrn.pas:119
    public void FinishScreen()
    {
        ClearFire();
        DialogHosts.UIGraphics.PyreFrames.ReleaseDiscardableImageData();
        DialogHosts.PyreScreen.DeadPnl.Visible = false;
        ShowResults(DialogHosts.ResultsWindow);
    }

    // AOWPyreScrn.pas:127
    public void ShowGameOver(GameOverEventLog eventValue, ExecuteDoneEvent done)
    {
        _animationFrame = 0;
        GameOverEvent = eventValue;
        GameOverEvent.AddRef();
        Done = done;
        SavedSongStatus = AudioPlayback.BackupSongStatus();
        AudioPlayback.FadeMusicVolume(0, 1);
        DialogHosts.General.Manager.OverlayPriority = 249;
        switch (AoWHSMap.Current.Players.GetPlayers(eventValue._player).GameOverStatus)
        {
            case GameOverStatus.Lost:
                _animation = AudioPlayback.PlaySample("Sfx\\Attack\\Fireloop.wav", 64, true, 0, SamplePanning.Center, null);
                AoWHSMap.Current.TutorialControl.ShowMessage(68, true, true);
                break;
            case GameOverStatus.Won:
                AoWHSMap.Current.TutorialControl.ShowMessage(67, true, true);
                break;
        }

        DialogHosts.UIGraphics.PyreFrames.LoadDiscardableImageData();
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
    public void DeadPnlMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        FinishScreen();
    }

    // AOWPyreScrn.pas:54
    public void LeaderFaceFrPreDraw(LockedSurface lockedSurface)
    {
        Rect R = default;
        if (AoWHSMap.Current != null)
        {
            R.Left = DeadPnl.WinLeft + LeaderFaceFr.WinLeft + 45;
            R.Right = R.Left + 103;
            R.Top = DeadPnl.WinTop + LeaderFaceFr.WinTop + 35;
            R.Bottom = R.Top + 128;
            AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Leader.ShowFace(R, lockedSurface);
        }
    }

    // AOWPyreScrn.pas:69
    public void PyreAnimationUpdate()
    {
        PyreAnimation.Drawn = false;
        DeadPnl.Drawn = false;
        _animationFrame += 1;
        if (_animationFrame > 450)
        {
            FinishScreen();
        }

        if (_animationFrame > 16777215)
        {
            _animationFrame = 0;
        }
    }

    // AOWPyreScrn.pas:77
    public void PyreAnimationDraw(LockedSurface lockedSurface)
    {
        int X;
        int Y;
        int frame;
        string text;
        X = PyreAnimation.WinWidth / 2;
        Y = PyreAnimation.WinHeight + 20 - _animationFrame / 2;
        if (Y < 400)
        {
            Y = 400;
        }

        Y += DeadPnl.WinTop;
        X += DeadPnl.WinLeft;
        switch (AoWHSMap.Current.Players.GetPlayers(GameOverEvent._player).GameOverStatus)
        {
            case GameOverStatus.Lost:
                frame = _animationFrame / 3 % 9;
                if (GameTables.GameEngine.Demo)
                {
                    frame = 0;
                }

                DialogHosts.UIGraphics.PyreFrames[frame].Show(DeadPnl.WinLeft + PyreAnimation.WinLeft, DeadPnl.WinTop + PyreAnimation.WinTop, lockedSurface);
                text = AoWHSMap.Current.Players.GetPlayers(GameOverEvent._player).GetNameWithRace() + ", " + AbilityRules.TranslateResourceString(s_yourReignHasEndedText);
                DialogHosts.UIFonts.Font15BlackBrown.SetTextCentered(X, Y, text, lockedSurface);
                break;
            case GameOverStatus.Won:
                DialogHosts.UIGraphics.VictoryFrames[0].Show(DeadPnl.WinLeft + PyreAnimation.WinLeft, DeadPnl.WinTop, lockedSurface);
                text = AoWHSMap.Current.Players.GetPlayers(GameOverEvent._player).GetNameWithRace() + ", " + AbilityRules.TranslateResourceString(s_youWereVictoriousText);
                DialogHosts.UIFonts.Font15WhiteGrey.SetTextCentered(X, Y, text, lockedSurface);
                break;
        }
    }

    // AOWPyreScrn.pas:110
    public void DeadPnlKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        switch (key)
        {
            case 13:
            case 27:
                FinishScreen();
                break;
        }
    }

    private static readonly string s_continueText = "Continue";

    private static readonly string s_replayText = "Replay";

    private static readonly string s_youWereVictoriousText = "you were victorious!";

    private static readonly string s_yourReignHasEndedText = "your reign has ended!";

    private static void ShowResults(ResultsWindow window)
    {
        if (GameTables.GameEngine.Demo)
        {
            AudioPlayback.PlaySong("Songs\\Special\\end2.s3m", 2, 2, true);
        }
        else
        {
            AudioPlayback.PlaySong("Songs\\Special\\Sunset.it", 2, 2, true);
        }

        AudioPlayback.SetMusicVolume(0);
        AudioPlayback.FadeMusicVolume(DialogHosts.SoundDialog.MusicVolume.Position, 1);
        window.Players.Clear();
        for (int I = 1, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
        {
            if (AoWHSMap.Current.Players.GetPlayers(I).PlayerType != PlayerType.Independent)
            {
                window.Players.Add((byte)I);
            }
        }

        for (int I = 0; I <= 11; I++)
        {
            (window.Controls[I]).Visible = I < window.Players.Count;
            bool condition = I < window.Players.Count && window.Players[I] == AoWHSMap.Current.SeatedPlayer;
            (window.Controls[I]).Checked = condition;
        }

        if (GameTables.GameEngine.Campaign != null && AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GameOverStatus == GameOverStatus.Lost)
        {
            var replay = AbilityRules.TranslateResourceString(s_replayText);
            window.ContinueBtn.Text = replay;
            window.ExitBtn.Visible = true;
        }
        else
        {
            var continueText = AbilityRules.TranslateResourceString(s_continueText);
            window.ContinueBtn.Text = continueText;
            window.ExitBtn.Visible = false;
        }

        DialogHosts.ResultsWindow.ResultsPnl.Visible = true;
    }

    internal static void ClearPyre(PyreScreen dialog)
    {
        if (dialog._animation != null)
        {
            (dialog._animation as IDisposable)?.Dispose();
            dialog._animation = null;
        }
    }

}
