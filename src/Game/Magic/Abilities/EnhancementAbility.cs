// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public class EnhancementAbility : AdjustableAbility
{

    // AoWAb.PAS:922
    public EnhancementAbility(CustomNode owner) : base(owner)
    {
        AbilityType = AbilityType.Enhancement;
        SelectionTypes = AbilitySelectionTypes.Unit;
    }
}
