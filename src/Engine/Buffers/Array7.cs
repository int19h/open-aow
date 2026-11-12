using System.Runtime.CompilerServices;

namespace Aow.Engine.Buffers;

// 7 values stored inline, for a record whose layout or value copy matters (a Delphi fixed array).
[InlineArray(7)]
public struct Array7<T>
{
    private T _element0;
}
