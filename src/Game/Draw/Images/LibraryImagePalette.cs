// Translated from ImageLib.pas.
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

// FIndex, FPalette, FRemapPalette (the palette remapped to a pixel format, made on demand),
// _refCount
public partial class LibraryImagePalette : Persistent
{
    internal int _index;
    private readonly Palette _palette;

    internal Palette RemapPaletteValue { get; private set; }

    private int _refCount;

    // ImageLib.pas:7084
    public LibraryImagePalette()
    {
        _palette = new Palette();
    }

    // ImageLib.pas:7090
    public override void Dispose()
    {
        base.Dispose();
    }

    // ImageLib.pas:7097
    public void SaveToStream(Stream stream)
    {
        _palette.SaveToStream(stream);
    }

    // ImageLib.pas:7102
    public void LoadFromStream(Stream stream)
    {
        _palette.LoadFromStream(stream);
    }

    // ImageLib.pas:7107
    public Palette RemapPalette(int pixelFormatId)
    {
        Palette result;
        if (pixelFormatId == _palette.PixelFormatId)
        {
            result = _palette;
        }
        else
        {
            PixelFormats.Color2PixelInfo(0, pixelFormatId, _palette);
            if (RemapPaletteValue == null || pixelFormatId != RemapPaletteValue.PixelFormatId)
            {
                if (RemapPaletteValue == null)
                {
                    RemapPaletteValue = new Palette();
                }

                RemapPaletteValue.Assign(_palette);
                RemapPaletteValue.Remap(pixelFormatId);
            }

            result = RemapPaletteValue;
        }

        return result;
    }

    // ImageLib.pas:7124
    public override void Assign(Persistent source)
    {
        if (source is LibraryImagePalette)
        {
            _palette.Assign(((LibraryImagePalette)source)._palette);
            if (((LibraryImagePalette)source).RemapPaletteValue != null)
            {
                if (RemapPaletteValue == null)
                {
                    RemapPaletteValue = new Palette();
                }

                RemapPaletteValue.Assign(((LibraryImagePalette)source).RemapPaletteValue);
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

    public int Index { get => _index; }
    public Palette Palette { get => _palette; }
    public int RefCount { get => _refCount; internal set => _refCount = value; }

}
