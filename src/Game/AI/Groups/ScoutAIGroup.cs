// Translated from IndependentAG.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public partial class ScoutAIGroup : AIGroup, IAIGroupClass
{
    // IndependentAG.pas:650
    public new const int ClassID = 136277;

    // IndependentAG.pas:643
    public ScoutAIGroup(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 10;
        FRecruitmentPriority = 10;
    }

    // IndependentAG.pas:656
    public override EngineClass GroupControlClass() => EngineClass.Of<ScoutAIGroupControl>();

    // IndependentAG.pas:653
    public static new GroupBehavior Behavior => GroupBehavior.Scout;

}
