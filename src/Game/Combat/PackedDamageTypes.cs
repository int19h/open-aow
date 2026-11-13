// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;

namespace Aow.Game.Combat;

/// <summary>
/// The stored image of a set of damage types (Delphi <c>TP3PackedDamageTypes</c>): the set's two
/// bytes. Game code holds the <see cref="Set{E}"/> itself. The record catalog stores this one-field
/// wrapper as the set.
/// </summary>
[StoredLayout, StoredRecord("PackedDamageTypes")]
[StructLayout(LayoutKind.Explicit, Size = 2)]
public readonly struct PackedDamageTypes : IRecordImage<PackedDamageTypes, Set<DamageType>>
{
    [FieldOffset(0)]
    public readonly ushort ValueImage;

    private PackedDamageTypes(ushort valueImage) => ValueImage = valueImage;

    public Set<DamageType> Value => Set<DamageType>.FromMask(ValueImage);

    public static PackedDamageTypes From(Set<DamageType> value) => new((ushort)value.Image);

    public Set<DamageType> ToValue() => Value;
}
