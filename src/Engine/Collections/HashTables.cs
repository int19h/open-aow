namespace Aow.Engine.Collections;

// A duplicate key throws the original message and leaves the first value in place.
internal static class HashTables
{
    internal static void Add<TKey, TValue>(Dictionary<TKey, TValue> table, TKey key, TValue value)
        where TValue : class
    {
        if (!table.TryAdd(key, value: value))
            throw new InvalidOperationException("ID already in HashTable");
    }
}
