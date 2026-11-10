#nullable disable
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Host;
using Aow.Game.Layout;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Ui.Dialogs;
using Aow.Rtl;

namespace Aow.Platform;

/// <summary>The host window and frame procedure from AoWUnit.pas.</summary>
public sealed class TMainForm : IGameHost, IDisplayEvents, IDisposable
{
    public global::Aow.Game.Draw.Display Display { get; }
    public SdlDC WinDC { get; }
    public SdlDC DXDC => WinDC;
    public short FCursor { get; set; }
    public Aow.Game.Draw.Images.LibraryImage FCursorImage { get; set; }
    public Rect FLastCursorRect { get; private set; }
    public bool FCursorChanged { get; private set; }
    public bool IsClosed { get; private set; }
    public LayoutSession Layouts { get; }

    private readonly IDisplayHost host;
    private bool disposed;
    private bool created;
    private WindowState windowState;

    private TMainForm(IDisplayHost host, LayoutSession layouts)
    {
        this.host = host;
        Layouts = layouts;
        GameHost.MainForm = this;
        Display = new global::Aow.Game.Draw.Display(null);
        Display.Host = host;
        Display.EventSink = this;
        Display.Terminated = () => IsClosed;
        WinDC = new SdlDC(host);
    }

    public static TMainForm Create(IDisplayHost host, string argument = "", bool mute = true, bool audioOutput = false)
    {
        Aow.Game.Ui.Dialogs.VideoDialog.DesktopSize = () => host.DesktopSize;
        Aow.Game.Ui.Dialogs.VideoDialog.FullscreenSizes = () => host.FullscreenSizes;
        Aow.Game.Engine.Startup.InitializeAll();
        TSdlSoundSystem.Install(mute, audioOutput);
        var layouts = new LayoutSession();
        GameLayouts.Initialize(layouts);
        if (layouts.Modules.Count != 77) throw new InvalidDataException("The startup requires 77 data modules.");
        var form = new TMainForm(host, layouts);
        try { form.Initialize(argument); return form; }
        catch { form.Dispose(); throw; }
    }

    private void Initialize(string argument)
    {
        Display.OnUpdateFrame = DisplayUpdateFrame;
        Display.OnResizeBackSurface = (_, w, h) => DisplayResizeBackSurface(w, h);
        Display.OnSurfaceDataLost = (_, _) => Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.FAligned = false;
        Display.OnSwapDC = _ => { Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.FModeChanged = true; Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.ApplyMode(); };
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
        Aow.Game.Combat.CombatClasses.Register(Aow.Game.Magic.Abilities.GameTables.AoWEngine);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.InitializeHSSet(Aow.Game.Magic.Abilities.GameTables.AoWEngine.StartupDirectory + "RELEASE\\RELEASE.HSS", null);
        windowState = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetMainWindowState();
        Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.MusicVolume.Position = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetMusicVolume();
        Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.SFXVolume.Position = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetSFXVolume();
        Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.FSettings.MusicEnabled = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetMusicEnabled();
        Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.FSettings.SFXEnabled = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetSFXEnabled();
        Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.FSettings.UseDirectSound = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetUseDirectSound();
        Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.FSoundQuality = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetSoundQuality();
        Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.FSettings.SampleVoices = 16;
        Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.FSettings.SampleRate = Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.FSoundQuality == 1 ? 22050 : 44100;
        Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.FSettings.OutputMode = Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.FSoundQuality switch
        {
            1 => Set<Aow.Game.Audio.SoundOutputModeElement>.Of(Aow.Game.Audio.SoundOutputModeElement.Mono, Aow.Game.Audio.SoundOutputModeElement.om8bit),
            2 => Set<Aow.Game.Audio.SoundOutputModeElement>.Of(Aow.Game.Audio.SoundOutputModeElement.Mono, Aow.Game.Audio.SoundOutputModeElement.om16bit),
            3 => Set<Aow.Game.Audio.SoundOutputModeElement>.Of(Aow.Game.Audio.SoundOutputModeElement.Stereo, Aow.Game.Audio.SoundOutputModeElement.om16bit),
            _ => Set<Aow.Game.Audio.SoundOutputModeElement>.Of(Aow.Game.Audio.SoundOutputModeElement.Stereo, Aow.Game.Audio.SoundOutputModeElement.om16bit, Aow.Game.Audio.SoundOutputModeElement.HighQuality)
        };
        if (Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.FSoundQuality != 0)
        {
            Aow.Game.Audio.AudioPlayback.InitializeSoundSystem("SDL", Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.FSettings);
            Aow.Game.Audio.AudioPlayback.SetMusicVolume(Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.MusicVolume.Position);
            Aow.Game.Audio.AudioPlayback.SetSFXVolume(Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.SFXVolume.Position);
        }
        Aow.Game.Ui.Dialogs.VideoDialog.SetVideoSize(Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog, Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetDisplayWidth(), Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetDisplayHeight());
        Aow.Game.Ui.Dialogs.VideoDialog.SetVideoMode(Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog, host.Fullscreen ? Aow.Game.Ui.Dialogs.VideoDialog.ScreenMode.Game : Aow.Game.Ui.Dialogs.VideoDialog.ScreenMode.Windowed);
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.WinWidth = Display.BackSurfaceWidth;
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.WinHeight = Display.BackSurfaceHeight;
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.FAligned = false;
        var name = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetString(0);
        if (!RtlUnit.IsEmpty(name)) { Aow.Game.Ui.Dialogs.DialogHosts.MultiWindow.TCPName.Text = name; Aow.Game.Ui.Dialogs.DialogHosts.MultiWindow.IPXName.Text = name; }
        var address = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetString(1);
        if (!RtlUnit.IsEmpty(address)) Aow.Game.Ui.Dialogs.DialogHosts.MultiWindow.IPEdit.Text = address;
        Aow.Game.Ui.Dialogs.WindowActions.RestoreWindowLayout(Aow.Game.Ui.Dialogs.DialogHosts.General);
        RtlUnit.SetCurrentDir(Aow.Game.Magic.Abilities.GameTables.AoWEngine.StartupDirectory);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCombatFont = Aow.Game.Ui.Dialogs.DialogHosts.FontModule.Age8;
    }

    // A command-line save names a world or campaign save by its original or own extension (own-formats.md 2.9).
    public static bool IsSaveName(string argument) =>
        Aow.Formats.DocumentKind.Save.HasExtension(Path.GetExtension(argument)) || Aow.Formats.DocumentKind.CampaignSave.HasExtension(Path.GetExtension(argument));

    private void FormShow(string argument)
    {
        Aow.Game.Audio.AudioHost.SongPlayer.OnSongChange = _ => Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.SongChange();
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.OutputDebugStr("ParamStr: " + argument);
        if (argument.Length != 0 && RtlUnit.FileExists(argument) && IsSaveName(argument))
        {
            Aow.Game.Ui.Dialogs.WindowActions.ShowTitleScene(Aow.Game.Ui.Dialogs.DialogHosts.TitleScene, false);
            GameHost.DeferredSetupAction = 2;
            Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FLastFileName = argument;
            return;
        }
        if (!Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetDebugMode() && !Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetSkipIntro()) Aow.Game.Ui.Dialogs.DialogHosts.IntroScene.Execute();
        else Aow.Game.Ui.Dialogs.WindowActions.ShowTitleScene(Aow.Game.Ui.Dialogs.DialogHosts.TitleScene, true);
    }

    public void DisplayResizeBackSurface(int width, int height)
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.WinWidth = Math.Max(640, Display.BackSurfaceWidth);
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.WinHeight = Math.Max(480, Display.BackSurfaceHeight);
        Aow.Game.Ui.Dialogs.DialogHosts.General.ResetLayout();
    }

    public void Run(int frames = 0)
    {
        if (frames > 0 && Display.PresentedFrames >= frames) return;
        Display.FramePresented = frame => { if (frames > 0 && frame >= frames) Display.StopUpdateFrameLoop(); };
        Display.StartUpdateFrameLoop();
    }

    public void DisplayUpdateFrame(object sender, AbstractDC dc, int surfaceType)
    {
        RtlUnit.Clock.WaitMilliseconds(Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetDebugMode() ? 1 : 0);
        switch (GameHost.DeferredSetupAction)
        {
            case 1: GameHost.DeferredSetupAction = 0; Aow.Game.Ui.Dialogs.DialogHosts.ScenSetup.FSetupControl.hPlayerSetupDone(); break;
            case 2: GameHost.DeferredSetupAction = 0; Aow.Game.Ui.Dialogs.FileDlgWindow.OpenSelectedGame(Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow); break;
        }
        if (Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.DPlay is not null)
            Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.CheckMessages();
        Aow.Game.Audio.AudioPlayback.ProcessSound();
        try { Aow.Game.World.Map.AoWHSMap.Current?.NewFrame(); }
        catch (System.Exception error) { Console.Error.WriteLine(error); RtlUnit.ShowMessage("Exception during AoWHSMap.NewFrame"); }
        if (dc?.GetSurface(1) is Surface back)
        {
            int x = -1, y = -1;
            Display.GetMousePos(ref x, ref y);
            Rect rect = default;
            GetCursorRect(x, y, ref rect);
            FCursorChanged |= (Aow.Game.World.Map.AoWHSMap.Current is not null && Aow.Game.World.Map.AoWHSMap.Current.Mouse.CursorVisible) ||
                FLastCursorRect.Left != rect.Left || FLastCursorRect.Top != rect.Top ||
                FLastCursorRect.Right != rect.Right || FLastCursorRect.Bottom != rect.Bottom;
            if (Aow.Game.World.Map.AoWHSMap.Current is not null) ScrollMap(x, y);
            try { Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.DrawSurface(back); }
            catch (System.Exception error) { Console.Error.WriteLine(error); RtlUnit.ShowMessage("Exception during WinManager DrawSurface"); }
            SurfaceDesc descriptor = default;
            if (x != -1 && back.Lock(ref descriptor) >= 0)
            {
                try { DrawCursor(x, y, descriptor); }
                finally { back.Unlock(); }
            }
            FLastCursorRect = rect;
            Aow.Game.Ui.Dialogs.DialogHosts.DialogWin.FrameLbl.Text = RtlUnit.IntToStr(Display.CurrentFrameRate);
        }
        if (Aow.Game.World.Map.AoWHSMap.Current is not null && Aow.Game.World.Map.AoWHSMap.Current.FExitPending) Aow.Game.Ui.Dialogs.DialogHosts.General.ReturnToTitle();
        Aow.Game.Ui.Dialogs.VideoDialog.ApplyVideoSettings(Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog);
    }

    private int CursorIndex => FCursor == 9999 && Aow.Game.World.Map.AoWHSMap.Current is not null ? Aow.Game.World.Map.AoWHSMap.Current.Mouse.Cursor : FCursor;

    public void GetCursorRect(int x, int y, ref Rect rect)
    {
        rect = default;
        if (x == -1) return;
        void Include(Aow.Game.Draw.Images.LibraryImage image, int dx, int dy)
        {
            if (image is null) return;
            Rect bounds = default;
            image.GetVisualDimensions(ref bounds);
            bounds.Left += x + dx; bounds.Right += x + dx;
            bounds.Top += y + dy; bounds.Bottom += y + dy;
            RtlUnit.UnionRect(ref FCursorBounds, FCursorBounds, bounds);
        }
        FCursorBounds = default;
        if (Aow.Game.World.Map.AoWHSMap.Current is not null)
        {
            Include(FCursorImage, -15, -20);
            if (CursorIndex != 1) Include(Aow.Game.Engine.AoWHSSet.Current.FCursorImages[CursorIndex], -25, -25);
        }
        else if (FCursor != 1 && Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.Cursors.ImageExist(FCursor)) Include(Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.Cursors[FCursor], -25, -25);
        rect = FCursorBounds;
    }

    private Rect FCursorBounds;

    public void DrawCursor(int x, int y, in SurfaceDesc surface)
    {
        if (x == -1) return;
        if (Aow.Game.World.Map.AoWHSMap.Current is not null && Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(Aow.Game.World.Map.HSMapStatusElement.Activated))
        {
            FCursorImage?.Show(x - 15, y - 20, surface);
            var image = CursorIndex != 1 ? Aow.Game.Engine.AoWHSSet.Current.FCursorImages[CursorIndex] : null;
            if (image is null) return;
            if (Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer != -1 && Aow.Game.World.Map.AoWHSMap.Current.FCombatMap is null)
            {
                var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer);
                if (player.GetBusy() && player.FGameOverStatus == Aow.Game.Session.GameOverStatus.None)
                {
                    image.ShowBlended(x - 25, y - 25, surface, Aow.Game.Draw.GfxState.AlphaBlendTable.GetLookupTables(Math.Abs((Aow.Game.World.Map.AoWHSMap.Current.Frame << 2) % 100 - 50) + 30));
                    FCursorChanged = true;
                    return;
                }
            }
            image.Show(x - 25, y - 25, surface);
        }
        else if (FCursor != 1 && Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.Cursors.ImageExist(FCursor)) Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.Cursors[FCursor].ShowClipped(x - 25, y - 25, surface);
    }

    public void ScrollMap(int x, int y)
    {
        if (Aow.Game.Ui.Dialogs.General.HasModalWindow(Aow.Game.Ui.Dialogs.DialogHosts.General) || x == -1 || !host.HasFocus) return;
        if (x <= 1) { Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneX -= 16; Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTLeft(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneX / 32 - 6); }
        if (x >= Display.ActiveDC.GetSurface(0).Width - 1) { Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneX += 16; Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTLeft(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneX / 32 - 6); }
        if (y <= 1) { Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneY -= 16; Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTTop(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneY / 32 - 6); }
        if (y >= Display.ActiveDC.GetSurface(0).Height - 1) { Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneY += 16; Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTTop(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneY / 32 - 6); }
    }

    public void MouseDown(MouseButton button, Set<ShiftState> shift, int x, int y) { Display.MouseButtons = shift; Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.CheckMouseDown(button, shift, x, y); }

    public void MouseUp(MouseButton button, Set<ShiftState> shift, int x, int y)
    {
        Display.MouseButtons = shift;
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.CheckMouseUp(button, shift, x, y);
        if (Aow.Game.World.Map.AoWHSMap.Current?.Mouse is Aow.Game.World.Map.AbstractAoWHSMapMouse mouse) mouse.ResetRectangleSelection();
    }

    public void MouseMove(Set<ShiftState> shift, int x, int y) { Display.MouseButtons = shift; Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.CheckMouseMove(shift, x, y); }

    public void MouseWheel(int steps, Set<ShiftState> shift, int x, int y) => Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.CheckMouseWheel(steps, shift, x, y);

    public void KeyDown(ushort key, Set<ShiftState> shift)
    {
        if (key == 13 && shift.Contains(ShiftState.Alt)) { Display.SwapDC(); return; }
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.CheckKeyDown(ref key, shift);
        if (key == 112 && Aow.Game.World.Map.AoWHSMap.Current is not null && Aow.Game.World.Map.AoWHSMap.Current.FGameType == Aow.Game.Session.GameType.Tutorial) Aow.Game.Ui.Dialogs.TutorialWindow.ShowTutorialMessage(Aow.Game.Layout.GameLayouts.TutorialWindow);
    }

    public void KeyPress(byte key) => Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.CheckKeyPress(ref key);

    public void DoubleClick() { if (Aow.Game.World.Map.AoWHSMap.Current is not null && !Aow.Game.Ui.Dialogs.General.HasModalWindow(Aow.Game.Ui.Dialogs.DialogHosts.General)) Aow.Game.World.Map.AoWHSMap.Current.Mouse.DblClick(); }

    public void Resize(int width, int height) => Display.Resize();

    public void FocusChanged(bool active) { if (!active) Display.MouseButtons = default; }

    public bool CloseRequested()
    {
        if (Aow.Game.World.Map.AoWHSMap.Current is null) return IsClosed = true;
        if (Aow.Game.World.Map.AoWHSMap.Current.FCombatMap is null)
        {
            Aow.Game.World.Map.AoWHSMap.Current.ExitGame(Aow.Game.Session.ExitGameMode.Terminate, (_, exited) => IsClosed |= exited);
            return IsClosed |= Aow.Game.World.Map.AoWHSMap.Current.FExitPending;
        }
        Aow.Game.Ui.Dialogs.DialogHosts.DialogWin.Confirm(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Session.PlayerControl.ConfirmRStr), Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.PopupWindows.QuitCombatRStr), 6);
        return false;
    }

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;
        Display?.Dispose();
        WinDC?.Dispose();
        Aow.Game.Audio.AudioPlayback.ReleaseSoundSystem();
        if (created)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.SaveLayout();
            var registry = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry;
            registry.SetMainWindowState(windowState);
            registry.SetString(0, Aow.Game.Ui.Dialogs.DialogHosts.MultiWindow.TCPName.Text);
            registry.SetString(1, Aow.Game.Ui.Dialogs.DialogHosts.MultiWindow.IPEdit.Text);
            registry.SetDisplayWidth(Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.FDisplayWidth);
            registry.SetDisplayHeight(Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.FDisplayHeight);
            registry.SetDisplayMode(host.Fullscreen ? 1 : 0);
            registry.SetUseDirectSound(Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.FSettings.UseDirectSound);
            registry.SetSFXEnabled(Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.FSettings.SFXEnabled);
            registry.SetMusicEnabled(Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.FSettings.MusicEnabled);
            registry.SetSoundQuality(Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.FSoundQuality);
            registry.SetSFXVolume(Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.SFXVolume.Position);
            registry.SetMusicVolume(Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.MusicVolume.Position);
        }
        if (GameHost.MainForm == this) GameHost.MainForm = null;
    }
}
