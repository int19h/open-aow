using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Pyramid
{
    public override string CombatMapName(CombatMapSite site) => SiteCombatMapName(site, "eZigg00", 2, true);
}
