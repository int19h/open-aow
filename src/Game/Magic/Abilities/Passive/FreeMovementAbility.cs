// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class FreeMovementAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:1187
    public FreeMovementAbility(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.FreeMovementText);
        AbilityId = 157;
    }
}
