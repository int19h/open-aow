using Aow.Rtl;
using Aow.Units.AoWE;

namespace Aow.Units.City;

public partial class TCity
{
    public void GenerateRebelUnits(TUnitList units)
    {
        int saved = Aow.Rtl.Rtl.RandSeed;
        Aow.Rtl.Rtl.RandSeed = unchecked(GetXhx() * 10 + AoWE.AoWE.AoWHSMap.FInitialRandomSeed + GetYhx());
        AoWE.AoWE.FillWithRandomUnits(AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace).FUnitIndexes, units, 2, GetSize() * 25, TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature));
        // The source restores only after a successful fill.
        Aow.Rtl.Rtl.RandSeed = saved;
    }
}
