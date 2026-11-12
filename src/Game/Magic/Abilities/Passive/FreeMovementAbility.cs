// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class FreeMovementAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:1187
    public FreeMovementAbility(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(GameText.s_freeMovementText);
        AbilityId = 157;
    }
}
