// Translated from BehaviorAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public partial class NormalAIGroup : BehaviorAIGroup, IAIGroupClass
{
    // BehaviorAG.pas:163
    public new const int ClassID = 136292;

    // BehaviorAG.pas:156
    public NormalAIGroup(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 10;
        FRecruitmentPriority = 10;
    }

    // BehaviorAG.pas:169
    public override EngineClass GroupControlClass() => EngineClass.Of<NormalAIGroupControl>();

    // BehaviorAG.pas:166
    public static new GroupBehavior Behavior => GroupBehavior.Normal;

}
