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
        FAbilityName = AbilityRules.TranslateRStr(AnimatedRStr);
        FAbilityID = 129;
        FAbilityType = AbilityType.Handicap;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }

    private static readonly string AnimatedRStr = "Animated";
}
