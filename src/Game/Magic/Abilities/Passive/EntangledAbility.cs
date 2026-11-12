// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class EntangledAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:620
    public EntangledAbility(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_entangledText);
        AbilityId = 94;
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:638
    public override sbyte GetDefense(AbilityOwner owner) => -2;

    // PassiveAb.pas:628
    public override DurationMode GetDurationMode() => DurationMode.CombatTurn;

    // PassiveAb.pas:633
    public override sbyte GetTotalDuration() => 3;

    // PassiveAb.pas:643
    public override UnitEnchantment CreateEnchantment()
    {
        UnitEnchantment result = base.CreateEnchantment();
        result.DispelResistance = 3;
        return result;
    }

    private static readonly string s_entangledText = "Entangled";
}
