// Translated from CombatObstacle.pas.
using Aow.Engine.Nodes;

using Aow.Game.World;

namespace Aow.Game.Draw;

public partial class CombatObstacleOverlay : HexSprite
{
    public CombatObstacleOverlay(ECustomNode owner) : base(owner)
    { }

    public CombatObstacle FObstacle;

    // CombatObstacle.pas:84
    public override int GetShowPriority() => (FObstacle.GetShowPriority() & 67108863) + 393216;

    // CombatObstacle.pas:89
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        FObstacle.ShowOverlay(XhpSurf, YhpSurf, SurfaceDesc);
    }

    // Engine.pas:524

}
