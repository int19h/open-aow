// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

[StructLayout(LayoutKind.Explicit, Size = 6)]
public struct AbilityCombatInfo
{
    [FieldOffset(0)]
    public byte TargetTypes__image;

    public Set<CombatTargetTypeElement> TargetTypes { get => Set<CombatTargetTypeElement>.FromMask(TargetTypes__image); set => TargetTypes__image = (byte)value.Image; }

    [FieldOffset(1)]
    public sbyte Attack;
    [FieldOffset(2)]
    public sbyte Damage;
    [FieldOffset(3)]
    public ushort DamageTypes__image;

    public Set<DamageType> DamageTypes { get => Set<DamageType>.FromMask(DamageTypes__image); set => DamageTypes__image = (ushort)value.Image; }

    [FieldOffset(5)]
    public sbyte RepeatCount;
}
