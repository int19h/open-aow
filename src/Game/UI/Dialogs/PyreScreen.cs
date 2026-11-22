#nullable disable warnings
// Translated from AOWPyreScrn.pas.
using Aow.Game.Audio;

using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.DelphiPaths;

namespace Aow.Game.UI.Dialogs;

// game-over event, animation frame
public sealed partial class PyreScreen(UIContext ui) : UIModule(ui)
{
    public Panel DeadPanel { get; private set; }
    public Image LeaderFaceFrame { get; private set; }
    public UserDrawBox PyreAnimationView { get; private set; }

    private object? _animation;

    public SongStatus? SavedSongStatus { get; set; }

    private ExecuteDoneEvent? _done;

    // The results window ends the pyre screen: it takes the done handler, the slot is empty
    // before the handler runs, and the handler runs once.
    internal ExecuteDoneEvent? TakeDoneHandler()
    {
        ExecuteDoneEvent? done = _done;
        _done = null;
        return done;
    }

    public GameOverEventLog? GameOverEvent { get; set; }

    private int _animationFrame;

    // AOWPyreScrn.pas:114
    public void ClearFire()
    {
        if (_animation is not null)
        {
            (_animation as IDisposable)?.Dispose();
            _animation = null;
        }
    }

    // AOWPyreScrn.pas:119
    public void FinishScreen()
    {
        ClearFire();
        UI.Graphics.PyreFrames.ReleaseDiscardableImageData();
        UI.PyreScreen.DeadPanel.Visible = false;
        ShowResults(UI.ResultsWindow);
    }

    // AOWPyreScrn.pas:127
    public void ShowGameOver(GameOverEventLog eventValue, ExecuteDoneEvent done)
    {
        _animationFrame = 0;
        GameOverEvent = eventValue;
        _ = GameOverEvent.AddRef();
        _done = done;
        SavedSongStatus = AudioPlayback.BackupSongStatus();
        _ = (AudioHost.SoundSystem?.FadeMusicVolume(0, speed: 1));
        UI.General.Manager.OverlayPriority = 249;
        switch (StrategicMap.Current.Players.GetPlayers(eventValue.Player).GameOverStatus)
        {
            case GameOverStatus.Lost:
                _animation = AudioPlayback.PlaySample("Sfx\\Attack\\Fireloop.wav", volume: 64, looped: true, delay: 0, panning: SamplePanning.Center, callback: null);
                StrategicMap.Current.TutorialControl.ShowMessage(68, immediate: true, onceOnly: true);
                break;
            case GameOverStatus.Won:
                StrategicMap.Current.TutorialControl.ShowMessage(67, immediate: true, onceOnly: true);
                break;
            case GameOverStatus.None:
            default:
                break;
        }

        UI.Graphics.PyreFrames.LoadDiscardableImageData();
        UI.TitleWindow.TitlePanel.Visible = false;
        UI.PyreScreen.DeadPanel.Visible = true;
        UI.VideoDialog.SetSceneSize(640, H: 480);
        VideoDialog.SetVideoMode(UI.VideoDialog, fullScreen: VideoDialog.ScreenMode.Title);
        UI.VideoDialog.ApplyMode();
        UI.TitleWindow.MainWindow.Visible = true;
        _ = RtlHost.Files.SetCurrentDirectory(ExtractFileDir(LegacyApplication.Application.ExeName));
    }

    // AOWPyreScrn.pas:48
    public void DeadPnlUpdate()
    {
    }

    // AOWPyreScrn.pas:51
    public void DeadPnlMouseDown(MouseButton button, ShiftStates shift, int X, int Y) => FinishScreen();

    // AOWPyreScrn.pas:54
    public void LeaderFaceFrPreDraw(LockedSurface lockedSurface)
    {
        Rect r = default;
        if (StrategicMap.Current is not null)
        {
            r.Left = DeadPanel.AbsoluteLeft + LeaderFaceFrame.WinLeft + 45;
            r.Right = r.Left + 103;
            r.Top = DeadPanel.AbsoluteTop + LeaderFaceFrame.WinTop + 35;
            r.Bottom = r.Top + 128;
            StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Leader.ShowFace(r, lockedSurface);
        }
    }

    // AOWPyreScrn.pas:69
    public void PyreAnimationUpdate()
    {
        PyreAnimationView.IsDrawn = false;
        DeadPanel.IsDrawn = false;
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
        int x = PyreAnimationView.WinWidth / 2;
        int y = PyreAnimationView.WinHeight + 20 - (_animationFrame / 2);
        if (y < 400)
        {
            y = 400;
        }

        y += DeadPanel.AbsoluteTop;
        x += DeadPanel.AbsoluteLeft;
        string text;
        switch (StrategicMap.Current.Players.GetPlayers(GameOverEvent.Player).GameOverStatus)
        {
            case GameOverStatus.Lost:
                int frame = _animationFrame / 3 % 9;
                if (UI.Engine.Demo)
                {
                    frame = 0;
                }

                UI.Graphics.PyreFrames[frame].Show(DeadPanel.AbsoluteLeft + PyreAnimationView.WinLeft, DeadPanel.AbsoluteTop + PyreAnimationView.WinTop, lockedSurface);
                text = $"{StrategicMap.Current.Players.GetPlayers(GameOverEvent.Player).GetNameWithRace()}, {UI.Engine.TranslateResourceString(s_yourReignHasEndedText)}";
                UI.Fonts.Age15BlackOnBrown.SetTextCentered(x, y, text, lockedSurface);
                break;
            case GameOverStatus.Won:
                UI.Graphics.VictoryFrames[0].Show(DeadPanel.AbsoluteLeft + PyreAnimationView.WinLeft, DeadPanel.AbsoluteTop, lockedSurface);
                text = $"{StrategicMap.Current.Players.GetPlayers(GameOverEvent.Player).GetNameWithRace()}, {UI.Engine.TranslateResourceString(s_youWereVictoriousText)}";
                UI.Fonts.Age15WhiteOnGrey.SetTextCentered(x, y, text, lockedSurface);
                break;
            case GameOverStatus.None:
            default:
                break;
        }
    }

    // AOWPyreScrn.pas:110
    public void DeadPnlKeyDown(in int key, ShiftStates shift)
    {
        switch (key)
        {
            case 13:
            case 27:
                FinishScreen();
                break;
            default:
                break;
        }
    }

    private static readonly string s_continueText = "Continue";

    private static readonly string s_replayText = "Replay";

    private static readonly string s_youWereVictoriousText = "you were victorious!";

    private static readonly string s_yourReignHasEndedText = "your reign has ended!";

    private static void ShowResults(ResultsWindow window)
    {
        _ = window.UI.Engine.Demo
            ? AudioPlayback.PlaySong("Songs\\Special\\end2.s3m", fadeOut: 2, fadeIn: 2, loop: true)
            : AudioPlayback.PlaySong("Songs\\Special\\Sunset.it", fadeOut: 2, fadeIn: 2, loop: true);

        _ = (AudioHost.SoundSystem?.SetMusicVolume(0));
        AudioPlayback.FadeMusicVolume(window.UI.SoundDialog.MusicVolumeSlider.Position, speed: 1);
        window.Players.Clear();
        for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            if (StrategicMap.Current.Players.GetPlayers(i).PlayerType != PlayerType.Independent)
            {
                _ = window.Players.Add(i.TruncateToByte());
            }
        }

        for (int i = 0; i <= 11; i++)
        {
            window.Controls[i].Visible = i < window.Players.Count;
            bool condition = i < window.Players.Count && window.Players[i] == StrategicMap.Current.SeatedPlayer;
            window.Controls[i].Checked = condition;
        }

        if (window.UI.Engine.Campaign is not null && StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GameOverStatus == GameOverStatus.Lost)
        {
            string replay = window.UI.Engine.TranslateResourceString(s_replayText);
            window.ContinueButton.Text = replay;
            window.ExitButton.Visible = true;
        }
        else
        {
            string continueText = window.UI.Engine.TranslateResourceString(s_continueText);
            window.ContinueButton.Text = continueText;
            window.ExitButton.Visible = false;
        }

        window.UI.ResultsWindow.ResultsPanel.Visible = true;
    }

    internal static void ClearPyre(PyreScreen dialog)
    {
        if (dialog._animation is not null)
        {
            (dialog._animation as IDisposable)?.Dispose();
            dialog._animation = null;
        }
    }
}
