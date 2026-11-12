// Translated from BehaviorAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public partial class DefenderAIGroup : BehaviorAIGroup, IAIGroupClass
{
    // BehaviorAG.pas:277
    public new const int ClassId = 136278;

    // BehaviorAG.pas:270
    public DefenderAIGroup(ECustomNode owner) : base(owner)
    {
        ProcessPriority = 10;
        RecruitmentPriority = 10;
    }

    // BehaviorAG.pas:283
    public override EngineClass GroupControlClass() => EngineClass.Of<DefenderAIGroupControl>();

    // BehaviorAG.pas:280
    public static new GroupBehavior Behavior => GroupBehavior.Defensive;

}
