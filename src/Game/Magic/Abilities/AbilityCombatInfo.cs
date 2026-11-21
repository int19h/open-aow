// Translated from AoWE.pas.
using Aow.Game.Combat;

namespace Aow.Game.Magic.Abilities;

// Each set keeps the width of its Delphi set: a value is cut to that width when it is assigned.
public struct AbilityCombatInfo
{
    public CombatTargetTypes TargetTypes { get; set; }

    public sbyte Attack { get; internal set; }
    public sbyte Damage { get; internal set; }

    public DamageTypes DamageTypes { get; set; }

    public sbyte RepeatCount { get; internal set; }
}
