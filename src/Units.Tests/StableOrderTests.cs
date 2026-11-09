using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HeroGrid;
using Aow.Units.ReflectingPool;

namespace Aow.Units.Tests;

public sealed class StableOrderTests
{
    [Fact]
    public void TextControlKeepsEqualItemsInInsertionOrder()
    {
        using var control = new TTextControl(null!);
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
        using var list = new TItemList(null!);
        var high = new TItem(list) { FOwnerName = "m" };
        var low = new TItem(list) { FOwnerName = "a" };
        var tied = new TItem(list) { FOwnerName = "a" };
        list.SortOnName();
        Assert.Same(low, list.FChildren[0]);
        Assert.Same(tied, list.FChildren[1]);
        Assert.Same(high, list.FChildren[2]);
    }

    [Fact]
    public void ItemTypeKeepsEqualTypesInInsertionOrder()
    {
        using var list = new TItemList(null!);
        var high = new TItem(list) { FItemType = TItemType.itScroll };
        var low = new TItem(list) { FItemType = TItemType.itHead };
        var tied = new TItem(list) { FItemType = TItemType.itHead };
        list.SortOnType();
        Assert.Same(low, list.FChildren[0]);
        Assert.Same(tied, list.FChildren[1]);
        Assert.Same(high, list.FChildren[2]);
    }

    [Fact]
    public void HeroNameKeepsEqualNamesInInsertionOrder()
    {
        var heroes = new THeroList(null!);
        _ = new THero(heroes) { FHeroName = "m" };
        _ = new THero(heroes) { FHeroName = "a" };
        _ = new THero(heroes) { FHeroName = "a" };
        HeroGridUnit.SortHeroList = heroes;
        var indices = new TIntegerList();
        indices.Add(0);
        indices.Add(1);
        indices.Add(2);
        indices.SortStable(HeroGridUnit.HeroGridCompareName);
        Assert.Equal(1, indices[0]);
        Assert.Equal(2, indices[1]);
        Assert.Equal(0, indices[2]);
        heroes.Dispose();
    }

    [Fact]
    public void HeroLevelKeepsEqualLevelsInInsertionOrder()
    {
        var heroes = new THeroList(null!);
        var high = new THero(heroes) { FExperience = 15 };
        var low = new THero(heroes);
        var tied = new THero(heroes);
        Assert.True(high.GetLevel() > low.GetLevel());
        Assert.Equal(low.GetLevel(), tied.GetLevel());
        HeroGridUnit.SortHeroList = heroes;
        var indices = new TIntegerList();
        indices.Add(0);
        indices.Add(1);
        indices.Add(2);
        indices.SortStable(HeroGridUnit.HeroGridCompareLevel);
        Assert.Equal(1, indices[0]);
        Assert.Equal(2, indices[1]);
        Assert.Equal(0, indices[2]);
        heroes.Dispose();
    }

    [Fact]
    public void ReflectingPoolsKeepInsertionOrder()
    {
        using var pools = new TPlayerStructureList(null!);
        var first = new TEObject(pools);
        var second = new TEObject(pools);
        var third = new TEObject(pools);
        Assert.Equal(0, ReflectingPoolUnit.ComparePools(first, third));
        pools.SortStable(ReflectingPoolUnit.ComparePools);
        Assert.Same(first, pools.FChildren[0]);
        Assert.Same(second, pools.FChildren[1]);
        Assert.Same(third, pools.FChildren[2]);
    }

    private static TTextControlItem Item(TTextControl control, int id, string name)
    {
        var item = new TTextControlItem(control) { FID = id, FName = name };
        item.FText.Add("line");
        return item;
    }
}
