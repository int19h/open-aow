// Translated from AoWE.pas.
using Aow.Engine.Buffers;

using Aow.Rtl;

namespace Aow.Game.Combat;

// dirty flag. FStrength (ten Integers indexed by TDamageType)
public partial class CombatPredictorSide : CombatPredictorUnitList
{
    public byte[] UnitRemovedFlags { get; } = new byte[4];
    public int[] Strength { get; } = new int[10];

    // CombatPredictor.pas:278
    public void Reset()
    {
        Array.Clear(Strength);
    }

    // CombatPredictor.pas:281
    public int GetStrength(DamageType damageType) => Strength[(int)damageType];

    // CombatPredictor.pas:284
    public int GetStrengthMax(Set<DamageType> damageTypes)
    {
        DamageType D;
        int result = 0;
        for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
        {
            D = (DamageType)dOrdinal;
            if (damageTypes.Contains(D) && result < Strength[(int)D])
            {
                result = Strength[(int)D];
            }
        }

        return result;
    }

    // System.pas:111

}
