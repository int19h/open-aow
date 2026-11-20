#nullable disable
// Translated from ImageLib.pas.
using System.Runtime.InteropServices;

using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.DelphiPaths;

namespace Aow.Game.Draw.Images;

// (ClassID $10): an opaque 16 bit picture. _pitch (bytes per row, a multiple of 4, or of 8 from
// 32 pixels on), FBuffer (the rows from its first byte. The original's FData, FBuffer aligned to
// 8 bytes, is FBuffer itself in the port)
public partial class Picture16(CustomImageLibrary owner) : Image16(owner), ILibraryImageClass
{
    static int ILibraryImageClass.ClassId => 16;
    static string ILibraryImageClass.Name => "Picture (16bpp)";

    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new Picture16(owner);

    private int _pitch;
    private byte[] _buffer;

    // The rows of the picture: FBuffer from its first byte, FPitch bytes apart.
    internal SurfaceView View => new(_buffer, 0, _pitch);

    // ImageLib.pas:2206
    internal void AllocateDiscardableImageData(int width, int height)
    {
        _pitch = width < 32 ? ((width << 1) + 3) & ~3 : ((width << 1) + 7) & ~7;

        // The rows start at the first byte of FBuffer, as TPicture08's (graphics.md 6.8).
        int size = (_pitch * height) + 7;
        _buffer = MemorySurfaces.Reallocate(_buffer, size);
    }

    // ImageLib.pas:2174
    public override void SetXshift(int value)
    {
        if (value < 0)
        {
            value = 0;
        }

        base.SetXshift(value);
        SetWidth(Width);
    }

    // ImageLib.pas:2181
    public override void SetYshift(int value)
    {
        if (value < 0)
        {
            value = 0;
        }

        base.SetYshift(value);
        SetHeight(Height);
    }

    // ImageLib.pas:2188
    public override void SetWidth(int value)
    {
        if (value < Xshift + DataWidth)
        {
            value = Xshift + DataWidth;
        }

        base.SetWidth(value);
    }

    // ImageLib.pas:2194
    public override void SetHeight(int value)
    {
        if (value < Yshift + DataHeight)
        {
            value = Yshift + DataHeight;
        }

        base.SetHeight(value);
    }

    // ImageLib.pas:2200
    public override bool SupportShowMode(ShowMode showMode, PixelFormat pixelFormatId) => (pixelFormatId is PixelFormat.None or PixelFormat.Rgb565 or PixelFormat.Rgb555) && Set<ShowMode>.Of(ShowMode.Opaque, ShowMode.Transparent, ShowMode.Blended).Contains(showMode);

    // ImageLib.pas:2219
    public override void ReleaseDiscardableImageData()
    {
        if (DataLoaded)
        {
            _buffer = null;
            DataLoaded = false;
        }
    }

    // ImageLib.pas:2228
    public override void ReadDiscardableImageData(Stream stream)
    {
        base.ReadDiscardableImageData(stream);
        AllocateDiscardableImageData(DataWidth, DataHeight);
        for (int y = 0, last = DataHeight - 1; y <= last; y++)
        {
            stream.ReadExactly(_buffer.AsSpan(y * _pitch, DataWidth * 2));
        }
    }

    // ImageLib.pas:2238
    public override void WriteDiscardableImageData(Stream stream)
    {
        base.WriteDiscardableImageData(stream);
        for (int y = 0, last = DataHeight - 1; y <= last; y++)
        {
            stream.Write(_buffer.AsSpan(y * _pitch, DataWidth * 2));
        }
    }

    // ImageLib.pas:2253
    public override bool SupportGrabPixelFormat(PixelFormat pixelFormatId) => pixelFormatId is PixelFormat.Rgb565 or PixelFormat.Rgb555 or PixelFormat.Rgb24 or PixelFormat.Pal8;

    // ImageLib.pas:2259
    public override bool GrabImage(in LockedSurface lockedSurface, in Rect R, GrabInfo grabInfo)
    {
        bool result;
        try
        {
            Clear();
            result = true;
            DataLoaded = true;
            DataWidth = R.Right - R.Left;
            DataHeight = R.Bottom - R.Top;
            SetWidthWithoutOverrides(DataWidth);
            SetHeightWithoutOverrides(DataHeight);
            FileName = ExtractFileName(grabInfo.FileName);
            FileIndex = grabInfo.FileIndex;
            DataSize = DataWidth * DataHeight * 2;
            AllocateDiscardableImageData(Width, Height);
            switch (lockedSurface.PixelFormatId)
            {
                case PixelFormat.Pal8:
                    {
                        for (int y = 0, last = Height - 1; y <= last; y++)
                        {
                            Span<byte> s = lockedSurface.View().Bytes(((R.Top + y) * lockedSurface.Pitch) + R.Left, Width);
                            Span<ushort> d = MemoryMarshal.Cast<byte, ushort>(_buffer.AsSpan(_pitch * y, Width * 2));
                            for (int x = 0, lastX = Width - 1; x <= lastX; x++)
                            {
                                Palette.Entry p = lockedSurface.Palette[s[x]];
                                d[x] = (ushort)(((p.Red & 248) << 8) + ((p.Green & 252) << 3) + (int)(((uint)p.Blue) >> 3));
                            }
                        }
                    }

                    StoredPixelFormatId = PixelFormat.Rgb565;
                    break;
                case PixelFormat.Rgb565:
                case PixelFormat.Rgb555:
                    {
                        for (int i = 0, last = Height - 1; i <= last; i++)
                        {
                            lockedSurface.View().Bytes(((R.Top + i) * lockedSurface.Pitch) + (R.Left * 2), Width * 2).CopyTo(_buffer.AsSpan(_pitch * i, Width * 2));
                        }
                    }

                    StoredPixelFormatId = lockedSurface.PixelFormatId;
                    break;
                case PixelFormat.Rgb24:
                    {
                        for (int y = 0, last = Height - 1; y <= last; y++)
                        {
                            Span<ushort> d = MemoryMarshal.Cast<byte, ushort>(_buffer.AsSpan(_pitch * y, Width * 2));
                            Span<byte> s24 = lockedSurface.View().Bytes(((R.Top + y) * lockedSurface.Pitch) + (R.Left * 3), Width * 3);
                            for (int x = 0, lastX = Width - 1; x <= lastX; x++)
                            {
                                d[x] = (ushort)(((s24[(x * 3) + 2] & 248) << 8) + ((s24[(x * 3) + 1] & 252) << 3) + (int)(((uint)s24[x * 3]) >> 3));
                            }
                        }
                    }

                    StoredPixelFormatId = PixelFormat.Rgb565;
                    break;
                case PixelFormat.None:
                case PixelFormat.Argb1555:
                case PixelFormat.Bgr24:
                case PixelFormat.Rgb32:
                case PixelFormat.Xrgb32:
                default:
                    result = false;
                    break;
            }

            PixelFormatId = StoredPixelFormatId;
        }
        catch
        {
            ReleaseDiscardableImageData();
            result = false;
        }

        return result;
    }

    // ImageLib.pas:2329
    public override GraphicsResult RemapToPixelFormat(PixelFormat pixelFormatId)
    {
        GraphicsResult result = GraphicsResult.Ok;
        if (pixelFormatId != PixelFormatId)
        {
            switch (pixelFormatId)
            {
                case PixelFormat.Rgb565:
                    PixelRemap.RemapSurface555To565(View, View, DataWidth, DataHeight);
                    PixelFormatId = PixelFormat.Rgb565;
                    break;
                case PixelFormat.Rgb555:
                    PixelRemap.RemapSurface565To555(View, View, DataWidth, DataHeight);
                    PixelFormatId = PixelFormat.Rgb555;
                    break;
                case PixelFormat.None:
                case PixelFormat.Pal8:
                case PixelFormat.Argb1555:
                case PixelFormat.Rgb24:
                case PixelFormat.Bgr24:
                case PixelFormat.Rgb32:
                case PixelFormat.Xrgb32:
                default:
                    result = GraphicsResult.NotSupported;
                    break;
            }
        }

        return result;
    }

    // ImageLib.pas:2349
    public override void ShowOpaque(int X, int Y, in LockedSurface lockedSurface)
    {
        int destinationX = X + Xshift;
        int w = DataWidth;
        int right = DataWidth + destinationX;
        int sx;
        if (destinationX < lockedSurface.ClipRectangle.Left)
        {
            sx = lockedSurface.ClipRectangle.Left - X;
            destinationX = lockedSurface.ClipRectangle.Left;
            w -= sx;
        }
        else
        {
            sx = 0;
        }

        if (right > lockedSurface.ClipRectangle.Right)
        {
            w -= right - lockedSurface.ClipRectangle.Right;
        }

        if (w > 0)
        {
            int destinationY = Y + Yshift;
            int h = DataHeight;
            int bottom = destinationY + DataHeight;
            int sy;
            if (destinationY < lockedSurface.ClipRectangle.Top)
            {
                sy = lockedSurface.ClipRectangle.Top - Y;
                destinationY = lockedSurface.ClipRectangle.Top;
                h -= sy;
            }
            else
            {
                sy = 0;
            }

            if (bottom > lockedSurface.ClipRectangle.Bottom)
            {
                h -= bottom - lockedSurface.ClipRectangle.Bottom;
            }

            if (h > 0)
            {
                SurfaceView source;
                SurfaceView destination;
                if (!DataLoaded)
                {
                    LoadDiscardableImageData();
                    source = View.At((sx * 2) + (sy * _pitch));
                    destination = lockedSurface.View((destinationX << 1) + (lockedSurface.Pitch * destinationY));
                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        BlitRects.BlitRectangle(source, destination, w * 2, h);
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitRects.BlitRectangle(source, destination, w * 2, h);
                    }

                    if (LoadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    source = View.At((sx * 2) + (sy * _pitch));
                    destination = lockedSurface.View((destinationX << 1) + (lockedSurface.Pitch * destinationY));
                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        BlitRects.BlitRectangle(source, destination, w * 2, h);
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitRects.BlitRectangle(source, destination, w * 2, h);
                    }
                }
            }
        }
    }

    // ImageLib.pas:2412
    public override void ShowBlended(int X, int Y, in LockedSurface lockedSurface, BlendTable table)
    {
        int destinationX = X + Xshift;
        int w = DataWidth;
        int right = DataWidth + destinationX;
        int sx;
        if (destinationX < lockedSurface.ClipRectangle.Left)
        {
            sx = lockedSurface.ClipRectangle.Left - X;
            destinationX = lockedSurface.ClipRectangle.Left;
            w -= sx;
        }
        else
        {
            sx = 0;
        }

        if (right > lockedSurface.ClipRectangle.Right)
        {
            w -= right - lockedSurface.ClipRectangle.Right;
        }

        if (w > 0)
        {
            int destinationY = Y + Yshift;
            int h = DataHeight;
            int bottom = destinationY + DataHeight;
            int sy;
            if (destinationY < lockedSurface.ClipRectangle.Top)
            {
                sy = lockedSurface.ClipRectangle.Top - Y;
                destinationY = lockedSurface.ClipRectangle.Top;
                h -= sy;
            }
            else
            {
                sy = 0;
            }

            if (bottom > lockedSurface.ClipRectangle.Bottom)
            {
                h -= bottom - lockedSurface.ClipRectangle.Bottom;
            }

            if (h > 0)
            {
                SurfaceView source;
                SurfaceView destination;
                if (!DataLoaded)
                {
                    LoadDiscardableImageData();
                    source = View.At((sx * 2) + (sy * _pitch));
                    destination = lockedSurface.View((destinationX << 1) + (lockedSurface.Pitch * destinationY));
                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        if (table.Bits == 5)
                        {
                            switch (PixelFormatId)
                            {
                                case PixelFormat.Rgb565:
                                    BlendSurface565(source, destination, w, h, table.Bytes);
                                    break;
                                case PixelFormat.Rgb555:
                                    BlendSurface555(source, destination, w, h, table.Bytes);
                                    break;
                                case PixelFormat.None:
                                case PixelFormat.Pal8:
                                case PixelFormat.Argb1555:
                                case PixelFormat.Rgb24:
                                case PixelFormat.Bgr24:
                                case PixelFormat.Rgb32:
                                case PixelFormat.Xrgb32:
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid BlendTable format");
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        if (table.Bits == 5)
                        {
                            switch (PixelFormatId)
                            {
                                case PixelFormat.Rgb565:
                                    BlendSurface565(source, destination, w, h, table.Bytes);
                                    break;
                                case PixelFormat.Rgb555:
                                    BlendSurface555(source, destination, w, h, table.Bytes);
                                    break;
                                case PixelFormat.None:
                                case PixelFormat.Pal8:
                                case PixelFormat.Argb1555:
                                case PixelFormat.Rgb24:
                                case PixelFormat.Bgr24:
                                case PixelFormat.Rgb32:
                                case PixelFormat.Xrgb32:
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid BlendTable format");
                        }
                    }

                    if (LoadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    source = View.At((sx * 2) + (sy * _pitch));
                    destination = lockedSurface.View((destinationX << 1) + (lockedSurface.Pitch * destinationY));
                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        if (table.Bits == 5)
                        {
                            switch (PixelFormatId)
                            {
                                case PixelFormat.Rgb565:
                                    BlendSurface565(source, destination, w, h, table.Bytes);
                                    break;
                                case PixelFormat.Rgb555:
                                    BlendSurface555(source, destination, w, h, table.Bytes);
                                    break;
                                case PixelFormat.None:
                                case PixelFormat.Pal8:
                                case PixelFormat.Argb1555:
                                case PixelFormat.Rgb24:
                                case PixelFormat.Bgr24:
                                case PixelFormat.Rgb32:
                                case PixelFormat.Xrgb32:
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid BlendTable format");
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        if (table.Bits == 5)
                        {
                            switch (PixelFormatId)
                            {
                                case PixelFormat.Rgb565:
                                    BlendSurface565(source, destination, w, h, table.Bytes);
                                    break;
                                case PixelFormat.Rgb555:
                                    BlendSurface555(source, destination, w, h, table.Bytes);
                                    break;
                                case PixelFormat.None:
                                case PixelFormat.Pal8:
                                case PixelFormat.Argb1555:
                                case PixelFormat.Rgb24:
                                case PixelFormat.Bgr24:
                                case PixelFormat.Rgb32:
                                case PixelFormat.Xrgb32:
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid BlendTable format");
                        }
                    }
                }
            }
        }
    }

    // ImageLib.pas:2511
    public override void ShowTransparent(int X, int Y, in LockedSurface lockedSurface)
    {
        int destinationX = X + Xshift;
        int w = DataWidth;
        int right = DataWidth + destinationX;
        int sx;
        if (destinationX < lockedSurface.ClipRectangle.Left)
        {
            sx = lockedSurface.ClipRectangle.Left - X;
            destinationX = lockedSurface.ClipRectangle.Left;
            w -= sx;
        }
        else
        {
            sx = 0;
        }

        if (right > lockedSurface.ClipRectangle.Right)
        {
            w -= right - lockedSurface.ClipRectangle.Right;
        }

        if (w > 0)
        {
            int destinationY = Y + Yshift;
            int h = DataHeight;
            int bottom = destinationY + DataHeight;
            int sy;
            if (destinationY < lockedSurface.ClipRectangle.Top)
            {
                sy = lockedSurface.ClipRectangle.Top - Y;
                destinationY = lockedSurface.ClipRectangle.Top;
                h -= sy;
            }
            else
            {
                sy = 0;
            }

            if (bottom > lockedSurface.ClipRectangle.Bottom)
            {
                h -= bottom - lockedSurface.ClipRectangle.Bottom;
            }

            if (h > 0)
            {
                SurfaceView source;
                SurfaceView destination;
                if (!DataLoaded)
                {
                    LoadDiscardableImageData();
                    source = View.At((sx * 2) + (sy * _pitch));
                    destination = lockedSurface.View((destinationX << 1) + (lockedSurface.Pitch * destinationY));
                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        if (PixelFormatId == PixelFormat.Rgb565)
                        {
                            BlitTransparent565(source, destination, w, h);
                        }
                        else
                        {
                            BlitTransparent555(source, destination, w, h);
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        if (PixelFormatId == PixelFormat.Rgb565)
                        {
                            BlitTransparent565(source, destination, w, h);
                        }
                        else
                        {
                            BlitTransparent555(source, destination, w, h);
                        }
                    }

                    if (LoadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    source = View.At((sx * 2) + (sy * _pitch));
                    destination = lockedSurface.View((destinationX << 1) + (lockedSurface.Pitch * destinationY));
                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        if (PixelFormatId == PixelFormat.Rgb565)
                        {
                            BlitTransparent565(source, destination, w, h);
                        }
                        else
                        {
                            BlitTransparent555(source, destination, w, h);
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        if (PixelFormatId == PixelFormat.Rgb565)
                        {
                            BlitTransparent565(source, destination, w, h);
                        }
                        else
                        {
                            BlitTransparent555(source, destination, w, h);
                        }
                    }
                }
            }
        }
    }

    private static void BlitTransparent565(SurfaceView source, SurfaceView destination, int width, int height) => Blit.BlitTransparent565(source, destination, width, height);

    private static void BlitTransparent555(SurfaceView source, SurfaceView destination, int width, int height) => Blit.BlitTransparent555(source, destination, width, height);

    internal static void BlitStretched16(SurfaceView source, ReadOnlySpan<int> rowTable, ReadOnlySpan<int> columnTable, SurfaceView destination, int width, int height) => Blit.BlitStretched16(source, rowTable, columnTable, destination, width, height);

    private static void BlendSurface555(SurfaceView source, SurfaceView destination, int width, int height, ReadOnlySpan<byte> table) => Blit.BlendSurface555(source, destination, width, height, table);

    private static void BlendSurface565(SurfaceView source, SurfaceView destination, int width, int height, ReadOnlySpan<byte> table) => Blit.BlendSurface565(source, destination, width, height, table);

    internal static void BlendStretched555(SurfaceView source, ReadOnlySpan<int> rowTable, ReadOnlySpan<int> columnTable, SurfaceView destination, int width, int height, ReadOnlySpan<byte> table) => Blit.BlendStretched555(source, rowTable, columnTable, destination, width, height, table);

    private static void BlendStretched565(SurfaceView source, ReadOnlySpan<int> rowTable, ReadOnlySpan<int> columnTable, SurfaceView destination, int width, int height, ReadOnlySpan<byte> table) => Blit.BlendStretched565(source, rowTable, columnTable, destination, width, height, table);

}
