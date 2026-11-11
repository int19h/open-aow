
using Aow.Engine.Buffers;

namespace Aow.Game.Magic.Abilities;

internal static class RankTables
{
    internal static SByteArray3 P2RankDefense = SByteArray3.From([0, 1, 1]);

    internal static SByteArray3 P2RankAttack = SByteArray3.From([0, 1, 1]);

    internal static SByteArray3 P2RankDamage = SByteArray3.From([0, 0, 1]);

    internal static SByteArray3 P2RankResistance = SByteArray3.From([0, 0, 1]);

    internal static SByteArray3 P2RankHits = SByteArray3.From([0, 1, 2]);

}
