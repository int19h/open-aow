// Translated from AoWE.pas.

namespace Aow.Game.AI.Groups;

[Flags]
public enum AIMoveExecutorModes : byte
{
    None = 0,
    Adjacent = 1 << 0,
    Simultaneous = 1 << 1,
    Attack = 1 << 2,
}
