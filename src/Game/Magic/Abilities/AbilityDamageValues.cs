// Translated from AoWE.pas.
using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// { Interface declaration run owned by P4. }
// Each set keeps the width of its Delphi set: a value is cut to that width when it is assigned.
public struct AbilityDamageValues
{
    public int SourceDamage;
    public int SourceCost;
    public int TargetDamage;
    public int TargetCost;

    public Set<DamageType> DamageTypes { get; set => field = Set<DamageType>.FromMask((ushort)value.Image); }
}
