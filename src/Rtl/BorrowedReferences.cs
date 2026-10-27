using System.Runtime.CompilerServices;

namespace Aow.Rtl;

// The original descriptor borrows memory. Its owner keeps that memory alive until Unlock.
// Registry entries preserve 32-bit handles without retaining the memory owner.
public static class BorrowedReferences
{
    private sealed record Entry(int Id, object Value);
    private sealed class Entries { public readonly Dictionary<int, Entry> Values = []; }
    private static readonly ConditionalWeakTable<object, Entries> owners = new();
    private static readonly Dictionary<int, WeakReference<Entry>> handles = [];
    private static int next;
    public static int Store(object? value)
    {
        if (value is null || value is IPointerBox { StorageIdentity: null }) return 0;
        object owner = value is IPointerBox pointer ? pointer.StorageIdentity! : value;
        int offset = value is IPointerBox p ? p.ByteIndex : 0;
        lock (handles)
        {
            var entries = owners.GetOrCreateValue(owner);
            if (entries.Values.TryGetValue(offset, out var existing)) return existing.Id;
            if ((next & 1023) == 0)
                foreach (int id in handles.Where(p => !p.Value.TryGetTarget(out _)).Select(p => p.Key).ToArray()) handles.Remove(id);
            var entry = new Entry(checked(++next), value);
            entries.Values.Add(offset, entry);
            handles.Add(entry.Id, new(entry));
            return entry.Id;
        }
    }
    public static object? Load(int handle)
    {
        if (handle == 0) return null;
        lock (handles)
            return handles.TryGetValue(handle, out var weak) && weak.TryGetTarget(out var entry)
                ? entry.Value : throw new EAccessViolation("Surface descriptor outlived its owner");
    }
}
