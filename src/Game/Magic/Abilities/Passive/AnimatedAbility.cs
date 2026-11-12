// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class AnimatedAbility : AdjustableAbility
{
    // PassiveAb.pas:902
    public AnimatedAbility(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(s_animatedText);
        AbilityId = 129;
        AbilityType = AbilityType.Handicap;
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }

    private static readonly string s_animatedText = "Animated";
}
