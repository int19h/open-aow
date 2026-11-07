namespace Aow.Rtl.Host;

public interface IClock
{
    DateTime Now { get; }
    DateTime UtcNow => Now.ToUniversalTime();
    uint TickCount { get; }
    double ElapsedMilliseconds { get; }

    void WaitMilliseconds(int milliseconds);

    void WaitUntil(double deadline)
    {
        int remaining = (int)Math.Ceiling(deadline - ElapsedMilliseconds);
        if (remaining > 0) WaitMilliseconds(remaining);
    }
}

public sealed class SystemClock : IClock
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
    public uint TickCount => unchecked((uint)Environment.TickCount64);
    public double ElapsedMilliseconds => System.Diagnostics.Stopwatch.GetTimestamp() * 1000.0 / System.Diagnostics.Stopwatch.Frequency;

    public void WaitMilliseconds(int milliseconds)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(milliseconds);
        if (milliseconds == 0) Thread.Yield();
        else Thread.Sleep(milliseconds);
    }
}
