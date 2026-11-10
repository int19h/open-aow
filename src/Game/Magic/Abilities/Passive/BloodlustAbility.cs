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
        FAbilityName = AbilityRules.TranslateRStr(BloodlustRStr);
        FAbilityID = 168;
        FAbilityType = AbilityType.Enchantment;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
        FDurationMode = DurationMode.Day;
    }

    // PassiveAb.pas:556
    public override sbyte GetAttack(AbilityOwner Owner) => 1;

    // PassiveAb.pas:561
    public override sbyte GetDefense(AbilityOwner Owner) => -1;

    // PassiveAb.pas:566
    public override sbyte GetDamage(AbilityOwner Owner) => 2;

    private static readonly string BloodlustRStr = "Bloodlust";
}
