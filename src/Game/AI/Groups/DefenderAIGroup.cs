// Translated from BehaviorAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public class DefenderAIGroup : BehaviorAIGroup, IAIGroupClass
{
    // BehaviorAG.pas:277
    public new const int ClassId = 136_278;

    // BehaviorAG.pas:270
    public DefenderAIGroup(CustomNode owner) : base(owner)
    {
        ProcessPriority = 10;
        RecruitmentPriority = 10;
    }

    // BehaviorAG.pas:283
    public override EngineClass GroupControlClass() => EngineClass.Of<DefenderAIGroupControl>();

    // BehaviorAG.pas:280
    public static new GroupBehavior Behavior => GroupBehavior.Defensive;

}
