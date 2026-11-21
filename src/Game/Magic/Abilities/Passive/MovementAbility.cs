// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Game.World.Movement;

namespace Aow.Game.Magic.Abilities.Passive;

// (MoveTypes.)
public class MovementAbility : AdjustableAbility
{
    public MoveTypes MoveTypes { get; init; }

    // PassiveAb.pas:1409
    public MovementAbility(CustomNode owner) : base(owner)
    {
        SelectionTypes = AbilitySelectionTypes.Unit | AbilitySelectionTypes.RingItem;
        AbilityType = AbilityType.Movement;
        SkillPointCost = 0;
    }

    // PassiveAb.pas:1417
    public override MoveTypes GetMoveTypes(AbilityOwner owner) => MoveTypes;

    // PassiveAb.pas:1422
    public override WallCombatFeatures GetWallCombatFeatures(AbilityOwner owner)
    {
        return AbilityId == 1
            ? WallCombatFeatures.WallPassing | WallCombatFeatures.Participate
            : WallCombatFeatures.None;
    }
}
