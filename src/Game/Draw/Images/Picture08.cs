// Translated from ImageLib.pas.
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// an opaque 8 bit picture (ClassID 1). FPitch (of the rows: a multiple of 4 below 32 pixels, of
// 8 from 32 on), FBuffer (the rows from its first byte. The original's FBits, FBuffer aligned to
// 8 bytes, is FBuffer itself in the port)
public partial class Picture08 : Image08, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 1;
    static string ILibraryImageClass.Name => "Picture (08bpp)";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new Picture08(owner);

    public Picture08(CustomImageLibrary owner) : base(owner)
    { }

    internal int FPitch;
    internal byte[] FBuffer;

    // The rows of the picture: FBuffer from its first byte, FPitch bytes apart.
    internal SurfaceView View => new(FBuffer, 0, FPitch);

    // ImageLib.pas:7305
    public override void SetXshift(int Value)
    {
        if (Value < 0)
        {
            Value = 0;
        }

        base.SetXshift(Value);
        SetWidth(FWidth);
    }

    // ImageLib.pas:7312
    public override void SetYshift(int Value)
    {
        if (Value < 0)
        {
            Value = 0;
        }

        base.SetYshift(Value);
        SetHeight(FHeight);
    }

    // ImageLib.pas:7319
    public override void SetWidth(int Value)
    {
        if (Value < FDataWidth + FXshift)
        {
            Value = FDataWidth + FXshift;
        }

        base.SetWidth(Value);
    }

    // ImageLib.pas:7325
    public override void SetHeight(int Value)
    {
        if (Value < FDataHeight + FYshift)
        {
            Value = FDataHeight + FYshift;
        }

        base.SetHeight(Value);
    }

    // ImageLib.pas:7331
    public override bool SupportShowMode(ShowMode ShowMode, int PixelFormatID) => (PixelFormatID == 0 || PixelFormatID == PixelFormats.pfPAL8 || PixelFormatID == PixelFormats.pfRGB555 || PixelFormatID == PixelFormats.pfRGB565) && ShowMode == ShowMode.Opaque;

    // ImageLib.pas:7337
    public void AllocateDiscardableImageData(int AWidth, int AHeight)
    {
        int Size;
        if (AWidth < 32)
        {
            FPitch = (AWidth + 3) & ~3;
        }
        else
        {
            FPitch = (AWidth + 7) & ~7;
        }

        // The original allocates 7 bytes more than the rows and aligns its pixel pointer to 8 for
        // MMX. The alignment has no effect on pixels, so the rows start at the first byte and the
        // 7 bytes stay after the last row (graphics.md 6.8).
        Size = FPitch * AHeight + 7;
        FBuffer = MemorySurfaces.Reallocate(FBuffer, Size);
    }

    // ImageLib.pas:7350
    public override void ReleaseDiscardableImageData()
    {
        if (FDataLoaded)
        {
            FBuffer = null;
            FDataLoaded = false;
        }
    }

    // ImageLib.pas:7359
    public override void ReadDiscardableImageData(Stream Stream)
    {
        base.ReadDiscardableImageData(Stream);
        AllocateDiscardableImageData(FDataWidth, FDataHeight);
        for (int Y = 0, last = FDataHeight - 1; Y <= last; Y++)
        {
            Stream.ReadExactly(FBuffer.AsSpan(Y * FPitch, FDataWidth));
        }
    }

    // ImageLib.pas:7369
    public override void WriteDiscardableImageData(Stream Stream)
    {
        base.WriteDiscardableImageData(Stream);
        for (int Y = 0, last = FDataHeight - 1; Y <= last; Y++)
        {
            Stream.Write(FBuffer.AsSpan(Y * FPitch, FDataWidth));
        }
    }

    // ImageLib.pas:7378
    public override Form CreateGrabForm(Component AOwner, in SurfaceDesc SD, ImagesGrabbedEvent OnImagesGrabbed) => null;

    // ImageLib.pas:7384
    public override bool SupportGrabPixelFormat(int PixelFormatID) => PixelFormatID == PixelFormats.pfPAL8;

    // ImageLib.pas:7389
    public override bool GrabImage(in SurfaceDesc SD, in Rect R, GrabInfo GrabInfo)
    {
        bool Result;
        try
        {
            Clear();
            Result = true;
            FDataLoaded = true;
            FDataWidth = R.Right - R.Left;
            FDataHeight = R.Bottom - R.Top;
            FWidth = FDataWidth;
            FHeight = FDataHeight;
            FFileName = ExtractFileName(GrabInfo.FileName);
            FFileIndex = GrabInfo.FileIndex;
            FDataSize = FDataWidth * FDataHeight;
            AllocateDiscardableImageData(FWidth, FHeight);
            if (SD.PixelFormatID == PixelFormats.pfPAL8)
            {
                for (int Y = 0, last = FHeight - 1; Y <= last; Y++)
                {
                    SD.View().Bytes((R.Top + Y) * SD.Pitch + R.Left, FDataWidth).CopyTo(FBuffer.AsSpan(FPitch * Y, FDataWidth));
                }

                CreateLibraryImagePalette(SD.Palette);
                if (FOwner != null)
                {
                    RegisterPalette();
                }
            }
            else
            {
                Result = false;
            }
        }
        catch
        {
            ReleaseDiscardableImageData();
            Result = false;
        }

        return Result;
    }

    public override void ShowOpaque(int x, int y, in SurfaceDesc sd)
    {
        int destX = x + FXshift, destY = y + FYshift;
        int right = destX + FDataWidth, bottom = destY + FDataHeight;
        int width = FDataWidth, height = FDataHeight, srcX = 0, srcY = 0;
        // Preserve the source offsets and byte destination offset of ImageLib.pas:7424.
        if (destX < sd.ClipRect.Left)
        {
            srcX = sd.ClipRect.Left - x;
            destX = sd.ClipRect.Left;
            width -= srcX;
        }
        if (right > sd.ClipRect.Right) width -= right - sd.ClipRect.Right;
        if (width <= 0) return;
        if (destY < sd.ClipRect.Top)
        {
            srcY = sd.ClipRect.Top - y;
            destY = sd.ClipRect.Top;
            height -= srcY;
        }
        if (bottom > sd.ClipRect.Bottom) height -= bottom - sd.ClipRect.Bottom;
        if (height <= 0) return;

        bool loadedHere = !FDataLoaded;
        if (loadedHere) LoadDiscardableImageData();
        var source = View.At(srcX + srcY * FPitch);
        var destination = sd.View(destX + destY * sd.Pitch);
        if (sd.PixelFormatID == PixelFormats.pfPAL8)
            BlitRects.BltRect(source, destination, width, height);
        else
        {
            if (sd.PixelFormatID != FRemapPalette.PixelFormatID &&
                RemapToPixelFormat(sd.PixelFormatID) != PixelFormats.GFXE_OK)
                throw new NotSupportedException("Surface pixel format not supported!");
            if (sd.PixelFormatID == PixelFormats.pfRGB555)
                PixelRemap.RemapSurface08to555(source, destination, width, height, FRemapPalette);
            else if (sd.PixelFormatID == PixelFormats.pfRGB565)
                PixelRemap.RemapSurface08to565(source, destination, width, height, FRemapPalette);
        }
        if (loadedHere && FLoadMode == LoadMode.LoadFreeOnDemand) ReleaseDiscardableImageData();
    }
}
