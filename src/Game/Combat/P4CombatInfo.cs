// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;

namespace Aow.Game.Combat;

[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct P4CombatInfo
{
    [FieldOffset(0)]
    public byte V0;
    [FieldOffset(1)]
    public byte V1;
    [FieldOffset(2)]
    public ushort DamageTypes__image;

    public Set<DamageType> DamageTypes { get => Set<DamageType>.FromMask(DamageTypes__image); set => DamageTypes__image = (ushort)value.Image; }

    [FieldOffset(4)]
    public byte V4;
    [FieldOffset(5)]
    public byte V5;
    [FieldOffset(6)]
    public byte V6;
    [FieldOffset(7)]
    public byte Modes__image;

    public Set<CombatMode> Modes { get => Set<CombatMode>.FromMask(Modes__image); set => Modes__image = (byte)value.Image; }
}
