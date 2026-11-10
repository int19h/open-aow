// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class PatrolGC : GroupControl
{
    // AIGroup.pas:1610
    public new const int ClassID = 136193;

    public PatrolGC(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:1615
    public override GroupBehavior Behavior() => GroupBehavior.Patrol;

}
