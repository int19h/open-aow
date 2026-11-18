namespace Aow.Game.Draw;

public enum DisplayControllerError
{
    Ok = 0,
    NotSupported = -2147414015,
    ActivateFailed = -2147414014,
    Failed = -2147414012,
    Activated = -2147414011,
    NotActivated = -2147414010,
    NoFlip = -2147414007,
}

public static class DisplayControllerErrorExtensions
{
    public static bool IsSuccess(this DisplayControllerError error) => error >= 0;
}
