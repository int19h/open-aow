
namespace Aow.Game.Host;

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
