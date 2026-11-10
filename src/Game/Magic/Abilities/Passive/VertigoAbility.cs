// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public partial class VertigoAbility : DurationAbility
{
    // PassiveAb.pas:952
    public VertigoAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(VertigoRStr);
        FAbilityID = 98;
        FAbilityType = AbilityType.Handicap;
        FDurationMode = DurationMode.Day;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:969
    public override sbyte GetAttack(AbilityOwner Owner) => -2;

    // PassiveAb.pas:975
    public override sbyte GetDefense(AbilityOwner Owner) => -2;

    // PassiveAb.pas:963
    public override int GetTotalDuration() => 3;

    private static readonly string VertigoRStr = "Vertigo";
}
