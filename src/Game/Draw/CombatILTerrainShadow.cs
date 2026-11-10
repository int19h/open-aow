// Translated from CombatILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Terrain;

namespace Aow.Game.Draw;

public partial class CombatILTerrainShadow : HexagonSprite
{
    public CombatILTerrainShadow(ECustomNode owner) : base(owner)
    { }

    public CombatILTerrain FTerrain;

    // CombatILTer.pas:82
    public override int GetShowPriority() => (FTerrain.GetShowPriority() & 67108863) - 1677721600;

    // CombatILTer.pas:96
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        FTerrain.ShowShadow(XhpSurf, YhpSurf, SurfaceDesc);
    }

    // Engine.pas:524

}
