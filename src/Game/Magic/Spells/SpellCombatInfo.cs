// Translated from AoWE.pas.
using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// Each set keeps the width of its Delphi set: a value is cut to that width when it is assigned.
public struct SpellCombatInfo
{
    public sbyte Attack { get; internal set; }
    public sbyte Damage { get; internal set; }

    public Set<DamageType> DamageTypes { get; set => field = Set<DamageType>.FromMask((ushort)value.Image); }

    public sbyte RepeatCount { get; internal set; }
    public sbyte Range { get; internal set; }
    public sbyte Radius { get; internal set; }

    public Set<CombatTargetTypeElement> TargetTypes { get; set => field = Set<CombatTargetTypeElement>.FromMask((byte)value.Image); }
}
