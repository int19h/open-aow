using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Crypt
{
    public override string CombatMapName(CombatMapSite site) => SiteCombatMapName(site, prefix: "eCrpt00", variants: 2, takesItems: true);
}
