using Aow.Rtl.Host;
using static SDL.SDL3;

namespace Aow.Platform;

public sealed class SdlClock : IClock
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
    public uint TickCount => unchecked((uint)(SDL_GetTicksNS() / 1_000_000));
    public double ElapsedMilliseconds => SDL_GetTicksNS() / 1_000_000.0;

    public void WaitUntil(double deadline)
    {
        double remaining = deadline - ElapsedMilliseconds;
        if (remaining > 2) SDL_DelayPrecise((ulong)((remaining - 2) * 1_000_000));
        while (ElapsedMilliseconds < deadline) Thread.SpinWait(16);
    }

    public void WaitMilliseconds(int milliseconds)
    {
        if (milliseconds < 0) throw new ArgumentOutOfRangeException(nameof(milliseconds));
        if (milliseconds == 0) { Thread.Yield(); return; }
        ulong end = SDL_GetTicksNS() + (ulong)milliseconds * 1_000_000;
        if (milliseconds == 1) { SDL_DelayPrecise(1_000_000); return; }
        if (milliseconds > 2) SDL_DelayPrecise((ulong)(milliseconds - 2) * 1_000_000);
        while (SDL_GetTicksNS() < end) Thread.SpinWait(16);
    }
}

/// <summary>The deadline starts at the last completed frame. Slow frames never accumulate debt.</summary>
public sealed class FramePacer(IClock clock, int frameRate = 30)
{
    private readonly int frameTime = frameRate > 0 ? 1000 / frameRate : throw new ArgumentOutOfRangeException(nameof(frameRate));
    private double last = clock.ElapsedMilliseconds;

    public void Wait()
    {
        double target = last + frameTime;
        clock.WaitUntil(target);
        last = clock.ElapsedMilliseconds;
    }
}
