// Translated from AoWE.pas.

namespace Aow.Game.World.Movement;

[Flags]
public enum CanMoveModes : byte
{
    None = 0,
    Fast = 1 << 0,
    Exact = 1 << 1,
}
