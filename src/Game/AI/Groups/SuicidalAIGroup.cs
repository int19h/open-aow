// Translated from MainAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public partial class SuicidalAIGroup : AIGroup, IAIGroupClass
{
    // MainAG.pas:729
    public new const int ClassId = 136276;

    // MainAG.pas:722
    public SuicidalAIGroup(ECustomNode owner) : base(owner)
    {
        ProcessPriority = 1;
        RecruitmentPriority = 2;
    }

    // MainAG.pas:735
    public override EngineClass GroupControlClass() => EngineClass.Of<SuicidalAIGroupControl>();

    // MainAG.pas:732
    public static new GroupBehavior Behavior => GroupBehavior.Suicidal;

}
