// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public sealed class HeroResourceAbilityOwner(CustomNode owner) : AbilityOwner(owner)
{

    // Army.PAS:6128
    public override Set<AbilitySelectionType> GetAbilitySelectionTypes() => Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.Editor);
}
