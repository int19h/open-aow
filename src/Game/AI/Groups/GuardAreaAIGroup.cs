// Translated from IndependentAG.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

// guard location
public partial class GuardAreaAIGroup : AIGroup, IAIGroupClass
{
    // IndependentAG.pas:161
    public new const int ClassID = 136273;

    public MapCoordinate FGuardLocation;

    // IndependentAG.pas:170
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FGuardLocation.Packed, 50);
    }

    // IndependentAG.pas:153
    public GuardAreaAIGroup(ECustomNode owner) : base(owner)
    {
        FGuardLocation.Packed = 16777215;
        FProcessPriority = 1;
        FRecruitmentPriority = 2;
    }

    // IndependentAG.pas:167
    public override EngineClass GroupControlClass() => EngineClass.Of<GuardAreaAIGroupControl>();

    // IndependentAG.pas:164
    public static new GroupBehavior Behavior => GroupBehavior.GuardArea;

}
