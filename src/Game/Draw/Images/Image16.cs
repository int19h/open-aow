// Translated from ImageLib.pas.
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

// the base of the 16 bit images (pfRGB565 or pfRGB555). FPixelFormatID (of the pixels in
// memory), FStoredPixelFormatID (of the pixels in the library file: WriteImageData remaps the
// pixels back to it before they are written)
public partial class Image16 : LibraryImage, ILibraryImageClass
{
    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new Image16(owner);

    internal int _pixelFormatId;
    internal int _storedPixelFormatId;

    // ImageLib.pas:2098
    public Image16(CustomImageLibrary owner) : base(owner)
    {
    }

    // ImageLib.pas:2103
    public override void ReadImageData(Stream stream)
    {
        base.ReadImageData(stream);
        stream.ReadExactly(Bytes.Of(ref _storedPixelFormatId, 4));
        _pixelFormatId = _storedPixelFormatId;
    }

    // ImageLib.pas:2110
    public override void WriteImageData(Stream stream)
    {
        base.WriteImageData(stream);
        RemapToPixelFormat(_storedPixelFormatId);
        stream.Write(Bytes.Of(ref _storedPixelFormatId, 4));
    }

    // ImageLib.pas:2117
    public override void ReadDiscardableImageData(Stream stream)
    {
        base.ReadDiscardableImageData(stream);
        _pixelFormatId = _storedPixelFormatId;
    }

    // ImageLib.pas:2123
    public override void WriteDiscardableImageData(Stream stream)
    {
        base.WriteDiscardableImageData(stream);
    }

    // ImageLib.pas:2128
    public override void ShowDisplayController(Rect? sourceRectangle, Rect? destinationRectangle, int displayController, int bkColor)
    {
        /* TODO: Implement Windows.WindowFromDC in the platform or editor service. */
        throw new NotImplementedException();
    }

}
