using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Crypt
{
    public override string CombatMapName(CombatMapSite Site) => SiteCombatMapName(Site, "eCrpt00", 2, true);
}
