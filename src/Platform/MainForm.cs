#nullable disable warnings
using Aow.Formats;
using Aow.Game;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Host;
using Aow.Game.Layout;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Platform;

/// <summary>The host window and frame procedure from AoWUnit.pas.</summary>
public sealed class MainForm : IGameHost, IDisplayEvents, IDisposable
{
    public Display Display { get; }
    public SdlDisplayController WinDisplayController { get; }
    public SdlDisplayController DisplayController => WinDisplayController;
    public int FCursor { get; set; }
    public LibraryImage? FCursorImage { get; set; }
    public Rect FLastCursorRectangle { get; private set; }
    public bool FCursorChanged { get; private set; }
    public bool IsClosed { get; private set; }
    public LayoutSession Layouts { get; }
    public UIContext UI { get; }

    private readonly IDisplayHost _host;
    private bool _disposed;
    private bool _created;
    private WindowState _windowState;

    private MainForm(IDisplayHost host, LayoutSession layouts, UIContext ui)
    {
        _host = host;
        Layouts = layouts;
        UI = ui;
        GameHost.MainForm = this;
        Display = new Display(null)
        {
            Host = host,
            EventSink = this,
            Terminated = () => IsClosed
        };
        WinDisplayController = new SdlDisplayController(host);
    }

    public static MainForm Create(IDisplayHost host, string argument = "", bool mute = true, bool audioOutput = false)
    {
        VideoDialog.DesktopSize = () => host.DesktopSize;
        VideoDialog.FullscreenSizes = () => host.FullscreenSizes;
        Startup.InitializeAll();
        UIContext ui = null;
        GameEngine engine = Startup.CreateEngine(new Game.UI.LocalPlayerInput(() => ui));
        SdlSoundSystem.Install(mute, audioOutput);
        LayoutSession layouts = new();
        ui = new(engine);
        try
        {
            GameLayouts.Initialize(layouts, ui: ui);
            if (layouts.Modules.Count != 77)
            {
                throw new InvalidDataException("The startup requires 77 data modules.");
            }
        }
        catch
        {
            ui.Dispose();
            throw;
        }
        MainForm form = new(host, layouts: layouts, ui: ui);
        try
        {
            form.Initialize(argument);
            return form;
        }
        catch
        {
            form.Dispose();
            throw;
        }
    }

    private void Initialize(string argument)
    {
        Display.SetUpdateFrameHandler(DisplayUpdateFrame);
        Display.SetResizeBackSurfaceHandler((_, w, h) => DisplayResizeBackSurface(w, h));
        Display.SetSurfaceDataLostHandler((_, _) => UI.General.Manager.Aligned = false);
        Display.SetSwapDisplayControllerHandler(_ =>
        {
            UI.VideoDialog.ModeChanged = true;
            UI.VideoDialog.ApplyMode();
        });
        Display.PrimaryDisplayController = WinDisplayController;
        Display.SecondaryDisplayController = WinDisplayController;
        FormCreate();
        _created = true;
        _ = Display.ActivateDisplayController(WinDisplayController);
        Display.PresentInitialFrame();
        // The game reads its command line as a path on its own drive, so a host path becomes the file system's public path.
        FormShow(argument.Length == 0 ? argument : RtlHost.Files.ExpandPath(argument));
    }

    private void FormCreate()
    {
        CombatClasses.Register(UI.Engine);
        UI.Engine.InitializeHexSpriteSet($"{UI.Engine.StartupDirectory}RELEASE\\RELEASE.HSS", progress: null);
        _windowState = UI.Engine.Registry.GetMainWindowState();
        UI.SoundDialog.MusicVolumeSlider.Position = UI.Engine.Registry.GetMusicVolume();
        UI.SoundDialog.EffectsVolumeSlider.Position = UI.Engine.Registry.GetSfxVolume();
        SoundSettings settings = UI.SoundDialog.Settings;
        settings.MusicEnabled = UI.Engine.Registry.GetMusicEnabled();
        UI.SoundDialog.Settings = settings;
        SoundSettings settings2 = UI.SoundDialog.Settings;
        settings2.SfxEnabled = UI.Engine.Registry.GetSfxEnabled();
        UI.SoundDialog.Settings = settings2;
        SoundSettings settings3 = UI.SoundDialog.Settings;
        settings3.UseDirectSound = UI.Engine.Registry.GetUseDirectSound();
        UI.SoundDialog.Settings = settings3;
        UI.SoundDialog.SoundQuality = UI.Engine.Registry.GetSoundQuality();
        SoundSettings settings4 = UI.SoundDialog.Settings;
        settings4.SampleVoices = 16;
        UI.SoundDialog.Settings = settings4;
        SoundSettings settings5 = UI.SoundDialog.Settings;
        settings5.SampleRate = UI.SoundDialog.SoundQuality == 1 ? 22050 : 44100;
        UI.SoundDialog.Settings = settings5;
        SoundSettings settings6 = UI.SoundDialog.Settings;
        settings6.OutputMode = UI.SoundDialog.SoundQuality switch
        {
            1 => SoundOutputModes.Mono | SoundOutputModes.Bits8,
            2 => SoundOutputModes.Mono | SoundOutputModes.Bits16,
            3 => SoundOutputModes.Stereo | SoundOutputModes.Bits16,
            _ => SoundOutputModes.Stereo | SoundOutputModes.Bits16 | SoundOutputModes.HighQuality
        };
        UI.SoundDialog.Settings = settings6;
        if (UI.SoundDialog.SoundQuality != 0)
        {
            _ = AudioPlayback.InitializeSoundSystem("SDL", UI.SoundDialog.Settings);
            AudioPlayback.SetMusicVolume(UI.SoundDialog.MusicVolumeSlider.Position);
            AudioPlayback.SetSfxVolume(UI.SoundDialog.EffectsVolumeSlider.Position);
        }
        VideoDialog.SetVideoSize(UI.VideoDialog, UI.Engine.Registry.GetDisplayWidth(), UI.Engine.Registry.GetDisplayHeight());
        VideoDialog.SetVideoMode(UI.VideoDialog, fullScreen: _host.Fullscreen ? VideoDialog.ScreenMode.Game : VideoDialog.ScreenMode.Windowed);
        UI.General.Manager.WinWidth = Display.BackSurfaceWidth;
        UI.General.Manager.WinHeight = Display.BackSurfaceHeight;
        UI.General.Manager.Aligned = false;
        string name = UI.Engine.Registry.GetString(0);
        if (!DelphiStrings.IsEmpty(name))
        {
            UI.MultiWindow.TcpNameEdit.Text = name;
            UI.MultiWindow.IpxNameEdit.Text = name;
        }
        string address = UI.Engine.Registry.GetString(1);
        if (!DelphiStrings.IsEmpty(address))
        {
            UI.MultiWindow.IPAddressEdit.Text = address;
        }

        WindowActions.RestoreWindowLayout(UI.General);
        _ = RtlHost.Files.SetCurrentDirectory(UI.Engine.StartupDirectory);
        UI.Engine.CombatFont = UI.Fonts.Age8;
    }

    // A command-line save names a world or campaign save by its original or own extension (own-formats.md 2.9).
    public static bool IsSaveName(string argument)
        => DocumentKind.Save.HasExtension(Path.GetExtension(argument)) || DocumentKind.CampaignSave.HasExtension(Path.GetExtension(argument));

    private void FormShow(string argument)
    {
        AudioHost.SongPlayer.SetSongChangeHandler(_ => UI.SoundDialog.SongChange());
        UI.Engine.OutputDebugString($"ParamStr: {argument}");
        if (argument.Length != 0 && RtlHost.Files.FileExists(argument) && IsSaveName(argument))
        {
            UI.TitleScene.ShowScene(false);
            GameHost.DeferredSetupAction = 2;
            UI.FileDialogWindow.LastFileName = argument;
            return;
        }
        if (!UI.Engine.Registry.GetDebugMode() && !UI.Engine.Registry.GetSkipIntro())
        {
            UI.IntroScene.Execute();
        }
        else
        {
            UI.TitleScene.ShowScene(true);
        }
    }

    public void DisplayResizeBackSurface(int width, int height)
    {
        UI.General.Manager.WinWidth = Math.Max(640, Display.BackSurfaceWidth);
        UI.General.Manager.WinHeight = Math.Max(480, Display.BackSurfaceHeight);
        UI.General.ResetLayout();
    }

    public void Run(int frames = 0)
    {
        if (frames > 0 && Display.PresentedFrames >= frames)
        {
            return;
        }

        Display.FramePresented = frame =>
        {
            if (frames > 0 && frame >= frames)
            {
                Display.StopUpdateFrameLoop();
            }
        };
        Display.StartUpdateFrameLoop();
    }

    public void DisplayUpdateFrame(object sender, DisplayController displayController, int surfaceType)
    {
        RtlHost.Clock.WaitMilliseconds(UI.Engine.Registry.GetDebugMode() ? 1 : 0);
        switch (GameHost.DeferredSetupAction)
        {
            case 1: GameHost.DeferredSetupAction = 0; _ = UI.ScenarioSetup.SetupControl.PlayerSetupDone(); break;
            case 2: GameHost.DeferredSetupAction = 0; FileDialogWindow.OpenSelectedGame(UI.FileDialogWindow); break;
            default:
                break;
        }
        if (UI.SetupEvents.DirectPlay.DPlay is not null)
        {
            UI.SetupEvents.DirectPlay.CheckMessages();
        }

        AudioPlayback.ProcessSound();
        try
        {
            StrategicMap.Current?.NewFrame();
        }
        catch (Exception error)
        {
            HostLog.CatchDialog(RtlHost.Logger, location: "MainForm.DisplayUpdateFrame.NewFrame", text: "Exception during AoWHSMap.NewFrame", error: error);
            LegacyApplication.ShowMessage("Exception during AoWHSMap.NewFrame");
        }
        if (displayController?.GetSurface(1) is Surface back)
        {
            int x = -1, y = -1;
            Display.GetMousePosition(ref x, ref y);
            Rect rectangle = GetCursorRectangle(x, y);
            FCursorChanged |= (StrategicMap.Current is not null && StrategicMap.Current.Mouse.CursorVisible) ||
                FLastCursorRectangle.Left != rectangle.Left || FLastCursorRectangle.Top != rectangle.Top ||
                FLastCursorRectangle.Right != rectangle.Right || FLastCursorRectangle.Bottom != rectangle.Bottom;
            if (StrategicMap.Current is not null)
            {
                ScrollMap(x, y);
            }

            try
            {
                UI.General.Manager.DrawSurface(back);
            }
            catch (Exception error)
            {
                HostLog.CatchDialog(RtlHost.Logger, location: "MainForm.DisplayUpdateFrame.DrawSurface", text: "Exception during WinManager DrawSurface", error: error);
                LegacyApplication.ShowMessage("Exception during WinManager DrawSurface");
            }
            LockedSurface descriptor = default;
            if (x != -1 && back.Lock(ref descriptor) >= 0)
            {
                try
                {
                    DrawCursor(x, y, surface: descriptor);
                }
                finally
                {
                    _ = back.Unlock();
                }
            }
            FLastCursorRectangle = rectangle;
            UI.DebugWindow.ShowFrameRate(Display.CurrentFrameRate);
        }
        if (StrategicMap.Current is not null && StrategicMap.Current.ExitPending)
        {
            UI.General.ReturnToTitle();
        }

        VideoDialog.ApplyVideoSettings(UI.VideoDialog);
    }

    private int CursorIndex => FCursor == 9999 && StrategicMap.Current is not null ? StrategicMap.Current.Mouse.Cursor : FCursor;

    public Rect GetCursorRectangle(int x, int y)
    {
        Rect rectangle = default;
        if (x == -1)
        {
            return rectangle;
        }

        void Include(LibraryImage? image, int dx, int dy)
        {
            if (image is null)
            {
                return;
            }

            Rect bounds = image.GetVisualDimensions();
            bounds.Left += x + dx;
            bounds.Right += x + dx;
            bounds.Top += y + dy;
            bounds.Bottom += y + dy;
            _ = _cursorBounds.Union(right: bounds, result: out _cursorBounds);
        }
        _cursorBounds = default;
        if (StrategicMap.Current is not null)
        {
            Include(FCursorImage, -15, -20);
            if (CursorIndex != 1)
            {
                Include(GamePack.Current.CursorImages[CursorIndex], -25, -25);
            }
        }
        else if (FCursor != 1 && UI.Graphics.Cursors.ImageExist(FCursor))
        {
            Include(UI.Graphics.Cursors[FCursor], -25, -25);
        }

        rectangle = _cursorBounds;
        return rectangle;
    }

    private Rect _cursorBounds;

    public void DrawCursor(int x, int y, in LockedSurface surface)
    {
        if (x == -1)
        {
            return;
        }

        if (StrategicMap.Current is not null && StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Activated))
        {
            FCursorImage?.Show(x - 15, y - 20, lockedSurface: surface);
            LibraryImage? image = CursorIndex != 1 ? GamePack.Current.CursorImages[CursorIndex] : null;
            if (image is null)
            {
                return;
            }

            if (StrategicMap.Current.SeatedPlayer is not null && StrategicMap.Current.CombatMap is null)
            {
                Player? player = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
                if (player.GetBusy() && player.GameOverStatus == GameOverStatus.None)
                {
                    image.ShowBlended(x - 25, y - 25, lockedSurface: surface, table: GraphicsState.AlphaBlendTable.GetLookupTables(Math.Abs(((StrategicMap.Current.Frame << 2) % 100) - 50) + 30));
                    FCursorChanged = true;
                    return;
                }
            }
            image.Show(x - 25, y - 25, lockedSurface: surface);
        }
        else if (FCursor != 1 && UI.Graphics.Cursors.ImageExist(FCursor))
        {
            UI.Graphics.Cursors[FCursor].ShowClipped(x - 25, y - 25, lockedSurface: surface);
        }
    }

    public void ScrollMap(int x, int y)
    {
        if (General.HasModalWindow(UI.General) || x == -1 || !_host.HasFocus)
        {
            return;
        }

        if (x <= 1)
        {
            UI.MapWindow.MapViewer.SceneX -= 16;
            UI.ScannerWindow.Scanner.SetTLeft((UI.MapWindow.MapViewer.SceneX / 32) - 6);
        }
        if (x >= Display.ActiveDisplayController.GetSurface(0).Width - 1)
        {
            UI.MapWindow.MapViewer.SceneX += 16;
            UI.ScannerWindow.Scanner.SetTLeft((UI.MapWindow.MapViewer.SceneX / 32) - 6);
        }
        if (y <= 1)
        {
            UI.MapWindow.MapViewer.SceneY -= 16;
            UI.ScannerWindow.Scanner.SetTTop((UI.MapWindow.MapViewer.SceneY / 32) - 6);
        }
        if (y >= Display.ActiveDisplayController.GetSurface(0).Height - 1)
        {
            UI.MapWindow.MapViewer.SceneY += 16;
            UI.ScannerWindow.Scanner.SetTTop((UI.MapWindow.MapViewer.SceneY / 32) - 6);
        }
    }

    public void MouseDown(MouseButton button, ShiftStates shift, int x, int y)
    {
        Display.MouseButtons = shift;
        UI.General.Manager.CheckMouseDown(button, shift, x, y);
    }

    public void MouseUp(MouseButton button, ShiftStates shift, int x, int y)
    {
        Display.MouseButtons = shift;
        UI.General.Manager.CheckMouseUp(button, shift, x, y);
        if (StrategicMap.Current?.Mouse is GameMapMouse mouse)
        {
            mouse.ResetRectangleSelection();
        }
    }

    public void MouseMove(ShiftStates shift, int x, int y)
    {
        Display.MouseButtons = shift;
        UI.General.Manager.CheckMouseMove(shift, x, y);
    }

    public void MouseWheel(int steps, ShiftStates shift, int x, int y) => UI.General.Manager.CheckMouseWheel(steps, shift, x, y);

    public void KeyDown(ushort key, ShiftStates shift)
    {
        if (key == 13 && shift.Contains(ShiftStates.Alt))
        {
            _ = Display.SwapDisplayController();
            return;
        }
        UI.General.Manager.CheckKeyDown(ref key, shift);
        if (key == 112 && StrategicMap.Current is not null && StrategicMap.Current.GameType == GameType.Tutorial)
        {
            TutorialWindow.ShowTutorialMessage(UI.TutorialWindow);
        }
    }

    public void KeyPress(byte key) => UI.General.Manager.CheckKeyPress(ref key);

    public void DoubleClick()
    {
        if (StrategicMap.Current is not null && !General.HasModalWindow(UI.General))
        {
            StrategicMap.Current.Mouse.DoubleClick();
        }
    }

    public void Resize(int width, int height) => Display.Resize();

    public void FocusChanged(bool active)
    {
        if (!active)
        {
            Display.MouseButtons = default;
        }
    }

    public bool CloseRequested()
    {
        if (StrategicMap.Current is null)
        {
            return IsClosed = true;
        }

        if (StrategicMap.Current.CombatMap is null)
        {
            StrategicMap.Current.ExitGame(ExitGameMode.Terminate, (_, exited) => IsClosed |= exited);
            return IsClosed |= StrategicMap.Current.ExitPending;
        }
        UI.ConfirmationDialog.Confirm(UI.Engine.TranslateResourceString(PlayerControl.ConfirmText), text: UI.Engine.TranslateResourceString(PopupWindows.QuitCombatText),
            onConfirmed: PopupWindows.SurrenderCombat);
        return false;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        Display?.Dispose();
        WinDisplayController?.Dispose();
        AudioPlayback.ReleaseSoundSystem();
        if (_created)
        {
            UI.General.SaveLayout();
            GameSettings registry = UI.Engine.Registry;
            registry.SetMainWindowState(_windowState);
            registry.SetString(0, value: UI.MultiWindow.TcpNameEdit.Text);
            registry.SetString(1, value: UI.MultiWindow.IPAddressEdit.Text);
            registry.SetDisplayWidth(UI.VideoDialog.DisplayWidth);
            registry.SetDisplayHeight(UI.VideoDialog.DisplayHeight);
            registry.SetDisplayMode(_host.Fullscreen ? 1 : 0);
            registry.SetUseDirectSound(UI.SoundDialog.Settings.UseDirectSound);
            registry.SetSfxEnabled(UI.SoundDialog.Settings.SfxEnabled);
            registry.SetMusicEnabled(UI.SoundDialog.Settings.MusicEnabled);
            registry.SetSoundQuality(UI.SoundDialog.SoundQuality);
            registry.SetSfxVolume(UI.SoundDialog.EffectsVolumeSlider.Position);
            registry.SetMusicVolume(UI.SoundDialog.MusicVolumeSlider.Position);
        }
        UI.Dispose();
        if (GameHost.MainForm == this)
        {
            GameHost.MainForm = null;
        }
    }
}
