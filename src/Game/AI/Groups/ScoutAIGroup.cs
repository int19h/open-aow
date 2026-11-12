// Translated from IndependentAG.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public partial class ScoutAIGroup : AIGroup, IAIGroupClass
{
    // IndependentAG.pas:650
    public new const int ClassId = 136277;

    // IndependentAG.pas:643
    public ScoutAIGroup(ECustomNode owner) : base(owner)
    {
        ProcessPriority = 10;
        RecruitmentPriority = 10;
    }

    // IndependentAG.pas:656
    public override EngineClass GroupControlClass() => EngineClass.Of<ScoutAIGroupControl>();

    // IndependentAG.pas:653
    public static new GroupBehavior Behavior => GroupBehavior.Scout;

}
