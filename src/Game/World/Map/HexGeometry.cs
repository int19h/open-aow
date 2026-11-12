using System.Collections.Immutable;
using System.Runtime.CompilerServices;

using Aow.Engine.Buffers;

using Aow.Game.World.Movement;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

public static class HexGeometry
{
    internal static void HVtoHex(int h, int v, ref int x, ref int y)
    {
        x = h + v;
        y = v - h >> 1;
    }

    internal static int RadiusToHexNumber(int radius)
    {
        int result;
        if (radius <= 10)
        {
            result = RadiusToHexNumberTable[radius];
        }
        else
        {
            result = (radius - 1) * radius * 3 + 1;
        }

        return result;
    }

    internal static int InverseHexNumber(int hexNumber)
    {
        int result;
        int R;
        int N;
        if (hexNumber < 37)
        {
            result = InverseHexNumberTable[hexNumber];
        }
        else
        {
            R = ((int)Math.Sqrt((hexNumber - 1) * 12 + 9) + 3) / 6;
            N = R * 6;
            result = (hexNumber - (R - 1) * R * 3 - 1 + (int)(((uint)N) >> 1)) % N + ((R - 1) * R * 3 + 1);
        }

        return result;
    }

    internal static void CenterHexNumberToHex(int hexX, int hexY, int hexNumber, ref int newHexX, ref int newHexY)
    {
        int R;
        int side;
        int position;
        if (hexNumber != 0)
        {
            R = ((int)Math.Sqrt((hexNumber - 1) * 12 + 9) + 3) / 6;
            hexNumber = hexNumber - (R - 1) * R * 3 - 1;
            side = hexNumber / R;
            position = hexNumber % R;
            switch (side)
            {
                case 0:
                    HVtoHex(R, position - R, ref newHexX, ref newHexY);
                    break;
                case 1:
                    newHexX = R;
                    newHexY = position - (R + 1) / 2;
                    break;
                case 2:
                    HVtoHex(-position, R, ref newHexX, ref newHexY);
                    break;
                case 3:
                    HVtoHex(-R, R - position, ref newHexX, ref newHexY);
                    break;
                case 4:
                    newHexX = -R;
                    newHexY = -position + R / 2;
                    break;
                case 5:
                    HVtoHex(position, -R, ref newHexX, ref newHexY);
                    break;
            }
        }
        else
        {
            newHexX = 0;
            newHexY = 0;
        }

        if ((hexX & 1) != 0 && (newHexX & 1) != 0)
        {
            newHexY = newHexY + hexY + 1;
        }
        else
        {
            newHexY = newHexY + hexY;
        }

        newHexX = newHexX + hexX;
    }

    internal static void TranslateHex(int fromHexX, int fromHexY, int toHexX, int toHexY, int hexX, int hexY, ref int newHexX, ref int newHexY)
    {
        int dX;
        int dY;
        dX = toHexX - fromHexX;
        dY = toHexY - fromHexY;
        if ((dX & 1) != 0)
        {
            if ((fromHexX & 1) != 0)
            {
                if (!((hexX & 1) != 0))
                {
                    dY -= 1;
                }
            }
            else
            {
                if ((hexX & 1) != 0)
                {
                    dY += 1;
                }
            }
        }

        newHexX = hexX + dX;
        newHexY = hexY + dY;
    }

    internal static void TranslateZeroHex(int hexX, int hexY, int deltaHexX, int deltaHexY, ref int newHexX, ref int newHexY)
    {
        if ((hexX & 1) != 0 && (deltaHexX & 1) != 0)
        {
            newHexY = deltaHexY + hexY + 1;
        }
        else
        {
            newHexY = deltaHexY + hexY;
        }

        newHexX = deltaHexX + hexX;
    }

    internal static void GenerateWalkAroundRoute(int hexX, int hexY, WalkAroundRouteCallBack callback)
    {
        int nHexX;
        int nHexY = default;
        int tHexX = default;
        int tHexY = default;
        int hexNumber;
        bool found;
        bool cancel;
        cancel = false;
        nHexX = 65535;
        found = false;
        hexNumber = 0;
        while (!found && hexNumber != 6 && !cancel)
        {
            hexNumber += 1;
            CenterHexNumberToHex(hexX, hexY, hexNumber, ref nHexX, ref nHexY);
            found = callback(nHexX, nHexY, ref cancel);
        }

        if (found && !cancel)
        {
            hexX = nHexX;
            hexY = nHexY;
            while (!cancel)
            {
                hexNumber += 2;
                if (hexNumber > 6)
                {
                    hexNumber -= 6;
                }

                do
                {
                    CenterHexNumberToHex(hexX, hexY, hexNumber, ref tHexX, ref tHexY);
                    hexNumber -= 1;
                    // { the original asks for NXhx, NYhx here, not for the hexagon it just stepped to }
                    found = callback(nHexX, nHexY, ref cancel);
                }
                while (!(found && !cancel));
                hexX = tHexX;
                hexY = tHexY;
            }
        }
    }

    internal static MapCoordinate PackMapCoordinate(sbyte X, sbyte Y, sbyte L)
    {
        MapCoordinate result = default;
        result.X = X;
        result.Y = Y;
        result.MapLevel = L;
        result.Tag = 0;
        return result;
    }

    internal static ReadOnlySpan<byte> InverseHexNumberTable => [0, 4, 5, 6, 1, 2, 3, 13, 14, 15, 16, 17, 18, 7, 8, 9, 10, 11, 12, 28, 29, 30, 31, 32, 33, 34, 35, 36, 19, 20, 21, 22, 23, 24, 25, 26, 27];

    internal static ReadOnlySpan<int> RadiusToHexNumberTable => [0, 1, 7, 19, 37, 61, 91, 127, 169, 217, 271];

    public static void OriginalAssert(string message, string fileName, int lineNumber)
    {
        Assert(false, message, fileName, lineNumber);
    }

    internal static ImmutableArray<ImmutableArray<XYTableEntry>> HexNumberWalkTable { get; } = [[new XYTableEntry { X = 1, Y = 0 }, new XYTableEntry { X = 0, Y = 1 }, new XYTableEntry { X = -1, Y = 0 }, new XYTableEntry { X = -1, Y = -1 }, new XYTableEntry { X = 0, Y = -1 }, new XYTableEntry { X = 1, Y = -1 }, new XYTableEntry { X = 0, Y = 0 }, new XYTableEntry { X = 0, Y = 0 }], [new XYTableEntry { X = 1, Y = 1 }, new XYTableEntry { X = 0, Y = 1 }, new XYTableEntry { X = -1, Y = 1 }, new XYTableEntry { X = -1, Y = 0 }, new XYTableEntry { X = 0, Y = -1 }, new XYTableEntry { X = 1, Y = 0 }, new XYTableEntry { X = 0, Y = 0 }, new XYTableEntry { X = 0, Y = 0 }]];

    internal static int HexNumberToRadius(int hexNumber)
    {
        int result;
        if (hexNumber < 37)
        {
            result = HexNumberToRadiusTable[hexNumber];
        }
        else
        {
            // { the original lacks the "div 6" of InvHN and HNtoHX here }
            result = (int)Math.Sqrt((hexNumber - 1) * 12 + 9) + 3;
        }

        return result;
    }

    internal static void HexNumberToHex(int hexNumber, ref int hexX, ref int hexY)
    {
        int R;
        int side;
        int position;
        if (hexNumber < 37)
        {
            hexX = s_hexNumberToHexTable[hexNumber][0];
            hexY = s_hexNumberToHexTable[hexNumber][1];
        }
        else
        {
            R = ((int)Math.Sqrt((hexNumber - 1) * 12 + 9) + 3) / 6;
            hexNumber = hexNumber - (R - 1) * R * 3 - 1;
            side = hexNumber / R;
            position = hexNumber % R;
            switch (side)
            {
                case 0:
                    HVtoHex(R, position - R, ref hexX, ref hexY);
                    break;
                case 1:
                    hexX = R;
                    hexY = position - (R + 1) / 2;
                    break;
                case 2:
                    HVtoHex(-position, R, ref hexX, ref hexY);
                    break;
                case 3:
                    HVtoHex(-R, R - position, ref hexX, ref hexY);
                    break;
                case 4:
                    hexX = -R;
                    hexY = -position + R / 2;
                    break;
                case 5:
                    HVtoHex(position, -R, ref hexX, ref hexY);
                    break;
            }
        }
    }

    internal static void HexNumberToPixel(int hexNumber, ref int pixelX, ref int pixelY)
    {
        HexNumberToHex(hexNumber, ref pixelX, ref pixelY);
        HexToPixel(pixelX, pixelY, ref pixelX, ref pixelY);
        pixelX -= 8;
    }

    internal static ImmutableArray<ImmutableArray<XYTableEntry>> HexNumberToXYTable { get; } = [[new XYTableEntry { X = 0, Y = 0 }, new XYTableEntry { X = 0, Y = -1 }, new XYTableEntry { X = 1, Y = -1 }, new XYTableEntry { X = 1, Y = 0 }, new XYTableEntry { X = 0, Y = 1 }, new XYTableEntry { X = -1, Y = 0 }, new XYTableEntry { X = -1, Y = -1 }, new XYTableEntry { X = 0, Y = 0 }], [new XYTableEntry { X = 0, Y = 0 }, new XYTableEntry { X = 0, Y = -1 }, new XYTableEntry { X = 1, Y = 0 }, new XYTableEntry { X = 1, Y = 1 }, new XYTableEntry { X = 0, Y = 1 }, new XYTableEntry { X = -1, Y = 1 }, new XYTableEntry { X = -1, Y = 0 }, new XYTableEntry { X = 0, Y = 0 }]];

    internal static ReadOnlySpan<byte> HexNumberToRadiusTable => [0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3];

    private static readonly ImmutableArray<ImmutableArray<int>> s_hexNumberToHexTable = [[0, 0], [0, -1], [1, -1], [1, 0], [0, 1], [-1, 0], [-1, -1], [0, -2], [1, -2], [2, -1], [2, 0], [2, 1], [1, 1], [0, 2], [-1, 1], [-2, 1], [-2, 0], [-2, -1], [-1, -2], [0, -3], [1, -3], [2, -2], [3, -2], [3, -1], [3, 0], [3, 1], [2, 2], [1, 2], [0, 3], [-1, 2], [-2, 2], [-3, 1], [-3, 0], [-3, -1], [-3, -2], [-2, -2], [-1, -3]];

    internal static void PixelToHex(int horizontal, int vertical, ref int x, ref int y)
    {
        int column = horizontal - 8 >> 5;
        x = column;
        y = vertical - (column & 1) * 16 >> 5;
    }

    internal static void PixelToHexes(int horizontal, int vertical, ref sbyte x, ref sbyte y)
    {
        int column = horizontal - 8 >> 5;
        Unsafe.WriteUnaligned(ref Unsafe.As<sbyte, byte>(ref x), column);
        int row = vertical - (column & 1) * 16 >> 5;
        Unsafe.WriteUnaligned(ref Unsafe.As<sbyte, byte>(ref y), row);
    }

    internal static int PixelToHexNumber(int pixelX, int pixelY)
    {
        PixelToHex(pixelX, pixelY, ref pixelX, ref pixelY);
        return HexToHexNumber(pixelX, pixelY);
    }

    internal static int HexToV(int x, int y) => ((x + 1) >> 1) + y;

    internal static int HexToH(int x, int y)
    {
        int difference = x - 2 * y;
        return difference == int.MinValue ? 1 << 30 : difference >> 1;
    }

    internal static int DeltaHexToRadius(int x1, int y1, int x2, int y2)
    {
        int result;
        int dX;
        int dY;
        int half;
        bool up;
        dX = Math.Abs(x1 - x2);
        dY = Math.Abs(y1 - y2);
        if (dY >= dX)
        {
            up = (dX & 1) != 0 && ((!((x1 & 1) != 0) && y1 < y2) || (!((x2 & 1) != 0) && y2 < y1));
            if (up)
            {
                result = (int)(((uint)(dX + 1)) >> 1) + dY;
            }
            else
            {
                result = (int)(((uint)dX) >> 1) + dY;
            }
        }
        else
        {
            result = dX;
            half = (int)(((uint)(dX + 1)) >> 1);
            if ((dX & 1) != 0 && ((!((x1 & 1) != 0) && y1 < y2) || (!((x2 & 1) != 0) && y2 < y1)))
            {
                dY += 1;
            }

            if (dY > half)
            {
                result += dY - half;
            }
        }

        return result;
    }

    internal static int HexToRadius(int hexX, int hexY)
    {
        int result;
        int dX;
        int dY;
        int half;
        bool up;
        dX = Math.Abs(hexX);
        dY = Math.Abs(hexY);
        if (dY >= dX)
        {
            up = (dX & 1) != 0 && (hexY > 0 || (!((hexX & 1) != 0) && hexY < 0));
            if (up)
            {
                result = (int)(((uint)(dX + 1)) >> 1) + dY;
            }
            else
            {
                result = (int)(((uint)dX) >> 1) + dY;
            }
        }
        else
        {
            result = dX;
            half = (int)(((uint)(dX + 1)) >> 1);
            if ((dX & 1) != 0 && (hexY > 0 || (!((hexX & 1) != 0) && hexY < 0)))
            {
                dY += 1;
            }

            if (dY > half)
            {
                result += dY - half;
            }
        }

        return result;
    }

    internal static void HexToHV(int hexX, int hexY, ref int H, ref int V)
    {
        H = HexToH(hexX, hexY);
        V = HexToV(hexX, hexY);
    }

    internal static int HexToHexNumber(int hexX, int hexY)
    {
        int result;
        int R;
        int side;
        int position;
        int H = default;
        int V = default;
        R = HexToRadius(hexX, hexY);
        if (R != 0)
        {
            HexToHV(hexX, hexY, ref H, ref V);
            if (R == H)
            {
                side = 0;
                position = V + R;
            }
            else
            {
                if (R == V)
                {
                    side = 2;
                    position = -H;
                }
                else
                {
                    if (-R == H)
                    {
                        side = 3;
                        position = -V + R;
                    }
                    else
                    {
                        if (-R == V)
                        {
                            side = 5;
                            position = H;
                        }
                        else
                        {
                            if (R == hexX)
                            {
                                side = 1;
                                position = (hexX + 1) / 2 + hexY;
                            }
                            else
                            {
                                if (R == -hexX)
                                {
                                    side = 4;
                                    position = -hexY - hexX / 2;
                                }
                                else
                                {
                                    throw new ArithmeticException("arithemetic error!");
                                }
                            }
                        }
                    }
                }
            }

            result = position + ((R - 1) * R * 3 + 1 + side * R);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    internal static int DeltaHexToHexNumber(int x1, int y1, int x2, int y2)
    {
        TranslateHex(x1, y1, 0, 0, x2, y2, ref x2, ref y2);
        return HexToHexNumber(x2, y2);
    }

    internal static int DeltaHexToHexNumberFast(int x1, int y1, int x2, int y2)
    {
        int result;
        if (x1 == x2)
        {
            if (y2 < y1)
            {
                result = 1;
            }
            else
            {
                if (y2 > y1)
                {
                    result = 4;
                }
                else
                {
                    result = 0;
                }
            }
        }
        else
        {
            if (!((x1 & 1) != 0))
            {
                if (x2 < x1)
                {
                    if (y2 < y1)
                    {
                        result = 6;
                    }
                    else
                    {
                        result = 5;
                    }
                }
                else
                {
                    if (y2 < y1)
                    {
                        result = 2;
                    }
                    else
                    {
                        result = 3;
                    }
                }
            }
            else
            {
                if (x2 < x1)
                {
                    if (y2 > y1)
                    {
                        result = 5;
                    }
                    else
                    {
                        result = 6;
                    }
                }
                else
                {
                    if (y2 > y1)
                    {
                        result = 3;
                    }
                    else
                    {
                        result = 2;
                    }
                }
            }
        }

        return result;
    }

    internal static void HexToPixel(int hexX, int hexY, ref int pixelX, ref int pixelY)
    {
        pixelX = hexX * 32 + 8;
        pixelY = hexY * 32 + (hexX & 1) * 16;
    }

    internal static int HexToHexDirection(int hexX, int hexY)
    {
        int result;
        int H;
        H = HexToH(hexX, hexY);
        if (H > 0)
        {
            if (hexX < 0)
            {
                result = 6;
            }
            else
            {
                if (HexToV(hexX, hexY) < 0)
                {
                    result = 1;
                }
                else
                {
                    result = 2;
                }
            }
        }
        else
        {
            if (H < 0)
            {
                if (hexX > 0)
                {
                    result = 3;
                }
                else
                {
                    if (HexToV(hexX, hexY) > 0)
                    {
                        result = 4;
                    }
                    else
                    {
                        result = 5;
                    }
                }
            }
            else
            {
                if (hexX < 0)
                {
                    result = 6;
                }
                else
                {
                    if (hexX > 0)
                    {
                        result = 3;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
        }

        return result;
    }

    internal static int DeltaHexToHexDirection(int x1, int y1, int x2, int y2)
    {
        x2 = x2 - x1;
        y2 = y2 - y1;
        if ((x2 & 1) != 0 && (x1 & 1) != 0)
        {
            y2 -= 1;
        }

        return HexToHexDirection(x2, y2);
    }

}
