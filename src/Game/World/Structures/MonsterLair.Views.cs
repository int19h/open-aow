using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class MonsterLair
{
    public override string CombatMapName(CombatMapSite site) => SiteCombatMapName(site, "eMrLr00", 4, true);
}
