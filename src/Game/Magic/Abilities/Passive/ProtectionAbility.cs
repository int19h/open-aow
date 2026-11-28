// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

namespace Aow.Game.Magic.Abilities.Passive;

// (ProtectionTypes.)
public class ProtectionAbility : EnhancementAbility
{
    public DamageTypes ProtectionTypes { get; init; }

    // PassiveAb.pas:1446
    public ProtectionAbility(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        SelectionTypes = AbilitySelectionTypes.Unit | AbilitySelectionTypes.DefenseItem | AbilitySelectionTypes.RingItem | AbilitySelectionTypes.Editor;
        AbilityType = AbilityType.Enhancement;
    }

    // PassiveAb.pas:1453
    public override DamageTypes GetProtectionTypes(AbilityOwner owner) => ProtectionTypes;
}
