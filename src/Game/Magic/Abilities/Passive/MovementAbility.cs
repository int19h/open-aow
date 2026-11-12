// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// (MoveTypes.)
public partial class MovementAbility : AdjustableAbility
{
    public Set<MoveType> MoveTypes;

    // PassiveAb.pas:1409
    public MovementAbility(ECustomNode owner) : base(owner)
    {
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.RingItem);
        AbilityType = AbilityType.Movement;
        SkillPointCost = 0;
    }

    // PassiveAb.pas:1417
    public override Set<MoveType> GetMoveTypes(AbilityOwner owner) => MoveTypes;

    // PassiveAb.pas:1422
    public override Set<WallCombatFeaturesElement> GetWallCombatFeatures(AbilityOwner owner)
    {
        Set<WallCombatFeaturesElement> result;
        if (AbilityId == 1)
        {
            result = Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallPassing, WallCombatFeaturesElement.Participate);
        }
        else
        {
            result = Set<WallCombatFeaturesElement>.Of();
        }

        return result;
    }
}
