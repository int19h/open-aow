#nullable disable
// Translated from ImageLib.pas.
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

// FIndex, FPalette, FRemapPalette (the palette remapped to a pixel format, made on demand),
// _refCount
public sealed class LibraryImagePalette : Persistent
{
    internal int IndexValue { get; set; }

    internal Palette RemapPaletteValue { get; private set; }

    // ImageLib.pas:7084
    public LibraryImagePalette() => Palette = new Palette();

    // ImageLib.pas:7090
    public override void Dispose() => base.Dispose();

    // ImageLib.pas:7097
    public void SaveToStream(Stream stream) => Palette.SaveToStream(stream);

    // ImageLib.pas:7102
    public void LoadFromStream(Stream stream) => Palette.LoadFromStream(stream);

    // ImageLib.pas:7107
    public Palette RemapPalette(PixelFormat pixelFormatId)
    {
        if (pixelFormatId == Palette.PixelFormatId)
        {
            return Palette;
        }
        else
        {
            _ = PixelFormats.Color2PixelInfo(0, pixelFormatId, Palette);
            if (RemapPaletteValue is null || pixelFormatId != RemapPaletteValue.PixelFormatId)
            {
                RemapPaletteValue ??= new Palette();

                RemapPaletteValue.Assign(Palette);
                _ = RemapPaletteValue.Remap(pixelFormatId);
            }

            return RemapPaletteValue;
        }
    }

    // ImageLib.pas:7124
    public override void Assign(Persistent source)
    {
        if (source is LibraryImagePalette palette)
        {
            Palette.Assign(palette.Palette);
            if (palette.RemapPaletteValue is not null)
            {
                RemapPaletteValue ??= new Palette();

                RemapPaletteValue.Assign(palette.RemapPaletteValue);
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

    public int Index => IndexValue;
    public Palette Palette { get; }
    public int RefCount { get; internal set; }
}
