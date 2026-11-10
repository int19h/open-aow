// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class TurnedUndeadAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:679
    public TurnedUndeadAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(TurnedUndeadRStr);
        FAbilityID = 34;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:693
    public override DurationMode GetDurationMode() => DurationMode.CombatTurn;

    // PassiveAb.pas:698
    public override sbyte GetTotalDuration() => 3;

    // PassiveAb.pas:687
    public override UnitEnchantment CreateEnchantment()
    {
        UnitEnchantment Result = base.CreateEnchantment();
        Result.FDispelResistance = 4;
        return Result;
    }

    private static readonly string TurnedUndeadRStr = "Turned Undead";
}
