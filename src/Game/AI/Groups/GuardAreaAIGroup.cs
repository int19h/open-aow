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
    public new const int ClassId = 136273;

    public MapCoordinate GuardLocation;

    // IndependentAG.pas:170
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref GuardLocation.Packed, 50);
    }

    // IndependentAG.pas:153
    public GuardAreaAIGroup(ECustomNode owner) : base(owner)
    {
        GuardLocation.Packed = 16777215;
        ProcessPriority = 1;
        RecruitmentPriority = 2;
    }

    // IndependentAG.pas:167
    public override EngineClass GroupControlClass() => EngineClass.Of<GuardAreaAIGroupControl>();

    // IndependentAG.pas:164
    public static new GroupBehavior Behavior => GroupBehavior.GuardArea;

}
