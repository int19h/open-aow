// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

public partial class PaletteManager : IDisposable
{
    internal readonly List<LibraryImagePalette> _palettes;

    // ImageLib.pas:6984
    internal int GetCount() => _palettes.Count;

    // ImageLib.pas:6989
    internal LibraryImagePalette GetPalette(int index) => _palettes[index];

    // ImageLib.pas:6994
    internal void UpdateIndices()
    {
        for (int i = 0, last = _palettes.Count - 1; i <= last; i++)
        {
            (_palettes[i])._index = i;
        }
    }

    // ImageLib.pas:6971
    public PaletteManager()
    {
        _palettes = new List<LibraryImagePalette>();
    }

    // ImageLib.pas:6977
    public virtual void Dispose()
    {
        Clear();
        _palettes?.Clear();
    }

    // ImageLib.pas:7001
    public void LoadFromStream(Stream stream)
    {
        int n = default;
        LibraryImagePalette p;
        Clear();
        stream.ReadExactly(Bytes.Of(ref n, 4));
        for (int i = 0, last = n - 1; i <= last; i++)
        {
            p = new LibraryImagePalette();
            p.LoadFromStream(stream);
            _palettes.Add(p);
            p._index = i;
        }
    }

    // ImageLib.pas:7017
    public void SaveToStream(Stream stream)
    {
        stream.Write(ValueBytes(_palettes.Count, 4));
        for (int i = 0, last = _palettes.Count - 1; i <= last; i++)
        {
            (_palettes[i]).SaveToStream(stream);
        }
    }

    // ImageLib.pas:7025
    public void Clear()
    {
        for (int i = _palettes.Count - 1; i >= 0; i--)
        {
            (_palettes[i])?.Dispose();
        }

        _palettes.Clear();
    }

    // ImageLib.pas:7033
    public LibraryImagePalette LinkToPalette(int index)
    {
        LibraryImagePalette result = _palettes[index];
        result.RefCount += 1;
        return result;
    }

    // ImageLib.pas:7039
    public LibraryImagePalette RegisterPalette(Palette palette)
    {
        LibraryImagePalette result;
        int i;
        bool found;
        LibraryImagePalette p;
        i = _palettes.Count;
        found = false;
        while (i != 0 && !found)
        {
            i -= 1;
            found = palette.Compare((_palettes[i]).Palette);
        }

        if (!found)
        {
            p = new LibraryImagePalette();
            p.Palette.Assign(palette);
            p._index = _palettes.Count;
            p.RefCount = 1;
            _palettes.Add(p);
            result = p;
        }
        else
        {
            var libraryImagePalette = _palettes[i];
            ((LibraryImagePalette)libraryImagePalette).RefCount += 1;
            result = _palettes[i];
        }

        return result;
    }

    // ImageLib.pas:7068
    public void UnregisterPalette(int index)
    {
        LibraryImagePalette p;
        p = _palettes[index];
        p.RefCount -= 1;
        if (p.RefCount <= 0)
        {
            _palettes.RemoveAt(index);
            UpdateIndices();
            p?.Dispose();
        }
    }

    public int Count { get => GetCount(); }

}
