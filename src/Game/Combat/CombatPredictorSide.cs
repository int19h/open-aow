// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.Combat;

// dirty flag. FStrength (ten Integers indexed by TDamageType)
public class CombatPredictorSide : CombatPredictorUnitList
{
    public byte[] UnitRemovedFlags { get; } = new byte[4];
    public int[] Strength { get; } = new int[10];

    // CombatPredictor.pas:278
    public void Reset() => Array.Clear(Strength);

    // CombatPredictor.pas:281
    public int GetStrength(DamageType damageType) => Strength[(int)damageType];

    // CombatPredictor.pas:284
    public int GetStrengthMax(Set<DamageType> damageTypes)
    {
        int result = 0;
        for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
        {
            DamageType d = (DamageType)dOrdinal;
            if (damageTypes.Contains(d) && result < Strength[(int)d])
            {
                result = Strength[(int)d];
            }
        }

        return result;
    }
}
