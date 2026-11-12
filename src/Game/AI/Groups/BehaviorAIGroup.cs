// Translated from BehaviorAG.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public partial class BehaviorAIGroup : AIGroup
{
    public BehaviorAIGroup(ECustomNode owner) : base(owner)
    { }

    // BehaviorAG.pas:137
    public override bool CooperateWithGroup(AIGroup group, AIGroupControlTarget target)
    {
        bool result = base.CooperateWithGroup(group, target);
        return result || group.GroupClass.Behavior == GroupBehavior.Fortify;
    }
}
