using System.Runtime.CompilerServices;

namespace Aow.Engine.Buffers;

// 10 values stored inline, for a record whose layout or value copy matters (a Delphi fixed array).
[InlineArray(10)]
public struct Array10<T>
{
    private T _element0;
}
