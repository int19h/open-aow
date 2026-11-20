namespace Aow.Rtl;

public readonly record struct HeapStatus(int TotalAllocated);

public static class Heap
{
    public static byte[] ValueBytes<T>(T value, int count) where T : unmanaged => Bytes.Of(ref value, count).ToArray();
}
