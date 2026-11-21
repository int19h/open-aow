// Translated from AoWE.pas.

namespace Aow.Game.Combat;

[Flags]
public enum TargetSelectionModes : byte
{
    None = 0,
    OwnerDraw = 1 << 0,
    FixedLevel = 1 << 1,
    ValidateDistance = 1 << 2,
    ExternalFloater = 1 << 3,
}
