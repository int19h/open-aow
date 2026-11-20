
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

public partial class ProductionControl
{
    public int GetProduceUnitLevelMax() => MaximumUnitLevel(true);
    public int GetResearchUnitLevelMax() => MaximumUnitLevel(false);

    private int MaximumUnitLevel(bool researched)
    {
        int result = 0;
        for (int i = UnitProductions.Count - 1; i >= 0; i--)
        {
            UnitProduction production = UnitProductions.GetItem(i);
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
