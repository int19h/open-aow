#nullable enable

namespace Aow.Game.Saves;

/// <summary>
/// A session cache of validated preview images bounded by their total bytes. The oldest entry
/// leaves first. 64 MiB holds about eighty of the largest shipped map images.
/// </summary>
public sealed class PreviewCache(long capacityBytes)
{
    private readonly Dictionary<PreviewKey, PreviewEntry> entries = [];
    private readonly Queue<PreviewKey> order = new();

    public int Count => entries.Count;

    /// <summary>The image bytes the cache holds.</summary>
    public long Bytes { get; private set; }

    public bool TryGet(PreviewKey key, out PreviewEntry entry) => entries.TryGetValue(key, out entry!);

    public void Add(PreviewKey key, PreviewEntry entry)
    {
        if (entry.Image.LongLength > capacityBytes || !entries.TryAdd(key, entry)) return;
        order.Enqueue(key);
        Bytes += entry.Image.LongLength;
        while (Bytes > capacityBytes && entries.Remove(order.Dequeue(), out var oldest)) Bytes -= oldest.Image.LongLength;
    }

    public void Clear()
    {
        entries.Clear();
        order.Clear();
        Bytes = 0;
    }
}
