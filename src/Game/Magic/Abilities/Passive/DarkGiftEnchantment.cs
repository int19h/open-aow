#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// (name, ID, selection types, ability type.)
public class DarkGiftEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1249
    public DarkGiftEnchantment(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.DarkGiftText);
        AbilityId = 169;
    }

    // PassiveAb.pas:1256
    public override int GetDamage(AbilityOwner owner) => 1;

    // PassiveAb.pas:1261
    public override int GetAIUpkeepPriority(AbilityOwner owner)
    {
        if (owner is not Troop troop)
        {
            return 0;
        }

        int result = troop.UnitLevel + 200;
        if (troop is Leader)
        {
            result += 50;
        }

        return result;
    }
}
