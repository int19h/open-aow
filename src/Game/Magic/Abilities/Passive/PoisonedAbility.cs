#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type. duration mode
public class PoisonedAbility : DurationAbility
{
    // PassiveAb.pas:571
    public PoisonedAbility(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_poisonedText);
        AbilityId = 96;
        AbilityType = AbilityType.Handicap;
        DurationMode = DurationMode.Day;
        SelectionTypes = AbilitySelectionTypes.None;
    }

    // PassiveAb.pas:581
    public override int GetAttack(AbilityOwner owner) => -1;

    // PassiveAb.pas:586
    public override int GetDefense(AbilityOwner owner) => -1;

    // PassiveAb.pas:591
    public override int GetResistance(AbilityOwner owner) => -1;

    // PassiveAb.pas:596
    public override int GetDamage(AbilityOwner owner) => -1;

    private static readonly string s_poisonedText = "Poisoned";
}
