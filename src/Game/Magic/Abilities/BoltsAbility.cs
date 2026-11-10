// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class BoltsAbility : RangedAttackAbility
{
    // AoWAb.PAS:4844
    public BoltsAbility(ECustomNode owner) : base(owner)
    {
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.UseItem);
        FAbilityType = AbilityType.Combat;
        FRangeRA = CombatRange.Medium;
        FAttackRepeatRA = 1;
    }
}
