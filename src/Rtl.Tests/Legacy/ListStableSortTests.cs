namespace Aow.Rtl.Tests;

public sealed class ListStableSortTests
{
    [Fact]
    public void StableSortKeepsEqualItemsAndQuickSortDoesNot()
    {
        var first = new object();
        var second = new object();
        var third = new object();
        var stable = new DelphiList<object>();
        stable.Add(first);
        stable.Add(second);
        stable.Add(third);
        stable.SortStable((_, _) => 0);
        Assert.Same(first, stable[0]);
        Assert.Same(second, stable[1]);
        Assert.Same(third, stable[2]);

        var quick = new DelphiList<object>();
        quick.Add(first);
        quick.Add(second);
        quick.Add(third);
        quick.Sort((_, _) => 0);
        Assert.False(ReferenceEquals(quick[0], first) && ReferenceEquals(quick[1], second) && ReferenceEquals(quick[2], third));
    }
}
