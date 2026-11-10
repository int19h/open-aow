// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class ResurrectedAbility : AdjustableAbility
{
    // PassiveAb.pas:892
    public ResurrectedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(GameText.ResurrectedRStr);
        FAbilityID = 128;
        FAbilityType = AbilityType.Handicap;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }
}
