// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class EnhancementAbility : AdjustableAbility
{

    // AoWAb.PAS:922
    public EnhancementAbility(ECustomNode owner) : base(owner)
    {
        AbilityType = AbilityType.Enhancement;
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);
    }
}
