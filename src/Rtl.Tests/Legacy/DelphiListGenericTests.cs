namespace Aow.Rtl.Tests;

public sealed class DelphiListGenericTests
{
    private class Keyed(int id, int key)
    {
        public int Id { get; } = id;
        public int Key { get; } = key;
    }

    private sealed class DerivedKeyed(int id, int key) : Keyed(id, key);

    private sealed class Identical
    {
        public override bool Equals(object? other) => other is Identical;
        public override int GetHashCode() => 0;
    }

    // CLASSES.PAS TList.QuickSort, written out on an array: the pivot is the stored middle
    // element, and equal keys keep the swap order of the original.
    private static void ReferenceQuickSort<T>(T[] items, int left, int right, Func<T, T, int> compare)
    {
        int i;
        do
        {
            i = left;
            int j = right;
            T pivot = items[(left + right) >> 1];
            do
            {
                while (compare(items[i], pivot) < 0)
                {
                    i++;
                }

                while (compare(items[j], pivot) > 0)
                {
                    j--;
                }

                if (i <= j)
                {
                    (items[i], items[j]) = (items[j], items[i]);
                    i++;
                    j--;
                }
            }
            while (i <= j);
            if (left < j)
            {
                ReferenceQuickSort(items, left, j, compare);
            }

            left = i;
        }
        while (i < right);
    }

    [Fact]
    public void QuickSortFollowsTheOriginalAndStableSortKeepsOrder()
    {
        object first = new();
        object second = new();
        object third = new();
        DelphiList<object> list = [];
        _ = list.Add(first);
        _ = list.Add(second);
        _ = list.Add(third);
        list.Sort((_, _) => 0);
        Assert.False(ReferenceEquals(list[0], first) && ReferenceEquals(list[1], second) && ReferenceEquals(list[2], third));

        list.Clear();
        _ = list.Add(first);
        _ = list.Add(second);
        _ = list.Add(third);
        list.SortStable((_, _) => 0);
        Assert.Same(first, list[0]);
        Assert.Same(second, list[1]);
        Assert.Same(third, list[2]);

        foreach (int pattern in new[] { 0, 1, 2 })
        {
            foreach (int n in new[] { 1, 2, 3, 4, 5, 7, 8, 9, 16, 17, 32 })
            {
                Keyed[] keys = [.. Enumerable.Range(0, count: n).Select(i => new Keyed(i, key: pattern switch { 0 => 0, 1 => i % 3, _ => (n - i) % 5 }))];
                DelphiList<Keyed> sorted = [];
                foreach (Keyed key in keys)
                {
                    _ = sorted.Add(key);
                }

                sorted.Sort((a, b) => a!.Key - b!.Key);
                Keyed[] expected = [.. keys];
                ReferenceQuickSort(expected, left: 0, right: n - 1, (a, b) => a.Key - b.Key);
                Assert.Equal(expected, sorted.List.Take(n));

                DelphiList<Keyed> stable = [];
                foreach (Keyed key in keys)
                {
                    _ = stable.Add(key);
                }

                stable.SortStable((a, b) => a!.Key - b!.Key);
                Assert.Equal(keys.OrderBy(key => key.Key), stable.List.Take(n));
            }
        }
    }

    [Fact]
    public void NarrowedSortsMatchTheElementSortsAndTraceTheirComparer()
    {
        foreach (int n in new[] { 1, 2, 5, 9, 17, 32 })
        {
            Keyed[] keys = [.. Enumerable.Range(0, count: n).Select(i => new DerivedKeyed(i, key: (n - i) % 5))];
            DelphiList<Keyed> plain = [];
            DelphiList<Keyed> narrowed = [];
            foreach (Keyed key in keys)
            {
                _ = plain.Add(key);
                _ = narrowed.Add(key);
            }

            plain.Sort((a, b) => a!.Key - b!.Key);
            narrowed.Sort<DerivedKeyed>((a, b) => a!.Key - b!.Key);
            Assert.Equal(plain.List, narrowed.List);
            plain.SortStable((a, b) => b!.Key - a!.Key);
            narrowed.SortStable<DerivedKeyed>((a, b) => b!.Key - a!.Key);
            Assert.Equal(plain.List, narrowed.List);
        }

        Delegate? traced = null;
        DelphiList<Keyed>.TraceSort = (_, compare) => traced = compare;
        try
        {
            Comparison<DerivedKeyed?> compare = (a, b) => a!.Id - b!.Id;
            DelphiList<Keyed> list = [];
            _ = list.Add(new DerivedKeyed(id: 1, key: 0));
            list.Sort(compare);
            Assert.Same(compare, traced);
        }
        finally
        {
            DelphiList<Keyed>.TraceSort = null;
        }
    }

    [Fact]
    public void RemoveReturnsTheIndexAndLeavesTheSpareSlot()
    {
        object a = new();
        object b = new();
        object c = new();
        DelphiList<object> list = [];
        _ = list.Add(a);
        _ = list.Add(b);
        _ = list.Add(c);
        Assert.Equal(1, list.Remove(b));
        Assert.Same(c, list.List[list.Count]);
        _ = list.Add(null);
        Assert.Equal(2, list.Remove(null));
        Assert.Equal(-1, list.Remove(b));
        Assert.Equal(-1, list.Remove(new object()));
        Assert.Equal(0, list.Remove(a));
        Assert.Equal(0, list.Remove(c));
        Assert.Empty(list);

        _ = list.Add(a);
        _ = list.Add(null);
        _ = list.Add(b);
        _ = list.Add(null);
        list.Pack();
        Assert.Equal([a, b], list.List.Take(list.Count));
        list.Capacity = list.Count;
        Assert.Same(list, list.Expand());
        Assert.Equal(6, list.Capacity);
        _ = list.Expand();
        Assert.Equal(6, list.Capacity);

        Identical present = new();
        Identical equal = new();
        DelphiList<Identical> identities = [];
        _ = identities.Add(present);
        Assert.Equal(-1, identities.IndexOf(equal));
        Assert.Equal(0, identities.IndexOf(present));
        Assert.Equal(0, identities.Remove(present));

        Ptr<int> records = Ptr<int>.Alloc(2);
        PtrBox<int> alias = new(records + 1);
        PtrBox<int> sameAlias = new(records + 1);
        PtrBox<int> other = new(records);
        DelphiList<PtrBox<int>> boxes = [];
        _ = boxes.Add(alias);
        Assert.Equal(0, boxes.IndexOf(sameAlias));
        Assert.Equal(-1, boxes.IndexOf(other));
        Assert.Equal(0, boxes.Remove(sameAlias));
        Assert.Same(alias, boxes.List[0]);

        DelphiList<object> empty = [];
        Assert.Equal("List index out of bounds (0)", Assert.Throws<ArgumentOutOfRangeException>(() => empty.Delete(0)).Message);
        _ = empty.Add(a);
        empty.Count = 3;
        Assert.Null(empty[1]);
        Assert.Null(empty[2]);
        empty.Move(-1, target: -1);
        empty.Exchange(0, second: 0);
        empty.Move(0, target: 2);
        Assert.Equal([null, null, a], empty.List.Take(3));
    }
}
