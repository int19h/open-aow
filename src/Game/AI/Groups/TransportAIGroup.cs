// Translated from MainAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

public partial class TransportAIGroup : AIGroup, IAIGroupClass
{
    // MainAG.pas:801
    public new const int ClassId = 136295;

    // MainAG.pas:789
    public TransportAIGroup(ECustomNode owner) : base(owner)
    {
        ProcessPriority = 2;
        RecruitmentPriority = 20;
    }

    // MainAG.pas:807
    public override EngineClass GroupControlClass() => EngineClass.Of<TransportAIGroupControl>();

    // MainAG.pas:804
    public static new GroupBehavior Behavior => GroupBehavior.Transport;

}
