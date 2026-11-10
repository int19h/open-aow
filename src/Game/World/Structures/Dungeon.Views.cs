using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Dungeon
{
    // The dungeon fight lays out no items.
    public override string CombatMapName(CombatMapSite Site) => SiteCombatMapName(Site, "eDung00", 2, false);
}
