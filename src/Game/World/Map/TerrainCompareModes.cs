// Translated from HSEngine.pas.

namespace Aow.Game.World.Map;

[Flags]
public enum TerrainCompareModes : byte
{
    None = 0,
    SameOverlayType = 1 << 0,
    SameTerrainType = 1 << 1,
}
