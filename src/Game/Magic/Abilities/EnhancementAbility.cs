// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class EnhancementAbility : AdjustableAbility
{

    // AoWAb.PAS:922
    public EnhancementAbility(ECustomNode owner) : base(owner)
    {
        FAbilityType = AbilityType.Enhancement;
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
    }
}
