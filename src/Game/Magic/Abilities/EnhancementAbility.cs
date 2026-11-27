// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public class EnhancementAbility : AdjustableAbility
{

    // AoWAb.PAS:922
    public EnhancementAbility(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        AbilityType = AbilityType.Enhancement;
        SelectionTypes = AbilitySelectionTypes.Unit;
    }
}
