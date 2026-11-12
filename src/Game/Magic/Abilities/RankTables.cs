
using Aow.Engine.Buffers;

namespace Aow.Game.Magic.Abilities;

internal static class RankTables
{
    internal static ReadOnlySpan<sbyte> RankDefense => [0, 1, 1];

    internal static ReadOnlySpan<sbyte> RankAttack => [0, 1, 1];

    internal static ReadOnlySpan<sbyte> RankDamage => [0, 0, 1];

    internal static ReadOnlySpan<sbyte> RankResistance => [0, 0, 1];

    internal static ReadOnlySpan<sbyte> RankHits => [0, 1, 2];

}
