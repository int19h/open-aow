#nullable disable
using Aow.Game.Host;
using Aow.Rtl;
using static Aow.Game.Draw.CustomDisplay.OptionsElement;
using static Aow.Game.Draw.DisplayStatusElement;

namespace Aow.Game.Draw;

public partial class CustomDisplay
{
    public IDisplayHost Host { get; set; }
    public IDisplayEvents EventSink { get; set; }
    public Func<bool> Terminated { get; set; } = () => false;
    public Set<ShiftState> MouseButtons { get; set; }
    public long PresentedFrames { get; private set; }
    public Action<long> FramePresented { get; set; }

    public void PresentInitialFrame()
    {
        // The initial paint presents the cleared surface before the first scene update.
        if (PresentedFrames != 0 || !_options.Contains(BlitOnPaint) || _activeDisplayController is null) return;
        ExecuteBlitSurfaceEvent();
        PresentedFrames++;
        FramePresented?.Invoke(PresentedFrames);
    }

    public CustomDisplay(Component owner) : base(owner)
    {
        Width = Height = 100;
        _options = Set<OptionsElement>.Of(AutoResizeBackSurface, AutoSwapDisplayController,
            AutoStartUpdateFrameLoop, AutoActivatePrimaryDisplayController, RestoreSurfacesOnSwap, BlitOnPaint);
        _bufferingScheme = BufferingScheme.Double;
        _backSurfaceControlMode = BackSurfaceControlMode.BlitSurface;
        _backSurfaceWidth = 640;
        _backSurfaceHeight = 480;
        SurfaceIndex = 1;
        _updateFrameMode = UpdateFrameMode.Synchronous;
        SetFrameRate(30);
    }

    public override void Dispose()
    {
        StopUpdateFrameLoop();
        DeactivateDisplayController();
        base.Dispose();
    }

    internal new void SetCursor(int value) { base.SetCursor(value); Host?.ShowSystemCursor(value != -1); }

    internal int GetCursor() => base.Cursor;
    public override void Loaded() => base.Loaded();

    public virtual void Resize()
    {
        if (_activeDisplayController is null) return;
        var screen = _activeDisplayController.GetSurface(0);
        if (_options.Contains(AutoResizeBackSurface)) ResizeBackSurfaces(screen.Width, screen.Height);
        _onResize?.Invoke(this);
    }

    public virtual void ShowError(string message) => RtlUnit.ShowMessage(message);
    public virtual void StopUpdateFrameLoop() => Status = Status.Without(FrameLoopActive);

    public virtual void PauseFrameUpdateLoop() { _pauseCount++; Status = Status.With(FrameLoopPaused); }

    public virtual void ResumeFrameUpdateLoop()
    {
        if (_pauseCount > 0) _pauseCount--;
        if (_pauseCount == 0) Status = Status.Without(FrameLoopPaused);
    }

    public virtual void StartUpdateFrameLoop()
    {
        if (Status.Contains(FrameLoopActive)) return;
        if (_updateFrameMode != UpdateFrameMode.Synchronous) throw new NotSupportedException("Asynchronous display loop");
        Status = Status.With(FrameLoopActive);
        try
        {
            while (Status.Contains(FrameLoopActive) && !Terminated())
            {
                if (!Status.Contains(FrameLoopPaused)) UpdateFrame();
                else RtlUnit.Clock.WaitMilliseconds(1);
                Host.PumpEvents(EventSink);
            }
        }
        finally { StopUpdateFrameLoop(); }
    }

    public virtual int GetMouseButton() => MouseButtons.Contains(ShiftState.Left) ? 1 : MouseButtons.Contains(ShiftState.Right) ? 2 : 0;

    public virtual void UpdateFrame()
    {
        if (_bufferingScheme == BufferingScheme.Triple) SurfaceIndex = SurfaceIndex == 1 ? 2 : 1;
        int x = -1, y = -1;
        GetMousePosition(ref x, ref y);
        if (_mouseAboveSurface && x == -1 && GetMouseButton() == 0) EventSink.MouseMove(default, -1, -1);
        _mouseAboveSurface = x != -1;
        _onUpdateFrame?.Invoke(this, _activeDisplayController, SurfaceIndex);
        if (_activeDisplayController is null) return;
        _activeDisplayController.NewFrame();
        _activeDisplayController.WaitForVerticalBlank();
        ExecuteBlitSurfaceEvent();
        PresentedFrames++;
        FramePresented?.Invoke(PresentedFrames);
    }

    public int ActivateDisplayController(CustomDisplayController displayController)
    {
        if (displayController == _activeDisplayController) return 0;
        var previous = _activeDisplayController;
        var saved = new DataSurface[3];
        PauseFrameUpdateLoop();
        try
        {
            if (previous is not null && _options.Contains(RestoreSurfacesOnSwap))
                for (int i = 1; i <= (int)_bufferingScheme; i++)
                {
                    saved[i] = new DataSurface(0, 0, 0, null);
                    saved[i].CaptureSurface(previous.GetSurface(i), default);
                }
            DeactivateDisplayController();
            if (displayController is null) return 0;
            int result = displayController.Activate(this);
            if (result < 0)
            {
                if (previous is not null && previous.Activate(this) >= 0) _activeDisplayController = previous;
                result = DCERR_ACTIVATEFAILED;
            }
            else _activeDisplayController = displayController;
            if (_activeDisplayController is null) return result;
            Resize();
            for (int i = 1; i <= (int)_bufferingScheme; i++)
                if (saved[i] is not null) RemapSurface(saved[i], _activeDisplayController.GetSurface(i));
            _onActivateDisplayController?.Invoke(this, _activeDisplayController);
            return result;
        }
        finally
        {
            foreach (var surface in saved) surface?.Dispose();
            ResumeFrameUpdateLoop();
        }
    }

    public int SwapDisplayController()
    {
        PauseFrameUpdateLoop();
        try
        {
            if (Host is null) return DisplayCodes.DCERR_NOTACTIVATED;
            Host.Fullscreen = !Host.Fullscreen;
            _onSwapDisplayController?.Invoke(_activeDisplayController);
            Resize();
            return 0;
        }
        finally { ResumeFrameUpdateLoop(); }
    }
}
