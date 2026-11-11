// Translated from BehaviorAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public partial class ExpanderAIGroup : BehaviorAIGroup, IAIGroupClass
{
    // BehaviorAG.pas:502
    public new const int ClassID = 136290;

    // BehaviorAG.pas:495
    public ExpanderAIGroup(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 10;
        FRecruitmentPriority = 10;
    }

    // BehaviorAG.pas:508
    public override EngineClass GroupControlClass() => EngineClass.Of<ExpanderAIGroupControl>();

    // BehaviorAG.pas:505
    public static new GroupBehavior Behavior => GroupBehavior.Expand;

}
