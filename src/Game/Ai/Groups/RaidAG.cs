// Translated from IndependentAG.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class RaidAG : AIGroup, IAIGroupClass
{
    // IndependentAG.pas:567
    public new const int ClassID = 136293;

    // IndependentAG.pas:560
    public RaidAG(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 1;
        FRecruitmentPriority = 2;
    }

    // IndependentAG.pas:573
    public override EClass GroupControlClass() => EClass.Of<RaidAGC>();

    // IndependentAG.pas:570
    public static new GroupBehavior Behavior => GroupBehavior.Raid;

}
