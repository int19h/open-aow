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
    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new Image08(owner);

    public Image08(CustomImageLibrary owner) : base(owner)
    { }

    private bool _ownPalette;
    private LibraryImagePalette _imagePalette;
    internal Palette _remapPalette;

    // ImageLib.pas:7152
    public override void Clear()
    {
        UnregisterPalette();
        _imagePalette?.Dispose();
        _imagePalette = null;
        base.Clear();
    }

    // ImageLib.pas:7160
    public override void ReadImageData(Stream stream)
    {
        int index = default;
        base.ReadImageData(stream);
        stream.ReadExactly(Bytes.Of(ref _ownPalette, 1));
        if (_ownPalette)
        {
            _imagePalette = new LibraryImagePalette();
            _imagePalette.LoadFromStream(stream);
        }
        else
        {
            stream.ReadExactly(Bytes.Of(ref index, 4));
            _imagePalette = Owner.PaletteManager.LinkToPalette(index);
        }

        _remapPalette = _imagePalette.Palette;
    }

    // ImageLib.pas:7179
    public override void WriteImageData(Stream stream)
    {
        base.WriteImageData(stream);
        stream.Write(Bytes.Of(ref _ownPalette, 1));
        if (_ownPalette)
        {
            _imagePalette.SaveToStream(stream);
        }
        else
        {
            stream.Write(Bytes.Of(ref _imagePalette._index, 4));
        }
    }

    // ImageLib.pas:7189
    public virtual void RegisterPalette()
    {
        LibraryImagePalette old;
        if (_ownPalette && _imagePalette != null && Owner != null)
        {
            old = _imagePalette;
            _imagePalette = Owner.PaletteManager.RegisterPalette(_imagePalette.Palette);
            old?.Dispose();
            _remapPalette = _imagePalette.Palette;
            _ownPalette = false;
        }
    }

    // ImageLib.pas:7203
    public virtual void UnregisterPalette()
    {
        LibraryImagePalette P;
        if (!_ownPalette && _imagePalette != null)
        {
            P = new LibraryImagePalette();
            P.Assign(_imagePalette);
            Owner.PaletteManager.UnregisterPalette(_imagePalette._index);
            _imagePalette = P;
            _remapPalette = _imagePalette.Palette;
            _ownPalette = true;
        }
    }

    // ImageLib.pas:7218
    public void CreateLibraryImagePalette(Palette palette)
    {
        UnregisterPalette();
        if (_imagePalette == null)
        {
            _imagePalette = new LibraryImagePalette();
        }

        _imagePalette.Palette.Assign(palette);
        _remapPalette = _imagePalette.Palette;
        _ownPalette = true;
    }

    // ImageLib.pas:7227
    public override void SetOwner(CustomImageLibrary owner)
    {
        if (owner != Owner)
        {
            UnregisterPalette();
            base.SetOwner(owner);
            RegisterPalette();
        }
    }

    // ImageLib.pas:7237
    public override int RemapToPixelFormat(int pixelFormatId)
    {
        int result;
        Palette P;
        P = _imagePalette.RemapPalette(pixelFormatId);
        if (P != null)
        {
            _remapPalette = P;
            result = PixelFormats.GFXE_OK;
        }
        else
        {
            result = PixelFormats.GFXE_NOTSUPPORTED;
        }

        return result;
    }

    // ImageLib.pas:7251
    public override bool SupportGrabPixelFormat(int pixelFormatId) => pixelFormatId == PixelFormats.pfPAL8;

    // ImageLib.pas:7258
    // { ShowDC remaps the palette to pfPAL8 (an original slip: the surface is pfRGB565; ShowOpaque
    //   remaps the palette again when it differs). }
    public override void ShowDisplayController(Rect? sourceRectangle, Rect? destinationRectangle, int displayController, int bkColor)
    {
        /* TODO: Implement Windows.WindowFromDC in the platform or editor service. */
        throw new NotImplementedException();
    }
}
