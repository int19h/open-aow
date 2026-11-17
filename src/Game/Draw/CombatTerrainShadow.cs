#nullable disable
// Translated from CombatTerrain.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Hexes;

namespace Aow.Game.Draw;

public partial class CombatTerrainShadow(CustomNode owner) : HexSprite(owner)
{
    public CombatTerrain Terrain { get; set; }

    // CombatTerrain.pas:72
    public override int GetShowPriority() => (Terrain.GetShowPriority() & 67108863) - 134217728;

    // CombatTerrain.pas:77
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface) => Terrain.ShowShadow(pixelXSurface, pixelYSurface, lockedSurface);

    // Engine.pas:524

}
