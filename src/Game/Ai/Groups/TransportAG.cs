// Translated from MainAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

public partial class TransportAG : AIGroup, IAIGroupClass
{
    // MainAG.pas:801
    public new const int ClassID = 136295;

    // MainAG.pas:789
    public TransportAG(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 2;
        FRecruitmentPriority = 20;
    }

    // MainAG.pas:807
    public override EClass GroupControlClass() => EClass.Of<TransportAGC>();

    // MainAG.pas:804
    public static new GroupBehavior Behavior => GroupBehavior.Transport;

}
