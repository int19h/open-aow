#nullable disable
// Translated from CombatObstacle.pas.
using Aow.Engine.Nodes;

using Aow.Game.World;

namespace Aow.Game.Draw;

public sealed class CombatObstacleOverlay(CustomNode owner) : HexSprite(owner)
{
    public CombatObstacle Obstacle { get; init; }

    // CombatObstacle.pas:84
    public override int GetShowPriority() => (Obstacle.GetShowPriority() & 0b0011_1111_1111_1111_1111_1111_1111) + 393_216;

    // CombatObstacle.pas:89
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface) => Obstacle.ShowOverlay(pixelXSurface, pixelYSurface, lockedSurface);
}
