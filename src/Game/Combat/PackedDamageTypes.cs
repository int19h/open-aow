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
public readonly struct PackedDamageTypes : IRecordImage<PackedDamageTypes, DamageTypes>
{
    [FieldOffset(0)]
    public readonly ushort ValueImage;

    private PackedDamageTypes(ushort valueImage) => ValueImage = valueImage;

    public DamageTypes Value => (DamageTypes)ValueImage;

    public static PackedDamageTypes From(DamageTypes value) => new((ushort)value);

    public DamageTypes ToValue() => Value;
}
