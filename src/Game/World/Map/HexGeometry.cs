using System.Collections.Immutable;
using Aow.Game.World.Movement;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

public static class HexGeometry
{
    internal static void HVtoHex(int h, int v, out int x, out int y)
    {
        x = h + v;
        y = (v - h) >> 1;
    }

    internal static int RadiusToHexNumber(int radius)
    {
        int result = radius <= 10 ? RadiusToHexNumberTable[radius] : ((radius - 1) * radius * 3) + 1;
        return result;
    }

    internal static int InverseHexNumber(int hexNumber)
    {
        int result;
        int r;
        int n;
        if (hexNumber < 37)
        {
            result = InverseHexNumberTable[hexNumber];
        }
        else
        {
            r = ((int)Math.Sqrt(((hexNumber - 1) * 12) + 9) + 3) / 6;
            n = r * 6;
            result = ((hexNumber - ((r - 1) * r * 3) - 1 + (int)(((uint)n) >> 1)) % n) + ((r - 1) * r * 3) + 1;
        }

        return result;
    }

    internal static void CenterHexNumberToHex(int hexX, int hexY, int hexNumber, ref int newHexX, ref int newHexY)
    {
        int r;
        int side;
        int position;
        if (hexNumber != 0)
        {
            r = ((int)Math.Sqrt(((hexNumber - 1) * 12) + 9) + 3) / 6;
            hexNumber = hexNumber - ((r - 1) * r * 3) - 1;
            side = hexNumber / r;
            position = hexNumber % r;
            switch (side)
            {
                case 0:
                    HVtoHex(r, position - r, out newHexX, out newHexY);
                    break;
                case 1:
                    newHexX = r;
                    newHexY = position - ((r + 1) / 2);
                    break;
                case 2:
                    HVtoHex(-position, r, out newHexX, out newHexY);
                    break;
                case 3:
                    HVtoHex(-r, r - position, out newHexX, out newHexY);
                    break;
                case 4:
                    newHexX = -r;
                    newHexY = -position + (r / 2);
                    break;
                case 5:
                    HVtoHex(position, -r, out newHexX, out newHexY);
                    break;
            }
        }
        else
        {
            newHexX = 0;
            newHexY = 0;
        }

        newHexY = (hexX & 1) != 0 && (newHexX & 1) != 0 ? newHexY + hexY + 1 : newHexY + hexY;

        newHexX = newHexX + hexX;
    }

    internal static void TranslateHex(int fromHexX, int fromHexY, int toHexX, int toHexY, int hexX, int hexY, out int newHexX, out int newHexY)
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

    internal static void TranslateZeroHex(int hexX, int hexY, int deltaHexX, int deltaHexY, out int newHexX, out int newHexY)
    {
        newHexY = (hexX & 1) != 0 && (deltaHexX & 1) != 0 ? deltaHexY + hexY + 1 : deltaHexY + hexY;

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

    internal static MapCoordinate PackMapCoordinate(sbyte x, sbyte y, sbyte mapLevel) => new(x, y, mapLevel);

    internal static ReadOnlySpan<byte> InverseHexNumberTable => [0, 4, 5, 6, 1, 2, 3, 13, 14, 15, 16, 17, 18, 7, 8, 9, 10, 11, 12, 28, 29, 30, 31, 32, 33, 34, 35, 36, 19, 20, 21, 22, 23, 24, 25, 26, 27];

    internal static ReadOnlySpan<int> RadiusToHexNumberTable => [0, 1, 7, 19, 37, 61, 91, 127, 169, 217, 271];

    public static void OriginalAssert(string message, string fileName, int lineNumber) => Assert(false, message, fileName, lineNumber);

    internal static ImmutableArray<ImmutableArray<XYTableEntry>> HexNumberWalkTable { get; } = [[new XYTableEntry(1, 0), new XYTableEntry(0, 1), new XYTableEntry(-1, 0), new XYTableEntry(-1, -1), new XYTableEntry(0, -1), new XYTableEntry(1, -1), new XYTableEntry(0, 0), new XYTableEntry(0, 0)], [new XYTableEntry(1, 1), new XYTableEntry(0, 1), new XYTableEntry(-1, 1), new XYTableEntry(-1, 0), new XYTableEntry(0, -1), new XYTableEntry(1, 0), new XYTableEntry(0, 0), new XYTableEntry(0, 0)]];

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
            result = (int)Math.Sqrt(((hexNumber - 1) * 12) + 9) + 3;
        }

        return result;
    }

    internal static void HexNumberToHex(int hexNumber, ref int hexX, ref int hexY)
    {
        int r;
        int side;
        int position;
        if (hexNumber < 37)
        {
            hexX = s_hexNumberToHexTable[hexNumber][0];
            hexY = s_hexNumberToHexTable[hexNumber][1];
        }
        else
        {
            r = ((int)Math.Sqrt(((hexNumber - 1) * 12) + 9) + 3) / 6;
            hexNumber = hexNumber - ((r - 1) * r * 3) - 1;
            side = hexNumber / r;
            position = hexNumber % r;
            switch (side)
            {
                case 0:
                    HVtoHex(r, position - r, out hexX, out hexY);
                    break;
                case 1:
                    hexX = r;
                    hexY = position - ((r + 1) / 2);
                    break;
                case 2:
                    HVtoHex(-position, r, out hexX, out hexY);
                    break;
                case 3:
                    HVtoHex(-r, r - position, out hexX, out hexY);
                    break;
                case 4:
                    hexX = -r;
                    hexY = -position + (r / 2);
                    break;
                case 5:
                    HVtoHex(position, -r, out hexX, out hexY);
                    break;
            }
        }
    }

    internal static void HexNumberToPixel(int hexNumber, ref int pixelX, ref int pixelY)
    {
        HexNumberToHex(hexNumber, ref pixelX, ref pixelY);
        HexToPixel(pixelX, pixelY, out pixelX, out pixelY);
        pixelX -= 8;
    }

    internal static ImmutableArray<ImmutableArray<XYTableEntry>> HexNumberToXYTable { get; } = [[new XYTableEntry(0, 0), new XYTableEntry(0, -1), new XYTableEntry(1, -1), new XYTableEntry(1, 0), new XYTableEntry(0, 1), new XYTableEntry(-1, 0), new XYTableEntry(-1, -1), new XYTableEntry(0, 0)], [new XYTableEntry(0, 0), new XYTableEntry(0, -1), new XYTableEntry(1, 0), new XYTableEntry(1, 1), new XYTableEntry(0, 1), new XYTableEntry(-1, 1), new XYTableEntry(-1, 0), new XYTableEntry(0, 0)]];

    internal static ReadOnlySpan<byte> HexNumberToRadiusTable => [0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3];

    private static readonly ImmutableArray<ImmutableArray<int>> s_hexNumberToHexTable = [[0, 0], [0, -1], [1, -1], [1, 0], [0, 1], [-1, 0], [-1, -1], [0, -2], [1, -2], [2, -1], [2, 0], [2, 1], [1, 1], [0, 2], [-1, 1], [-2, 1], [-2, 0], [-2, -1], [-1, -2], [0, -3], [1, -3], [2, -2], [3, -2], [3, -1], [3, 0], [3, 1], [2, 2], [1, 2], [0, 3], [-1, 2], [-2, 2], [-3, 1], [-3, 0], [-3, -1], [-3, -2], [-2, -2], [-1, -3]];

    internal static void PixelToHex(int horizontal, int vertical, out int x, out int y)
    {
        int column = (horizontal - 8) >> 5;
        x = column;
        y = (vertical - ((column & 1) * 16)) >> 5;
    }

    internal static int PixelToHexNumber(int pixelX, int pixelY)
    {
        PixelToHex(pixelX, pixelY, out pixelX, out pixelY);
        return HexToHexNumber(pixelX, pixelY);
    }

    internal static int HexToV(int x, int y) => ((x + 1) >> 1) + y;

    internal static int HexToH(int x, int y)
    {
        int difference = x - (2 * y);
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
            result = up ? (int)(((uint)(dX + 1)) >> 1) + dY : (int)(((uint)dX) >> 1) + dY;
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
            result = up ? (int)(((uint)(dX + 1)) >> 1) + dY : (int)(((uint)dX) >> 1) + dY;
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

    internal static void HexToHV(int hexX, int hexY, out int H, out int V)
    {
        H = HexToH(hexX, hexY);
        V = HexToV(hexX, hexY);
    }

    internal static int HexToHexNumber(int hexX, int hexY)
    {
        int result;
        int r;
        int side;
        int position;
        int h = default;
        int v = default;
        r = HexToRadius(hexX, hexY);
        if (r != 0)
        {
            HexToHV(hexX, hexY, out h, out v);
            if (r == h)
            {
                side = 0;
                position = v + r;
            }
            else
            {
                if (r == v)
                {
                    side = 2;
                    position = -h;
                }
                else
                {
                    if (-r == h)
                    {
                        side = 3;
                        position = -v + r;
                    }
                    else
                    {
                        if (-r == v)
                        {
                            side = 5;
                            position = h;
                        }
                        else
                        {
                            if (r == hexX)
                            {
                                side = 1;
                                position = ((hexX + 1) / 2) + hexY;
                            }
                            else
                            {
                                if (r == -hexX)
                                {
                                    side = 4;
                                    position = -hexY - (hexX / 2);
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

            result = position + ((r - 1) * r * 3) + 1 + (side * r);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    internal static int DeltaHexToHexNumber(int x1, int y1, int x2, int y2)
    {
        TranslateHex(x1, y1, 0, 0, x2, y2, out x2, out y2);
        return HexToHexNumber(x2, y2);
    }

    internal static int DeltaHexToHexNumberFast(int x1, int y1, int x2, int y2)
    {
        int result = x1 == x2
            ? y2 < y1 ? 1 : y2 > y1 ? 4 : 0
            : !((x1 & 1) != 0) ? x2 < x1 ? y2 < y1 ? 6 : 5 : y2 < y1 ? 2 : 3 : x2 < x1 ? y2 > y1 ? 5 : 6 : y2 > y1 ? 3 : 2;
        return result;
    }

    internal static void HexToPixel(int hexX, int hexY, out int pixelX, out int pixelY)
    {
        pixelX = (hexX * 32) + 8;
        pixelY = (hexY * 32) + ((hexX & 1) * 16);
    }

    internal static int HexToHexDirection(int hexX, int hexY)
    {
        int result;
        int h;
        h = HexToH(hexX, hexY);
        result = h > 0
            ? hexX < 0 ? 6 : HexToV(hexX, hexY) < 0 ? 1 : 2
            : h < 0 ? hexX > 0 ? 3 : HexToV(hexX, hexY) > 0 ? 4 : 5 : hexX < 0 ? 6 : hexX > 0 ? 3 : 0;

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
