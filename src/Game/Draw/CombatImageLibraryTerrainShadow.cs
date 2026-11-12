// Translated from CombatILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Terrain;

namespace Aow.Game.Draw;

public partial class CombatImageLibraryTerrainShadow : HexSprite
{
    public CombatImageLibraryTerrainShadow(ECustomNode owner) : base(owner)
    { }

    public CombatImageLibraryTerrain _terrain;

    // CombatILTer.pas:82
    public override int GetShowPriority() => (_terrain.GetShowPriority() & 67108863) - 1677721600;

    // CombatILTer.pas:96
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        _terrain.ShowShadow(pixelXSurface, pixelYSurface, lockedSurface);
    }

    // Engine.pas:524

}
