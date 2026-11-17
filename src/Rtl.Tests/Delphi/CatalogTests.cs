namespace Aow.Rtl.Tests;

public sealed class CatalogTests
{
    [Fact]
    public void DuplicateAndAbsentKeysNameTheKindAndKey()
    {
        Catalog<int, object> catalog = new("production order");
        object value = new();
        Assert.Same(value, catalog.Add(42, value));
        Assert.Same(value, catalog[42]);
        InvalidOperationException duplicate = Assert.Throws<InvalidOperationException>(() => catalog.Add(42, new object()));
        Assert.Contains("production order", duplicate.Message);
        Assert.Contains("42", duplicate.Message);
        InvalidOperationException absent = Assert.Throws<InvalidOperationException>(() => catalog[17]);
        Assert.Contains("production order", absent.Message);
        Assert.Contains("17", absent.Message);
        Assert.Same(value, catalog[42]);
        _ = Assert.Single(catalog.All);
    }

    [Fact]
    public void TryGetReturnsTheRegisteredValueOrNull()
    {
        Catalog<string, object> catalog = new("renderer");
        Assert.False(catalog.TryGet("map", out object? absent));
        Assert.Null(absent);
        object value = catalog.Add("map", new object());
        Assert.True(catalog.TryGet("map", out object? found));
        Assert.Same(value, found);
    }

    [Fact]
    public void AllKeepsInsertionOrderAndEarlierSnapshotsStayReadOnly()
    {
        Catalog<int, object> catalog = new("kind");
        IReadOnlyList<object> empty = catalog.All;
        object first = catalog.Add(9, new object());
        IReadOnlyList<object> previous = catalog.All;
        object second = catalog.Add(2, new object());
        Assert.Empty(empty);
        Assert.Equal([first], previous);
        Assert.Equal([first, second], catalog.All);
        IList<object> list = Assert.IsAssignableFrom<IList<object>>(catalog.All);
        Assert.True(list.IsReadOnly);
        _ = Assert.Throws<NotSupportedException>(() => list[0] = second);
        _ = Assert.Throws<NotSupportedException>(() => list.Add(new object()));
        Assert.Same(first, catalog[9]);
    }

    [Fact]
    public async Task ConcurrentAddsPublishEveryEntryAndRejectTheDuplicate()
    {
        Catalog<int, object> catalog = new("kind");
        object[] values = Enumerable.Range(0, 32).Select(_ => new object()).ToArray();
        await Task.WhenAll(values.Select((value, key) => Task.Run(() =>
        {
            _ = catalog.Add(key, value);
            Assert.Same(value, catalog[key]);
            foreach (object item in catalog.All)
            {
                Assert.Contains(item, values);
            }
        })));
        Assert.Equal(values.Length, catalog.All.Count);
        for (int key = 0; key < values.Length; key++)
        {
            Assert.Same(values[key], catalog[key]);
        }

        bool[] results = await Task.WhenAll(Enumerable.Range(0, 2).Select(attempt => Task.Run(() =>
        {
            try { _ = catalog.Add(99, new object()); return true; }
            catch (InvalidOperationException) { return false; }
        })));
        _ = Assert.Single(results, added => added);
        Assert.Equal(values.Length + 1, catalog.All.Count);
    }
}
