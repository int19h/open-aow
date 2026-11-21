#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class SlowEnchantmentAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:601
    public SlowEnchantmentAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.SlowText);
        AbilityId = 132;
        SelectionTypes = AbilitySelectionTypes.None;
    }

    // PassiveAb.pas:615
    public override void CombatDone(CombatObject combatObject) => combatObject.AbilityOwner.RemoveAbility(AbilityId);

    // PassiveAb.pas:609
    public override UnitEnchantment CreateEnchantment()
    {
        UnitEnchantment result = base.CreateEnchantment();
        result.DispelResistance = 3;
        return result;
    }
}
