// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class EntangledAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:620
    public EntangledAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(EntangledRStr);
        FAbilityID = 94;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:638
    public override sbyte GetDefense(AbilityOwner Owner) => -2;

    // PassiveAb.pas:628
    public override DurationMode GetDurationMode() => DurationMode.CombatTurn;

    // PassiveAb.pas:633
    public override sbyte GetTotalDuration() => 3;

    // PassiveAb.pas:643
    public override UnitEnchantment CreateEnchantment()
    {
        UnitEnchantment Result = base.CreateEnchantment();
        Result.FDispelResistance = 3;
        return Result;
    }

    private static readonly string EntangledRStr = "Entangled";
}
