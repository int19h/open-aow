// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// (FImmunityTypes.)
public partial class ImmunityAbility : EnhancementAbility
{
    public Set<DamageType> FImmunityTypes;

    // PassiveAb.pas:1428
    public ImmunityAbility(ECustomNode owner) : base(owner)
    {
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.DefenseItem, AbilitySelectionType.RingItem);
        FAbilityType = AbilityType.Enhancement;
    }

    // PassiveAb.pas:1440
    public override Set<MoveType> GetMoveTypes(AbilityOwner Owner)
    {
        Set<MoveType> Result;
        if (FImmunityTypes.Contains(DamageType.Fire))
        {
            Result = Set<MoveType>.Of(MoveType.LavaCrossing);
        }
        else
        {
            Result = Set<MoveType>.Of();
        }

        return Result;
    }

    // PassiveAb.pas:1435
    public override Set<DamageType> GetImmunityTypes(AbilityOwner Owner) => FImmunityTypes;
}
