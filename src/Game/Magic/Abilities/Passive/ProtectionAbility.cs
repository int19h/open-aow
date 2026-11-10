// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// (FProtectionTypes.)
public partial class ProtectionAbility : EnhancementAbility
{
    public Set<DamageType> FProtectionTypes;

    // PassiveAb.pas:1446
    public ProtectionAbility(ECustomNode owner) : base(owner)
    {
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.DefenseItem, AbilitySelectionType.RingItem, AbilitySelectionType.Editor);
        FAbilityType = AbilityType.Enhancement;
    }

    // PassiveAb.pas:1453
    public override Set<DamageType> GetProtectionTypes(AbilityOwner Owner) => FProtectionTypes;
}
