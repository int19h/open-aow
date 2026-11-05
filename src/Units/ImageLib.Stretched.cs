using Aow.Graphics;
using Aow.Rtl;
using Aow.Units.GFXE;
namespace Aow.Units.ImageLib;

public partial class TPicture16
{
    public override void ShowOpaqueStretched(in TRect R, in GFXE.TSurfaceDesc SD)
    {
        Ptr<int> RowTable = default;
        Ptr<int> ColTable = default;
        int W = default;
        object Dst = default;
        int XPos = default;
        int YPos = default;
        int XStep = default;
        int YStep = default;
        int SX = default;
        int SY = default;
        float XScale = default;
        float YScale = default;
        int H = default;
        int L = default;
        int T = default;
        int Rt = default;
        int B = default;
        int I = default;
        int C = default;
        int D = default;
        W = R.Right - R.Left;
        H = R.Bottom - R.Top;
        if (W > 0 && H > 0)
        {
            XScale = (float)((double)W / FDataWidth);
            YScale = (float)((double)H / FHeight);
            L = R.Left;
            if (L < SD.ClipRect.Left)
            {
                SX = SD.ClipRect.Left - L;
                L += SX;
            }
            else
            {
                SX = 0;
            }

            T = R.Top;
            if (T < SD.ClipRect.Top)
            {
                SY = SD.ClipRect.Top - T;
                T += SY;
            }
            else
            {
                SY = 0;
            }

            Rt = R.Right;
            if (Rt > SD.ClipRect.Right)
            {
                D = Rt - SD.ClipRect.Right;
                Rt -= D;
            }

            B = R.Bottom;
            if (B > SD.ClipRect.Bottom)
            {
                D = B - SD.ClipRect.Bottom;
                B -= D;
            }

            W = Rt - L;
            H = B - T;
            if (W > 0 && H > 0)
            {
                Dst = (Rtl.Rtl.BytePointer(SD.Data) + L * 2 + SD.Pitch * T).Box();
                RowTable = Ptr<int>.Alloc(W + H);
                ColTable = (RowTable.Reinterpret<byte>() + H * 4).Reinterpret<int>();
                XStep = Rtl.Rtl.Round((double)65535 / XScale);
                if (SX != 0)
                {
                    XPos = Rtl.Rtl.Round((double)((double)SX / XScale) * 65535);
                }
                else
                {
                    XPos = 0;
                }

                {
                    I = 0;
                    int end__122 = W - 1;
                    for (; I <= end__122; I += 1)
                    {
                        C = (int)(((uint)XPos) >> 16);
                        if (C >= FDataWidth)
                        {
                            C = FDataWidth - 1;
                        }

                        ColTable[I] = C;
                        XPos += XStep;
                    }
                }

                YStep = Rtl.Rtl.Round((double)65535 / YScale);
                if (SY != 0)
                {
                    YPos = Rtl.Rtl.Round((double)((double)SY / YScale) * 65535);
                }
                else
                {
                    YPos = 0;
                }

                if (!FDataLoaded)
                {
                    LoadDiscardableImageData();
                    {
                        I = 0;
                        int end__123 = H - 1;
                        for (; I <= end__123; I += 1)
                        {
                            C = (int)(((uint)YPos) >> 16);
                            if (C >= FDataHeight)
                            {
                                C = FDataHeight - 1;
                            }

                            RowTable[I] = C * FPitch;
                            YPos += YStep;
                        }
                    }

                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        GFXE.GFXE.BltStretched16(new StretchRows(Rtl.Rtl.BytePointer(FData), RowTable.AsSpan(H).ToArray()), ColTable.Box(), Dst, SD.Pitch, W, H);
                    }
                    else
                    {
                        var call__124 = RemapToPixelFormat(SD.PixelFormatID);
                        if (call__124 != 0)
                        {
                            throw Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                        }

                        GFXE.GFXE.BltStretched16(new StretchRows(Rtl.Rtl.BytePointer(FData), RowTable.AsSpan(H).ToArray()), ColTable.Box(), Dst, SD.Pitch, W, H);
                    }

                    if (FLoadMode == TLoadMode.lmLoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    {
                        I = 0;
                        int end__125 = H - 1;
                        for (; I <= end__125; I += 1)
                        {
                            C = (int)(((uint)YPos) >> 16);
                            if (C >= FDataHeight)
                            {
                                C = FDataHeight - 1;
                            }

                            RowTable[I] = C * FPitch;
                            YPos += YStep;
                        }
                    }

                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        GFXE.GFXE.BltStretched16(new StretchRows(Rtl.Rtl.BytePointer(FData), RowTable.AsSpan(H).ToArray()), ColTable.Box(), Dst, SD.Pitch, W, H);
                    }
                    else
                    {
                        var call__126 = RemapToPixelFormat(SD.PixelFormatID);
                        if (call__126 != 0)
                        {
                            throw Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                        }

                        GFXE.GFXE.BltStretched16(new StretchRows(Rtl.Rtl.BytePointer(FData), RowTable.AsSpan(H).ToArray()), ColTable.Box(), Dst, SD.Pitch, W, H);
                    }
                }

                RowTable = default;
            }
        }
    }
    public override void ShowBlendedStretched(in TRect R, in GFXE.TSurfaceDesc SD, TBlendTable Table)
    {
        Ptr<int> RowTable = default;
        Ptr<int> ColTable = default;
        int W = default;
        int H = default;
        int Rt = default;
        object Dst = default;
        int XPos = default;
        int YPos = default;
        int XStep = default;
        int YStep = default;
        int SX = default;
        int SY = default;
        float XScale = default;
        float YScale = default;
        int L = default;
        int T = default;
        int B = default;
        int I = default;
        int C = default;
        int D = default;
        W = R.Right - R.Left;
        H = R.Bottom - R.Top;
        if (W > 0 && H > 0)
        {
            XScale = (float)((double)W / FDataWidth);
            YScale = (float)((double)H / FHeight);
            L = R.Left;
            if (L < SD.ClipRect.Left)
            {
                SX = SD.ClipRect.Left - L;
                L += SX;
            }
            else
            {
                SX = 0;
            }

            T = R.Top;
            if (T < SD.ClipRect.Top)
            {
                SY = SD.ClipRect.Top - T;
                T += SY;
            }
            else
            {
                SY = 0;
            }

            Rt = R.Right;
            if (Rt > SD.ClipRect.Right)
            {
                D = Rt - SD.ClipRect.Right;
                Rt -= D;
            }

            B = R.Bottom;
            if (B > SD.ClipRect.Bottom)
            {
                D = B - SD.ClipRect.Bottom;
                B -= D;
            }

            W = Rt - L;
            H = B - T;
            if (W > 0 && H > 0)
            {
                Dst = (Rtl.Rtl.BytePointer(SD.Data) + L * 2 + SD.Pitch * T).Box();
                RowTable = Ptr<int>.Alloc(W + H);
                ColTable = (RowTable.Reinterpret<byte>() + H * 4).Reinterpret<int>();
                XStep = Rtl.Rtl.Round((double)65535 / XScale);
                if (SX != 0)
                {
                    XPos = Rtl.Rtl.Round((double)((double)SX / XScale) * 65535);
                }
                else
                {
                    XPos = 0;
                }

                {
                    I = 0;
                    int end__127 = W - 1;
                    for (; I <= end__127; I += 1)
                    {
                        C = (int)(((uint)XPos) >> 16);
                        if (C >= FDataWidth)
                        {
                            C = FDataWidth - 1;
                        }

                        ColTable[I] = C;
                        XPos += XStep;
                    }
                }

                YStep = Rtl.Rtl.Round((double)65535 / YScale);
                if (SY != 0)
                {
                    YPos = Rtl.Rtl.Round((double)((double)SY / YScale) * 65535);
                }
                else
                {
                    YPos = 0;
                }

                if (!FDataLoaded)
                {
                    LoadDiscardableImageData();
                    {
                        I = 0;
                        int end__128 = H - 1;
                        for (; I <= end__128; I += 1)
                        {
                            C = (int)(((uint)YPos) >> 16);
                            if (C >= FDataHeight)
                            {
                                C = FDataHeight - 1;
                            }

                            RowTable[I] = C * FPitch;
                            YPos += YStep;
                        }
                    }

                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        switch (SD.PixelFormatID)
                        {
                            case GFXE.GFXE.pfRGB555:
                                GFXE.GFXE.BlendStretched555(new StretchRows(Rtl.Rtl.BytePointer(FData), RowTable.AsSpan(H).ToArray()), ColTable.Box(), Dst, SD.Pitch, W, H, Ptr<FixedArray_byte_32>.To(((TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                break;
                            case GFXE.GFXE.pfRGB565:
                                GFXE.GFXE.BlendStretched565(new StretchRows(Rtl.Rtl.BytePointer(FData), RowTable.AsSpan(H).ToArray()), ColTable.Box(), Dst, SD.Pitch, W, H, Ptr<FixedArray_byte_32>.To(((TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                break;
                        }
                    }
                    else
                    {
                        var call__129 = RemapToPixelFormat(SD.PixelFormatID);
                        if (call__129 != 0)
                        {
                            throw Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                        }

                        switch (SD.PixelFormatID)
                        {
                            case GFXE.GFXE.pfRGB555:
                                GFXE.GFXE.BlendStretched555(new StretchRows(Rtl.Rtl.BytePointer(FData), RowTable.AsSpan(H).ToArray()), ColTable.Box(), Dst, SD.Pitch, W, H, Ptr<FixedArray_byte_32>.To(((TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                break;
                            case GFXE.GFXE.pfRGB565:
                                GFXE.GFXE.BlendStretched565(new StretchRows(Rtl.Rtl.BytePointer(FData), RowTable.AsSpan(H).ToArray()), ColTable.Box(), Dst, SD.Pitch, W, H, Ptr<FixedArray_byte_32>.To(((TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                break;
                        }
                    }

                    if (FLoadMode == TLoadMode.lmLoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    {
                        I = 0;
                        int end__130 = H - 1;
                        for (; I <= end__130; I += 1)
                        {
                            C = (int)(((uint)YPos) >> 16);
                            if (C >= FDataHeight)
                            {
                                C = FDataHeight - 1;
                            }

                            RowTable[I] = C * FPitch;
                            YPos += YStep;
                        }
                    }

                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        switch (SD.PixelFormatID)
                        {
                            case GFXE.GFXE.pfRGB555:
                                GFXE.GFXE.BlendStretched555(new StretchRows(Rtl.Rtl.BytePointer(FData), RowTable.AsSpan(H).ToArray()), ColTable.Box(), Dst, SD.Pitch, W, H, Ptr<FixedArray_byte_32>.To(((TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                break;
                            case GFXE.GFXE.pfRGB565:
                                GFXE.GFXE.BlendStretched565(new StretchRows(Rtl.Rtl.BytePointer(FData), RowTable.AsSpan(H).ToArray()), ColTable.Box(), Dst, SD.Pitch, W, H, Ptr<FixedArray_byte_32>.To(((TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                break;
                        }
                    }
                    else
                    {
                        var call__131 = RemapToPixelFormat(SD.PixelFormatID);
                        if (call__131 != 0)
                        {
                            throw Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                        }

                        switch (SD.PixelFormatID)
                        {
                            case GFXE.GFXE.pfRGB555:
                                GFXE.GFXE.BlendStretched555(new StretchRows(Rtl.Rtl.BytePointer(FData), RowTable.AsSpan(H).ToArray()), ColTable.Box(), Dst, SD.Pitch, W, H, Ptr<FixedArray_byte_32>.To(((TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                break;
                            case GFXE.GFXE.pfRGB565:
                                GFXE.GFXE.BlendStretched565(new StretchRows(Rtl.Rtl.BytePointer(FData), RowTable.AsSpan(H).ToArray()), ColTable.Box(), Dst, SD.Pitch, W, H, Ptr<FixedArray_byte_32>.To(((TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                break;
                        }
                    }
                }

                RowTable = default;
            }
        }
    }
}
