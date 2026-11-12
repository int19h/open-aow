// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class StoneSkinAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:1029
    public StoneSkinAbility(ECustomNode owner, EngineObject holder) : base(owner)
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
        var unitEnchantmentAbilityData = (UnitEnchantmentAbilityData)owner.GetAbilityData(AbilityId);
        if (unitEnchantmentAbilityData.Enchantment.Source == -2)
        {
            result = AbilityRules.TranslateResourceString(GetEngine(), GameText.EarthMasteryText);
        }
        else
        {
            result = base.GetSourceName(owner);
        }

        return result;
    }
}
