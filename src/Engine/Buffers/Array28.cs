using System.Runtime.CompilerServices;

namespace Aow.Engine.Buffers;

// 28 values stored inline, for a record whose layout or value copy matters (a Delphi fixed array).
[InlineArray(28)]
public struct Array28<T>
{
    private T _element0;
}
