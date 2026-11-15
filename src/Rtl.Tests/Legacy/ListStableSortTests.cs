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
        stable.Add(first);
        stable.Add(second);
        stable.Add(third);
        stable.SortStable((_, _) => 0);
        Assert.Same(first, stable[0]);
        Assert.Same(second, stable[1]);
        Assert.Same(third, stable[2]);

        DelphiList<object> quick = new();
        quick.Add(first);
        quick.Add(second);
        quick.Add(third);
        quick.Sort((_, _) => 0);
        Assert.False(ReferenceEquals(quick[0], first) && ReferenceEquals(quick[1], second) && ReferenceEquals(quick[2], third));
    }
}
