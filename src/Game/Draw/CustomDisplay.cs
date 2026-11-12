// Translated from DisplayC.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// the display control. FFSWinControl, the events (OnBltSurface, OnUpdateFrame,
// OnSurfaceDataLost, OnResize, OnError, OnActivateDisplayController, OnDeactivateDisplayController, OnHide, OnShow,
// OnResizeBackSurface, OnSwapDisplayController), _frameRate, FrameTime (ms per frame), _currentFrameRate,
// UpdateFrameThread, Status, _options (2 bytes), _updateFrameMode, _backSurfaceControlMode,
// FBufferingScheme, FBackSurfaceWidth, FBackSurfaceHeight, FSurfaceIndex (the back surface to
// show), _pauseCount, _mouseAboveSurface, _primaryDisplayController, _secondaryDisplayController, _activeDisplayController
public partial class CustomDisplay : WinControl
{

    private DirectDrawFullScreenWindowControl _fullScreenWindowControl;
    private NotifyEvent<object> _onBlitSurface;
    private UpdateFrameEvent _onUpdateFrame;
    private SurfaceDataLostEvent _onSurfaceDataLost;
    private NotifyEvent<object> _onResize;
    private ErrorEvent _onError;
    private DCNotifyEvent _onActivateDisplayController;
    private DCNotifyEvent _onDeactivateDisplayController;
    private NotifyEvent<object> _onHide;
    private NotifyEvent<object> _onShow;
    private ResizeBackSurfaceEvent _onResizeBackSurface;
    private SwapDCEvent _onSwapDisplayController;
    private int _frameRate;

    internal int FrameTime { get; private set; }

    private int _currentFrameRate;

    public int CurrentFrameRate { get => _currentFrameRate; internal set => _currentFrameRate = value; }

    internal UpdateFrameThread UpdateFrameThread { get; }
    public Set<DisplayStatusElement> Status { get; private set; }

    private Set<OptionsElement> _options;
    private UpdateFrameMode _updateFrameMode;
    private BackSurfaceControlMode _backSurfaceControlMode;
    private BufferingScheme _bufferingScheme;
    private int _backSurfaceWidth;
    private int _backSurfaceHeight;

    public int SurfaceIndex { get; private set; }

    // direct frame blit
    private int _pauseCount;
    private bool _mouseAboveSurface;
    private CustomDisplayController _primaryDisplayController;
    private CustomDisplayController _secondaryDisplayController;
    private CustomDisplayController _activeDisplayController;

    // DisplayC.pas:1397
    internal void SetBufferingScheme(BufferingScheme value)
    {
        if (_activeDisplayController == null)
        {
            _bufferingScheme = value;
        }
        else
        {
            ShowError("Cannot change buffering scheme when DC is activated");
        }

        switch (_bufferingScheme)
        {
            case BufferingScheme.Single:
                SurfaceIndex = 0;
                break;
            case BufferingScheme.Double:
                SurfaceIndex = 1;
                break;
            case BufferingScheme.Triple:
                SurfaceIndex = 2;
                break;
        }
    }

    // DisplayC.pas:1410
    internal void SetUpdateFrameMode(UpdateFrameMode value)
    {
        if (!Status.Contains(DisplayStatusElement.FrameLoopActive))
        {
            _updateFrameMode = value;
            if (!ComponentState.Contains(Rtl.ComponentState.Designing) && _options.Contains(OptionsElement.AutoStartUpdateFrameLoop) && !ComponentState.Contains(Rtl.ComponentState.Loading))
            {
                StartUpdateFrameLoop();
            }
        }
        else
        {
            ShowError("Cannot change update frame loop mode when UpdateFrame loop is activated");
        }
    }

    // DisplayC.pas:1509
    internal void SetBackSurfaceWidth(int value)
    {
        if (value >= 0)
        {
            ResizeBackSurfaces(value, _backSurfaceHeight);
        }
    }

    // DisplayC.pas:1515
    internal void SetBackSurfaceHeight(int value)
    {
        if (value >= 0)
        {
            ResizeBackSurfaces(_backSurfaceWidth, value);
        }
    }

    // DisplayC.pas:1552
    internal void CMWantSpecialKey(ref WMKey message)
    {
        if (message.CharCode == 37 || message.CharCode == 39 || message.CharCode == 38 || message.CharCode == 40)
        {
            message.Result = 1;
        }
    }

    // DisplayC.pas:1560
    internal void CMKeyUP(ref WMKey message)
    {
        if (message.CharCode == 37 || message.CharCode == 39 || message.CharCode == 38 || message.CharCode == 40)
        {
            message.Result = 0;
        }
    }

    // DisplayC.pas:1390
    protected internal override void KeyDown(ref ushort key, Set<ShiftState> shift)
    {
        /* TODO: Implement Controls.KeyDown in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1327
    // (inc\TDisplay.PAS in the original) How the display works, as far as the code of this unit
    // tells: The display control has no surfaces of its own. It shows the surfaces of its
    // active display controller (_activeDisplayController), which is one of PrimaryDisplayController and SecondaryDisplayController. SwapDisplayController
    // (also Alt+Enter in KeyDown) switches between the two. A controller owns surface 0, the
    // output (the window through GDI, or the DirectDraw primary surface), and with double or
    // triple buffering (BufferingScheme) back surfaces 1 and 2 that the game draws on.
    // FSurfaceIndex is the back surface to show next. with triple buffering UpdateFrame
    // alternates it between 1 and 2. Life cycle: - Create sets the size to 100 x 100, the color
    // to black, the default options [doAutoSwapDC, doAutoStartUpdateFrameLoop,
    // doAutoActivatePrimaryDC, doBltOnWMPAINT], double buffering and back surfaces of 640 x
    // 480, and (outside the designer) makes the owner the parent and creates the
    // TDDrawFSWinControl that the full screen controllers draw in. - Loaded creates the window
    // handle. CreateWnd lets the active controller know that the window was recreated
    // (WindowRecreated) and, with doAutoActivatePrimaryDC, posts WM_DISPLAYC with WParam 2 to
    // activate the primary controller once the window exists. - ActivateDC deactivates the
    // active controller and activates the new one. With doRestoreSurfacesOnSwap the contents of
    // the old surfaces are captured in TDataSurfaces first and remapped into the surfaces of
    // the new controller. When the new controller cannot be activated, the old one is activated
    // again and the result is DCERR_ACTIVATEFAILED. The frame loop is paused meanwhile. a
    // change of the output size calls Resize, a change of the back surface size
    // OnResizeBackSurface, then OnActivateDC follows and, with doAutoStartUpdateFrameLoop, the
    // frame loop starts. - DeactivateDC calls OnDeactivateDC and deactivates the active
    // controller. - Destroy stops the frame loop, deactivates the controller and frees the full
    // screen window unless it is already being destroyed. The frame loop: -
    // StartUpdateFrameLoop runs UpdateFrame at FrameRate frames per second. In umAsynchrone
    // mode a TUpdateFrameThread waits for the frame time (FFrameTime, 1000 / FrameRate ms, 2 ms
    // less for the thread) and calls UpdateFrame in the main thread through Synchronize. in
    // umSynchrone mode StartUpdateFrameLoop itself loops, calling UpdateFrame and
    // Application.ProcessMessages, until the loop is stopped or the application terminates. -
    // PauseFrameUpdateLoop and ResumeFrameUpdateLoop nest (_pauseCount) and suspend and resume
    // the thread. StopUpdateFrameLoop resumes the thread as often as needed and terminates it.
    // - UpdateFrame switches the triple buffer, sends a MouseMove(-1, -1) when the mouse left
    // the output with no button down, calls OnUpdateFrame (the game draws the frame on the back
    // surface it gets. an exception there only shows a message), counts the frame for the frame
    // rate (NewFrame), waits for the frame time or the vertical blank (WaitForVerticalBlank)
    // and shows the back surface (ExecuteBltSurfaceEvent). - ExecuteBltSurfaceEvent calls
    // OnBltSurface and then, depending on BackSurfaceControlMode: bmBltSurface blits the whole
    // back surface to the output (BltSurface), bmBltInvalidatedSurface blits only the blocks
    // marked in the controller's invalidation matrix (BltInvalidatedSurface. Invalidate and
    // InvalidateAll mark them), bmPageFlipping flips (FlipSurface. controllers that cannot flip
    // blit and return DCERR_NOFLIP) and bmNone shows nothing. With doShowFrameRate the
    // controller writes the frame rate on the output. The options (Options): -
    // doAutoResizeBackSurface: the back surfaces follow the size of the output (Resize and the
    // controllers' Activate). - doAutoSwapDC: not tested in this unit. -
    // doAutoStartUpdateFrameLoop: ActivateDC and SetUpdateFrameMode start the frame loop. -
    // doAutoActivatePrimaryDC: CreateWnd activates the primary controller. - doShowFrameRate:
    // ExecuteBltSurfaceEvent shows the frame rate. - doShowErrors: ShowError shows a message
    // box. - doFocusAble: a mouse click focuses the output window. - doRestoreSurfacesOnSwap:
    // ActivateDC copies the surface contents to the new controller. - doBltOnWMPAINT: WM_PAINT
    // invalidates everything and shows the back surface again. - doAutoEnableDDraw:
    // TWindows16DC activates DirectDraw to find the best 16 bit format. The status (FStatus):
    // dsFrameLoopActive and dsFrameLoopPaused belong to the frame loop. dsDDrawEnabled,
    // dsDirectInputEnabled, dsAsynchrone and dsExclusiveAccess are not used here. The events: -
    // OnBltSurface(Sender): before the back surface is shown. - OnUpdateFrame(Sender, DC,
    // SurfaceType): draw the frame on back surface SurfaceType of DC. - OnSurfaceDataLost(DC,
    // SurfaceType): the contents of a surface were lost (a DirectDraw surface was restored, or
    // the back surfaces were resized) and must be drawn again. - OnResize(Sender): the output
    // changed its size. - OnError(ErrorStr, Error): declared and published, not called in this
    // unit. - OnActivateDisplayController, OnDeactivateDisplayController(Sender, DC): a controller was activated or is
    // deactivated. - OnHide, OnShow(Sender): the output is hidden or shown (Hide and Show. the
    // full screen controllers hide when the application is deactivated and show again through
    // WM_DISPLAYC with WParam 1). - OnResizeBackSurface(DC, Width, Height): the back surfaces
    // got a new size. - OnSwapDC(DC): SwapDC is about to activate DC. The messages: WM_SIZE
    // calls Resize. WM_PAINT is handled as described for doBltOnWMPAINT. the windowed
    // controllers see the window messages first (TAbstractDC.WndProc) and swallow WM_NCPAINT
    // and WM_ERASEBKGND. CM_WANTSPECIALKEY and CN_KEYUP keep the arrow keys for the display.
    // WM_DISPLAYC carries the posted requests. The controllers (Register puts them on the GFX
    // Engine page of the component palette. TDDrawFullScr08DC and the TDDrawWnd DCs have
    // bitmaps in the package but no code): - TWindowsDC and its descendants draw in the
    // display's window through GDI: TWindows08DC with 8 bit DIBs and a logical palette,
    // TWindows16DC with 16 bit DIB sections (555 or 565, PixelFormat), TWindows24DC and
    // TWindows32DC with 24 and 32 bit ones. Every block of the invalidation matrix is blitted
    // on its own (GetInvBlockSize). - TDDrawFullScrDC and its descendants switch the screen to
    // DisplayWidth x DisplayHeight at 16, 24 or 32 bits and draw in the full screen window
    // through DirectDraw: a flipping chain in video memory when the back surfaces have the size
    // of the screen (UseVideoMemory), else separate back surfaces that are blitted. They hide
    // the application's forms while they are active, hook the main window for WM_ACTIVATEAPP
    // and WM_DISPLAYCHANGE, and restore lost surfaces (VerifyLostSurfaces). UseVerticalBlank
    // waits for the vertical blank. DebugMode leaves the window where it is. Enable3D asks for
    // 3D capable surfaces. The error codes: DCERR_NOTSUPPORTED (not supported by the
    // controller), DCERR_ACTIVATEFAILED, DCERR_FAILED (no invalidation matrix, or an exception
    // during activation), DCERR_ACTIVATED (already activated), DCERR_NOTACTIVATED and
    // DCERR_NOFLIP (blitted instead of flipped). The controller interface (TAbstractDC), in the
    // methods: - GetInvMatrix, InitInvMatrix, Invalidate, InvalidateAll: the invalidation
    // matrix (blocks of the back surface that changed. TMatrix values count down once per shown
    // frame). - SetPrimarySurfaceRgn, GetPrimarySurfaceRgn: the rectangle of the output that
    // shows the back surface. - GetStatus (dsVisible, dsActivated, dsWindowed). GetDisplay,
    // OutputWnd and Handle: the display, the control and the window drawn in. - GetPalette,
    // GetHPalette, SetPalet, RealizePaletNow: the palette of 8 bit controllers. - GetSurface,
    // GetSurfaceCount: surface 0 (the output) and the back surfaces. - WndProc: the window
    // messages of the windowed controllers (False swallows a message). - SupportDisplayMode:
    // whether the controller can work with the display (SupportDC). - WindowRecreated: the
    // window handle changed. - ShowError passes an error message to the display. Hide and Show
    // hide and show the output. - GetPixPosofBlock, GetBlockPosofPix, GetInvBlockSize: block
    // and pixel coordinates of the invalidation matrix. - NewFrame, ShowFrameRate: the frame
    // rate. Activate, Deactivate: start and stop working. - FlipSurface, BltSurface,
    // BltInvalidatedSurface: show a back surface. - ResizeBackSurfaces: gives the back surfaces
    // a new size. - GetMousePos: the mouse position in output coordinates (-1, -1 outside). -
    // WaitForVerticalBlank: waits for the frame time or the vertical blank. This comment also
    // keeps the Assert in CreateWnd on the line that the original reports (1350, counted
    // through the include file)
    protected internal override int GetPalette()
    {
        /* TODO: Implement Controls.GetPalette in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1382
    protected internal override void MouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        /* TODO: Implement Windows.SetFocus in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1341
    protected internal override void CreateParams(ref CreateParams paramsValue)
    {
        /* TODO: Implement Controls.CreateParams in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1348
    protected internal override void CreateWindow()
    {
        /* TODO: Implement Controls.CreateWnd in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1588
    protected internal override void WindowProc(ref Message message)
    {
        /* TODO: Implement Controls.WndProc in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1541
    public virtual CustomDisplayController GetDisplayController(int index)
    {
        CustomDisplayController result;
        switch (index)
        {
            case -1:
                result = _activeDisplayController;
                break;
            case 0:
                result = _primaryDisplayController;
                break;
            case 1:
                result = _secondaryDisplayController;
                break;
            default:
                result = null;
                break;
        }

        return result;
    }

    // DisplayC.pas:1361
    public virtual void SetFrameRate(int value)
    {
        _frameRate = value;
        if (_frameRate != 0)
        {
            FrameTime = Round((double)1000 / _frameRate);
        }
        else
        {
            FrameTime = 0;
        }
    }

    // DisplayC.pas:1492
    public virtual void ResizeBackSurfaces(int width, int height)
    {
        _backSurfaceWidth = width;
        _backSurfaceHeight = height;
        if (!ComponentState.Contains(Rtl.ComponentState.Designing) && _activeDisplayController != null && _bufferingScheme != BufferingScheme.Single)
        {
            if (_activeDisplayController.GetSurface(1).Width != width || _activeDisplayController.GetSurface(1).Height != height)
            {
                _activeDisplayController.ResizeBackSurfaces(_backSurfaceWidth, _backSurfaceHeight);
                for (int I = 1, last = (int)_bufferingScheme; I <= last; I++)
                {
                    SurfaceDataLost(I);
                }
            }
        }

        if (_onResizeBackSurface != null)
        {
            _onResizeBackSurface(_activeDisplayController, width, height);
        }
    }

    // DisplayC.pas:1486
    public virtual void SurfaceDataLost(int surfaceType)
    {
        if (_onSurfaceDataLost != null)
        {
            _onSurfaceDataLost(_activeDisplayController, surfaceType);
        }
    }

    // DisplayC.pas:1469
    public virtual void GetMousePosition(ref int X, ref int Y)
    {
        if (_activeDisplayController != null)
        {
            _activeDisplayController.GetMousePosition(ref X, ref Y);
        }
    }

    // DisplayC.pas:1630
    public virtual int ExecuteBlitSurfaceEvent()
    {
        int result;
        try
        {
            result = 0;
            if (_activeDisplayController != null && _activeDisplayController.GetStatus().Contains(DisplayControllerStatusElement.Visible))
            {
                if (_onBlitSurface != null)
                {
                    _onBlitSurface(this);
                }

                if (_bufferingScheme != BufferingScheme.Single)
                {
                    switch (_backSurfaceControlMode)
                    {
                        case BackSurfaceControlMode.PageFlipping:
                            result = _activeDisplayController.FlipSurface(SurfaceIndex);
                            break;
                        case BackSurfaceControlMode.BlitSurface:
                            result = _activeDisplayController.BlitSurface(SurfaceIndex);
                            break;
                        case BackSurfaceControlMode.BlitInvalidatedSurface:
                            result = _activeDisplayController.BlitInvalidatedSurface(SurfaceIndex);
                            break;
                    }
                }

                if (_options.Contains(OptionsElement.ShowFrameRate))
                {
                    _activeDisplayController.ShowFrameRate();
                }
            }
        }
        catch
        {
            ShowMessage("Exception occured during ExecuteBltSurface event");
            throw;
        }

        return result;
    }

    // DisplayC.pas:1568
    public bool GetMouseAboveSurface()
    {
        /* TODO: Implement Windows.GetWindowRect in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1653
    public new void Hide()
    {
        if (_onHide != null)
        {
            _onHide(this);
        }

        if (_activeDisplayController != null && !ComponentState.Contains(Rtl.ComponentState.Designing))
        {
            _activeDisplayController.Hide();
        }
    }

    // DisplayC.pas:1661
    public new void Show()
    {
        if (_activeDisplayController != null && !ComponentState.Contains(Rtl.ComponentState.Designing))
        {
            _activeDisplayController.Show();
        }

        if (_onShow != null)
        {
            _onShow(this);
        }
    }

    // DisplayC.pas:1669
    public int SupportDisplayController(CustomDisplayController displayController)
    {
        int result;
        if (displayController != null)
        {
            result = displayController.SupportDisplayMode(this);
        }
        else
        {
            result = DisplayCodes.DCERR_NOTSUPPORTED;
        }

        return result;
    }

    // DisplayC.pas:1755
    public int DeactivateDisplayController()
    {
        if (_activeDisplayController != null && _activeDisplayController.GetStatus().Contains(DisplayControllerStatusElement.Activated))
        {
            if (_onDeactivateDisplayController != null)
            {
                _onDeactivateDisplayController(this, _activeDisplayController);
            }

            _activeDisplayController.Deactivate();
            _activeDisplayController = null;
        }

        return 0;
    }

    public new int Cursor { get => GetCursor(); set => SetCursor(value); }
    public CustomDisplayController ActiveDisplayController { get => _activeDisplayController; }
    public DirectDrawFullScreenWindowControl FullScreenWindowControl { get => _fullScreenWindowControl; }
    protected internal Set<OptionsElement> Options { get => _options; set => _options = value; }
    protected internal BufferingScheme BufferingScheme { get => _bufferingScheme; set => SetBufferingScheme(value); }
    protected internal BackSurfaceControlMode BackSurfaceControlMode { get => _backSurfaceControlMode; set => _backSurfaceControlMode = value; }
    protected internal int BackSurfaceWidth { get => _backSurfaceWidth; set => SetBackSurfaceWidth(value); }
    protected internal int BackSurfaceHeight { get => _backSurfaceHeight; set => SetBackSurfaceHeight(value); }
    protected internal int FrameRate { get => _frameRate; set => SetFrameRate(value); }
    protected internal CustomDisplayController PrimaryDisplayController { get => _primaryDisplayController; set => _primaryDisplayController = value; }
    protected internal CustomDisplayController SecondaryDisplayController { get => _secondaryDisplayController; set => _secondaryDisplayController = value; }
    protected internal UpdateFrameMode UpdateFrameMode { get => _updateFrameMode; set => SetUpdateFrameMode(value); }
    protected internal NotifyEvent<object> OnResize { get => _onResize; set => _onResize = value; }
    protected internal NotifyEvent<object> OnBlitSurface { get => _onBlitSurface; set => _onBlitSurface = value; }
    protected internal DCNotifyEvent OnDeactivateDisplayController { get => _onDeactivateDisplayController; set => _onDeactivateDisplayController = value; }
    protected internal DCNotifyEvent OnActivateDisplayController { get => _onActivateDisplayController; set => _onActivateDisplayController = value; }
    protected internal UpdateFrameEvent OnUpdateFrame { get => _onUpdateFrame; set => _onUpdateFrame = value; }
    protected internal NotifyEvent<object> OnHide { get => _onHide; set => _onHide = value; }
    protected internal NotifyEvent<object> OnShow { get => _onShow; set => _onShow = value; }
    protected internal ErrorEvent OnError { get => _onError; set => _onError = value; }
    protected internal SurfaceDataLostEvent OnSurfaceDataLost { get => _onSurfaceDataLost; set => _onSurfaceDataLost = value; }
    protected internal ResizeBackSurfaceEvent OnResizeBackSurface { get => _onResizeBackSurface; set => _onResizeBackSurface = value; }
    protected internal SwapDCEvent OnSwapDisplayController { get => _onSwapDisplayController; set => _onSwapDisplayController = value; }

    private const int DCERR_ACTIVATEFAILED = -2147414014;

    private static int RemapSurface(Surface source, Surface destination)
    {
        LockedSurface lockedSurface = default(LockedSurface);
        LockedSurface lockedSurface2 = default(LockedSurface);
        source.Lock(ref lockedSurface);
        destination.Lock(ref lockedSurface2);
        int result = LockedSurface.RemapSurfacePart(in lockedSurface, 0, 0, lockedSurface.Width, lockedSurface.Height, in lockedSurface2, 0, 0);
        source.Unlock();
        destination.Unlock();
        return result;
    }

    public enum OptionsElement : byte
    {
        AutoResizeBackSurface = 0, AutoSwapDisplayController = 1, AutoStartUpdateFrameLoop = 2, AutoActivatePrimaryDisplayController = 3, ShowFrameRate = 4, ShowErrors = 5, FocusAble = 6, RestoreSurfacesOnSwap = 7, BlitOnPaint = 8, AutoEnableDDraw = 9
    }

    public delegate void UpdateFrameEvent(object Sender, CustomDisplayController DC, int SurfaceType);
    public delegate void DCNotifyEvent(object Sender, CustomDisplayController DC);

    public delegate void ErrorEvent(string ErrorStr, int Error);
    public delegate void SurfaceDataLostEvent(CustomDisplayController DC, int SurfaceType);

    public delegate void ResizeBackSurfaceEvent(CustomDisplayController DC, int Width, int Height);
    public delegate void SwapDCEvent(CustomDisplayController DC);

}
