// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.Combat;

// { ot=1 }
// { Storm damage summary (20 bytes): $0C friendly damage, $10 enemy damage. }
// Each set keeps the width of its Delphi set: a value is cut to that width when it is assigned.
public struct StormDamageInfo
{
    public sbyte Player;
    public sbyte IndependentRelation;

    public Set<byte> FriendlyRelations { get; set => field = Set<byte>.FromMask((byte)value.Image); }

    public Set<byte> EnemyRelations { get; set => field = Set<byte>.FromMask((byte)value.Image); }

    public int FriendlyValue;
    public int EnemyValue;
    public int FriendlyDamage;
    public int EnemyDamage;
}
