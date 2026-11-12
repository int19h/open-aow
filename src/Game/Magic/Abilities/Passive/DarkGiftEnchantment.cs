// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// (name, ID, selection types, ability type.)
public partial class DarkGiftEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1249
    public DarkGiftEnchantment(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(GameText.DarkGiftText);
        AbilityId = 169;
    }

    // PassiveAb.pas:1256
    public override sbyte GetDamage(AbilityOwner owner) => 1;

    // PassiveAb.pas:1261
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
}
