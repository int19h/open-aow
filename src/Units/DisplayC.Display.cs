#nullable disable
using Aow.Platform;
using Aow.Rtl;
using Aow.Units.GFXE;
using static Aow.Units.DisplayC.TDisplayOptionsElement;
using static Aow.Units.DisplayC.TDisplayStatusElement;

namespace Aow.Units.DisplayC;

public partial class TCustomDisplay
{
    public IDisplayHost Host { get; set; }
    public IDisplayEvents EventSink { get; set; }
    public Func<bool> Terminated { get; set; } = () => false;
    public TSet<TShiftState> MouseButtons { get; set; }
    public long PresentedFrames { get; private set; }
    public Action<long> FramePresented { get; set; }

    public void PresentInitialFrame()
    {
        // The initial paint presents the cleared surface before the first scene update.
        if (PresentedFrames != 0 || !FOptions.Contains(doBltOnWMPAINT) || FActiveDC is null) return;
        ExecuteBltSurfaceEvent();
        PresentedFrames++;
        FramePresented?.Invoke(PresentedFrames);
    }

    public TCustomDisplay(TComponent owner) : base(owner)
    {
        Width = Height = 100;
        FOptions = TSet<TDisplayOptionsElement>.Of(doAutoResizeBackSurface, doAutoSwapDC,
            doAutoStartUpdateFrameLoop, doAutoActivatePrimaryDC, doRestoreSurfacesOnSwap, doBltOnWMPAINT);
        FBufferingScheme = TBufferingScheme.bsDouble;
        FBackSurfaceControlMode = TBackSurfaceControlMode.bmBltSurface;
        FBackSurfaceWidth = 640;
        FBackSurfaceHeight = 480;
        FSurfaceIndex = 1;
        FUpdateFrameMode = TUpdateFrameMode.umSynchrone;
        SetFrameRate(30);
    }

    public override void Destroy()
    {
        StopUpdateFrameLoop();
        DeactivateDC();
        base.Destroy();
    }

    internal new void SetCursor(int value) { base.SetCursor(value); Host?.ShowSystemCursor(value != -1); }

    internal int GetCursor() => base.Cursor;
    public override void Loaded() => base.Loaded();

    public virtual void Resize()
    {
        if (FActiveDC is null) return;
        var screen = FActiveDC.GetSurface(0);
        if (FOptions.Contains(doAutoResizeBackSurface)) ResizeBackSurfaces(screen.Width, screen.Height);
        FOnResize?.Invoke(this);
    }

    public virtual void ShowError(string message) => RtlUnit.ShowMessage(message);
    public virtual void StopUpdateFrameLoop() => FStatus = FStatus.Without(dsFrameLoopActive);

    public virtual void PauseFrameUpdateLoop() { FPauseCount++; FStatus = FStatus.With(dsFrameLoopPaused); }

    public virtual void ResumeFrameUpdateLoop()
    {
        if (FPauseCount > 0) FPauseCount--;
        if (FPauseCount == 0) FStatus = FStatus.Without(dsFrameLoopPaused);
    }

    public virtual void StartUpdateFrameLoop()
    {
        if (FStatus.Contains(dsFrameLoopActive)) return;
        if (FUpdateFrameMode != TUpdateFrameMode.umSynchrone) throw new ENotSupported("Asynchronous display loop");
        FStatus = FStatus.With(dsFrameLoopActive);
        try
        {
            while (FStatus.Contains(dsFrameLoopActive) && !Terminated())
            {
                if (!FStatus.Contains(dsFrameLoopPaused)) UpdateFrame();
                else RtlUnit.Clock.WaitMilliseconds(1);
                Host.PumpEvents(EventSink);
            }
        }
        finally { StopUpdateFrameLoop(); }
    }

    public virtual int GetMouseButton() => MouseButtons.Contains(TShiftState.ssLeft) ? 1 : MouseButtons.Contains(TShiftState.ssRight) ? 2 : 0;

    public virtual void UpdateFrame()
    {
        if (FBufferingScheme == TBufferingScheme.bsTripple) FSurfaceIndex = FSurfaceIndex == 1 ? 2 : 1;
        int x = -1, y = -1;
        GetMousePos(ref x, ref y);
        if (FMouseAboveSurface && x == -1 && GetMouseButton() == 0) EventSink.MouseMove(default, -1, -1);
        FMouseAboveSurface = x != -1;
        FOnUpdateFrame?.Invoke(this, FActiveDC, FSurfaceIndex);
        if (FActiveDC is null) return;
        FActiveDC.NewFrame();
        FActiveDC.WaitForVerticalBlank();
        ExecuteBltSurfaceEvent();
        PresentedFrames++;
        FramePresented?.Invoke(PresentedFrames);
    }

    public int ActivateDC(TAbstractDC dc)
    {
        if (dc == FActiveDC) return 0;
        var previous = FActiveDC;
        var saved = new TDataSurface[3];
        PauseFrameUpdateLoop();
        try
        {
            if (previous is not null && FOptions.Contains(doRestoreSurfacesOnSwap))
                for (int i = 1; i <= (int)FBufferingScheme; i++)
                {
                    saved[i] = new TDataSurface(0, 0, 0, null);
                    saved[i].CaptureSurface(previous.GetSurface(i), default);
                }
            DeactivateDC();
            if (dc is null) return 0;
            int result = dc.Activate(this);
            if (result < 0)
            {
                if (previous is not null && previous.Activate(this) >= 0) FActiveDC = previous;
                result = DisplayCUnit.DCERR_ACTIVATEFAILED;
            }
            else FActiveDC = dc;
            if (FActiveDC is null) return result;
            Resize();
            for (int i = 1; i <= (int)FBufferingScheme; i++)
                if (saved[i] is not null) GFXEUnit.RemapSurface(saved[i], FActiveDC.GetSurface(i));
            FOnActivateDC?.Invoke(this, FActiveDC);
            return result;
        }
        finally
        {
            foreach (var surface in saved) surface?.Free();
            ResumeFrameUpdateLoop();
        }
    }

    public int SwapDC()
    {
        PauseFrameUpdateLoop();
        try
        {
            if (Host is null) return DisplayCUnit.DCERR_NOTACTIVATED;
            Host.Fullscreen = !Host.Fullscreen;
            FOnSwapDC?.Invoke(FActiveDC);
            Resize();
            return 0;
        }
        finally { ResumeFrameUpdateLoop(); }
    }
}

public partial class TDisplayCtrl
{
    private double lastVerticalBlank;

    public override int WaitForVerticalBlank()
    {
        var clock = RtlUnit.Clock;
        double target = lastVerticalBlank + FDisplay.FFrameTime;
        clock.WaitUntil(target);
        lastVerticalBlank = clock.ElapsedMilliseconds;
        FLastVBTime = (int)lastVerticalBlank;
        return 0;
    }

    public override int Handle() => 0;
    public override TWinControl OutputWnd() => FDisplay;
    public override void ShowError(string message) => RtlUnit.ShowMessage(message);
}
