// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// (ProtectionTypes.)
public partial class ProtectionAbility : EnhancementAbility
{
    public Set<DamageType> ProtectionTypes { get; set; }

    // PassiveAb.pas:1446
    public ProtectionAbility(ECustomNode owner) : base(owner)
    {
        SelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.DefenseItem, AbilitySelectionType.RingItem, AbilitySelectionType.Editor);
        AbilityType = AbilityType.Enhancement;
    }

    // PassiveAb.pas:1453
    public override Set<DamageType> GetProtectionTypes(AbilityOwner owner) => ProtectionTypes;
}
