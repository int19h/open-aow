// Translated from AoWE.pas.

namespace Aow.Game.AI.Groups;

public enum AIGroupTargetingStatus : byte
{
    Idle = 0,
    Busy = 1,
    SettingupTarget = 2,
    ExecutingTarget = 3,
    NoTargetsFound = 4,
    TargetConquered = 5,
    Failed = 6
}
