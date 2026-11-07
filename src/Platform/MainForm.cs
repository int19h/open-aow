#nullable disable
using Aow.Layout;
using Aow.Rtl;
using Aow.Units.DisplayC;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using static Aow.Units.E2Globals.E2GlobalsUnit;
using static Aow.Units.E2GameFlow.E2GameFlowUnit;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.aowIntGfxMod.aowIntGfxModUnit;
using static Aow.Units.AoWVideoDlg.AoWVideoDlgUnit;
using static Aow.Units.AOWMWindow.AOWMWindowUnit;
using static Aow.Units.AoWSWindow.AoWSWindowUnit;
using static Aow.Units.AoWFileDialog.AoWFileDialogUnit;
using Aow.Units.AoWTCReg;
using Aow.Units.Sound;
using Aow.Units.E2WindowLayout;
using Aow.Units.aowSetupEvents;
using Aow.Units.AoWE;

namespace Aow.Platform;

/// <summary>The host window and frame procedure from AoWUnit.pas.</summary>
public sealed class TMainForm : IGameHost, IDisplayEvents, IDisposable
{
    public TDisplay Display { get; }
    public TSdlDC WinDC { get; }
    public TSdlDC DXDC => WinDC;
    public short FCursor { get; set; }
    public Aow.Units.ImageLib.TLibraryImage FCursorImage { get; set; }
    public TRect FLastCursorRect { get; private set; }
    public bool FCursorChanged { get; private set; }
    public bool IsClosed { get; private set; }
    public LayoutSession Layouts { get; }
    private readonly IDisplayHost host;
    private bool disposed;
    private bool created;
    private TWindowState windowState;

    private TMainForm(IDisplayHost host, LayoutSession layouts)
    {
        this.host = host;
        Layouts = layouts;
        GameHost.MainForm = this;
        Display = new TDisplay(null);
        Display.Host = host;
        Display.EventSink = this;
        Display.Terminated = () => IsClosed;
        WinDC = new TSdlDC(host);
    }

    public static TMainForm Create(IDisplayHost host, string argument = "", bool mute = true, bool audioOutput = false)
    {
        GFXEUnit.DesktopSize = () => host.DesktopSize;
        GFXEUnit.FullscreenSizes = () => host.FullscreenSizes;
        Aow.Units.Startup.InitializeAll();
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
        Display.OnSurfaceDataLost = (_, _) => General.Manager.FAligned = false;
        Display.OnSwapDC = _ => { VideoDialog.FModeChanged = true; VideoDialog.ApplyMode(); };
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
        AoWTCRegUnit.RegisterTacticalCombat(AoWEngine);
        AoWEngine.InitializeHSSet(AoWEngine.StartupDirectory + "RELEASE\\RELEASE.HSS", null);
        windowState = AoWEngine.FRegistry.GetMainWindowState();
        SoundDlg.MusicVolume.Position = AoWEngine.FRegistry.GetMusicVolume();
        SoundDlg.SFXVolume.Position = AoWEngine.FRegistry.GetSFXVolume();
        SoundDlg.FSettings.MusicEnabled = AoWEngine.FRegistry.GetMusicEnabled();
        SoundDlg.FSettings.SFXEnabled = AoWEngine.FRegistry.GetSFXEnabled();
        SoundDlg.FSettings.UseDirectSound = AoWEngine.FRegistry.GetUseDirectSound();
        SoundDlg.FSoundQuality = AoWEngine.FRegistry.GetSoundQuality();
        SoundDlg.FSettings.SampleVoices = 16;
        SoundDlg.FSettings.SampleRate = SoundDlg.FSoundQuality == 1 ? 22050 : 44100;
        SoundDlg.FSettings.OutputMode = SoundDlg.FSoundQuality switch
        {
            1 => TSet<Aow.Units.Sound.TSoundOutputModeElement>.Of(Aow.Units.Sound.TSoundOutputModeElement.omMono, Aow.Units.Sound.TSoundOutputModeElement.om8bit),
            2 => TSet<Aow.Units.Sound.TSoundOutputModeElement>.Of(Aow.Units.Sound.TSoundOutputModeElement.omMono, Aow.Units.Sound.TSoundOutputModeElement.om16bit),
            3 => TSet<Aow.Units.Sound.TSoundOutputModeElement>.Of(Aow.Units.Sound.TSoundOutputModeElement.omStereo, Aow.Units.Sound.TSoundOutputModeElement.om16bit),
            _ => TSet<Aow.Units.Sound.TSoundOutputModeElement>.Of(Aow.Units.Sound.TSoundOutputModeElement.omStereo, Aow.Units.Sound.TSoundOutputModeElement.om16bit, Aow.Units.Sound.TSoundOutputModeElement.omHighQuality)
        };
        if (SoundDlg.FSoundQuality != 0)
        {
            SoundUnit.InitializeSoundSystem("SDL", SoundDlg.FSettings);
            SoundUnit.SetMusicVolume(SoundDlg.MusicVolume.Position);
            SoundUnit.SetSFXVolume(SoundDlg.SFXVolume.Position);
        }
        SetVideoSize(VideoDialog, AoWEngine.FRegistry.GetDisplayWidth(), AoWEngine.FRegistry.GetDisplayHeight());
        SetVideoMode(VideoDialog, host.Fullscreen ? Aow.Units.AoWVideoDlg.TScreenMode.scmGame : Aow.Units.AoWVideoDlg.TScreenMode.scmWindowed);
        General.Manager.WinWidth = Display.BackSurfaceWidth;
        General.Manager.WinHeight = Display.BackSurfaceHeight;
        General.Manager.FAligned = false;
        var name = AoWEngine.FRegistry.GetString(0);
        if (!RtlUnit.IsEmpty(name)) { MultiWindow.TCPName.Text = name; MultiWindow.IPXName.Text = name; }
        var address = AoWEngine.FRegistry.GetString(1);
        if (!RtlUnit.IsEmpty(address)) MultiWindow.IPEdit.Text = address;
        E2WindowLayoutUnit.RestoreWindowLayout(General);
        RtlUnit.SetCurrentDir(AoWEngine.StartupDirectory);
        AoWEngine.FCombatFont = FontModule.Age8;
    }
    private void FormShow(string argument)
    {
        SoundUnit.SongPlayer.OnSongChange = SoundDlg.SongChange;
        AoWEngine.OutputDebugStr("ParamStr: " + argument);
        if (argument.Length != 0 && RtlUnit.FileExists(argument) && Path.GetExtension(argument).ToLowerInvariant() is ".asg" or ".acg")
        {
            ShowTitleScene(TitleScene, false);
            GameHost.DeferredSetupAction = 2;
            FileDlgWindow.FLastFileName = argument;
            return;
        }
        if (!AoWEngine.FRegistry.GetDebugMode() && !AoWEngine.FRegistry.GetSkipIntro()) IntroScene.Execute();
        else ShowTitleScene(TitleScene, true);
    }
    public void DisplayResizeBackSurface(int width, int height)
    {
        General.Manager.WinWidth = Math.Max(640, Display.BackSurfaceWidth);
        General.Manager.WinHeight = Math.Max(480, Display.BackSurfaceHeight);
        General.ResetLayout();
    }
    public void Run(int frames = 0)
    {
        if (frames > 0 && Display.PresentedFrames >= frames) return;
        Display.FramePresented = frame => { if (frames > 0 && frame >= frames) Display.StopUpdateFrameLoop(); };
        Display.StartUpdateFrameLoop();
    }
    public void DisplayUpdateFrame(TObject sender, TAbstractDC dc, int surfaceType)
    {
        RtlUnit.Clock.WaitMilliseconds(AoWEngine.FRegistry.GetDebugMode() ? 1 : 0);
        switch (GameHost.DeferredSetupAction)
        {
            case 1: GameHost.DeferredSetupAction = 0; ScenSetup.FSetupControl.hPlayerSetupDone(); break;
            case 2: GameHost.DeferredSetupAction = 0; OpenSelectedGame(FileDlgWindow); break;
        }
        if (aowSetupEventsUnit.SetupEvents.DPlay.DPlay is not null)
            aowSetupEventsUnit.SetupEvents.DPlay.CheckMessages();
        SoundUnit.ProcessSound();
        try { AoWHSMap?.NewFrame(); }
        catch (System.Exception error) { Console.Error.WriteLine(error); RtlUnit.ShowMessage("Exception during AoWHSMap.NewFrame"); }
        if (dc?.GetSurface(1) is TSurface back)
        {
            int x = -1, y = -1;
            Display.GetMousePos(ref x, ref y);
            TRect rect = default;
            GetCursorRect(x, y, ref rect);
            FCursorChanged |= (AoWHSMap is not null && AoWHSMap.Mouse.CursorVisible) ||
                FLastCursorRect.Left != rect.Left || FLastCursorRect.Top != rect.Top ||
                FLastCursorRect.Right != rect.Right || FLastCursorRect.Bottom != rect.Bottom;
            if (AoWHSMap is not null) ScrollMap(x, y);
            try { General.Manager.DrawSurface(back); }
            catch (System.Exception error) { Console.Error.WriteLine(error); RtlUnit.ShowMessage("Exception during WinManager DrawSurface"); }
            TSurfaceDesc descriptor = default;
            if (x != -1 && back.Lock(ref descriptor) >= 0)
            {
                try { DrawCursor(x, y, descriptor); }
                finally { back.Unlock(); }
            }
            FLastCursorRect = rect;
            DialogWin.FrameLbl.Text = RtlUnit.IntToStr(Display.CurrentFrameRate);
        }
        if (AoWHSMap is not null && AoWHSMap.FExitPending) General.ReturnToTitle();
        ApplyVideoSettings(VideoDialog);
    }
    private int CursorIndex => FCursor == 9999 && AoWHSMap is not null ? AoWHSMap.Mouse.Cursor : FCursor;
    public void GetCursorRect(int x, int y, ref TRect rect)
    {
        rect = default;
        if (x == -1) return;
        void Include(Aow.Units.ImageLib.TLibraryImage image, int dx, int dy)
        {
            if (image is null) return;
            TRect bounds = default;
            image.GetVisualDimensions(ref bounds);
            bounds.Left += x + dx; bounds.Right += x + dx;
            bounds.Top += y + dy; bounds.Bottom += y + dy;
            RtlUnit.UnionRect(ref FCursorBounds, FCursorBounds, bounds);
        }
        FCursorBounds = default;
        if (AoWHSMap is not null)
        {
            Include(FCursorImage, -15, -20);
            if (CursorIndex != 1) Include(AoWHSSet.FCursorImages[CursorIndex], -25, -25);
        }
        else if (FCursor != 1 && IntGfxMod.Cursors.ImageExist(FCursor)) Include(IntGfxMod.Cursors[FCursor], -25, -25);
        rect = FCursorBounds;
    }
    private TRect FCursorBounds;
    public void DrawCursor(int x, int y, in TSurfaceDesc surface)
    {
        if (x == -1) return;
        if (AoWHSMap is not null && AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msActivated))
        {
            FCursorImage?.Show(x - 15, y - 20, surface);
            var image = CursorIndex != 1 ? AoWHSSet.FCursorImages[CursorIndex] : null;
            if (image is null) return;
            if (AoWHSMap.FSeatedPlayer != -1 && AoWHSMap.FCombatMap is null)
            {
                var player = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                if (player.GetBusy() && player.FGameOverStatus == Aow.Units.AoWE.TGameOverStatus.goNone)
                {
                    image.ShowBlended(x - 25, y - 25, surface, GFXEUnit.AlphaBlendTable.GetLookupTables(Math.Abs((AoWHSMap.Frame << 2) % 100 - 50) + 30));
                    FCursorChanged = true;
                    return;
                }
            }
            image.Show(x - 25, y - 25, surface);
        }
        else if (FCursor != 1 && IntGfxMod.Cursors.ImageExist(FCursor)) IntGfxMod.Cursors[FCursor].ShowClipped(x - 25, y - 25, surface);
    }
    public void ScrollMap(int x, int y)
    {
        if (HasModalWindow(General) || x == -1 || !host.HasFocus) return;
        if (x <= 1) { MWindow.MapViewer.SceneX -= 16; SWindow.Scanner.SetTLeft(MWindow.MapViewer.SceneX / 32 - 6); }
        if (x >= Display.ActiveDC.GetSurface(0).Width - 1) { MWindow.MapViewer.SceneX += 16; SWindow.Scanner.SetTLeft(MWindow.MapViewer.SceneX / 32 - 6); }
        if (y <= 1) { MWindow.MapViewer.SceneY -= 16; SWindow.Scanner.SetTTop(MWindow.MapViewer.SceneY / 32 - 6); }
        if (y >= Display.ActiveDC.GetSurface(0).Height - 1) { MWindow.MapViewer.SceneY += 16; SWindow.Scanner.SetTTop(MWindow.MapViewer.SceneY / 32 - 6); }
    }
    public void MouseDown(TMouseButton button, TSet<TShiftState> shift, int x, int y) { Display.MouseButtons = shift; General.Manager.CheckMouseDown(button, shift, x, y); }
    public void MouseUp(TMouseButton button, TSet<TShiftState> shift, int x, int y)
    {
        Display.MouseButtons = shift;
        General.Manager.CheckMouseUp(button, shift, x, y);
        if (AoWHSMap?.Mouse is Aow.Units.AoWE.TAbstractAoWHSMapMouse mouse) mouse.ResetRectangleSelection();
    }
    public void MouseMove(TSet<TShiftState> shift, int x, int y) { Display.MouseButtons = shift; General.Manager.CheckMouseMove(shift, x, y); }
    public void KeyDown(ushort key, TSet<TShiftState> shift)
    {
        if (key == 13 && shift.Contains(TShiftState.ssAlt)) { Display.SwapDC(); return; }
        General.Manager.CheckKeyDown(ref key, shift);
        if (key == 112 && AoWHSMap is not null && AoWHSMap.FGameType == Aow.Units.AoWE.TGameType.gtTutorial) ShowTutorialMessage(TutorialWindow);
    }
    public void KeyPress(byte key) => General.Manager.CheckKeyPress(ref key);
    public void DoubleClick() { if (AoWHSMap is not null && !HasModalWindow(General)) AoWHSMap.Mouse.DblClick(); }
    public void Resize(int width, int height) => Display.Resize();
    public void FocusChanged(bool active) { if (!active) Display.MouseButtons = default; }
    public bool CloseRequested()
    {
        if (AoWHSMap is null) return IsClosed = true;
        if (AoWHSMap.FCombatMap is null)
        {
            AoWHSMap.ExitGame(Aow.Units.AoWE.TExitGameMode.egmTerminate, (_, exited) => IsClosed |= exited);
            return IsClosed |= AoWHSMap.FExitPending;
        }
        DialogWin.Confirm(TranslateRStr(AoWEUnit.ConfirmRStr), TranslateRStr(QuitCombatRStr), 6);
        return false;
    }
    public void Dispose()
    {
        if (disposed) return;
        disposed = true;
        Display.Free();
        WinDC.Free();
        SoundUnit.ReleaseSoundSystem();
        if (created)
        {
            General.SaveLayout();
            var registry = AoWEngine.FRegistry;
            registry.SetMainWindowState(windowState);
            registry.SetString(0, MultiWindow.TCPName.Text);
            registry.SetString(1, MultiWindow.IPEdit.Text);
            registry.SetDisplayWidth(VideoDialog.FDisplayWidth);
            registry.SetDisplayHeight(VideoDialog.FDisplayHeight);
            registry.SetDisplayMode(host.Fullscreen ? 1 : 0);
            registry.SetUseDirectSound(SoundDlg.FSettings.UseDirectSound);
            registry.SetSFXEnabled(SoundDlg.FSettings.SFXEnabled);
            registry.SetMusicEnabled(SoundDlg.FSettings.MusicEnabled);
            registry.SetSoundQuality(SoundDlg.FSoundQuality);
            registry.SetSFXVolume(SoundDlg.SFXVolume.Position);
            registry.SetMusicVolume(SoundDlg.MusicVolume.Position);
        }
        if (GameHost.MainForm == this) GameHost.MainForm = null;
    }
}
