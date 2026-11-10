// Translated from IndependentAG.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class GuardAG : AIGroup, IAIGroupClass
{
    // IndependentAG.pas:144
    public new const int ClassID = 136275;

    // IndependentAG.pas:137
    public GuardAG(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 1;
        FRecruitmentPriority = 2;
    }

    // IndependentAG.pas:150
    public override EClass GroupControlClass() => EClass.Of<GuardAGC>();

    // IndependentAG.pas:147
    public static new GroupBehavior Behavior => GroupBehavior.Guard;

}
