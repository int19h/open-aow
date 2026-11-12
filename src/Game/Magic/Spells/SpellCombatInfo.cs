// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct SpellCombatInfo
{
    [FieldOffset(0)]
    public sbyte Attack;
    [FieldOffset(1)]
    public sbyte Damage;
    [FieldOffset(2)]
    public ushort DamageTypesImage;

    public Set<DamageType> DamageTypes { get => Set<DamageType>.FromMask(DamageTypesImage); set => DamageTypesImage = (ushort)value.Image; }

    [FieldOffset(4)]
    public sbyte RepeatCount;
    [FieldOffset(5)]
    public sbyte Range;
    [FieldOffset(6)]
    public sbyte Radius;
    [FieldOffset(7)]
    public byte TargetTypesImage;

    public Set<CombatTargetTypeElement> TargetTypes { get => Set<CombatTargetTypeElement>.FromMask(TargetTypesImage); set => TargetTypesImage = (byte)value.Image; }
}
