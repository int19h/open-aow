using System.Runtime.CompilerServices;

namespace Aow.Engine.Buffers;

// 3 values stored inline, for a record whose layout or value copy matters (a Delphi fixed array).
[InlineArray(3)]
public struct Array3<T>
{
    private T _element0;

    public static Array3<T> From(ReadOnlySpan<T> values)
    {
        Array3<T> result = default;
        values.CopyTo(result);
        return result;
    }
}
