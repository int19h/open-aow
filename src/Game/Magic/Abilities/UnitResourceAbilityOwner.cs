// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

// { Interface declaration run owned by P2. }
public sealed class UnitResourceAbilityOwner(CustomNode owner) : AbilityOwner(owner)
{

    // Army.PAS:6125
    public override AbilitySelectionTypes GetAbilitySelectionTypes() => AbilitySelectionTypes.Unit;
}
