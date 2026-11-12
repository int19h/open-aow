using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Ruin
{
    public override string CombatMapName(CombatMapSite site) => SiteCombatMapName(site, "eRuin00", 2, true);
}
