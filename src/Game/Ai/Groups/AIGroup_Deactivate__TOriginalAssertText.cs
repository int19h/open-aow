// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Buffers;

namespace Aow.Game.Ai.Groups;

// { Review fix: retain the original assertion filename and line. }
[StructLayout(LayoutKind.Explicit, Size = 136)]
public struct AIGroup_Deactivate__TOriginalAssertText
{
    [FieldOffset(0)]
    public int RefCount;
    [FieldOffset(4)]
    public int Len;
    [FieldOffset(8)]
    public FixedArray_byte_128 Text;
}
