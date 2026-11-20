using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Dungeon
{
    // The dungeon fight lays out no items.
    public override string CombatMapName(CombatMapSite site) => SiteCombatMapName(site, prefix: "eDung00", variants: 2, takesItems: false);
}
