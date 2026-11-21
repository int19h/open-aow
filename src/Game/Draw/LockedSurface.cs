#nullable disable warnings

using Aow.Graphics;
using Aow.Rtl;

namespace Aow.Game.Draw;

// GFXE.pas: the description of a locked surface (graphics.md 3.1). The original record is 60
// bytes and its assembler readers use the field offsets. No reader in the port sees it as bytes,
// so it is a plain struct. The original's `Data: Pointer` is two fields: Data, the block of memory
// that holds the surface's rows, and DataIndex, the byte index of the first byte of row 0 in it.
// `Pointer(Integer(SD.Data) + n)` is DataIndex + n, and `SD.Data = nil` is a null Data.
public struct LockedSurface
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int BitCount { get; set; }
    public int Pitch { get; set; }

    public Rect ClipRectangle { get; set; }

    public byte[]? Data { get; set; }
    public int DataIndex { get; set; }
    public Palette? Palette { get; init; }
    public PixelFormat PixelFormatId { get; init; }

    public int RMask { get; internal set; }
    public int GMask { get; internal set; }
    public int BMask { get; internal set; }
    public int AMask { get; internal set; }

    // The view of the surface's rows for the blitters, moved by a byte offset from the first
    // pixel of row 0 (`Pointer(Integer(SD.Data) + Offset)` with `SD.Pitch` in the original), with
    // the descriptor's clip rectangle.
    public readonly SurfaceView View(int offset = 0) => new(Data, start: DataIndex + offset, Pitch, clip: ClipRectangle);

    internal static GraphicsResult BlitSprite16(in LockedSurface source, Rect sourceRectangle, in LockedSurface destination, int X, int Y, int colorKey)
    {
        if (source.BitCount == 16 && destination.PixelFormatId == source.PixelFormatId)
        {
            int key = PixelFormats.Color2PixelInfo(colorKey, source.PixelFormatId, palette: null);
            SurfaceView s = source.View((source.Pitch * sourceRectangle.Top) + (sourceRectangle.Left * 2));
            SurfaceView d = destination.View((destination.Pitch * Y) + (X * 2));
            Blit.BlitColorKey16(s, d: d, w: sourceRectangle.Right - sourceRectangle.Left, h: sourceRectangle.Bottom - sourceRectangle.Top, key: key);
            return GraphicsResult.Ok;
        }
        else
        {
            return GraphicsResult.PixelFormatNotSupported;
        }
    }

    internal static GraphicsResult BlitSurfaceDescriptionRectangleClipped(in LockedSurface source, Rect sourceRectangle, in LockedSurface destination, int X, int Y)
    {
        if (sourceRectangle.Right > source.ClipRectangle.Right)
        {
            sourceRectangle.Right = source.ClipRectangle.Right;
        }

        if (sourceRectangle.Left < source.ClipRectangle.Left)
        {
            X += source.ClipRectangle.Left - sourceRectangle.Left;
            sourceRectangle.Left = source.ClipRectangle.Left;
        }

        if (X < destination.ClipRectangle.Left)
        {
            sourceRectangle.Left += destination.ClipRectangle.Left - X;
            X = destination.ClipRectangle.Left;
        }

        int w = sourceRectangle.Right - sourceRectangle.Left;
        if (X + w > destination.ClipRectangle.Right)
        {
            w -= X + w - destination.ClipRectangle.Right;
        }

        if (sourceRectangle.Bottom > source.ClipRectangle.Bottom)
        {
            sourceRectangle.Bottom = source.ClipRectangle.Bottom;
        }

        if (sourceRectangle.Top < source.ClipRectangle.Top)
        {
            Y += source.ClipRectangle.Top - sourceRectangle.Top;
            sourceRectangle.Top = source.ClipRectangle.Top;
        }

        if (destination.ClipRectangle.Top > Y)
        {
            sourceRectangle.Top += destination.ClipRectangle.Top - Y;
            Y = destination.ClipRectangle.Top;
        }

        int h = sourceRectangle.Bottom - sourceRectangle.Top;
        if (Y + h > destination.ClipRectangle.Bottom)
        {
            h -= Y + h - destination.ClipRectangle.Bottom;
        }
        if (source.PixelFormatId == destination.PixelFormatId)
        {
            if (w > 0 && h > 0)
            {
                w = (source.BitCount * w) >>> 3;
                SurfaceView s = source.View((source.Pitch * sourceRectangle.Top) + ((source.BitCount * sourceRectangle.Left) >>> 3));
                SurfaceView d = destination.View((destination.Pitch * Y) + ((destination.BitCount * X) >>> 3));
                Blit.BlitRectangle(s, d: d, w: w, h: h);
            }

            return GraphicsResult.Ok;
        }
        else
        {
            return GraphicsResult.PixelFormatNotSupported;
        }
    }

    internal static GraphicsResult BlitSpriteClipped16(in LockedSurface source, Rect sourceRectangle, in LockedSurface destination, int X, int Y, int colorKey)
    {
        if (source.BitCount == 16 && destination.BitCount == 16)
        {
            if (sourceRectangle.Right > source.ClipRectangle.Right)
            {
                sourceRectangle.Right = source.ClipRectangle.Right;
            }

            if (sourceRectangle.Left < source.ClipRectangle.Left)
            {
                X += source.ClipRectangle.Left - sourceRectangle.Left;
                sourceRectangle.Left = source.ClipRectangle.Left;
            }

            if (X < destination.ClipRectangle.Left)
            {
                sourceRectangle.Left += destination.ClipRectangle.Left - X;
                X = destination.ClipRectangle.Left;
            }

            int w = sourceRectangle.Right - sourceRectangle.Left;
            if (w + X > destination.ClipRectangle.Right)
            {
                w -= w + X - destination.ClipRectangle.Right;
            }

            if (sourceRectangle.Bottom > source.ClipRectangle.Bottom)
            {
                sourceRectangle.Bottom = source.ClipRectangle.Bottom;
            }

            if (sourceRectangle.Top < source.ClipRectangle.Top)
            {
                Y += source.ClipRectangle.Top - sourceRectangle.Top;
                sourceRectangle.Top = source.ClipRectangle.Top;
            }

            if (Y < destination.ClipRectangle.Top)
            {
                sourceRectangle.Top += destination.ClipRectangle.Top - Y;
                Y = destination.ClipRectangle.Top;
            }

            int h = sourceRectangle.Bottom - sourceRectangle.Top;
            if (Y + h > destination.ClipRectangle.Bottom)
            {
                h -= Y + h - destination.ClipRectangle.Bottom;
            }

            if (w > 0 && h > 0)
            {
                int key = PixelFormats.Color2PixelInfo(colorKey, source.PixelFormatId, palette: null);
                SurfaceView s = source.View((source.Pitch * sourceRectangle.Top) + (sourceRectangle.Left * 2));
                SurfaceView d = destination.View(destination.Pitch * Y);
                X += X;
                d = d.At(X);
                Blit.BlitColorKey16(s, d: d, w: w, h: h, key: key);
            }

            return GraphicsResult.Ok;
        }
        else
        {
            return GraphicsResult.PixelFormatNotSupported;
        }
    }

    public static LockedSurface CreateFrom(in LockedSurface source, in Rect R)
    {
        LockedSurface destination = source;
        _ = destination.ClipRectangle;
        _ = source.ClipRectangle.Intersect(right: R, result: out Rect clipRectangle);
        destination.ClipRectangle = clipRectangle;
        destination.DataIndex = source.DataIndex + (R.Top * source.Pitch) + ((source.BitCount >>> 3) * R.Left);
        Rect clipRectangle2 = destination.ClipRectangle;
        clipRectangle2.Left -= R.Left;
        destination.ClipRectangle = clipRectangle2;
        Rect clipRectangle3 = destination.ClipRectangle;
        clipRectangle3.Right -= R.Left;
        destination.ClipRectangle = clipRectangle3;
        Rect clipRectangle4 = destination.ClipRectangle;
        clipRectangle4.Top -= R.Top;
        destination.ClipRectangle = clipRectangle4;
        Rect clipRectangle5 = destination.ClipRectangle;
        clipRectangle5.Bottom -= R.Top;
        destination.ClipRectangle = clipRectangle5;
        return destination;
    }

    internal static void MinimizeTransparentRectangle(in LockedSurface lockedSurface, in Rect R, int transparentColor, out Rect result)
    {
        LockedSurface lockedSurfaceValue = lockedSurface;
        Rect rectangle;
        int GetPixel(int X, int Y)
        {
            return lockedSurfaceValue.BitCount switch
            {
                8 => lockedSurfaceValue.View().Byte(X + (lockedSurfaceValue.Pitch * Y)),
                16 => lockedSurfaceValue.View().UInt16(X + X + (lockedSurfaceValue.Pitch * Y)),
                24 => (int)lockedSurfaceValue.View().Dword((X * 3) + (lockedSurfaceValue.Pitch * Y)) & 0xFF_FF_FF,// Four bytes at the last pixel of a row too, as GFXEUnit.GetPixelInfo.
                32 => (int)lockedSurfaceValue.View().Dword((X * 4) + (lockedSurfaceValue.Pitch * Y)),
                _ => throw new NotSupportedException("PixelFormat is not supported"),
            };
        }

        rectangle = R;
        bool found = false;
        int x;
        while (!found && rectangle.Top < rectangle.Bottom)
        {
            x = rectangle.Left;
            while (!found && x != rectangle.Right)
            {
                found = GetPixel(x, rectangle.Top) != transparentColor;
                x += 1;
            }

            if (!found)
            {
                rectangle.Top += 1;
            }
        }

        found = false;
        while (!found && rectangle.Bottom > rectangle.Top)
        {
            x = rectangle.Left;
            while (!found && x != rectangle.Right)
            {
                found = GetPixel(x, rectangle.Bottom - 1) != transparentColor;
                x += 1;
            }

            if (!found)
            {
                rectangle.Bottom -= 1;
            }
        }

        found = false;
        int y;
        while (!found && rectangle.Left < rectangle.Right)
        {
            y = rectangle.Top;
            while (!found && y != rectangle.Bottom)
            {
                found = GetPixel(rectangle.Left, y) != transparentColor;
                y += 1;
            }

            if (!found)
            {
                rectangle.Left += 1;
            }
        }

        found = false;
        while (!found && rectangle.Right > rectangle.Left)
        {
            y = rectangle.Top;
            while (!found && y != rectangle.Bottom)
            {
                found = GetPixel(rectangle.Right - 1, y) != transparentColor;
                y += 1;
            }

            if (!found)
            {
                rectangle.Right -= 1;
            }
        }

        result = rectangle;
    }

    internal static GraphicsResult RemapSurfacePart(in LockedSurface source, int sourceX, int sourceY, int width, int height, in LockedSurface destination, int destinationX, int destinationY)
    {
        if (sourceX + width > source.Width)
        {
            width = source.Width - sourceX;
        }

        if (sourceY + height > source.Height)
        {
            height = source.Height - sourceY;
        }

        if (sourceX < 0)
        {
            destinationX -= sourceX;
            width += sourceX;
            sourceX = 0;
        }

        if (sourceY < 0)
        {
            destinationY -= sourceY;
            height += sourceY;
            sourceY = 0;
        }

        if (destinationX + width > destination.Width)
        {
            width = destination.Width - destinationX;
        }

        if (destinationY + height > destination.Height)
        {
            height = destination.Height - destinationY;
        }

        if (destinationX < 0)
        {
            sourceX -= destinationX;
            width += destinationX;
            destinationX = 0;
        }

        if (destinationY < 0)
        {
            sourceY -= destinationY;
            height += destinationY;
            destinationY = 0;
        }

        if (width > 0 && height > 0)
        {
            SurfaceView s = source.View((source.Pitch * sourceY) + (((source.BitCount + 7) >>> 3) * sourceX));
            SurfaceView d = destination.View((destination.Pitch * destinationY) + (((destination.BitCount + 7) >>> 3) * destinationX));
            switch (source.PixelFormatId)
            {
                case PixelFormat.Pal8:
                    switch (destination.PixelFormatId)
                    {
                        case PixelFormat.Rgb565:
                            PixelRemap.RemapSurface08To565(s, destination: d, width, height, source.Palette);
                            break;
                        case PixelFormat.Rgb555:
                            PixelRemap.RemapSurface08To555(s, destination: d, width, height, source.Palette);
                            break;
                        case PixelFormat.Rgb24:
                            PixelRemap.RemapSurface08To24(s, destination: d, width, height, source.Palette);
                            break;
                        case PixelFormat.Pal8:
                            Blit.BlitRectangle(s, d: d, w: width, h: height);
                            break;
                        case PixelFormat.None:
                        case PixelFormat.Argb1555:
                        case PixelFormat.Bgr24:
                        case PixelFormat.Rgb32:
                        case PixelFormat.Xrgb32:
                        default:
                            return GraphicsResult.NotSupported;
                    }

                    break;
                case PixelFormat.Rgb555:
                case PixelFormat.Argb1555:
                    switch (destination.PixelFormatId)
                    {
                        case PixelFormat.Rgb24:
                            PixelRemap.RemapSurface555To24(s, destination: d, width, height);
                            break;
                        case PixelFormat.Rgb32:
                            Blit.RemapSurface555To32(s, d: d, w: width, h: height);
                            break;
                        case PixelFormat.Rgb565:
                            Blit.RemapSurface555To565(s, d: d, w: width, h: height);
                            break;
                        case PixelFormat.Rgb555:
                            Blit.BlitRectangle(s, d: d, w: width * 2, h: height);
                            break;
                        case PixelFormat.None:
                        case PixelFormat.Pal8:
                        case PixelFormat.Argb1555:
                        case PixelFormat.Bgr24:
                        case PixelFormat.Xrgb32:
                        default:
                            return GraphicsResult.NotSupported;
                    }

                    break;
                case PixelFormat.Rgb565:
                    switch (destination.PixelFormatId)
                    {
                        case PixelFormat.Rgb24:
                            PixelRemap.RemapSurface565To24(s, destination: d, width, height);
                            break;
                        case PixelFormat.Rgb32:
                            Blit.RemapSurface565To32(s, d: d, w: width, h: height);
                            break;
                        case PixelFormat.Rgb555:
                            Blit.RemapSurface565To555(s, d: d, w: width, h: height);
                            break;
                        case PixelFormat.Rgb565:
                            Blit.BlitRectangle(s, d: d, w: width * 2, h: height);
                            break;
                        case PixelFormat.None:
                        case PixelFormat.Pal8:
                        case PixelFormat.Argb1555:
                        case PixelFormat.Bgr24:
                        case PixelFormat.Xrgb32:
                        default:
                            return GraphicsResult.NotSupported;
                    }

                    break;
                case PixelFormat.Rgb24:
                    switch (destination.PixelFormatId)
                    {
                        case PixelFormat.Rgb565:
                            PixelRemap.RemapSurface24To565(s, destination: d, width, height);
                            break;
                        case PixelFormat.Rgb555:
                            PixelRemap.RemapSurface24To555(s, destination: d, width, height);
                            break;
                        case PixelFormat.Rgb24:
                            Blit.BlitRectangle(s, d: d, w: width * 3, h: height);
                            break;
                        case PixelFormat.None:
                        case PixelFormat.Pal8:
                        case PixelFormat.Argb1555:
                        case PixelFormat.Bgr24:
                        case PixelFormat.Rgb32:
                        case PixelFormat.Xrgb32:
                        default:
                            return GraphicsResult.NotSupported;
                    }

                    break;
                case PixelFormat.Rgb32:
                    switch (destination.PixelFormatId)
                    {
                        case PixelFormat.Rgb565:
                            PixelRemap.RemapSurface32To555(s, destination: d, width, height);
                            break;
                        case PixelFormat.Rgb555:
                            PixelRemap.RemapSurface32To555(s, destination: d, width, height);
                            break;
                        case PixelFormat.Rgb32:
                            Blit.BlitRectangle(s, d: d, w: width * 4, h: height);
                            break;
                        case PixelFormat.None:
                        case PixelFormat.Pal8:
                        case PixelFormat.Argb1555:
                        case PixelFormat.Rgb24:
                        case PixelFormat.Bgr24:
                        case PixelFormat.Xrgb32:
                        default:
                            return GraphicsResult.NotSupported;
                    }

                    break;
                case PixelFormat.None:
                case PixelFormat.Bgr24:
                case PixelFormat.Xrgb32:
                default:
                    return GraphicsResult.NotSupported;
            }
        }

        return GraphicsResult.Ok;
    }
}
