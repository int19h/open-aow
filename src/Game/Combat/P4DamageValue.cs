// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;

namespace Aow.Game.Combat;

[StructLayout(LayoutKind.Explicit, Size = 20)]
public struct P4DamageValue
{
    [FieldOffset(0)]
    public int V0;
    [FieldOffset(4)]
    public int V1;
    [FieldOffset(8)]
    public int V2;
    [FieldOffset(12)]
    public int V3;
    [FieldOffset(16)]
    public ushort DamageTypes__image;

    public Set<DamageType> DamageTypes { get => Set<DamageType>.FromMask(DamageTypes__image); set => DamageTypes__image = (ushort)value.Image; }
}
