// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class StoneSkinAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:1029
    public StoneSkinAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), GameText.StoneSkinText);
        AbilityId = 153;
    }

    // PassiveAb.pas:1048
    public override sbyte GetDefense(AbilityOwner owner) => 2;

    // PassiveAb.pas:1037
    public override string GetSourceName(AbilityOwner owner)
    {
        string result;
        // { Original assumes the ability data and enchantment exist. }
        UnitEnchantmentAbilityData unitEnchantmentAbilityData = owner.GetAbilityData<UnitEnchantmentAbilityData>(AbilityId);
        result = unitEnchantmentAbilityData.Enchantment.Source == -2
            ? AbilityRules.TranslateResourceString(GetEngine(), GameText.EarthMasteryText)
            : base.GetSourceName(owner);

        return result;
    }
}
