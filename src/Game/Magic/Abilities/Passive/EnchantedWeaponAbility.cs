// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class EnchantedWeaponAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:1073
    public EnchantedWeaponAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(EnchantedWeaponRStr);
        FAbilityID = 154;
    }

    // PassiveAb.pas:1081
    public override sbyte GetAttack(AbilityOwner Owner) => 1;

    // PassiveAb.pas:1087
    public override sbyte GetDamage(AbilityOwner Owner) => 1;

    // PassiveAb.pas:1093
    public override int GetAIUpkeepPriority(AbilityOwner Owner)
    {
        if (Owner is not AbstractUnit unit)
        {
            return 0;
        }

        int Result = unit.GetUnitLevel() + 225;
        if (unit is Leader)
        {
            Result += 50;
        }

        return Result;
    }

    private static readonly string EnchantedWeaponRStr = "Enchanted Weapon";
}
