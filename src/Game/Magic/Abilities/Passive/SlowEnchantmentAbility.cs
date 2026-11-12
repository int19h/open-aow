// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class SlowEnchantmentAbility : UnitEnchantmentAbility
{
    // PassiveAb.pas:601
    public SlowEnchantmentAbility(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(GameText.s_slowText);
        AbilityId = 132;
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:615
    public override void CombatDone(CombatObject combatObject)
    {
        combatObject.GetAbilityOwner().RemoveAbility(AbilityId);
    }

    // PassiveAb.pas:609
    public override UnitEnchantment CreateEnchantment()
    {
        UnitEnchantment result = base.CreateEnchantment();
        result.DispelResistance = 3;
        return result;
    }
}
