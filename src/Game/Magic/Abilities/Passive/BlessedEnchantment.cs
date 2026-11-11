// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class BlessedEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1211
    public BlessedEnchantment(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(BlessedRStr);
        FAbilityID = 158;
    }

    // PassiveAb.pas:1219
    public override sbyte GetDefense(AbilityOwner Owner) => 1;

    // PassiveAb.pas:1225
    public override sbyte GetResistance(AbilityOwner Owner) => 1;

    // PassiveAb.pas:1231
    public override string GetSourceName(AbilityOwner Owner)
    {
        string Result;
        // { Original assumes the ability data and enchantment exist. }
        var unitEnchantmentAbilityData = (UnitEnchantmentAbilityData)Owner.GetAbilityData(FAbilityID);
        if (unitEnchantmentAbilityData.FEnchantment.FSource == -2)
        {
            Result = AbilityRules.TranslateRStr(GameText.LifeMasteryRStr);
        }
        else
        {
            Result = base.GetSourceName(Owner);
        }

        return Result;
    }

    // PassiveAb.pas:1242
    public override int GetAIUpkeepPriority(AbilityOwner Owner)
    {
        if (Owner is not Troop unit)
        {
            return 0;
        }

        int Result = unit.GetUnitLevel() + 200;
        if (unit is Leader)
        {
            Result += 50;
        }

        return Result;
    }

    private static readonly string BlessedRStr = "Blessed";
}
