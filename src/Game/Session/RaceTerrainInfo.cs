// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;

namespace Aow.Game.Session;

[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct RaceTerrainInfo
{
    [FieldOffset(0)]
    public ushort BonusImage;

    public Set<byte> Bonus { get => Set<byte>.FromMask(BonusImage); set => BonusImage = (ushort)value.Image; }

    [FieldOffset(2)]
    public ushort PenaltyImage;

    public Set<byte> Penalty { get => Set<byte>.FromMask(PenaltyImage); set => PenaltyImage = (ushort)value.Image; }
}
