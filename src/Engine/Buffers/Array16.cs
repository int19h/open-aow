using System.Runtime.CompilerServices;

namespace Aow.Engine.Buffers;

// 16 values stored inline, for a record whose layout or value copy matters (a Delphi fixed array).
[InlineArray(16)]
public struct Array16<T>
{
    private T _element0;
}
