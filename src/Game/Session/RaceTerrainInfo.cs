// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;

namespace Aow.Game.Session;

[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct RaceTerrainInfo
{
    [FieldOffset(0)]
    public ushort Bonus__image;

    public Set<byte> Bonus { get => Set<byte>.FromMask(Bonus__image); set => Bonus__image = (ushort)value.Image; }

    [FieldOffset(2)]
    public ushort Penalty__image;

    public Set<byte> Penalty { get => Set<byte>.FromMask(Penalty__image); set => Penalty__image = (ushort)value.Image; }
}
