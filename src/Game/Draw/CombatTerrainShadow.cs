// Translated from CombatTerrain.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Hexes;

namespace Aow.Game.Draw;

public partial class CombatTerrainShadow : HexSprite
{
    public CombatTerrainShadow(ECustomNode owner) : base(owner)
    { }

    public CombatTerrain FTerrain;

    // CombatTerrain.pas:72
    public override int GetShowPriority() => (FTerrain.GetShowPriority() & 67108863) - 134217728;

    // CombatTerrain.pas:77
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        FTerrain.ShowShadow(XhpSurf, YhpSurf, SurfaceDesc);
    }

    // Engine.pas:524

}
