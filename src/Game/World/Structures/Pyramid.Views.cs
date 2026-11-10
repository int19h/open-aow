using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Pyramid
{
    public override string CombatMapName(CombatMapSite Site) => SiteCombatMapName(Site, "eZigg00", 2, true);
}
