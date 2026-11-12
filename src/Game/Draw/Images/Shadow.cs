// Translated from ImageLib.pas.
using Aow.Graphics;

namespace Aow.Game.Draw.Images;

// a bit mask shown as a shadow, darkening the 16 bit surface under the set bits (ClassID $14)
public partial class Shadow : BitMask, ILibraryImageClass
{
    static int ILibraryImageClass.ClassId => 20;
    static string ILibraryImageClass.Name => "Shadow";

    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new Shadow(owner);

    // ImageLib.pas:5100
    public Shadow(CustomImageLibrary owner) : base(owner)
    {
    }

    // ImageLib.pas:5106
    // { SupportShowMode asks TLibraryImage, which supports nothing. }
    public override bool SupportShowMode(ShowMode showMode, int pixelFormatId) => (pixelFormatId == PixelFormats.pfRGB565 || pixelFormatId == PixelFormats.pfRGB555) && base.SupportShowMode(showMode, pixelFormatId);

    // ImageLib.pas:5122
    public override void ShowOpaque(int X, int Y, in LockedSurface lockedSurface)
    {
        if (!DataLoaded)
        {
            LoadDiscardableImageData();
            switch (lockedSurface.PixelFormatId)
            {
                case PixelFormats.pfRGB565:
                    ShowShadow565(X + _xshift + _maskLeft, Y + _yshift + _maskTop, lockedSurface, MaskData, _maskWidth, _maskHeight);
                    break;
                case PixelFormats.pfRGB555:
                    ShowShadow555(X + _xshift + _maskLeft, Y + _yshift + _maskTop, lockedSurface, MaskData, _maskWidth, _maskHeight);
                    break;
                default:
                    throw new NotSupportedException("Surface format not supported");
            }

            if (_loadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            switch (lockedSurface.PixelFormatId)
            {
                case PixelFormats.pfRGB565:
                    ShowShadow565(X + _xshift + _maskLeft, Y + _yshift + _maskTop, lockedSurface, MaskData, _maskWidth, _maskHeight);
                    break;
                case PixelFormats.pfRGB555:
                    ShowShadow555(X + _xshift + _maskLeft, Y + _yshift + _maskTop, lockedSurface, MaskData, _maskWidth, _maskHeight);
                    break;
                default:
                    throw new NotSupportedException("Surface format not supported");
            }
        }
    }

    private static void ShowShadow565(int X, int Y, in LockedSurface lockedSurface, ReadOnlySpan<byte> data, int width, int height)
    {
        Blit.ShowShadow565(X, Y, lockedSurface.View(), data, width, height);
    }

    private static void ShowShadow555(int X, int Y, in LockedSurface lockedSurface, ReadOnlySpan<byte> data, int width, int height)
    {
        Blit.ShowShadow555(X, Y, lockedSurface.View(), data, width, height);
    }

}
