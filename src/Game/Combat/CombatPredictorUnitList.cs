// Translated from AoWE.pas.
using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Combat;

public partial class CombatPredictorUnitList : DelphiList<CombatPredictorUnit>
{
    // CombatPredictor.pas:253
    public CombatPredictorUnit GetUnits(int Index) => List[Index];

    // CombatPredictor.pas:256
    public int IndexOfUnit(AbstractUnit AUnit)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (List[Result].FUnit == AUnit)
            {
                return Result;
            }
        }

        return -1;
    }

    // CombatPredictor.pas:267
    public int IndexOfCPUID(int CPUID)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (List[Result].FCPUID == CPUID)
            {
                return Result;
            }
        }

        return -1;
    }

    // System.pas:111

}
