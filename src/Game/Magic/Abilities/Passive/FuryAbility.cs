#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class FuryAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:1160
    public FuryAbility(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.FuryText);
        AbilityId = 156;
    }

    // PassiveAb.pas:1168
    public override int GetAttack(AbilityOwner owner) => 2;

    // PassiveAb.pas:1174
    public override int GetDefense(AbilityOwner owner) => -1;

    // PassiveAb.pas:1180
    public override int GetAIUpkeepPriority(AbilityOwner owner)
    {
        if (owner is not Troop troop)
        {
            return 0;
        }

        int result = troop.UnitLevel + 200;
        if (troop is Leader)
        {
            result -= 50;
        }

        return result;
    }
}
