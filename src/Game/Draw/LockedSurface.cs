
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// GFXE.pas: the description of a locked surface (graphics.md 3.1). The original record is 60
// bytes and its assembler readers use the field offsets. No reader in the port sees it as bytes,
// so it is a plain struct. The original's `Data: Pointer` is two fields: Data, the block of memory
// that holds the surface's rows, and DataIndex, the byte index of the first byte of row 0 in it.
// `Pointer(Integer(SD.Data) + n)` is DataIndex + n, and `SD.Data = nil` is a null Data.
public struct LockedSurface
{
    public int Width;
    public int Height;
    public int BitCount;
    public int Pitch;
    public Rect ClipRect;
    public byte[] Data;
    public int DataIndex;
    public Palette Palette;
    public int PixelFormatID;
    public int RMask;
    public int GMask;
    public int BMask;
    public int AMask;

    // The view of the surface's rows for the blitters, moved by a byte offset from the first
    // pixel of row 0 (`Pointer(Integer(SD.Data) + Offset)` with `SD.Pitch` in the original), with
    // the descriptor's clip rectangle.
    public readonly SurfaceView View(int offset = 0) => new(Data, DataIndex + offset, Pitch, ClipRect);

    internal static int BltSprite16(in LockedSurface Src, Rect SrcRect, in LockedSurface Dest, int X, int Y, int ColorKey)
    {
        int Result;
        int Key;
        SurfaceView S;
        SurfaceView D;
        if (Src.BitCount == 16 && Dest.PixelFormatID == Src.PixelFormatID)
        {
            Key = PixelFormats.Color2PixelInfo(ColorKey, Src.PixelFormatID, null);
            S = Src.View(Src.Pitch * SrcRect.Top + SrcRect.Left * 2);
            D = Dest.View(Dest.Pitch * Y + X * 2);
            BlitRects.BltColorKey16(S, D, SrcRect.Right - SrcRect.Left, SrcRect.Bottom - SrcRect.Top, Key);
            Result = PixelFormats.GFXE_OK;
        }
        else
        {
            Result = PixelFormats.GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return Result;
    }

    internal static int BltSDRectClipped(in LockedSurface Src, Rect SrcRect, in LockedSurface Dest, int X, int Y)
    {
        int Result;
        int W;
        int H;
        SurfaceView S;
        SurfaceView D;
        if (SrcRect.Right > Src.ClipRect.Right)
        {
            SrcRect.Right = Src.ClipRect.Right;
        }

        if (SrcRect.Left < Src.ClipRect.Left)
        {
            X = X + (Src.ClipRect.Left - SrcRect.Left);
            SrcRect.Left = Src.ClipRect.Left;
        }

        if (X < Dest.ClipRect.Left)
        {
            SrcRect.Left = SrcRect.Left + (Dest.ClipRect.Left - X);
            X = Dest.ClipRect.Left;
        }

        W = SrcRect.Right - SrcRect.Left;
        if (X + W > Dest.ClipRect.Right)
        {
            W = W - (X + W - Dest.ClipRect.Right);
        }

        if (SrcRect.Bottom > Src.ClipRect.Bottom)
        {
            SrcRect.Bottom = Src.ClipRect.Bottom;
        }

        if (SrcRect.Top < Src.ClipRect.Top)
        {
            Y = Y + (Src.ClipRect.Top - SrcRect.Top);
            SrcRect.Top = Src.ClipRect.Top;
        }

        if (Dest.ClipRect.Top > Y)
        {
            SrcRect.Top = SrcRect.Top + (Dest.ClipRect.Top - Y);
            Y = Dest.ClipRect.Top;
        }

        H = SrcRect.Bottom - SrcRect.Top;
        if (Y + H > Dest.ClipRect.Bottom)
        {
            H = H - (Y + H - Dest.ClipRect.Bottom);
        }

        if (Src.PixelFormatID == Dest.PixelFormatID)
        {
            if (W > 0 && H > 0)
            {
                W = (int)(((uint)(Src.BitCount * W)) >> 3);
                S = Src.View(Src.Pitch * SrcRect.Top + (int)(((uint)(Src.BitCount * SrcRect.Left)) >> 3));
                D = Dest.View(Dest.Pitch * Y + (int)(((uint)(Dest.BitCount * X)) >> 3));
                BlitRects.BltRect(S, D, W, H);
            }

            Result = PixelFormats.GFXE_OK;
        }
        else
        {
            Result = PixelFormats.GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return Result;
    }

    internal static int BltSpriteClipped16(in LockedSurface Src, Rect SrcRect, in LockedSurface Dest, int X, int Y, int ColorKey)
    {
        int Result;
        int W;
        int H;
        SurfaceView S;
        SurfaceView D;
        int Key;
        if (Src.BitCount == 16 && Dest.BitCount == 16)
        {
            if (SrcRect.Right > Src.ClipRect.Right)
            {
                SrcRect.Right = Src.ClipRect.Right;
            }

            if (SrcRect.Left < Src.ClipRect.Left)
            {
                X = X + (Src.ClipRect.Left - SrcRect.Left);
                SrcRect.Left = Src.ClipRect.Left;
            }

            if (X < Dest.ClipRect.Left)
            {
                SrcRect.Left = SrcRect.Left + (Dest.ClipRect.Left - X);
                X = Dest.ClipRect.Left;
            }

            W = SrcRect.Right - SrcRect.Left;
            if (W + X > Dest.ClipRect.Right)
            {
                W = W - (W + X - Dest.ClipRect.Right);
            }

            if (SrcRect.Bottom > Src.ClipRect.Bottom)
            {
                SrcRect.Bottom = Src.ClipRect.Bottom;
            }

            if (SrcRect.Top < Src.ClipRect.Top)
            {
                Y = Y + (Src.ClipRect.Top - SrcRect.Top);
                SrcRect.Top = Src.ClipRect.Top;
            }

            if (Y < Dest.ClipRect.Top)
            {
                SrcRect.Top = SrcRect.Top + (Dest.ClipRect.Top - Y);
                Y = Dest.ClipRect.Top;
            }

            H = SrcRect.Bottom - SrcRect.Top;
            if (Y + H > Dest.ClipRect.Bottom)
            {
                H = H - (Y + H - Dest.ClipRect.Bottom);
            }

            if (W > 0 && H > 0)
            {
                Key = PixelFormats.Color2PixelInfo(ColorKey, Src.PixelFormatID, null);
                S = Src.View(Src.Pitch * SrcRect.Top + SrcRect.Left * 2);
                D = Dest.View(Dest.Pitch * Y);
                X += X;
                D = D.At(X);
                BlitRects.BltColorKey16(S, D, W, H, Key);
            }

            Result = PixelFormats.GFXE_OK;
        }
        else
        {
            Result = PixelFormats.GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return Result;
    }

    public static void CreateSDfromSD(in LockedSurface Src, in Rect R, ref LockedSurface Dest)
    {
        Dest = Src;
        IntersectRect(ref Dest.ClipRect, Src.ClipRect, R);
        Dest.DataIndex = Src.DataIndex + R.Top * Src.Pitch + (int)(((uint)Src.BitCount) >> 3) * R.Left;
        Dest.ClipRect.Left -= R.Left;
        Dest.ClipRect.Right -= R.Left;
        Dest.ClipRect.Top -= R.Top;
        Dest.ClipRect.Bottom -= R.Top;
    }

    internal static void MinimizeTransparentRect(in LockedSurface SD, in Rect R, int TransparentColor, ref Rect Result)
    {
        LockedSurface SD__value = SD;
        bool Found;
        int X;
        int Y;
        Rect Rect;
        int GetPixel(int X, int Y)
        {
            int Result;
            switch (SD__value.BitCount)
            {
                case 8:
                    Result = SD__value.View().Byte(X + SD__value.Pitch * Y);
                    break;
                case 16:
                    Result = SD__value.View().Word(X + X + SD__value.Pitch * Y);
                    break;
                case 24:
                    // Four bytes at the last pixel of a row too, as GFXEUnit.GetPixelInfo.
                    Result = (int)SD__value.View().Dword(X * 3 + SD__value.Pitch * Y) & 16777215;
                    break;
                case 32:
                    Result = (int)SD__value.View().Dword(X * 4 + SD__value.Pitch * Y);
                    break;
                default:
                    throw new NotSupportedException("PixelFormat is not supported");
            }

            return Result;
        }

        Rect = R;
        Found = false;
        while (!Found && Rect.Top < Rect.Bottom)
        {
            X = Rect.Left;
            while (!Found && X != Rect.Right)
            {
                Found = GetPixel(X, Rect.Top) != TransparentColor;
                X += 1;
            }

            if (!Found)
            {
                Rect.Top += 1;
            }
        }

        Found = false;
        while (!Found && Rect.Bottom > Rect.Top)
        {
            X = Rect.Left;
            while (!Found && X != Rect.Right)
            {
                Found = GetPixel(X, Rect.Bottom - 1) != TransparentColor;
                X += 1;
            }

            if (!Found)
            {
                Rect.Bottom -= 1;
            }
        }

        Found = false;
        while (!Found && Rect.Left < Rect.Right)
        {
            Y = Rect.Top;
            while (!Found && Y != Rect.Bottom)
            {
                Found = GetPixel(Rect.Left, Y) != TransparentColor;
                Y += 1;
            }

            if (!Found)
            {
                Rect.Left += 1;
            }
        }

        Found = false;
        while (!Found && Rect.Right > Rect.Left)
        {
            Y = Rect.Top;
            while (!Found && Y != Rect.Bottom)
            {
                Found = GetPixel(Rect.Right - 1, Y) != TransparentColor;
                Y += 1;
            }

            if (!Found)
            {
                Rect.Right -= 1;
            }
        }

        Result = Rect;
    }

    internal static int RemapSurfacePart(in LockedSurface Src, int SrcX, int SrcY, int Width, int Height, in LockedSurface Dest, int DestX, int DestY)
    {
        SurfaceView S;
        SurfaceView D;
        int Result = PixelFormats.GFXE_OK;
        if (SrcX + Width > Src.Width)
        {
            Width = Src.Width - SrcX;
        }

        if (SrcY + Height > Src.Height)
        {
            Height = Src.Height - SrcY;
        }

        if (SrcX < 0)
        {
            DestX = DestX - SrcX;
            Width = Width + SrcX;
            SrcX = 0;
        }

        if (SrcY < 0)
        {
            DestY = DestY - SrcY;
            Height = Height + SrcY;
            SrcY = 0;
        }

        if (DestX + Width > Dest.Width)
        {
            Width = Dest.Width - DestX;
        }

        if (DestY + Height > Dest.Height)
        {
            Height = Dest.Height - DestY;
        }

        if (DestX < 0)
        {
            SrcX = SrcX - DestX;
            Width = Width + DestX;
            DestX = 0;
        }

        if (DestY < 0)
        {
            SrcY = SrcY - DestY;
            Height = Height + DestY;
            DestY = 0;
        }

        if (Width > 0 && Height > 0)
        {
            S = Src.View(Src.Pitch * SrcY + (int)(((uint)(Src.BitCount + 7)) >> 3) * SrcX);
            D = Dest.View(Dest.Pitch * DestY + (int)(((uint)(Dest.BitCount + 7)) >> 3) * DestX);
            switch (Src.PixelFormatID)
            {
                case PixelFormats.pfPAL8:
                    switch (Dest.PixelFormatID)
                    {
                        case PixelFormats.pfRGB565:
                            PixelRemap.RemapSurface08to565(S, D, Width, Height, Src.Palette);
                            break;
                        case PixelFormats.pfRGB555:
                            PixelRemap.RemapSurface08to555(S, D, Width, Height, Src.Palette);
                            break;
                        case PixelFormats.pfRGB24:
                            PixelRemap.RemapSurface08to24(S, D, Width, Height, Src.Palette);
                            break;
                        case PixelFormats.pfPAL8:
                            BlitRects.BltRect(S, D, Width, Height);
                            break;
                        default:
                            Result = PixelFormats.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case PixelFormats.pfRGB555:
                case PixelFormats.pfARGB1555:
                    switch (Dest.PixelFormatID)
                    {
                        case PixelFormats.pfRGB24:
                            PixelRemap.RemapSurface555to24(S, D, Width, Height);
                            break;
                        case PixelFormats.pfRGB32:
                            PixelRemap.RemapSurface555to32(S, D, Width, Height);
                            break;
                        case PixelFormats.pfRGB565:
                            PixelRemap.RemapSurface555to565(S, D, Width, Height);
                            break;
                        case PixelFormats.pfRGB555:
                            BlitRects.BltRect(S, D, Width * 2, Height);
                            break;
                        default:
                            Result = PixelFormats.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case PixelFormats.pfRGB565:
                    switch (Dest.PixelFormatID)
                    {
                        case PixelFormats.pfRGB24:
                            PixelRemap.RemapSurface565to24(S, D, Width, Height);
                            break;
                        case PixelFormats.pfRGB32:
                            PixelRemap.RemapSurface565to32(S, D, Width, Height);
                            break;
                        case PixelFormats.pfRGB555:
                            PixelRemap.RemapSurface565to555(S, D, Width, Height);
                            break;
                        case PixelFormats.pfRGB565:
                            BlitRects.BltRect(S, D, Width * 2, Height);
                            break;
                        default:
                            Result = PixelFormats.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case PixelFormats.pfRGB24:
                    switch (Dest.PixelFormatID)
                    {
                        case PixelFormats.pfRGB565:
                            PixelRemap.RemapSurface24to565(S, D, Width, Height);
                            break;
                        case PixelFormats.pfRGB555:
                            PixelRemap.RemapSurface24to555(S, D, Width, Height);
                            break;
                        case PixelFormats.pfRGB24:
                            BlitRects.BltRect(S, D, Width * 3, Height);
                            break;
                        default:
                            Result = PixelFormats.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case PixelFormats.pfRGB32:
                    switch (Dest.PixelFormatID)
                    {
                        case PixelFormats.pfRGB565:
                            PixelRemap.RemapSurface32to565(S, D, Width, Height);
                            break;
                        case PixelFormats.pfRGB555:
                            PixelRemap.RemapSurface32to555(S, D, Width, Height);
                            break;
                        case PixelFormats.pfRGB32:
                            BlitRects.BltRect(S, D, Width * 4, Height);
                            break;
                        default:
                            Result = PixelFormats.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                default:
                    Result = PixelFormats.GFXE_NOTSUPPORTED;
                    break;
            }
        }

        return Result;
    }

}
