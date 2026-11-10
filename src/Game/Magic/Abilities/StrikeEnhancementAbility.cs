// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public partial class StrikeEnhancementAbility : AdjustableAbility
{

    // AoWAb.PAS:1475
    public StrikeEnhancementAbility(ECustomNode owner) : base(owner)
    {
        FAbilityType = AbilityType.Enhancement;
    }

    // AoWAb.PAS:1481
    public override bool CanExpand(AbilityOwner owner)
    {
        bool Result = base.CanExpand(owner);
        return Result && (owner.GetAbSet(21) || owner is Item);
    }
}
