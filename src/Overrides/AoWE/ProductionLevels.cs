namespace Aow.Units.AoWE;

public partial class TProductionControl
{
    public int GetProduceUnitLevelMax() => MaximumUnitLevel(true);
    public int GetResearchUnitLevelMax() => MaximumUnitLevel(false);

    private int MaximumUnitLevel(bool researched)
    {
        int result = 0;
        for (int i = FUnitProductions.Count - 1; i >= 0; i--)
        {
            var production = FUnitProductions.GetItem(i);
            if (production.FResearched != researched || production.FUnitIndex < 0) continue;
            var resource = AoWE.AoWHSSet.FUnitResources.GetUnitResource(production.FUnitIndex);
            if (resource != null && result < resource.FLevel) result = resource.FLevel;
        }
        return result;
    }
}
