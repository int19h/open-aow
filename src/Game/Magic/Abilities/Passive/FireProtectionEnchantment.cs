// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class FireProtectionEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1054
    public FireProtectionEnchantment(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(s_fireProtectionText);
        AbilityId = 167;
    }

    // PassiveAb.pas:1062
    public override string GetSourceName(AbilityOwner owner)
    {
        string result;
        // { Original assumes the ability data and enchantment exist. }
        var unitEnchantmentAbilityData = (UnitEnchantmentAbilityData)owner.GetAbilityData(AbilityId);
        if (unitEnchantmentAbilityData.Enchantment.Source == -2)
        {
            result = AbilityRules.TranslateResourceString(GameText.s_fireMasteryText);
        }
        else
        {
            result = base.GetSourceName(owner);
        }

        return result;
    }

    internal static readonly string s_fireProtectionText = "Fire Protection";
}
