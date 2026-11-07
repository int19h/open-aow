using System.Text.Json;
using Aow.Platform;
using Aow.Rtl;
using Aow.Units.DisplayC;
using Aow.Units.GFXE;

namespace Aow.Game;

internal static class FrameVerification
{
    public static void Run(string directory)
    {
        var clock = new FixtureClock(new(2000, 1, 1));
        var savedClock = RtlUnit.Clock;
        RtlUnit.Clock = clock;
        var host = new TestHost();
        var display = new TestDisplay();
        var controller = new TSdlDC(host);
        int checks = 0;
        void Check(bool success, string label)
        {
            checks++;
            if (!success) throw new InvalidOperationException("Display check: " + label);
        }
        try
        {
            display.Host = host;
            display.EventSink = host;
            Check(display.ActivateDC(controller) == 0, "activation");
            Check(controller.GetStatus().Contains(TDisplayCtrlStatusElement.dsActivated) && controller.GetStatus().Contains(TDisplayCtrlStatusElement.dsWindowed), "controller status");
            var screen = controller.GetSurface(0);
            var back = controller.GetSurface(1);
            Check(screen.Width == 640 && screen.Height == 480 && back.Width == 640 && back.Height == 480, "screen and back sizes");
            Check(controller.GetSurfaceCount() == 1 && controller.GetSurface(2) is null, "double buffering");
            TSurfaceDesc sd = default;
            Check(back.Lock(ref sd) == 0 && sd.PixelFormatID == GFXEUnit.pfRGB555, "555 back surface");
            back.Unlock();
            Check(controller.FlipSurface(1) == DisplayCUnit.DCERR_NOFLIP && host.Presents == 1, "flip presents and reports fallback");
            display.PresentInitialFrame();
            display.PresentInitialFrame();
            Check(display.PresentedFrames == 1 && host.Presents == 2, "initial paint counts once");
            int x = 0, y = 0;
            controller.GetMousePos(ref x, ref y);
            Check(x == 639 && y == 479 && !host.SystemCursor, "inside pointer");
            host.HasFocus = false;
            controller.GetMousePos(ref x, ref y);
            Check(x == -1 && y == -1 && host.SystemCursor, "unfocused pointer");
            host.HasFocus = true;
            host.ClientSize = (800, 600);
            display.Resize();
            Check(screen.Width == 800 && back.Width == 800 && back.Height == 600, "resize uses client pixels");
            int draws = 0;
            display.SetUpdate((_, _, index) => { Check(index == 1, "double buffer index"); draws++; });
            display.FramePresented = frame => { if (frame == 4) display.StopUpdateFrameLoop(); };
            display.StartUpdateFrameLoop();
            Check(draws == 3 && host.Pumps == 3 && host.Presents == 5, "synchronous frame loop");
            Check(clock.ElapsedMilliseconds == 99, "33 ms limiter");
            display.PauseFrameUpdateLoop();
            display.PauseFrameUpdateLoop();
            display.ResumeFrameUpdateLoop();
            Check(display.FStatus.Contains(TDisplayStatusElement.dsFrameLoopPaused), "nested pause");
            display.ResumeFrameUpdateLoop();
            Check(!display.FStatus.Contains(TDisplayStatusElement.dsFrameLoopPaused), "balanced resume");
            Check(display.SwapDC() == 0 && host.Fullscreen && ReferenceEquals(back, controller.GetSurface(1)), "fullscreen keeps surface pixels");

            var recorder = new HeadlessDisplayHost(directory);
            byte[] pixels = [1, 0, 2, 0, 99, 99, 3, 0, 4, 0, 99, 99];
            sd = new() { Width = 2, Height = 2, Pitch = -6, BitCount = 16, PixelFormatID = GFXEUnit.pfRGB555, Data = Ptr<byte>.To(pixels, 6) };
            recorder.Present(sd);
            Check(File.ReadAllBytes(Path.Combine(directory, "frame-1.raw")).SequenceEqual(new byte[] { 3, 0, 4, 0, 1, 0, 2, 0 }), "top-down recording excludes padding and handles negative pitch");
            using var metadata = JsonDocument.Parse(File.ReadAllText(Path.Combine(directory, "frame-1.json")));
            Check(metadata.RootElement.GetProperty("frame").GetInt32() == 1 && metadata.RootElement.GetProperty("pitch").GetInt32() == 4, "frame metadata");
        }
        finally
        {
            display.Free();
            controller.Free();
            RtlUnit.Clock = savedClock;
        }
        Console.WriteLine($"Frame display PASS: {checks} checks, memory surfaces, focus, fullscreen, limiter, headless rows, no window");
    }
    private sealed class TestDisplay : TDisplay
    {
        public TestDisplay() : base(null!) { }

        public void SetUpdate(TUpdateFrameEvent action) => OnUpdateFrame = action;
    }
    private sealed class TestHost : IDisplayHost, IDisplayEvents
    {
        public (int Width, int Height) ClientSize { get; set; } = (640, 480);
        public bool Fullscreen { get; set; }
        public bool HasFocus { get; set; } = true;
        public bool SystemCursor;
        public int Presents, Pumps;
        public void Present(in TSurfaceDesc surface) => Presents++;
        public (int X, int Y, bool Inside) PointerPosition() => (639, 479, true);
        public void PumpEvents(IDisplayEvents sink) => Pumps++;
        public void ShowSystemCursor(bool show) => SystemCursor = show;
    }
}
