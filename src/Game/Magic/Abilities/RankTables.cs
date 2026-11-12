
using Aow.Engine.Buffers;

namespace Aow.Game.Magic.Abilities;

internal static class RankTables
{
    internal static SByteArray3 s_rankDefense = SByteArray3.From([0, 1, 1]);

    internal static SByteArray3 s_rankAttack = SByteArray3.From([0, 1, 1]);

    internal static SByteArray3 s_rankDamage = SByteArray3.From([0, 0, 1]);

    internal static SByteArray3 s_rankResistance = SByteArray3.From([0, 0, 1]);

    internal static SByteArray3 s_rankHits = SByteArray3.From([0, 1, 2]);

}
