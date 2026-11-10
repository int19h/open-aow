// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class PassiveGC : GroupControl
{
    // AIGroup.pas:1580
    public new const int ClassID = 136199;

    public PassiveGC(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:1585
    public override GroupBehavior Behavior() => GroupBehavior.Passive;

}
