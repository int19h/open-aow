// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// { Interface declaration run owned by P2. }
public partial class UnitResourceAbilityOwner(CustomNode owner) : AbilityOwner(owner)
{

    // Army.PAS:6125
    public override Set<AbilitySelectionType> GetAbilitySelectionTypes() => Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit);

    // intf_P1_06.inc:120

}
