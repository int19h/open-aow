namespace Aow.Rtl.Tests;

public sealed class CatalogTests
{
    [Fact]
    public void DuplicateAndAbsentKeysNameTheKindAndKey()
    {
        var catalog = new Catalog<int, object>("production order");
        var value = new object();
        Assert.Same(value, catalog.Add(42, value));
        Assert.Same(value, catalog[42]);
        var duplicate = Assert.Throws<InvalidOperationException>(() => catalog.Add(42, new object()));
        Assert.Contains("production order", duplicate.Message);
        Assert.Contains("42", duplicate.Message);
        var absent = Assert.Throws<InvalidOperationException>(() => catalog[17]);
        Assert.Contains("production order", absent.Message);
        Assert.Contains("17", absent.Message);
        Assert.Same(value, catalog[42]);
        Assert.Single(catalog.All);
    }

    [Fact]
    public void TryGetReturnsTheRegisteredValueOrNull()
    {
        var catalog = new Catalog<string, object>("renderer");
        Assert.False(catalog.TryGet("map", out var absent));
        Assert.Null(absent);
        var value = catalog.Add("map", new object());
        Assert.True(catalog.TryGet("map", out var found));
        Assert.Same(value, found);
    }

    [Fact]
    public void AllKeepsInsertionOrderAndEarlierSnapshotsStayReadOnly()
    {
        var catalog = new Catalog<int, object>("kind");
        var empty = catalog.All;
        var first = catalog.Add(9, new object());
        var previous = catalog.All;
        var second = catalog.Add(2, new object());
        Assert.Empty(empty);
        Assert.Equal([first], previous);
        Assert.Equal([first, second], catalog.All);
        var list = Assert.IsAssignableFrom<IList<object>>(catalog.All);
        Assert.True(list.IsReadOnly);
        Assert.Throws<NotSupportedException>(() => list[0] = second);
        Assert.Throws<NotSupportedException>(() => list.Add(new object()));
        Assert.Same(first, catalog[9]);
    }

    [Fact]
    public async Task ConcurrentAddsPublishEveryEntryAndRejectTheDuplicate()
    {
        var catalog = new Catalog<int, object>("kind");
        var values = Enumerable.Range(0, 32).Select(_ => new object()).ToArray();
        await Task.WhenAll(values.Select((value, key) => Task.Run(() =>
        {
            catalog.Add(key, value);
            Assert.Same(value, catalog[key]);
            foreach (var item in catalog.All) Assert.Contains(item, values);
        })));
        Assert.Equal(values.Length, catalog.All.Count);
        for (int key = 0; key < values.Length; key++) Assert.Same(values[key], catalog[key]);

        var results = await Task.WhenAll(Enumerable.Range(0, 2).Select(_ => Task.Run(() =>
        {
            try { catalog.Add(99, new object()); return true; }
            catch (InvalidOperationException) { return false; }
        })));
        Assert.Single(results, added => added);
        Assert.Equal(values.Length + 1, catalog.All.Count);
    }
}
