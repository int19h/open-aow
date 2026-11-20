// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public class BoltsAbility : RangedAttackAbility
{
    // AoWAb.PAS:4844
    public BoltsAbility(CustomNode owner) : base(owner)
    {
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.UseItem);
        AbilityType = AbilityType.Combat;
        RangeRangedAttack = CombatRange.Medium;
        AttackRepeatRangedAttack = 1;
    }
}
