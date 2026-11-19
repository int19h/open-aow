#nullable disable
// Translated from AOWPyreScrn.pas.
using Aow.Game.Audio;

using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiFiles;
using static Aow.Rtl.DelphiPaths;

namespace Aow.Game.UI.Dialogs;

// game-over event, animation frame
public partial class PyreScreen(UIContext ui) : UIModule(ui)
{
    public Panel DeadPanel { get; private set; }
    public Image LeaderFaceFrame { get; private set; }
    public UserDrawBox PyreAnimationView { get; private set; }

    private object _animation;

    public SongStatus SavedSongStatus { get; set; }

    public event ExecuteDoneEvent Done;

    // The results window ends the pyre screen: it takes the done handler, the slot is empty
    // before the handler runs, and the handler runs once.
    internal ExecuteDoneEvent TakeDoneHandler()
    {
        ExecuteDoneEvent done = Done;
        Done = null;
        return done;
    }

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
        Done = done;
        SavedSongStatus = AudioPlayback.BackupSongStatus();
        AudioPlayback.FadeMusicVolume(0, 1);
        UI.General.Manager.OverlayPriority = 249;
        switch (StrategicMap.Current.Players.GetPlayers(eventValue.Player).GameOverStatus)
        {
            case GameOverStatus.Lost:
                _animation = AudioPlayback.PlaySample("Sfx\\Attack\\Fireloop.wav", 64, true, 0, SamplePanning.Center, null);
                StrategicMap.Current.TutorialControl.ShowMessage(68, true, true);
                break;
            case GameOverStatus.Won:
                StrategicMap.Current.TutorialControl.ShowMessage(67, true, true);
                break;
            case GameOverStatus.None:
            default:
                break;
        }

        UI.Graphics.PyreFrames.LoadDiscardableImageData();
        UI.TitleWindow.TitlePanel.Visible = false;
        UI.PyreScreen.DeadPanel.Visible = true;
        UI.VideoDialog.SetSceneSize(640, 480);
        VideoDialog.SetVideoMode(UI.VideoDialog, VideoDialog.ScreenMode.Title);
        UI.VideoDialog.ApplyMode();
        UI.TitleWindow.MainWindow.Visible = true;
        _ = SetCurrentDir(ExtractFileDir(LegacyApplication.Application.ExeName));
    }

    // AOWPyreScrn.pas:48
    public void DeadPnlUpdate()
    {
    }

    // AOWPyreScrn.pas:51
    public void DeadPnlMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y) => FinishScreen();

    // AOWPyreScrn.pas:54
    public void LeaderFaceFrPreDraw(LockedSurface lockedSurface)
    {
        Rect r = default;
        if (StrategicMap.Current != null)
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
        PyreAnimationView.Drawn = false;
        DeadPanel.Drawn = false;
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
        int x;
        int y;
        int frame;
        string text;
        x = PyreAnimationView.WinWidth / 2;
        y = PyreAnimationView.WinHeight + 20 - (_animationFrame / 2);
        if (y < 400)
        {
            y = 400;
        }

        y += DeadPanel.AbsoluteTop;
        x += DeadPanel.AbsoluteLeft;
        switch (StrategicMap.Current.Players.GetPlayers(GameOverEvent.Player).GameOverStatus)
        {
            case GameOverStatus.Lost:
                frame = _animationFrame / 3 % 9;
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
    public void DeadPnlKeyDown(in ushort key, Set<ShiftState> shift)
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
            ? AudioPlayback.PlaySong("Songs\\Special\\end2.s3m", 2, 2, true)
            : AudioPlayback.PlaySong("Songs\\Special\\Sunset.it", 2, 2, true);

        AudioPlayback.SetMusicVolume(0);
        AudioPlayback.FadeMusicVolume(window.UI.SoundDialog.MusicVolumeSlider.Position, 1);
        window.Players.Clear();
        for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            if (StrategicMap.Current.Players.GetPlayers(i).PlayerType != PlayerType.Independent)
            {
                _ = window.Players.Add((byte)i);
            }
        }

        for (int i = 0; i <= 11; i++)
        {
            window.Controls[i].Visible = i < window.Players.Count;
            bool condition = i < window.Players.Count && window.Players[i] == StrategicMap.Current.SeatedPlayer;
            window.Controls[i].Checked = condition;
        }

        if (window.UI.Engine.Campaign != null && StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GameOverStatus == GameOverStatus.Lost)
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
        if (dialog._animation != null)
        {
            (dialog._animation as IDisposable)?.Dispose();
            dialog._animation = null;
        }
    }

}
