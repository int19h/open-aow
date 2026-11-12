// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class BoltsAbility : RangedAttackAbility
{
    // AoWAb.PAS:4844
    public BoltsAbility(ECustomNode owner) : base(owner)
    {
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.UseItem);
        AbilityType = AbilityType.Combat;
        RangeRangedAttack = CombatRange.Medium;
        AttackRepeatRangedAttack = 1;
    }
}
