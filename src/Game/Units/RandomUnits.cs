using Aow.Engine.Collections;
using Aow.Game.World.Map;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Units;

/// <summary>
/// Fills a unit collection with random units of a budget (PlayerC.PAS <c>FillWithRandomUnits</c>).
/// </summary>
internal static class RandomUnits
{
    // PlayerC.PAS:2263
    internal static int FillWithRandomUnits(Int32List indexes, UnitCollection units, int level, int value, UnitTypes types)
    {
        bool CanUse(Unit unit)
        {
            bool condition = unit.UnitResource is not null;
            if (condition)
            {
                condition = types.Contains(unit.GetUnitType().ToFlag());
            }

            return condition && unit.UnitLevel <= level && units.CanAddUnit(unit);
        }

        Unit testUnit = new(null) { Holder = units.Holder };
        GamePack gamePack = GamePack.Current!;
        int result = 0;
        using (units.Holder.BeginUpdate())
        {
            int tries = 50;
            while (result < value && tries != 0)
            {
                tries -= 1;
                int index = indexes[Random(indexes.Count)];
                if (index != -1)
                {
                    testUnit.SetUnitResource(gamePack.UnitResources.GetUnitResource(index));
                    if (CanUse(testUnit))
                    {
                        if (testUnit.GetObtainValue() + result > value)
                        {
                            int bestIndex = index;
                            int i = 20;
                            int difference = Math.Abs(testUnit.GetObtainValue() - (value - result));
                            do
                            {
                                index = indexes[Random(indexes.Count)];
                                if (index != -1 && index != bestIndex)
                                {
                                    testUnit.SetUnitResource(gamePack.UnitResources.GetUnitResource(index));
                                    if (CanUse(testUnit))
                                    {
                                        int delta = Math.Abs(testUnit.GetObtainValue() - (value - result));
                                        if (delta < difference)
                                        {
                                            bestIndex = index;
                                            difference = delta;
                                        }
                                    }
                                }

                                i -= 1;
                            }
                            while (!(i == 0));
                            index = bestIndex;
                        }

                        Unit unit = new(null) { Holder = units.Holder };
                        unit.SetUnitResource(gamePack.UnitResources.GetUnitResource(index));
                        _ = units.AddUnit(unit);
                        result += unit.GetObtainValue();
                        tries = 50;
                        _ = unit.Release();
                    }
                }
            }
        }
        _ = testUnit.Release();
        return result;
    }
}
