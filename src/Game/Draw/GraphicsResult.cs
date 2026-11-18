namespace Aow.Game.Draw;

public enum GraphicsResult
{
    Ok = 0,
    Error = -2147483647,
    NotSupported = -2147483646,
    PixelFormatNotSupported = -2147483645,
    BitmapFormatNotSupported = -2147483644,
    CannotLock = -2147483643,
}

public static class GraphicsResultExtensions
{
    public static bool IsSuccess(this GraphicsResult result) => result >= 0;
}
