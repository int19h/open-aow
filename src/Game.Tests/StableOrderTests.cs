using Aow.Engine.Nodes;

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
        using var control = new TextControl(null!);
        var high = Item(control, 2, "high");
        var low = Item(control, 1, "low");
        var tied = Item(control, 1, "tied");
        control.Sort();
        Assert.Same(low, control.GetItem(0));
        Assert.Same(tied, control.GetItem(1));
        Assert.Same(high, control.GetItem(2));
    }

    [Fact]
    public void ItemNameKeepsEqualNamesInInsertionOrder()
    {
        using var list = new ItemList(null!);
        var high = new Item(list) { OwnerName = "m" };
        var low = new Item(list) { OwnerName = "a" };
        var tied = new Item(list) { OwnerName = "a" };
        list.SortOnName();
        Assert.Same(low, list.GetChildInRange(0));
        Assert.Same(tied, list.GetChildInRange(1));
        Assert.Same(high, list.GetChildInRange(2));
    }

    [Fact]
    public void ItemTypeKeepsEqualTypesInInsertionOrder()
    {
        using var list = new ItemList(null!);
        var high = new Item(list) { ItemType = ItemType.Scroll };
        var low = new Item(list) { ItemType = ItemType.Head };
        var tied = new Item(list) { ItemType = ItemType.Head };
        list.SortOnType();
        Assert.Same(low, list.GetChildInRange(0));
        Assert.Same(tied, list.GetChildInRange(1));
        Assert.Same(high, list.GetChildInRange(2));
    }

    [Fact]
    public void ReflectingPoolsKeepInsertionOrder()
    {
        using var pools = new PlayerStructureList(null!);
        var first = new ReflectingPool(null!);
        var second = new ReflectingPool(null!);
        var third = new ReflectingPool(null!);
        pools.AddChild(first);
        pools.AddChild(second);
        pools.AddChild(third);
        Assert.Equal(0, ReflectingPool.ComparePools(first, third));
        pools.SortStable(ReflectingPool.ComparePools);
        Assert.Same(first, pools.GetChildInRange(0));
        Assert.Same(second, pools.GetChildInRange(1));
        Assert.Same(third, pools.GetChildInRange(2));
    }

    private static TextControlItem Item(TextControl control, int id, string name)
    {
        var item = new TextControlItem(control) { Id = id, Name = name };
        item.Text.Add("line");
        return item;
    }
}
