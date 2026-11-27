// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

// (ability type)
public class AdjustableAbility(CustomNode owner, EngineObject holder) : Ability(owner, holder: holder)
{
    public AbilityType AbilityType { get; protected init; }

    // AoWAb.PAS:1
    public override AbilityType GetAbilityType() => AbilityType;
}
