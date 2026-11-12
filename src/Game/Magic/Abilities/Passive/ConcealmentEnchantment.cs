// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// (name, ID, selection types, ability type.)
public partial class ConcealmentEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1287
    public ConcealmentEnchantment(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(GameText.s_concealmentText);
        AbilityId = 162;
    }
}
