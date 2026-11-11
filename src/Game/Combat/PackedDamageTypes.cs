// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;

namespace Aow.Game.Combat;

// { Interface declaration run owned by P3. }
[StructLayout(LayoutKind.Explicit, Size = 2)]
public struct PackedDamageTypes
{
    [FieldOffset(0)]
    public ushort Value__image;

    public Set<DamageType> Value { get => Set<DamageType>.FromMask(Value__image); set => Value__image = (ushort)value.Image; }
}
