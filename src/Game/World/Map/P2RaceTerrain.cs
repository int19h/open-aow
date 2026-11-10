// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;

namespace Aow.Game.World.Map;

[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct P2RaceTerrain
{
    [FieldOffset(0)]
    public ushort Preferred__image;

    public Set<AoWTerrainType> Preferred { get => Set<AoWTerrainType>.FromMask(Preferred__image); set => Preferred__image = (ushort)value.Image; }

    [FieldOffset(2)]
    public ushort Disliked__image;

    public Set<AoWTerrainType> Disliked { get => Set<AoWTerrainType>.FromMask(Disliked__image); set => Disliked__image = (ushort)value.Image; }
}
