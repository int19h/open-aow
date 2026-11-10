using Aow.Game.Draw;
using Aow.Rtl;

namespace Aow.Game.Host;

public interface IDisplayHost
{
    (int Width, int Height) ClientSize { get; }
    (int Width, int Height) DesktopSize => ClientSize;
    /// <summary>The full screen sizes to offer, or null for the fixed list.</summary>
    IReadOnlyList<(int Width, int Height)> FullscreenSizes => null;

    void ResizeImage(int width, int height) { }

    bool Fullscreen { get; set; }

    void Present(in SurfaceDesc backSurface);
    (int X, int Y, bool Inside) PointerPosition();

    bool HasFocus { get; }

    void PumpEvents(IDisplayEvents sink);
    void ShowSystemCursor(bool show);
}

public interface IDisplayEvents
{
    void MouseDown(MouseButton button, Set<ShiftState> shift, int x, int y) { }

    void MouseUp(MouseButton button, Set<ShiftState> shift, int x, int y) { }

    void MouseMove(Set<ShiftState> shift, int x, int y) { }

    // Positive steps are a roll away from the user. A port addition the original does not have.
    void MouseWheel(int steps, Set<ShiftState> shift, int x, int y) { }

    void DoubleClick() { }

    void KeyDown(ushort key, Set<ShiftState> shift) { }

    void KeyUp(ushort key, Set<ShiftState> shift) { }

    void KeyPress(byte character) { }

    void Resize(int width, int height) { }

    void FocusChanged(bool active) { }

    bool CloseRequested() => true;
}

/// <summary>
/// Where the image sits in the window's output, in output pixels. A point in the margin around the
/// image maps to the nearest edge pixel, so that edge scrolling works with the pointer anywhere
/// past the image's edge. Only a point outside the output is outside.
/// </summary>
public readonly record struct DisplayViewport(int Width, int Height, int Scale, int Left, int Top, int OutputWidth, int OutputHeight)
{
    public static DisplayViewport Fit(int imageWidth, int imageHeight, int windowWidth, int windowHeight)
    {
        if (imageWidth <= 0 || imageHeight <= 0) throw new ArgumentOutOfRangeException(nameof(imageWidth));
        int scale = Math.Max(1, Math.Min(windowWidth / imageWidth, windowHeight / imageHeight));
        return new(imageWidth, imageHeight, scale, (windowWidth - imageWidth * scale) / 2, (windowHeight - imageHeight * scale) / 2, windowWidth, windowHeight);
    }

    public (int X, int Y, bool Inside) Map(float x, float y)
    {
        if (!(x >= 0 && y >= 0 && x < OutputWidth && y < OutputHeight)) return (-1, -1, false);
        int column = (int)Math.Floor((x - Left) / Scale), row = (int)Math.Floor((y - Top) / Scale);
        return (Math.Clamp(column, 0, Width - 1), Math.Clamp(row, 0, Height - 1), true);
    }
}
