// Translated from AoWE.pas.
using Aow.Game.Combat;

namespace Aow.Game.Magic.Abilities;

// { Interface declaration run owned by P4. }
// Each set keeps the width of its Delphi set: a value is cut to that width when it is assigned.
public struct AbilityDamageValues
{
    public int SourceDamage { get; internal set; }
    public int SourceCost { get; internal set; }
    public int TargetDamage { get; internal set; }
    public int TargetCost { get; internal set; }

    public DamageTypes DamageTypes { get; set; }
}
