using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Cave
{
    public override StructureView InfoPanel(sbyte viewer) => StatePanel() ?? StructureView.Cave;

    public override StructureView InfoButton(sbyte viewer)
    {
        Enter(viewer);
        return StructureView.None;
    }

    public override string CombatMapName(CombatMapSite site) => site.RandomTerrain ? "CaveN000" : "CaveE000";
}
