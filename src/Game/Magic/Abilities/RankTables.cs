
using Aow.Engine.Buffers;

namespace Aow.Game.Magic.Abilities;

internal static class RankTables
{
    internal static FixedArray_sbyte_3 P2RankDefense = FixedArray_sbyte_3.From([0, 1, 1]);

    internal static FixedArray_sbyte_3 P2RankAttack = FixedArray_sbyte_3.From([0, 1, 1]);

    internal static FixedArray_sbyte_3 P2RankDamage = FixedArray_sbyte_3.From([0, 0, 1]);

    internal static FixedArray_sbyte_3 P2RankResistance = FixedArray_sbyte_3.From([0, 0, 1]);

    internal static FixedArray_sbyte_3 P2RankHits = FixedArray_sbyte_3.From([0, 1, 2]);

}
