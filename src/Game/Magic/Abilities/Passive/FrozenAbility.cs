// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class FrozenAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:649
    public FrozenAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(FrozenRStr);
        FAbilityID = 95;
        FAbilityType = AbilityType.Enchantment;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:674
    public override sbyte GetDefense(AbilityOwner Owner) => -2;

    // PassiveAb.pas:658
    public override DurationMode GetDurationMode() => DurationMode.CombatTurn;

    // PassiveAb.pas:663
    public override sbyte GetTotalDuration() => 2;

    // PassiveAb.pas:668
    public override UnitEnchantment CreateEnchantment()
    {
        UnitEnchantment Result = base.CreateEnchantment();
        Result.FDispelResistance = 3;
        return Result;
    }

    private static readonly string FrozenRStr = "Frozen";
}
