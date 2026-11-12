// Translated from ImageLib.pas.

namespace Aow.Game.Draw.Images;

// (ClassID $15): a TPicture16 shown transparent by default. it cannot be grabbed
public partial class TransparentPicture16 : Picture16, ILibraryImageClass
{
    static int ILibraryImageClass.ClassId => 21;
    static string ILibraryImageClass.Name => "Transparent Picture (16bpp)";

    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new TransparentPicture16(owner);

    // ImageLib.pas:2845
    public TransparentPicture16(CustomImageLibrary owner) : base(owner)
    {
        _showInfo.ShowMode = ShowMode.Transparent;
    }

    // ImageLib.pas:2861
    public override bool SupportGrabPixelFormat(int pixelFormatId) => false;

}
