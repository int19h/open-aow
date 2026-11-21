#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class BlessedEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1211
    public BlessedEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_blessedText);
        AbilityId = 158;
    }

    // PassiveAb.pas:1219
    public override sbyte GetDefense(AbilityOwner owner) => 1;

    // PassiveAb.pas:1225
    public override sbyte GetResistance(AbilityOwner owner) => 1;

    // PassiveAb.pas:1231
    public override string GetSourceName(AbilityOwner owner)
    {
        // { Original assumes the ability data and enchantment exist. }
        UnitEnchantmentAbilityData unitEnchantmentAbilityData = owner.GetAbilityData<UnitEnchantmentAbilityData>(AbilityId);
        return unitEnchantmentAbilityData.Enchantment.Source == -2
            ? AbilityRules.TranslateResourceString(GetEngine(), S: GameText.LifeMasteryText)
            : base.GetSourceName(owner);
    }

    // PassiveAb.pas:1242
    public override int GetAIUpkeepPriority(AbilityOwner owner)
    {
        if (owner is not Troop troop)
        {
            return 0;
        }

        int result = troop.UnitLevel + 200;
        if (troop is Leader)
        {
            result += 50;
        }

        return result;
    }

    private static readonly string s_blessedText = "Blessed";
}
