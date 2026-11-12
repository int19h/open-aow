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
        AbilityName = AbilityRules.TranslateResourceString(s_vertigoText);
        AbilityId = 98;
        AbilityType = AbilityType.Handicap;
        DurationMode = DurationMode.Day;
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:969
    public override sbyte GetAttack(AbilityOwner owner) => -2;

    // PassiveAb.pas:975
    public override sbyte GetDefense(AbilityOwner owner) => -2;

    // PassiveAb.pas:963
    public override int GetTotalDuration() => 3;

    private static readonly string s_vertigoText = "Vertigo";
}
