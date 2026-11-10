// Translated from BehaviorAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class ScorcherAG : AbstractBehaviorAG, IAIGroupClass
{
    // BehaviorAG.pas:616
    public new const int ClassID = 136291;

    // BehaviorAG.pas:609
    public ScorcherAG(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 10;
        FRecruitmentPriority = 10;
    }

    // BehaviorAG.pas:622
    public override EClass GroupControlClass() => EClass.Of<ScorcherAGC>();

    // BehaviorAG.pas:619
    public static new GroupBehavior Behavior => GroupBehavior.Scorch;

}
