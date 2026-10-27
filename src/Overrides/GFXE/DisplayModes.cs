namespace Aow.Units.GFXE;

public static partial class GFXE
{
    public static Func<(int Width, int Height)> DesktopSize { get; set; } = () => (1280, 1024);
    public static void ListDisplayModes(TDisplayModeList list, int width, int height, int pixelFormatID)
    {
        var desktop = DesktopSize();
        foreach (var size in new (int Width, int Height)[] { (640, 480), (800, 600), (1024, 768), (1280, 1024) })
            if (size.Width <= desktop.Width && size.Height <= desktop.Height &&
                (width == 0 || width == size.Width) && (height == 0 || height == size.Height) &&
                (pixelFormatID == 0 || pixelFormatID == 16 || pixelFormatID == pfRGB555))
                list.Add(size.Width, size.Height, pfRGB555);
    }
}
