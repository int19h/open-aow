// Translated from BehaviorAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public partial class AggressorAIGroup : BehaviorAIGroup, IAIGroupClass
{
    // BehaviorAG.pas:390
    public new const int ClassId = 136279;

    // BehaviorAG.pas:383
    public AggressorAIGroup(ECustomNode owner) : base(owner)
    {
        ProcessPriority = 10;
        RecruitmentPriority = 10;
    }

    // BehaviorAG.pas:396
    public override EngineClass GroupControlClass() => EngineClass.Of<AggressorAIGroupControl>();

    // BehaviorAG.pas:393
    public static new GroupBehavior Behavior => GroupBehavior.Aggressive;

}
