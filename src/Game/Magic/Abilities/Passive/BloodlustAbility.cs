// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type. duration mode
public partial class BloodlustAbility : DurationAbility
{
    // PassiveAb.pas:546
    public BloodlustAbility(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(s_bloodlustText);
        AbilityId = 168;
        AbilityType = AbilityType.Enchantment;
        SelectionTypes = Set<AbilitySelectionType>.Of();
        DurationMode = DurationMode.Day;
    }

    // PassiveAb.pas:556
    public override sbyte GetAttack(AbilityOwner owner) => 1;

    // PassiveAb.pas:561
    public override sbyte GetDefense(AbilityOwner owner) => -1;

    // PassiveAb.pas:566
    public override sbyte GetDamage(AbilityOwner owner) => 2;

    private static readonly string s_bloodlustText = "Bloodlust";
}
