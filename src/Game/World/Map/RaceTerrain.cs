// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;

namespace Aow.Game.World.Map;

[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct RaceTerrain
{
    [FieldOffset(0)]
    public ushort PreferredImage;

    public Set<TerrainType> Preferred { get => Set<TerrainType>.FromMask(PreferredImage); set => PreferredImage = (ushort)value.Image; }

    [FieldOffset(2)]
    public ushort DislikedImage;

    public Set<TerrainType> Disliked { get => Set<TerrainType>.FromMask(DislikedImage); set => DislikedImage = (ushort)value.Image; }
}
