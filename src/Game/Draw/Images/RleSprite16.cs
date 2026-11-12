// Translated from ImageLib.pas.
using System.Buffers.Binary;

using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// (ClassID $11): a run length encoded 16 bit sprite. FColorKey (the transparent color, in
// FPixelFormatID), _data (FDataSize bytes), _spriteX, _spriteY, _spriteWidth, _spriteHeight (as
// TSprite16). The data is FSpriteHeight rows. a row is a word with its size in bytes (the
// header included), a zero word, then pixels and skips (the color key and the size of the skip
// in bytes), padded with the color key to a dword
public partial class RleSprite16 : Image16, ILibraryImageClass
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

    // ImageLib.pas:2881
    public RleSprite16(CustomImageLibrary owner) : base(owner)
    {
    }

    // ImageLib.pas:2896
    public override void SetXshift(int value)
    {
        if (value < -_spriteX)
        {
            value = -_spriteX;
        }

        base.SetXshift(value);
        SetWidth(_width);
    }

    // ImageLib.pas:2903
    public override void SetYshift(int value)
    {
        if (value < -_spriteY)
        {
            value = -_spriteY;
        }

        base.SetYshift(value);
        SetHeight(_height);
    }

    // ImageLib.pas:2910
    public override void SetWidth(int value)
    {
        int W;
        W = _xshift + _spriteX + _spriteWidth;
        if (value < W)
        {
            value = W;
        }

        base.SetWidth(value);
    }

    // ImageLib.pas:2919
    public override void SetHeight(int value)
    {
        int W;
        W = _yshift + _spriteY + _spriteHeight;
        if (value < W)
        {
            value = W;
        }

        base.SetHeight(value);
    }

    // ImageLib.pas:2928
    public override void GetVisualDimensions(out Rect R)
    {
        R.Left = _xshift + _spriteX;
        R.Right = R.Left + _spriteWidth;
        R.Top = _yshift + _spriteY;
        R.Bottom = R.Top + _spriteHeight;
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
        _data = MemorySurfaces.Reallocate(_data, _dataSize);
        stream.ReadExactly(_data.AsSpan(0, _dataSize));
    }

    // ImageLib.pas:2980
    public override void WriteDiscardableImageData(Stream stream)
    {
        base.WriteDiscardableImageData(stream);
        stream.Write(_data.AsSpan(0, _dataSize));
    }

    // ImageLib.pas:2986
    public override int RemapToPixelFormat(int pixelFormatId)
    {
        int result = 0;
        if (pixelFormatId != PixelFormatId)
        {
            switch (pixelFormatId)
            {
                case PixelFormats.pfRGB565:
                    RemapRle555To565(_data, _spriteHeight, _colorKey);
                    PixelFormatId = PixelFormats.pfRGB565;
                    break;
                case PixelFormats.pfRGB555:
                    RemapRle565To555(_data, _spriteHeight, _colorKey);
                    PixelFormatId = PixelFormats.pfRGB555;
                    break;
                default:
                    result = PixelFormats.GFXE_NOTSUPPORTED;
                    break;
            }
        }

        return result;
    }

    // ImageLib.pas:3006
    public override bool SupportShowMode(ShowMode showMode, int pixelFormatId) => (pixelFormatId == 0 || pixelFormatId == PixelFormats.pfRGB565 || pixelFormatId == PixelFormats.pfRGB555) && Set<ShowMode>.Of(ShowMode.Opaque, ShowMode.Transparent).Contains(showMode);

    // ImageLib.pas:3012
    public override void ShowOpaque(int X, int Y, in LockedSurface lockedSurface)
    {
        if (!DataLoaded)
        {
            LoadDiscardableImageData();
            if (lockedSurface.PixelFormatId == PixelFormatId)
            {
                BlitRle16(X + _xshift + _spriteX, Y + _yshift + _spriteY, lockedSurface, _data, _spriteWidth, _spriteHeight, _colorKey);
            }
            else
            {
                if (RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                BlitRle16(X + _xshift + _spriteX, Y + _yshift + _spriteY, lockedSurface, _data, _spriteWidth, _spriteHeight, _colorKey);
            }

            if (_loadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            if (lockedSurface.PixelFormatId == PixelFormatId)
            {
                BlitRle16(X + _xshift + _spriteX, Y + _yshift + _spriteY, lockedSurface, _data, _spriteWidth, _spriteHeight, _colorKey);
            }
            else
            {
                if (RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                BlitRle16(X + _xshift + _spriteX, Y + _yshift + _spriteY, lockedSurface, _data, _spriteWidth, _spriteHeight, _colorKey);
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
            if (lockedSurface.PixelFormatId != PixelFormatId && RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
            {
                throw new NotSupportedException("Surface pixel format not supported!");
            }

            switch (PixelFormatId)
            {
                case PixelFormats.pfRGB565:
                    BlendRle565(X + _xshift + _spriteX, Y + _yshift + _spriteY, lockedSurface, _data, _spriteWidth, _spriteHeight, _colorKey);
                    break;
                case PixelFormats.pfRGB555:
                    BlendRle555(X + _xshift + _spriteX, Y + _yshift + _spriteY, lockedSurface, _data, _spriteWidth, _spriteHeight, _colorKey);
                    break;
            }

            if (_loadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            if (lockedSurface.PixelFormatId != PixelFormatId && RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
            {
                throw new NotSupportedException("Surface pixel format not supported!");
            }

            switch (PixelFormatId)
            {
                case PixelFormats.pfRGB565:
                    BlendRle565(X + _xshift + _spriteX, Y + _yshift + _spriteY, lockedSurface, _data, _spriteWidth, _spriteHeight, _colorKey);
                    break;
                case PixelFormats.pfRGB555:
                    BlendRle555(X + _xshift + _spriteX, Y + _yshift + _spriteY, lockedSurface, _data, _spriteWidth, _spriteHeight, _colorKey);
                    break;
            }
        }
    }

    // ImageLib.pas:3073
    public override bool SupportGrabPixelFormat(int pixelFormatId) => pixelFormatId == PixelFormats.pfRGB565 || pixelFormatId == PixelFormats.pfRGB555 || pixelFormatId == PixelFormats.pfRGB24 || pixelFormatId == PixelFormats.pfPAL8;

    // ImageLib.pas:3090
    // The sprites take their transparent color from TGrabInfo (declared Reserved in the core),
    // not from TransparentColor (+4, the color GetSurfaceRectangles separates the images with).
    // GrabImage encodes the rows into FData, which grows in 4 KB steps. FDataSize stays 0 until
    // the end, so every word added past the first step reallocates (an original inefficiency)
    public override bool GrabImage(in LockedSurface lockedSurface, in Rect R, GrabInfo grabInfo)
    {
        int position;
        int rowStart;
        int skip;
        DataSurface surface;
        LockedSurface lSD = default;
        Rect gr = default;
        Rect mr = default;
        int C;
        void AddWord(ushort W)
        {
            if (_dataSize <= position)
            {
                _data = MemorySurfaces.Reallocate(_data, position + 4096);
            }

            BinaryPrimitives.WriteUInt16LittleEndian(_data.AsSpan(position, 2), W);
            position += 2;
        }

        void StartRow()
        {
            rowStart = position;
            AddWord(0);
            AddWord(0);
        }

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
            if (_dataSize <= position)
            {
                _data = MemorySurfaces.Reallocate(_data, position + 4096);
            }

            BinaryPrimitives.WriteUInt16LittleEndian(_data.AsSpan(rowStart, 2), (ushort)(position - rowStart));

            if ((position & 3) != 0)
            {
                AddWord((ushort)_colorKey);
            }
        }

        bool result = true;
        try
        {
            surface = null;
            try
            {
                if (lockedSurface.BitCount != 16)
                {
                    surface = new DataSurface(R.Right - R.Left, R.Bottom - R.Top, PixelFormats.pfRGB565, null);
                    gr.Left = 0;
                    gr.Top = 0;
                    var width = surface.GetWidth();
                    gr.Right = width;
                    var height = surface.GetHeight();
                    gr.Bottom = height;
                    surface.Lock(ref lSD);
                    var surfaceWidth = surface.GetWidth();
                    var surfaceHeight = surface.GetHeight();
                    if (LockedSurface.RemapSurfacePart(lockedSurface, R.Left, R.Top, surfaceWidth, surfaceHeight, lSD, 0, 0) != 0)
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
                _dataWidth = gr.Right - gr.Left;
                _dataHeight = gr.Bottom - gr.Top;
                _width = _dataWidth;
                _height = _dataHeight;
                _fileName = ExtractFileName(grabInfo.FileName);
                _fileIndex = grabInfo.FileIndex;
                _storedPixelFormatId = lSD.PixelFormatId;
                PixelFormatId = _storedPixelFormatId;
                position = 0;
                skip = 0;
                LockedSurface.MinimizeTransparentRectangle(lSD, gr, _colorKey, out mr);
                _spriteX = mr.Left - gr.Left;
                _spriteY = mr.Top - gr.Top;
                _spriteWidth = mr.Right - mr.Left;
                _spriteHeight = mr.Bottom - mr.Top;
                for (int Y = mr.Top, last = mr.Bottom - 1; Y <= last; Y++)
                {
                    int row = lSD.Pitch * Y;
                    StartRow();
                    for (int X = mr.Left, lastX = mr.Right - 1; X <= lastX; X++)
                    {
                        C = lSD.View().UInt16(row + X + X);
                        if (C != _colorKey)
                        {
                            FlushSkip();
                            AddWord((ushort)C);
                        }
                        else
                        {
                            skip += 1;
                        }
                    }

                    EndRow();
                }

                _data = MemorySurfaces.Reallocate(_data, position);
                _dataSize = position;
            }
            finally
            {
                if (surface != null)
                {
                    surface.Dispose();
                }
            }
        }
        catch
        {
            ReleaseDiscardableImageData();
            result = false;
        }

        return result;
    }

    private static void RemapRle565To555(Span<byte> data, int height, int colorKey)
    {
        Blit.RemapRle565To555(data, height, colorKey);
    }

    private static void RemapRle555To565(Span<byte> data, int height, int colorKey)
    {
        Blit.RemapRle555To565(data, height, colorKey);
    }

    private static void BlitRle16(int X, int Y, in LockedSurface lockedSurface, ReadOnlySpan<byte> data, int width, int height, int colorKey)
    {
        Blit.BlitRle16(X, Y, lockedSurface.View(), data, width, height, colorKey);
    }

    private static void BlendRle565(int X, int Y, in LockedSurface lockedSurface, ReadOnlySpan<byte> data, int width, int height, int colorKey)
    {
        Blit.BlendRle565(X, Y, lockedSurface.View(), data, width, height, colorKey);
    }

    private static void BlendRle555(int X, int Y, in LockedSurface lockedSurface, ReadOnlySpan<byte> data, int width, int height, int colorKey)
    {
        Blit.BlendRle555(X, Y, lockedSurface.View(), data, width, height, colorKey);
    }

}
