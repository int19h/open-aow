using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Aow.Rtl;

// Explicit registrations publish a snapshot that readers use without taking the write lock.
public sealed class Catalog<TKey, TValue>(string kind) where TKey : notnull where TValue : class
{
    private sealed record Snapshot(Dictionary<TKey, TValue> Entries, ReadOnlyCollection<TValue> All);

    private readonly object _writeLock = new();
    private Snapshot _snapshot = new([], Array.AsReadOnly(Array.Empty<TValue>()));

    public TValue Add(TKey key, TValue value)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(value);
        lock (_writeLock)
        {
            var previous = _snapshot;
            if (previous.Entries.ContainsKey(key))
                throw new InvalidOperationException($"Duplicate {kind} key: {key}.");
            var entries = new Dictionary<TKey, TValue>(previous.Entries) { [key] = value };
            var values = new TValue[previous.All.Count + 1];
            previous.All.CopyTo(values, 0);
            values[^1] = value;
            Volatile.Write(ref _snapshot, new Snapshot(entries, Array.AsReadOnly(values)));
        }
        return value;
    }

    public TValue this[TKey key] => TryGet(key, out var value) ? value
        : throw new InvalidOperationException($"Absent {kind} key: {key}.");

    public bool TryGet(TKey key, [MaybeNullWhen(false)] out TValue value) =>
        Volatile.Read(ref _snapshot).Entries.TryGetValue(key, out value);

    public IReadOnlyList<TValue> All => Volatile.Read(ref _snapshot).All;
}
