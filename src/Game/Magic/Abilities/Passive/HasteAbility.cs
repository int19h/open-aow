// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class HasteAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:1021
    public HasteAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(GameText.HasteRStr);
        FAbilityID = 152;
    }
}
