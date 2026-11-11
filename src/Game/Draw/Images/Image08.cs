// Translated from ImageLib.pas.
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

// the base of the 8 bit images. FOwnPalette (the image owns FImagePalette. otherwise it is
// shared through the palette manager of the library), FImagePalette, FRemapPalette (the palette
// the pixels are shown with: the image palette, or the copy remapped to the pixel format of the
// last surface). RegisterPalette (share the palette through the owner's palette manager) and
// UnregisterPalette (make a private copy again)
public partial class Image08 : LibraryImage, ILibraryImageClass
{
    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new Image08(owner);

    public Image08(CustomImageLibrary owner) : base(owner)
    { }

    internal bool FOwnPalette;
    internal LibraryImagePalette FImagePalette;
    internal GFXEPalette FRemapPalette;

    // ImageLib.pas:7152
    public override void Clear()
    {
        UnregisterPalette();
        FImagePalette?.Dispose();
        FImagePalette = null;
        base.Clear();
    }

    // ImageLib.pas:7160
    public override void ReadImageData(Stream Stream)
    {
        int Index = default;
        base.ReadImageData(Stream);
        Stream.ReadExactly(Bytes.Of(ref FOwnPalette, 1));
        if (FOwnPalette)
        {
            FImagePalette = new LibraryImagePalette();
            FImagePalette.LoadFromStream(Stream);
        }
        else
        {
            Stream.ReadExactly(Bytes.Of(ref Index, 4));
            FImagePalette = FOwner.FPaletteManager.LinkToPalette(Index);
        }

        FRemapPalette = FImagePalette.FPalette;
    }

    // ImageLib.pas:7179
    public override void WriteImageData(Stream Stream)
    {
        base.WriteImageData(Stream);
        Stream.Write(Bytes.Of(ref FOwnPalette, 1));
        if (FOwnPalette)
        {
            FImagePalette.SaveToStream(Stream);
        }
        else
        {
            Stream.Write(Bytes.Of(ref FImagePalette.FIndex, 4));
        }
    }

    // ImageLib.pas:7189
    public virtual void RegisterPalette()
    {
        LibraryImagePalette Old;
        if (FOwnPalette && FImagePalette != null && FOwner != null)
        {
            Old = FImagePalette;
            FImagePalette = FOwner.FPaletteManager.RegisterPalette(FImagePalette.FPalette);
            Old?.Dispose();
            FRemapPalette = FImagePalette.FPalette;
            FOwnPalette = false;
        }
    }

    // ImageLib.pas:7203
    public virtual void UnregisterPalette()
    {
        LibraryImagePalette P;
        if (!FOwnPalette && FImagePalette != null)
        {
            P = new LibraryImagePalette();
            P.Assign(FImagePalette);
            FOwner.FPaletteManager.UnregisterPalette(FImagePalette.FIndex);
            FImagePalette = P;
            FRemapPalette = FImagePalette.FPalette;
            FOwnPalette = true;
        }
    }

    // ImageLib.pas:7218
    public void CreateLibraryImagePalette(GFXEPalette Palette)
    {
        UnregisterPalette();
        if (FImagePalette == null)
        {
            FImagePalette = new LibraryImagePalette();
        }

        FImagePalette.FPalette.Assign(Palette);
        FRemapPalette = FImagePalette.FPalette;
        FOwnPalette = true;
    }

    // ImageLib.pas:7227
    public override void SetOwner(CustomImageLibrary AOwner)
    {
        if (AOwner != FOwner)
        {
            UnregisterPalette();
            base.SetOwner(AOwner);
            RegisterPalette();
        }
    }

    // ImageLib.pas:7237
    public override int RemapToPixelFormat(int PixelFormatID)
    {
        int Result;
        GFXEPalette P;
        P = FImagePalette.RemapPalette(PixelFormatID);
        if (P != null)
        {
            FRemapPalette = P;
            Result = PixelFormats.GFXE_OK;
        }
        else
        {
            Result = PixelFormats.GFXE_NOTSUPPORTED;
        }

        return Result;
    }

    // ImageLib.pas:7251
    public override bool SupportGrabPixelFormat(int PixelFormatID) => PixelFormatID == PixelFormats.pfPAL8;

    // ImageLib.pas:7258
    // { ShowDC remaps the palette to pfPAL8 (an original slip: the surface is pfRGB565; ShowOpaque
    //   remaps the palette again when it differs). }
    public override void ShowDC(Rect? SrcRect, Rect? DestRect, int DC, int BkColor)
    {
        /* TODO: Implement Windows.WindowFromDC in the platform or editor service. */
        throw new NotImplementedException();
    }
}
