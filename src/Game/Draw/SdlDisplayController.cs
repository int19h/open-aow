#nullable disable
using Aow.Game.Host;
using Aow.Rtl;
using static Aow.Game.Draw.DisplayControllerStatusElement;

namespace Aow.Game.Draw;

// The primary constructor captures host before the base inserts this component.
public sealed class SdlDisplayController(IDisplayHost host, Component owner = null) : DisplayController(owner)
{
    private readonly Surface[] _surfaces = new Surface[3];

    public bool UseVerticalBlank { get; set; } = true;

    public override Surface GetSurface(int index) => index >= 0 && index < _surfaces.Length ? _surfaces[index] : null;

    public override int Activate(CustomDisplay display)
    {
        int result = base.Activate(display);
        if (result < 0) return result;
        _status = _status.With(Windowed).With(Visible);
        _surfaces[0] = new ScreenSurface(host);
        try
        {
            for (int i = 1; i <= (int)display._bufferingScheme; i++)
            {
                _surfaces[i] = new Dib16Surface(display._backSurfaceWidth, display._backSurfaceHeight, PixelFormats.pfRGB555);
                _surfaces[i].Clear(display.Color);
            }
            return 0;
        }
        catch { Deactivate(); throw; }
    }

    public override int Deactivate()
    {
        for (int i = 0; i < _surfaces.Length; i++) { _surfaces[i]?.Dispose(); _surfaces[i] = null; }
        return base.Deactivate();
    }

    public override int BlitSurface(int index)
    {
        var surface = GetSurface(index);
        if (surface is null) return DisplayCodes.DCERR_FAILED;
        LockedSurface descriptor = default;
        int result = surface.Lock(ref descriptor);
        if (result < 0) return result;
        try { host.Present(descriptor); }
        finally { surface.Unlock(); }
        return 0;
    }

    public override int FlipSurface(int index)
    {
        int result = BlitSurface(index);
        return result < 0 ? result : DisplayCodes.DCERR_NOFLIP;
    }

    public override int ResizeBackSurfaces(int width, int height)
    {
        foreach (var surface in _surfaces.Skip(1)) surface?.Resize(width, height);
        host.ResizeImage(width, height);
        InvalidateAll();
        return 0;
    }

    public override void GetMousePosition(ref int x, ref int y)
    {
        var pointer = host.PointerPosition();
        (x, y) = host.HasFocus && pointer.Inside ? (pointer.X, pointer.Y) : (-1, -1);
        host.ShowSystemCursor(x == -1);
    }

    public override int Hide() { _status = _status.Without(Visible); return 0; }

    public override int Show() { _status = _status.With(Visible); return 0; }

    public override bool WindowProc(ref Message message) => false;
    public override int SupportDisplayMode(CustomDisplay display) => 0;

    public override void WindowRecreated() { }

    private sealed class ScreenSurface(IDisplayHost host) : Surface
    {
        public override int GetWidth() => host.ClientSize.Width;
        public override int GetHeight() => host.ClientSize.Height;
        public override int GetPixelFormatId() => PixelFormats.pfRGB555;
        public override int Lock(ref LockedSurface descriptor) => PixelFormats.GFXE_NOTSUPPORTED;
    }
}
