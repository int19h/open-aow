namespace Aow.Rtl.Tests;

public sealed class DelphiListGenericTests
{
    private sealed class Keyed(int id, int key)
    {
        public int Id { get; } = id;
        public int Key { get; } = key;
    }

    private sealed class Identical
    {
        public override bool Equals(object? other) => other is Identical;
        public override int GetHashCode() => 0;
    }

    private static void AssertSame<T>(DelphiList plain, DelphiList<T> typed) where T : class
    {
        Assert.Equal(plain.Count, typed.Count);
        Assert.Equal(plain.Capacity, typed.Capacity);
        for (int i = 0; i < plain.Capacity; i++) Assert.Same(plain.List[i], typed.List[i]);
    }

    [Fact]
    public void QuickSortTiesMatchAndStableSortKeepsOrder()
    {
        var first = new object();
        var second = new object();
        var third = new object();
        var plain = new DelphiList();
        var typed = new DelphiList<object>();
        plain.Add(first);
        plain.Add(second);
        plain.Add(third);
        typed.Add(first);
        typed.Add(second);
        typed.Add(third);
        plain.Sort((_, _) => 0);
        typed.Sort((_, _) => 0);
        AssertSame(plain, typed);
        Assert.False(ReferenceEquals(plain[0], first) && ReferenceEquals(plain[1], second) && ReferenceEquals(plain[2], third));

        plain.Clear();
        typed.Clear();
        plain.Add(first);
        plain.Add(second);
        plain.Add(third);
        typed.Add(first);
        typed.Add(second);
        typed.Add(third);
        plain.SortStable((_, _) => 0);
        typed.SortStable((_, _) => 0);
        Assert.Same(first, typed[0]);
        Assert.Same(second, typed[1]);
        Assert.Same(third, typed[2]);
        AssertSame(plain, typed);

        Keyed[] keys = Enumerable.Range(0, 32).Select(i => new Keyed(i, 0)).ToArray();
        foreach (int pattern in new[] { 0, 1, 2 })
        {
            foreach (int n in new[] { 1, 2, 3, 4, 5, 7, 8, 9, 16, 17, 32 })
            {
                var untyped = new DelphiList();
                var generic = new DelphiList<Keyed>();
                for (int i = 0; i < n; i++)
                {
                    keys[i] = new Keyed(i, pattern switch { 0 => 0, 1 => i % 3, _ => (n - i) % 5 });
                    untyped.Add(keys[i]);
                    generic.Add(keys[i]);
                }

                untyped.Sort((a, b) => ((Keyed)a!).Key - ((Keyed)b!).Key);
                generic.Sort((a, b) => a!.Key - b!.Key);
                AssertSame(untyped, generic);
                for (int i = 0; i < n; i++)
                {
                    untyped[i] = keys[i];
                    generic[i] = keys[i];
                }

                untyped.SortStable((a, b) => ((Keyed)a!).Key - ((Keyed)b!).Key);
                generic.SortStable((a, b) => a!.Key - b!.Key);
                AssertSame(untyped, generic);
            }
        }
    }

    [Fact]
    public void RemoveReturnsTheSameIndexAndLeavesTheSameSpareSlot()
    {
        var a = new object();
        var b = new object();
        var c = new object();
        var plain = new DelphiList();
        var typed = new DelphiList<object>();
        plain.Add(a);
        plain.Add(b);
        plain.Add(c);
        typed.Add(a);
        typed.Add(b);
        typed.Add(c);
        int removed = plain.Remove(b);
        Assert.Equal(1, removed);
        Assert.Equal(removed, typed.Remove(b));
        Assert.Same(c, plain.List[plain.Count]);
        AssertSame(plain, typed);
        plain.Add(null);
        typed.Add(null);
        Assert.Equal(plain.Remove(null), typed.Remove(null));
        AssertSame(plain, typed);
        Assert.Equal(-1, plain.Remove(b));
        Assert.Equal(-1, typed.Remove(b));
        Assert.Equal(plain.Remove(new object()), typed.Remove(new object()));
        Assert.Equal(plain.Remove(a), typed.Remove(a));
        Assert.Equal(plain.Remove(c), typed.Remove(c));
        AssertSame(plain, typed);

        plain.Add(a);
        plain.Add(null);
        plain.Add(b);
        plain.Add(null);
        typed.Add(a);
        typed.Add(null);
        typed.Add(b);
        typed.Add(null);
        plain.Pack();
        typed.Pack();
        AssertSame(plain, typed);
        plain.Capacity = plain.Count;
        typed.Capacity = typed.Count;
        Assert.Same(plain, plain.Expand());
        Assert.Same(typed, typed.Expand());
        AssertSame(plain, typed);
        plain.Expand();
        typed.Expand();
        AssertSame(plain, typed);

        var present = new Identical();
        var equal = new Identical();
        var identities = new DelphiList();
        var typedIdentities = new DelphiList<Identical>();
        identities.Add(present);
        typedIdentities.Add(present);
        Assert.Equal(-1, identities.IndexOf(equal));
        Assert.Equal(-1, typedIdentities.IndexOf(equal));
        Assert.Equal(0, identities.IndexOf(present));
        Assert.Equal(identities.Remove(present), typedIdentities.Remove(present));
        AssertSame(identities, typedIdentities);

        var records = Ptr<int>.Alloc(2);
        var alias = new PtrBox<int>(records + 1);
        var sameAlias = new PtrBox<int>(records + 1);
        var other = new PtrBox<int>(records);
        var boxes = new DelphiList();
        var typedBoxes = new DelphiList<PtrBox<int>>();
        boxes.Add(alias);
        typedBoxes.Add(alias);
        Assert.Equal(0, boxes.IndexOf(sameAlias));
        Assert.Equal(0, typedBoxes.IndexOf(sameAlias));
        Assert.Equal(-1, boxes.IndexOf(other));
        Assert.Equal(-1, typedBoxes.IndexOf(other));
        Assert.Equal(boxes.Remove(sameAlias), typedBoxes.Remove(sameAlias));
        AssertSame(boxes, typedBoxes);

        var empty = new DelphiList();
        var typedEmpty = new DelphiList<object>();
        Assert.Equal(
            Assert.Throws<ArgumentOutOfRangeException>(() => empty.Delete(0)).Message,
            Assert.Throws<ArgumentOutOfRangeException>(() => typedEmpty.Delete(0)).Message);
        empty.Add(a);
        typedEmpty.Add(a);
        empty.Count = 3;
        typedEmpty.Count = 3;
        Assert.Null(typedEmpty[1]);
        Assert.Null(typedEmpty[2]);
        AssertSame(empty, typedEmpty);
        empty.Move(-1, -1);
        typedEmpty.Move(-1, -1);
        empty.Exchange(0, 0);
        typedEmpty.Exchange(0, 0);
        empty.Move(0, 2);
        typedEmpty.Move(0, 2);
        AssertSame(empty, typedEmpty);
    }
}
