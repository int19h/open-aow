// Translated from IndependentAG.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public partial class RaidAIGroup : AIGroup, IAIGroupClass
{
    // IndependentAG.pas:567
    public new const int ClassId = 136293;

    // IndependentAG.pas:560
    public RaidAIGroup(ECustomNode owner) : base(owner)
    {
        ProcessPriority = 1;
        RecruitmentPriority = 2;
    }

    // IndependentAG.pas:573
    public override EngineClass GroupControlClass() => EngineClass.Of<RaidAIGroupControl>();

    // IndependentAG.pas:570
    public static new GroupBehavior Behavior => GroupBehavior.Raid;

}
