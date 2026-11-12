// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

// (ability type)
public partial class AdjustableAbility : Ability
{

    public AdjustableAbility(ECustomNode owner) : base(owner)
    {
    }

    public AbilityType AbilityType;

    // AoWAb.PAS:1
    public override AbilityType GetAbilityType() => AbilityType;
}
