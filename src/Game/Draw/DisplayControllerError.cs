namespace Aow.Game.Draw;

public enum DisplayControllerError
{
    Ok = 0,
    NotSupported = -2_147_414_015,
    ActivateFailed = -2_147_414_014,
    Failed = -2_147_414_012,
    Activated = -2_147_414_011,
    NotActivated = -2_147_414_010,
    NoFlip = -2_147_414_007,
}

public static class DisplayControllerErrorExtensions
{
    public static bool IsSuccess(this DisplayControllerError error) => error >= 0;
}
