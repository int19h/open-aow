namespace Aow.Game.Draw;

public enum GraphicsResult
{
    Ok = 0,
    Error = -2_147_483_647,
    NotSupported = -2_147_483_646,
    PixelFormatNotSupported = -2_147_483_645,
    BitmapFormatNotSupported = -2_147_483_644,
    CannotLock = -2_147_483_643,
}

public static class GraphicsResultExtensions
{
    public static bool IsSuccess(this GraphicsResult result) => result >= 0;
}
