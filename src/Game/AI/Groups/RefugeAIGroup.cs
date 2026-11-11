// Translated from IndependentAG.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public partial class RefugeAIGroup : AIGroup, IAIGroupClass
{
    // IndependentAG.pas:551
    public new const int ClassID = 136289;

    // IndependentAG.pas:544
    public RefugeAIGroup(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 1;
        FRecruitmentPriority = 2;
    }

    // IndependentAG.pas:557
    public override EngineClass GroupControlClass() => EngineClass.Of<RefugeAIGroupControl>();

    // IndependentAG.pas:554
    public static new GroupBehavior Behavior => GroupBehavior.Refuge;

}
