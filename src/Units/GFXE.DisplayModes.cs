namespace Aow.Units.GFXE;

public static partial class GFXEUnit
{
    public static Func<(int Width, int Height)> DesktopSize { get; set; } = () => (1280, 1024);
    /// <summary>The full screen sizes the display host offers, or null for the fixed list of the headless host.</summary>
    public static Func<IReadOnlyList<(int Width, int Height)>> FullscreenSizes { get; set; } = () => null;

    private static readonly (int Width, int Height)[] FixedSizes = [(640, 480), (800, 600), (1024, 768), (1280, 1024)];

    /// <summary>
    /// The sizes that fill a desktop at a whole pixel scale from 8 down to 1, smallest first. A
    /// desktop that does not divide evenly leaves a margin of less than one scaled pixel. Sizes
    /// below 640x480 are left out, as the video dialog does.
    /// </summary>
    public static IReadOnlyList<(int Width, int Height)> IntegerScaleSizes(int desktopWidth, int desktopHeight)
    {
        var sizes = new List<(int Width, int Height)>();
        for (int scale = 8; scale >= 1; scale--)
        {
            var size = (desktopWidth / scale, desktopHeight / scale);
            if (size.Item1 >= 640 && size.Item2 >= 480 && !sizes.Contains(size)) sizes.Add(size);
        }
        return sizes;
    }

    public static void ListDisplayModes(TDisplayModeList list, int width, int height, int pixelFormatID)
    {
        var desktop = DesktopSize();
        var sizes = FullscreenSizes() ?? [.. FixedSizes.Where(size => size.Width <= desktop.Width && size.Height <= desktop.Height)];
        foreach (var size in sizes)
            if ((width == 0 || width == size.Width) && (height == 0 || height == size.Height) &&
                (pixelFormatID == 0 || pixelFormatID == 16 || pixelFormatID == pfRGB555))
                list.Add(size.Width, size.Height, pfRGB555);
    }
}
