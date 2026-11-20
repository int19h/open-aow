#nullable disable
// Translated from ImageLib.pas.
using System.Buffers.Binary;

using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.DelphiPaths;

namespace Aow.Game.Draw.Images;

// (ClassID $11): a run length encoded 16 bit sprite. FColorKey (the transparent color, in
// FPixelFormatID), _data (FDataSize bytes), _spriteX, _spriteY, _spriteWidth, _spriteHeight (as
// TSprite16). The data is FSpriteHeight rows. a row is a word with its size in bytes (the
// header included), a zero word, then pixels and skips (the color key and the size of the skip
// in bytes), padded with the color key to a dword
public class RleSprite16(CustomImageLibrary owner) : Image16(owner), ILibraryImageClass
{
    static int ILibraryImageClass.ClassId => 17;
    static string ILibraryImageClass.Name => "RLE Sprite (16bpp)";

    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new RleSprite16(owner);

    private int _colorKey;
    private byte[] _data;
    private int _spriteX;
    private int _spriteY;
    private int _spriteWidth;
    private int _spriteHeight;

    // ImageLib.pas:2896
    public override void SetXshift(int value)
    {
        if (value < -_spriteX)
        {
            value = -_spriteX;
        }

        base.SetXshift(value);
        SetWidth(Width);
    }

    // ImageLib.pas:2903
    public override void SetYshift(int value)
    {
        if (value < -_spriteY)
        {
            value = -_spriteY;
        }

        base.SetYshift(value);
        SetHeight(Height);
    }

    // ImageLib.pas:2910
    public override void SetWidth(int value)
    {
        int w = Xshift + _spriteX + _spriteWidth;
        if (value < w)
        {
            value = w;
        }

        base.SetWidth(value);
    }

    // ImageLib.pas:2919
    public override void SetHeight(int value)
    {
        int w = Yshift + _spriteY + _spriteHeight;
        if (value < w)
        {
            value = w;
        }

        base.SetHeight(value);
    }

    // ImageLib.pas:2928
    public override void GetVisualDimensions(out Rect R)
    {
        int left = Xshift + _spriteX;
        int top = Yshift + _spriteY;
        R = new() { Left = left, Right = left + _spriteWidth, Top = top, Bottom = top + _spriteHeight };
    }

    // ImageLib.pas:2936
    public override void ReleaseDiscardableImageData()
    {
        if (DataLoaded)
        {
            _data = null;
            DataLoaded = false;
        }
    }

    // ImageLib.pas:2947
    // { The last dword of the image data is not used: ReadImageData skips it and WriteImageData
    //   writes an uninitialized local variable (an original bug). }
    public override void ReadImageData(Stream stream)
    {
        int reserved = default;
        base.ReadImageData(stream);
        stream.ReadExactly(Bytes.Of(ref _spriteWidth, 4));
        stream.ReadExactly(Bytes.Of(ref _spriteHeight, 4));
        stream.ReadExactly(Bytes.Of(ref _spriteX, 4));
        stream.ReadExactly(Bytes.Of(ref _spriteY, 4));
        stream.ReadExactly(Bytes.Of(ref _colorKey, 4));
        stream.ReadExactly(Bytes.Of(ref reserved, 4));
    }

    // ImageLib.pas:2960
    public override void WriteImageData(Stream stream)
    {
        int reserved = default;
        base.WriteImageData(stream);
        stream.Write(Bytes.Of(ref _spriteWidth, 4));
        stream.Write(Bytes.Of(ref _spriteHeight, 4));
        stream.Write(Bytes.Of(ref _spriteX, 4));
        stream.Write(Bytes.Of(ref _spriteY, 4));
        stream.Write(Bytes.Of(ref _colorKey, 4));
        stream.Write(Bytes.Of(ref reserved, 4));
    }

    // ImageLib.pas:2973
    public override void ReadDiscardableImageData(Stream stream)
    {
        base.ReadDiscardableImageData(stream);
        _data = MemorySurfaces.Reallocate(_data, DataSize);
        stream.ReadExactly(_data.AsSpan(0, DataSize));
    }

    // ImageLib.pas:2980
    public override void WriteDiscardableImageData(Stream stream)
    {
        base.WriteDiscardableImageData(stream);
        stream.Write(_data.AsSpan(0, DataSize));
    }

    // ImageLib.pas:2986
    public override GraphicsResult RemapToPixelFormat(PixelFormat pixelFormatId)
    {
        if (pixelFormatId != PixelFormatId)
        {
            switch (pixelFormatId)
            {
                case PixelFormat.Rgb565:
                    RemapRle555To565(_data, _spriteHeight, _colorKey);
                    PixelFormatId = PixelFormat.Rgb565;
                    break;
                case PixelFormat.Rgb555:
                    RemapRle565To555(_data, _spriteHeight, _colorKey);
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
                    return GraphicsResult.NotSupported;
            }
        }

        return GraphicsResult.Ok;
    }

    // ImageLib.pas:3006
    public override bool SupportShowMode(ShowMode showMode, PixelFormat pixelFormatId) => (pixelFormatId is PixelFormat.None or PixelFormat.Rgb565 or PixelFormat.Rgb555) && Set<ShowMode>.Of(ShowMode.Opaque, ShowMode.Transparent).Contains(showMode);

    // ImageLib.pas:3012
    public override void ShowOpaque(int X, int Y, in LockedSurface lockedSurface)
    {
        if (!DataLoaded)
        {
            LoadDiscardableImageData();
            if (lockedSurface.PixelFormatId == PixelFormatId)
            {
                BlitRle16(X + Xshift + _spriteX, Y + Yshift + _spriteY, lockedSurface, _data, _spriteWidth, _spriteHeight, _colorKey);
            }
            else
            {
                if (RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                BlitRle16(X + Xshift + _spriteX, Y + Yshift + _spriteY, lockedSurface, _data, _spriteWidth, _spriteHeight, _colorKey);
            }

            if (LoadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            if (lockedSurface.PixelFormatId == PixelFormatId)
            {
                BlitRle16(X + Xshift + _spriteX, Y + Yshift + _spriteY, lockedSurface, _data, _spriteWidth, _spriteHeight, _colorKey);
            }
            else
            {
                if (RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                BlitRle16(X + Xshift + _spriteX, Y + Yshift + _spriteY, lockedSurface, _data, _spriteWidth, _spriteHeight, _colorKey);
            }
        }
    }

    // ImageLib.pas:3045
    // { ShowTransparent shows the sprite blended 50% }
    public override void ShowTransparent(int X, int Y, in LockedSurface lockedSurface)
    {
        if (!DataLoaded)
        {
            LoadDiscardableImageData();
            if (lockedSurface.PixelFormatId != PixelFormatId && RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
            {
                throw new NotSupportedException("Surface pixel format not supported!");
            }

            switch (PixelFormatId)
            {
                case PixelFormat.Rgb565:
                    BlendRle565(X + Xshift + _spriteX, Y + Yshift + _spriteY, lockedSurface, _data, _spriteWidth, _spriteHeight, _colorKey);
                    break;
                case PixelFormat.Rgb555:
                    BlendRle555(X + Xshift + _spriteX, Y + Yshift + _spriteY, lockedSurface, _data, _spriteWidth, _spriteHeight, _colorKey);
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

            if (LoadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            if (lockedSurface.PixelFormatId != PixelFormatId && RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
            {
                throw new NotSupportedException("Surface pixel format not supported!");
            }

            switch (PixelFormatId)
            {
                case PixelFormat.Rgb565:
                    BlendRle565(X + Xshift + _spriteX, Y + Yshift + _spriteY, lockedSurface, _data, _spriteWidth, _spriteHeight, _colorKey);
                    break;
                case PixelFormat.Rgb555:
                    BlendRle555(X + Xshift + _spriteX, Y + Yshift + _spriteY, lockedSurface, _data, _spriteWidth, _spriteHeight, _colorKey);
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
    }

    // ImageLib.pas:3073
    public override bool SupportGrabPixelFormat(PixelFormat pixelFormatId) => pixelFormatId is PixelFormat.Rgb565 or PixelFormat.Rgb555 or PixelFormat.Rgb24 or PixelFormat.Pal8;

    // ImageLib.pas:3090
    // The sprites take their transparent color from TGrabInfo (declared Reserved in the core),
    // not from TransparentColor (+4, the color GetSurfaceRectangles separates the images with).
    // GrabImage encodes the rows into FData, which grows in 4 KB steps. FDataSize stays 0 until
    // the end, so every word added past the first step reallocates (an original inefficiency)
    public override bool GrabImage(in LockedSurface lockedSurface, in Rect R, GrabInfo grabInfo)
    {
        int position;
        Rect gr = default;
        void AddWord(ushort W)
        {
            if (DataSize <= position)
            {
                _data = MemorySurfaces.Reallocate(_data, position + 4096);
            }

            BinaryPrimitives.WriteUInt16LittleEndian(_data.AsSpan(position, 2), W);
            position += 2;
        }

        int rowStart;
        void StartRow()
        {
            rowStart = position;
            AddWord(0);
            AddWord(0);
        }

        int skip;
        void FlushSkip()
        {
            if (skip != 0)
            {
                AddWord((ushort)_colorKey);
                AddWord((ushort)(skip * 2));
                skip = 0;
            }
        }

        void EndRow()
        {
            FlushSkip();
            if (DataSize <= position)
            {
                _data = MemorySurfaces.Reallocate(_data, position + 4096);
            }

            BinaryPrimitives.WriteUInt16LittleEndian(_data.AsSpan(rowStart, 2), (ushort)(position - rowStart));

            if ((position & 3) != 0)
            {
                AddWord((ushort)_colorKey);
            }
        }
        try
        {
            DataSurface surface = null;
            try
            {
                LockedSurface lSD = default;
                if (lockedSurface.BitCount != 16)
                {
                    surface = new DataSurface(R.Right - R.Left, R.Bottom - R.Top, PixelFormat.Rgb565, null);
                    gr.Left = 0;
                    gr.Top = 0;
                    int width = surface.Width;
                    gr.Right = width;
                    int height = surface.Height;
                    gr.Bottom = height;
                    _ = surface.Lock(ref lSD);
                    if (LockedSurface.RemapSurfacePart(width: surface.Width, height: surface.Height, source: lockedSurface, sourceX: R.Left, sourceY: R.Top, destination: lSD, destinationX: 0, destinationY: 0) != 0)
                    {
                        throw new ArgumentException("Invalid PixelFormat");
                    }

                    _colorKey = PixelRemap.RemapPixelInfo(grabInfo.TransparentPixel, lockedSurface.PixelFormatId, lockedSurface.Palette, lSD.PixelFormatId, null);
                }
                else
                {
                    lSD = lockedSurface;
                    gr = R;
                    _colorKey = grabInfo.TransparentPixel;
                }

                Clear();
                DataLoaded = true;
                DataWidth = gr.Right - gr.Left;
                DataHeight = gr.Bottom - gr.Top;
                SetWidthWithoutOverrides(DataWidth);
                SetHeightWithoutOverrides(DataHeight);
                FileName = ExtractFileName(grabInfo.FileName);
                FileIndex = grabInfo.FileIndex;
                StoredPixelFormatId = lSD.PixelFormatId;
                PixelFormatId = StoredPixelFormatId;
                position = 0;
                skip = 0;
                LockedSurface.MinimizeTransparentRectangle(lSD, gr, _colorKey, out Rect mr);
                _spriteX = mr.Left - gr.Left;
                _spriteY = mr.Top - gr.Top;
                _spriteWidth = mr.Right - mr.Left;
                _spriteHeight = mr.Bottom - mr.Top;
                for (int y = mr.Top, last = mr.Bottom - 1; y <= last; y++)
                {
                    int row = lSD.Pitch * y;
                    StartRow();
                    for (int x = mr.Left, lastX = mr.Right - 1; x <= lastX; x++)
                    {
                        int c = lSD.View().UInt16(row + x + x);
                        if (c != _colorKey)
                        {
                            FlushSkip();
                            AddWord((ushort)c);
                        }
                        else
                        {
                            skip += 1;
                        }
                    }

                    EndRow();
                }

                _data = MemorySurfaces.Reallocate(_data, position);
                DataSize = position;
            }
            finally
            {
                surface?.Dispose();
            }
        }
        catch
        {
            ReleaseDiscardableImageData();
            return false;
        }

        return true;
    }

    private static void RemapRle565To555(Span<byte> data, int height, int colorKey) => Blit.RemapRle565To555(data, height, colorKey);

    private static void RemapRle555To565(Span<byte> data, int height, int colorKey) => Blit.RemapRle555To565(data, height, colorKey);

    private static void BlitRle16(int X, int Y, in LockedSurface lockedSurface, ReadOnlySpan<byte> data, int width, int height, int colorKey) => Blit.BlitRle16(X, Y, lockedSurface.View(), data, width, height, colorKey);

    private static void BlendRle565(int X, int Y, in LockedSurface lockedSurface, ReadOnlySpan<byte> data, int width, int height, int colorKey) => Blit.BlendRle565(X, Y, lockedSurface.View(), data, width, height, colorKey);

    private static void BlendRle555(int X, int Y, in LockedSurface lockedSurface, ReadOnlySpan<byte> data, int width, int height, int colorKey) => Blit.BlendRle555(X, Y, lockedSurface.View(), data, width, height, colorKey);

}
