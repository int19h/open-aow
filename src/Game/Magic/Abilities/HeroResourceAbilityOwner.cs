// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class HeroResourceAbilityOwner : AbilityOwner
{
    public HeroResourceAbilityOwner(ECustomNode owner) : base(owner)
    { }

    // Army.PAS:6128
    public override Set<AbilitySelectionType> GetAbilitySelectionTypes() => Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.Editor);

    // intf_P1_06.inc:120

}
