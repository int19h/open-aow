// Translated from AoWE.pas.
using Aow.Engine.Buffers;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Combat;

public struct BattleFieldInfo
{
    public sbyte Player { get; private set; }
    public sbyte IndependentRelation { get; }
    public Set<BattleFieldInfoRequestsElement> Requests { get; set; }
    public Set<DamageType> DamageTypes { get; private set; }
    public UnitList Units { get; set; }
    public Set<byte> FriendlyRelations { get; set; }
    public Set<byte> EnemyRelations { get; set; }
    public int FriendlyStrength { get; set; }
    public int Strength { get; set; }

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
