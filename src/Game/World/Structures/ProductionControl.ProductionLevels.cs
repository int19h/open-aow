
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
            var production = UnitProductions.GetItem(i);
            if (production.Researched != researched || production.UnitIndex < 0) continue;
            var resource = GamePack.Current.UnitResources.GetUnitResource(production.UnitIndex);
            if (resource != null && result < resource.Level) result = resource.Level;
        }
        return result;
    }
}
