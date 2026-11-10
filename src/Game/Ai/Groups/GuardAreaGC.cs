// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class GuardAreaGC : GroupControl
{
    // AIGroup.pas:1590
    public new const int ClassID = 136192;

    public GuardAreaGC(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:1595
    public override GroupBehavior Behavior() => GroupBehavior.GuardArea;

}
