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
        var high = new Item(list) { FOwnerName = "m" };
        var low = new Item(list) { FOwnerName = "a" };
        var tied = new Item(list) { FOwnerName = "a" };
        list.SortOnName();
        Assert.Same(low, list.FChildren[0]);
        Assert.Same(tied, list.FChildren[1]);
        Assert.Same(high, list.FChildren[2]);
    }

    [Fact]
    public void ItemTypeKeepsEqualTypesInInsertionOrder()
    {
        using var list = new ItemList(null!);
        var high = new Item(list) { FItemType = ItemType.Scroll };
        var low = new Item(list) { FItemType = ItemType.Head };
        var tied = new Item(list) { FItemType = ItemType.Head };
        list.SortOnType();
        Assert.Same(low, list.FChildren[0]);
        Assert.Same(tied, list.FChildren[1]);
        Assert.Same(high, list.FChildren[2]);
    }

    [Fact]
    public void ReflectingPoolsKeepInsertionOrder()
    {
        using var pools = new PlayerStructureList(null!);
        var first = new EngineObject(pools);
        var second = new EngineObject(pools);
        var third = new EngineObject(pools);
        Assert.Equal(0, ReflectingPool.ComparePools(first, third));
        pools.SortStable(ReflectingPool.ComparePools);
        Assert.Same(first, pools.FChildren[0]);
        Assert.Same(second, pools.FChildren[1]);
        Assert.Same(third, pools.FChildren[2]);
    }

    private static TextControlItem Item(TextControl control, int id, string name)
    {
        var item = new TextControlItem(control) { FID = id, FName = name };
        item.FText.Add("line");
        return item;
    }
}
