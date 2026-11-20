// Translated from IndependentAG.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

// from, to, previous heading, strength
public class PatrolAIGroup : AIGroup, IAIGroupClass
{
    // IndependentAG.pas:295
    public new const int ClassId = 136_274;

    public MapCoordinate PatrolFrom { get; internal set; }
    public MapCoordinate PatrolTo { get; internal set; }

    public sbyte PreviousHeading { get; set; }
    public int PatrolStrength { get; set; }

    // IndependentAG.pas:304
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        int from = persister.Int32("PatrolFrom", value: StoredCoordinateImage.From(PatrolFrom).ToInt32());
        PatrolFrom = StoredCoordinateImage.FromInt32(from).Location;
        int to = persister.Int32("PatrolTo", value: StoredCoordinateImage.From(PatrolTo).ToInt32());
        PatrolTo = StoredCoordinateImage.FromInt32(to).Location;
        PreviousHeading = PersistSupport.UnsignedByte(persister, name: "PreviousHeading", value: PreviousHeading);
        PatrolStrength = persister.Int32("PatrolStrength", value: PatrolStrength);
    }

    // IndependentAG.pas:285
    public PatrolAIGroup(CustomNode owner) : base(owner)
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
