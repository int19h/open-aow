// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class ScoutGC : GroupControl
{
    // AIGroup.pas:1620
    public new const int ClassID = 136196;

    public ScoutGC(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:1625
    public override GroupBehavior Behavior() => GroupBehavior.Scout;

}
