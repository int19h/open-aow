using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Engine;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Ui.Grids;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Units.Tests;

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
    public void HeroNameKeepsEqualNamesInInsertionOrder()
    {
        var heroes = new HeroList(null!);
        _ = new Hero(heroes) { FHeroName = "m" };
        _ = new Hero(heroes) { FHeroName = "a" };
        _ = new Hero(heroes) { FHeroName = "a" };
        HeroGrid.SortHeroList = heroes;
        var indices = new IntegerList();
        indices.Add(0);
        indices.Add(1);
        indices.Add(2);
        indices.SortStable(HeroGrid.HeroGridCompareName);
        Assert.Equal(1, indices[0]);
        Assert.Equal(2, indices[1]);
        Assert.Equal(0, indices[2]);
        heroes.Dispose();
    }

    [Fact]
    public void HeroLevelKeepsEqualLevelsInInsertionOrder()
    {
        var heroes = new HeroList(null!);
        var high = new Hero(heroes) { FExperience = 15 };
        var low = new Hero(heroes);
        var tied = new Hero(heroes);
        Assert.True(high.GetLevel() > low.GetLevel());
        Assert.Equal(low.GetLevel(), tied.GetLevel());
        HeroGrid.SortHeroList = heroes;
        var indices = new IntegerList();
        indices.Add(0);
        indices.Add(1);
        indices.Add(2);
        indices.SortStable(HeroGrid.HeroGridCompareLevel);
        Assert.Equal(1, indices[0]);
        Assert.Equal(2, indices[1]);
        Assert.Equal(0, indices[2]);
        heroes.Dispose();
    }

    [Fact]
    public void ReflectingPoolsKeepInsertionOrder()
    {
        using var pools = new PlayerStructureList(null!);
        var first = new EObject(pools);
        var second = new EObject(pools);
        var third = new EObject(pools);
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
