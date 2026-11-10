using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Ruin
{
    public override string CombatMapName(CombatMapSite Site) => SiteCombatMapName(Site, "eRuin00", 2, true);
}
