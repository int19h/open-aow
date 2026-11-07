using Aow.Graphics;
using Aow.Rtl;
using Aow.Units.GFXE;

namespace Aow.Units.ImageLib;

public partial class TPicture16
{
    public override void ShowOpaqueStretched(in TRect R, in GFXE.TSurfaceDesc SD)
    {
        Span<int> RowTable;
        Span<int> ColTable;
        int W;
        SurfaceView Dst;
        int XPos;
        int YPos;
        int XStep;
        int YStep;
        int SX;
        int SY;
        float XScale;
        float YScale;
        int H;
        int L;
        int T;
        int Rt;
        int B;
        int C;
        int D;
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
                Dst = SD.View(L * 2 + SD.Pitch * T);
                // One table of H row offsets followed by W columns, as the original's GetMem.
                int[] Tables = new int[W + H];
                RowTable = Tables.AsSpan(0, H);
                ColTable = Tables.AsSpan(H, W);
                XStep = RtlUnit.Round((double)65535 / XScale);
                if (SX != 0)
                {
                    XPos = RtlUnit.Round((double)((double)SX / XScale) * 65535);
                }
                else
                {
                    XPos = 0;
                }

                for (int I = 0; I <= W - 1; I++)
                {
                    C = (int)(((uint)XPos) >> 16);
                    if (C >= FDataWidth)
                    {
                        C = FDataWidth - 1;
                    }

                    ColTable[I] = C;
                    XPos += XStep;
                }

                YStep = RtlUnit.Round((double)65535 / YScale);
                if (SY != 0)
                {
                    YPos = RtlUnit.Round((double)((double)SY / YScale) * 65535);
                }
                else
                {
                    YPos = 0;
                }

                if (!FDataLoaded)
                {
                    LoadDiscardableImageData();
                    for (int I = 0; I <= H - 1; I++)
                    {
                        C = (int)(((uint)YPos) >> 16);
                        if (C >= FDataHeight)
                        {
                            C = FDataHeight - 1;
                        }

                        RowTable[I] = C * FPitch;
                        YPos += YStep;
                    }

                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        GFXEUnit.BltStretched16(SurfaceView.Of(RtlUnit.BytePointer(FData), FPitch), RowTable, ColTable, Dst, W, H);
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw Rtl.Exception.Create("Surface pixel format not supported!");
                        }

                        GFXEUnit.BltStretched16(SurfaceView.Of(RtlUnit.BytePointer(FData), FPitch), RowTable, ColTable, Dst, W, H);
                    }

                    if (FLoadMode == TLoadMode.lmLoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    for (int I = 0; I <= H - 1; I++)
                    {
                        C = (int)(((uint)YPos) >> 16);
                        if (C >= FDataHeight)
                        {
                            C = FDataHeight - 1;
                        }

                        RowTable[I] = C * FPitch;
                        YPos += YStep;
                    }

                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        GFXEUnit.BltStretched16(SurfaceView.Of(RtlUnit.BytePointer(FData), FPitch), RowTable, ColTable, Dst, W, H);
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw Rtl.Exception.Create("Surface pixel format not supported!");
                        }

                        GFXEUnit.BltStretched16(SurfaceView.Of(RtlUnit.BytePointer(FData), FPitch), RowTable, ColTable, Dst, W, H);
                    }
                }
            }
        }
    }

    public override void ShowBlendedStretched(in TRect R, in GFXE.TSurfaceDesc SD, TBlendTable Table)
    {
        Span<int> RowTable;
        Span<int> ColTable;
        int W;
        int H;
        int Rt;
        SurfaceView Dst;
        int XPos;
        int YPos;
        int XStep;
        int YStep;
        int SX;
        int SY;
        float XScale;
        float YScale;
        int L;
        int T;
        int B;
        int C;
        int D;
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
                Dst = SD.View(L * 2 + SD.Pitch * T);
                // One table of H row offsets followed by W columns, as the original's GetMem.
                int[] Tables = new int[W + H];
                RowTable = Tables.AsSpan(0, H);
                ColTable = Tables.AsSpan(H, W);
                XStep = RtlUnit.Round((double)65535 / XScale);
                if (SX != 0)
                {
                    XPos = RtlUnit.Round((double)((double)SX / XScale) * 65535);
                }
                else
                {
                    XPos = 0;
                }

                for (int I = 0; I <= W - 1; I++)
                {
                    C = (int)(((uint)XPos) >> 16);
                    if (C >= FDataWidth)
                    {
                        C = FDataWidth - 1;
                    }

                    ColTable[I] = C;
                    XPos += XStep;
                }

                YStep = RtlUnit.Round((double)65535 / YScale);
                if (SY != 0)
                {
                    YPos = RtlUnit.Round((double)((double)SY / YScale) * 65535);
                }
                else
                {
                    YPos = 0;
                }

                if (!FDataLoaded)
                {
                    LoadDiscardableImageData();
                    for (int I = 0; I <= H - 1; I++)
                    {
                        C = (int)(((uint)YPos) >> 16);
                        if (C >= FDataHeight)
                        {
                            C = FDataHeight - 1;
                        }

                        RowTable[I] = C * FPitch;
                        YPos += YStep;
                    }

                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        switch (SD.PixelFormatID)
                        {
                            case GFXEUnit.pfRGB555:
                                GFXEUnit.BlendStretched555(SurfaceView.Of(RtlUnit.BytePointer(FData), FPitch), RowTable, ColTable, Dst, W, H, ((TBlendTable5Bit)Table).Bytes);
                                break;
                            case GFXEUnit.pfRGB565:
                                GFXEUnit.BlendStretched565(SurfaceView.Of(RtlUnit.BytePointer(FData), FPitch), RowTable, ColTable, Dst, W, H, ((TBlendTable5Bit)Table).Bytes);
                                break;
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw Rtl.Exception.Create("Surface pixel format not supported!");
                        }

                        switch (SD.PixelFormatID)
                        {
                            case GFXEUnit.pfRGB555:
                                GFXEUnit.BlendStretched555(SurfaceView.Of(RtlUnit.BytePointer(FData), FPitch), RowTable, ColTable, Dst, W, H, ((TBlendTable5Bit)Table).Bytes);
                                break;
                            case GFXEUnit.pfRGB565:
                                GFXEUnit.BlendStretched565(SurfaceView.Of(RtlUnit.BytePointer(FData), FPitch), RowTable, ColTable, Dst, W, H, ((TBlendTable5Bit)Table).Bytes);
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
                    for (int I = 0; I <= H - 1; I++)
                    {
                        C = (int)(((uint)YPos) >> 16);
                        if (C >= FDataHeight)
                        {
                            C = FDataHeight - 1;
                        }

                        RowTable[I] = C * FPitch;
                        YPos += YStep;
                    }

                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        switch (SD.PixelFormatID)
                        {
                            case GFXEUnit.pfRGB555:
                                GFXEUnit.BlendStretched555(SurfaceView.Of(RtlUnit.BytePointer(FData), FPitch), RowTable, ColTable, Dst, W, H, ((TBlendTable5Bit)Table).Bytes);
                                break;
                            case GFXEUnit.pfRGB565:
                                GFXEUnit.BlendStretched565(SurfaceView.Of(RtlUnit.BytePointer(FData), FPitch), RowTable, ColTable, Dst, W, H, ((TBlendTable5Bit)Table).Bytes);
                                break;
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw Rtl.Exception.Create("Surface pixel format not supported!");
                        }

                        switch (SD.PixelFormatID)
                        {
                            case GFXEUnit.pfRGB555:
                                GFXEUnit.BlendStretched555(SurfaceView.Of(RtlUnit.BytePointer(FData), FPitch), RowTable, ColTable, Dst, W, H, ((TBlendTable5Bit)Table).Bytes);
                                break;
                            case GFXEUnit.pfRGB565:
                                GFXEUnit.BlendStretched565(SurfaceView.Of(RtlUnit.BytePointer(FData), FPitch), RowTable, ColTable, Dst, W, H, ((TBlendTable5Bit)Table).Bytes);
                                break;
                        }
                    }
                }
            }
        }
    }
}
