using System.Runtime.CompilerServices;

using Aow.Engine.Buffers;

using Aow.Game.World.Movement;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

public static class HexGeometry
{
    internal static void HVtoHX(int h, int v, ref int x, ref int y)
    {
        x = h + v;
        y = v - h >> 1;
    }

    internal static int RadToHN(int Rad)
    {
        int Result;
        if (Rad <= 10)
        {
            Result = RadToHNTable[Rad];
        }
        else
        {
            Result = (Rad - 1) * Rad * 3 + 1;
        }

        return Result;
    }

    internal static int InvHN(int HN)
    {
        int Result;
        int R;
        int N;
        if (HN < 37)
        {
            Result = InvHNTable[HN];
        }
        else
        {
            R = ((int)Math.Sqrt((HN - 1) * 12 + 9) + 3) / 6;
            N = R * 6;
            Result = (HN - (R - 1) * R * 3 - 1 + (int)(((uint)N) >> 1)) % N + ((R - 1) * R * 3 + 1);
        }

        return Result;
    }

    internal static void CenterHNtoHX(int Xhx, int Yhx, int HN, ref int NewXhx, ref int NewYhx)
    {
        int R;
        int Side;
        int Pos;
        if (HN != 0)
        {
            R = ((int)Math.Sqrt((HN - 1) * 12 + 9) + 3) / 6;
            HN = HN - (R - 1) * R * 3 - 1;
            Side = HN / R;
            Pos = HN % R;
            switch (Side)
            {
                case 0:
                    HVtoHX(R, Pos - R, ref NewXhx, ref NewYhx);
                    break;
                case 1:
                    NewXhx = R;
                    NewYhx = Pos - (R + 1) / 2;
                    break;
                case 2:
                    HVtoHX(-Pos, R, ref NewXhx, ref NewYhx);
                    break;
                case 3:
                    HVtoHX(-R, R - Pos, ref NewXhx, ref NewYhx);
                    break;
                case 4:
                    NewXhx = -R;
                    NewYhx = -Pos + R / 2;
                    break;
                case 5:
                    HVtoHX(Pos, -R, ref NewXhx, ref NewYhx);
                    break;
            }
        }
        else
        {
            NewXhx = 0;
            NewYhx = 0;
        }

        if ((Xhx & 1) != 0 && (NewXhx & 1) != 0)
        {
            NewYhx = NewYhx + Yhx + 1;
        }
        else
        {
            NewYhx = NewYhx + Yhx;
        }

        NewXhx = NewXhx + Xhx;
    }

    internal static void TranslateHX(int FromXhx, int FromYhx, int ToXhx, int ToYhx, int Xhx, int Yhx, ref int NewXhx, ref int NewYhx)
    {
        int dX;
        int dY;
        dX = ToXhx - FromXhx;
        dY = ToYhx - FromYhx;
        if ((dX & 1) != 0)
        {
            if ((FromXhx & 1) != 0)
            {
                if (!((Xhx & 1) != 0))
                {
                    dY -= 1;
                }
            }
            else
            {
                if ((Xhx & 1) != 0)
                {
                    dY += 1;
                }
            }
        }

        NewXhx = Xhx + dX;
        NewYhx = Yhx + dY;
    }

    internal static void TranslateZeroHX(int Xhx, int Yhx, int dXhx, int dYhx, ref int NewXhx, ref int NewYhx)
    {
        if ((Xhx & 1) != 0 && (dXhx & 1) != 0)
        {
            NewYhx = dYhx + Yhx + 1;
        }
        else
        {
            NewYhx = dYhx + Yhx;
        }

        NewXhx = dXhx + Xhx;
    }

    internal static void GenerateWalkAroundRoute(int Xhx, int Yhx, WalkAroundRouteCallBack CallBack)
    {
        int NXhx;
        int NYhx = default;
        int TXhx = default;
        int TYhx = default;
        int HN;
        bool Found;
        bool Cancel;
        Cancel = false;
        NXhx = 65535;
        Found = false;
        HN = 0;
        while (!Found && HN != 6 && !Cancel)
        {
            HN += 1;
            CenterHNtoHX(Xhx, Yhx, HN, ref NXhx, ref NYhx);
            Found = CallBack(NXhx, NYhx, ref Cancel);
        }

        if (Found && !Cancel)
        {
            Xhx = NXhx;
            Yhx = NYhx;
            while (!Cancel)
            {
                HN += 2;
                if (HN > 6)
                {
                    HN -= 6;
                }

                do
                {
                    CenterHNtoHX(Xhx, Yhx, HN, ref TXhx, ref TYhx);
                    HN -= 1;
                    // { the original asks for NXhx, NYhx here, not for the hexagon it just stepped to }
                    Found = CallBack(NXhx, NYhx, ref Cancel);
                }
                while (!(Found && !Cancel));
                Xhx = TXhx;
                Yhx = TYhx;
            }
        }
    }

    internal static XYL PackXYL(sbyte X, sbyte Y, sbyte L)
    {
        XYL Result = default;
        Result.X = X;
        Result.Y = Y;
        Result.L = L;
        Result.Flag = 0;
        return Result;
    }

    internal static FixedArray_byte_37 InvHNTable = FixedArray_byte_37.From([0, 4, 5, 6, 1, 2, 3, 13, 14, 15, 16, 17, 18, 7, 8, 9, 10, 11, 12, 28, 29, 30, 31, 32, 33, 34, 35, 36, 19, 20, 21, 22, 23, 24, 25, 26, 27]);

    internal static FixedArray_int_11 RadToHNTable = FixedArray_int_11.From([0, 1, 7, 19, 37, 61, 91, 127, 169, 217, 271]);

    public static void OriginalAssert(string Message, string Filename, int LineNumber)
    {
        Assert(false, Message, Filename, LineNumber);
    }

    internal static FixedArray_Aow_Units_FixedArray_Aow_Units_HSEngine_XYTableEntry_8_2 HNWalkTable = FixedArray_Aow_Units_FixedArray_Aow_Units_HSEngine_XYTableEntry_8_2.From([FixedArray_Aow_Units_HSEngine_XYTableEntry_8.From([new XYTableEntry { X = 1, Y = 0 }, new XYTableEntry { X = 0, Y = 1 }, new XYTableEntry { X = -1, Y = 0 }, new XYTableEntry { X = -1, Y = -1 }, new XYTableEntry { X = 0, Y = -1 }, new XYTableEntry { X = 1, Y = -1 }, new XYTableEntry { X = 0, Y = 0 }, new XYTableEntry { X = 0, Y = 0 }]), FixedArray_Aow_Units_HSEngine_XYTableEntry_8.From([new XYTableEntry { X = 1, Y = 1 }, new XYTableEntry { X = 0, Y = 1 }, new XYTableEntry { X = -1, Y = 1 }, new XYTableEntry { X = -1, Y = 0 }, new XYTableEntry { X = 0, Y = -1 }, new XYTableEntry { X = 1, Y = 0 }, new XYTableEntry { X = 0, Y = 0 }, new XYTableEntry { X = 0, Y = 0 }])]);

    internal static int HNtoRad(int HN)
    {
        int Result;
        if (HN < 37)
        {
            Result = HNtoRadTable[HN];
        }
        else
        {
            // { the original lacks the "div 6" of InvHN and HNtoHX here }
            Result = (int)Math.Sqrt((HN - 1) * 12 + 9) + 3;
        }

        return Result;
    }

    internal static void HNtoHX(int HN, ref int Xhx, ref int Yhx)
    {
        int R;
        int Side;
        int Pos;
        if (HN < 37)
        {
            Xhx = HNtoHXTable[HN][0];
            Yhx = HNtoHXTable[HN][1];
        }
        else
        {
            R = ((int)Math.Sqrt((HN - 1) * 12 + 9) + 3) / 6;
            HN = HN - (R - 1) * R * 3 - 1;
            Side = HN / R;
            Pos = HN % R;
            switch (Side)
            {
                case 0:
                    HVtoHX(R, Pos - R, ref Xhx, ref Yhx);
                    break;
                case 1:
                    Xhx = R;
                    Yhx = Pos - (R + 1) / 2;
                    break;
                case 2:
                    HVtoHX(-Pos, R, ref Xhx, ref Yhx);
                    break;
                case 3:
                    HVtoHX(-R, R - Pos, ref Xhx, ref Yhx);
                    break;
                case 4:
                    Xhx = -R;
                    Yhx = -Pos + R / 2;
                    break;
                case 5:
                    HVtoHX(Pos, -R, ref Xhx, ref Yhx);
                    break;
            }
        }
    }

    internal static void HNtoHP(int HN, ref int Xhp, ref int Yhp)
    {
        HNtoHX(HN, ref Xhp, ref Yhp);
        HXtoHP(Xhp, Yhp, ref Xhp, ref Yhp);
        Xhp -= 8;
    }

    internal static FixedArray_Aow_Units_FixedArray_Aow_Units_HSEngine_XYTableEntry_8_2 HNtoXYTable = FixedArray_Aow_Units_FixedArray_Aow_Units_HSEngine_XYTableEntry_8_2.From([FixedArray_Aow_Units_HSEngine_XYTableEntry_8.From([new XYTableEntry { X = 0, Y = 0 }, new XYTableEntry { X = 0, Y = -1 }, new XYTableEntry { X = 1, Y = -1 }, new XYTableEntry { X = 1, Y = 0 }, new XYTableEntry { X = 0, Y = 1 }, new XYTableEntry { X = -1, Y = 0 }, new XYTableEntry { X = -1, Y = -1 }, new XYTableEntry { X = 0, Y = 0 }]), FixedArray_Aow_Units_HSEngine_XYTableEntry_8.From([new XYTableEntry { X = 0, Y = 0 }, new XYTableEntry { X = 0, Y = -1 }, new XYTableEntry { X = 1, Y = 0 }, new XYTableEntry { X = 1, Y = 1 }, new XYTableEntry { X = 0, Y = 1 }, new XYTableEntry { X = -1, Y = 1 }, new XYTableEntry { X = -1, Y = 0 }, new XYTableEntry { X = 0, Y = 0 }])]);

    internal static FixedArray_byte_37 HNtoRadTable = FixedArray_byte_37.From([0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3]);

    internal static FixedArray_Aow_Units_FixedArray_int_2_37 HNtoHXTable = FixedArray_Aow_Units_FixedArray_int_2_37.From([FixedArray_int_2.From([0, 0]), FixedArray_int_2.From([0, -1]), FixedArray_int_2.From([1, -1]), FixedArray_int_2.From([1, 0]), FixedArray_int_2.From([0, 1]), FixedArray_int_2.From([-1, 0]), FixedArray_int_2.From([-1, -1]), FixedArray_int_2.From([0, -2]), FixedArray_int_2.From([1, -2]), FixedArray_int_2.From([2, -1]), FixedArray_int_2.From([2, 0]), FixedArray_int_2.From([2, 1]), FixedArray_int_2.From([1, 1]), FixedArray_int_2.From([0, 2]), FixedArray_int_2.From([-1, 1]), FixedArray_int_2.From([-2, 1]), FixedArray_int_2.From([-2, 0]), FixedArray_int_2.From([-2, -1]), FixedArray_int_2.From([-1, -2]), FixedArray_int_2.From([0, -3]), FixedArray_int_2.From([1, -3]), FixedArray_int_2.From([2, -2]), FixedArray_int_2.From([3, -2]), FixedArray_int_2.From([3, -1]), FixedArray_int_2.From([3, 0]), FixedArray_int_2.From([3, 1]), FixedArray_int_2.From([2, 2]), FixedArray_int_2.From([1, 2]), FixedArray_int_2.From([0, 3]), FixedArray_int_2.From([-1, 2]), FixedArray_int_2.From([-2, 2]), FixedArray_int_2.From([-3, 1]), FixedArray_int_2.From([-3, 0]), FixedArray_int_2.From([-3, -1]), FixedArray_int_2.From([-3, -2]), FixedArray_int_2.From([-2, -2]), FixedArray_int_2.From([-1, -3])]);

    internal static void HPtoHX(int horizontal, int vertical, ref int x, ref int y)
    {
        int column = horizontal - 8 >> 5;
        x = column;
        y = vertical - (column & 1) * 16 >> 5;
    }

    internal static void HPtoHXs(int horizontal, int vertical, ref sbyte x, ref sbyte y)
    {
        int column = horizontal - 8 >> 5;
        Unsafe.WriteUnaligned(ref Unsafe.As<sbyte, byte>(ref x), column);
        int row = vertical - (column & 1) * 16 >> 5;
        Unsafe.WriteUnaligned(ref Unsafe.As<sbyte, byte>(ref y), row);
    }

    internal static int HPtoHN(int Xhp, int Yhp)
    {
        HPtoHX(Xhp, Yhp, ref Xhp, ref Yhp);
        return HXtoHN(Xhp, Yhp);
    }

    internal static int HXtoV(int x, int y) => ((x + 1) >> 1) + y;

    internal static int HXtoH(int x, int y)
    {
        int difference = x - 2 * y;
        return difference == int.MinValue ? 1 << 30 : difference >> 1;
    }

    internal static int dHXtoRad(int X1, int Y1, int X2, int Y2)
    {
        int Result;
        int dX;
        int dY;
        int Half;
        bool Up;
        dX = Math.Abs(X1 - X2);
        dY = Math.Abs(Y1 - Y2);
        if (dY >= dX)
        {
            Up = (dX & 1) != 0 && ((!((X1 & 1) != 0) && Y1 < Y2) || (!((X2 & 1) != 0) && Y2 < Y1));
            if (Up)
            {
                Result = (int)(((uint)(dX + 1)) >> 1) + dY;
            }
            else
            {
                Result = (int)(((uint)dX) >> 1) + dY;
            }
        }
        else
        {
            Result = dX;
            Half = (int)(((uint)(dX + 1)) >> 1);
            if ((dX & 1) != 0 && ((!((X1 & 1) != 0) && Y1 < Y2) || (!((X2 & 1) != 0) && Y2 < Y1)))
            {
                dY += 1;
            }

            if (dY > Half)
            {
                Result += dY - Half;
            }
        }

        return Result;
    }

    internal static int HXtoRad(int Xhx, int Yhx)
    {
        int Result;
        int dX;
        int dY;
        int Half;
        bool Up;
        dX = Math.Abs(Xhx);
        dY = Math.Abs(Yhx);
        if (dY >= dX)
        {
            Up = (dX & 1) != 0 && (Yhx > 0 || (!((Xhx & 1) != 0) && Yhx < 0));
            if (Up)
            {
                Result = (int)(((uint)(dX + 1)) >> 1) + dY;
            }
            else
            {
                Result = (int)(((uint)dX) >> 1) + dY;
            }
        }
        else
        {
            Result = dX;
            Half = (int)(((uint)(dX + 1)) >> 1);
            if ((dX & 1) != 0 && (Yhx > 0 || (!((Xhx & 1) != 0) && Yhx < 0)))
            {
                dY += 1;
            }

            if (dY > Half)
            {
                Result += dY - Half;
            }
        }

        return Result;
    }

    internal static void HXtoHV(int Xhx, int Yhx, ref int H, ref int V)
    {
        H = HXtoH(Xhx, Yhx);
        V = HXtoV(Xhx, Yhx);
    }

    internal static int HXtoHN(int Xhx, int Yhx)
    {
        int Result;
        int R;
        int Side;
        int Pos;
        int H = default;
        int V = default;
        R = HXtoRad(Xhx, Yhx);
        if (R != 0)
        {
            HXtoHV(Xhx, Yhx, ref H, ref V);
            if (R == H)
            {
                Side = 0;
                Pos = V + R;
            }
            else
            {
                if (R == V)
                {
                    Side = 2;
                    Pos = -H;
                }
                else
                {
                    if (-R == H)
                    {
                        Side = 3;
                        Pos = -V + R;
                    }
                    else
                    {
                        if (-R == V)
                        {
                            Side = 5;
                            Pos = H;
                        }
                        else
                        {
                            if (R == Xhx)
                            {
                                Side = 1;
                                Pos = (Xhx + 1) / 2 + Yhx;
                            }
                            else
                            {
                                if (R == -Xhx)
                                {
                                    Side = 4;
                                    Pos = -Yhx - Xhx / 2;
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

            Result = Pos + ((R - 1) * R * 3 + 1 + Side * R);
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    internal static int dHXtoHN(int X1, int Y1, int X2, int Y2)
    {
        TranslateHX(X1, Y1, 0, 0, X2, Y2, ref X2, ref Y2);
        return HXtoHN(X2, Y2);
    }

    internal static int dHXtoHNfast(int X1, int Y1, int X2, int Y2)
    {
        int Result;
        if (X1 == X2)
        {
            if (Y2 < Y1)
            {
                Result = 1;
            }
            else
            {
                if (Y2 > Y1)
                {
                    Result = 4;
                }
                else
                {
                    Result = 0;
                }
            }
        }
        else
        {
            if (!((X1 & 1) != 0))
            {
                if (X2 < X1)
                {
                    if (Y2 < Y1)
                    {
                        Result = 6;
                    }
                    else
                    {
                        Result = 5;
                    }
                }
                else
                {
                    if (Y2 < Y1)
                    {
                        Result = 2;
                    }
                    else
                    {
                        Result = 3;
                    }
                }
            }
            else
            {
                if (X2 < X1)
                {
                    if (Y2 > Y1)
                    {
                        Result = 5;
                    }
                    else
                    {
                        Result = 6;
                    }
                }
                else
                {
                    if (Y2 > Y1)
                    {
                        Result = 3;
                    }
                    else
                    {
                        Result = 2;
                    }
                }
            }
        }

        return Result;
    }

    internal static void HXtoHP(int Xhx, int Yhx, ref int Xhp, ref int Yhp)
    {
        Xhp = Xhx * 32 + 8;
        Yhp = Yhx * 32 + (Xhx & 1) * 16;
    }

    internal static int HXtoHT(int Xhx, int Yhx)
    {
        int Result;
        int H;
        H = HXtoH(Xhx, Yhx);
        if (H > 0)
        {
            if (Xhx < 0)
            {
                Result = 6;
            }
            else
            {
                if (HXtoV(Xhx, Yhx) < 0)
                {
                    Result = 1;
                }
                else
                {
                    Result = 2;
                }
            }
        }
        else
        {
            if (H < 0)
            {
                if (Xhx > 0)
                {
                    Result = 3;
                }
                else
                {
                    if (HXtoV(Xhx, Yhx) > 0)
                    {
                        Result = 4;
                    }
                    else
                    {
                        Result = 5;
                    }
                }
            }
            else
            {
                if (Xhx < 0)
                {
                    Result = 6;
                }
                else
                {
                    if (Xhx > 0)
                    {
                        Result = 3;
                    }
                    else
                    {
                        Result = 0;
                    }
                }
            }
        }

        return Result;
    }

    internal static int dHXtoHT(int X1, int Y1, int X2, int Y2)
    {
        X2 = X2 - X1;
        Y2 = Y2 - Y1;
        if ((X2 & 1) != 0 && (X1 & 1) != 0)
        {
            Y2 -= 1;
        }

        return HXtoHT(X2, Y2);
    }

}
