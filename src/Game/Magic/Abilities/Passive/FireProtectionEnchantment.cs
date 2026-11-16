// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class FireProtectionEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1054
    public FireProtectionEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), FireProtectionText);
        AbilityId = 167;
    }

    // PassiveAb.pas:1062
    public override string GetSourceName(AbilityOwner owner)
    {
        string result;
        // { Original assumes the ability data and enchantment exist. }
        UnitEnchantmentAbilityData unitEnchantmentAbilityData = owner.GetAbilityData<UnitEnchantmentAbilityData>(AbilityId);
        result = unitEnchantmentAbilityData.Enchantment.Source == -2
            ? AbilityRules.TranslateResourceString(GetEngine(), GameText.FireMasteryText)
            : base.GetSourceName(owner);

        return result;
    }

    internal static string FireProtectionText { get; } = "Fire Protection";
}
