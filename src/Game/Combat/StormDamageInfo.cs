// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;

namespace Aow.Game.Combat;

// { ot=1 }
// { Storm damage summary (20 bytes): $0C friendly damage, $10 enemy damage. }
[StructLayout(LayoutKind.Explicit, Size = 20)]
public struct StormDamageInfo
{
    [FieldOffset(0)]
    public sbyte Player;
    [FieldOffset(1)]
    public sbyte IndependentRelation;
    [FieldOffset(2)]
    public byte FriendlyRelationsImage;

    public Set<byte> FriendlyRelations { get => Set<byte>.FromMask(FriendlyRelationsImage); set => FriendlyRelationsImage = (byte)value.Image; }

    [FieldOffset(3)]
    public byte EnemyRelationsImage;

    public Set<byte> EnemyRelations { get => Set<byte>.FromMask(EnemyRelationsImage); set => EnemyRelationsImage = (byte)value.Image; }

    [FieldOffset(4)]
    public int FriendlyValue;
    [FieldOffset(8)]
    public int EnemyValue;
    [FieldOffset(12)]
    public int FriendlyDamage;
    [FieldOffset(16)]
    public int EnemyDamage;
}
