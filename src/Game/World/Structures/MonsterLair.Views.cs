using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class MonsterLair
{
    public override string CombatMapName(CombatMapSite Site) => SiteCombatMapName(Site, "eMrLr00", 4, true);
}
