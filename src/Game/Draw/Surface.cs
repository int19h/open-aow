// Translated from GFXE.pas.
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// the abstract surface. GetWidth and GetHeight are abstract
public partial class Surface : IDisposable
{
    // GFXE.pas:11964
    public virtual int GetColor(int X, int Y)
    {
        int Result = default;
        LockedSurface SD = default;
        int V;
        ushort W;
        byte Index;
        if (Succeeded(Lock(ref SD)))
        {
            try
            {
                Assert(X >= 0 && X < SD.Width && Y >= 0 && Y < SD.Height, "Coordinates out of bounds!", "GFXE.pas", 11974);
                // { 8 bit pixels need the palette; 1555 and the TColor ordered 24 bit layout are not handled }
                switch (SD.PixelFormatID)
                {
                    case PixelFormats.pfRGB555:
                        W = SD.View().Word(X + X + Y * SD.Pitch);
                        Result = (int)(((uint)(W & 31744)) >> 7) + ((W & 992) << 6) + ((W & 31) << 19);
                        break;
                    case PixelFormats.pfRGB565:
                        W = SD.View().Word(X + X + Y * SD.Pitch);
                        Result = (int)(((uint)(W & 63488)) >> 8) + ((W & 2016) << 5) + ((W & 31) << 19);
                        break;
                    case PixelFormats.pfPAL8:
                        Assert(SD.Palette != null, "Palette not found, cannot retrieve color (GetColor)", "GFXE.pas", 11986);
                        Index = SD.View().Byte(X + Y * SD.Pitch);
                        ref readonly Palette.Entry E = ref SD.Palette.Entries[Index];
                        Result = E.R + (E.G << 8) + (E.B << 16);
                        break;
                    case PixelFormats.pfRGB24:
                        var P = SD.View(X + X + X + Y * SD.Pitch);
                        Result = P[2] + (P[1] << 8) + (P[0] << 16);
                        break;
                    case PixelFormats.pfRGB32:
                        V = (int)SD.View().Dword((X << 2) + Y * SD.Pitch);
                        Result = (int)(((uint)(V & 16711680)) >> 16) + (V & 65280) + ((V & 255) << 16);
                        break;
                    default:
                        Assert(false, "Pixel format not supported (GetColor)", "GFXE.pas", 11999);
                        Result = 0;
                        break;
                }
            }
            finally
            {
                Unlock();
            }
        }
        else
        {
            Assert(false, "Cannot Lock Surface", "GFXE.pas", 12004);
            Result = -1;
        }

        return Result;
    }

    // GFXE.pas:11924
    // { The original unit was longer: the line numbers compiled into the Assert calls (11932 for the next Assert below) prove that 737 more lines preceded this point. Their content is unknown; the blank lines below keep the Assert line numbers identical. }
    public virtual int GetPixelInfo(int X, int Y)
    {
        int Result = default;
        LockedSurface SD = default;
        int P;
        if (Succeeded(Lock(ref SD)))
        {
            try
            {
                Assert(X >= 0 && X < SD.Width && Y >= 0 && Y < SD.Height, "Coordinates out of bounds!", "GFXE.pas", 11932);
                switch (SD.BitCount)
                {
                    case 8:
                        Result = SD.View().Byte(X + Y * SD.Pitch);
                        break;
                    case 16:
                        Result = SD.View().Word(X + X + Y * SD.Pitch);
                        break;
                    case 24:
                        if (X + 1 < SD.Width)
                        {
                            Result = (int)SD.View().Dword(X + X + X + Y * SD.Pitch) & 16777215;
                        }
                        else
                        {
                            P = X + X + X + Y * SD.Pitch;
                            Result = SD.View().Word(P) + (SD.View().Byte(P + 2) << 16);
                        }

                        break;
                    case 32:
                        Result = (int)SD.View().Dword((X << 2) + Y * SD.Pitch);
                        break;
                    default:
                        Assert(false, "Pixel format not supported (GetColor)", "GFXE.pas", 11946);
                        Result = 0;
                        break;
                }
            }
            finally
            {
                Unlock();
            }
        }
        else
        {
            Assert(false, "Cannot Lock Surface", "GFXE.pas", 11951);
            Result = -1;
        }

        return Result;
    }

    // GFXE.pas:398
    public virtual int GetWidth()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GFXE.pas:399
    public virtual int GetHeight()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GFXE.pas:11954
    public virtual int GetPixelFormatID()
    {
        int Result;
        LockedSurface SD = default;
        if (Succeeded(Lock(ref SD)))
        {
            Result = SD.PixelFormatID;
            Unlock();
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // GFXE.pas:12007
    public virtual int Blt(Surface Dest, Rect? SrcRect, int X, int Y)
    {
        int Result;
        LockedSurface DSD = default;
        LockedSurface SSD = default;
        Rect R = default;
        SurfaceView S;
        SurfaceView D;
        if (Dest.PixelFormatID == PixelFormatID)
        {
            if (Succeeded(Dest.Lock(ref DSD)))
            {
                if (Succeeded(Lock(ref SSD)))
                {
                    if (SrcRect is null)
                    {
                        R.Left = 0;
                        R.Right = SSD.Width;
                        R.Top = 0;
                        R.Bottom = SSD.Height;
                        SrcRect = R;
                    }

                    S = SSD.View(SrcRect.Value.Left * SSD.BitCount / 8 + SrcRect.Value.Top * SSD.Pitch);
                    D = DSD.View(X * DSD.BitCount / 8 + Y * DSD.Pitch);
                    BlitRects.BltRect(S, D, (SrcRect.Value.Right - SrcRect.Value.Left) * DSD.BitCount / 8, SrcRect.Value.Bottom - SrcRect.Value.Top);
                    Unlock();
                    Result = PixelFormats.GFXE_OK;
                }
                else
                {
                    Result = GFXE_CANNOTLOCK;
                }

                Dest.Unlock();
            }
            else
            {
                Result = GFXE_CANNOTLOCK;
            }
        }
        else
        {
            Result = PixelFormats.GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:12045
    public virtual int BltClipped(Surface Dest, Rect? SrcRect, int X, int Y)
    {
        int Result;
        int W;
        int H;
        Rect R = default;
        if (SrcRect is null)
        {
            R.Left = 0;
            var width = GetWidth();
            R.Right = width;
            R.Top = 0;
            var height = GetHeight();
            R.Bottom = height;
        }
        else
        {
            R = SrcRect.Value;
            var width = GetWidth();
            if (R.Right > width)
            {
                var width2 = GetWidth();
                R.Right = width2;
            }

            if (R.Left < 0)
            {
                X = X - R.Left;
                R.Left = 0;
            }
        }

        if (X < 0)
        {
            R.Left = R.Left - X;
            X = 0;
        }

        W = R.Right - R.Left;
        if (Dest.GetWidth() < X + W)
        {
            W = W - (X + W - Dest.GetWidth());
        }

        var height2 = GetHeight();
        if (R.Bottom > height2)
        {
            var height = GetHeight();
            R.Bottom = height;
        }

        if (R.Top < 0)
        {
            Y = Y - R.Top;
            R.Top = 0;
        }

        if (Y < 0)
        {
            R.Top = R.Top - Y;
            Y = 0;
        }

        H = R.Bottom - R.Top;
        if (Dest.GetHeight() < H + Y)
        {
            H = H - (H + Y - Dest.GetHeight());
        }

        if (W > 0 && H > 0)
        {
            R.Right = W + R.Left;
            R.Bottom = R.Top + H;
            Result = Blt(Dest, R, X, Y);
        }
        else
        {
            Result = PixelFormats.GFXE_OK;
        }

        return Result;
    }

    // GFXE.pas:12130
    public virtual int BltColorKey(Surface Dest, Rect? SrcRect, int X, int Y, int ColorKey)
    {
        int Result;
        LockedSurface DSD = default;
        LockedSurface SSD = default;
        Rect R = default;
        SurfaceView S;
        SurfaceView D;
        if (Dest.PixelFormatID == PixelFormatID && (PixelFormatID & 255) == 16)
        {
            if (Succeeded(Dest.Lock(ref DSD)))
            {
                if (Succeeded(Lock(ref SSD)))
                {
                    if (SrcRect is null)
                    {
                        R.Left = 0;
                        R.Right = SSD.Width;
                        R.Top = 0;
                        R.Bottom = SSD.Height;
                        SrcRect = R;
                    }

                    S = SSD.View(SrcRect.Value.Left * SSD.BitCount / 8 + SrcRect.Value.Top * SSD.Pitch);
                    D = DSD.View(X * DSD.BitCount / 8 + Y * DSD.Pitch);
                    BlitRects.BltColorKey16(S, D, SrcRect.Value.Right - SrcRect.Value.Left, SrcRect.Value.Bottom - SrcRect.Value.Top, ColorKey);
                    Result = PixelFormats.GFXE_OK;
                    Unlock();
                }
                else
                {
                    Result = GFXE_CANNOTLOCK;
                }

                Dest.Unlock();
            }
            else
            {
                Result = GFXE_CANNOTLOCK;
            }
        }
        else
        {
            Result = PixelFormats.GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:12168
    public virtual int BltColorKeyClipped(Surface Dest, Rect? SrcRect, int X, int Y, int ColorKey)
    {
        int Result;
        int W;
        int H;
        Rect R = default;
        if (SrcRect is null)
        {
            R.Left = 0;
            var width = GetWidth();
            R.Right = width;
            R.Top = 0;
            var height = GetHeight();
            R.Bottom = height;
        }
        else
        {
            R = SrcRect.Value;
            var width = GetWidth();
            if (R.Right > width)
            {
                var width2 = GetWidth();
                R.Right = width2;
            }

            if (R.Left < 0)
            {
                X = X - R.Left;
                R.Left = 0;
            }
        }

        if (X < 0)
        {
            R.Left = R.Left - X;
            X = 0;
        }

        W = R.Right - R.Left;
        if (Dest.GetWidth() < X + W)
        {
            W = W - (X + W - Dest.GetWidth());
        }

        var height2 = GetHeight();
        if (R.Bottom > height2)
        {
            var height = GetHeight();
            R.Bottom = height;
        }

        if (R.Top < 0)
        {
            Y = Y - R.Top;
            R.Top = 0;
        }

        if (Y < 0)
        {
            R.Top = R.Top - Y;
            Y = 0;
        }

        H = R.Bottom - R.Top;
        if (Dest.GetHeight() < H + Y)
        {
            H = H - (H + Y - Dest.GetHeight());
        }

        if (W > 0 && H > 0)
        {
            R.Right = W + R.Left;
            R.Bottom = R.Top + H;
            Result = BltColorKey(Dest, R, X, Y, ColorKey);
        }
        else
        {
            Result = PixelFormats.GFXE_OK;
        }

        return Result;
    }

    // GFXE.pas:12102
    public virtual int BltStretch(Surface Dest, Rect? SrcRect, Rect? DestRect)
    {
        int Result;
        Rect R = default;
        Rect DR = default;
        if (SrcRect is null)
        {
            R.Left = 0;
            var width = GetWidth();
            R.Right = width;
            R.Top = 0;
            var height = GetHeight();
            R.Bottom = height;
            SrcRect = R;
        }

        if (DestRect is null)
        {
            DR.Left = 0;
            var width = Dest.GetWidth();
            DR.Right = width;
            DR.Top = 0;
            var height = Dest.GetHeight();
            DR.Bottom = height;
            DestRect = DR;
        }

        if (SrcRect.Value.Right - SrcRect.Value.Left == DestRect.Value.Right - DestRect.Value.Left && SrcRect.Value.Bottom - SrcRect.Value.Top == DestRect.Value.Bottom - DestRect.Value.Top)
        {
            Result = Blt(Dest, SrcRect, DestRect.Value.Left, DestRect.Value.Top);
        }
        else
        {
            Result = PixelFormats.GFXE_NOTSUPPORTED;
        }

        return Result;
    }

    // GFXE.pas:12225
    public virtual int Lock(ref LockedSurface SD) => PixelFormats.GFXE_NOTSUPPORTED;

    // GFXE.pas:12248
    public virtual int Unlock() => PixelFormats.GFXE_NOTSUPPORTED;

    // GFXE.pas:12230
    public virtual int LockEx(ref LockedSurface SD, in Rect R)
    {
        int Result;
        bool condition = R.Left >= 0 && R.Top >= 0;
        if (condition)
        {
            var height = GetHeight();
            condition = R.Bottom <= height;
        }

        bool test = condition;
        if (test)
        {
            var width = GetWidth();
            test = R.Right <= width;
        }

        if (test)
        {
            Result = Lock(ref SD);
            if (Succeeded(Result))
            {
                SD.DataIndex = SD.DataIndex + (int)(((uint)(R.Left * SD.BitCount)) >> 3) + R.Top * SD.Pitch;
                SD.Width = R.Right - R.Left;
                SD.Height = R.Bottom - R.Top;
                SD.ClipRect.Right = SD.Width;
                SD.ClipRect.Bottom = SD.Height;
            }
        }
        else
        {
            Result = PixelFormats.GFXE_ERROR;
        }

        return Result;
    }

    // GFXE.pas:12253
    public virtual int UnlockEx(in LockedSurface SD) => Unlock();

    // GFXE.pas:12220
    public virtual int UpdatePalette() => PixelFormats.GFXE_NOTSUPPORTED;

    // GFXE.pas:12258
    public virtual int Resize(int AWidth, int AHeight) => PixelFormats.GFXE_NOTSUPPORTED;

    public int Width { get => GetWidth(); }
    public int Height { get => GetHeight(); }
    public int PixelFormatID { get => GetPixelFormatID(); }

    public virtual void Dispose()
    {
    }

    private const int GFXE_CANNOTLOCK = -2147483643;
}
