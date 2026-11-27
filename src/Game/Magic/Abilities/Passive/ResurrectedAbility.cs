// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class ResurrectedAbility : AdjustableAbility
{
    // PassiveAb.pas:892
    public ResurrectedAbility(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.ResurrectedText);
        AbilityId = 128;
        AbilityType = AbilityType.Handicap;
        SelectionTypes = AbilitySelectionTypes.None;
    }
}
