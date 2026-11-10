// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// { Interface declaration run owned by P4. }
[StructLayout(LayoutKind.Explicit, Size = 20)]
public struct AbilityDamageValues
{
    [FieldOffset(0)]
    public int SourceDamage;
    [FieldOffset(4)]
    public int SourceCost;
    [FieldOffset(8)]
    public int TargetDamage;
    [FieldOffset(12)]
    public int TargetCost;
    [FieldOffset(16)]
    public ushort DamageTypes__image;

    public Set<DamageType> DamageTypes { get => Set<DamageType>.FromMask(DamageTypes__image); set => DamageTypes__image = (ushort)value.Image; }
}
