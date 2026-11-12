
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

    public Rect ClipRectangle;

    public byte[] Data { get; set; }
    public int DataIndex { get; set; }
    public Palette Palette { get; init; }
    public int PixelFormatId { get; init; }

    public int RMask;
    public int GMask;
    public int BMask;
    public int AMask;

    // The view of the surface's rows for the blitters, moved by a byte offset from the first
    // pixel of row 0 (`Pointer(Integer(SD.Data) + Offset)` with `SD.Pitch` in the original), with
    // the descriptor's clip rectangle.
    public readonly SurfaceView View(int offset = 0) => new(Data, DataIndex + offset, Pitch, ClipRectangle);

    internal static int BlitSprite16(in LockedSurface source, Rect sourceRectangle, in LockedSurface destination, int X, int Y, int colorKey)
    {
        int result;
        int key;
        SurfaceView S;
        SurfaceView D;
        if (source.BitCount == 16 && destination.PixelFormatId == source.PixelFormatId)
        {
            key = PixelFormats.Color2PixelInfo(colorKey, source.PixelFormatId, null);
            S = source.View(source.Pitch * sourceRectangle.Top + sourceRectangle.Left * 2);
            D = destination.View(destination.Pitch * Y + X * 2);
            BlitRects.BlitColorKey16(S, D, sourceRectangle.Right - sourceRectangle.Left, sourceRectangle.Bottom - sourceRectangle.Top, key);
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
        int W;
        int H;
        SurfaceView S;
        SurfaceView D;
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

        W = sourceRectangle.Right - sourceRectangle.Left;
        if (X + W > destination.ClipRectangle.Right)
        {
            W = W - (X + W - destination.ClipRectangle.Right);
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

        H = sourceRectangle.Bottom - sourceRectangle.Top;
        if (Y + H > destination.ClipRectangle.Bottom)
        {
            H = H - (Y + H - destination.ClipRectangle.Bottom);
        }

        if (source.PixelFormatId == destination.PixelFormatId)
        {
            if (W > 0 && H > 0)
            {
                W = (int)(((uint)(source.BitCount * W)) >> 3);
                S = source.View(source.Pitch * sourceRectangle.Top + (int)(((uint)(source.BitCount * sourceRectangle.Left)) >> 3));
                D = destination.View(destination.Pitch * Y + (int)(((uint)(destination.BitCount * X)) >> 3));
                BlitRects.BlitRectangle(S, D, W, H);
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
        int W;
        int H;
        SurfaceView S;
        SurfaceView D;
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

            W = sourceRectangle.Right - sourceRectangle.Left;
            if (W + X > destination.ClipRectangle.Right)
            {
                W = W - (W + X - destination.ClipRectangle.Right);
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

            H = sourceRectangle.Bottom - sourceRectangle.Top;
            if (Y + H > destination.ClipRectangle.Bottom)
            {
                H = H - (Y + H - destination.ClipRectangle.Bottom);
            }

            if (W > 0 && H > 0)
            {
                key = PixelFormats.Color2PixelInfo(colorKey, source.PixelFormatId, null);
                S = source.View(source.Pitch * sourceRectangle.Top + sourceRectangle.Left * 2);
                D = destination.View(destination.Pitch * Y);
                X += X;
                D = D.At(X);
                BlitRects.BlitColorKey16(S, D, W, H, key);
            }

            result = PixelFormats.GFXE_OK;
        }
        else
        {
            result = PixelFormats.GFXE_PIXELFORMATNOTSUPPORTED;
        }

        return result;
    }

    public static void CreateFrom(in LockedSurface source, in Rect R, ref LockedSurface destination)
    {
        destination = source;
        IntersectRect(ref destination.ClipRectangle, source.ClipRectangle, R);
        destination.DataIndex = source.DataIndex + R.Top * source.Pitch + (int)(((uint)source.BitCount) >> 3) * R.Left;
        destination.ClipRectangle.Left -= R.Left;
        destination.ClipRectangle.Right -= R.Left;
        destination.ClipRectangle.Top -= R.Top;
        destination.ClipRectangle.Bottom -= R.Top;
    }

    internal static void MinimizeTransparentRectangle(in LockedSurface lockedSurface, in Rect R, int transparentColor, ref Rect result)
    {
        LockedSurface lockedSurfaceValue = lockedSurface;
        bool found;
        int X;
        int Y;
        Rect rectangle;
        int GetPixel(int X, int Y)
        {
            int result;
            switch (lockedSurfaceValue.BitCount)
            {
                case 8:
                    result = lockedSurfaceValue.View().Byte(X + lockedSurfaceValue.Pitch * Y);
                    break;
                case 16:
                    result = lockedSurfaceValue.View().UInt16(X + X + lockedSurfaceValue.Pitch * Y);
                    break;
                case 24:
                    // Four bytes at the last pixel of a row too, as GFXEUnit.GetPixelInfo.
                    result = (int)lockedSurfaceValue.View().Dword(X * 3 + lockedSurfaceValue.Pitch * Y) & 16777215;
                    break;
                case 32:
                    result = (int)lockedSurfaceValue.View().Dword(X * 4 + lockedSurfaceValue.Pitch * Y);
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
            X = rectangle.Left;
            while (!found && X != rectangle.Right)
            {
                found = GetPixel(X, rectangle.Top) != transparentColor;
                X += 1;
            }

            if (!found)
            {
                rectangle.Top += 1;
            }
        }

        found = false;
        while (!found && rectangle.Bottom > rectangle.Top)
        {
            X = rectangle.Left;
            while (!found && X != rectangle.Right)
            {
                found = GetPixel(X, rectangle.Bottom - 1) != transparentColor;
                X += 1;
            }

            if (!found)
            {
                rectangle.Bottom -= 1;
            }
        }

        found = false;
        while (!found && rectangle.Left < rectangle.Right)
        {
            Y = rectangle.Top;
            while (!found && Y != rectangle.Bottom)
            {
                found = GetPixel(rectangle.Left, Y) != transparentColor;
                Y += 1;
            }

            if (!found)
            {
                rectangle.Left += 1;
            }
        }

        found = false;
        while (!found && rectangle.Right > rectangle.Left)
        {
            Y = rectangle.Top;
            while (!found && Y != rectangle.Bottom)
            {
                found = GetPixel(rectangle.Right - 1, Y) != transparentColor;
                Y += 1;
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
        SurfaceView S;
        SurfaceView D;
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
            S = source.View(source.Pitch * sourceY + (int)(((uint)(source.BitCount + 7)) >> 3) * sourceX);
            D = destination.View(destination.Pitch * destinationY + (int)(((uint)(destination.BitCount + 7)) >> 3) * destinationX);
            switch (source.PixelFormatId)
            {
                case PixelFormats.pfPAL8:
                    switch (destination.PixelFormatId)
                    {
                        case PixelFormats.pfRGB565:
                            PixelRemap.RemapSurface08To565(S, D, width, height, source.Palette);
                            break;
                        case PixelFormats.pfRGB555:
                            PixelRemap.RemapSurface08To555(S, D, width, height, source.Palette);
                            break;
                        case PixelFormats.pfRGB24:
                            PixelRemap.RemapSurface08To24(S, D, width, height, source.Palette);
                            break;
                        case PixelFormats.pfPAL8:
                            BlitRects.BlitRectangle(S, D, width, height);
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
                            PixelRemap.RemapSurface555To24(S, D, width, height);
                            break;
                        case PixelFormats.pfRGB32:
                            PixelRemap.RemapSurface555To32(S, D, width, height);
                            break;
                        case PixelFormats.pfRGB565:
                            PixelRemap.RemapSurface555To565(S, D, width, height);
                            break;
                        case PixelFormats.pfRGB555:
                            BlitRects.BlitRectangle(S, D, width * 2, height);
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
                            PixelRemap.RemapSurface565To24(S, D, width, height);
                            break;
                        case PixelFormats.pfRGB32:
                            PixelRemap.RemapSurface565To32(S, D, width, height);
                            break;
                        case PixelFormats.pfRGB555:
                            PixelRemap.RemapSurface565To555(S, D, width, height);
                            break;
                        case PixelFormats.pfRGB565:
                            BlitRects.BlitRectangle(S, D, width * 2, height);
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
                            PixelRemap.RemapSurface24To565(S, D, width, height);
                            break;
                        case PixelFormats.pfRGB555:
                            PixelRemap.RemapSurface24To555(S, D, width, height);
                            break;
                        case PixelFormats.pfRGB24:
                            BlitRects.BlitRectangle(S, D, width * 3, height);
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
                            PixelRemap.RemapSurface32To565(S, D, width, height);
                            break;
                        case PixelFormats.pfRGB555:
                            PixelRemap.RemapSurface32To555(S, D, width, height);
                            break;
                        case PixelFormats.pfRGB32:
                            BlitRects.BlitRectangle(S, D, width * 4, height);
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
