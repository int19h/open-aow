// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public class BoltsAbility : RangedAttackAbility
{
    // AoWAb.PAS:4844
    public BoltsAbility(CustomNode owner) : base(owner)
    {
        SelectionTypes = AbilitySelectionTypes.Unit | AbilitySelectionTypes.UseItem;
        AbilityType = AbilityType.Combat;
        RangeRangedAttack = CombatRange.Medium;
        AttackRepeatRangedAttack = 1;
    }
}
