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
        FAbilityName = AbilityRules.TranslateRStr(GameText.SlowRStr);
        FAbilityID = 132;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:615
    public override void CombatDone(CombatObject CombatObject)
    {
        CombatObject.GetAbilityOwner().RemoveAbility(FAbilityID);
    }

    // PassiveAb.pas:609
    public override UnitEnchantment CreateEnchantment()
    {
        UnitEnchantment Result = base.CreateEnchantment();
        Result.FDispelResistance = 3;
        return Result;
    }
}
