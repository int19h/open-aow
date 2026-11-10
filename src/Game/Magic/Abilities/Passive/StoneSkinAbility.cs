// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class StoneSkinAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:1029
    public StoneSkinAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(GameText.StoneSkinRStr);
        FAbilityID = 153;
    }

    // PassiveAb.pas:1048
    public override sbyte GetDefense(AbilityOwner Owner) => 2;

    // PassiveAb.pas:1037
    public override string GetSourceName(AbilityOwner Owner)
    {
        string Result;
        // { Original assumes the ability data and enchantment exist. }
        var unitEnchantmentAbilityData = (UnitEnchantmentAbilityData)Owner.GetAbilityData(FAbilityID);
        if (unitEnchantmentAbilityData.FEnchantment.FSource == -2)
        {
            Result = AbilityRules.TranslateRStr(GameText.EarthMasteryRStr);
        }
        else
        {
            Result = base.GetSourceName(Owner);
        }

        return Result;
    }
}
