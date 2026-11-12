// Translated from ImageLib.pas.
using System.Buffers.Binary;

using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// a mask of one bit per pixel (ClassID $13), shown as white pixels. _pitch (bytes per row,
// whole dwords), FMaskData (rows of dwords, the first pixel in the high bit), FMaskLeft,
// FMaskTop, FMaskWidth, FMaskHeight (the rectangle of the mask in the data rectangle)
public partial class BitMask : LibraryImage, ILibraryImageClass
{
    static int ILibraryImageClass.ClassId => 19;
    static string ILibraryImageClass.Name => "Bit Mask";

    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new BitMask(owner);

    public BitMask(CustomImageLibrary owner) : base(owner)
    { }

    private int _pitch;

    internal byte[] MaskData { get; private set; }

    internal int _maskLeft;
    internal int _maskTop;
    internal int _maskWidth;
    internal int _maskHeight;

    // ImageLib.pas:4662
    public override void SetXshift(int value)
    {
        if (value < -_maskLeft)
        {
            value = -_maskLeft;
        }

        base.SetXshift(value);
        SetWidth(_width);
    }

    // ImageLib.pas:4669
    public override void SetYshift(int value)
    {
        if (value < -_maskTop)
        {
            value = -_maskTop;
        }

        base.SetYshift(value);
        SetHeight(_height);
    }

    // ImageLib.pas:4676
    public override void SetWidth(int value)
    {
        int W;
        W = _xshift + _maskLeft + _maskWidth;
        if (value < W)
        {
            value = W;
        }

        base.SetWidth(value);
    }

    // ImageLib.pas:4685
    public override void SetHeight(int value)
    {
        int H;
        H = _yshift + _maskTop + _maskHeight;
        if (value < H)
        {
            value = H;
        }

        base.SetHeight(value);
    }

    // ImageLib.pas:4694
    public override void ReadImageData(Stream stream)
    {
        base.ReadImageData(stream);
        stream.ReadExactly(Bytes.Of(ref _maskWidth, 4));
        stream.ReadExactly(Bytes.Of(ref _maskHeight, 4));
        stream.ReadExactly(Bytes.Of(ref _maskLeft, 4));
        stream.ReadExactly(Bytes.Of(ref _maskTop, 4));
    }

    // ImageLib.pas:4703
    public override void WriteImageData(Stream stream)
    {
        base.WriteImageData(stream);
        stream.Write(Bytes.Of(ref _maskWidth, 4));
        stream.Write(Bytes.Of(ref _maskHeight, 4));
        stream.Write(Bytes.Of(ref _maskLeft, 4));
        stream.Write(Bytes.Of(ref _maskTop, 4));
    }

    // ImageLib.pas:4721
    public override void ReleaseDiscardableImageData()
    {
        MaskData = null;
        DataLoaded = false;
    }

    // ImageLib.pas:4727
    public override void ReadDiscardableImageData(Stream stream)
    {
        base.ReadDiscardableImageData(stream);
        AllocateMaskData(_maskWidth, _maskHeight);
        stream.ReadExactly(MaskData.AsSpan(0, _dataSize));
    }

    // ImageLib.pas:4734
    public override void WriteDiscardableImageData(Stream stream)
    {
        base.WriteDiscardableImageData(stream);
        stream.Write(MaskData.AsSpan(0, _dataSize));
    }

    // ImageLib.pas:4740
    public override bool SupportGrabPixelFormat(int pixelFormatId) => pixelFormatId == PixelFormats.pfRGB565 || pixelFormatId == PixelFormats.pfRGB555 || pixelFormatId == PixelFormats.pfRGB24 || pixelFormatId == PixelFormats.pfPAL8;

    // ImageLib.pas:4746
    public override Images.GrabInfo CreateGrabInfo() => new GrabInfo();

    // ImageLib.pas:4751
    public override Form CreateGrabForm(Component owner, in LockedSurface lockedSurface, ImagesGrabbedEvent onImagesGrabbed) => null;

    // ImageLib.pas:4759
    // { GrabImage sets a bit for every pixel that differs from the transparent pixel value. With an
    //   inverse grab info no bit is added at all (an original bug: the mask stays empty). }
    public override bool GrabImage(in LockedSurface lockedSurface, in Rect R, Images.GrabInfo grabInfo)
    {
        int bits;
        int bitCount;
        int offset;
        int transparentColor;
        int left;
        int top;
        int bottom;
        Rect minRectangle = default;
        void AddBit(bool bit)
        {
            bits = bits << 1;
            if (bit)
            {
                bits += 1;
            }

            bitCount += 1;
            if (bitCount == 32)
            {
                BinaryPrimitives.WriteInt32LittleEndian(MaskData.AsSpan(offset, 4), bits);

                offset += 4;
                bitCount = 0;
                bits = 0;
            }
        }

        void FlushBits()
        {
            bits = bits << (32 - bitCount);
            BinaryPrimitives.WriteInt32LittleEndian(MaskData.AsSpan(offset, 4), bits);

            offset += 4;
            bitCount = 0;
            bits = 0;
        }

        Clear();
        bool result = true;
        DataLoaded = true;
        bits = 0;
        offset = 0;
        bitCount = 0;
        _dataWidth = R.Right - R.Left;
        _dataHeight = R.Bottom - R.Top;
        _width = _dataWidth;
        _height = _dataHeight;
        _fileName = ExtractFileName(grabInfo.FileName);
        _fileIndex = grabInfo.FileIndex;
        transparentColor = grabInfo.TransparentPixel;
        if (!((GrabInfo)grabInfo).Inverse)
        {
            LockedSurface.MinimizeTransparentRectangle(lockedSurface, R, transparentColor, ref minRectangle);
        }
        else
        {
            MinimizeInverseTransparentRectangle(lockedSurface, R, transparentColor, ref minRectangle);
        }

        left = minRectangle.Left;
        _maskLeft = left - R.Left;
        top = minRectangle.Top;
        _maskTop = top - R.Top;
        _maskWidth = minRectangle.Right - left;
        bottom = minRectangle.Bottom;
        _maskHeight = bottom - top;
        AllocateMaskData(_maskWidth, _maskHeight);
        for (int Y = top; Y <= bottom - 1; Y++)
        {
            offset = (Y - minRectangle.Top) * _pitch;
            for (int X = minRectangle.Left, last = minRectangle.Right - 1; X <= last; X++)
            {
                if (!((GrabInfo)grabInfo).Inverse)
                {
                    AddBit(GetPixelInfo(lockedSurface, X, Y) != transparentColor);
                }
            }

            FlushBits();
        }

        return result;
    }

    // ImageLib.pas:4827
    // { ShowDC clears the surface to gray, not to BkColor. }
    public override void ShowDisplayController(Rect? sourceRectangle, Rect? destinationRectangle, int displayController, int bkColor)
    {
        /* TODO: Implement Windows.WindowFromDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:4861
    public override void ShowOpaque(int X, int Y, in LockedSurface lockedSurface)
    {
        if (!DataLoaded)
        {
            LoadDiscardableImageData();
        }

        ShowBitMask16(X + _xshift + _maskLeft, Y + _yshift + _maskTop, lockedSurface, MaskData, _maskWidth, _maskHeight);
    }

    public void AllocateMaskData(int width, int height)
    {
        _pitch = checked(((width + 31) >> 5) * 4);
        _dataSize = checked(_pitch * _maskHeight);
        // GrabImage flushes one extra zero dword after a full final word.
        MaskData = MemorySurfaces.Reallocate(MaskData, checked(_dataSize + 4));
    }

    internal static int GetPixelInfo(in LockedSurface lockedSurface, int X, int Y)
    {
        SurfaceView value;
        switch (lockedSurface.BitCount)
        {
            case 8:
                value = lockedSurface.View();
                return value.Byte(X + lockedSurface.Pitch * Y);
            case 16:
                value = lockedSurface.View();
                return value.UInt16(X + X + lockedSurface.Pitch * Y);
            case 24:
                value = lockedSurface.View();
                return (int)(value.Dword(X * 3 + lockedSurface.Pitch * Y) & 0xFFFFFF);
            case 32:
                value = lockedSurface.View();
                return (int)value.Dword(X * 4 + lockedSurface.Pitch * Y);
            default:
                throw new NotSupportedException("PixelFormat is not supported");
        }
    }

    internal static void MinimizeInverseTransparentRectangle(in LockedSurface lockedSurface, in Rect R, int transparentColor, ref Rect result)
    {
        LockedSurface lockedSurfaceValue = lockedSurface;
        Rect value = R;
        bool flag = false;
        while (!flag && value.Top < value.Bottom)
        {
            int number = value.Left;
            while (!flag && number != value.Right)
            {
                flag = GetPixel(number, value.Top) == transparentColor;
                number++;
            }
            if (!flag)
            {
                value.Top++;
            }
        }
        flag = false;
        while (!flag && value.Bottom > value.Top)
        {
            int number = value.Left;
            while (!flag && number != value.Right)
            {
                flag = GetPixel(number, value.Bottom - 1) == transparentColor;
                number++;
            }
            if (!flag)
            {
                value.Bottom--;
            }
        }
        flag = false;
        while (!flag && value.Left < value.Right)
        {
            int number2 = value.Top;
            while (!flag && number2 != value.Bottom)
            {
                flag = GetPixel(value.Left, number2) == transparentColor;
                number2++;
            }
            if (!flag)
            {
                value.Left++;
            }
        }
        flag = false;
        while (!flag && value.Right > value.Left)
        {
            int number2 = value.Top;
            while (!flag && number2 != value.Bottom)
            {
                flag = GetPixel(value.Right - 1, number2) == transparentColor;
                number2++;
            }
            if (!flag)
            {
                value.Right--;
            }
        }
        result = value;
        int GetPixel(int X, int Y)
        {
            SurfaceView value2;
            switch (lockedSurfaceValue.BitCount)
            {
                case 8:
                    value2 = lockedSurfaceValue.View();
                    return value2.Byte(X + lockedSurfaceValue.Pitch * Y);
                case 16:
                    value2 = lockedSurfaceValue.View();
                    return value2.UInt16(X + X + lockedSurfaceValue.Pitch * Y);
                case 24:
                    value2 = lockedSurfaceValue.View();
                    return (int)(value2.Dword(X * 3 + lockedSurfaceValue.Pitch * Y) & 0xFFFFFF);
                case 32:
                    value2 = lockedSurfaceValue.View();
                    return (int)value2.Dword(X * 4 + lockedSurfaceValue.Pitch * Y);
                default:
                    throw new NotSupportedException("PixelFormat is not supported");
            }
        }
    }

    private static void ShowBitMask16(int X, int Y, in LockedSurface lockedSurface, ReadOnlySpan<byte> data, int width, int height)
    {
        Blit.ShowBitMask16(X, Y, lockedSurface.View(), data, width, height);
    }

    // Inverse (grab the non-transparent rectangle with MinimizeInvTransparentRect)
    public partial class GrabInfo : Images.GrabInfo
    {
        public static new GrabInfoClass Descriptor { get; } = new("BitMask.GrabInfo", static () => new GrabInfo());

        public bool Inverse;

        // ImageLib.pas:4638
        public override void LoadFromStream(Stream stream)
        {
            base.LoadFromStream(stream);
            stream.ReadExactly(Bytes.Of(ref Inverse, 1));
        }

        // ImageLib.pas:4644
        public override void SaveToStream(Stream stream)
        {
            base.SaveToStream(stream);
            stream.Write(Bytes.Of(ref Inverse, 1));
        }

        // ImageLib.pas:4633
        public override GrabInfoClass GrabInfoClass() => Descriptor;
    }
}
