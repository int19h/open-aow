using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Teleport
{
    public override StructureView InfoPanel(sbyte viewer) => StatePanel() ?? StructureView.Teleport;

    public override StructureView InfoButton(sbyte viewer)
    {
        TeleportPlayer(viewer);
        return StructureView.None;
    }

    // The original read FRandomTerrain here. It equals RandomTerrain at this point, because the
    // only other writer is the bridge arm of the generator, and a bridge is no structure.
    public override string CombatMapName(CombatMapSite site)
    {
        string stem = site.RandomTerrain ? "TeleN000" : "Telep000";
        site.RandomTerrain = true;
        return stem;
    }
}
