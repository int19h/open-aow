// Translated from IndependentAG.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

// guard location
public class GuardAreaAIGroup : AIGroup, IAIGroupClass
{
    // IndependentAG.pas:161
    public new const int ClassId = 136_273;

    public MapCoordinate GuardLocation { get; internal set; }

    // IndependentAG.pas:170. Delphi compares FGuardLocation as a full integer, so a non-zero
    // fourth byte on load is refused.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        int location = persister.Int32("GuardLocation", value: StoredCoordinateImage.From(GuardLocation).ToInt32());
        if (persister.Loading)
        {
            GuardLocation = StoredCoordinateImage.FromInt32(location).ReadCheckedLocation("GuardLocation");
        }
    }

    // IndependentAG.pas:153
    public GuardAreaAIGroup(CustomNode owner) : base(owner)
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
