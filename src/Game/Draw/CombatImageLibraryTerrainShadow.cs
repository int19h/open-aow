#nullable disable
// Translated from CombatILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Terrain;

namespace Aow.Game.Draw;

public class CombatImageLibraryTerrainShadow(CustomNode owner) : HexSprite(owner)
{
    public CombatImageLibraryTerrain Terrain { get; init; }

    // CombatILTer.pas:82
    public override int GetShowPriority() => (Terrain.GetShowPriority() & 0b0011_1111_1111_1111_1111_1111_1111) - 1_677_721_600;

    // CombatILTer.pas:96
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface) => Terrain.ShowShadow(pixelXSurface, pixelYSurface, lockedSurface);
}
