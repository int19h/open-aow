using System.Runtime.CompilerServices;

namespace Aow.Engine.Buffers;

// 8 values stored inline, for a record whose layout or value copy matters (a Delphi fixed array).
[InlineArray(8)]
public struct Array8<T>
{
    private T _element0;
}
