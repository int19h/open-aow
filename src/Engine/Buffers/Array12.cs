using System.Runtime.CompilerServices;

namespace Aow.Engine.Buffers;

// 12 values stored inline, for a record whose layout or value copy matters (a Delphi fixed array).
[InlineArray(12)]
public struct Array12<T>
{
    private T _element0;
}
