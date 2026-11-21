// Translated from AoWE.pas.

namespace Aow.Game.Units;

[Flags]
public enum ItemStatuses : byte
{
    None = 0,
    Activated = 1 << 0,
    Registered = 1 << 1,
    Readonly = 1 << 2,
}
