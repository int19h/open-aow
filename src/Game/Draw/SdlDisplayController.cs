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

    public override DisplayControllerError Activate(CustomDisplay display)
    {
        DisplayControllerError result = base.Activate(display);
        if (result != DisplayControllerError.Ok)
        {
            return result;
        }

        Status = Status.With(Windowed).With(Visible);
        _surfaces[0] = new ScreenSurface(host);
        try
        {
            for (int i = 1; i <= (int)display.BufferingScheme; i++)
            {
                _surfaces[i] = new Dib16Surface(display.BackSurfaceWidth, display.BackSurfaceHeight, PixelFormat.Rgb555);
                _ = _surfaces[i].Clear(display.Color);
            }
            return DisplayControllerError.Ok;
        }
        catch
        {
            _ = Deactivate();
            throw;
        }
    }

    public override DisplayControllerError Deactivate()
    {
        for (int i = 0; i < _surfaces.Length; i++)
        {
            _surfaces[i]?.Dispose();
            _surfaces[i] = null;
        }
        return base.Deactivate();
    }

    public override DisplayControllerError BlitSurface(int index)
    {
        Surface surface = GetSurface(index);
        if (surface is null)
        {
            return DisplayControllerError.Failed;
        }

        LockedSurface descriptor = default;
        GraphicsResult result = surface.Lock(ref descriptor);
        if (result != GraphicsResult.Ok)
        {
            return (DisplayControllerError)result;
        }

        try
        {
            host.Present(descriptor);
        }
        finally
        {
            _ = surface.Unlock();
        }
        return DisplayControllerError.Ok;
    }

    public override DisplayControllerError FlipSurface(int index)
    {
        DisplayControllerError result = BlitSurface(index);
        return result != DisplayControllerError.Ok ? result : DisplayControllerError.NoFlip;
    }

    public override DisplayControllerError ResizeBackSurfaces(int width, int height)
    {
        foreach (Surface surface in _surfaces.Skip(1))
        {
            _ = (surface?.Resize(width, height));
        }

        host.ResizeImage(width, height);
        InvalidateAll();
        return DisplayControllerError.Ok;
    }

    public override void GetMousePosition(ref int x, ref int y)
    {
        (int pointerX, int pointerY, bool inside) = host.PointerPosition();
        (x, y) = host.HasFocus && inside ? (pointerX, pointerY) : (-1, -1);
        host.ShowSystemCursor(x == -1);
    }

    public override DisplayControllerError Hide()
    {
        Status = Status.Without(Visible);
        return DisplayControllerError.Ok;
    }

    public override DisplayControllerError Show()
    {
        Status = Status.With(Visible);
        return DisplayControllerError.Ok;
    }

    public override DisplayControllerError SupportDisplayMode(CustomDisplay display) => DisplayControllerError.Ok;

    public override void WindowRecreated() { }

    private sealed class ScreenSurface(IDisplayHost host) : Surface
    {
        public override int Width => host.ClientSize.Width;
        public override int Height => host.ClientSize.Height;

        public override PixelFormat GetPixelFormatId() => PixelFormat.Rgb555;
        public override GraphicsResult Lock(ref LockedSurface descriptor) => GraphicsResult.NotSupported;
    }
}
