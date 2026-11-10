// Translated from MainAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class SuicidalAG : AIGroup, IAIGroupClass
{
    // MainAG.pas:729
    public new const int ClassID = 136276;

    // MainAG.pas:722
    public SuicidalAG(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 1;
        FRecruitmentPriority = 2;
    }

    // MainAG.pas:735
    public override EClass GroupControlClass() => EClass.Of<SuicidalAGC>();

    // MainAG.pas:732
    public static new GroupBehavior Behavior => GroupBehavior.Suicidal;

}
