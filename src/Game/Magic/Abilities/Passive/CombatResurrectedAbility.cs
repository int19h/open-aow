// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class CombatResurrectedAbility : AdjustableAbility
{
    // PassiveAb.pas:912
    public CombatResurrectedAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.ResurrectedText);
        AbilityId = 130;
        AbilityType = AbilityType.Handicap;
        SelectionTypes = AbilitySelectionTypes.None;
    }
}
