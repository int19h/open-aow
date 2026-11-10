// Translated from DisplayC.pas.
using System.Runtime.InteropServices;
using Aow.Game.Draw;
using Aow.Game.Engine.Buffers;
using Aow.Rtl;
using static Aow.Game.Draw.GFXEUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

[StructLayout(LayoutKind.Explicit, Size = 1028)]
public struct Record_TWindows08DC_CreateHPalette_LogPal_2256_3
{
    [FieldOffset(0)]
    public ushort palVersion;
    [FieldOffset(2)]
    public ushort palNumEntries;
    [FieldOffset(4)]
    public FixedArray_Aow_Rtl_TPaletteEntry_256 palPalEntry;
}

public enum TDisplayCtrlStatusElement : byte
{
    dsVisible = 0,
    dsActivated = 1,
    dsWindowed = 2
}

public enum TDisplayStatusElement : byte
{
    dsFrameLoopActive = 0,
    dsDDrawEnabled = 1,
    dsDirectInputEnabled = 2,
    dsAsynchrone = 3,
    dsExclusiveAccess = 4,
    dsFrameLoopPaused = 5
}

public enum TDisplayOptionsElement : byte
{
    doAutoResizeBackSurface = 0,
    doAutoSwapDC = 1,
    doAutoStartUpdateFrameLoop = 2,
    doAutoActivatePrimaryDC = 3,
    doShowFrameRate = 4,
    doShowErrors = 5,
    doFocusAble = 6,
    doRestoreSurfacesOnSwap = 7,
    doBltOnWMPAINT = 8,
    doAutoEnableDDraw = 9
}

[LegacyObject]
// a byte matrix (the invalidated blocks of a surface). FData, FWidth, FHeight. The port's FData
// is the array of FWidth * FHeight bytes, row by row, and null for an empty matrix as GetMem of
// size 0 gives nil.
public partial class TMatrix : IDisposable
{
    internal byte[] FData;
    internal byte FWidth;
    internal byte FHeight;

    // DisplayC.pas:3145
    // { TMatrix (DCTypes in the original) }
    public TMatrix(byte AWidth, byte AHeight)
    {
        CaptureAddress.Assign(this);
        FWidth = AWidth;
        FHeight = AHeight;
        FData = FWidth * FHeight == 0 ? null : new byte[FWidth * FHeight];
    }

    // DisplayC.pas:3155
    public virtual void Dispose()
    {
        FData = null;
    }

    // DisplayC.pas:3166
    // { The value at X, Y, clipped to 2. }
    public int GetVal(byte X, byte Y)
    {
        int Result;
        if (X < FWidth && Y < FHeight)
        {
            Result = FData[X + Y * FWidth];
            if (Result > 2)
            {
                Result = 2;
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(paramName: null, "TMatrix out of bounds (DCTypes)");
        }

        return Result;
    }

    // DisplayC.pas:3178
    public void SetVal(byte X, byte Y, byte Value)
    {
        if (X < FWidth && Y < FHeight)
        {
            FData[X + Y * FWidth] = Value;
        }
    }

    public byte Width { get => FWidth; }
    public byte Height { get => FHeight; }

}

// the display control. FFSWinControl, the events (OnBltSurface, OnUpdateFrame,
// OnSurfaceDataLost, OnResize, OnError, OnActivateDC, OnDeactivateDC, OnHide, OnShow,
// OnResizeBackSurface, OnSwapDC), FFrameRate, FFrameTime (ms per frame), FCurrentFrameRate,
// FUpdateFrameThread, FStatus, FOptions (2 bytes), FUpdateFrameMode, FBackSurfaceControlMode,
// FBufferingScheme, FBackSurfaceWidth, FBackSurfaceHeight, FSurfaceIndex (the back surface to
// show), FPauseCount, FMouseAboveSurface, FPrimaryDC, FSecondaryDC, FActiveDC
public partial class TCustomDisplay : TWinControl
{

    internal TDDrawFSWinControl FFSWinControl;
    internal TNotifyEvent<object> FOnBltSurface;
    internal TUpdateFrameEvent FOnUpdateFrame;
    internal TSurfaceDataLostEvent FOnSurfaceDataLost;
    internal TNotifyEvent<object> FOnResize;
    internal TErrorEvent FOnError;
    internal TDCNotifyEvent FOnActivateDC;
    internal TDCNotifyEvent FOnDeactivateDC;
    internal TNotifyEvent<object> FOnHide;
    internal TNotifyEvent<object> FOnShow;
    internal TResizeBackSurfaceEvent FOnResizeBackSurface;
    internal TSwapDCEvent FOnSwapDC;
    internal int FFrameRate;
    internal int FFrameTime;
    protected internal int FCurrentFrameRate;

    public int CurrentFrameRate { get => FCurrentFrameRate; }

    internal TUpdateFrameThread FUpdateFrameThread;
    public TSet<TDisplayStatusElement> FStatus;
    internal TSet<TDisplayOptionsElement> FOptions;
    internal TUpdateFrameMode FUpdateFrameMode;
    internal TBackSurfaceControlMode FBackSurfaceControlMode;
    internal TBufferingScheme FBufferingScheme;
    internal int FBackSurfaceWidth;
    internal int FBackSurfaceHeight;
    public int FSurfaceIndex;
    // direct frame blit
    internal int FPauseCount;
    internal bool FMouseAboveSurface;
    internal TAbstractDC FPrimaryDC;
    internal TAbstractDC FSecondaryDC;
    internal TAbstractDC FActiveDC;

    // DisplayC.pas:1397
    internal void SetBufferingScheme(TBufferingScheme Value)
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
            case TBufferingScheme.bsSingle:
                FSurfaceIndex = 0;
                break;
            case TBufferingScheme.bsDouble:
                FSurfaceIndex = 1;
                break;
            case TBufferingScheme.bsTripple:
                FSurfaceIndex = 2;
                break;
        }
    }

    // DisplayC.pas:1410
    internal void SetUpdateFrameMode(TUpdateFrameMode Value)
    {
        if (!FStatus.Contains(TDisplayStatusElement.dsFrameLoopActive))
        {
            FUpdateFrameMode = Value;
            if (!ComponentState.Contains(TComponentState.csDesigning) && FOptions.Contains(TDisplayOptionsElement.doAutoStartUpdateFrameLoop) && !ComponentState.Contains(TComponentState.csLoading))
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
    internal void CMWantSpecialKey(ref TWMKey Message)
    {
        if (Message.CharCode == 37 || Message.CharCode == 39 || Message.CharCode == 38 || Message.CharCode == 40)
        {
            Message.Result = 1;
        }
    }

    // DisplayC.pas:1560
    internal void CMKeyUP(ref TWMKey Message)
    {
        if (Message.CharCode == 37 || Message.CharCode == 39 || Message.CharCode == 38 || Message.CharCode == 40)
        {
            Message.Result = 0;
        }
    }

    // DisplayC.pas:1390
    protected internal override void KeyDown(ref ushort Key, TSet<TShiftState> Shift)
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
    protected internal override void MouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        /* TODO: Implement Windows.SetFocus in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1341
    protected internal override void CreateParams(ref TCreateParams Params)
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
    protected internal override void WndProc(ref TMessage Message)
    {
        /* TODO: Implement Controls.WndProc in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1541
    public virtual TAbstractDC GetDC(int Index)
    {
        TAbstractDC Result;
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
        if (!ComponentState.Contains(TComponentState.csDesigning) && FActiveDC != null && FBufferingScheme != TBufferingScheme.bsSingle)
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
            if (FActiveDC != null && FActiveDC.GetStatus().Contains(TDisplayCtrlStatusElement.dsVisible))
            {
                if (FOnBltSurface != null)
                {
                    FOnBltSurface(this);
                }

                if (FBufferingScheme != TBufferingScheme.bsSingle)
                {
                    switch (FBackSurfaceControlMode)
                    {
                        case TBackSurfaceControlMode.bmPageFlipping:
                            Result = FActiveDC.FlipSurface(FSurfaceIndex);
                            break;
                        case TBackSurfaceControlMode.bmBltSurface:
                            Result = FActiveDC.BltSurface(FSurfaceIndex);
                            break;
                        case TBackSurfaceControlMode.bmBltInvalidatedSurface:
                            Result = FActiveDC.BltInvalidatedSurface(FSurfaceIndex);
                            break;
                    }
                }

                if (FOptions.Contains(TDisplayOptionsElement.doShowFrameRate))
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

        if (FActiveDC != null && !ComponentState.Contains(TComponentState.csDesigning))
        {
            FActiveDC.Hide();
        }
    }

    // DisplayC.pas:1661
    public new void Show()
    {
        if (FActiveDC != null && !ComponentState.Contains(TComponentState.csDesigning))
        {
            FActiveDC.Show();
        }

        if (FOnShow != null)
        {
            FOnShow(this);
        }
    }

    // DisplayC.pas:1669
    public int SupportDC(TAbstractDC DC)
    {
        int Result;
        if (DC != null)
        {
            Result = DC.SupportDisplayMode(this);
        }
        else
        {
            Result = DisplayCUnit.DCERR_NOTSUPPORTED;
        }

        return Result;
    }

    // DisplayC.pas:1755
    public int DeactivateDC()
    {
        if (FActiveDC != null && FActiveDC.GetStatus().Contains(TDisplayCtrlStatusElement.dsActivated))
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
    public TAbstractDC ActiveDC { get => FActiveDC; }
    public TDDrawFSWinControl FSWinControl { get => FFSWinControl; }
    protected internal TSet<TDisplayOptionsElement> Options { get => FOptions; set => FOptions = value; }
    protected internal TBufferingScheme BufferingScheme { get => FBufferingScheme; set => SetBufferingScheme(value); }
    protected internal TBackSurfaceControlMode BackSurfaceControlMode { get => FBackSurfaceControlMode; set => FBackSurfaceControlMode = value; }
    protected internal int BackSurfaceWidth { get => FBackSurfaceWidth; set => SetBackSurfaceWidth(value); }
    protected internal int BackSurfaceHeight { get => FBackSurfaceHeight; set => SetBackSurfaceHeight(value); }
    protected internal int FrameRate { get => FFrameRate; set => SetFrameRate(value); }
    protected internal TAbstractDC PrimaryDC { get => FPrimaryDC; set => FPrimaryDC = value; }
    protected internal TAbstractDC SecondaryDC { get => FSecondaryDC; set => FSecondaryDC = value; }
    protected internal TUpdateFrameMode UpdateFrameMode { get => FUpdateFrameMode; set => SetUpdateFrameMode(value); }
    protected internal TNotifyEvent<object> OnResize { get => FOnResize; set => FOnResize = value; }
    protected internal TNotifyEvent<object> OnBltSurface { get => FOnBltSurface; set => FOnBltSurface = value; }
    protected internal TDCNotifyEvent OnDeactivateDC { get => FOnDeactivateDC; set => FOnDeactivateDC = value; }
    protected internal TDCNotifyEvent OnActivateDC { get => FOnActivateDC; set => FOnActivateDC = value; }
    protected internal TUpdateFrameEvent OnUpdateFrame { get => FOnUpdateFrame; set => FOnUpdateFrame = value; }
    protected internal TNotifyEvent<object> OnHide { get => FOnHide; set => FOnHide = value; }
    protected internal TNotifyEvent<object> OnShow { get => FOnShow; set => FOnShow = value; }
    protected internal TErrorEvent OnError { get => FOnError; set => FOnError = value; }
    protected internal TSurfaceDataLostEvent OnSurfaceDataLost { get => FOnSurfaceDataLost; set => FOnSurfaceDataLost = value; }
    protected internal TResizeBackSurfaceEvent OnResizeBackSurface { get => FOnResizeBackSurface; set => FOnResizeBackSurface = value; }
    protected internal TSwapDCEvent OnSwapDC { get => FOnSwapDC; set => FOnSwapDC = value; }

}

// the interface of a display controller. 35 new virtual methods, all abstract but GetSurface
// and BltInvalidatedSurface
public partial class TAbstractDC : TComponent
{
    public TAbstractDC(TComponent owner) : base(owner)
    { }

    // DisplayC.pas:74
    public virtual TMatrix GetInvMatrix()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:75
    public virtual void SetPrimarySurfaceRgn(in TRect R)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:76
    public virtual TRect? GetPrimarySurfaceRgn()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:77
    public virtual TSet<TDisplayCtrlStatusElement> GetStatus()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:78
    public virtual TCustomDisplay GetDisplay()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:79
    public virtual TGFXEPalette GetPalette()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:554
    public virtual TSurface GetSurface(int Index) => null;

    // DisplayC.pas:81
    public virtual int GetSurfaceCount()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:82
    public virtual bool WndProc(ref TMessage Message)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:83
    public virtual int SupportDisplayMode(TCustomDisplay ADisplay)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:84
    public virtual void WindowRecreated()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:85
    public virtual void ShowError(string Msg)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:86
    public virtual int GetHPalette()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:87
    public virtual int Hide()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:88
    public virtual int Show()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:89
    public virtual void GetPixPosofBlock(int BX, int BY, TRect R, ref int PX, ref int PY, ref int BW, ref int BH)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:90
    public virtual void GetBlockPosofPix(int PX, int PY, TRect R, ref int BX, ref int BY)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:91
    public virtual int GetInvBlockSize(int X, int Y, TMatrix Matrix, ref int BW, ref int BH)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:92
    public virtual void NewFrame()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:93
    public virtual void ShowFrameRate()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:94
    public virtual int Activate(TCustomDisplay ADisplay)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:95
    public virtual int Deactivate()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:96
    public virtual int FlipSurface(int Index)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:97
    public virtual int BltSurface(int Index)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:562
    // { Blits the invalidated blocks of back surface Index to the primary surface: every block the
    //   matrix marks is merged with its marked neighbours (GetInvBlockSize) into one rectangle, and
    //   the marks count down, so a block is blitted once per back surface. }
    public virtual int BltInvalidatedSurface(int Index)
    {
        int Result;
        TMatrix M;
        int BW = default;
        int BH = default;
        int PX2 = default;
        int PY2 = default;
        int BlkW = default;
        int BlkH = default;
        int V;
        TRect ClipR = default;
        TRect SrcR = default;
        TRect DstR = default;
        if (GetInvMatrix() != null)
        {
            Result = 0;
            var invMatrix = GetInvMatrix();
            M = new TMatrix(GetInvMatrix().Width, invMatrix.Height);
            for (int I = 0, last = GetInvMatrix().Width - 1; I <= last; I++)
            {
                for (int J = 0, lastJ = GetInvMatrix().Height - 1; J <= lastJ; J++)
                {
                    if (GetInvMatrix().GetVal((byte)I, (byte)J) != 0)
                    {
                        M.SetVal((byte)I, (byte)J, 1);
                    }
                }
            }

            ClipR.Left = 0;
            ClipR.Right = GetSurface(1).Width;
            ClipR.Top = 0;
            ClipR.Bottom = GetSurface(1).Height;
            for (int J = 0, lastJ = M.Height - 1; J <= lastJ; J++)
            {
                for (int I = 0, last = M.Width - 1; I <= last; I++)
                {
                    if (Result >= 0)
                    {
                        V = GetInvMatrix().GetVal((byte)I, (byte)J);
                        if (V != 0)
                        {
                            GetInvMatrix().SetVal((byte)I, (byte)J, (byte)(V - 1));
                        }

                        if (M.GetVal((byte)I, (byte)J) != 0)
                        {
                            GetInvBlockSize(I, J, M, ref BW, ref BH);
                            GetPixPosofBlock(I, J, ClipR, ref SrcR.Left, ref SrcR.Top, ref BlkW, ref BlkH);
                            if (BW > 1 || BH > 1)
                            {
                                GetPixPosofBlock(I + BW - 1, J + BH - 1, ClipR, ref PX2, ref PY2, ref BlkW, ref BlkH);
                                SrcR.Right = PX2 + BlkW;
                                SrcR.Bottom = PY2 + BlkH;
                            }
                            else
                            {
                                SrcR.Right = SrcR.Left + BlkW;
                                SrcR.Bottom = SrcR.Top + BlkH;
                            }

                            var primarySurfaceRgn = GetPrimarySurfaceRgn();
                            GetPixPosofBlock(I, J, primarySurfaceRgn.Value, ref DstR.Left, ref DstR.Top, ref BlkW, ref BlkH);
                            var primarySurfaceRgn2 = GetPrimarySurfaceRgn();
                            DstR.Left += primarySurfaceRgn2.Value.Left;
                            var primarySurfaceRgn3 = GetPrimarySurfaceRgn();
                            DstR.Top += primarySurfaceRgn3.Value.Top;
                            if (BW > 1 || BH > 1)
                            {
                                var primarySurfaceRgn4 = GetPrimarySurfaceRgn();
                                GetPixPosofBlock(I + BW - 1, J + BH - 1, primarySurfaceRgn4.Value, ref PX2, ref PY2, ref BlkW, ref BlkH);
                                var primarySurfaceRgn5 = GetPrimarySurfaceRgn();
                                DstR.Right = PX2 + BlkW + primarySurfaceRgn5.Value.Left;
                                var primarySurfaceRgn6 = GetPrimarySurfaceRgn();
                                DstR.Bottom = PY2 + BlkH + primarySurfaceRgn6.Value.Top;
                            }
                            else
                            {
                                DstR.Right = DstR.Left + BlkW;
                                DstR.Bottom = DstR.Top + BlkH;
                            }

                            var surface = GetSurface(0);
                            Result = GetSurface(Index).BltStretch(surface, SrcR, DstR);
                            for (int X = 0, lastX = BW - 1; X <= lastX; X++)
                            {
                                for (int Y = 0, lastY = BH - 1; Y <= lastY; Y++)
                                {
                                    M.SetVal((byte)(I + X), (byte)(J + Y), 0);
                                }
                            }
                        }
                    }
                }
            }

            M.Dispose();
        }
        else
        {
            Result = DisplayCUnit.DCERR_FAILED;
        }

        return Result;
    }

    // DisplayC.pas:99
    public virtual int Handle()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:100
    public virtual TWinControl OutputWnd()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:101
    public virtual int ResizeBackSurfaces(int AWidth, int AHeight)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:102
    public virtual void GetMousePos(ref int X, ref int Y)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:103
    public virtual int RealizePaletNow()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:104
    public virtual int SetPalet(TGFXEPalette Palette)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:105
    public virtual int WaitForVerticalBlank()
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:106
    public virtual void InitInvMatrix(byte AWidth, byte AHeight)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:107
    public virtual void Invalidate(int X, int Y, int W, int H)
    {
        throw EAbstractError.Create("Abstract Error");
    }

    // DisplayC.pas:108
    public virtual void InvalidateAll()
    {
        throw EAbstractError.Create("Abstract Error");
    }
}

// the common part of the display controllers. FLastFrameTime, FFrameCount (Word), FStatus,
// FDisplay, FPrimarySurfaceRgn (the output rectangle), FInvMatrix, FRealizePalet, FLastVBTime
public partial class TDisplayCtrl : TAbstractDC
{

    internal int FLastFrameTime;
    internal ushort FFrameCount;
    internal TSet<TDisplayCtrlStatusElement> FStatus;
    internal TCustomDisplay FDisplay;
    internal TRect FPrimarySurfaceRgn;
    internal TMatrix FInvMatrix;
    internal bool FRealizePalet;
    internal int FLastVBTime;

    public TDisplayCtrl(TComponent owner) : base(owner)
    {
        FInvMatrix = new TMatrix(4, 4);
    }

    // DisplayC.pas:1031
    public override void Dispose()
    {
        if (FStatus.Contains(TDisplayCtrlStatusElement.dsActivated))
        {
            Deactivate();
        }

        if (FInvMatrix != null)
        {
            FInvMatrix.Dispose();
            FInvMatrix = null;
        }

        base.Dispose();
    }

    // DisplayC.pas:634
    public override TMatrix GetInvMatrix() => FInvMatrix;

    // DisplayC.pas:639
    public override int GetSurfaceCount()
    {
        int Result;
        if (GetDisplay() == null)
        {
            Result = 0;
        }
        else
        {
            Result = (int)GetDisplay().FBufferingScheme;
        }

        return Result;
    }

    // DisplayC.pas:647
    public override int BltSurface(int Index)
    {
        var primarySurfaceRgn = GetPrimarySurfaceRgn();
        var surface = GetSurface(0);
        return GetSurface(Index).BltStretch(surface, default, primarySurfaceRgn);
    }

    // DisplayC.pas:652
    public override int FlipSurface(int Index)
    {
        var primarySurfaceRgn = GetPrimarySurfaceRgn();
        var surface = GetSurface(0);
        int Result = GetSurface(Index).BltStretch(surface, default, primarySurfaceRgn);
        if (Succeeded(Result))
        {
            Result = DisplayCUnit.DCERR_NOFLIP;
        }

        return Result;
    }

    // DisplayC.pas:660
    // { Bug kept from the original: the result of Resize is ignored, so Result stays 0. }
    public override int ResizeBackSurfaces(int AWidth, int AHeight)
    {
        /* TODO: Implement Controls.Color in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:675
    public override void SetPrimarySurfaceRgn(in TRect R)
    {
        FPrimarySurfaceRgn = R;
    }

    // DisplayC.pas:680
    // The original returns @FPrimarySurfaceRgn, which is never nil.
    public override TRect? GetPrimarySurfaceRgn() => FPrimarySurfaceRgn;

    // DisplayC.pas:685
    public override TSet<TDisplayCtrlStatusElement> GetStatus() => FStatus;

    // DisplayC.pas:690
    public override int GetHPalette() => 0;

    // DisplayC.pas:695
    public override bool WndProc(ref TMessage Message)
    {
        bool Result = true;
        if (Message.Msg == 133)
        {
            Result = false;
            Message.Result = 0;
        }

        if (Message.Msg == 20)
        {
            Result = false;
            Message.Result = 1;
        }

        return Result;
    }

    // DisplayC.pas:710
    public override TCustomDisplay GetDisplay() => FDisplay;

    // DisplayC.pas:720
    public override void WindowRecreated()
    {
    }

    // DisplayC.pas:734
    public override int Activate(TCustomDisplay ADisplay)
    {
        int Result;
        if (!FStatus.Contains(TDisplayCtrlStatusElement.dsActivated))
        {
            FDisplay = ADisplay;
            FStatus = FStatus + TSet<TDisplayCtrlStatusElement>.Of(TDisplayCtrlStatusElement.dsActivated);
            FDisplay.FCurrentFrameRate = FDisplay.FFrameRate;
            InvalidateAll();
            Result = 0;
        }
        else
        {
            Result = DisplayCUnit.DCERR_ACTIVATED;
        }

        return Result;
    }

    // DisplayC.pas:748
    public override int Deactivate()
    {
        int Result;
        if (FStatus.Contains(TDisplayCtrlStatusElement.dsActivated))
        {
            FStatus = FStatus - TSet<TDisplayCtrlStatusElement>.Of(TDisplayCtrlStatusElement.dsActivated);
            if (FDisplay != null)
            {
                FDisplay.DeactivateDC();
            }

            Result = 0;
            FDisplay = null;
        }
        else
        {
            Result = DisplayCUnit.DCERR_NOTACTIVATED;
        }

        return Result;
    }

    // DisplayC.pas:762
    public override int SupportDisplayMode(TCustomDisplay ADisplay) => DisplayCUnit.DCERR_NOTSUPPORTED;

    // DisplayC.pas:767
    public override int SetPalet(TGFXEPalette Palette) => DisplayCUnit.DCERR_NOTSUPPORTED;

    // DisplayC.pas:772
    public void RealizePalet()
    {
        if (FDisplay.FStatus.Contains(TDisplayStatusElement.dsFrameLoopActive))
        {
            FRealizePalet = true;
        }
        else
        {
            RealizePaletNow();
        }
    }

    // DisplayC.pas:780
    public override int RealizePaletNow() => DisplayCUnit.DCERR_NOTSUPPORTED;

    // DisplayC.pas:787
    // { The mouse position in the display, or -1, -1 when the mouse is outside or another window has
    //   captured it. }
    public override void GetMousePos(ref int X, ref int Y)
    {
        /* TODO: Implement Windows.GetCapture in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:830
    public override int Hide()
    {
        FStatus = FStatus - TSet<TDisplayCtrlStatusElement>.Of(TDisplayCtrlStatusElement.dsVisible);
        return 0;
    }

    // DisplayC.pas:836
    public override int Show()
    {
        FStatus = FStatus + TSet<TDisplayCtrlStatusElement>.Of(TDisplayCtrlStatusElement.dsVisible);
        return 0;
    }

    // DisplayC.pas:842
    public override TGFXEPalette GetPalette() => null;

    // DisplayC.pas:873
    public override void InitInvMatrix(byte AWidth, byte AHeight)
    {
        if (FInvMatrix != null)
        {
            FInvMatrix.Dispose();
        }

        FInvMatrix = new TMatrix(AWidth, AHeight);
    }

    // DisplayC.pas:880
    public override void InvalidateAll()
    {
        if (FInvMatrix != null)
        {
            for (int I = 0, last = this.FInvMatrix.Width - 1; I <= last; I++)
            {
                for (int J = 0, lastJ = this.FInvMatrix.Height - 1; J <= lastJ; J++)
                {
                    this.FInvMatrix.SetVal((byte)I, (byte)J, 1);
                }
            }
        }
    }

    // DisplayC.pas:894
    // { The size of the largest rectangle of marked blocks with its top left corner at X, Y, grown
    //   first downwards (rows of shrinking width) and then to the right (columns of shrinking
    //   height); BW and BH get its size in blocks, the result is its area. }
    public override int GetInvBlockSize(int X, int Y, TMatrix Matrix, ref int BW, ref int BH)
    {
        int Result;
        int Size1;
        int Size2;
        int W1;
        int H2;
        int W2;
        int H1;
        // P and Q are indexes into Matrix.FData.
        int P;
        int Q;
        int N;
        int L;
        int Off;
        bool Ok;
        P = X + Matrix.FWidth * Y;
        if (Matrix.FData[P] != 0)
        {
            H1 = 0;
            Q = P;
            W1 = Matrix.FWidth - X;
            Size1 = 0;
            do
            {
                N = W1;
                L = 0;
                while (N != 0 && Matrix.FData[Q + L] != 0)
                {
                    L += 1;
                    N -= 1;
                }

                Ok = L * H1 >= Size1;
                if (Ok)
                {
                    H1 += 1;
                    Q = Q + Matrix.FWidth;
                    W1 = L;
                    Size1 = H1 * L;
                }
            }
            while (!(!Ok || H1 + Y == Matrix.FHeight));
            BH = H1;
            BW = W1;
            W2 = 0;
            Q = P;
            H2 = Matrix.FHeight - Y;
            Size2 = 0;
            do
            {
                N = H2;
                L = 0;
                Off = 0;
                while (N != 0 && Matrix.FData[Q + Off] != 0)
                {
                    L += 1;
                    N -= 1;
                    Off += Matrix.FWidth;
                }

                Ok = L * W2 >= Size2;
                if (Ok)
                {
                    Q += 1;
                    W2 += 1;
                    H2 = L;
                    Size2 = W2 * H2;
                }
            }
            while (!(!Ok || W2 + X == Matrix.FWidth));
            if (Size2 > Size1)
            {
                BW = W2;
                BH = H2;
                Result = Size2;
            }
            else
            {
                Result = Size1;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // DisplayC.pas:964
    public override void GetPixPosofBlock(int BX, int BY, TRect R, ref int PX, ref int PY, ref int BW, ref int BH)
    {
        int W;
        int H;
        W = (R.Right - R.Left + FInvMatrix.FWidth - 1) / FInvMatrix.FWidth;
        H = (R.Bottom - R.Top + FInvMatrix.FHeight - 1) / FInvMatrix.FHeight;
        PX = W * BX;
        PY = H * BY;
        BW = W;
        BH = H;
    }

    // DisplayC.pas:976
    public override void GetBlockPosofPix(int PX, int PY, TRect R, ref int BX, ref int BY)
    {
        int W;
        int H;
        W = (R.Right - R.Left + 1 + FInvMatrix.FWidth - 1) / FInvMatrix.FWidth;
        H = (R.Bottom - R.Top + 1 + FInvMatrix.FHeight - 1) / FInvMatrix.FHeight;
        BX = (PX - R.Left) / W;
        BY = (PY - R.Top) / H;
    }

    // DisplayC.pas:988
    // { Marks the blocks of the pixel rectangle X, Y, W, H with Value. Bug kept from the original:
    //   the range test checks BY2 >= 0 where BX2 >= 0 was meant (and BY1 twice). }
    public void InvWndwithValue(int X, int Y, int W, int H, byte Value)
    {
        int BX1 = default;
        int BX2 = default;
        int BY1 = default;
        int BY2 = default;
        TMatrix M;
        TRect R = default;
        if (FInvMatrix != null)
        {
            M = FInvMatrix;
            R.Left = 0;
            R.Right = GetSurface(1).Width;
            R.Top = 0;
            R.Bottom = GetSurface(1).Height;
            GetBlockPosofPix(X, Y, R, ref BX1, ref BY1);
            GetBlockPosofPix(X + W, Y + H, R, ref BX2, ref BY2);
            if (M.FWidth > BX1 && M.FHeight > BY1 && BY2 >= 0 && BY1 >= 0)
            {
                if (BX1 < 0)
                {
                    BX1 = 0;
                }

                if (BY1 < 0)
                {
                    BY1 = 0;
                }

                if (M.FWidth <= BX2)
                {
                    BX2 = M.FWidth - 1;
                }

                if (M.FHeight <= BY2)
                {
                    BY2 = M.FHeight - 1;
                }

                X = BX1;
                int last = BX2;
                for (; X <= last; X += 1)
                {
                    Y = BY1;
                    int limit = BY2;
                    for (; Y <= limit; Y += 1)
                    {
                        M.SetVal((byte)X, (byte)Y, Value);
                    }
                }
            }
        }
    }

    // DisplayC.pas:1020
    public override void Invalidate(int X, int Y, int W, int H)
    {
        InvWndwithValue(X, Y, W, H, 1);
    }

    // DisplayC.pas:1043
    public int GetCurrentFrameRate() => FDisplay.FCurrentFrameRate;

    // DisplayC.pas:1049
    // { Counts frames; every 5 frames the frame rate is measured. }
    public override void NewFrame()
    {
        int T;
        if (FFrameCount >= 5)
        {
            T = GetElapsedMilliSeconds();
            FDisplay.FCurrentFrameRate = Round((double)5000 / (T - FLastFrameTime + 1));
            FFrameCount = 0;
            FLastFrameTime = T;
        }

        FFrameCount = (ushort)(FFrameCount + 1);
    }

    // DisplayC.pas:1063
    public override void ShowFrameRate()
    {
    }

}

// the full screen window of the DirectDraw controllers. passes the mouse and key events on to
// its display. FDisplay
public partial class TDDrawFSWinControl : TWinControl
{
    internal TCustomDisplay FDisplay;

    // DisplayC.pas:1129
    protected internal override void MouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FDisplay.MouseDown(Button, Shift, X, Y);
    }

    // DisplayC.pas:1134
    protected internal override void MouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        /* TODO: Implement Controls.MouseMove in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1139
    protected internal override void MouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        /* TODO: Implement Controls.MouseUp in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1159
    protected internal override void DblClick()
    {
        /* TODO: Implement Controls.DblClick in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1164
    protected internal override void Click()
    {
        /* TODO: Implement Controls.Click in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1144
    protected internal override void KeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        FDisplay.KeyDown(ref Key, Shift);
    }

    // DisplayC.pas:1149
    protected internal override void KeyUp(ref ushort Key, TSet<TShiftState> Shift)
    {
        /* TODO: Implement Controls.KeyUp in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1154
    protected internal override void KeyPress(ref byte Key)
    {
        /* TODO: Implement Controls.KeyPress in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1169
    protected internal override void CreateParams(ref TCreateParams Params)
    {
        /* TODO: Implement Controls.CreateParams in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1180
    protected internal override void WndProc(ref TMessage Message)
    {
        /* TODO: Implement Forms.Active in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1114

    public TDDrawFSWinControl(TComponent owner) : base(owner)
    {
        /* TODO: Implement Controls.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1121
    public override void Dispose()
    {
        /* TODO: Implement Controls.Handle in the platform or editor service. */
        throw new NotImplementedException();
    }
}

public enum TActivatedDCType : byte
{
    dtPrimaryDC = 0,
    dtSecondaryDC = 1,
    dtUserDC = 2
}

public enum TBackSurfaceControlMode : byte
{
    bmBltSurface = 0,
    bmBltInvalidatedSurface = 1,
    bmPageFlipping = 2,
    bmNone = 3
}

public delegate void TUpdateFrameEvent(object Sender, TAbstractDC DC, int SurfaceType);
public delegate void TDCNotifyEvent(object Sender, TAbstractDC DC);

public enum TBufferingScheme : byte
{
    bsSingle = 0,
    bsDouble = 1,
    bsTripple = 2
}

public delegate void TErrorEvent(string ErrorStr, int Error);
public delegate void TSurfaceDataLostEvent(TAbstractDC DC, int SurfaceType);

public enum TUpdateFrameMode : byte
{
    umAsynchrone = 0,
    umSynchrone = 1
}

public delegate void TResizeBackSurfaceEvent(TAbstractDC DC, int Width, int Height);
public delegate void TSwapDCEvent(TAbstractDC DC);

[LegacyObject]
// the asynchronous frame loop. calls TCustomDisplay.UpdateFrame through Synchronize at the
// frame rate. FDisplay, FBusy, FLastTime
public partial class TUpdateFrameThread
{
    internal TCustomDisplay FDisplay;
    internal bool FBusy;
    internal int FLastTime;

    // DisplayC.pas:1069
    internal void TriggerUpdateFrame()
    {
        /* TODO: Implement Classes.Terminated in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1078
    protected internal void Execute()
    {
        /* TODO: Implement Classes.Terminated in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1105
    public TUpdateFrameThread(TCustomDisplay ADisplay)
    {
        CaptureAddress.Assign(this);
        /* TODO: Implement Classes.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:358

}

// TCustomDisplay with its properties published
public partial class TDisplay : TCustomDisplay
{

    public TDisplay(TComponent owner) : base(owner)
    { }

}

// a TGDISurface on the window of a control, taking the handle from the control. FControl
public partial class TDisplayGDISurface : TGDISurface
{
    internal TWinControl FControl;

    // DisplayC.pas:1878
    public TDisplayGDISurface(TWinControl AControl) : base(0)
    {
        /* TODO: Implement Controls.Handle in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1887
    public override int GetHandle()
    {
        /* TODO: Implement Controls.Handle in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:367

}

// the windowed controllers (GDI). FDC, FWnd, FGDISurface (the window, surface 0)
public partial class TWindowsDC : TDisplayCtrl
{

    internal int FDC;
    internal int FWnd;
    internal TDisplayGDISurface FGDISurface;

    // DisplayC.pas:1894
    public TWindowsDC(TComponent owner) : base(owner)
    {
        InitInvMatrix(4, 4);
        FStatus = FStatus + TSet<TDisplayCtrlStatusElement>.Of(TDisplayCtrlStatusElement.dsWindowed);
    }

    // DisplayC.pas:1901
    public override int Activate(TCustomDisplay ADisplay)
    {
        /* TODO: Implement Windows.GetDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1920
    public override int Deactivate()
    {
        /* TODO: Implement Windows.ReleaseDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1932
    public override void WindowRecreated()
    {
        /* TODO: Implement Windows.GetDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1939
    public override int SupportDisplayMode(TCustomDisplay ADisplay) => 0;

    // DisplayC.pas:1944
    public override int BltSurface(int Index)
    {
        int Result;
        /* TODO: Implement cell or array backing for this address. */
        throw new NotImplementedException();
        return Result;
    }

    // DisplayC.pas:1949
    public override int FlipSurface(int Index)
    {
        int Result;
        /* TODO: Implement cell or array backing for this address. */
        throw new NotImplementedException();
        if (Result >= 0)
        {
            Result = DisplayCUnit.DCERR_NOFLIP;
        }

        return Result;
    }

    // DisplayC.pas:1957
    // { In a window every block is a block of its own. }
    public override int GetInvBlockSize(int X, int Y, TMatrix Matrix, ref int BW, ref int BH)
    {
        if (GetInvMatrix().GetVal((byte)X, (byte)Y) != 0)
        {
            BW = 1;
            BH = 1;
        }
        else
        {
            BW = 0;
            BH = 0;
        }

        return BW * BH;
    }

    // DisplayC.pas:1972
    public override void ShowFrameRate()
    {
        /* TODO: Implement Windows.TextOut in the platform or editor service. */
        throw new NotImplementedException();
    }

}

// 8 bit back surfaces drawn with StretchDIBits through a palette. FBack1, FBack2
// (TWindows08DIBSurface, declared in the implementation), FPalette, FBitmapInfo (header and 256
// colors), FHPalette, FOldPalette
public partial class TWindows08DC : TWindowsDC
{

    internal TSurface FBack1;
    internal TSurface FBack2;
    internal TGFXEPalette FPalette;
    internal TBitmapInfoHeader FBitmapInfo;
    internal FixedArray_Aow_Rtl_TRGBQuad_256 FColors;
    internal int FHPalette;
    internal int FOldPalette;

    // DisplayC.pas:2165
    public TWindows08DC(TComponent owner) : base(owner)
    {
        FPalette = new TGFXEPalette();
        FPalette.PixelFormatID = pfRGB24;
    }

    // DisplayC.pas:2172
    public override void Dispose()
    {
        base.Dispose();
    }

    // DisplayC.pas:2178
    public override TGFXEPalette GetPalette() => FPalette;

    // DisplayC.pas:2183
    public override int GetHPalette() => FHPalette;

    // DisplayC.pas:2188
    public override TSurface GetSurface(int Index)
    {
        TSurface Result;
        switch (Index)
        {
            case 0:
                Result = FGDISurface;
                break;
            case 1:
                Result = FBack1;
                break;
            case 2:
                Result = FBack2;
                break;
            default:
                Result = null;
                break;
        }

        return Result;
    }

    // DisplayC.pas:2199
    public override int Deactivate()
    {
        if (FStatus.Contains(TDisplayCtrlStatusElement.dsActivated))
        {
            FBack1?.Dispose();
            FBack1 = null;
            FBack2?.Dispose();
            FBack2 = null;
        }

        ReleaseHPalette();
        return base.Deactivate();
    }

    // DisplayC.pas:2212
    public override int Activate(TCustomDisplay ADisplay)
    {
        /* TODO: Implement Controls.Color in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:2254
    public void CreateHPalette()
    {
        /* TODO: Implement Windows.CreatePalette in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:2270
    public void ReleaseHPalette()
    {
        /* TODO: Implement Windows.SelectPalette in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:2281
    public override void WindowRecreated()
    {
        /* TODO: Implement Windows.DeleteObject in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:2290
    public void UpdatePalette()
    {
        /* TODO: Implement Windows.SetPaletteEntries in the platform or editor service. */
        throw new NotImplementedException();
    }

}

public enum TWin16DCPixelFormat : byte
{
    pfAuto = 0,
    pf555 = 1,
    pf565 = 2
}

// 16 bit DIB back surfaces. FPixelFormat, FBack1, FBack2
public partial class TWindows16DC : TWindowsDC
{

    internal TWin16DCPixelFormat FPixelFormat;
    internal TDIB16Surface FBack1;
    internal TDIB16Surface FBack2;

    // DisplayC.pas:2343
    internal void SetPixelFormat(TWin16DCPixelFormat Value)
    {
        if (!FStatus.Contains(TDisplayCtrlStatusElement.dsActivated))
        {
            FPixelFormat = Value;
        }
        else
        {
            ShowMessage("Cannot change PixelFormat when activated");
        }
    }

    // DisplayC.pas:2314
    public TWindows16DC(TComponent owner) : base(owner)
    {
        FPixelFormat = TWin16DCPixelFormat.pfAuto;
    }

    // DisplayC.pas:2320
    public override TSurface GetSurface(int Index)
    {
        TSurface Result;
        switch (Index)
        {
            case 0:
                Result = FGDISurface;
                break;
            case 1:
                Result = FBack1;
                break;
            case 2:
                Result = FBack2;
                break;
            default:
                Result = null;
                break;
        }

        return Result;
    }

    // DisplayC.pas:2331
    public override int Deactivate()
    {
        if (FStatus.Contains(TDisplayCtrlStatusElement.dsActivated))
        {
            FBack1?.Dispose();
            FBack1 = null;
            FBack2?.Dispose();
            FBack2 = null;
        }

        return base.Deactivate();
    }

    // DisplayC.pas:2351
    public override int Activate(TCustomDisplay ADisplay)
    {
        /* TODO: Implement Controls.Color in the platform or editor service. */
        throw new NotImplementedException();
    }

    [Published]
    public TWin16DCPixelFormat PixelFormat { get => FPixelFormat; set => SetPixelFormat(value); }

}

// 24 bit DIB back surfaces. FBack1, FBack2
public partial class TWindows24DC : TWindowsDC
{

    public TWindows24DC(TComponent owner) : base(owner)
    { }

    internal TDIB24Surface FBack1;
    internal TDIB24Surface FBack2;

    // DisplayC.pas:2390
    public override TSurface GetSurface(int Index)
    {
        TSurface Result;
        switch (Index)
        {
            case 0:
                Result = FGDISurface;
                break;
            case 1:
                Result = FBack1;
                break;
            case 2:
                Result = FBack2;
                break;
            default:
                Result = null;
                break;
        }

        return Result;
    }

    // DisplayC.pas:2401
    public override int Deactivate()
    {
        if (FStatus.Contains(TDisplayCtrlStatusElement.dsActivated))
        {
            FBack1?.Dispose();
            FBack1 = null;
            FBack2?.Dispose();
            FBack2 = null;
        }

        return base.Deactivate();
    }

    // DisplayC.pas:2413
    public override int Activate(TCustomDisplay ADisplay)
    {
        /* TODO: Implement Controls.Color in the platform or editor service. */
        throw new NotImplementedException();
    }
}

// 32 bit back surfaces. FBack1, FBack2
public partial class TWindows32DC : TWindowsDC
{

    public TWindows32DC(TComponent owner) : base(owner)
    { }

    internal TDIB32Surface FBack1;
    internal TDIB32Surface FBack2;

    // DisplayC.pas:2435
    public override TSurface GetSurface(int Index)
    {
        TSurface Result;
        switch (Index)
        {
            case 0:
                Result = FGDISurface;
                break;
            case 1:
                Result = FBack1;
                break;
            case 2:
                Result = FBack2;
                break;
            default:
                Result = null;
                break;
        }

        return Result;
    }

    // DisplayC.pas:2446
    public override int Deactivate()
    {
        if (FStatus.Contains(TDisplayCtrlStatusElement.dsActivated))
        {
            FBack1?.Dispose();
            FBack1 = null;
            FBack2?.Dispose();
            FBack2 = null;
        }

        return base.Deactivate();
    }

    // DisplayC.pas:2458
    public override int Activate(TCustomDisplay ADisplay)
    {
        /* TODO: Implement Controls.Color in the platform or editor service. */
        throw new NotImplementedException();
    }
}

// an 8 bit bottom-up DIB of a TWindows08DC, drawn with the bitmap info and palette of its
// owner. FWidth, FHeight, FMem, FBits (FMem aligned to 8), FPitch (negative), FOwner. The port's
// FMem is the pixel array and FBits, its first byte, has no field of its own (graphics.md 3.4).
public partial class TWindows08DIBSurface : TSurface
{
    internal int FWidth;
    internal int FHeight;
    internal byte[] FMem;
    internal int FPitch;
    internal TWindows08DC FOwner;

    // DisplayC.pas:2011
    public TWindows08DIBSurface(int AWidth, int AHeight, TWindows08DC owner)
    {
        FOwner = owner;
        Resize(AWidth, AHeight);
    }

    // DisplayC.pas:2018
    public override void Dispose()
    {
        FMem = null;
        base.Dispose();
    }

    // DisplayC.pas:2024
    public override int GetWidth() => FWidth;

    // DisplayC.pas:2029
    public override int GetHeight() => FHeight;

    // DisplayC.pas:2036
    // { Copies SrcRect to the top left of DestRect (no stretching); a GDI surface that cannot be
    //   locked is drawn on with BltDC. }
    public override int BltStretch(TSurface Dest, TRect? SrcRect, TRect? DestRect)
    {
        /* TODO: Implement Windows.GetDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:2078
    public override int BltDC(int DC, TRect? SrcRect, TRect? DestRect)
    {
        /* TODO: Implement Windows.GetClientRect in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:2104
    public override int Lock(ref TSurfaceDesc SD)
    {
        SD = default;
        SD.Width = FWidth;
        SD.Height = FHeight;
        SD.BitCount = 8;
        SD.PixelFormatID = pfPAL8;
        SD.ClipRect.Left = 0;
        SD.ClipRect.Right = FWidth;
        SD.ClipRect.Top = 0;
        SD.ClipRect.Bottom = FHeight;
        SD.Pitch = FPitch;
        SD.Palette = FOwner.FPalette;
        SD.Data = FMem;
        SD.DataIndex = Math.Abs(FHeight * FPitch) + FPitch;
        return 0;
    }

    // DisplayC.pas:2121
    public override int Unlock() => 0;

    // DisplayC.pas:2126
    public override int UpdatePalette()
    {
        FOwner.UpdatePalette();
        return 0;
    }

    // DisplayC.pas:2134
    // { Bug kept from the original: fills from the field FBits instead of the bits (with a negative
    //   count, so nothing happens). }
    public override int Cls(int Color)
    {
        FOwner.FPalette.GetNearestColor(Color);
        /* TODO: Implement the pointer byte image. */
        throw new NotImplementedException();
        return 0;
    }

    // DisplayC.pas:2143
    public override int Resize(int AWidth, int AHeight)
    {
        int Result;
        if (AWidth != FWidth || AHeight != FHeight)
        {
            FWidth = AWidth;
            FHeight = AHeight;
            FPitch = -((FWidth + 7) & ~7);
            ref var bitmapInfoHeader = ref FOwner.FBitmapInfo;
            bitmapInfoHeader.biWidth = Math.Abs(FPitch);
            bitmapInfoHeader.biHeight = FHeight;

            FMem = MemorySurfaces.Reallocate(FMem, Math.Abs(FPitch * FHeight));
            Result = 0;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

}

public static partial class DisplayCUnit
{
    // The DisplayC unit of Dcpack.dpl (the display layer of Age of Wonders: the display control
    // TDisplay and the display controllers ("DCs") that put the surfaces of GFXE on the screen,
    // in a window through GDI or full screen through DirectDraw, plus the frame update loop).
    // The original was D:\GEngine\GFXE\DisplayC\DisplayC.pas with inc\TDisplay.PAS included
    // (the Assert in TCustomDisplay.CreateWnd names it). TMatrix came from a unit DCTypes (its
    // error message says so) but is compiled into this unit. The port has no DirectDraw: the
    // DirectDraw controllers (TDDrawDC, TDDrawFullScrDC and its 16, 24 and 32 bit descendants)
    // and their surface TDDrawDCSurface are dropped, and TSdlDC is the one controller
    // (graphics.md 7.3).
    // { HResult codes of the display controllers }
    public const int DCERR_NOTSUPPORTED = -2147414015;
    public const int DCERR_ACTIVATEFAILED = -2147414014;
    public const int DCERR_FAILED = -2147414012;
    public const int DCERR_ACTIVATED = -2147414011;
    public const int DCERR_NOTACTIVATED = -2147414010;
    public const int DCERR_NOFLIP = -2147414007;
    // { Posted to the display: WParam 1 shows the active DC, 2 activates the primary DC }
    public const short WM_DISPLAYC = 27135;
}
