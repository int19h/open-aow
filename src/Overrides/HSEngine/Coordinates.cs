using System.Runtime.CompilerServices;

namespace Aow.Units.HSEngine;

public static partial class HSEngine
{
    // HSEngine.pas:3952. INC wraps before the arithmetic shift.
    public static int HXtoV(int x, int y) => unchecked(((x + 1) >> 1) + y);

    // HSEngine.pas:3964. DEC at MinInt is the one exception to floor division.
    public static int HXtoH(int x, int y)
    {
        int difference = unchecked(x - 2 * y);
        return difference == int.MinValue ? 1 << 30 : difference >> 1;
    }

    public static void HVtoHX(int h, int v, ref int x, ref int y)
    {
        x = unchecked(h + v);
        y = unchecked(v - h) >> 1;
    }

    public static void HPtoHX(int horizontal, int vertical, ref int x, ref int y)
    {
        int column = unchecked(horizontal - 8) >> 5;
        x = column;
        y = unchecked(vertical - (column & 1) * 16) >> 5;
    }

    // HSEngine.pas:4239 writes four bytes at each shortint destination.
    public static void HPtoHXs(int horizontal, int vertical, ref sbyte x, ref sbyte y)
    {
        int column = unchecked(horizontal - 8) >> 5;
        Unsafe.WriteUnaligned(ref Unsafe.As<sbyte, byte>(ref x), column);
        int row = unchecked(vertical - (column & 1) * 16) >> 5;
        Unsafe.WriteUnaligned(ref Unsafe.As<sbyte, byte>(ref y), row);
    }

    public static void WalkToNextHN(ref THNWalkData walk)
    {
        walk.HN = unchecked(walk.HN + 1);
        int parity = walk.X & 1;
        int side = walk.Side;
        walk.Step = unchecked(walk.Step + 1);
        if (walk.Step < walk.Radius || side != 5)
        {
            walk.X = unchecked(walk.X + HNWalkTable[parity][side].X);
            walk.Y = unchecked(walk.Y + HNWalkTable[parity][side].Y);
            if (walk.Step >= walk.Radius)
            {
                walk.Side = side + 1;
                walk.Step = 0;
            }
        }
        else
        {
            walk.Radius = unchecked(walk.Radius + 1);
            walk.Side = 0;
            walk.Step = 0;
            walk.X = walk.CenterX;
            walk.Y = unchecked(walk.CenterY - walk.Radius);
        }
    }
}
