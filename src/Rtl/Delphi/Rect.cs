using System.Runtime.InteropServices;

namespace Aow.Rtl;

[StructLayout(LayoutKind.Explicit, Size = 16)]
public struct Rect
{
    public Rect(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    [FieldOffset(0)] public int Left;
    [FieldOffset(4)] public int Top;
    [FieldOffset(8)] public int Right;
    [FieldOffset(12)] public int Bottom;
}

public static partial class RtlUnit
{

    public static bool EqualRect(in Rect left, in Rect right) => left.Left == right.Left && left.Top == right.Top && left.Right == right.Right && left.Bottom == right.Bottom;

    public static bool IntersectRect(ref Rect result, in Rect left, in Rect right)
    {
        var intersection = new Rect { Left = Math.Max(left.Left, right.Left), Top = Math.Max(left.Top, right.Top), Right = Math.Min(left.Right, right.Right), Bottom = Math.Min(left.Bottom, right.Bottom) };
        bool nonempty = intersection.Right > intersection.Left && intersection.Bottom > intersection.Top;
        result = nonempty ? intersection : default;
        return nonempty;
    }

    public static bool UnionRect(ref Rect result, in Rect left, in Rect right)
    {
        bool a = left.Right > left.Left && left.Bottom > left.Top, b = right.Right > right.Left && right.Bottom > right.Top;
        result = !a ? b ? right : default : !b ? left : new Rect { Left = Math.Min(left.Left, right.Left), Top = Math.Min(left.Top, right.Top), Right = Math.Max(left.Right, right.Right), Bottom = Math.Max(left.Bottom, right.Bottom) };
        return a || b;
    }
}
