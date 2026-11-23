namespace Aow.Rtl.Tests;

public sealed class ListQueriesTests
{
    [Fact]
    public void FindLastIndexWalksBackwardsAndStopsAtTheFirstHit()
    {
        IReadOnlyList<int> list = [5, 7, 5, 7];
        List<int> visited = [];
        int found = list.FindLastIndex(item =>
        {
            visited.Add(item);
            return item == 5;
        });
        Assert.Equal(2, found);
        Assert.Equal([7, 5], visited);
    }

    [Fact]
    public void FindIndexFromASlotPastZeroKeepsTheAbsoluteIndex()
    {
        IReadOnlyList<string?> list = ["a", null, "a"];
        Assert.Equal(2, list.FindIndex(startIndex: 1, match: item => item == "a"));
        Assert.Equal(-1, list.FindIndex(startIndex: 3, match: item => item == "a"));
    }

    [Fact]
    public void FindLastReturnsTheLastMatchOrTheDefault()
    {
        IReadOnlyList<string?> list = ["a", null, "b"];
        Assert.Equal("b", list.FindLast(item => item is not null));
        Assert.Null(list.FindLast(item => item == "z"));
    }

    [Fact]
    public void EmptyListsFindNothing()
    {
        IReadOnlyList<int> list = [];
        Assert.Equal(-1, list.FindIndex(item => item == 1));
        Assert.Equal(-1, list.FindLastIndex(item => item == 1));
        Assert.Equal(0, list.FindLast(item => item == 1));
    }
}
