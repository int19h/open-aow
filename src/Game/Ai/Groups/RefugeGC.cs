// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class RefugeGC : GroupControl
{
    // AIGroup.pas:1630
    public new const int ClassID = 136208;

    public RefugeGC(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:1635
    public override GroupBehavior Behavior() => GroupBehavior.Refuge;

}
