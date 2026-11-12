using System.Runtime.CompilerServices;

namespace Aow.Engine.Buffers;

// 4 values stored inline, for a record whose layout or value copy matters (a Delphi fixed array).
[InlineArray(4)]
public struct Array4<T>
{
    private T _element0;
}
