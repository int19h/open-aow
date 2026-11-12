// Translated from ImageLib.pas.
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

// FIndex, FPalette, FRemapPalette (the palette remapped to a pixel format, made on demand),
// _refCount
public partial class LibraryImagePalette : Persistent
{
    internal int _index;
    internal Palette _palette;
    internal Palette _remapPaletteValue;
    internal int _refCount;

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
            if (_remapPaletteValue == null || pixelFormatId != _remapPaletteValue.PixelFormatId)
            {
                if (_remapPaletteValue == null)
                {
                    _remapPaletteValue = new Palette();
                }

                _remapPaletteValue.Assign(_palette);
                _remapPaletteValue.Remap(pixelFormatId);
            }

            result = _remapPaletteValue;
        }

        return result;
    }

    // ImageLib.pas:7124
    public override void Assign(Persistent source)
    {
        if (source is LibraryImagePalette)
        {
            _palette.Assign(((LibraryImagePalette)source)._palette);
            if (((LibraryImagePalette)source)._remapPaletteValue != null)
            {
                if (_remapPaletteValue == null)
                {
                    _remapPaletteValue = new Palette();
                }

                _remapPaletteValue.Assign(((LibraryImagePalette)source)._remapPaletteValue);
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
    public int RefCount { get => _refCount; }

}
