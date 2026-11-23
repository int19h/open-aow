#nullable disable warnings
// Translated from GFXE.pas.
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;

namespace Aow.Game.Draw;

// the abstract surface. GetWidth and GetHeight are abstract
public partial class Surface : IDisposable
{
    // GFXE.pas:11964
    public virtual Color? GetColor(int X, int Y)
    {
        LockedSurface lockedSurface = default;
        if (Lock(ref lockedSurface).IsSuccess())
        {
            try
            {
                Assert(X >= 0 && X < lockedSurface.Width && Y >= 0 && Y < lockedSurface.Height, message: "Coordinates out of bounds!", file: "GFXE.pas", line: 11974);
                // { 8 bit pixels need the palette; 1555 and the TColor ordered 24 bit layout are not handled }
                ushort w;
                switch (lockedSurface.PixelFormatId)
                {
                    case PixelFormat.Rgb555:
                        w = lockedSurface.View().UInt16(X + X + (Y * lockedSurface.Pitch));
                        return Color.FromPacked(((w & 0b0111_1100_0000_0000) >>> 7) + ((w & 0b0011_1110_0000) << 6) + ((w & 0b0001_1111) << 19));
                    case PixelFormat.Rgb565:
                        w = lockedSurface.View().UInt16(X + X + (Y * lockedSurface.Pitch));
                        return Color.FromPacked(((w & 0b1111_1000_0000_0000) >>> 8) + ((w & 0b0111_1110_0000) << 5) + ((w & 0b0001_1111) << 19));
                    case PixelFormat.Pal8:
                        Assert(lockedSurface.Palette is not null, message: "Palette not found, cannot retrieve color (GetColor)", file: "GFXE.pas", line: 11986);
                        byte index = lockedSurface.View().Byte(X + (Y * lockedSurface.Pitch));
                        Palette.Entry e = lockedSurface.Palette[index];
                        return Color.FromPacked(e.Red + (e.Green << 8) + (e.Blue << 16));
                    case PixelFormat.Rgb24:
                        SurfaceView p = lockedSurface.View(X + X + X + (Y * lockedSurface.Pitch));
                        return Color.FromPacked(p[2] + (p[1] << 8) + (p[0] << 16));
                    case PixelFormat.Rgb32:
                        int v = (int)lockedSurface.View().Dword((X << 2) + (Y * lockedSurface.Pitch));
                        return Color.FromPacked(((v & 0xFF_00_00) >>> 16) + (v & 0xFF00) + ((v & 0xFF) << 16));
                    case PixelFormat.None:
                    case PixelFormat.Argb1555:
                    case PixelFormat.Bgr24:
                    case PixelFormat.Xrgb32:
                    default:
                        Assert(false, message: "Pixel format not supported (GetColor)", file: "GFXE.pas", line: 11999);
                        return null;
                }
            }
            finally
            {
                _ = Unlock();
            }
        }
        else
        {
            Assert(false, message: "Cannot Lock Surface", file: "GFXE.pas", line: 12004);
            return null;
        }
    }

    // GFXE.pas:11924
    // { The original unit was longer: the line numbers compiled into the Assert calls (11932 for the next Assert below) prove that 737 more lines preceded this point. Their content is unknown; the blank lines below keep the Assert line numbers identical. }
    public virtual int GetPixelInfo(int X, int Y)
    {
        LockedSurface lockedSurface = default;
        if (Lock(ref lockedSurface).IsSuccess())
        {
            try
            {
                Assert(X >= 0 && X < lockedSurface.Width && Y >= 0 && Y < lockedSurface.Height, message: "Coordinates out of bounds!", file: "GFXE.pas", line: 11932);
                switch (lockedSurface.BitCount)
                {
                    case 8:
                        return lockedSurface.View().Byte(X + (Y * lockedSurface.Pitch));
                    case 16:
                        return lockedSurface.View().UInt16(X + X + (Y * lockedSurface.Pitch));
                    case 24:
                        if (X + 1 < lockedSurface.Width)
                        {
                            return (int)lockedSurface.View().Dword(X + X + X + (Y * lockedSurface.Pitch)) & 0xFF_FF_FF;
                        }
                        else
                        {
                            int p = X + X + X + (Y * lockedSurface.Pitch);
                            return lockedSurface.View().UInt16(p) + (lockedSurface.View().Byte(p + 2) << 16);
                        }
                    case 32:
                        return (int)lockedSurface.View().Dword((X << 2) + (Y * lockedSurface.Pitch));
                    default:
                        Assert(false, message: "Pixel format not supported (GetColor)", file: "GFXE.pas", line: 11946);
                        return 0;
                }
            }
            finally
            {
                _ = Unlock();
            }
        }
        else
        {
            Assert(false, message: "Cannot Lock Surface", file: "GFXE.pas", line: 11951);
            return -1;
        }
    }

    // GFXE.pas:398
    public virtual int Width => throw AbstractErrorException.Create("Abstract Error");

    // GFXE.pas:399
    public virtual int Height => throw AbstractErrorException.Create("Abstract Error");

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
            return PixelFormat.None;
        }

        return result;
    }

    // GFXE.pas:12007
    public virtual GraphicsResult Blit(Surface? destination, Rect? sourceRectangle, int X, int Y)
    {
        GraphicsResult result;
        Rect r = default;
        if (destination.PixelFormatId == PixelFormatId)
        {
            LockedSurface destinationSurface = default;
            if (destination.Lock(ref destinationSurface).IsSuccess())
            {
                LockedSurface sourceSurface = default;
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

                    SurfaceView s = sourceSurface.View((sourceRectangle.Value.Left * sourceSurface.BitCount / 8) + (sourceRectangle.Value.Top * sourceSurface.Pitch));
                    SurfaceView d = destinationSurface.View((X * destinationSurface.BitCount / 8) + (Y * destinationSurface.Pitch));
                    Graphics.Blit.BlitRectangle(s, d: d, w: (sourceRectangle.Value.Right - sourceRectangle.Value.Left) * destinationSurface.BitCount / 8, h: sourceRectangle.Value.Bottom - sourceRectangle.Value.Top);
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
                return GraphicsResult.CannotLock;
            }
        }
        else
        {
            return GraphicsResult.PixelFormatNotSupported;
        }

        return result;
    }

    // GFXE.pas:12045
    public virtual GraphicsResult BlitClipped(Surface destination, Rect? sourceRectangle, int X, int Y)
    {
        Rect r = default;
        if (sourceRectangle is null)
        {
            r.Left = 0;
            int width = Width;
            r.Right = width;
            r.Top = 0;
            int height = Height;
            r.Bottom = height;
        }
        else
        {
            r = sourceRectangle.Value;
            int width = Width;
            if (r.Right > width)
            {
                int width2 = Width;
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

        int w = r.Right - r.Left;
        if (destination.Width < X + w)
        {
            w -= X + w - destination.Width;
        }

        int height2 = Height;
        if (r.Bottom > height2)
        {
            int height = Height;
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

        int h = r.Bottom - r.Top;
        if (destination.Height < h + Y)
        {
            h -= h + Y - destination.Height;
        }
        if (w > 0 && h > 0)
        {
            r.Right = w + r.Left;
            r.Bottom = r.Top + h;
            return Blit(destination, sourceRectangle: r, X, Y);
        }
        else
        {
            return GraphicsResult.Ok;
        }
    }

    // GFXE.pas:12130
    public virtual GraphicsResult BlitColorKey(Surface destination, Rect? sourceRectangle, int X, int Y, int colorKey)
    {
        GraphicsResult result;
        Rect r = default;
        if (destination.PixelFormatId == PixelFormatId && PixelFormatId.BitCount() == 16)
        {
            LockedSurface destinationSurface = default;
            if (destination.Lock(ref destinationSurface).IsSuccess())
            {
                LockedSurface sourceSurface = default;
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

                    SurfaceView s = sourceSurface.View((sourceRectangle.Value.Left * sourceSurface.BitCount / 8) + (sourceRectangle.Value.Top * sourceSurface.Pitch));
                    SurfaceView d = destinationSurface.View((X * destinationSurface.BitCount / 8) + (Y * destinationSurface.Pitch));
                    Graphics.Blit.BlitColorKey16(s, d: d, w: sourceRectangle.Value.Right - sourceRectangle.Value.Left, h: sourceRectangle.Value.Bottom - sourceRectangle.Value.Top, key: colorKey);
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
                return GraphicsResult.CannotLock;
            }
        }
        else
        {
            return GraphicsResult.PixelFormatNotSupported;
        }

        return result;
    }

    // GFXE.pas:12168
    public virtual GraphicsResult BlitColorKeyClipped(Surface destination, Rect? sourceRectangle, int X, int Y, int colorKey)
    {
        Rect r = default;
        if (sourceRectangle is null)
        {
            r.Left = 0;
            int width = Width;
            r.Right = width;
            r.Top = 0;
            int height = Height;
            r.Bottom = height;
        }
        else
        {
            r = sourceRectangle.Value;
            int width = Width;
            if (r.Right > width)
            {
                int width2 = Width;
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

        int w = r.Right - r.Left;
        if (destination.Width < X + w)
        {
            w -= X + w - destination.Width;
        }

        int height2 = Height;
        if (r.Bottom > height2)
        {
            int height = Height;
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

        int h = r.Bottom - r.Top;
        if (destination.Height < h + Y)
        {
            h -= h + Y - destination.Height;
        }
        if (w > 0 && h > 0)
        {
            r.Right = w + r.Left;
            r.Bottom = r.Top + h;
            return BlitColorKey(destination, sourceRectangle: r, X, Y, colorKey);
        }
        else
        {
            return GraphicsResult.Ok;
        }
    }

    // GFXE.pas:12102
    public virtual GraphicsResult BlitStretch(Surface? destination, Rect? sourceRectangle, Rect? destinationRectangle)
    {
        Rect r = default;
        Rect dr = default;
        if (sourceRectangle is null)
        {
            r.Left = 0;
            int width = Width;
            r.Right = width;
            r.Top = 0;
            int height = Height;
            r.Bottom = height;
            sourceRectangle = r;
        }

        if (destinationRectangle is null)
        {
            dr.Left = 0;
            int width = destination.Width;
            dr.Right = width;
            dr.Top = 0;
            int height = destination.Height;
            dr.Bottom = height;
            destinationRectangle = dr;
        }

        return sourceRectangle.Value.Right - sourceRectangle.Value.Left == destinationRectangle.Value.Right - destinationRectangle.Value.Left && sourceRectangle.Value.Bottom - sourceRectangle.Value.Top == destinationRectangle.Value.Bottom - destinationRectangle.Value.Top
            ? Blit(destination, sourceRectangle, destinationRectangle.Value.Left, destinationRectangle.Value.Top)
            : GraphicsResult.NotSupported;
    }

    // GFXE.pas:12225
    public virtual GraphicsResult Lock(ref LockedSurface lockedSurface) => GraphicsResult.NotSupported;

    // GFXE.pas:12248
    public virtual GraphicsResult Unlock() => GraphicsResult.NotSupported;

    // GFXE.pas:12230
    public virtual GraphicsResult Lock(ref LockedSurface lockedSurface, in Rect R)
    {
        bool condition = R.Left >= 0 && R.Top >= 0;
        if (condition)
        {
            int height = Height;
            condition = R.Bottom <= height;
        }

        bool test = condition;
        if (test)
        {
            int width = Width;
            test = R.Right <= width;
        }

        GraphicsResult result;
        if (test)
        {
            result = Lock(ref lockedSurface);
            if (result.IsSuccess())
            {
                lockedSurface.DataIndex = lockedSurface.DataIndex + ((R.Left * lockedSurface.BitCount) >>> 3) + (R.Top * lockedSurface.Pitch);
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
            return GraphicsResult.Error;
        }

        return result;
    }

    // GFXE.pas:12253
    public virtual GraphicsResult Unlock(in LockedSurface lockedSurface) => Unlock();

    // GFXE.pas:12220
    public virtual GraphicsResult UpdatePalette() => GraphicsResult.NotSupported;

    // GFXE.pas:12258
    public virtual GraphicsResult Resize(int width, int height) => GraphicsResult.NotSupported;

    public PixelFormat PixelFormatId => GetPixelFormatId();

    public virtual void Dispose()
    {
    }
}
