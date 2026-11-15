#nullable disable

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
    public int Width { get; set; }
    public int Height { get; set; }
    public int BitCount { get; set; }
    public int Pitch { get; set; }

    public Rect ClipRectangle { get; set; }

    public byte[] Data { get; set; }
    public int DataIndex { get; set; }
    public Palette Palette { get; init; }
    public int PixelFormatId { get; init; }

    public int RMask { get; internal set; }
    public int GMask { get; internal set; }
    public int BMask { get; internal set; }
    public int AMask { get; internal set; }

    // The view of the surface's rows for the blitters, moved by a byte offset from the first
    // pixel of row 0 (`Pointer(Integer(SD.Data) + Offset)` with `SD.Pitch` in the original), with
    // the descriptor's clip rectangle.
    public readonly SurfaceView View(int offset = 0) => new(Data, DataIndex + offset, Pitch, ClipRectangle);

    internal static int BlitSprite16(in LockedSurface source, Rect sourceRectangle, in LockedSurface destination, int X, int Y, int colorKey)
    {
        int result;
        int key;
        SurfaceView s;
        SurfaceView d;
        if (source.BitCount == 16 && destination.PixelFormatId == source.PixelFormatId)
        {
            key = PixelFormats.Color2PixelInfo(colorKey, source.PixelFormatId, null);
            s = source.View((source.Pitch * sourceRectangle.Top) + (sourceRectangle.Left * 2));
            d = destination.View((destination.Pitch * Y) + (X * 2));
            BlitRects.BlitColorKey16(s, d, sourceRectangle.Right - sourceRectangle.Left, sourceRectangle.Bottom - sourceRectangle.Top, key);
            result = PixelFormats.GFXE_OK;
        }
        else
        {
            result = PixelFormats.GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return result;
    }

    internal static int BlitSurfaceDescriptionRectangleClipped(in LockedSurface source, Rect sourceRectangle, in LockedSurface destination, int X, int Y)
    {
        int result;
        int w;
        int h;
        SurfaceView s;
        SurfaceView d;
        if (sourceRectangle.Right > source.ClipRectangle.Right)
        {
            sourceRectangle.Right = source.ClipRectangle.Right;
        }

        if (sourceRectangle.Left < source.ClipRectangle.Left)
        {
            X = X + (source.ClipRectangle.Left - sourceRectangle.Left);
            sourceRectangle.Left = source.ClipRectangle.Left;
        }

        if (X < destination.ClipRectangle.Left)
        {
            sourceRectangle.Left = sourceRectangle.Left + (destination.ClipRectangle.Left - X);
            X = destination.ClipRectangle.Left;
        }

        w = sourceRectangle.Right - sourceRectangle.Left;
        if (X + w > destination.ClipRectangle.Right)
        {
            w = w - (X + w - destination.ClipRectangle.Right);
        }

        if (sourceRectangle.Bottom > source.ClipRectangle.Bottom)
        {
            sourceRectangle.Bottom = source.ClipRectangle.Bottom;
        }

        if (sourceRectangle.Top < source.ClipRectangle.Top)
        {
            Y = Y + (source.ClipRectangle.Top - sourceRectangle.Top);
            sourceRectangle.Top = source.ClipRectangle.Top;
        }

        if (destination.ClipRectangle.Top > Y)
        {
            sourceRectangle.Top = sourceRectangle.Top + (destination.ClipRectangle.Top - Y);
            Y = destination.ClipRectangle.Top;
        }

        h = sourceRectangle.Bottom - sourceRectangle.Top;
        if (Y + h > destination.ClipRectangle.Bottom)
        {
            h = h - (Y + h - destination.ClipRectangle.Bottom);
        }

        if (source.PixelFormatId == destination.PixelFormatId)
        {
            if (w > 0 && h > 0)
            {
                w = (int)(((uint)(source.BitCount * w)) >> 3);
                s = source.View((source.Pitch * sourceRectangle.Top) + (int)(((uint)(source.BitCount * sourceRectangle.Left)) >> 3));
                d = destination.View((destination.Pitch * Y) + (int)(((uint)(destination.BitCount * X)) >> 3));
                BlitRects.BlitRectangle(s, d, w, h);
            }

            result = PixelFormats.GFXE_OK;
        }
        else
        {
            result = PixelFormats.GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return result;
    }

    internal static int BlitSpriteClipped16(in LockedSurface source, Rect sourceRectangle, in LockedSurface destination, int X, int Y, int colorKey)
    {
        int result;
        int w;
        int h;
        SurfaceView s;
        SurfaceView d;
        int key;
        if (source.BitCount == 16 && destination.BitCount == 16)
        {
            if (sourceRectangle.Right > source.ClipRectangle.Right)
            {
                sourceRectangle.Right = source.ClipRectangle.Right;
            }

            if (sourceRectangle.Left < source.ClipRectangle.Left)
            {
                X = X + (source.ClipRectangle.Left - sourceRectangle.Left);
                sourceRectangle.Left = source.ClipRectangle.Left;
            }

            if (X < destination.ClipRectangle.Left)
            {
                sourceRectangle.Left = sourceRectangle.Left + (destination.ClipRectangle.Left - X);
                X = destination.ClipRectangle.Left;
            }

            w = sourceRectangle.Right - sourceRectangle.Left;
            if (w + X > destination.ClipRectangle.Right)
            {
                w = w - (w + X - destination.ClipRectangle.Right);
            }

            if (sourceRectangle.Bottom > source.ClipRectangle.Bottom)
            {
                sourceRectangle.Bottom = source.ClipRectangle.Bottom;
            }

            if (sourceRectangle.Top < source.ClipRectangle.Top)
            {
                Y = Y + (source.ClipRectangle.Top - sourceRectangle.Top);
                sourceRectangle.Top = source.ClipRectangle.Top;
            }

            if (Y < destination.ClipRectangle.Top)
            {
                sourceRectangle.Top = sourceRectangle.Top + (destination.ClipRectangle.Top - Y);
                Y = destination.ClipRectangle.Top;
            }

            h = sourceRectangle.Bottom - sourceRectangle.Top;
            if (Y + h > destination.ClipRectangle.Bottom)
            {
                h = h - (Y + h - destination.ClipRectangle.Bottom);
            }

            if (w > 0 && h > 0)
            {
                key = PixelFormats.Color2PixelInfo(colorKey, source.PixelFormatId, null);
                s = source.View((source.Pitch * sourceRectangle.Top) + (sourceRectangle.Left * 2));
                d = destination.View(destination.Pitch * Y);
                X += X;
                d = d.At(X);
                BlitRects.BlitColorKey16(s, d, w, h, key);
            }

            result = PixelFormats.GFXE_OK;
        }
        else
        {
            result = PixelFormats.GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return result;
    }

    public static void CreateFrom(in LockedSurface source, in Rect R, out LockedSurface destination)
    {
        destination = source;
        var clipRectangle = destination.ClipRectangle;
        IntersectRect(out clipRectangle, source.ClipRectangle, R);
        destination.ClipRectangle = clipRectangle;
        destination.DataIndex = source.DataIndex + (R.Top * source.Pitch) + ((int)(((uint)source.BitCount) >> 3) * R.Left);
        var clipRectangle2 = destination.ClipRectangle;
        clipRectangle2.Left -= R.Left;
        destination.ClipRectangle = clipRectangle2;
        var clipRectangle3 = destination.ClipRectangle;
        clipRectangle3.Right -= R.Left;
        destination.ClipRectangle = clipRectangle3;
        var clipRectangle4 = destination.ClipRectangle;
        clipRectangle4.Top -= R.Top;
        destination.ClipRectangle = clipRectangle4;
        var clipRectangle5 = destination.ClipRectangle;
        clipRectangle5.Bottom -= R.Top;
        destination.ClipRectangle = clipRectangle5;
    }

    internal static void MinimizeTransparentRectangle(in LockedSurface lockedSurface, in Rect R, int transparentColor, out Rect result)
    {
        LockedSurface lockedSurfaceValue = lockedSurface;
        bool found;
        int x;
        int y;
        Rect rectangle;
        int GetPixel(int X, int Y)
        {
            int result;
            switch (lockedSurfaceValue.BitCount)
            {
                case 8:
                    result = lockedSurfaceValue.View().Byte(X + (lockedSurfaceValue.Pitch * Y));
                    break;
                case 16:
                    result = lockedSurfaceValue.View().UInt16(X + X + (lockedSurfaceValue.Pitch * Y));
                    break;
                case 24:
                    // Four bytes at the last pixel of a row too, as GFXEUnit.GetPixelInfo.
                    result = (int)lockedSurfaceValue.View().Dword((X * 3) + (lockedSurfaceValue.Pitch * Y)) & 16777215;
                    break;
                case 32:
                    result = (int)lockedSurfaceValue.View().Dword((X * 4) + (lockedSurfaceValue.Pitch * Y));
                    break;
                default:
                    throw new NotSupportedException("PixelFormat is not supported");
            }

            return result;
        }

        rectangle = R;
        found = false;
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

    internal static int RemapSurfacePart(in LockedSurface source, int sourceX, int sourceY, int width, int height, in LockedSurface destination, int destinationX, int destinationY)
    {
        SurfaceView s;
        SurfaceView d;
        int result = PixelFormats.GFXE_OK;
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
            destinationX = destinationX - sourceX;
            width = width + sourceX;
            sourceX = 0;
        }

        if (sourceY < 0)
        {
            destinationY = destinationY - sourceY;
            height = height + sourceY;
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
            sourceX = sourceX - destinationX;
            width = width + destinationX;
            destinationX = 0;
        }

        if (destinationY < 0)
        {
            sourceY = sourceY - destinationY;
            height = height + destinationY;
            destinationY = 0;
        }

        if (width > 0 && height > 0)
        {
            s = source.View((source.Pitch * sourceY) + ((int)(((uint)(source.BitCount + 7)) >> 3) * sourceX));
            d = destination.View((destination.Pitch * destinationY) + ((int)(((uint)(destination.BitCount + 7)) >> 3) * destinationX));
            switch (source.PixelFormatId)
            {
                case PixelFormats.pfPAL8:
                    switch (destination.PixelFormatId)
                    {
                        case PixelFormats.pfRGB565:
                            PixelRemap.RemapSurface08To565(s, d, width, height, source.Palette);
                            break;
                        case PixelFormats.pfRGB555:
                            PixelRemap.RemapSurface08To555(s, d, width, height, source.Palette);
                            break;
                        case PixelFormats.pfRGB24:
                            PixelRemap.RemapSurface08To24(s, d, width, height, source.Palette);
                            break;
                        case PixelFormats.pfPAL8:
                            BlitRects.BlitRectangle(s, d, width, height);
                            break;
                        default:
                            result = PixelFormats.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case PixelFormats.pfRGB555:
                case PixelFormats.pfARGB1555:
                    switch (destination.PixelFormatId)
                    {
                        case PixelFormats.pfRGB24:
                            PixelRemap.RemapSurface555To24(s, d, width, height);
                            break;
                        case PixelFormats.pfRGB32:
                            PixelRemap.RemapSurface555To32(s, d, width, height);
                            break;
                        case PixelFormats.pfRGB565:
                            PixelRemap.RemapSurface555To565(s, d, width, height);
                            break;
                        case PixelFormats.pfRGB555:
                            BlitRects.BlitRectangle(s, d, width * 2, height);
                            break;
                        default:
                            result = PixelFormats.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case PixelFormats.pfRGB565:
                    switch (destination.PixelFormatId)
                    {
                        case PixelFormats.pfRGB24:
                            PixelRemap.RemapSurface565To24(s, d, width, height);
                            break;
                        case PixelFormats.pfRGB32:
                            PixelRemap.RemapSurface565To32(s, d, width, height);
                            break;
                        case PixelFormats.pfRGB555:
                            PixelRemap.RemapSurface565To555(s, d, width, height);
                            break;
                        case PixelFormats.pfRGB565:
                            BlitRects.BlitRectangle(s, d, width * 2, height);
                            break;
                        default:
                            result = PixelFormats.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case PixelFormats.pfRGB24:
                    switch (destination.PixelFormatId)
                    {
                        case PixelFormats.pfRGB565:
                            PixelRemap.RemapSurface24To565(s, d, width, height);
                            break;
                        case PixelFormats.pfRGB555:
                            PixelRemap.RemapSurface24To555(s, d, width, height);
                            break;
                        case PixelFormats.pfRGB24:
                            BlitRects.BlitRectangle(s, d, width * 3, height);
                            break;
                        default:
                            result = PixelFormats.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                case PixelFormats.pfRGB32:
                    switch (destination.PixelFormatId)
                    {
                        case PixelFormats.pfRGB565:
                            PixelRemap.RemapSurface32To565(s, d, width, height);
                            break;
                        case PixelFormats.pfRGB555:
                            PixelRemap.RemapSurface32To555(s, d, width, height);
                            break;
                        case PixelFormats.pfRGB32:
                            BlitRects.BlitRectangle(s, d, width * 4, height);
                            break;
                        default:
                            result = PixelFormats.GFXE_NOTSUPPORTED;
                            break;
                    }

                    break;
                default:
                    result = PixelFormats.GFXE_NOTSUPPORTED;
                    break;
            }
        }

        return result;
    }

}
