// Translated from BehaviorAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class SkirmishAG : AbstractBehaviorAG, IAIGroupClass
{
    // BehaviorAG.pas:729
    public new const int ClassID = 136294;

    // BehaviorAG.pas:722
    public SkirmishAG(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 12;
        FRecruitmentPriority = 12;
    }

    // BehaviorAG.pas:735
    public override EClass GroupControlClass() => EClass.Of<SkirmishAGC>();

    // BehaviorAG.pas:732
    public static new GroupBehavior Behavior => GroupBehavior.Skirmish;

}
