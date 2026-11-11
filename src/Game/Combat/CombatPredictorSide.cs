// Translated from AoWE.pas.
using Aow.Engine.Buffers;

using Aow.Rtl;

namespace Aow.Game.Combat;

// dirty flag. FStrength (ten Integers indexed by TDamageType)
public partial class CombatPredictorSide : CombatPredictorUnitList
{
    public ByteArray4 F_CombatPredictorSide_10;
    public Int32Array10 FStrength;

    // CombatPredictor.pas:278
    public void Reset()
    {
        FStrength = default;
    }

    // CombatPredictor.pas:281
    public int GetStrength(DamageType DamageType) => FStrength[(int)DamageType];

    // CombatPredictor.pas:284
    public int GetStrengthMax(Set<DamageType> DamageTypes)
    {
        DamageType D;
        int Result = 0;
        for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
        {
            D = (DamageType)dOrdinal;
            if (DamageTypes.Contains(D) && Result < FStrength[(int)D])
            {
                Result = FStrength[(int)D];
            }
        }

        return Result;
    }

    // System.pas:111

}
