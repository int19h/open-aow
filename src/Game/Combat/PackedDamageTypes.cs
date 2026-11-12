// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;

namespace Aow.Game.Combat;

// { Interface declaration run owned by P3. }
[StructLayout(LayoutKind.Explicit, Size = 2)]
public struct PackedDamageTypes
{
    [FieldOffset(0)]
    public ushort ValueImage;

    public Set<DamageType> Value { get => Set<DamageType>.FromMask(ValueImage); set => ValueImage = (ushort)value.Image; }
}
