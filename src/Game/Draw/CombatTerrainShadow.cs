// Translated from CombatTerrain.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Hexes;

namespace Aow.Game.Draw;

public partial class CombatTerrainShadow : HexSprite
{
    public CombatTerrainShadow(ECustomNode owner) : base(owner)
    { }

    public CombatTerrain _terrain;

    // CombatTerrain.pas:72
    public override int GetShowPriority() => (_terrain.GetShowPriority() & 67108863) - 134217728;

    // CombatTerrain.pas:77
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        _terrain.ShowShadow(pixelXSurface, pixelYSurface, lockedSurface);
    }

    // Engine.pas:524

}
