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
    public void FindLastIndexFromASlotKeepsTheAbsoluteIndex()
    {
        IReadOnlyList<int> list = [5, 7, 5, 7];
        Assert.Equal(0, list.FindLastIndex(startIndex: 1, match: item => item == 5));
        Assert.Equal(-1, list.FindLastIndex(startIndex: 2, match: item => item == 9));
    }

    [Fact]
    public void RangedStartsValidateLikeList()
    {
        IReadOnlyList<int> list = [5, 7, 5];
        Assert.Equal(-1, list.FindIndex(startIndex: 3, match: item => item == 5));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => list.FindIndex(startIndex: -1, match: item => true));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => list.FindIndex(startIndex: 4, match: item => true));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => list.FindLastIndex(startIndex: -1, match: item => true));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => list.FindLastIndex(startIndex: 3, match: item => true));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => list.FindLastIndex(startIndex: 4, match: item => true));
        IReadOnlyList<int> empty = [];
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => empty.FindLastIndex(startIndex: 0, match: item => true));
        Assert.Equal(-1, empty.FindIndex(startIndex: 0, match: item => true));
    }

    [Fact]
    public void NullPredicatesThrow()
    {
        IReadOnlyList<int> list = [1];
        _ = Assert.Throws<ArgumentNullException>(() => list.FindIndex(match: null!));
        _ = Assert.Throws<ArgumentNullException>(() => list.FindLastIndex(match: null!));
        _ = Assert.Throws<ArgumentNullException>(() => list.FindLastIndex(state: 0, match: null!));
        _ = Assert.Throws<ArgumentNullException>(() => list.FindIndex(state: 0, match: null!));
        _ = Assert.Throws<ArgumentNullException>(() => list.Find(state: 0, match: null!));
        _ = Assert.Throws<ArgumentNullException>(() => list.FindLast(match: null!));
    }

    [Fact]
    public void FindLastIndexPassesStateAndWalksBackwards()
    {
        IReadOnlyList<int> list = [5, 7, 5, 7];
        List<int> visited = [];
        int found = list.FindLastIndex(5, match: (sought, item) =>
        {
            visited.Add(item);
            return item == sought;
        });
        Assert.Equal(2, found);
        Assert.Equal([7, 5], visited);
        Assert.Equal(2, list.FindLastIndex(5, match: static (sought, item) => item == sought));
        Assert.Equal(-1, list.FindLastIndex(9, match: static (sought, item) => item == sought));
    }

    [Fact]
    public void FindIndexPassesStateAndWalksForwards()
    {
        IReadOnlyList<int> list = [7, 5, 7, 5];
        List<int> visited = [];
        int found = list.FindIndex(5, match: (sought, item) =>
        {
            visited.Add(item);
            return item == sought;
        });
        Assert.Equal(1, found);
        Assert.Equal([7, 5], visited);
        Assert.Equal(-1, list.FindIndex(9, match: static (sought, item) => item == sought));
        IReadOnlyList<int> empty = [];
        Assert.Equal(-1, empty.FindIndex(5, match: static (sought, item) => item == sought));
    }

    [Fact]
    public void FindPassesStateAndReturnsTheFirstMatchOrTheDefault()
    {
        IReadOnlyList<string?> list = ["ab", null, "ac"];
        Assert.Equal("ab", list.Find('a', match: static (sought, item) => item is not null && item[0] == sought));
        Assert.Null(list.Find('z', match: static (sought, item) => item is not null && item[0] == sought));
        CountingList counting = new(5, 7, 5);
        Assert.Equal(7, counting.Find(7, match: static (sought, item) => item == sought));
        Assert.Equal(2, counting.Reads);
    }

    [Fact]
    public void FindLastReadsAHitOnce()
    {
        CountingList list = new(5, 7, 5);
        Assert.Equal(5, list.FindLast(item => item == 5));
        Assert.Equal(1, list.Reads);
    }

    [Fact]
    public void AListKeepsItsOwnOverloads()
    {
        List<int> list = [1, 2, 3];
        Assert.Equal(1, list.FindIndex(item => item == 2));
        Assert.Equal(2, list.FindLastIndex(item => item == 3));
    }

    [Fact]
    public void EmptyListsFindNothing()
    {
        IReadOnlyList<int> list = [];
        Assert.Equal(-1, list.FindIndex(item => item == 1));
        Assert.Equal(-1, list.FindLastIndex(item => item == 1));
        Assert.Equal(0, list.FindLast(item => item == 1));
    }

    private sealed class CountingList(params int[] items) : IReadOnlyList<int>
    {
        internal int Reads;

        public int Count => items.Length;

        public int this[int index]
        {
            get
            {
                Reads++;
                return items[index];
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return items[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
