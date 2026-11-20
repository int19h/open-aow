#nullable disable
// Translated from GFXE.pas.
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;

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
        if (Lock(ref lockedSurface).IsSuccess())
        {
            try
            {
                Assert(X >= 0 && X < lockedSurface.Width && Y >= 0 && Y < lockedSurface.Height, "Coordinates out of bounds!", "GFXE.pas", 11974);
                // { 8 bit pixels need the palette; 1555 and the TColor ordered 24 bit layout are not handled }
                switch (lockedSurface.PixelFormatId)
                {
                    case PixelFormat.Rgb555:
                        w = lockedSurface.View().UInt16(X + X + (Y * lockedSurface.Pitch));
                        result = (int)(((uint)(w & 31744)) >> 7) + ((w & 992) << 6) + ((w & 31) << 19);
                        break;
                    case PixelFormat.Rgb565:
                        w = lockedSurface.View().UInt16(X + X + (Y * lockedSurface.Pitch));
                        result = (int)(((uint)(w & 63488)) >> 8) + ((w & 2016) << 5) + ((w & 31) << 19);
                        break;
                    case PixelFormat.Pal8:
                        Assert(lockedSurface.Palette is not null, "Palette not found, cannot retrieve color (GetColor)", "GFXE.pas", 11986);
                        index = lockedSurface.View().Byte(X + (Y * lockedSurface.Pitch));
                        Palette.Entry e = lockedSurface.Palette[index];
                        result = e.Red + (e.Green << 8) + (e.Blue << 16);
                        break;
                    case PixelFormat.Rgb24:
                        SurfaceView p = lockedSurface.View(X + X + X + (Y * lockedSurface.Pitch));
                        result = p[2] + (p[1] << 8) + (p[0] << 16);
                        break;
                    case PixelFormat.Rgb32:
                        v = (int)lockedSurface.View().Dword((X << 2) + (Y * lockedSurface.Pitch));
                        result = (int)(((uint)(v & 16711680)) >> 16) + (v & 65280) + ((v & 255) << 16);
                        break;
                    case PixelFormat.None:
                    case PixelFormat.Argb1555:
                    case PixelFormat.Bgr24:
                    case PixelFormat.Xrgb32:
                    default:
                        Assert(false, "Pixel format not supported (GetColor)", "GFXE.pas", 11999);
                        result = 0;
                        break;
                }
            }
            finally
            {
                _ = Unlock();
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
        if (Lock(ref lockedSurface).IsSuccess())
        {
            try
            {
                Assert(X >= 0 && X < lockedSurface.Width && Y >= 0 && Y < lockedSurface.Height, "Coordinates out of bounds!", "GFXE.pas", 11932);
                switch (lockedSurface.BitCount)
                {
                    case 8:
                        result = lockedSurface.View().Byte(X + (Y * lockedSurface.Pitch));
                        break;
                    case 16:
                        result = lockedSurface.View().UInt16(X + X + (Y * lockedSurface.Pitch));
                        break;
                    case 24:
                        if (X + 1 < lockedSurface.Width)
                        {
                            result = (int)lockedSurface.View().Dword(X + X + X + (Y * lockedSurface.Pitch)) & 16777215;
                        }
                        else
                        {
                            p = X + X + X + (Y * lockedSurface.Pitch);
                            result = lockedSurface.View().UInt16(p) + (lockedSurface.View().Byte(p + 2) << 16);
                        }

                        break;
                    case 32:
                        result = (int)lockedSurface.View().Dword((X << 2) + (Y * lockedSurface.Pitch));
                        break;
                    default:
                        Assert(false, "Pixel format not supported (GetColor)", "GFXE.pas", 11946);
                        result = 0;
                        break;
                }
            }
            finally
            {
                _ = Unlock();
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
    public virtual int GetWidth() => throw AbstractErrorException.Create("Abstract Error");

    // GFXE.pas:399
    public virtual int GetHeight() => throw AbstractErrorException.Create("Abstract Error");

    // GFXE.pas:11954
    public virtual PixelFormat GetPixelFormatId()
    {
        PixelFormat result;
        LockedSurface lockedSurface = default;
        if (Lock(ref lockedSurface).IsSuccess())
        {
            result = lockedSurface.PixelFormatId;
            _ = Unlock();
        }
        else
        {
            result = PixelFormat.None;
        }

        return result;
    }

    // GFXE.pas:12007
    public virtual GraphicsResult Blit(Surface destination, Rect? sourceRectangle, int X, int Y)
    {
        GraphicsResult result;
        LockedSurface destinationSurface = default;
        LockedSurface sourceSurface = default;
        Rect r = default;
        SurfaceView s;
        SurfaceView d;
        if (destination.PixelFormatId == PixelFormatId)
        {
            if (destination.Lock(ref destinationSurface).IsSuccess())
            {
                if (Lock(ref sourceSurface).IsSuccess())
                {
                    if (sourceRectangle is null)
                    {
                        r.Left = 0;
                        r.Right = sourceSurface.Width;
                        r.Top = 0;
                        r.Bottom = sourceSurface.Height;
                        sourceRectangle = r;
                    }

                    s = sourceSurface.View((sourceRectangle.Value.Left * sourceSurface.BitCount / 8) + (sourceRectangle.Value.Top * sourceSurface.Pitch));
                    d = destinationSurface.View((X * destinationSurface.BitCount / 8) + (Y * destinationSurface.Pitch));
                    BlitRects.BlitRectangle(s, d, (sourceRectangle.Value.Right - sourceRectangle.Value.Left) * destinationSurface.BitCount / 8, sourceRectangle.Value.Bottom - sourceRectangle.Value.Top);
                    _ = Unlock();
                    result = GraphicsResult.Ok;
                }
                else
                {
                    result = GraphicsResult.CannotLock;
                }

                _ = destination.Unlock();
            }
            else
            {
                result = GraphicsResult.CannotLock;
            }
        }
        else
        {
            result = GraphicsResult.PixelFormatNotSupported;
        }

        return result;
    }

    // GFXE.pas:12045
    public virtual GraphicsResult BlitClipped(Surface destination, Rect? sourceRectangle, int X, int Y)
    {
        GraphicsResult result;
        int w;
        int h;
        Rect r = default;
        if (sourceRectangle is null)
        {
            r.Left = 0;
            int width = GetWidth();
            r.Right = width;
            r.Top = 0;
            int height = GetHeight();
            r.Bottom = height;
        }
        else
        {
            r = sourceRectangle.Value;
            int width = GetWidth();
            if (r.Right > width)
            {
                int width2 = GetWidth();
                r.Right = width2;
            }

            if (r.Left < 0)
            {
                X -= r.Left;
                r.Left = 0;
            }
        }

        if (X < 0)
        {
            r.Left -= X;
            X = 0;
        }

        w = r.Right - r.Left;
        if (destination.GetWidth() < X + w)
        {
            w -= X + w - destination.GetWidth();
        }

        int height2 = GetHeight();
        if (r.Bottom > height2)
        {
            int height = GetHeight();
            r.Bottom = height;
        }

        if (r.Top < 0)
        {
            Y -= r.Top;
            r.Top = 0;
        }

        if (Y < 0)
        {
            r.Top -= Y;
            Y = 0;
        }

        h = r.Bottom - r.Top;
        if (destination.GetHeight() < h + Y)
        {
            h -= h + Y - destination.GetHeight();
        }

        if (w > 0 && h > 0)
        {
            r.Right = w + r.Left;
            r.Bottom = r.Top + h;
            result = Blit(destination, r, X, Y);
        }
        else
        {
            result = GraphicsResult.Ok;
        }

        return result;
    }

    // GFXE.pas:12130
    public virtual GraphicsResult BlitColorKey(Surface destination, Rect? sourceRectangle, int X, int Y, int colorKey)
    {
        GraphicsResult result;
        LockedSurface destinationSurface = default;
        LockedSurface sourceSurface = default;
        Rect r = default;
        SurfaceView s;
        SurfaceView d;
        if (destination.PixelFormatId == PixelFormatId && PixelFormatId.BitCount() == 16)
        {
            if (destination.Lock(ref destinationSurface).IsSuccess())
            {
                if (Lock(ref sourceSurface).IsSuccess())
                {
                    if (sourceRectangle is null)
                    {
                        r.Left = 0;
                        r.Right = sourceSurface.Width;
                        r.Top = 0;
                        r.Bottom = sourceSurface.Height;
                        sourceRectangle = r;
                    }

                    s = sourceSurface.View((sourceRectangle.Value.Left * sourceSurface.BitCount / 8) + (sourceRectangle.Value.Top * sourceSurface.Pitch));
                    d = destinationSurface.View((X * destinationSurface.BitCount / 8) + (Y * destinationSurface.Pitch));
                    BlitRects.BlitColorKey16(s, d, sourceRectangle.Value.Right - sourceRectangle.Value.Left, sourceRectangle.Value.Bottom - sourceRectangle.Value.Top, colorKey);
                    result = GraphicsResult.Ok;
                    _ = Unlock();
                }
                else
                {
                    result = GraphicsResult.CannotLock;
                }

                _ = destination.Unlock();
            }
            else
            {
                result = GraphicsResult.CannotLock;
            }
        }
        else
        {
            result = GraphicsResult.PixelFormatNotSupported;
        }

        return result;
    }

    // GFXE.pas:12168
    public virtual GraphicsResult BlitColorKeyClipped(Surface destination, Rect? sourceRectangle, int X, int Y, int colorKey)
    {
        GraphicsResult result;
        int w;
        int h;
        Rect r = default;
        if (sourceRectangle is null)
        {
            r.Left = 0;
            int width = GetWidth();
            r.Right = width;
            r.Top = 0;
            int height = GetHeight();
            r.Bottom = height;
        }
        else
        {
            r = sourceRectangle.Value;
            int width = GetWidth();
            if (r.Right > width)
            {
                int width2 = GetWidth();
                r.Right = width2;
            }

            if (r.Left < 0)
            {
                X -= r.Left;
                r.Left = 0;
            }
        }

        if (X < 0)
        {
            r.Left -= X;
            X = 0;
        }

        w = r.Right - r.Left;
        if (destination.GetWidth() < X + w)
        {
            w -= X + w - destination.GetWidth();
        }

        int height2 = GetHeight();
        if (r.Bottom > height2)
        {
            int height = GetHeight();
            r.Bottom = height;
        }

        if (r.Top < 0)
        {
            Y -= r.Top;
            r.Top = 0;
        }

        if (Y < 0)
        {
            r.Top -= Y;
            Y = 0;
        }

        h = r.Bottom - r.Top;
        if (destination.GetHeight() < h + Y)
        {
            h -= h + Y - destination.GetHeight();
        }

        if (w > 0 && h > 0)
        {
            r.Right = w + r.Left;
            r.Bottom = r.Top + h;
            result = BlitColorKey(destination, r, X, Y, colorKey);
        }
        else
        {
            result = GraphicsResult.Ok;
        }

        return result;
    }

    // GFXE.pas:12102
    public virtual GraphicsResult BlitStretch(Surface destination, Rect? sourceRectangle, Rect? destinationRectangle)
    {
        GraphicsResult result;
        Rect r = default;
        Rect dr = default;
        if (sourceRectangle is null)
        {
            r.Left = 0;
            int width = GetWidth();
            r.Right = width;
            r.Top = 0;
            int height = GetHeight();
            r.Bottom = height;
            sourceRectangle = r;
        }

        if (destinationRectangle is null)
        {
            dr.Left = 0;
            int width = destination.GetWidth();
            dr.Right = width;
            dr.Top = 0;
            int height = destination.GetHeight();
            dr.Bottom = height;
            destinationRectangle = dr;
        }

        result = sourceRectangle.Value.Right - sourceRectangle.Value.Left == destinationRectangle.Value.Right - destinationRectangle.Value.Left && sourceRectangle.Value.Bottom - sourceRectangle.Value.Top == destinationRectangle.Value.Bottom - destinationRectangle.Value.Top
            ? Blit(destination, sourceRectangle, destinationRectangle.Value.Left, destinationRectangle.Value.Top)
            : GraphicsResult.NotSupported;

        return result;
    }

    // GFXE.pas:12225
    public virtual GraphicsResult Lock(ref LockedSurface lockedSurface) => GraphicsResult.NotSupported;

    // GFXE.pas:12248
    public virtual GraphicsResult Unlock() => GraphicsResult.NotSupported;

    // GFXE.pas:12230
    public virtual GraphicsResult Lock(ref LockedSurface lockedSurface, in Rect R)
    {
        GraphicsResult result;
        bool condition = R.Left >= 0 && R.Top >= 0;
        if (condition)
        {
            int height = GetHeight();
            condition = R.Bottom <= height;
        }

        bool test = condition;
        if (test)
        {
            int width = GetWidth();
            test = R.Right <= width;
        }

        if (test)
        {
            result = Lock(ref lockedSurface);
            if (result.IsSuccess())
            {
                lockedSurface.DataIndex = lockedSurface.DataIndex + (int)(((uint)(R.Left * lockedSurface.BitCount)) >> 3) + (R.Top * lockedSurface.Pitch);
                lockedSurface.Width = R.Right - R.Left;
                lockedSurface.Height = R.Bottom - R.Top;
                Rect clipRectangle = lockedSurface.ClipRectangle;
                clipRectangle.Right = lockedSurface.Width;
                lockedSurface.ClipRectangle = clipRectangle;
                Rect clipRectangle2 = lockedSurface.ClipRectangle;
                clipRectangle2.Bottom = lockedSurface.Height;
                lockedSurface.ClipRectangle = clipRectangle2;
            }
        }
        else
        {
            result = GraphicsResult.Error;
        }

        return result;
    }

    // GFXE.pas:12253
    public virtual GraphicsResult Unlock(in LockedSurface lockedSurface) => Unlock();

    // GFXE.pas:12220
    public virtual GraphicsResult UpdatePalette() => GraphicsResult.NotSupported;

    // GFXE.pas:12258
    public virtual GraphicsResult Resize(int width, int height) => GraphicsResult.NotSupported;

    public int Width => GetWidth();
    public int Height => GetHeight();
    public PixelFormat PixelFormatId => GetPixelFormatId();

    public virtual void Dispose()
    {
    }
}
