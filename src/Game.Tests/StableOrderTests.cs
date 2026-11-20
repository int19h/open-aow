#nullable disable
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

public sealed class StableOrderTests
{
    [Fact]
    public void TextControlKeepsEqualItemsInInsertionOrder()
    {
        using TextControl control = new(null) { Holder = EngineFixture.Engine };
        TextControlItem high = Item(control, id: 2, name: "high");
        TextControlItem low = Item(control, id: 1, name: "low");
        TextControlItem tied = Item(control, id: 1, name: "tied");
        control.Sort();
        Assert.Same(low, control.GetItem(0));
        Assert.Same(tied, control.GetItem(1));
        Assert.Same(high, control.GetItem(2));
    }

    [Fact]
    public void ItemNameKeepsEqualNamesInInsertionOrder()
    {
        using ItemList list = new(null) { Holder = EngineFixture.Engine };
        Item high = new(list) { OwnerName = "m" };
        Item low = new(list) { OwnerName = "a" };
        Item tied = new(list) { OwnerName = "a" };
        list.SortOnName();
        Assert.Same(low, list.GetChildInRange(0));
        Assert.Same(tied, list.GetChildInRange(1));
        Assert.Same(high, list.GetChildInRange(2));
    }

    [Fact]
    public void ItemTypeKeepsEqualTypesInInsertionOrder()
    {
        using ItemList list = new(null) { Holder = EngineFixture.Engine };
        Item high = new(list) { ItemType = ItemType.Scroll };
        Item low = new(list) { ItemType = ItemType.Head };
        Item tied = new(list) { ItemType = ItemType.Head };
        list.SortOnType();
        Assert.Same(low, list.GetChildInRange(0));
        Assert.Same(tied, list.GetChildInRange(1));
        Assert.Same(high, list.GetChildInRange(2));
    }

    [Fact]
    public void ReflectingPoolsKeepInsertionOrder()
    {
        using PlayerStructureList pools = new(null) { Holder = EngineFixture.Engine };
        ReflectingPool first = new(null) { Holder = EngineFixture.Engine };
        ReflectingPool second = new(null) { Holder = EngineFixture.Engine };
        ReflectingPool third = new(null) { Holder = EngineFixture.Engine };
        _ = pools.AddChild(first);
        _ = pools.AddChild(second);
        _ = pools.AddChild(third);
        Assert.Equal(0, ReflectingPool.ComparePools(first, third));
        pools.SortStable(ReflectingPool.ComparePools);
        Assert.Same(first, pools.GetChildInRange(0));
        Assert.Same(second, pools.GetChildInRange(1));
        Assert.Same(third, pools.GetChildInRange(2));
    }

    private static TextControlItem Item(TextControl control, int id, string name)
    {
        TextControlItem item = new(control) { Id = id, Name = name };
        _ = item.Text.Add("line");
        return item;
    }
}
