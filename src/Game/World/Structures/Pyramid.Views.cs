using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Pyramid
{
    public override string CombatMapName(CombatMapSite site) => SiteCombatMapName(site, prefix: "eZigg00", variants: 2, takesItems: true);
}
