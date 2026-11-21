// Translated from AoWE.pas.

namespace Aow.Game.Combat;

// dirty flag. FStrength (ten Integers indexed by TDamageType)
public sealed class CombatPredictorSide : CombatPredictorUnitList
{
    public byte[] UnitRemovedFlags { get; } = new byte[4];
    public int[] Strength { get; } = new int[10];

    // CombatPredictor.pas:278
    public void Reset() => Array.Clear(Strength);

    // CombatPredictor.pas:281
    public int GetStrength(DamageType damageType) => Strength[(int)damageType];

    // CombatPredictor.pas:284
    public int GetStrengthMax(DamageTypes damageTypes)
    {
        int result = 0;
        for (int dOrdinal = (int)DamageType.Fire; dOrdinal <= (int)DamageType.NonSpecific; dOrdinal++)
        {
            DamageType d = (DamageType)dOrdinal;
            if (damageTypes.Contains(d.ToFlag()) && result < Strength[(int)d])
            {
                result = Strength[(int)d];
            }
        }

        return result;
    }
}
