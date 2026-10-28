using Aow.Rtl;

namespace Aow.Units.AoWE;

public partial class TAbstractUnit
{
    // Unit.PAS:1715. Native hard casts retain identity across the intermediate local type.
    public TSet<TDamageType> ExecuteCombatDamageEffects(TCombat combat, TSet<TDamageType> damageTypes)
    {
        TSet<TDamageType> result = default;
        var active = damageTypes - GetImmunityTypes();
        if (active == default(TSet<TDamageType>)) return result;
        if (active.Contains(TDamageType.dtFire) && ExpandAbility(127))
        {
            RemoveAbility(95);
            result += TSet<TDamageType>.Of(TDamageType.dtFire);
        }
        if (active.Contains(TDamageType.dtCold) && ExpandAbility(95))
        {
            var data = (TUnitEnchantmentAbilityData)GetAbilityData(95);
            if (data != null) data.FEnchantmentPlayerIndex = (sbyte)combat.FCombatData.FCurrentPlayer;
            RemoveAbility(127);
            result += TSet<TDamageType>.Of(TDamageType.dtCold);
        }
        if (active.Contains(TDamageType.dtLightning) && ExpandAbility(92))
        {
            var data = (TDurationAbilityData)GetAbilityData(92);
            if (data != null) data.FPlayerIndex = (byte)combat.FCombatData.FCurrentPlayer;
            result += TSet<TDamageType>.Of(TDamageType.dtLightning);
        }
        if (active.Contains(TDamageType.dtPoison) && ExpandAbility(96))
            result += TSet<TDamageType>.Of(TDamageType.dtPoison);
        if (active.Contains(TDamageType.dtDeath) && ExpandAbility(93))
            result += TSet<TDamageType>.Of(TDamageType.dtDeath);
        if (active.Contains(TDamageType.dtHoly) && ExpandAbility(98))
            result += TSet<TDamageType>.Of(TDamageType.dtHoly);
        return result;
    }
}
