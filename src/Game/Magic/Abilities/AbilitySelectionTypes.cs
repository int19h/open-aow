// Translated from AoWE.pas.

namespace Aow.Game.Magic.Abilities;

[Flags]
public enum AbilitySelectionTypes : ushort
{
    None = 0,
    Unit = 1 << 0,
    HeadItem = 1 << 1,
    TorsoItem = 1 << 2,
    AttackItem = 1 << 3,
    DefenseItem = 1 << 4,
    RingItem = 1 << 5,
    UseItem = 1 << 6,
    CustomizeLeader = 1 << 7,
    HeroUpgrade = 1 << 8,
    Editor = 1 << 9,
}
