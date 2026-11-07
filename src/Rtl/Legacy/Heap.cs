namespace Aow.Rtl;

public readonly record struct THeapStatus(int TotalAllocated);

public static partial class RtlUnit
{
    public static byte[] ValueBytes<T>(T value, int count) where T : unmanaged => Bytes.Of(ref value, count).ToArray();
    public static THeapStatus GetHeapStatus() => new(unchecked((int)GC.GetTotalMemory(false)));
    public static void FillBytes(Span<byte> bytes, int value) => bytes.Fill(unchecked((byte)value));
    public static bool ArrayEquals<TArray, TElement>(TArray left, TArray right) where TArray : unmanaged where TElement : unmanaged => Bytes.Of(ref left).SequenceEqual(Bytes.Of(ref right));
}
