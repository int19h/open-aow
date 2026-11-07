using System.Security.Cryptography;
using System.Text.Json;
using Aow.Rtl;
using Aow.Units.GFXE;

namespace Aow.Platform;

/// <summary>Records the same completed surfaces as SDL without initializing video or audio.</summary>
public sealed class HeadlessDisplayHost(string directory, int width = 640, int height = 480, long captureFrame = 0) : IDisplayHost
{
    public (int Width, int Height) ClientSize { get; private set; } = (width, height);
    public bool Fullscreen { get; set; }
    public bool HasFocus { get; set; } = true;
    public long Frame { get; private set; }
    public (int X, int Y) Mouse { get; set; } = (-1, -1);

    public (int X, int Y, bool Inside) PointerPosition() => HasFocus && Mouse.X >= 0 && Mouse.Y >= 0 && Mouse.X < ClientSize.Width && Mouse.Y < ClientSize.Height ? (Mouse.X, Mouse.Y, true) : (-1, -1, false);

    public Func<bool>? CaptureWhen { get; set; }
    public Action? AfterPresent { get; set; }
    public int? FixtureFrame { get; set; }
    public string? CaptureName { get; set; }

    public void ShowSystemCursor(bool show) { }

    public void ResizeImage(int width, int height) => ClientSize = (width, height);

    public void PumpEvents(IDisplayEvents sink) { }

    public void Present(in TSurfaceDesc surface)
    {
        if (surface.Width < 1 || surface.Height < 1 || surface.BitCount != 16 || surface.PixelFormatID != GFXEUnit.pfRGB555 || Math.Abs((long)surface.Pitch) < surface.Width * 2L)
            throw new ArgumentException("The presenter requires 555 rows with a valid pitch.", nameof(surface));
        ClientSize = (surface.Width, surface.Height);
        Frame++;
        if (CaptureWhen is not null ? !CaptureWhen() : captureFrame != 0 && Frame != captureFrame) { AfterPresent?.Invoke(); return; }
        int pitch = checked(surface.Width * 2);
        byte[] bytes = new byte[checked(pitch * surface.Height)];
        Ptr<byte> source = surface.Data;
        for (int y = 0; y < surface.Height; y++)
            (source + checked(y * surface.Pitch)).AsSpan(pitch).CopyTo(bytes.AsSpan(y * pitch, pitch));
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
