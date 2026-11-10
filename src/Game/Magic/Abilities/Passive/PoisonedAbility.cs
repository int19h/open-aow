// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type. duration mode
public partial class PoisonedAbility : DurationAbility
{
    // PassiveAb.pas:571
    public PoisonedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(PoisonedRStr);
        FAbilityID = 96;
        FAbilityType = AbilityType.Handicap;
        FDurationMode = DurationMode.Day;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:581
    public override sbyte GetAttack(AbilityOwner Owner) => -1;

    // PassiveAb.pas:586
    public override sbyte GetDefense(AbilityOwner Owner) => -1;

    // PassiveAb.pas:591
    public override sbyte GetResistance(AbilityOwner Owner) => -1;

    // PassiveAb.pas:596
    public override sbyte GetDamage(AbilityOwner Owner) => -1;

    private static readonly string PoisonedRStr = "Poisoned";
}
