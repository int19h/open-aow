// Translated from AoWE.pas.

namespace Aow.Game.World.Movement;

[Flags]
public enum CreateMoveModes : byte
{
    None = 0,
    DoNotQueue = 1 << 0,
    Surround = 1 << 1,
    SurroundNearest = 1 << 2,
}
