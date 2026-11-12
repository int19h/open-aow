using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Crypt
{
    public override string CombatMapName(CombatMapSite site) => SiteCombatMapName(site, "eCrpt00", 2, true);
}
