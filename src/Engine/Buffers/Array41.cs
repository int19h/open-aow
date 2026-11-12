using System.Runtime.CompilerServices;

namespace Aow.Engine.Buffers;

// 41 values stored inline, for a record whose layout or value copy matters (a Delphi fixed array).
[InlineArray(41)]
public struct Array41<T>
{
    private T _element0;
}
