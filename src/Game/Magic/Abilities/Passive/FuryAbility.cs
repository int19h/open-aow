// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class FuryAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:1160
    public FuryAbility(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), GameText.FuryText);
        AbilityId = 156;
    }

    // PassiveAb.pas:1168
    public override sbyte GetAttack(AbilityOwner owner) => 2;

    // PassiveAb.pas:1174
    public override sbyte GetDefense(AbilityOwner owner) => -1;

    // PassiveAb.pas:1180
    public override int GetAIUpkeepPriority(AbilityOwner owner)
    {
        if (owner is not Troop troop)
        {
            return 0;
        }

        int result = troop.GetUnitLevel() + 200;
        if (troop is Leader)
        {
            result -= 50;
        }

        return result;
    }
}
