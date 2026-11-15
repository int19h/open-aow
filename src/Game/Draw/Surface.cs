#nullable disable
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
        int result = default;
        LockedSurface lockedSurface = default;
        int v;
        ushort w;
        byte index;
        if (Succeeded(Lock(ref lockedSurface)))
        {
            try
            {
                Assert(X >= 0 && X < lockedSurface.Width && Y >= 0 && Y < lockedSurface.Height, "Coordinates out of bounds!", "GFXE.pas", 11974);
                // { 8 bit pixels need the palette; 1555 and the TColor ordered 24 bit layout are not handled }
                switch (lockedSurface.PixelFormatId)
                {
                    case PixelFormats.pfRGB555:
                        w = lockedSurface.View().UInt16(X + X + Y * lockedSurface.Pitch);
                        result = (int)(((uint)(w & 31744)) >> 7) + ((w & 992) << 6) + ((w & 31) << 19);
                        break;
                    case PixelFormats.pfRGB565:
                        w = lockedSurface.View().UInt16(X + X + Y * lockedSurface.Pitch);
                        result = (int)(((uint)(w & 63488)) >> 8) + ((w & 2016) << 5) + ((w & 31) << 19);
                        break;
                    case PixelFormats.pfPAL8:
                        Assert(lockedSurface.Palette != null, "Palette not found, cannot retrieve color (GetColor)", "GFXE.pas", 11986);
                        index = lockedSurface.View().Byte(X + Y * lockedSurface.Pitch);
                        Palette.Entry e = lockedSurface.Palette[index];
                        result = e.Red + (e.Green << 8) + (e.Blue << 16);
                        break;
                    case PixelFormats.pfRGB24:
                        var p = lockedSurface.View(X + X + X + Y * lockedSurface.Pitch);
                        result = p[2] + (p[1] << 8) + (p[0] << 16);
                        break;
                    case PixelFormats.pfRGB32:
                        v = (int)lockedSurface.View().Dword((X << 2) + Y * lockedSurface.Pitch);
                        result = (int)(((uint)(v & 16711680)) >> 16) + (v & 65280) + ((v & 255) << 16);
                        break;
                    default:
                        Assert(false, "Pixel format not supported (GetColor)", "GFXE.pas", 11999);
                        result = 0;
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
            result = -1;
        }

        return result;
    }

    // GFXE.pas:11924
    // { The original unit was longer: the line numbers compiled into the Assert calls (11932 for the next Assert below) prove that 737 more lines preceded this point. Their content is unknown; the blank lines below keep the Assert line numbers identical. }
    public virtual int GetPixelInfo(int X, int Y)
    {
        int result = default;
        LockedSurface lockedSurface = default;
        int p;
        if (Succeeded(Lock(ref lockedSurface)))
        {
            try
            {
                Assert(X >= 0 && X < lockedSurface.Width && Y >= 0 && Y < lockedSurface.Height, "Coordinates out of bounds!", "GFXE.pas", 11932);
                switch (lockedSurface.BitCount)
                {
                    case 8:
                        result = lockedSurface.View().Byte(X + Y * lockedSurface.Pitch);
                        break;
                    case 16:
                        result = lockedSurface.View().UInt16(X + X + Y * lockedSurface.Pitch);
                        break;
                    case 24:
                        if (X + 1 < lockedSurface.Width)
                        {
                            result = (int)lockedSurface.View().Dword(X + X + X + Y * lockedSurface.Pitch) & 16777215;
                        }
                        else
                        {
                            p = X + X + X + Y * lockedSurface.Pitch;
                            result = lockedSurface.View().UInt16(p) + (lockedSurface.View().Byte(p + 2) << 16);
                        }

                        break;
                    case 32:
                        result = (int)lockedSurface.View().Dword((X << 2) + Y * lockedSurface.Pitch);
                        break;
                    default:
                        Assert(false, "Pixel format not supported (GetColor)", "GFXE.pas", 11946);
                        result = 0;
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
            result = -1;
        }

        return result;
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
    public virtual int GetPixelFormatId()
    {
        int result;
        LockedSurface lockedSurface = default;
        if (Succeeded(Lock(ref lockedSurface)))
        {
            result = lockedSurface.PixelFormatId;
            Unlock();
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // GFXE.pas:12007
    public virtual int Blit(Surface destination, Rect? sourceRectangle, int X, int Y)
    {
        int result;
        LockedSurface destinationSurface = default;
        LockedSurface sourceSurface = default;
        Rect r = default;
        SurfaceView s;
        SurfaceView d;
        if (destination.PixelFormatId == PixelFormatId)
        {
            if (Succeeded(destination.Lock(ref destinationSurface)))
            {
                if (Succeeded(Lock(ref sourceSurface)))
                {
                    if (sourceRectangle is null)
                    {
                        r.Left = 0;
                        r.Right = sourceSurface.Width;
                        r.Top = 0;
                        r.Bottom = sourceSurface.Height;
                        sourceRectangle = r;
                    }

                    s = sourceSurface.View(sourceRectangle.Value.Left * sourceSurface.BitCount / 8 + sourceRectangle.Value.Top * sourceSurface.Pitch);
                    d = destinationSurface.View(X * destinationSurface.BitCount / 8 + Y * destinationSurface.Pitch);
                    BlitRects.BlitRectangle(s, d, (sourceRectangle.Value.Right - sourceRectangle.Value.Left) * destinationSurface.BitCount / 8, sourceRectangle.Value.Bottom - sourceRectangle.Value.Top);
                    Unlock();
                    result = PixelFormats.GFXE_OK;
                }
                else
                {
                    result = GFXE_CANNOTLOCK;
                }

                destination.Unlock();
            }
            else
            {
                result = GFXE_CANNOTLOCK;
            }
        }
        else
        {
            result = PixelFormats.GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return result;
    }

    // GFXE.pas:12045
    public virtual int BlitClipped(Surface destination, Rect? sourceRectangle, int X, int Y)
    {
        int result;
        int w;
        int h;
        Rect r = default;
        if (sourceRectangle is null)
        {
            r.Left = 0;
            var width = GetWidth();
            r.Right = width;
            r.Top = 0;
            var height = GetHeight();
            r.Bottom = height;
        }
        else
        {
            r = sourceRectangle.Value;
            var width = GetWidth();
            if (r.Right > width)
            {
                var width2 = GetWidth();
                r.Right = width2;
            }

            if (r.Left < 0)
            {
                X = X - r.Left;
                r.Left = 0;
            }
        }

        if (X < 0)
        {
            r.Left = r.Left - X;
            X = 0;
        }

        w = r.Right - r.Left;
        if (destination.GetWidth() < X + w)
        {
            w = w - (X + w - destination.GetWidth());
        }

        var height2 = GetHeight();
        if (r.Bottom > height2)
        {
            var height = GetHeight();
            r.Bottom = height;
        }

        if (r.Top < 0)
        {
            Y = Y - r.Top;
            r.Top = 0;
        }

        if (Y < 0)
        {
            r.Top = r.Top - Y;
            Y = 0;
        }

        h = r.Bottom - r.Top;
        if (destination.GetHeight() < h + Y)
        {
            h = h - (h + Y - destination.GetHeight());
        }

        if (w > 0 && h > 0)
        {
            r.Right = w + r.Left;
            r.Bottom = r.Top + h;
            result = Blit(destination, r, X, Y);
        }
        else
        {
            result = PixelFormats.GFXE_OK;
        }

        return result;
    }

    // GFXE.pas:12130
    public virtual int BlitColorKey(Surface destination, Rect? sourceRectangle, int X, int Y, int colorKey)
    {
        int result;
        LockedSurface destinationSurface = default;
        LockedSurface sourceSurface = default;
        Rect r = default;
        SurfaceView s;
        SurfaceView d;
        if (destination.PixelFormatId == PixelFormatId && (PixelFormatId & 255) == 16)
        {
            if (Succeeded(destination.Lock(ref destinationSurface)))
            {
                if (Succeeded(Lock(ref sourceSurface)))
                {
                    if (sourceRectangle is null)
                    {
                        r.Left = 0;
                        r.Right = sourceSurface.Width;
                        r.Top = 0;
                        r.Bottom = sourceSurface.Height;
                        sourceRectangle = r;
                    }

                    s = sourceSurface.View(sourceRectangle.Value.Left * sourceSurface.BitCount / 8 + sourceRectangle.Value.Top * sourceSurface.Pitch);
                    d = destinationSurface.View(X * destinationSurface.BitCount / 8 + Y * destinationSurface.Pitch);
                    BlitRects.BlitColorKey16(s, d, sourceRectangle.Value.Right - sourceRectangle.Value.Left, sourceRectangle.Value.Bottom - sourceRectangle.Value.Top, colorKey);
                    result = PixelFormats.GFXE_OK;
                    Unlock();
                }
                else
                {
                    result = GFXE_CANNOTLOCK;
                }

                destination.Unlock();
            }
            else
            {
                result = GFXE_CANNOTLOCK;
            }
        }
        else
        {
            result = PixelFormats.GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return result;
    }

    // GFXE.pas:12168
    public virtual int BlitColorKeyClipped(Surface destination, Rect? sourceRectangle, int X, int Y, int colorKey)
    {
        int result;
        int w;
        int h;
        Rect r = default;
        if (sourceRectangle is null)
        {
            r.Left = 0;
            var width = GetWidth();
            r.Right = width;
            r.Top = 0;
            var height = GetHeight();
            r.Bottom = height;
        }
        else
        {
            r = sourceRectangle.Value;
            var width = GetWidth();
            if (r.Right > width)
            {
                var width2 = GetWidth();
                r.Right = width2;
            }

            if (r.Left < 0)
            {
                X = X - r.Left;
                r.Left = 0;
            }
        }

        if (X < 0)
        {
            r.Left = r.Left - X;
            X = 0;
        }

        w = r.Right - r.Left;
        if (destination.GetWidth() < X + w)
        {
            w = w - (X + w - destination.GetWidth());
        }

        var height2 = GetHeight();
        if (r.Bottom > height2)
        {
            var height = GetHeight();
            r.Bottom = height;
        }

        if (r.Top < 0)
        {
            Y = Y - r.Top;
            r.Top = 0;
        }

        if (Y < 0)
        {
            r.Top = r.Top - Y;
            Y = 0;
        }

        h = r.Bottom - r.Top;
        if (destination.GetHeight() < h + Y)
        {
            h = h - (h + Y - destination.GetHeight());
        }

        if (w > 0 && h > 0)
        {
            r.Right = w + r.Left;
            r.Bottom = r.Top + h;
            result = BlitColorKey(destination, r, X, Y, colorKey);
        }
        else
        {
            result = PixelFormats.GFXE_OK;
        }

        return result;
    }

    // GFXE.pas:12102
    public virtual int BlitStretch(Surface destination, Rect? sourceRectangle, Rect? destinationRectangle)
    {
        int result;
        Rect r = default;
        Rect dr = default;
        if (sourceRectangle is null)
        {
            r.Left = 0;
            var width = GetWidth();
            r.Right = width;
            r.Top = 0;
            var height = GetHeight();
            r.Bottom = height;
            sourceRectangle = r;
        }

        if (destinationRectangle is null)
        {
            dr.Left = 0;
            var width = destination.GetWidth();
            dr.Right = width;
            dr.Top = 0;
            var height = destination.GetHeight();
            dr.Bottom = height;
            destinationRectangle = dr;
        }

        if (sourceRectangle.Value.Right - sourceRectangle.Value.Left == destinationRectangle.Value.Right - destinationRectangle.Value.Left && sourceRectangle.Value.Bottom - sourceRectangle.Value.Top == destinationRectangle.Value.Bottom - destinationRectangle.Value.Top)
        {
            result = Blit(destination, sourceRectangle, destinationRectangle.Value.Left, destinationRectangle.Value.Top);
        }
        else
        {
            result = PixelFormats.GFXE_NOTSUPPORTED;
        }

        return result;
    }

    // GFXE.pas:12225
    public virtual int Lock(ref LockedSurface lockedSurface) => PixelFormats.GFXE_NOTSUPPORTED;

    // GFXE.pas:12248
    public virtual int Unlock() => PixelFormats.GFXE_NOTSUPPORTED;

    // GFXE.pas:12230
    public virtual int Lock(ref LockedSurface lockedSurface, in Rect R)
    {
        int result;
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
            result = Lock(ref lockedSurface);
            if (Succeeded(result))
            {
                lockedSurface.DataIndex = lockedSurface.DataIndex + (int)(((uint)(R.Left * lockedSurface.BitCount)) >> 3) + R.Top * lockedSurface.Pitch;
                lockedSurface.Width = R.Right - R.Left;
                lockedSurface.Height = R.Bottom - R.Top;
                var clipRectangle = lockedSurface.ClipRectangle;
                clipRectangle.Right = lockedSurface.Width;
                lockedSurface.ClipRectangle = clipRectangle;
                var clipRectangle2 = lockedSurface.ClipRectangle;
                clipRectangle2.Bottom = lockedSurface.Height;
                lockedSurface.ClipRectangle = clipRectangle2;
            }
        }
        else
        {
            result = PixelFormats.GFXE_ERROR;
        }

        return result;
    }

    // GFXE.pas:12253
    public virtual int Unlock(in LockedSurface lockedSurface) => Unlock();

    // GFXE.pas:12220
    public virtual int UpdatePalette() => PixelFormats.GFXE_NOTSUPPORTED;

    // GFXE.pas:12258
    public virtual int Resize(int width, int height) => PixelFormats.GFXE_NOTSUPPORTED;

    public int Width { get => GetWidth(); }
    public int Height { get => GetHeight(); }
    public int PixelFormatId { get => GetPixelFormatId(); }

    public virtual void Dispose()
    {
    }

    private const int GFXE_CANNOTLOCK = -2147483643;
}
