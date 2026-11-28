// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class AnimatedAbility : AdjustableAbility
{
    // PassiveAb.pas:902
    public AnimatedAbility(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_animatedText);
        AbilityId = 129;
        AbilityType = AbilityType.Handicap;
        SelectionTypes = AbilitySelectionTypes.None;
    }

    private static readonly string s_animatedText = "Animated";
}
