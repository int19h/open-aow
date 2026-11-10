// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class GuardGC : GroupControl
{
    // AIGroup.pas:1570
    public new const int ClassID = 136194;

    public GuardGC(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:1575
    public override GroupBehavior Behavior() => GroupBehavior.Guard;

}
