using Aow.Rtl;

namespace Aow.Units.AoWE;

public partial class TAbstractUnit
{
    public void UpdateOffensiveStrength()
    {
        var abilities = AoWE.AoWHSSet.FAbilityControl.FAbilitiesByType[(int)TAbilityType.atCombat];
        FMaxOffensiveStrength = 0;
        FOffensiveStrength = default;
        for (int i = abilities.Count - 1; i >= 0; i--)
        {
            var ability = abilities.GetAbility(i);
            if (!GetAbilityEnabled(ability.FAbilityID)) continue;
            int strength = 0;
            TSet<TDamageType> types = default;
            ability.GetOffensiveStrength(this, ref strength, ref types);
            if (strength > 255) strength = 255;
            if (types == default)
                Rtl.Rtl.Assert(false, $"Ability: {ability.GetName(null)}", @"D:\AoWDev\AoWE\inc\Unit.PAS", 0xE2B4);
            if (types == default) continue;
            if (FMaxOffensiveStrength < strength) FMaxOffensiveStrength = unchecked((byte)strength);
            for (int kind = (int)TDamageType.dtFire; kind <= (int)TDamageType.dtNone; kind++)
                if (types.Contains((TDamageType)kind) && FOffensiveStrength[kind] < strength)
                    FOffensiveStrength[kind] = unchecked((byte)strength);
        }
    }
}
