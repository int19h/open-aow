// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class AnimatedAbility : AdjustableAbility
{
    // PassiveAb.pas:902
    public AnimatedAbility(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_animatedText);
        AbilityId = 129;
        AbilityType = AbilityType.Handicap;
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }

    private static readonly string s_animatedText = "Animated";
}
