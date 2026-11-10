using System.Diagnostics;
using System.Text.Json;
using Aow.Game.Draw;
using Aow.Game.Host;
using Aow.Platform;
using Aow.Rtl;

namespace Aow;

internal static class PacingVerification
{
    public static int Run(string output)
    {
        var host = new NullPresenter();
        var clock = new SdlClock();
        var savedClock = RtlUnit.Clock;
        RtlUnit.Clock = clock;
        var display = new TimedDisplay();
        var controller = new SdlDC(host);
        var times = new List<double>(306);
        const int warmupFrames = 150;
        long previous = 0;
        var priority = Thread.CurrentThread.Priority;
        using var process = Process.GetCurrentProcess();
        var processPriority = process.PriorityClass;
        bool noGc = false;
        try
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            process.PriorityClass = ProcessPriorityClass.High;
            noGc = GC.TryStartNoGCRegion(16 * 1024 * 1024);
            display.Host = host;
            display.EventSink = host;
            display.ActivateDC(controller);
            display.SetUpdate((_, _, _) =>
            {
                if (times.Count == 300) clock.WaitMilliseconds(70);
            });
            display.FramePresented = frame =>
            {
                long now = Stopwatch.GetTimestamp();
                if (frame < warmupFrames) return;
                if (previous != 0) times.Add(Stopwatch.GetElapsedTime(previous, now).TotalMilliseconds);
                previous = now;
                if (times.Count == 306) display.StopUpdateFrameLoop();
            };
            display.StartUpdateFrameLoop();
        }
        finally
        {
            if (noGc) GC.EndNoGCRegion();
            Thread.CurrentThread.Priority = priority;
            process.PriorityClass = processPriority;
            display?.Dispose();
            controller?.Dispose();
            RtlUnit.Clock = savedClock;
        }
        double[] regular = times.Take(300).ToArray();
        double slow = times[300];
        double[] following = times.Skip(301).ToArray();
        bool equal = regular.Concat(following).All(t => Math.Abs(t - 33) <= 1) && slow >= 70;
        Directory.CreateDirectory(output);
        File.WriteAllText(Path.Combine(output, "pacing.json"), JsonSerializer.Serialize(new
        {
            equal,
            frames = 300,
            warmup_frames = warmupFrames,
            fps = 30,
            target_ms = 33,
            tolerance_ms = 1,
            minimum_ms = regular.Min(),
            maximum_ms = regular.Max(),
            slow_ms = slow,
            following_ms = following,
            frame_ms = regular,
            no_window = true
        }, new JsonSerializerOptions { WriteIndented = true }) + "\n");
        Console.WriteLine($"G5: {(equal ? "PASS" : "FAIL")}, 300 frames, {regular.Min():F3}..{regular.Max():F3} ms, slow {slow:F3} ms, following {string.Join(", ", following.Select(t => t.ToString("F3")))} ms, no window");
        return equal ? 0 : 1;
    }

    private sealed class TimedDisplay : Display
    {
        public TimedDisplay() : base(null!) { }

        public void SetUpdate(UpdateFrameEvent action) => OnUpdateFrame = action;
    }

    private sealed class NullPresenter : IDisplayHost, IDisplayEvents
    {
        public (int Width, int Height) ClientSize => (1, 1);
        public bool Fullscreen { get; set; }
        public bool HasFocus => false;

        public void Present(in SurfaceDesc surface) { }

        public (int X, int Y, bool Inside) PointerPosition() => (-1, -1, false);

        public void PumpEvents(IDisplayEvents sink) { }

        public void ShowSystemCursor(bool show) { }
    }
}
