#nullable disable
using Aow.Game.Host;
using Aow.Rtl;
using static Aow.Game.Draw.CustomDisplay.DisplayOptions;
using static Aow.Game.Draw.DisplayStatuses;

namespace Aow.Game.Draw;

public partial class CustomDisplay
{
    public IDisplayHost Host { get; set; }
    public IDisplayEvents EventSink { get; set; }
    public Func<bool> Terminated { get; init; } = () => false;
    public ShiftStates MouseButtons { get; set; }
    public long PresentedFrames { get; private set; }
    public Action<long> FramePresented { get; set; }

    // The color that clears new back surfaces. Create sets it to black (clBlack).
    public int Color { get; init; }

    public void PresentInitialFrame()
    {
        // The initial paint presents the cleared surface before the first scene update.
        if (PresentedFrames != 0 || !Options.Contains(BlitOnPaint) || ActiveDisplayController is null)
        {
            return;
        }

        _ = ExecuteBlitSurfaceEvent();
        PresentedFrames++;
        FramePresented?.Invoke(PresentedFrames);
    }

    public CustomDisplay(Component owner) : base(owner)
    {
        Options = AutoResizeBackSurface | AutoSwapDisplayController | AutoStartUpdateFrameLoop | AutoActivatePrimaryDisplayController | RestoreSurfacesOnSwap | BlitOnPaint;
        _bufferingScheme = BufferingScheme.Double;
        BackSurfaceControlMode = BackSurfaceControlMode.BlitSurface;
        _backSurfaceWidth = 640;
        _backSurfaceHeight = 480;
        SurfaceIndex = 1;
        _updateFrameMode = UpdateFrameMode.Synchronous;
        SetFrameRate(30);
    }

    public override void Dispose()
    {
        StopUpdateFrameLoop();
        _ = DeactivateDisplayController();
        base.Dispose();
    }

    private int _cursor;

    internal void SetCursor(int value)
    {
        _cursor = value;
        Host?.ShowSystemCursor(value != -1);
    }

    internal int GetCursor() => _cursor;
    public override void Loaded() => base.Loaded();

    public virtual void Resize()
    {
        if (ActiveDisplayController is null)
        {
            return;
        }

        Surface screen = ActiveDisplayController.GetSurface(0);
        if (Options.Contains(AutoResizeBackSurface))
        {
            ResizeBackSurfaces(screen.Width, screen.Height);
        }

        OnResize?.Invoke(this);
    }

    public virtual void ShowError(string message) => LegacyApplication.ShowMessage(message);
    public virtual void StopUpdateFrameLoop() => Status &= ~FrameLoopActive;

    public virtual void PauseFrameUpdateLoop()
    {
        _pauseCount++;
        Status |= FrameLoopPaused;
    }

    public virtual void ResumeFrameUpdateLoop()
    {
        if (_pauseCount > 0)
        {
            _pauseCount--;
        }

        if (_pauseCount == 0)
        {
            Status &= ~FrameLoopPaused;
        }
    }

    public virtual void StartUpdateFrameLoop()
    {
        if (Status.Contains(FrameLoopActive))
        {
            return;
        }

        if (_updateFrameMode != UpdateFrameMode.Synchronous)
        {
            throw new NotSupportedException("Asynchronous display loop");
        }

        Status |= FrameLoopActive;
        try
        {
            while (Status.Contains(FrameLoopActive) && !Terminated())
            {
                if (!Status.Contains(FrameLoopPaused))
                {
                    UpdateFrame();
                }
                else
                {
                    RtlHost.Clock.WaitMilliseconds(1);
                }

                Host.PumpEvents(EventSink);
            }
        }
        finally
        {
            StopUpdateFrameLoop();
        }
    }

    public virtual int MouseButton => MouseButtons.Contains(ShiftStates.Left) ? 1 : MouseButtons.Contains(ShiftStates.Right) ? 2 : 0;

    public virtual void UpdateFrame()
    {
        if (_bufferingScheme == BufferingScheme.Triple)
        {
            SurfaceIndex = SurfaceIndex == 1 ? 2 : 1;
        }

        int x = -1, y = -1;
        GetMousePosition(ref x, ref y);
        if (_mouseAboveSurface && x == -1 && MouseButton == 0)
        {
            EventSink.MouseMove(default, -1, -1);
        }

        _mouseAboveSurface = x != -1;
        _updateFrame?.Invoke(this, ActiveDisplayController, SurfaceIndex);
        if (ActiveDisplayController is null)
        {
            return;
        }

        ActiveDisplayController.NewFrame();
        _ = ActiveDisplayController.WaitForVerticalBlank();
        _ = ExecuteBlitSurfaceEvent();
        PresentedFrames++;
        FramePresented?.Invoke(PresentedFrames);
    }

    public DisplayControllerError ActivateDisplayController(DisplayController displayController)
    {
        if (displayController == ActiveDisplayController)
        {
            return DisplayControllerError.Ok;
        }

        DisplayController previous = ActiveDisplayController;
        DataSurface[] saved = new DataSurface[3];
        PauseFrameUpdateLoop();
        try
        {
            if (previous is not null && Options.Contains(RestoreSurfacesOnSwap))
            {
                for (int i = 1; i <= (int)_bufferingScheme; i++)
                {
                    saved[i] = new DataSurface(0, 0, pixelFormatId: PixelFormat.None, palette: null);
                    _ = saved[i].CaptureSurface(previous.GetSurface(i), R: default);
                }
            }

            _ = DeactivateDisplayController();
            if (displayController is null)
            {
                return DisplayControllerError.Ok;
            }

            DisplayControllerError result = displayController.Activate(this);
            if (result != DisplayControllerError.Ok)
            {
                if (previous is not null && previous.Activate(this).IsSuccess())
                {
                    ActiveDisplayController = previous;
                }

                result = DisplayControllerError.ActivateFailed;
            }
            else
            {
                ActiveDisplayController = displayController;
            }

            if (ActiveDisplayController is null)
            {
                return result;
            }

            Resize();
            for (int i = 1; i <= (int)_bufferingScheme; i++)
            {
                if (saved[i] is not null)
                {
                    _ = RemapSurface(saved[i], destination: ActiveDisplayController.GetSurface(i));
                }
            }

            OnActivateDisplayController?.Invoke(this, ActiveDisplayController);
            return result;
        }
        finally
        {
            foreach (DataSurface surface in saved)
            {
                surface?.Dispose();
            }

            ResumeFrameUpdateLoop();
        }
    }

    public DisplayControllerError SwapDisplayController()
    {
        PauseFrameUpdateLoop();
        try
        {
            if (Host is null)
            {
                return DisplayControllerError.NotActivated;
            }

            Host.Fullscreen = !Host.Fullscreen;
            _swapDisplayController?.Invoke(ActiveDisplayController);
            Resize();
            return DisplayControllerError.Ok;
        }
        finally
        {
            ResumeFrameUpdateLoop();
        }
    }
}
