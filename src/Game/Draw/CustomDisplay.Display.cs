#nullable disable
using Aow.Game.Host;
using Aow.Rtl;
using static Aow.Game.Draw.CustomDisplay.DisplayOptionsElement;
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
        if (PresentedFrames != 0 || !FOptions.Contains(BltOnWMPAINT) || FActiveDC is null) return;
        ExecuteBltSurfaceEvent();
        PresentedFrames++;
        FramePresented?.Invoke(PresentedFrames);
    }

    public CustomDisplay(Component owner) : base(owner)
    {
        Width = Height = 100;
        FOptions = Set<DisplayOptionsElement>.Of(AutoResizeBackSurface, AutoSwapDC,
            AutoStartUpdateFrameLoop, AutoActivatePrimaryDC, RestoreSurfacesOnSwap, BltOnWMPAINT);
        FBufferingScheme = BufferingScheme.Double;
        FBackSurfaceControlMode = BackSurfaceControlMode.BltSurface;
        FBackSurfaceWidth = 640;
        FBackSurfaceHeight = 480;
        FSurfaceIndex = 1;
        FUpdateFrameMode = UpdateFrameMode.Synchrone;
        SetFrameRate(30);
    }

    public override void Dispose()
    {
        StopUpdateFrameLoop();
        DeactivateDC();
        base.Dispose();
    }

    internal new void SetCursor(int value) { base.SetCursor(value); Host?.ShowSystemCursor(value != -1); }

    internal int GetCursor() => base.Cursor;
    public override void Loaded() => base.Loaded();

    public virtual void Resize()
    {
        if (FActiveDC is null) return;
        var screen = FActiveDC.GetSurface(0);
        if (FOptions.Contains(AutoResizeBackSurface)) ResizeBackSurfaces(screen.Width, screen.Height);
        FOnResize?.Invoke(this);
    }

    public virtual void ShowError(string message) => RtlUnit.ShowMessage(message);
    public virtual void StopUpdateFrameLoop() => FStatus = FStatus.Without(FrameLoopActive);

    public virtual void PauseFrameUpdateLoop() { FPauseCount++; FStatus = FStatus.With(FrameLoopPaused); }

    public virtual void ResumeFrameUpdateLoop()
    {
        if (FPauseCount > 0) FPauseCount--;
        if (FPauseCount == 0) FStatus = FStatus.Without(FrameLoopPaused);
    }

    public virtual void StartUpdateFrameLoop()
    {
        if (FStatus.Contains(FrameLoopActive)) return;
        if (FUpdateFrameMode != UpdateFrameMode.Synchrone) throw new NotSupportedException("Asynchronous display loop");
        FStatus = FStatus.With(FrameLoopActive);
        try
        {
            while (FStatus.Contains(FrameLoopActive) && !Terminated())
            {
                if (!FStatus.Contains(FrameLoopPaused)) UpdateFrame();
                else RtlUnit.Clock.WaitMilliseconds(1);
                Host.PumpEvents(EventSink);
            }
        }
        finally { StopUpdateFrameLoop(); }
    }

    public virtual int GetMouseButton() => MouseButtons.Contains(ShiftState.Left) ? 1 : MouseButtons.Contains(ShiftState.Right) ? 2 : 0;

    public virtual void UpdateFrame()
    {
        if (FBufferingScheme == BufferingScheme.Tripple) FSurfaceIndex = FSurfaceIndex == 1 ? 2 : 1;
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

    public int ActivateDC(AbstractDC dc)
    {
        if (dc == FActiveDC) return 0;
        var previous = FActiveDC;
        var saved = new DataSurface[3];
        PauseFrameUpdateLoop();
        try
        {
            if (previous is not null && FOptions.Contains(RestoreSurfacesOnSwap))
                for (int i = 1; i <= (int)FBufferingScheme; i++)
                {
                    saved[i] = new DataSurface(0, 0, 0, null);
                    saved[i].CaptureSurface(previous.GetSurface(i), default);
                }
            DeactivateDC();
            if (dc is null) return 0;
            int result = dc.Activate(this);
            if (result < 0)
            {
                if (previous is not null && previous.Activate(this) >= 0) FActiveDC = previous;
                result = DCERR_ACTIVATEFAILED;
            }
            else FActiveDC = dc;
            if (FActiveDC is null) return result;
            Resize();
            for (int i = 1; i <= (int)FBufferingScheme; i++)
                if (saved[i] is not null) RemapSurface(saved[i], FActiveDC.GetSurface(i));
            FOnActivateDC?.Invoke(this, FActiveDC);
            return result;
        }
        finally
        {
            foreach (var surface in saved) surface?.Dispose();
            ResumeFrameUpdateLoop();
        }
    }

    public int SwapDC()
    {
        PauseFrameUpdateLoop();
        try
        {
            if (Host is null) return DisplayCodes.DCERR_NOTACTIVATED;
            Host.Fullscreen = !Host.Fullscreen;
            FOnSwapDC?.Invoke(FActiveDC);
            Resize();
            return 0;
        }
        finally { ResumeFrameUpdateLoop(); }
    }
}
