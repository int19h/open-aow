// Translated from IndependentAG.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

// from, to, previous heading, strength
public partial class PatrolAIGroup : AIGroup, IAIGroupClass
{
    // IndependentAG.pas:295
    public new const int ClassId = 136274;

    public MapCoordinate PatrolFrom;
    public MapCoordinate PatrolTo;

    // The fourth bytes of the packed locations stored at ids 40 (low byte) and 41 (high byte), 0 from
    // every writer and in every corpus file, kept so that a loaded value saves back unchanged.
    public ushort UnknownBytes { get; set; }

    public sbyte PreviousHeading { get; set; }
    public int PatrolStrength { get; set; }

    // IndependentAG.pas:304
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        int from = StoredCoordinateImage.From(PatrolFrom, unknownBytes: (byte)UnknownBytes).ToInt32();
        from = persister.Int32("PatrolFrom", value: from);
        (PatrolFrom, var fromUnknown) = StoredCoordinateImage.FromInt32(from);
        int to = StoredCoordinateImage.From(PatrolTo, unknownBytes: (byte)(UnknownBytes >> 8)).ToInt32();
        to = persister.Int32("PatrolTo", value: to);
        (PatrolTo, var toUnknown) = StoredCoordinateImage.FromInt32(to);
        UnknownBytes = (ushort)(fromUnknown | (toUnknown << 8));
        PreviousHeading = PersistSupport.UnsignedByte(persister, name: "PreviousHeading", value: PreviousHeading);
        PatrolStrength = persister.Int32("PatrolStrength", value: PatrolStrength);
    }

    // IndependentAG.pas:285
    public PatrolAIGroup(ECustomNode owner) : base(owner)
    {
        ProcessPriority = 1;
        RecruitmentPriority = 2;
        PatrolFrom = MapCoordinate.None;
        PatrolTo = MapCoordinate.None;
        PreviousHeading = 0;
    }

    // IndependentAG.pas:301
    public override EngineClass GroupControlClass() => EngineClass.Of<PatrolAIGroupControl>();

    // IndependentAG.pas:298
    public static new GroupBehavior Behavior => GroupBehavior.Patrol;

}
