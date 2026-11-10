// Translated from ImageLib.pas.
using Aow.Graphics;

namespace Aow.Game.Draw.Images;

// a bit mask shown as a shadow, darkening the 16 bit surface under the set bits (ClassID $14)
public partial class Shadow : BitMask, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 20;
    static string ILibraryImageClass.Name => "Shadow";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new Shadow(owner);

    // ImageLib.pas:5100
    public Shadow(CustomImageLibrary owner) : base(owner)
    {
    }

    // ImageLib.pas:5106
    // { SupportShowMode asks TLibraryImage, which supports nothing. }
    public override bool SupportShowMode(ShowMode ShowMode, int PixelFormatID) => (PixelFormatID == PixelFormats.pfRGB565 || PixelFormatID == PixelFormats.pfRGB555) && base.SupportShowMode(ShowMode, PixelFormatID);

    // ImageLib.pas:5122
    public override void ShowOpaque(int X, int Y, in SurfaceDesc SD)
    {
        if (!FDataLoaded)
        {
            LoadDiscardableImageData();
            switch (SD.PixelFormatID)
            {
                case PixelFormats.pfRGB565:
                    ShowShadow565(X + FXshift + FMaskLeft, Y + FYshift + FMaskTop, SD, FMaskData, FMaskWidth, FMaskHeight);
                    break;
                case PixelFormats.pfRGB555:
                    ShowShadow555(X + FXshift + FMaskLeft, Y + FYshift + FMaskTop, SD, FMaskData, FMaskWidth, FMaskHeight);
                    break;
                default:
                    throw new NotSupportedException("Surface format not supported");
            }

            if (FLoadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            switch (SD.PixelFormatID)
            {
                case PixelFormats.pfRGB565:
                    ShowShadow565(X + FXshift + FMaskLeft, Y + FYshift + FMaskTop, SD, FMaskData, FMaskWidth, FMaskHeight);
                    break;
                case PixelFormats.pfRGB555:
                    ShowShadow555(X + FXshift + FMaskLeft, Y + FYshift + FMaskTop, SD, FMaskData, FMaskWidth, FMaskHeight);
                    break;
                default:
                    throw new NotSupportedException("Surface format not supported");
            }
        }
    }

    private static void ShowShadow565(int X, int Y, in SurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height)
    {
        Blit.ShowShadow565(X, Y, SD.View(), Data, Width, Height);
    }

    private static void ShowShadow555(int X, int Y, in SurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height)
    {
        Blit.ShowShadow555(X, Y, SD.View(), Data, Width, Height);
    }

}
