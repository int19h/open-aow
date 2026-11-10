// Translated from DisplayC.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// the display control. FFSWinControl, the events (OnBltSurface, OnUpdateFrame,
// OnSurfaceDataLost, OnResize, OnError, OnActivateDC, OnDeactivateDC, OnHide, OnShow,
// OnResizeBackSurface, OnSwapDC), FFrameRate, FFrameTime (ms per frame), FCurrentFrameRate,
// FUpdateFrameThread, FStatus, FOptions (2 bytes), FUpdateFrameMode, FBackSurfaceControlMode,
// FBufferingScheme, FBackSurfaceWidth, FBackSurfaceHeight, FSurfaceIndex (the back surface to
// show), FPauseCount, FMouseAboveSurface, FPrimaryDC, FSecondaryDC, FActiveDC
public partial class CustomDisplay : WinControl
{

    internal DDrawFSWinControl FFSWinControl;
    internal NotifyEvent<object> FOnBltSurface;
    internal UpdateFrameEvent FOnUpdateFrame;
    internal SurfaceDataLostEvent FOnSurfaceDataLost;
    internal NotifyEvent<object> FOnResize;
    internal ErrorEvent FOnError;
    internal DCNotifyEvent FOnActivateDC;
    internal DCNotifyEvent FOnDeactivateDC;
    internal NotifyEvent<object> FOnHide;
    internal NotifyEvent<object> FOnShow;
    internal ResizeBackSurfaceEvent FOnResizeBackSurface;
    internal SwapDCEvent FOnSwapDC;
    internal int FFrameRate;
    internal int FFrameTime;
    protected internal int FCurrentFrameRate;

    public int CurrentFrameRate { get => FCurrentFrameRate; }

    internal UpdateFrameThread FUpdateFrameThread;
    public Set<DisplayStatusElement> FStatus;
    internal Set<DisplayOptionsElement> FOptions;
    internal UpdateFrameMode FUpdateFrameMode;
    internal BackSurfaceControlMode FBackSurfaceControlMode;
    internal BufferingScheme FBufferingScheme;
    internal int FBackSurfaceWidth;
    internal int FBackSurfaceHeight;
    public int FSurfaceIndex;
    // direct frame blit
    internal int FPauseCount;
    internal bool FMouseAboveSurface;
    internal AbstractDC FPrimaryDC;
    internal AbstractDC FSecondaryDC;
    internal AbstractDC FActiveDC;

    // DisplayC.pas:1397
    internal void SetBufferingScheme(BufferingScheme Value)
    {
        if (FActiveDC == null)
        {
            FBufferingScheme = Value;
        }
        else
        {
            ShowError("Cannot change buffering scheme when DC is activated");
        }

        switch (FBufferingScheme)
        {
            case BufferingScheme.Single:
                FSurfaceIndex = 0;
                break;
            case BufferingScheme.Double:
                FSurfaceIndex = 1;
                break;
            case BufferingScheme.Tripple:
                FSurfaceIndex = 2;
                break;
        }
    }

    // DisplayC.pas:1410
    internal void SetUpdateFrameMode(UpdateFrameMode Value)
    {
        if (!FStatus.Contains(DisplayStatusElement.FrameLoopActive))
        {
            FUpdateFrameMode = Value;
            if (!ComponentState.Contains(Rtl.ComponentState.Designing) && FOptions.Contains(DisplayOptionsElement.AutoStartUpdateFrameLoop) && !ComponentState.Contains(Rtl.ComponentState.Loading))
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
    internal void SetBackSurfaceWidth(int Value)
    {
        if (Value >= 0)
        {
            ResizeBackSurfaces(Value, FBackSurfaceHeight);
        }
    }

    // DisplayC.pas:1515
    internal void SetBackSurfaceHeight(int Value)
    {
        if (Value >= 0)
        {
            ResizeBackSurfaces(FBackSurfaceWidth, Value);
        }
    }

    // DisplayC.pas:1552
    internal void CMWantSpecialKey(ref WMKey Message)
    {
        if (Message.CharCode == 37 || Message.CharCode == 39 || Message.CharCode == 38 || Message.CharCode == 40)
        {
            Message.Result = 1;
        }
    }

    // DisplayC.pas:1560
    internal void CMKeyUP(ref WMKey Message)
    {
        if (Message.CharCode == 37 || Message.CharCode == 39 || Message.CharCode == 38 || Message.CharCode == 40)
        {
            Message.Result = 0;
        }
    }

    // DisplayC.pas:1390
    protected internal override void KeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        /* TODO: Implement Controls.KeyDown in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1327
    // (inc\TDisplay.PAS in the original) How the display works, as far as the code of this unit
    // tells: The display control has no surfaces of its own. It shows the surfaces of its
    // active display controller (FActiveDC), which is one of PrimaryDC and SecondaryDC. SwapDC
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
    // PauseFrameUpdateLoop and ResumeFrameUpdateLoop nest (FPauseCount) and suspend and resume
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
    // unit. - OnActivateDC, OnDeactivateDC(Sender, DC): a controller was activated or is
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
    protected internal override void MouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        /* TODO: Implement Windows.SetFocus in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1341
    protected internal override void CreateParams(ref CreateParams Params)
    {
        /* TODO: Implement Controls.CreateParams in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1348
    protected internal override void CreateWnd()
    {
        /* TODO: Implement Controls.CreateWnd in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1588
    protected internal override void WndProc(ref Message Message)
    {
        /* TODO: Implement Controls.WndProc in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1541
    public virtual AbstractDC GetDC(int Index)
    {
        AbstractDC Result;
        switch (Index)
        {
            case -1:
                Result = FActiveDC;
                break;
            case 0:
                Result = FPrimaryDC;
                break;
            case 1:
                Result = FSecondaryDC;
                break;
            default:
                Result = null;
                break;
        }

        return Result;
    }

    // DisplayC.pas:1361
    public virtual void SetFrameRate(int Value)
    {
        FFrameRate = Value;
        if (FFrameRate != 0)
        {
            FFrameTime = Round((double)1000 / FFrameRate);
        }
        else
        {
            FFrameTime = 0;
        }
    }

    // DisplayC.pas:1492
    public virtual void ResizeBackSurfaces(int AWidth, int AHeight)
    {
        FBackSurfaceWidth = AWidth;
        FBackSurfaceHeight = AHeight;
        if (!ComponentState.Contains(Rtl.ComponentState.Designing) && FActiveDC != null && FBufferingScheme != BufferingScheme.Single)
        {
            if (FActiveDC.GetSurface(1).Width != AWidth || FActiveDC.GetSurface(1).Height != AHeight)
            {
                FActiveDC.ResizeBackSurfaces(FBackSurfaceWidth, FBackSurfaceHeight);
                for (int I = 1, last = (int)FBufferingScheme; I <= last; I++)
                {
                    SurfaceDataLost(I);
                }
            }
        }

        if (FOnResizeBackSurface != null)
        {
            FOnResizeBackSurface(FActiveDC, AWidth, AHeight);
        }
    }

    // DisplayC.pas:1486
    public virtual void SurfaceDataLost(int SurfaceType)
    {
        if (FOnSurfaceDataLost != null)
        {
            FOnSurfaceDataLost(FActiveDC, SurfaceType);
        }
    }

    // DisplayC.pas:1469
    public virtual void GetMousePos(ref int X, ref int Y)
    {
        if (FActiveDC != null)
        {
            FActiveDC.GetMousePos(ref X, ref Y);
        }
    }

    // DisplayC.pas:1630
    public virtual int ExecuteBltSurfaceEvent()
    {
        int Result;
        try
        {
            Result = 0;
            if (FActiveDC != null && FActiveDC.GetStatus().Contains(DisplayCtrlStatusElement.Visible))
            {
                if (FOnBltSurface != null)
                {
                    FOnBltSurface(this);
                }

                if (FBufferingScheme != BufferingScheme.Single)
                {
                    switch (FBackSurfaceControlMode)
                    {
                        case BackSurfaceControlMode.PageFlipping:
                            Result = FActiveDC.FlipSurface(FSurfaceIndex);
                            break;
                        case BackSurfaceControlMode.BltSurface:
                            Result = FActiveDC.BltSurface(FSurfaceIndex);
                            break;
                        case BackSurfaceControlMode.BltInvalidatedSurface:
                            Result = FActiveDC.BltInvalidatedSurface(FSurfaceIndex);
                            break;
                    }
                }

                if (FOptions.Contains(DisplayOptionsElement.ShowFrameRate))
                {
                    FActiveDC.ShowFrameRate();
                }
            }
        }
        catch
        {
            ShowMessage("Exception occured during ExecuteBltSurface event");
            throw;
        }

        return Result;
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
        if (FOnHide != null)
        {
            FOnHide(this);
        }

        if (FActiveDC != null && !ComponentState.Contains(Rtl.ComponentState.Designing))
        {
            FActiveDC.Hide();
        }
    }

    // DisplayC.pas:1661
    public new void Show()
    {
        if (FActiveDC != null && !ComponentState.Contains(Rtl.ComponentState.Designing))
        {
            FActiveDC.Show();
        }

        if (FOnShow != null)
        {
            FOnShow(this);
        }
    }

    // DisplayC.pas:1669
    public int SupportDC(AbstractDC DC)
    {
        int Result;
        if (DC != null)
        {
            Result = DC.SupportDisplayMode(this);
        }
        else
        {
            Result = DisplayCodes.DCERR_NOTSUPPORTED;
        }

        return Result;
    }

    // DisplayC.pas:1755
    public int DeactivateDC()
    {
        if (FActiveDC != null && FActiveDC.GetStatus().Contains(DisplayCtrlStatusElement.Activated))
        {
            if (FOnDeactivateDC != null)
            {
                FOnDeactivateDC(this, FActiveDC);
            }

            FActiveDC.Deactivate();
            FActiveDC = null;
        }

        return 0;
    }

    public new int Cursor { get => GetCursor(); set => SetCursor(value); }
    public AbstractDC ActiveDC { get => FActiveDC; }
    public DDrawFSWinControl FSWinControl { get => FFSWinControl; }
    protected internal Set<DisplayOptionsElement> Options { get => FOptions; set => FOptions = value; }
    protected internal BufferingScheme BufferingScheme { get => FBufferingScheme; set => SetBufferingScheme(value); }
    protected internal BackSurfaceControlMode BackSurfaceControlMode { get => FBackSurfaceControlMode; set => FBackSurfaceControlMode = value; }
    protected internal int BackSurfaceWidth { get => FBackSurfaceWidth; set => SetBackSurfaceWidth(value); }
    protected internal int BackSurfaceHeight { get => FBackSurfaceHeight; set => SetBackSurfaceHeight(value); }
    protected internal int FrameRate { get => FFrameRate; set => SetFrameRate(value); }
    protected internal AbstractDC PrimaryDC { get => FPrimaryDC; set => FPrimaryDC = value; }
    protected internal AbstractDC SecondaryDC { get => FSecondaryDC; set => FSecondaryDC = value; }
    protected internal UpdateFrameMode UpdateFrameMode { get => FUpdateFrameMode; set => SetUpdateFrameMode(value); }
    protected internal NotifyEvent<object> OnResize { get => FOnResize; set => FOnResize = value; }
    protected internal NotifyEvent<object> OnBltSurface { get => FOnBltSurface; set => FOnBltSurface = value; }
    protected internal DCNotifyEvent OnDeactivateDC { get => FOnDeactivateDC; set => FOnDeactivateDC = value; }
    protected internal DCNotifyEvent OnActivateDC { get => FOnActivateDC; set => FOnActivateDC = value; }
    protected internal UpdateFrameEvent OnUpdateFrame { get => FOnUpdateFrame; set => FOnUpdateFrame = value; }
    protected internal NotifyEvent<object> OnHide { get => FOnHide; set => FOnHide = value; }
    protected internal NotifyEvent<object> OnShow { get => FOnShow; set => FOnShow = value; }
    protected internal ErrorEvent OnError { get => FOnError; set => FOnError = value; }
    protected internal SurfaceDataLostEvent OnSurfaceDataLost { get => FOnSurfaceDataLost; set => FOnSurfaceDataLost = value; }
    protected internal ResizeBackSurfaceEvent OnResizeBackSurface { get => FOnResizeBackSurface; set => FOnResizeBackSurface = value; }
    protected internal SwapDCEvent OnSwapDC { get => FOnSwapDC; set => FOnSwapDC = value; }

    private const int DCERR_ACTIVATEFAILED = -2147414014;

    private static int RemapSurface(Surface Src, Surface Dest)
    {
        SurfaceDesc SD = default(SurfaceDesc);
        SurfaceDesc SD2 = default(SurfaceDesc);
        Src.Lock(ref SD);
        Dest.Lock(ref SD2);
        int result = SurfaceDesc.RemapSurfacePart(in SD, 0, 0, SD.Width, SD.Height, in SD2, 0, 0);
        Src.Unlock();
        Dest.Unlock();
        return result;
    }

    public enum DisplayOptionsElement : byte
    {
        AutoResizeBackSurface = 0, AutoSwapDC = 1, AutoStartUpdateFrameLoop = 2, AutoActivatePrimaryDC = 3, ShowFrameRate = 4, ShowErrors = 5, FocusAble = 6, RestoreSurfacesOnSwap = 7, BltOnWMPAINT = 8, AutoEnableDDraw = 9
    }

    public delegate void UpdateFrameEvent(object Sender, AbstractDC DC, int SurfaceType);
    public delegate void DCNotifyEvent(object Sender, AbstractDC DC);

    public delegate void ErrorEvent(string ErrorStr, int Error);
    public delegate void SurfaceDataLostEvent(AbstractDC DC, int SurfaceType);

    public delegate void ResizeBackSurfaceEvent(AbstractDC DC, int Width, int Height);
    public delegate void SwapDCEvent(AbstractDC DC);

}
