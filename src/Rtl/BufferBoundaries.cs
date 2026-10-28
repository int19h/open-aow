namespace Aow.Rtl;

public static partial class Rtl
{
    public static ReadOnlySpan<byte> BufferBytes(object? value) => value switch
    {
        null => default,
        byte[] bytes => bytes,
        ReadOnlyMemory<byte> memory => memory.Span,
        Memory<byte> memory => memory.Span,
        IPointerBox pointer => pointer.As<byte>().AsSpan(),
        _ => throw new EInvalidCast("Buffer has no byte storage")
    };
}
