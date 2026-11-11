// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class FuryAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:1160
    public FuryAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(GameText.FuryRStr);
        FAbilityID = 156;
    }

    // PassiveAb.pas:1168
    public override sbyte GetAttack(AbilityOwner Owner) => 2;

    // PassiveAb.pas:1174
    public override sbyte GetDefense(AbilityOwner Owner) => -1;

    // PassiveAb.pas:1180
    public override int GetAIUpkeepPriority(AbilityOwner Owner)
    {
        if (Owner is not Troop unit)
        {
            return 0;
        }

        int Result = unit.GetUnitLevel() + 200;
        if (unit is Leader)
        {
            Result -= 50;
        }

        return Result;
    }
}
