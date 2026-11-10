// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// (name, ID, selection types, ability type.)
public partial class DarkGiftEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1249
    public DarkGiftEnchantment(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(GameText.DarkGiftRStr);
        FAbilityID = 169;
    }

    // PassiveAb.pas:1256
    public override sbyte GetDamage(AbilityOwner Owner) => 1;

    // PassiveAb.pas:1261
    public override int GetAIUpkeepPriority(AbilityOwner Owner)
    {
        if (Owner is not AbstractUnit unit)
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
}
