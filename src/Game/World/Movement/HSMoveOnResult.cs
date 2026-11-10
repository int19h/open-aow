// Translated from HSEngine.pas.
using System.Runtime.InteropServices;

namespace Aow.Game.World.Movement;

[StructLayout(LayoutKind.Explicit, Size = 12)]
public struct HSMoveOnResult
{
    [FieldOffset(0)]
    public int Code;
    [FieldOffset(4)]
    public int Data;
    [FieldOffset(8)]
    public bool Done;
}
