using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Teleport
{
    public override StructureView InfoPanel(sbyte Viewer) => StatePanel() ?? StructureView.Teleport;

    public override StructureView InfoButton(sbyte Viewer)
    {
        TeleportPlayer(Viewer);
        return StructureView.None;
    }

    // The original read FRandomTerrain here. It equals RandomTerrain at this point, because the
    // only other writer is the bridge arm of the generator, and a bridge is no structure.
    public override string CombatMapName(CombatMapSite Site)
    {
        string Stem = Site.RandomTerrain ? "TeleN000" : "Telep000";
        Site.RandomTerrain = true;
        return Stem;
    }
}
