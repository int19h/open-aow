// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public partial class WebbedAbility : DurationAbility
{
    // PassiveAb.pas:736
    public WebbedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(WebbedRStr);
        FAbilityID = 97;
        FAbilityType = AbilityType.Handicap;
        FDurationMode = DurationMode.CombatTurn;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:751
    public override sbyte GetDefense(AbilityOwner Owner) => -2;

    // PassiveAb.pas:746
    public override int GetTotalDuration() => 3;

    private static readonly string WebbedRStr = "Webbed";
}
