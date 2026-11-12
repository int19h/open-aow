// Translated from ImageLib.pas.

namespace Aow.Game.Draw.Images;

// (ClassID $12): a TRLESprite16 shown transparent by default. it cannot be grabbed
public partial class TransparentRleSprite16 : RleSprite16, ILibraryImageClass
{
    static int ILibraryImageClass.ClassId => 18;
    static string ILibraryImageClass.Name => "Transparent RLE Sprite (16bpp)";

    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new TransparentRleSprite16(owner);

    // ImageLib.pas:3592
    public TransparentRleSprite16(CustomImageLibrary owner) : base(owner)
    {
        _showInfo.ShowMode = ShowMode.Transparent;
    }

    // ImageLib.pas:3608
    public override bool SupportGrabPixelFormat(int pixelFormatId) => false;

}
