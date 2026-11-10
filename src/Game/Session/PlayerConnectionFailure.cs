// Translated from AoWE.pas.

namespace Aow.Game.Session;

public enum PlayerConnectionFailure : byte
{
    None = 0,
    TimeOut = 1,
    Disconnected = 2,
    OutOfSync = 3
}
