// Translated from AoWE.pas.
using Aow.Engine.Buffers;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Combat;

public struct BattleFieldInfo
{
    public sbyte Player;
    public sbyte IndependentRelation;
    public Set<BattleFieldInfoRequestsElement> Requests;
    public Set<DamageType> DamageTypes;
    public ByteArray3 Reserved05;
    public UnitList Units;
    public Set<byte> FriendlyRelations;
    public Set<byte> EnemyRelations;
    public ByteArray2 Reserved0E;
    public int FriendlyStrength;
    public int Strength;
    public int Reserved18;

    internal static void SetupBattleFieldInfo(ref BattleFieldInfo Info, sbyte Player, sbyte Relation, Set<BattleFieldInfoRequestsElement> Requests, Set<DamageType> DamageTypes)
    {
        Info = default;
        Info.Player = Player;
        // { Original ignores the independent relation parameter. }
        Info.Requests = Requests;
        Info.DamageTypes = DamageTypes;
        Info.FriendlyRelations = Set<byte>.WithBounds(0, 7).With(2);
        Info.EnemyRelations = Set<byte>.WithBounds(0, 7).With(1).With(3);
    }

}
