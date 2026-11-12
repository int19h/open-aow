// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// (ImmunityTypes.)
public partial class ImmunityAbility : EnhancementAbility
{
    public Set<DamageType> ImmunityTypes;

    // PassiveAb.pas:1428
    public ImmunityAbility(ECustomNode owner) : base(owner)
    {
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.DefenseItem, AbilitySelectionType.RingItem);
        AbilityType = AbilityType.Enhancement;
    }

    // PassiveAb.pas:1440
    public override Set<MoveType> GetMoveTypes(AbilityOwner owner)
    {
        Set<MoveType> result;
        if (ImmunityTypes.Contains(DamageType.Fire))
        {
            result = Set<MoveType>.Of(MoveType.LavaCrossing);
        }
        else
        {
            result = Set<MoveType>.Of();
        }

        return result;
    }

    // PassiveAb.pas:1435
    public override Set<DamageType> GetImmunityTypes(AbilityOwner owner) => ImmunityTypes;
}
