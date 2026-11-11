// Translated from ImageLib.pas.
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

// the base of the 16 bit images (pfRGB565 or pfRGB555). FPixelFormatID (of the pixels in
// memory), FStoredPixelFormatID (of the pixels in the library file: WriteImageData remaps the
// pixels back to it before they are written)
public partial class Image16 : LibraryImage, ILibraryImageClass
{
    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new Image16(owner);

    internal int FPixelFormatID;
    internal int FStoredPixelFormatID;

    // ImageLib.pas:2098
    public Image16(CustomImageLibrary owner) : base(owner)
    {
    }

    // ImageLib.pas:2103
    public override void ReadImageData(Stream Stream)
    {
        base.ReadImageData(Stream);
        Stream.ReadExactly(Bytes.Of(ref FStoredPixelFormatID, 4));
        FPixelFormatID = FStoredPixelFormatID;
    }

    // ImageLib.pas:2110
    public override void WriteImageData(Stream Stream)
    {
        base.WriteImageData(Stream);
        RemapToPixelFormat(FStoredPixelFormatID);
        Stream.Write(Bytes.Of(ref FStoredPixelFormatID, 4));
    }

    // ImageLib.pas:2117
    public override void ReadDiscardableImageData(Stream Stream)
    {
        base.ReadDiscardableImageData(Stream);
        FPixelFormatID = FStoredPixelFormatID;
    }

    // ImageLib.pas:2123
    public override void WriteDiscardableImageData(Stream Stream)
    {
        base.WriteDiscardableImageData(Stream);
    }

    // ImageLib.pas:2128
    public override void ShowDC(Rect? SrcRect, Rect? DestRect, int DC, int BkColor)
    {
        /* TODO: Implement Windows.WindowFromDC in the platform or editor service. */
        throw new NotImplementedException();
    }

}
