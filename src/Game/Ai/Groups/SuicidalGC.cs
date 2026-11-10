// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class SuicidalGC : GroupControl
{
    // AIGroup.pas:1600
    public new const int ClassID = 136195;

    public SuicidalGC(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:1605
    public override GroupBehavior Behavior() => GroupBehavior.Suicidal;

}
