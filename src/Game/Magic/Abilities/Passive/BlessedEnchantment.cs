// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class BlessedEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1211
    public BlessedEnchantment(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_blessedText);
        AbilityId = 158;
    }

    // PassiveAb.pas:1219
    public override sbyte GetDefense(AbilityOwner owner) => 1;

    // PassiveAb.pas:1225
    public override sbyte GetResistance(AbilityOwner owner) => 1;

    // PassiveAb.pas:1231
    public override string GetSourceName(AbilityOwner owner)
    {
        string result;
        // { Original assumes the ability data and enchantment exist. }
        var unitEnchantmentAbilityData = (UnitEnchantmentAbilityData)owner.GetAbilityData(AbilityId);
        if (unitEnchantmentAbilityData.Enchantment.Source == -2)
        {
            result = AbilityRules.TranslateResourceString(GetEngine(), GameText.LifeMasteryText);
        }
        else
        {
            result = base.GetSourceName(owner);
        }

        return result;
    }

    // PassiveAb.pas:1242
    public override int GetAIUpkeepPriority(AbilityOwner owner)
    {
        if (owner is not Troop troop)
        {
            return 0;
        }

        int result = troop.GetUnitLevel() + 200;
        if (troop is Leader)
        {
            result += 50;
        }

        return result;
    }

    private static readonly string s_blessedText = "Blessed";
}
