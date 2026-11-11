// Translated from ImageLib.pas.
using System.Buffers.Binary;

using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// a mask of one bit per pixel (ClassID $13), shown as white pixels. FPitch (bytes per row,
// whole dwords), FMaskData (rows of dwords, the first pixel in the high bit), FMaskLeft,
// FMaskTop, FMaskWidth, FMaskHeight (the rectangle of the mask in the data rectangle)
public partial class BitMask : LibraryImage, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 19;
    static string ILibraryImageClass.Name => "Bit Mask";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new BitMask(owner);

    public BitMask(CustomImageLibrary owner) : base(owner)
    { }

    internal int FPitch;
    internal byte[] FMaskData;
    internal int FMaskLeft;
    internal int FMaskTop;
    internal int FMaskWidth;
    internal int FMaskHeight;

    // ImageLib.pas:4662
    public override void SetXshift(int Value)
    {
        if (Value < -FMaskLeft)
        {
            Value = -FMaskLeft;
        }

        base.SetXshift(Value);
        SetWidth(FWidth);
    }

    // ImageLib.pas:4669
    public override void SetYshift(int Value)
    {
        if (Value < -FMaskTop)
        {
            Value = -FMaskTop;
        }

        base.SetYshift(Value);
        SetHeight(FHeight);
    }

    // ImageLib.pas:4676
    public override void SetWidth(int Value)
    {
        int W;
        W = FXshift + FMaskLeft + FMaskWidth;
        if (Value < W)
        {
            Value = W;
        }

        base.SetWidth(Value);
    }

    // ImageLib.pas:4685
    public override void SetHeight(int Value)
    {
        int H;
        H = FYshift + FMaskTop + FMaskHeight;
        if (Value < H)
        {
            Value = H;
        }

        base.SetHeight(Value);
    }

    // ImageLib.pas:4694
    public override void ReadImageData(Stream Stream)
    {
        base.ReadImageData(Stream);
        Stream.ReadExactly(Bytes.Of(ref FMaskWidth, 4));
        Stream.ReadExactly(Bytes.Of(ref FMaskHeight, 4));
        Stream.ReadExactly(Bytes.Of(ref FMaskLeft, 4));
        Stream.ReadExactly(Bytes.Of(ref FMaskTop, 4));
    }

    // ImageLib.pas:4703
    public override void WriteImageData(Stream Stream)
    {
        base.WriteImageData(Stream);
        Stream.Write(Bytes.Of(ref FMaskWidth, 4));
        Stream.Write(Bytes.Of(ref FMaskHeight, 4));
        Stream.Write(Bytes.Of(ref FMaskLeft, 4));
        Stream.Write(Bytes.Of(ref FMaskTop, 4));
    }

    // ImageLib.pas:4721
    public override void ReleaseDiscardableImageData()
    {
        FMaskData = null;
        FDataLoaded = false;
    }

    // ImageLib.pas:4727
    public override void ReadDiscardableImageData(Stream Stream)
    {
        base.ReadDiscardableImageData(Stream);
        AllocateMaskData(FMaskWidth, FMaskHeight);
        Stream.ReadExactly(FMaskData.AsSpan(0, FDataSize));
    }

    // ImageLib.pas:4734
    public override void WriteDiscardableImageData(Stream Stream)
    {
        base.WriteDiscardableImageData(Stream);
        Stream.Write(FMaskData.AsSpan(0, FDataSize));
    }

    // ImageLib.pas:4740
    public override bool SupportGrabPixelFormat(int PixelFormatID) => PixelFormatID == PixelFormats.pfRGB565 || PixelFormatID == PixelFormats.pfRGB555 || PixelFormatID == PixelFormats.pfRGB24 || PixelFormatID == PixelFormats.pfPAL8;

    // ImageLib.pas:4746
    public override Images.GrabInfo CreateGrabInfo() => new GrabInfo();

    // ImageLib.pas:4751
    public override Form CreateGrabForm(Component AOwner, in SurfaceDesc SD, ImagesGrabbedEvent OnImagesGrabbed) => null;

    // ImageLib.pas:4759
    // { GrabImage sets a bit for every pixel that differs from the transparent pixel value. With an
    //   inverse grab info no bit is added at all (an original bug: the mask stays empty). }
    public override bool GrabImage(in SurfaceDesc SD, in Rect R, Images.GrabInfo GrabInfo)
    {
        int Bits;
        int BitCount;
        int Offset;
        int TransparentColor;
        int Left;
        int Top;
        int Bottom;
        Rect MinRect = default;
        void AddBit(bool Bit)
        {
            Bits = Bits << 1;
            if (Bit)
            {
                Bits += 1;
            }

            BitCount += 1;
            if (BitCount == 32)
            {
                BinaryPrimitives.WriteInt32LittleEndian(FMaskData.AsSpan(Offset, 4), Bits);

                Offset += 4;
                BitCount = 0;
                Bits = 0;
            }
        }

        void FlushBits()
        {
            Bits = Bits << (32 - BitCount);
            BinaryPrimitives.WriteInt32LittleEndian(FMaskData.AsSpan(Offset, 4), Bits);

            Offset += 4;
            BitCount = 0;
            Bits = 0;
        }

        Clear();
        bool Result = true;
        FDataLoaded = true;
        Bits = 0;
        Offset = 0;
        BitCount = 0;
        FDataWidth = R.Right - R.Left;
        FDataHeight = R.Bottom - R.Top;
        FWidth = FDataWidth;
        FHeight = FDataHeight;
        FFileName = ExtractFileName(GrabInfo.FileName);
        FFileIndex = GrabInfo.FileIndex;
        TransparentColor = GrabInfo.TransparentPixel;
        if (!((GrabInfo)GrabInfo).Inverse)
        {
            SurfaceDesc.MinimizeTransparentRect(SD, R, TransparentColor, ref MinRect);
        }
        else
        {
            MinimizeInvTransparentRect(SD, R, TransparentColor, ref MinRect);
        }

        Left = MinRect.Left;
        FMaskLeft = Left - R.Left;
        Top = MinRect.Top;
        FMaskTop = Top - R.Top;
        FMaskWidth = MinRect.Right - Left;
        Bottom = MinRect.Bottom;
        FMaskHeight = Bottom - Top;
        AllocateMaskData(FMaskWidth, FMaskHeight);
        for (int Y = Top; Y <= Bottom - 1; Y++)
        {
            Offset = (Y - MinRect.Top) * FPitch;
            for (int X = MinRect.Left, last = MinRect.Right - 1; X <= last; X++)
            {
                if (!((GrabInfo)GrabInfo).Inverse)
                {
                    AddBit(GetPixelInfo(SD, X, Y) != TransparentColor);
                }
            }

            FlushBits();
        }

        return Result;
    }

    // ImageLib.pas:4827
    // { ShowDC clears the surface to gray, not to BkColor. }
    public override void ShowDC(Rect? SrcRect, Rect? DestRect, int DC, int BkColor)
    {
        /* TODO: Implement Windows.WindowFromDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:4861
    public override void ShowOpaque(int X, int Y, in SurfaceDesc SD)
    {
        if (!FDataLoaded)
        {
            LoadDiscardableImageData();
        }

        ShowBitMask16(X + FXshift + FMaskLeft, Y + FYshift + FMaskTop, SD, FMaskData, FMaskWidth, FMaskHeight);
    }

    public void AllocateMaskData(int AWidth, int AHeight)
    {
        FPitch = checked(((AWidth + 31) >> 5) * 4);
        FDataSize = checked(FPitch * FMaskHeight);
        // GrabImage flushes one extra zero dword after a full final word.
        FMaskData = MemorySurfaces.Reallocate(FMaskData, checked(FDataSize + 4));
    }

    internal static int GetPixelInfo(in SurfaceDesc SD, int X, int Y)
    {
        SurfaceView val;
        switch (SD.BitCount)
        {
            case 8:
                val = SD.View();
                return val.Byte(X + SD.Pitch * Y);
            case 16:
                val = SD.View();
                return val.Word(X + X + SD.Pitch * Y);
            case 24:
                val = SD.View();
                return (int)(val.Dword(X * 3 + SD.Pitch * Y) & 0xFFFFFF);
            case 32:
                val = SD.View();
                return (int)val.Dword(X * 4 + SD.Pitch * Y);
            default:
                throw new NotSupportedException("PixelFormat is not supported");
        }
    }

    internal static void MinimizeInvTransparentRect(in SurfaceDesc SD, in Rect R, int TransparentColor, ref Rect Result)
    {
        SurfaceDesc SD__value = SD;
        Rect val = R;
        bool flag = false;
        while (!flag && val.Top < val.Bottom)
        {
            int num = val.Left;
            while (!flag && num != val.Right)
            {
                flag = GetPixel(num, val.Top) == TransparentColor;
                num++;
            }
            if (!flag)
            {
                val.Top++;
            }
        }
        flag = false;
        while (!flag && val.Bottom > val.Top)
        {
            int num = val.Left;
            while (!flag && num != val.Right)
            {
                flag = GetPixel(num, val.Bottom - 1) == TransparentColor;
                num++;
            }
            if (!flag)
            {
                val.Bottom--;
            }
        }
        flag = false;
        while (!flag && val.Left < val.Right)
        {
            int num2 = val.Top;
            while (!flag && num2 != val.Bottom)
            {
                flag = GetPixel(val.Left, num2) == TransparentColor;
                num2++;
            }
            if (!flag)
            {
                val.Left++;
            }
        }
        flag = false;
        while (!flag && val.Right > val.Left)
        {
            int num2 = val.Top;
            while (!flag && num2 != val.Bottom)
            {
                flag = GetPixel(val.Right - 1, num2) == TransparentColor;
                num2++;
            }
            if (!flag)
            {
                val.Right--;
            }
        }
        Result = val;
        int GetPixel(int X, int Y)
        {
            SurfaceView val2;
            switch (SD__value.BitCount)
            {
                case 8:
                    val2 = SD__value.View();
                    return val2.Byte(X + SD__value.Pitch * Y);
                case 16:
                    val2 = SD__value.View();
                    return val2.Word(X + X + SD__value.Pitch * Y);
                case 24:
                    val2 = SD__value.View();
                    return (int)(val2.Dword(X * 3 + SD__value.Pitch * Y) & 0xFFFFFF);
                case 32:
                    val2 = SD__value.View();
                    return (int)val2.Dword(X * 4 + SD__value.Pitch * Y);
                default:
                    throw new NotSupportedException("PixelFormat is not supported");
            }
        }
    }

    private static void ShowBitMask16(int X, int Y, in SurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height)
    {
        Blit.ShowBitMask16(X, Y, SD.View(), Data, Width, Height);
    }

    // Inverse (grab the non-transparent rectangle with MinimizeInvTransparentRect)
    public partial class GrabInfo : Images.GrabInfo
    {
        public static new GrabInfoClass Descriptor { get; } = new("BitMask.GrabInfo", static () => new GrabInfo());

        public bool Inverse;

        // ImageLib.pas:4638
        public override void LoadFromStream(Stream Stream)
        {
            base.LoadFromStream(Stream);
            Stream.ReadExactly(Bytes.Of(ref Inverse, 1));
        }

        // ImageLib.pas:4644
        public override void SaveToStream(Stream Stream)
        {
            base.SaveToStream(Stream);
            Stream.Write(Bytes.Of(ref Inverse, 1));
        }

        // ImageLib.pas:4633
        public override GrabInfoClass GrabInfoClass() => Descriptor;
    }
}
