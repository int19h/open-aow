// Translated from MainAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class FortifyAG : AIGroup, IAIGroupClass
{
    // MainAG.pas:247
    public new const int ClassID = 136281;

    // MainAG.pas:240
    public FortifyAG(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 15;
        FRecruitmentPriority = 0;
    }

    // MainAG.pas:253
    public override EClass GroupControlClass() => EClass.Of<FortifyAGC>();

    // MainAG.pas:250
    public static new GroupBehavior Behavior => GroupBehavior.Fortify;

}
