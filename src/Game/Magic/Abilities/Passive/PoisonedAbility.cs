// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type. duration mode
public partial class PoisonedAbility : DurationAbility
{
    // PassiveAb.pas:571
    public PoisonedAbility(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_poisonedText);
        AbilityId = 96;
        AbilityType = AbilityType.Handicap;
        DurationMode = DurationMode.Day;
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:581
    public override sbyte GetAttack(AbilityOwner owner) => -1;

    // PassiveAb.pas:586
    public override sbyte GetDefense(AbilityOwner owner) => -1;

    // PassiveAb.pas:591
    public override sbyte GetResistance(AbilityOwner owner) => -1;

    // PassiveAb.pas:596
    public override sbyte GetDamage(AbilityOwner owner) => -1;

    private static readonly string s_poisonedText = "Poisoned";
}
