// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

// (ability type)
public partial class AdjustableAbility(CustomNode owner) : Ability(owner)
{
    public AbilityType AbilityType { get; protected set; }

    // AoWAb.PAS:1
    public override AbilityType GetAbilityType() => AbilityType;
}
