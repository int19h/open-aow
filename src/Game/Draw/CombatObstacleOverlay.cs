// Translated from CombatObstacle.pas.
using Aow.Engine.Nodes;

using Aow.Game.World;

namespace Aow.Game.Draw;

public partial class CombatObstacleOverlay : HexSprite
{
    public CombatObstacleOverlay(ECustomNode owner) : base(owner)
    { }

    public CombatObstacle Obstacle;

    // CombatObstacle.pas:84
    public override int GetShowPriority() => (Obstacle.GetShowPriority() & 67108863) + 393216;

    // CombatObstacle.pas:89
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        Obstacle.ShowOverlay(pixelXSurface, pixelYSurface, lockedSurface);
    }

    // Engine.pas:524

}
