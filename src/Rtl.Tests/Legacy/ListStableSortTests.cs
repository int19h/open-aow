namespace Aow.Rtl.Tests;

public sealed class ListStableSortTests
{
    [Fact]
    public void StableSortKeepsEqualItemsAndQuickSortDoesNot()
    {
        object first = new();
        object second = new();
        object third = new();
        DelphiList<object> stable = new();
        _ = stable.Add(first);
        _ = stable.Add(second);
        _ = stable.Add(third);
        stable.SortStable((_, _) => 0);
        Assert.Same(first, stable[0]);
        Assert.Same(second, stable[1]);
        Assert.Same(third, stable[2]);

        DelphiList<object> quick = new();
        _ = quick.Add(first);
        _ = quick.Add(second);
        _ = quick.Add(third);
        quick.Sort((_, _) => 0);
        Assert.False(ReferenceEquals(quick[0], first) && ReferenceEquals(quick[1], second) && ReferenceEquals(quick[2], third));
    }
}
