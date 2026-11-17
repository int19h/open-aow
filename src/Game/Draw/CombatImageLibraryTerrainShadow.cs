#nullable disable
// Translated from CombatILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Terrain;

namespace Aow.Game.Draw;

public partial class CombatImageLibraryTerrainShadow(CustomNode owner) : HexSprite(owner)
{
    public CombatImageLibraryTerrain Terrain { get; set; }

    // CombatILTer.pas:82
    public override int GetShowPriority() => (Terrain.GetShowPriority() & 67108863) - 1677721600;

    // CombatILTer.pas:96
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface) => Terrain.ShowShadow(pixelXSurface, pixelYSurface, lockedSurface);

    // Engine.pas:524

}
