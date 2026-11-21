// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public sealed class HeroResourceAbilityOwner(CustomNode owner) : AbilityOwner(owner)
{

    // Army.PAS:6128
    public override AbilitySelectionTypes GetAbilitySelectionTypes() => AbilitySelectionTypes.Unit | AbilitySelectionTypes.Editor;
}
