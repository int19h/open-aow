// Translated from ImageLib.pas.

namespace Aow.Game.Draw.Images;

// (ClassID $12): a TRLESprite16 shown transparent by default. it cannot be grabbed
public partial class TransparentRLESprite16 : RLESprite16, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 18;
    static string ILibraryImageClass.Name => "Transparent RLE Sprite (16bpp)";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new TransparentRLESprite16(owner);

    // ImageLib.pas:3592
    public TransparentRLESprite16(CustomImageLibrary owner) : base(owner)
    {
        FShowInfo.ShowMode = ShowMode.Transparent;
    }

    // ImageLib.pas:3608
    public override bool SupportGrabPixelFormat(int PixelFormatID) => false;

}
