#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public class StrikeEnhancementAbility : AdjustableAbility
{

    // AoWAb.PAS:1475
    public StrikeEnhancementAbility(CustomNode owner) : base(owner) => AbilityType = AbilityType.Enhancement;

    // AoWAb.PAS:1481
    public override bool CanExpand(AbilityOwner owner)
    {
        bool result = base.CanExpand(owner);
        return result && (owner.GetAbilitySetAt(21) || owner is Item);
    }
}
