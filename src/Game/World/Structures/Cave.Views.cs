using Aow.Game.Combat;
using Aow.Game.Session;

namespace Aow.Game.World.Structures;

public partial class Cave
{
    public override StructureView InfoPanel(PlayerId? viewer) => StatePanel() ?? StructureView.Cave;

    public override StructureView InfoButton(PlayerId? viewer)
    {
        _ = Enter(viewer);
        return StructureView.None;
    }

    public override string CombatMapName(CombatMapSite site) => site.RandomTerrain ? "CaveN000" : "CaveE000";
}
