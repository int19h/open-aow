using Aow.Game.Draw;

namespace Aow.Game.Host;

public interface IDisplayHost
{
    (int Width, int Height) ClientSize { get; }
    (int Width, int Height) DesktopSize => ClientSize;
    /// <summary>The full screen sizes to offer, or null for the fixed list.</summary>
    IReadOnlyList<(int Width, int Height)> FullscreenSizes => null;

    void ResizeImage(int width, int height) { }

    bool Fullscreen { get; set; }

    void Present(in LockedSurface backSurface);
    (int X, int Y, bool Inside) PointerPosition();

    bool HasFocus { get; }

    void PumpEvents(IDisplayEvents sink);
    void ShowSystemCursor(bool show);
}
