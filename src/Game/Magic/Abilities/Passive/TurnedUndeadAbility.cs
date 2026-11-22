// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class TurnedUndeadAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:679
    public TurnedUndeadAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_turnedUndeadText);
        AbilityId = 34;
        SelectionTypes = AbilitySelectionTypes.None;
    }

    // PassiveAb.pas:693
    public override DurationMode GetDurationMode() => DurationMode.CombatTurn;

    // PassiveAb.pas:698
    public override int TotalDuration => 3;

    // PassiveAb.pas:687
    public override UnitEnchantment CreateEnchantment()
    {
        UnitEnchantment result = base.CreateEnchantment();
        result.DispelResistance = 4;
        return result;
    }

    private static readonly string s_turnedUndeadText = "Turned Undead";
}
