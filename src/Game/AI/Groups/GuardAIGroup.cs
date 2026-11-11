// Translated from IndependentAG.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public partial class GuardAIGroup : AIGroup, IAIGroupClass
{
    // IndependentAG.pas:144
    public new const int ClassID = 136275;

    // IndependentAG.pas:137
    public GuardAIGroup(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 1;
        FRecruitmentPriority = 2;
    }

    // IndependentAG.pas:150
    public override EngineClass GroupControlClass() => EngineClass.Of<GuardAIGroupControl>();

    // IndependentAG.pas:147
    public static new GroupBehavior Behavior => GroupBehavior.Guard;

}
