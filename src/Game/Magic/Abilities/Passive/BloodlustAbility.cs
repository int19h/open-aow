#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type. duration mode
public class BloodlustAbility : DurationAbility
{
    // PassiveAb.pas:546
    public BloodlustAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_bloodlustText);
        AbilityId = 168;
        AbilityType = AbilityType.Enchantment;
        SelectionTypes = AbilitySelectionTypes.None;
        DurationMode = DurationMode.Day;
    }

    // PassiveAb.pas:556
    public override int GetAttack(AbilityOwner owner) => 1;

    // PassiveAb.pas:561
    public override int GetDefense(AbilityOwner owner) => -1;

    // PassiveAb.pas:566
    public override int GetDamage(AbilityOwner owner) => 2;

    private static readonly string s_bloodlustText = "Bloodlust";
}
