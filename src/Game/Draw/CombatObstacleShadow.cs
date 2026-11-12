// Translated from CombatObstacle.pas.
using Aow.Engine.Nodes;

using Aow.Game.World;

namespace Aow.Game.Draw;

public partial class CombatObstacleShadow : HexSprite
{
    public CombatObstacleShadow(ECustomNode owner) : base(owner)
    { }

    public CombatObstacle Obstacle { get; set; }

    // CombatObstacle.pas:74
    public override int GetShowPriority() => (Obstacle.GetShowPriority() & 67108863) - 1677721600;

    // CombatObstacle.pas:79
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        Obstacle.ShowShadow(pixelXSurface, pixelYSurface, lockedSurface);
    }

    // Engine.pas:524

}
