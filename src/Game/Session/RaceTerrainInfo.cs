// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.Session;

// Each set keeps the 16-bit width of the Delphi set: a value is cut to its low 16 bits.
public readonly record struct RaceTerrainInfo
{
    public Set<byte> Bonus { get; init => field = Set<byte>.FromMask((ushort)value.Image); }

    public Set<byte> Penalty { get; init => field = Set<byte>.FromMask((ushort)value.Image); }
}
