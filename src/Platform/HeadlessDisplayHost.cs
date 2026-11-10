using System.Security.Cryptography;
using System.Text.Json;
using Aow.Game.Draw;
using Aow.Game.Host;
using Aow.Rtl.Host;

namespace Aow.Platform;

/// <summary>Records the same completed surfaces as SDL without initializing video or audio.</summary>
public sealed class HeadlessDisplayHost(string directory, int width = 640, int height = 480, long captureFrame = 0) : IDisplayHost
{
    public (int Width, int Height) ClientSize { get; private set; } = (width, height);
    public bool Fullscreen { get; set; }
    public bool HasFocus { get; set; } = true;
    public long Frame { get; private set; }
    public (int X, int Y) Mouse { get; set; } = (-1, -1);

    private readonly InputMapper input = new();

    public (int X, int Y, bool Inside) PointerPosition() => HasFocus && Mouse.X >= 0 && Mouse.Y >= 0 && Mouse.X < ClientSize.Width && Mouse.Y < ClientSize.Height ? (Mouse.X, Mouse.Y, true) : (-1, -1, false);

    public Func<bool>? CaptureWhen { get; set; }
    public Action? AfterPresent { get; set; }
    public int? FixtureFrame { get; set; }
    public string? CaptureName { get; set; }

    public void ShowSystemCursor(bool show) { }

    public void ResizeImage(int width, int height) => ClientSize = (width, height);

    public void PumpEvents(IDisplayEvents sink) { }

    // The same mapper entry the SDL host uses, over a 1:1 view of the client, so a test can roll the wheel.
    public void Wheel(float y, int direction, float x, float yPosition, IDisplayEvents sink)
    {
        var size = ClientSize;
        var viewport = new DisplayViewport(size.Width, size.Height, 1, 0, 0, size.Width, size.Height);
        input.Wheel(y, direction, x, yPosition, viewport, sink);
    }

    public void Present(in SurfaceDesc surface)
    {
        if (surface.Width < 1 || surface.Height < 1 || surface.BitCount != 16 || surface.PixelFormatID != PixelFormats.pfRGB555 || Math.Abs((long)surface.Pitch) < surface.Width * 2L)
            throw new ArgumentException("The presenter requires 555 rows with a valid pitch.", nameof(surface));
        ClientSize = (surface.Width, surface.Height);
        Frame++;
        if (CaptureWhen is not null ? !CaptureWhen() : captureFrame != 0 && Frame != captureFrame) { AfterPresent?.Invoke(); return; }
        int pitch = checked(surface.Width * 2);
        byte[] bytes = new byte[checked(pitch * surface.Height)];
        for (int y = 0; y < surface.Height; y++)
            surface.Data.AsSpan(checked(surface.DataIndex + y * surface.Pitch), pitch).CopyTo(bytes.AsSpan(y * pitch, pitch));
        Directory.CreateDirectory(directory);
        string name = Path.Combine(directory, CaptureName ?? $"frame-{FixtureFrame ?? Frame}");
        File.WriteAllBytes(name + ".raw", bytes);
        File.WriteAllText(name + ".json", JsonSerializer.Serialize(new
        {
            width = surface.Width,
            height = surface.Height,
            frame = FixtureFrame ?? Frame,
            pitch,
            format = "RGB555LE",
            orientation = "top-down",
            sha256 = Convert.ToHexStringLower(SHA256.HashData(bytes))
        }, new JsonSerializerOptions { WriteIndented = true }) + "\n");
        AfterPresent?.Invoke();
    }
}

public sealed class FixtureClock(DateTime now) : IClock
{
    public DateTime Now => now;
    public double ElapsedMilliseconds { get; private set; }
    public uint TickCount => unchecked((uint)ElapsedMilliseconds);

    public void WaitMilliseconds(int milliseconds)
    {
        if (milliseconds < 0) throw new ArgumentOutOfRangeException(nameof(milliseconds));
        ElapsedMilliseconds += milliseconds;
    }
}
