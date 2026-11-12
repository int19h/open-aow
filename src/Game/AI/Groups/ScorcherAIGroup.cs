// Translated from BehaviorAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public partial class ScorcherAIGroup : BehaviorAIGroup, IAIGroupClass
{
    // BehaviorAG.pas:616
    public new const int ClassId = 136291;

    // BehaviorAG.pas:609
    public ScorcherAIGroup(ECustomNode owner) : base(owner)
    {
        ProcessPriority = 10;
        RecruitmentPriority = 10;
    }

    // BehaviorAG.pas:622
    public override EngineClass GroupControlClass() => EngineClass.Of<ScorcherAIGroupControl>();

    // BehaviorAG.pas:619
    public static new GroupBehavior Behavior => GroupBehavior.Scorch;

}
