// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.Combat;

// { ot=1 }
// { Storm damage summary (20 bytes): $0C friendly damage, $10 enemy damage. }
// Each set keeps the width of its Delphi set: a value is cut to that width when it is assigned.
public struct StormDamageInfo
{
    public sbyte Player { get; internal set; }
    public sbyte IndependentRelation { get; }

    public Set<byte> FriendlyRelations { get; set => field = Set<byte>.FromMask((byte)value.Image); }

    public Set<byte> EnemyRelations { get; set => field = Set<byte>.FromMask((byte)value.Image); }

    public int FriendlyValue { get; internal set; }
    public int EnemyValue { get; internal set; }
    public int FriendlyDamage { get; internal set; }
    public int EnemyDamage { get; internal set; }
}
