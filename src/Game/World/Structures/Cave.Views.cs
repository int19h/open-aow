using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Cave
{
    public override StructureView InfoPanel(sbyte Viewer) => StatePanel() ?? StructureView.Cave;

    public override StructureView InfoButton(sbyte Viewer)
    {
        Enter(Viewer);
        return StructureView.None;
    }

    public override string CombatMapName(CombatMapSite Site) => Site.RandomTerrain ? "CaveN000" : "CaveE000";
}
