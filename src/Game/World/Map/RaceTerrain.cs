// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;

namespace Aow.Game.World.Map;

[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct RaceTerrain
{
    [FieldOffset(0)]
    public ushort Preferred__image;

    public Set<TerrainType> Preferred { get => Set<TerrainType>.FromMask(Preferred__image); set => Preferred__image = (ushort)value.Image; }

    [FieldOffset(2)]
    public ushort Disliked__image;

    public Set<TerrainType> Disliked { get => Set<TerrainType>.FromMask(Disliked__image); set => Disliked__image = (ushort)value.Image; }
}
