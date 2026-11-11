// Translated from ImageLib.pas.
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

// FIndex, FPalette, FRemapPalette (the palette remapped to a pixel format, made on demand),
// FRefCount
public partial class LibraryImagePalette : Persistent
{
    internal int FIndex;
    internal Palette FPalette;
    internal Palette FRemapPalette;
    internal int FRefCount;

    // ImageLib.pas:7084
    public LibraryImagePalette()
    {
        FPalette = new Palette();
    }

    // ImageLib.pas:7090
    public override void Dispose()
    {
        base.Dispose();
    }

    // ImageLib.pas:7097
    public void SaveToStream(Stream Stream)
    {
        FPalette.SaveToStream(Stream);
    }

    // ImageLib.pas:7102
    public void LoadFromStream(Stream Stream)
    {
        FPalette.LoadFromStream(Stream);
    }

    // ImageLib.pas:7107
    public Palette RemapPalette(int PixelFormatID)
    {
        Palette Result;
        if (PixelFormatID == FPalette.PixelFormatID)
        {
            Result = FPalette;
        }
        else
        {
            PixelFormats.Color2PixelInfo(0, PixelFormatID, FPalette);
            if (FRemapPalette == null || PixelFormatID != FRemapPalette.PixelFormatID)
            {
                if (FRemapPalette == null)
                {
                    FRemapPalette = new Palette();
                }

                FRemapPalette.Assign(FPalette);
                FRemapPalette.Remap(PixelFormatID);
            }

            Result = FRemapPalette;
        }

        return Result;
    }

    // ImageLib.pas:7124
    public override void Assign(Persistent Source)
    {
        if (Source is LibraryImagePalette)
        {
            FPalette.Assign(((LibraryImagePalette)Source).FPalette);
            if (((LibraryImagePalette)Source).FRemapPalette != null)
            {
                if (FRemapPalette == null)
                {
                    FRemapPalette = new Palette();
                }

                FRemapPalette.Assign(((LibraryImagePalette)Source).FRemapPalette);
            }
            else
            {
            }
        }
        else
        {
            throw new FormatException("Can only assign to a TLibraryImagePalette descendant");
        }
    }

    public int Index { get => FIndex; }
    public Palette Palette { get => FPalette; }
    public int RefCount { get => FRefCount; }

}
