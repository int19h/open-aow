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
        for (int I = 0, last = _palettes.Count - 1; I <= last; I++)
        {
            (_palettes[I])._index = I;
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
        int N = default;
        LibraryImagePalette P;
        Clear();
        stream.ReadExactly(Bytes.Of(ref N, 4));
        for (int I = 0, last = N - 1; I <= last; I++)
        {
            P = new LibraryImagePalette();
            P.LoadFromStream(stream);
            _palettes.Add(P);
            P._index = I;
        }
    }

    // ImageLib.pas:7017
    public void SaveToStream(Stream stream)
    {
        stream.Write(ValueBytes(_palettes.Count, 4));
        for (int I = 0, last = _palettes.Count - 1; I <= last; I++)
        {
            (_palettes[I]).SaveToStream(stream);
        }
    }

    // ImageLib.pas:7025
    public void Clear()
    {
        for (int I = _palettes.Count - 1; I >= 0; I--)
        {
            (_palettes[I])?.Dispose();
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
        int I;
        bool found;
        LibraryImagePalette P;
        I = _palettes.Count;
        found = false;
        while (I != 0 && !found)
        {
            I -= 1;
            found = palette.Compare((_palettes[I]).Palette);
        }

        if (!found)
        {
            P = new LibraryImagePalette();
            P.Palette.Assign(palette);
            P._index = _palettes.Count;
            P.RefCount = 1;
            _palettes.Add(P);
            result = P;
        }
        else
        {
            var libraryImagePalette = _palettes[I];
            ((LibraryImagePalette)libraryImagePalette).RefCount += 1;
            result = _palettes[I];
        }

        return result;
    }

    // ImageLib.pas:7068
    public void UnregisterPalette(int index)
    {
        LibraryImagePalette P;
        P = _palettes[index];
        P.RefCount -= 1;
        if (P.RefCount <= 0)
        {
            _palettes.RemoveAt(index);
            UpdateIndices();
            P?.Dispose();
        }
    }

    public int Count { get => GetCount(); }

}
