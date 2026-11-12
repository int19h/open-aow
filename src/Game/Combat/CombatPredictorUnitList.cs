// Translated from AoWE.pas.
using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Combat;

public partial class CombatPredictorUnitList : DelphiList<CombatPredictorUnit>
{
    // CombatPredictor.pas:253
    public CombatPredictorUnit GetUnits(int index) => List[index];

    // CombatPredictor.pas:256
    public int IndexOfUnit(Troop troop)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (List[result].Troop == troop)
            {
                return result;
            }
        }

        return -1;
    }

    // CombatPredictor.pas:267
    public int IndexOfCpuId(int cpuId)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (List[result].CpuId == cpuId)
            {
                return result;
            }
        }

        return -1;
    }

    // System.pas:111

}
