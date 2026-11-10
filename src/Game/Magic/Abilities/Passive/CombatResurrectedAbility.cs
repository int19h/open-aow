// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class CombatResurrectedAbility : AdjustableAbility
{
    // PassiveAb.pas:912
    public CombatResurrectedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(GameText.ResurrectedRStr);
        FAbilityID = 130;
        FAbilityType = AbilityType.Handicap;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }
}
