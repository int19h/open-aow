// Translated from BehaviorAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class ExpanderAG : AbstractBehaviorAG, IAIGroupClass
{
    // BehaviorAG.pas:502
    public new const int ClassID = 136290;

    // BehaviorAG.pas:495
    public ExpanderAG(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 10;
        FRecruitmentPriority = 10;
    }

    // BehaviorAG.pas:508
    public override EClass GroupControlClass() => EClass.Of<ExpanderAGC>();

    // BehaviorAG.pas:505
    public static new GroupBehavior Behavior => GroupBehavior.Expand;

}
