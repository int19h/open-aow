// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

public partial class PaletteManager : IDisposable
{
    internal List<LibraryImagePalette> Palettes { get; }

    // ImageLib.pas:6984
    internal int GetCount() => Palettes.Count;

    // ImageLib.pas:6989
    internal LibraryImagePalette GetPalette(int index) => Palettes[index];

    // ImageLib.pas:6994
    internal void UpdateIndices()
    {
        for (int i = 0, last = Palettes.Count - 1; i <= last; i++)
        {
            Palettes[i].IndexValue = i;
        }
    }

    // ImageLib.pas:6971
    public PaletteManager() => Palettes = [];

    // ImageLib.pas:6977
    public virtual void Dispose()
    {
        Clear();
        Palettes?.Clear();
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
            Palettes.Add(p);
            p.IndexValue = i;
        }
    }

    // ImageLib.pas:7017
    public void SaveToStream(Stream stream)
    {
        stream.Write(ValueBytes(Palettes.Count, 4));
        for (int i = 0, last = Palettes.Count - 1; i <= last; i++)
        {
            Palettes[i].SaveToStream(stream);
        }
    }

    // ImageLib.pas:7025
    public void Clear()
    {
        for (int i = Palettes.Count - 1; i >= 0; i--)
        {
            Palettes[i]?.Dispose();
        }

        Palettes.Clear();
    }

    // ImageLib.pas:7033
    public LibraryImagePalette LinkToPalette(int index)
    {
        LibraryImagePalette result = Palettes[index];
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
        i = Palettes.Count;
        found = false;
        while (i != 0 && !found)
        {
            i -= 1;
            found = palette.Compare(Palettes[i].Palette);
        }

        if (!found)
        {
            p = new LibraryImagePalette();
            p.Palette.Assign(palette);
            p.IndexValue = Palettes.Count;
            p.RefCount = 1;
            Palettes.Add(p);
            result = p;
        }
        else
        {
            LibraryImagePalette libraryImagePalette = Palettes[i];
            libraryImagePalette.RefCount += 1;
            result = Palettes[i];
        }

        return result;
    }

    // ImageLib.pas:7068
    public void UnregisterPalette(int index)
    {
        LibraryImagePalette p;
        p = Palettes[index];
        p.RefCount -= 1;
        if (p.RefCount <= 0)
        {
            Palettes.RemoveAt(index);
            UpdateIndices();
            p?.Dispose();
        }
    }

    public int Count => GetCount();

}
