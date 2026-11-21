// Translated from AoWE.pas.

namespace Aow.Game.Combat;

[Flags]
public enum CombatObjectControlModes : byte
{
    None = 0,
    ConquerObject = 1 << 0,
}
