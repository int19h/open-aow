// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class FrozenAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:649
    public FrozenAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_frozenText);
        AbilityId = 95;
        AbilityType = AbilityType.Enchantment;
        SelectionTypes = AbilitySelectionTypes.None;
    }

    // PassiveAb.pas:674
    public override sbyte GetDefense(AbilityOwner owner) => -2;

    // PassiveAb.pas:658
    public override DurationMode GetDurationMode() => DurationMode.CombatTurn;

    // PassiveAb.pas:663
    public override sbyte TotalDuration => 2;

    // PassiveAb.pas:668
    public override UnitEnchantment CreateEnchantment()
    {
        UnitEnchantment result = base.CreateEnchantment();
        result.DispelResistance = 3;
        return result;
    }

    private static readonly string s_frozenText = "Frozen";
}
