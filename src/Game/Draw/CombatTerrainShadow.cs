#nullable disable
// Translated from CombatTerrain.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Hexes;

namespace Aow.Game.Draw;

public sealed class CombatTerrainShadow(CustomNode owner) : HexSprite(owner)
{
    public CombatTerrain Terrain { get; init; }

    // CombatTerrain.pas:72
    public override int GetShowPriority() => (Terrain.GetShowPriority() & 0b0011_1111_1111_1111_1111_1111_1111) - 134_217_728;

    // CombatTerrain.pas:77
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface) => Terrain.ShowShadow(pixelXSurface, pixelYSurface, lockedSurface);
}
