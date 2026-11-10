// Translated from BehaviorAG.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class AbstractBehaviorAG : AIGroup
{
    public AbstractBehaviorAG(ECustomNode owner) : base(owner)
    { }

    // BehaviorAG.pas:137
    public override bool CooperateWithGroup(AIGroup Group, AIGroupControlTarget Target)
    {
        bool Result = base.CooperateWithGroup(Group, Target);
        return Result || Group.GroupClass.Behavior == GroupBehavior.Fortify;
    }
}
