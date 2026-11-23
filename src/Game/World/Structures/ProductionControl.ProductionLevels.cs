#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.

using Aow.Game.Units;
using Aow.Game.World.Map;
using System.Linq;

namespace Aow.Game.World.Structures;

public partial class ProductionControl
{
    public int GetProduceUnitLevelMax() => MaximumUnitLevel(true);
    public int GetResearchUnitLevelMax() => MaximumUnitLevel(false);

    private int MaximumUnitLevel(bool researched)
    {
        int result = 0;
        foreach (UnitProduction production in UnitProductions.Reverse())
        {
            if (production.Researched != researched || production.UnitIndex < 0)
            {
                continue;
            }

            UnitResource resource = GamePack.Current.UnitResources.GetUnitResource(production.UnitIndex);
            if (resource is not null && result < resource.Level)
            {
                result = resource.Level;
            }
        }
        return result;
    }
}
