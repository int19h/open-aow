// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class FireProtectionEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1054
    public FireProtectionEnchantment(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(FireProtectionRStr);
        FAbilityID = 167;
    }

    // PassiveAb.pas:1062
    public override string GetSourceName(AbilityOwner Owner)
    {
        string Result;
        // { Original assumes the ability data and enchantment exist. }
        var unitEnchantmentAbilityData = (UnitEnchantmentAbilityData)Owner.GetAbilityData(FAbilityID);
        if (unitEnchantmentAbilityData.FEnchantment.FSource == -2)
        {
            Result = AbilityRules.TranslateRStr(GameText.FireMasteryRStr);
        }
        else
        {
            Result = base.GetSourceName(Owner);
        }

        return Result;
    }

    internal static readonly string FireProtectionRStr = "Fire Protection";
}
