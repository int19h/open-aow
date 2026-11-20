// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// (ProtectionTypes.)
public class ProtectionAbility : EnhancementAbility
{
    public Set<DamageType> ProtectionTypes { get; init; }

    // PassiveAb.pas:1446
    public ProtectionAbility(CustomNode owner) : base(owner)
    {
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.DefenseItem, AbilitySelectionType.RingItem, AbilitySelectionType.Editor);
        AbilityType = AbilityType.Enhancement;
    }

    // PassiveAb.pas:1453
    public override Set<DamageType> GetProtectionTypes(AbilityOwner owner) => ProtectionTypes;
}
