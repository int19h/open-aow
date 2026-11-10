// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public partial class StunnedAbility : DurationAbility
{
    // PassiveAb.pas:793
    public StunnedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(StunnedRStr);
        FAbilityID = 92;
        FAbilityType = AbilityType.Handicap;
        FDurationMode = DurationMode.CombatTurn;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:810
    public override sbyte GetDefense(AbilityOwner Owner) => -2;

    // PassiveAb.pas:804
    public override int GetTotalDuration() => 1;

    private static readonly string StunnedRStr = "Stunned";
}
