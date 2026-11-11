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
    public SdlDisplayController WinDC { get; }
    public SdlDisplayController DXDC => WinDC;
    public short FCursor { get; set; }
    public LibraryImage FCursorImage { get; set; }
    public Rect FLastCursorRect { get; private set; }
    public bool FCursorChanged { get; private set; }
    public bool IsClosed { get; private set; }
    public LayoutSession Layouts { get; }

    private readonly IDisplayHost host;
    private bool disposed;
    private bool created;
    private WindowState windowState;

    private MainForm(IDisplayHost host, LayoutSession layouts)
    {
        this.host = host;
        Layouts = layouts;
        GameHost.MainForm = this;
        Display = new Display(null);
        Display.Host = host;
        Display.EventSink = this;
        Display.Terminated = () => IsClosed;
        WinDC = new SdlDisplayController(host);
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
        Display.OnSurfaceDataLost = (_, _) => DialogHosts.General.Manager.FAligned = false;
        Display.OnSwapDC = _ => { DialogHosts.VideoDialog.FModeChanged = true; DialogHosts.VideoDialog.ApplyMode(); };
        Display.PrimaryDC = WinDC;
        Display.SecondaryDC = WinDC;
        FormCreate();
        created = true;
        Display.ActivateDC(WinDC);
        Display.PresentInitialFrame();
        // The game reads its command line as a path on its own drive, so a host path becomes the file system's public path.
        FormShow(argument.Length == 0 ? argument : RtlUnit.Files.ExpandPath(argument));
    }

    private void FormCreate()
    {
        CombatClasses.Register(GameTables.AoWEngine);
        GameTables.AoWEngine.InitializeHSSet(GameTables.AoWEngine.StartupDirectory + "RELEASE\\RELEASE.HSS", null);
        windowState = GameTables.AoWEngine.FRegistry.GetMainWindowState();
        DialogHosts.SoundDlg.MusicVolume.Position = GameTables.AoWEngine.FRegistry.GetMusicVolume();
        DialogHosts.SoundDlg.SFXVolume.Position = GameTables.AoWEngine.FRegistry.GetSFXVolume();
        DialogHosts.SoundDlg.FSettings.MusicEnabled = GameTables.AoWEngine.FRegistry.GetMusicEnabled();
        DialogHosts.SoundDlg.FSettings.SFXEnabled = GameTables.AoWEngine.FRegistry.GetSFXEnabled();
        DialogHosts.SoundDlg.FSettings.UseDirectSound = GameTables.AoWEngine.FRegistry.GetUseDirectSound();
        DialogHosts.SoundDlg.FSoundQuality = GameTables.AoWEngine.FRegistry.GetSoundQuality();
        DialogHosts.SoundDlg.FSettings.SampleVoices = 16;
        DialogHosts.SoundDlg.FSettings.SampleRate = DialogHosts.SoundDlg.FSoundQuality == 1 ? 22050 : 44100;
        DialogHosts.SoundDlg.FSettings.OutputMode = DialogHosts.SoundDlg.FSoundQuality switch
        {
            1 => Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Mono, SoundOutputModeElement.Bits8),
            2 => Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Mono, SoundOutputModeElement.Bits16),
            3 => Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.Bits16),
            _ => Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.Bits16, SoundOutputModeElement.HighQuality)
        };
        if (DialogHosts.SoundDlg.FSoundQuality != 0)
        {
            AudioPlayback.InitializeSoundSystem("SDL", DialogHosts.SoundDlg.FSettings);
            AudioPlayback.SetMusicVolume(DialogHosts.SoundDlg.MusicVolume.Position);
            AudioPlayback.SetSFXVolume(DialogHosts.SoundDlg.SFXVolume.Position);
        }
        VideoDialog.SetVideoSize(DialogHosts.VideoDialog, GameTables.AoWEngine.FRegistry.GetDisplayWidth(), GameTables.AoWEngine.FRegistry.GetDisplayHeight());
        VideoDialog.SetVideoMode(DialogHosts.VideoDialog, host.Fullscreen ? VideoDialog.ScreenMode.Game : VideoDialog.ScreenMode.Windowed);
        DialogHosts.General.Manager.WinWidth = Display.BackSurfaceWidth;
        DialogHosts.General.Manager.WinHeight = Display.BackSurfaceHeight;
        DialogHosts.General.Manager.FAligned = false;
        var name = GameTables.AoWEngine.FRegistry.GetString(0);
        if (!RtlUnit.IsEmpty(name)) { DialogHosts.MultiWindow.TCPName.Text = name; DialogHosts.MultiWindow.IPXName.Text = name; }
        var address = GameTables.AoWEngine.FRegistry.GetString(1);
        if (!RtlUnit.IsEmpty(address)) DialogHosts.MultiWindow.IPEdit.Text = address;
        WindowActions.RestoreWindowLayout(DialogHosts.General);
        RtlUnit.SetCurrentDir(GameTables.AoWEngine.StartupDirectory);
        GameTables.AoWEngine.FCombatFont = DialogHosts.FontModule.Age8;
    }

    // A command-line save names a world or campaign save by its original or own extension (own-formats.md 2.9).
    public static bool IsSaveName(string argument) =>
        DocumentKind.Save.HasExtension(Path.GetExtension(argument)) || DocumentKind.CampaignSave.HasExtension(Path.GetExtension(argument));

    private void FormShow(string argument)
    {
        AudioHost.SongPlayer.OnSongChange = _ => DialogHosts.SoundDlg.SongChange();
        GameTables.AoWEngine.OutputDebugStr("ParamStr: " + argument);
        if (argument.Length != 0 && RtlUnit.FileExists(argument) && IsSaveName(argument))
        {
            WindowActions.ShowTitleScene(DialogHosts.TitleScene, false);
            GameHost.DeferredSetupAction = 2;
            DialogHosts.FileDlgWindow.FLastFileName = argument;
            return;
        }
        if (!GameTables.AoWEngine.FRegistry.GetDebugMode() && !GameTables.AoWEngine.FRegistry.GetSkipIntro()) DialogHosts.IntroScene.Execute();
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

    public void DisplayUpdateFrame(object sender, CustomDisplayController dc, int surfaceType)
    {
        RtlUnit.Clock.WaitMilliseconds(GameTables.AoWEngine.FRegistry.GetDebugMode() ? 1 : 0);
        switch (GameHost.DeferredSetupAction)
        {
            case 1: GameHost.DeferredSetupAction = 0; DialogHosts.ScenSetup.FSetupControl.hPlayerSetupDone(); break;
            case 2: GameHost.DeferredSetupAction = 0; FileDialogWindow.OpenSelectedGame(DialogHosts.FileDlgWindow); break;
        }
        if (DialogHosts.SetupEvents.DPlay.DPlay is not null)
            DialogHosts.SetupEvents.DPlay.CheckMessages();
        AudioPlayback.ProcessSound();
        try { AoWHSMap.Current?.NewFrame(); }
        catch (Exception error) { Console.Error.WriteLine(error); RtlUnit.ShowMessage("Exception during AoWHSMap.NewFrame"); }
        if (dc?.GetSurface(1) is Surface back)
        {
            int x = -1, y = -1;
            Display.GetMousePos(ref x, ref y);
            Rect rect = default;
            GetCursorRect(x, y, ref rect);
            FCursorChanged |= (AoWHSMap.Current is not null && AoWHSMap.Current.Mouse.CursorVisible) ||
                FLastCursorRect.Left != rect.Left || FLastCursorRect.Top != rect.Top ||
                FLastCursorRect.Right != rect.Right || FLastCursorRect.Bottom != rect.Bottom;
            if (AoWHSMap.Current is not null) ScrollMap(x, y);
            try { DialogHosts.General.Manager.DrawSurface(back); }
            catch (Exception error) { Console.Error.WriteLine(error); RtlUnit.ShowMessage("Exception during WinManager DrawSurface"); }
            LockedSurface descriptor = default;
            if (x != -1 && back.Lock(ref descriptor) >= 0)
            {
                try { DrawCursor(x, y, descriptor); }
                finally { back.Unlock(); }
            }
            FLastCursorRect = rect;
            DialogHosts.DialogWin.FrameLbl.Text = RtlUnit.IntToStr(Display.CurrentFrameRate);
        }
        if (AoWHSMap.Current is not null && AoWHSMap.Current.FExitPending) DialogHosts.General.ReturnToTitle();
        VideoDialog.ApplyVideoSettings(DialogHosts.VideoDialog);
    }

    private int CursorIndex => FCursor == 9999 && AoWHSMap.Current is not null ? AoWHSMap.Current.Mouse.Cursor : FCursor;

    public void GetCursorRect(int x, int y, ref Rect rect)
    {
        rect = default;
        if (x == -1) return;
        void Include(LibraryImage image, int dx, int dy)
        {
            if (image is null) return;
            Rect bounds = default;
            image.GetVisualDimensions(ref bounds);
            bounds.Left += x + dx; bounds.Right += x + dx;
            bounds.Top += y + dy; bounds.Bottom += y + dy;
            RtlUnit.UnionRect(ref FCursorBounds, FCursorBounds, bounds);
        }
        FCursorBounds = default;
        if (AoWHSMap.Current is not null)
        {
            Include(FCursorImage, -15, -20);
            if (CursorIndex != 1) Include(AoWHSSet.Current.FCursorImages[CursorIndex], -25, -25);
        }
        else if (FCursor != 1 && DialogHosts.IntGfxMod.Cursors.ImageExist(FCursor)) Include(DialogHosts.IntGfxMod.Cursors[FCursor], -25, -25);
        rect = FCursorBounds;
    }

    private Rect FCursorBounds;

    public void DrawCursor(int x, int y, in LockedSurface surface)
    {
        if (x == -1) return;
        if (AoWHSMap.Current is not null && AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Activated))
        {
            FCursorImage?.Show(x - 15, y - 20, surface);
            var image = CursorIndex != 1 ? AoWHSSet.Current.FCursorImages[CursorIndex] : null;
            if (image is null) return;
            if (AoWHSMap.Current.FSeatedPlayer != -1 && AoWHSMap.Current.FCombatMap is null)
            {
                var player = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer);
                if (player.GetBusy() && player.FGameOverStatus == GameOverStatus.None)
                {
                    image.ShowBlended(x - 25, y - 25, surface, GraphicsState.AlphaBlendTable.GetLookupTables(Math.Abs((AoWHSMap.Current.Frame << 2) % 100 - 50) + 30));
                    FCursorChanged = true;
                    return;
                }
            }
            image.Show(x - 25, y - 25, surface);
        }
        else if (FCursor != 1 && DialogHosts.IntGfxMod.Cursors.ImageExist(FCursor)) DialogHosts.IntGfxMod.Cursors[FCursor].ShowClipped(x - 25, y - 25, surface);
    }

    public void ScrollMap(int x, int y)
    {
        if (General.HasModalWindow(DialogHosts.General) || x == -1 || !host.HasFocus) return;
        if (x <= 1) { DialogHosts.MWindow.MapViewer.SceneX -= 16; DialogHosts.SWindow.Scanner.SetTLeft(DialogHosts.MWindow.MapViewer.SceneX / 32 - 6); }
        if (x >= Display.ActiveDC.GetSurface(0).Width - 1) { DialogHosts.MWindow.MapViewer.SceneX += 16; DialogHosts.SWindow.Scanner.SetTLeft(DialogHosts.MWindow.MapViewer.SceneX / 32 - 6); }
        if (y <= 1) { DialogHosts.MWindow.MapViewer.SceneY -= 16; DialogHosts.SWindow.Scanner.SetTTop(DialogHosts.MWindow.MapViewer.SceneY / 32 - 6); }
        if (y >= Display.ActiveDC.GetSurface(0).Height - 1) { DialogHosts.MWindow.MapViewer.SceneY += 16; DialogHosts.SWindow.Scanner.SetTTop(DialogHosts.MWindow.MapViewer.SceneY / 32 - 6); }
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
        if (key == 13 && shift.Contains(ShiftState.Alt)) { Display.SwapDC(); return; }
        DialogHosts.General.Manager.CheckKeyDown(ref key, shift);
        if (key == 112 && AoWHSMap.Current is not null && AoWHSMap.Current.FGameType == GameType.Tutorial) TutorialWindow.ShowTutorialMessage(GameLayouts.TutorialWindow);
    }

    public void KeyPress(byte key) => DialogHosts.General.Manager.CheckKeyPress(ref key);

    public void DoubleClick() { if (AoWHSMap.Current is not null && !General.HasModalWindow(DialogHosts.General)) AoWHSMap.Current.Mouse.DblClick(); }

    public void Resize(int width, int height) => Display.Resize();

    public void FocusChanged(bool active) { if (!active) Display.MouseButtons = default; }

    public bool CloseRequested()
    {
        if (AoWHSMap.Current is null) return IsClosed = true;
        if (AoWHSMap.Current.FCombatMap is null)
        {
            AoWHSMap.Current.ExitGame(ExitGameMode.Terminate, (_, exited) => IsClosed |= exited);
            return IsClosed |= AoWHSMap.Current.FExitPending;
        }
        DialogHosts.DialogWin.Confirm(AbilityRules.TranslateRStr(PlayerControl.ConfirmRStr), AbilityRules.TranslateRStr(PopupWindows.QuitCombatRStr), 6);
        return false;
    }

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;
        Display?.Dispose();
        WinDC?.Dispose();
        AudioPlayback.ReleaseSoundSystem();
        if (created)
        {
            DialogHosts.General.SaveLayout();
            var registry = GameTables.AoWEngine.FRegistry;
            registry.SetMainWindowState(windowState);
            registry.SetString(0, DialogHosts.MultiWindow.TCPName.Text);
            registry.SetString(1, DialogHosts.MultiWindow.IPEdit.Text);
            registry.SetDisplayWidth(DialogHosts.VideoDialog.FDisplayWidth);
            registry.SetDisplayHeight(DialogHosts.VideoDialog.FDisplayHeight);
            registry.SetDisplayMode(host.Fullscreen ? 1 : 0);
            registry.SetUseDirectSound(DialogHosts.SoundDlg.FSettings.UseDirectSound);
            registry.SetSFXEnabled(DialogHosts.SoundDlg.FSettings.SFXEnabled);
            registry.SetMusicEnabled(DialogHosts.SoundDlg.FSettings.MusicEnabled);
            registry.SetSoundQuality(DialogHosts.SoundDlg.FSoundQuality);
            registry.SetSFXVolume(DialogHosts.SoundDlg.SFXVolume.Position);
            registry.SetMusicVolume(DialogHosts.SoundDlg.MusicVolume.Position);
        }
        if (GameHost.MainForm == this) GameHost.MainForm = null;
    }
}
