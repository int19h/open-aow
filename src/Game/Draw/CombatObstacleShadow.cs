// Translated from CombatObstacle.pas.
using Aow.Engine.Nodes;

using Aow.Game.World;

namespace Aow.Game.Draw;

public partial class CombatObstacleShadow : HexagonSprite
{
    public CombatObstacleShadow(ECustomNode owner) : base(owner)
    { }

    public CombatObstacle FObstacle;

    // CombatObstacle.pas:74
    public override int GetShowPriority() => (FObstacle.GetShowPriority() & 67108863) - 1677721600;

    // CombatObstacle.pas:79
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        FObstacle.ShowShadow(XhpSurf, YhpSurf, SurfaceDesc);
    }

    // Engine.pas:524

}
