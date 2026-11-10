// Translated from BehaviorAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class DefenderAG : AbstractBehaviorAG, IAIGroupClass
{
    // BehaviorAG.pas:277
    public new const int ClassID = 136278;

    // BehaviorAG.pas:270
    public DefenderAG(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 10;
        FRecruitmentPriority = 10;
    }

    // BehaviorAG.pas:283
    public override EClass GroupControlClass() => EClass.Of<DefenderAGC>();

    // BehaviorAG.pas:280
    public static new GroupBehavior Behavior => GroupBehavior.Defensive;

}
