#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class FireProtectionEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1054
    public FireProtectionEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: FireProtectionText);
        AbilityId = 167;
    }

    // PassiveAb.pas:1062
    public override string GetSourceName(AbilityOwner owner)
    {
        // { Original assumes the ability data and enchantment exist. }
        UnitEnchantmentAbilityData unitEnchantmentAbilityData = owner.GetAbilityData<UnitEnchantmentAbilityData>(AbilityId);
        return unitEnchantmentAbilityData.Enchantment.Source == -2
            ? AbilityRules.TranslateResourceString(GetEngine(), S: GameText.FireMasteryText)
            : base.GetSourceName(owner);
    }

    internal static string FireProtectionText { get; } = "Fire Protection";
}
