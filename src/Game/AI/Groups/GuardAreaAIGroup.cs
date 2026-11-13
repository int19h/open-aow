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

    // The fourth byte of the packed location stored at id 50, 0 from every writer and in every
    // corpus file, kept so that a loaded value saves back unchanged.
    public byte UnknownBytes { get; set; }

    // IndependentAG.pas:170
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        int location = StoredCoordinateImage.From(GuardLocation, unknownBytes: UnknownBytes).ToInt32();
        stream.Int32(ref location, 50);
        (GuardLocation, UnknownBytes) = StoredCoordinateImage.FromInt32(location);
    }

    // IndependentAG.pas:153
    public GuardAreaAIGroup(ECustomNode owner) : base(owner)
    {
        GuardLocation = MapCoordinate.None;
        ProcessPriority = 1;
        RecruitmentPriority = 2;
    }

    // IndependentAG.pas:167
    public override EngineClass GroupControlClass() => EngineClass.Of<GuardAreaAIGroupControl>();

    // IndependentAG.pas:164
    public static new GroupBehavior Behavior => GroupBehavior.GuardArea;

}
