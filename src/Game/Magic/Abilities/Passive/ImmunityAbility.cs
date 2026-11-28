// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Game.World.Movement;

namespace Aow.Game.Magic.Abilities.Passive;

// (ImmunityTypes.)
public class ImmunityAbility : EnhancementAbility
{
    public DamageTypes ImmunityTypes { get; init; }

    // PassiveAb.pas:1428
    public ImmunityAbility(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        SelectionTypes = AbilitySelectionTypes.Unit | AbilitySelectionTypes.DefenseItem | AbilitySelectionTypes.RingItem;
        AbilityType = AbilityType.Enhancement;
    }

    // PassiveAb.pas:1440
    public override MoveTypes GetMoveTypes(AbilityOwner owner) => ImmunityTypes.Contains(DamageTypes.Fire) ? MoveTypes.LavaCrossing : MoveTypes.None;

    // PassiveAb.pas:1435
    public override DamageTypes GetImmunityTypes(AbilityOwner owner) => ImmunityTypes;
}
