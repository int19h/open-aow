using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class MonsterLair
{
    public override string CombatMapName(CombatMapSite site) => SiteCombatMapName(site, prefix: "eMrLr00", variants: 4, takesItems: true);
}
