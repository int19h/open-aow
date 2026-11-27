#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public class VertigoAbility : DurationAbility
{
    // PassiveAb.pas:952
    public VertigoAbility(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_vertigoText);
        AbilityId = 98;
        AbilityType = AbilityType.Handicap;
        DurationMode = DurationMode.Day;
        SelectionTypes = AbilitySelectionTypes.None;
    }

    // PassiveAb.pas:969
    public override int GetAttack(AbilityOwner owner) => -2;

    // PassiveAb.pas:975
    public override int GetDefense(AbilityOwner owner) => -2;

    // PassiveAb.pas:963
    public override int TotalDuration => 3;

    private static readonly string s_vertigoText = "Vertigo";
}
