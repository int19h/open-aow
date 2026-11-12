#nullable disable
using Aow.Formats;
using Aow.Game;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Host;
using Aow.Game.Layout;
using Aow.Game.Magic.Abilities;
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
    public short FCursor { get; set; }
    public LibraryImage FCursorImage { get; set; }
    public Rect FLastCursorRectangle { get; private set; }
    public bool FCursorChanged { get; private set; }
    public bool IsClosed { get; private set; }
    public LayoutSession Layouts { get; }

    private readonly IDisplayHost _host;
    private bool _disposed;
    private bool _created;
    private WindowState _windowState;

    private MainForm(IDisplayHost host, LayoutSession layouts)
    {
        this._host = host;
        Layouts = layouts;
        GameHost.MainForm = this;
        Display = new Display(null);
        Display.Host = host;
        Display.EventSink = this;
        Display.Terminated = () => IsClosed;
        WinDisplayController = new SdlDisplayController(host);
    }

    public static MainForm Create(IDisplayHost host, string argument = "", bool mute = true, bool audioOutput = false)
    {
        VideoDialog.DesktopSize = () => host.DesktopSize;
        VideoDialog.FullscreenSizes = () => host.FullscreenSizes;
        Startup.InitializeAll();
        SdlSoundSystem.Install(mute, audioOutput);
        var layouts = new LayoutSession();
        GameLayouts.Initialize(layouts);
        if (layouts.Modules.Count != 77) throw new InvalidDataException("The startup requires 77 data modules.");
        MainForm form = new MainForm(host, layouts);
        try { form.Initialize(argument); return form; }
        catch { form.Dispose(); throw; }
    }

    private void Initialize(string argument)
    {
        Display.OnUpdateFrame = DisplayUpdateFrame;
        Display.OnResizeBackSurface = (_, w, h) => DisplayResizeBackSurface(w, h);
        Display.OnSurfaceDataLost = (_, _) => DialogHosts.General.Manager.Aligned = false;
        Display.OnSwapDisplayController = _ => { DialogHosts.VideoDialog.ModeChanged = true; DialogHosts.VideoDialog.ApplyMode(); };
        Display.PrimaryDisplayController = WinDisplayController;
        Display.SecondaryDisplayController = WinDisplayController;
        FormCreate();
        _created = true;
        Display.ActivateDisplayController(WinDisplayController);
        Display.PresentInitialFrame();
        // The game reads its command line as a path on its own drive, so a host path becomes the file system's public path.
        FormShow(argument.Length == 0 ? argument : RtlUnit.Files.ExpandPath(argument));
    }

    private void FormCreate()
    {
        CombatClasses.Register(GameTables.GameEngine);
        GameTables.GameEngine.InitializeHexSpriteSet(GameTables.GameEngine.StartupDirectory + "RELEASE\\RELEASE.HSS", null);
        _windowState = GameTables.GameEngine.Registry.GetMainWindowState();
        DialogHosts.SoundDialog.MusicVolumeSlider.Position = GameTables.GameEngine.Registry.GetMusicVolume();
        DialogHosts.SoundDialog.EffectsVolumeSlider.Position = GameTables.GameEngine.Registry.GetSfxVolume();
        DialogHosts.SoundDialog.Settings.MusicEnabled = GameTables.GameEngine.Registry.GetMusicEnabled();
        DialogHosts.SoundDialog.Settings.SfxEnabled = GameTables.GameEngine.Registry.GetSfxEnabled();
        DialogHosts.SoundDialog.Settings.UseDirectSound = GameTables.GameEngine.Registry.GetUseDirectSound();
        DialogHosts.SoundDialog.SoundQuality = GameTables.GameEngine.Registry.GetSoundQuality();
        DialogHosts.SoundDialog.Settings.SampleVoices = 16;
        DialogHosts.SoundDialog.Settings.SampleRate = DialogHosts.SoundDialog.SoundQuality == 1 ? 22050 : 44100;
        DialogHosts.SoundDialog.Settings.OutputMode = DialogHosts.SoundDialog.SoundQuality switch
        {
            1 => Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Mono, SoundOutputModeElement.Bits8),
            2 => Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Mono, SoundOutputModeElement.Bits16),
            3 => Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.Bits16),
            _ => Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.Bits16, SoundOutputModeElement.HighQuality)
        };
        if (DialogHosts.SoundDialog.SoundQuality != 0)
        {
            AudioPlayback.InitializeSoundSystem("SDL", DialogHosts.SoundDialog.Settings);
            AudioPlayback.SetMusicVolume(DialogHosts.SoundDialog.MusicVolumeSlider.Position);
            AudioPlayback.SetSfxVolume(DialogHosts.SoundDialog.EffectsVolumeSlider.Position);
        }
        VideoDialog.SetVideoSize(DialogHosts.VideoDialog, GameTables.GameEngine.Registry.GetDisplayWidth(), GameTables.GameEngine.Registry.GetDisplayHeight());
        VideoDialog.SetVideoMode(DialogHosts.VideoDialog, _host.Fullscreen ? VideoDialog.ScreenMode.Game : VideoDialog.ScreenMode.Windowed);
        DialogHosts.General.Manager.WinWidth = Display.BackSurfaceWidth;
        DialogHosts.General.Manager.WinHeight = Display.BackSurfaceHeight;
        DialogHosts.General.Manager.Aligned = false;
        var name = GameTables.GameEngine.Registry.GetString(0);
        if (!RtlUnit.IsEmpty(name)) { DialogHosts.MultiWindow.TcpNameEdit.Text = name; DialogHosts.MultiWindow.IpxNameEdit.Text = name; }
        var address = GameTables.GameEngine.Registry.GetString(1);
        if (!RtlUnit.IsEmpty(address)) DialogHosts.MultiWindow.IPAddressEdit.Text = address;
        WindowActions.RestoreWindowLayout(DialogHosts.General);
        RtlUnit.SetCurrentDir(GameTables.GameEngine.StartupDirectory);
        GameTables.GameEngine.CombatFont = DialogHosts.UIFonts.Age8;
    }

    // A command-line save names a world or campaign save by its original or own extension (own-formats.md 2.9).
    public static bool IsSaveName(string argument) =>
        DocumentKind.Save.HasExtension(Path.GetExtension(argument)) || DocumentKind.CampaignSave.HasExtension(Path.GetExtension(argument));

    private void FormShow(string argument)
    {
        AudioHost.SongPlayer.OnSongChange = _ => DialogHosts.SoundDialog.SongChange();
        GameTables.GameEngine.OutputDebugString("ParamStr: " + argument);
        if (argument.Length != 0 && RtlUnit.FileExists(argument) && IsSaveName(argument))
        {
            WindowActions.ShowTitleScene(DialogHosts.TitleScene, false);
            GameHost.DeferredSetupAction = 2;
            DialogHosts.FileDialogWindow.LastFileName = argument;
            return;
        }
        if (!GameTables.GameEngine.Registry.GetDebugMode() && !GameTables.GameEngine.Registry.GetSkipIntro()) DialogHosts.IntroScene.Execute();
        else WindowActions.ShowTitleScene(DialogHosts.TitleScene, true);
    }

    public void DisplayResizeBackSurface(int width, int height)
    {
        DialogHosts.General.Manager.WinWidth = Math.Max(640, Display.BackSurfaceWidth);
        DialogHosts.General.Manager.WinHeight = Math.Max(480, Display.BackSurfaceHeight);
        DialogHosts.General.ResetLayout();
    }

    public void Run(int frames = 0)
    {
        if (frames > 0 && Display.PresentedFrames >= frames) return;
        Display.FramePresented = frame => { if (frames > 0 && frame >= frames) Display.StopUpdateFrameLoop(); };
        Display.StartUpdateFrameLoop();
    }

    public void DisplayUpdateFrame(object sender, CustomDisplayController displayController, int surfaceType)
    {
        RtlUnit.Clock.WaitMilliseconds(GameTables.GameEngine.Registry.GetDebugMode() ? 1 : 0);
        switch (GameHost.DeferredSetupAction)
        {
            case 1: GameHost.DeferredSetupAction = 0; DialogHosts.ScenarioSetup.SetupControl.PlayerSetupDone(); break;
            case 2: GameHost.DeferredSetupAction = 0; FileDialogWindow.OpenSelectedGame(DialogHosts.FileDialogWindow); break;
        }
        if (DialogHosts.SetupEvents.DirectPlay.DPlay is not null)
            DialogHosts.SetupEvents.DirectPlay.CheckMessages();
        AudioPlayback.ProcessSound();
        try { AoWHSMap.Current?.NewFrame(); }
        catch (Exception error) { Console.Error.WriteLine(error); RtlUnit.ShowMessage("Exception during AoWHSMap.NewFrame"); }
        if (displayController?.GetSurface(1) is Surface back)
        {
            int x = -1, y = -1;
            Display.GetMousePosition(ref x, ref y);
            Rect rectangle = default;
            GetCursorRectangle(x, y, ref rectangle);
            FCursorChanged |= (AoWHSMap.Current is not null && AoWHSMap.Current.Mouse.CursorVisible) ||
                FLastCursorRectangle.Left != rectangle.Left || FLastCursorRectangle.Top != rectangle.Top ||
                FLastCursorRectangle.Right != rectangle.Right || FLastCursorRectangle.Bottom != rectangle.Bottom;
            if (AoWHSMap.Current is not null) ScrollMap(x, y);
            try { DialogHosts.General.Manager.DrawSurface(back); }
            catch (Exception error) { Console.Error.WriteLine(error); RtlUnit.ShowMessage("Exception during WinManager DrawSurface"); }
            LockedSurface descriptor = default;
            if (x != -1 && back.Lock(ref descriptor) >= 0)
            {
                try { DrawCursor(x, y, descriptor); }
                finally { back.Unlock(); }
            }
            FLastCursorRectangle = rectangle;
            DialogHosts.ConfirmationDialog.FrameRateLabel.Text = RtlUnit.IntToStr(Display.CurrentFrameRate);
        }
        if (AoWHSMap.Current is not null && AoWHSMap.Current.ExitPending) DialogHosts.General.ReturnToTitle();
        VideoDialog.ApplyVideoSettings(DialogHosts.VideoDialog);
    }

    private int CursorIndex => FCursor == 9999 && AoWHSMap.Current is not null ? AoWHSMap.Current.Mouse.Cursor : FCursor;

    public void GetCursorRectangle(int x, int y, ref Rect rectangle)
    {
        rectangle = default;
        if (x == -1) return;
        void Include(LibraryImage image, int dx, int dy)
        {
            if (image is null) return;
            Rect bounds = default;
            image.GetVisualDimensions(ref bounds);
            bounds.Left += x + dx; bounds.Right += x + dx;
            bounds.Top += y + dy; bounds.Bottom += y + dy;
            RtlUnit.UnionRect(ref _cursorBounds, _cursorBounds, bounds);
        }
        _cursorBounds = default;
        if (AoWHSMap.Current is not null)
        {
            Include(FCursorImage, -15, -20);
            if (CursorIndex != 1) Include(AoWHSSet.Current.CursorImages[CursorIndex], -25, -25);
        }
        else if (FCursor != 1 && DialogHosts.UIGraphics.Cursors.ImageExist(FCursor)) Include(DialogHosts.UIGraphics.Cursors[FCursor], -25, -25);
        rectangle = _cursorBounds;
    }

    private Rect _cursorBounds;

    public void DrawCursor(int x, int y, in LockedSurface surface)
    {
        if (x == -1) return;
        if (AoWHSMap.Current is not null && AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Activated))
        {
            FCursorImage?.Show(x - 15, y - 20, surface);
            var image = CursorIndex != 1 ? AoWHSSet.Current.CursorImages[CursorIndex] : null;
            if (image is null) return;
            if (AoWHSMap.Current.SeatedPlayer != -1 && AoWHSMap.Current.CombatMap is null)
            {
                var player = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
                if (player.GetBusy() && player.GameOverStatus == GameOverStatus.None)
                {
                    image.ShowBlended(x - 25, y - 25, surface, GraphicsState.AlphaBlendTable.GetLookupTables(Math.Abs((AoWHSMap.Current.Frame << 2) % 100 - 50) + 30));
                    FCursorChanged = true;
                    return;
                }
            }
            image.Show(x - 25, y - 25, surface);
        }
        else if (FCursor != 1 && DialogHosts.UIGraphics.Cursors.ImageExist(FCursor)) DialogHosts.UIGraphics.Cursors[FCursor].ShowClipped(x - 25, y - 25, surface);
    }

    public void ScrollMap(int x, int y)
    {
        if (General.HasModalWindow(DialogHosts.General) || x == -1 || !_host.HasFocus) return;
        if (x <= 1) { DialogHosts.MapWindow.MapViewer.SceneX -= 16; DialogHosts.ScannerWindow.Scanner.SetTLeft(DialogHosts.MapWindow.MapViewer.SceneX / 32 - 6); }
        if (x >= Display.ActiveDisplayController.GetSurface(0).Width - 1) { DialogHosts.MapWindow.MapViewer.SceneX += 16; DialogHosts.ScannerWindow.Scanner.SetTLeft(DialogHosts.MapWindow.MapViewer.SceneX / 32 - 6); }
        if (y <= 1) { DialogHosts.MapWindow.MapViewer.SceneY -= 16; DialogHosts.ScannerWindow.Scanner.SetTTop(DialogHosts.MapWindow.MapViewer.SceneY / 32 - 6); }
        if (y >= Display.ActiveDisplayController.GetSurface(0).Height - 1) { DialogHosts.MapWindow.MapViewer.SceneY += 16; DialogHosts.ScannerWindow.Scanner.SetTTop(DialogHosts.MapWindow.MapViewer.SceneY / 32 - 6); }
    }

    public void MouseDown(MouseButton button, Set<ShiftState> shift, int x, int y) { Display.MouseButtons = shift; DialogHosts.General.Manager.CheckMouseDown(button, shift, x, y); }

    public void MouseUp(MouseButton button, Set<ShiftState> shift, int x, int y)
    {
        Display.MouseButtons = shift;
        DialogHosts.General.Manager.CheckMouseUp(button, shift, x, y);
        if (AoWHSMap.Current?.Mouse is GameMapMouse mouse) mouse.ResetRectangleSelection();
    }

    public void MouseMove(Set<ShiftState> shift, int x, int y) { Display.MouseButtons = shift; DialogHosts.General.Manager.CheckMouseMove(shift, x, y); }

    public void MouseWheel(int steps, Set<ShiftState> shift, int x, int y) => DialogHosts.General.Manager.CheckMouseWheel(steps, shift, x, y);

    public void KeyDown(ushort key, Set<ShiftState> shift)
    {
        if (key == 13 && shift.Contains(ShiftState.Alt)) { Display.SwapDisplayController(); return; }
        DialogHosts.General.Manager.CheckKeyDown(ref key, shift);
        if (key == 112 && AoWHSMap.Current is not null && AoWHSMap.Current.GameType == GameType.Tutorial) TutorialWindow.ShowTutorialMessage(GameLayouts.TutorialWindow);
    }

    public void KeyPress(byte key) => DialogHosts.General.Manager.CheckKeyPress(ref key);

    public void DoubleClick() { if (AoWHSMap.Current is not null && !General.HasModalWindow(DialogHosts.General)) AoWHSMap.Current.Mouse.DoubleClick(); }

    public void Resize(int width, int height) => Display.Resize();

    public void FocusChanged(bool active) { if (!active) Display.MouseButtons = default; }

    public bool CloseRequested()
    {
        if (AoWHSMap.Current is null) return IsClosed = true;
        if (AoWHSMap.Current.CombatMap is null)
        {
            AoWHSMap.Current.ExitGame(ExitGameMode.Terminate, (_, exited) => IsClosed |= exited);
            return IsClosed |= AoWHSMap.Current.ExitPending;
        }
        DialogHosts.ConfirmationDialog.Confirm(AbilityRules.TranslateResourceString(PlayerControl.ConfirmText), AbilityRules.TranslateResourceString(PopupWindows.QuitCombatText), 6);
        return false;
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        Display?.Dispose();
        WinDisplayController?.Dispose();
        AudioPlayback.ReleaseSoundSystem();
        if (_created)
        {
            DialogHosts.General.SaveLayout();
            var registry = GameTables.GameEngine.Registry;
            registry.SetMainWindowState(_windowState);
            registry.SetString(0, DialogHosts.MultiWindow.TcpNameEdit.Text);
            registry.SetString(1, DialogHosts.MultiWindow.IPAddressEdit.Text);
            registry.SetDisplayWidth(DialogHosts.VideoDialog.DisplayWidth);
            registry.SetDisplayHeight(DialogHosts.VideoDialog.DisplayHeight);
            registry.SetDisplayMode(_host.Fullscreen ? 1 : 0);
            registry.SetUseDirectSound(DialogHosts.SoundDialog.Settings.UseDirectSound);
            registry.SetSfxEnabled(DialogHosts.SoundDialog.Settings.SfxEnabled);
            registry.SetMusicEnabled(DialogHosts.SoundDialog.Settings.MusicEnabled);
            registry.SetSoundQuality(DialogHosts.SoundDialog.SoundQuality);
            registry.SetSfxVolume(DialogHosts.SoundDialog.EffectsVolumeSlider.Position);
            registry.SetMusicVolume(DialogHosts.SoundDialog.MusicVolumeSlider.Position);
        }
        if (GameHost.MainForm == this) GameHost.MainForm = null;
    }
}
