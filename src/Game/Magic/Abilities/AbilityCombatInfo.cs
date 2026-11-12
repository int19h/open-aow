// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

[StructLayout(LayoutKind.Explicit, Size = 6)]
public struct AbilityCombatInfo
{
    [FieldOffset(0)]
    public byte TargetTypesImage;

    public Set<CombatTargetTypeElement> TargetTypes { get => Set<CombatTargetTypeElement>.FromMask(TargetTypesImage); set => TargetTypesImage = (byte)value.Image; }

    [FieldOffset(1)]
    public sbyte Attack;
    [FieldOffset(2)]
    public sbyte Damage;
    [FieldOffset(3)]
    public ushort DamageTypesImage;

    public Set<DamageType> DamageTypes { get => Set<DamageType>.FromMask(DamageTypesImage); set => DamageTypesImage = (ushort)value.Image; }

    [FieldOffset(5)]
    public sbyte RepeatCount;
}
