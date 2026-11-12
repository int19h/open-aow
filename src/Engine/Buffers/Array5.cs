using System.Runtime.CompilerServices;

namespace Aow.Engine.Buffers;

// 5 values stored inline, for a record whose layout or value copy matters (a Delphi fixed array).
[InlineArray(5)]
public struct Array5<T>
{
    private T _element0;
}
