// Translated from BehaviorAG.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public partial class BehaviorAIGroup : AIGroup
{
    public BehaviorAIGroup(ECustomNode owner) : base(owner)
    { }

    // BehaviorAG.pas:137
    public override bool CooperateWithGroup(AIGroup Group, AIGroupControlTarget Target)
    {
        bool Result = base.CooperateWithGroup(Group, Target);
        return Result || Group.GroupClass.Behavior == GroupBehavior.Fortify;
    }
}
