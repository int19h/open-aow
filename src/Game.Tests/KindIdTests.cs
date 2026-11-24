using Aow.Engine;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Units;

namespace Aow.Game.Tests;

public sealed class KindIdTests
{
    [Fact]
    public void SpellAndAbilityIdsAreOrdered()
    {
        Assert.True(SpellId.FromInt32(1).CompareTo(SpellId.FromInt32(2)) < 0);
        Assert.True(AbilityId.FromInt32(2).CompareTo(AbilityId.FromInt32(1)) > 0);
    }

    [Fact]
    public void ItemIdHasNoOptionalSnapshotForm()
        => Assert.False(OriginalValueSnapshot.TryRender(null, declaredType: typeof(ItemId?), rendered: out _));
}
