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

    internal static void SetupBattleFieldInfo(ref BattleFieldInfo info, sbyte player, sbyte relation, Set<BattleFieldInfoRequestsElement> requests, Set<DamageType> damageTypes)
    {
        info = default;
        info.Player = player;
        // { Original ignores the independent relation parameter. }
        info.Requests = requests;
        info.DamageTypes = damageTypes;
        info.FriendlyRelations = Set<byte>.WithBounds(0, 7).With(2);
        info.EnemyRelations = Set<byte>.WithBounds(0, 7).With(1).With(3);
    }

}
