// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class EnchantedWeaponAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:1073
    public EnchantedWeaponAbility(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(s_enchantedWeaponText);
        AbilityId = 154;
    }

    // PassiveAb.pas:1081
    public override sbyte GetAttack(AbilityOwner owner) => 1;

    // PassiveAb.pas:1087
    public override sbyte GetDamage(AbilityOwner owner) => 1;

    // PassiveAb.pas:1093
    public override int GetAIUpkeepPriority(AbilityOwner owner)
    {
        if (owner is not Troop troop)
        {
            return 0;
        }

        int result = troop.GetUnitLevel() + 225;
        if (troop is Leader)
        {
            result += 50;
        }

        return result;
    }

    private static readonly string s_enchantedWeaponText = "Enchanted Weapon";
}
