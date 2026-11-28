// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class UnholyChampionEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1203
    public UnholyChampionEnchantment(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.UnholyChampionText);
        AbilityId = 161;
    }
}
