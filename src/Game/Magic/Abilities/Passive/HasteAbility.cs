// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class HasteAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:1021
    public HasteAbility(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(GameText.s_hasteText);
        AbilityId = 152;
    }
}
