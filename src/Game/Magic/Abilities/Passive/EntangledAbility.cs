#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class EntangledAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:620
    public EntangledAbility(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_entangledText);
        AbilityId = 94;
        SelectionTypes = AbilitySelectionTypes.None;
    }

    // PassiveAb.pas:638
    public override int GetDefense(AbilityOwner owner) => -2;

    // PassiveAb.pas:628
    public override DurationMode GetDurationMode() => DurationMode.CombatTurn;

    // PassiveAb.pas:633
    public override int TotalDuration => 3;

    // PassiveAb.pas:643
    public override UnitEnchantment CreateEnchantment()
    {
        UnitEnchantment result = base.CreateEnchantment();
        result.DispelResistance = 3;
        return result;
    }

    private static readonly string s_entangledText = "Entangled";
}
