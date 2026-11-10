// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// (FMoveTypes.)
public partial class MovementAbility : AdjustableAbility
{
    public Set<MoveType> FMoveTypes;

    // PassiveAb.pas:1409
    public MovementAbility(ECustomNode owner) : base(owner)
    {
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.RingItem);
        FAbilityType = AbilityType.Movement;
        FSkillPointCost = 0;
    }

    // PassiveAb.pas:1417
    public override Set<MoveType> GetMoveTypes(AbilityOwner Owner) => FMoveTypes;

    // PassiveAb.pas:1422
    public override Set<WallCombatFeaturesElement> GetWallCombatFeatures(AbilityOwner Owner)
    {
        Set<WallCombatFeaturesElement> Result;
        if (FAbilityID == 1)
        {
            Result = Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallPassing, WallCombatFeaturesElement.Participate);
        }
        else
        {
            Result = Set<WallCombatFeaturesElement>.Of();
        }

        return Result;
    }
}
