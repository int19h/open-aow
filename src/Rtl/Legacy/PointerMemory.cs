namespace Aow.Rtl;

public static partial class RtlUnit
{
    public static void GetMem<T>(out Ptr<T> pointer, int count) => pointer = Ptr<T>.Alloc(count);
    public static void ReallocMem<T>(ref Ptr<T> pointer, int count) => pointer = pointer.Realloc(count);
    public static void FreeMem<T>(out Ptr<T> pointer) => pointer = default;
    public static void New<T>(out Ptr<T> pointer) => pointer = Ptr<T>.Alloc(1);
    public static void Dispose<T>(out Ptr<T> pointer) => pointer = default;

    public static ReadOnlySpan<byte> BufferBytes(object? value) => value switch
    {
        null => default,
        byte[] bytes => bytes,
        ReadOnlyMemory<byte> memory => memory.Span,
        Memory<byte> memory => memory.Span,
        IPointerBox pointer => pointer.As<byte>().AsSpan(),
        _ => throw new InvalidCastException("Buffer has no byte storage")
    };

    public static T ReadLegacyRecord<T>(object value) => Ptr<T>.Unbox(value).Value;
}
