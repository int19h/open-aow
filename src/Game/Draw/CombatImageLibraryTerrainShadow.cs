// Translated from CombatILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Terrain;

namespace Aow.Game.Draw;

public partial class CombatImageLibraryTerrainShadow : HexSprite
{
    public CombatImageLibraryTerrainShadow(ECustomNode owner) : base(owner)
    { }

    public CombatImageLibraryTerrain FTerrain;

    // CombatILTer.pas:82
    public override int GetShowPriority() => (FTerrain.GetShowPriority() & 67108863) - 1677721600;

    // CombatILTer.pas:96
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        FTerrain.ShowShadow(XhpSurf, YhpSurf, SurfaceDesc);
    }

    // Engine.pas:524

}
