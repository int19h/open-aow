// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public class SlowEnchantmentAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:601
    public SlowEnchantmentAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), GameText.SlowText);
        AbilityId = 132;
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:615
    public override void CombatDone(CombatObject combatObject) => combatObject.GetAbilityOwner().RemoveAbility(AbilityId);

    // PassiveAb.pas:609
    public override UnitEnchantment CreateEnchantment()
    {
        UnitEnchantment result = base.CreateEnchantment();
        result.DispelResistance = 3;
        return result;
    }
}
