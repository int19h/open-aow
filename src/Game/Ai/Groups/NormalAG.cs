// Translated from BehaviorAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class NormalAG : AbstractBehaviorAG, IAIGroupClass
{
    // BehaviorAG.pas:163
    public new const int ClassID = 136292;

    // BehaviorAG.pas:156
    public NormalAG(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 10;
        FRecruitmentPriority = 10;
    }

    // BehaviorAG.pas:169
    public override EClass GroupControlClass() => EClass.Of<NormalAGC>();

    // BehaviorAG.pas:166
    public static new GroupBehavior Behavior => GroupBehavior.Normal;

}
