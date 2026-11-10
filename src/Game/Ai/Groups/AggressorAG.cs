// Translated from BehaviorAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class AggressorAG : AbstractBehaviorAG, IAIGroupClass
{
    // BehaviorAG.pas:390
    public new const int ClassID = 136279;

    // BehaviorAG.pas:383
    public AggressorAG(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 10;
        FRecruitmentPriority = 10;
    }

    // BehaviorAG.pas:396
    public override EClass GroupControlClass() => EClass.Of<AggressorAGC>();

    // BehaviorAG.pas:393
    public static new GroupBehavior Behavior => GroupBehavior.Aggressive;

}
