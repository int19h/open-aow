#nullable disable
// Translated from CombatObstacle.pas.
using Aow.Engine.Nodes;

using Aow.Game.World;

namespace Aow.Game.Draw;

public class CombatObstacleShadow(CustomNode owner) : HexSprite(owner)
{
    public CombatObstacle Obstacle { get; init; }

    // CombatObstacle.pas:74
    public override int GetShowPriority() => (Obstacle.GetShowPriority() & 0b0011_1111_1111_1111_1111_1111_1111) - 1_677_721_600;

    // CombatObstacle.pas:79
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface) => Obstacle.ShowShadow(pixelXSurface, pixelYSurface, lockedSurface);
}
