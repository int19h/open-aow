#nullable disable
// Translated from DisplayC.pas.
using Aow.Rtl;
using static Aow.Rtl.DelphiRounding;
using static Aow.Rtl.LegacyApplication;

namespace Aow.Game.Draw;

// the display control. FFSWinControl, the events (OnBltSurface, OnUpdateFrame,
// OnSurfaceDataLost, OnResize, OnError, OnActivateDisplayController, OnDeactivateDisplayController, OnHide, OnShow,
// OnResizeBackSurface, OnSwapDisplayController), _frameRate, FrameTime (ms per frame), _currentFrameRate,
// UpdateFrameThread, Status, _options (2 bytes), _updateFrameMode, _backSurfaceControlMode,
// FBufferingScheme, FBackSurfaceWidth, FBackSurfaceHeight, FSurfaceIndex (the back surface to
// show), _pauseCount, _mouseAboveSurface, _primaryDisplayController, _secondaryDisplayController, _activeDisplayController
public partial class CustomDisplay : Component
{
    protected internal event NotifyEvent<object> OnBlitSurface;
    protected internal event UpdateFrameEvent OnUpdateFrame;

    internal void SetUpdateFrameHandler(UpdateFrameEvent handler) => OnUpdateFrame = handler;

    protected internal event SurfaceDataLostEvent OnSurfaceDataLost;

    internal void SetSurfaceDataLostHandler(SurfaceDataLostEvent handler) => OnSurfaceDataLost = handler;

    protected internal event NotifyEvent<object> OnResize;
    protected internal event DCNotifyEvent OnActivateDisplayController;
    protected internal event DCNotifyEvent OnDeactivateDisplayController;
    protected internal event NotifyEvent<object> OnHide;
    protected internal event NotifyEvent<object> OnShow;
    protected internal event ResizeBackSurfaceEvent OnResizeBackSurface;

    internal void SetResizeBackSurfaceHandler(ResizeBackSurfaceEvent handler) => OnResizeBackSurface = handler;

    protected internal event SwapDCEvent OnSwapDisplayController;

    internal void SetSwapDisplayControllerHandler(SwapDCEvent handler) => OnSwapDisplayController = handler;

    private int _frameRate;

    internal int FrameTime { get; private set; }

    public int CurrentFrameRate { get; internal set; }
    public Set<DisplayStatusElement> Status { get; private set; }

    private UpdateFrameMode _updateFrameMode;
    private BufferingScheme _bufferingScheme;
    private int _backSurfaceWidth;
    private int _backSurfaceHeight;

    public int SurfaceIndex { get; private set; }

    // direct frame blit
    private int _pauseCount;
    private bool _mouseAboveSurface;

    // DisplayC.pas:1397
    internal void SetBufferingScheme(BufferingScheme value)
    {
        if (ActiveDisplayController is null)
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
            default:
                break;
        }
    }

    // DisplayC.pas:1410
    internal void SetUpdateFrameMode(UpdateFrameMode value)
    {
        if (!Status.Contains(DisplayStatusElement.FrameLoopActive))
        {
            _updateFrameMode = value;
            if (!ComponentState.Contains(Rtl.ComponentState.Designing) && Options.Contains(OptionsElement.AutoStartUpdateFrameLoop) && !ComponentState.Contains(Rtl.ComponentState.Loading))
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

    // DisplayC.pas: How the display works in the port.
    // The display has no surfaces of its own. It shows the surfaces of its active display
    // controller (ActiveDisplayController), which is PrimaryDisplayController or
    // SecondaryDisplayController. SwapDisplayController toggles Host.Fullscreen through IDisplayHost,
    // raises OnSwapDisplayController and resizes the back surfaces. A controller owns surface 0
    // (the output surface) and, with double or triple buffering (BufferingScheme), back surfaces 1
    // and 2 that the game draws on. SurfaceIndex is the back surface to show next. With triple
    // buffering UpdateFrame alternates it between 1 and 2.
    // - Construction sets Double buffering, BlitSurface mode, 640 x 480 back surfaces, 30 fps and
    //   Synchronous update mode.
    // - ActivateDisplayController pauses the frame loop, optionally captures and remaps existing
    //   back surfaces when Options contains RestoreSurfacesOnSwap, deactivates the previous
    //   controller, activates the new one (falling back to the previous on failure), resizes and
    //   raises OnActivateDisplayController.
    // - DeactivateDisplayController raises OnDeactivateDisplayController and deactivates the active
    //   controller. Dispose stops the frame loop and deactivates the controller.
    // - StartUpdateFrameLoop runs a synchronous loop calling UpdateFrame and Host.PumpEvents until
    //   StopUpdateFrameLoop or Terminated returns true. UpdateFrameMode.Asynchronous throws
    //   NotSupportedException. PauseFrameUpdateLoop and ResumeFrameUpdateLoop nest via _pauseCount.
    // - UpdateFrame alternates the triple buffer, emits MouseMove(-1, -1) when the mouse left the
    //   output with no button down, raises OnUpdateFrame, calls NewFrame and WaitForVerticalBlank,
    //   presents via ExecuteBlitSurfaceEvent and increments PresentedFrames.
    // - ExecuteBlitSurfaceEvent raises OnBlitSurface, copies or flips the back surface according to
    //   BackSurfaceControlMode and optionally calls ShowFrameRate.
    // - Status: FrameLoopActive and FrameLoopPaused track the frame loop. DirectInputEnabled,
    //   Asynchronous and ExclusiveAccess are unused.
    // - Host windowing, events, palette and focus belong to IDisplayHost and IDisplayEvents.

    // DisplayC.pas:1541
    public virtual DisplayController GetDisplayController(int index)
    {
        return index switch
        {
            -1 => ActiveDisplayController,
            0 => PrimaryDisplayController,
            1 => SecondaryDisplayController,
            _ => null,
        };
    }

    // DisplayC.pas:1361
    public virtual void SetFrameRate(int value)
    {
        _frameRate = value;
        FrameTime = _frameRate != 0 ? Round((double)1000 / _frameRate) : 0;
    }

    // DisplayC.pas:1492
    public virtual void ResizeBackSurfaces(int width, int height)
    {
        _backSurfaceWidth = width;
        _backSurfaceHeight = height;
        if (!ComponentState.Contains(Rtl.ComponentState.Designing) && ActiveDisplayController is not null && _bufferingScheme != BufferingScheme.Single)
        {
            if (ActiveDisplayController.GetSurface(1).Width != width || ActiveDisplayController.GetSurface(1).Height != height)
            {
                _ = ActiveDisplayController.ResizeBackSurfaces(_backSurfaceWidth, _backSurfaceHeight);
                for (int i = 1, last = (int)_bufferingScheme; i <= last; i++)
                {
                    SurfaceDataLost(i);
                }
            }
        }

        OnResizeBackSurface?.Invoke(ActiveDisplayController, width, height);
    }

    // DisplayC.pas:1486
    public virtual void SurfaceDataLost(int surfaceType) => OnSurfaceDataLost?.Invoke(ActiveDisplayController, surfaceType);

    // DisplayC.pas:1469
    public virtual void GetMousePosition(ref int X, ref int Y) => ActiveDisplayController?.GetMousePosition(ref X, ref Y);

    // DisplayC.pas:1630
    public virtual DisplayControllerError ExecuteBlitSurfaceEvent()
    {
        DisplayControllerError result;
        try
        {
            result = DisplayControllerError.Ok;
            if (ActiveDisplayController is not null && ActiveDisplayController.GetStatus().Contains(DisplayControllerStatusElement.Visible))
            {
                OnBlitSurface?.Invoke(this);

                if (_bufferingScheme != BufferingScheme.Single)
                {
                    switch (BackSurfaceControlMode)
                    {
                        case BackSurfaceControlMode.PageFlipping:
                            result = ActiveDisplayController.FlipSurface(SurfaceIndex);
                            break;
                        case BackSurfaceControlMode.BlitSurface:
                            result = ActiveDisplayController.BlitSurface(SurfaceIndex);
                            break;
                        case BackSurfaceControlMode.BlitInvalidatedSurface:
                            result = ActiveDisplayController.BlitInvalidatedSurface(SurfaceIndex);
                            break;
                        case BackSurfaceControlMode.None:
                        default:
                            break;
                    }
                }

                if (Options.Contains(OptionsElement.ShowFrameRate))
                {
                    ActiveDisplayController.ShowFrameRate();
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

    // DisplayC.pas:1653
    public void Hide()
    {
        OnHide?.Invoke(this);

        if (ActiveDisplayController is not null && !ComponentState.Contains(Rtl.ComponentState.Designing))
        {
            _ = ActiveDisplayController.Hide();
        }
    }

    // DisplayC.pas:1661
    public void Show()
    {
        if (ActiveDisplayController is not null && !ComponentState.Contains(Rtl.ComponentState.Designing))
        {
            _ = ActiveDisplayController.Show();
        }

        OnShow?.Invoke(this);
    }

    // DisplayC.pas:1669
    public DisplayControllerError SupportDisplayController(DisplayController displayController) => displayController is not null ? displayController.SupportDisplayMode(this) : DisplayControllerError.NotSupported;

    // DisplayC.pas:1755
    public DisplayControllerError DeactivateDisplayController()
    {
        if (ActiveDisplayController is not null && ActiveDisplayController.GetStatus().Contains(DisplayControllerStatusElement.Activated))
        {
            OnDeactivateDisplayController?.Invoke(this, DC: ActiveDisplayController);

            _ = ActiveDisplayController.Deactivate();
            ActiveDisplayController = null;
        }

        return DisplayControllerError.Ok;
    }

    public int Cursor { get => GetCursor(); set => SetCursor(value); }
    public DisplayController ActiveDisplayController { get; private set; }
    protected internal Set<OptionsElement> Options { get; }
    protected internal BufferingScheme BufferingScheme { get => _bufferingScheme; set => SetBufferingScheme(value); }
    protected internal BackSurfaceControlMode BackSurfaceControlMode { get; }
    protected internal int BackSurfaceWidth { get => _backSurfaceWidth; set => SetBackSurfaceWidth(value); }
    protected internal int BackSurfaceHeight { get => _backSurfaceHeight; set => SetBackSurfaceHeight(value); }
    protected internal int FrameRate { get => _frameRate; set => SetFrameRate(value); }
    protected internal DisplayController PrimaryDisplayController { get; set; }
    protected internal DisplayController SecondaryDisplayController { get; set; }
    protected internal UpdateFrameMode UpdateFrameMode { get => _updateFrameMode; set => SetUpdateFrameMode(value); }

    private static GraphicsResult RemapSurface(Surface source, Surface destination)
    {
        LockedSurface lockedSurface = default;
        LockedSurface lockedSurface2 = default;
        _ = source.Lock(ref lockedSurface);
        _ = destination.Lock(ref lockedSurface2);
        GraphicsResult result = LockedSurface.RemapSurfacePart(in lockedSurface, 0, 0, lockedSurface.Width, lockedSurface.Height, destination: in lockedSurface2, 0, 0);
        _ = source.Unlock();
        _ = destination.Unlock();
        return result;
    }

    public enum OptionsElement : byte
    {
        AutoResizeBackSurface = 0, AutoSwapDisplayController = 1, AutoStartUpdateFrameLoop = 2, AutoActivatePrimaryDisplayController = 3, ShowFrameRate = 4, ShowErrors = 5, FocusAble = 6, RestoreSurfacesOnSwap = 7, BlitOnPaint = 8, AutoEnableDDraw = 9
    }

    public delegate void UpdateFrameEvent(object Sender, DisplayController DC, int SurfaceType);
    public delegate void DCNotifyEvent(object Sender, DisplayController DC);
    public delegate void SurfaceDataLostEvent(DisplayController DC, int SurfaceType);

    public delegate void ResizeBackSurfaceEvent(DisplayController DC, int Width, int Height);
    public delegate void SwapDCEvent(DisplayController DC);

}
