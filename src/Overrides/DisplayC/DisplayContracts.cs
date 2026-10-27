using Aow.Rtl;
using Aow.Units.GFXE;

namespace Aow.Platform;

public interface IDisplayHost
{
    (int Width, int Height) ClientSize { get; }
    (int Width, int Height) DesktopSize => ClientSize;
    void ResizeImage(int width, int height) { }
    bool Fullscreen { get; set; }
    void Present(in TSurfaceDesc backSurface);
    (int X, int Y, bool Inside) PointerPosition();
    bool HasFocus { get; }
    void PumpEvents(IDisplayEvents sink);
    void ShowSystemCursor(bool show);
}

public interface IDisplayEvents
{
    void MouseDown(TMouseButton button, TSet<TShiftState> shift, int x, int y) { }
    void MouseUp(TMouseButton button, TSet<TShiftState> shift, int x, int y) { }
    void MouseMove(TSet<TShiftState> shift, int x, int y) { }
    void DoubleClick() { }
    void KeyDown(ushort key, TSet<TShiftState> shift) { }
    void KeyUp(ushort key, TSet<TShiftState> shift) { }
    void KeyPress(byte character) { }
    void Resize(int width, int height) { }
    void FocusChanged(bool active) { }
    bool CloseRequested() => true;
}

public readonly record struct DisplayViewport(int Width, int Height, int Scale, int Left, int Top)
{
    public static DisplayViewport Fit(int imageWidth, int imageHeight, int windowWidth, int windowHeight)
    {
        if (imageWidth <= 0 || imageHeight <= 0) throw new ArgumentOutOfRangeException(nameof(imageWidth));
        int scale = Math.Max(1, Math.Min(windowWidth / imageWidth, windowHeight / imageHeight));
        return new(imageWidth, imageHeight, scale, (windowWidth - imageWidth * scale) / 2, (windowHeight - imageHeight * scale) / 2);
    }
    public (int X, int Y, bool Inside) Map(float x, float y)
    {
        bool inside = x >= Left && y >= Top && x < Left + Width * Scale && y < Top + Height * Scale;
        return inside ? ((int)((x - Left) / Scale), (int)((y - Top) / Scale), true) : (-1, -1, false);
    }
}
