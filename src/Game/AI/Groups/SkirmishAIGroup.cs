// Translated from BehaviorAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public partial class SkirmishAIGroup : BehaviorAIGroup, IAIGroupClass
{
    // BehaviorAG.pas:729
    public new const int ClassID = 136294;

    // BehaviorAG.pas:722
    public SkirmishAIGroup(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 12;
        FRecruitmentPriority = 12;
    }

    // BehaviorAG.pas:735
    public override EngineClass GroupControlClass() => EngineClass.Of<SkirmishAIGroupControl>();

    // BehaviorAG.pas:732
    public static new GroupBehavior Behavior => GroupBehavior.Skirmish;

}
