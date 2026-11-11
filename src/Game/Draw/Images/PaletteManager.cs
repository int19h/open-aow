// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

public partial class PaletteManager : IDisposable
{
    internal List<LibraryImagePalette> FPalettes;

    // ImageLib.pas:6984
    internal int GetCount() => FPalettes.Count;

    // ImageLib.pas:6989
    internal LibraryImagePalette GetPalette(int Index) => FPalettes[Index];

    // ImageLib.pas:6994
    internal void UpdateIndices()
    {
        for (int I = 0, last = FPalettes.Count - 1; I <= last; I++)
        {
            (FPalettes[I]).FIndex = I;
        }
    }

    // ImageLib.pas:6971
    public PaletteManager()
    {
        FPalettes = new List<LibraryImagePalette>();
    }

    // ImageLib.pas:6977
    public virtual void Dispose()
    {
        Clear();
        FPalettes?.Clear();
    }

    // ImageLib.pas:7001
    public void LoadFromStream(Stream Stream)
    {
        int N = default;
        LibraryImagePalette P;
        Clear();
        Stream.ReadBytes(Bytes.Of(ref N, 4));
        for (int I = 0, last = N - 1; I <= last; I++)
        {
            P = new LibraryImagePalette();
            P.LoadFromStream(Stream);
            FPalettes.Add(P);
            P.FIndex = I;
        }
    }

    // ImageLib.pas:7017
    public void SaveToStream(Stream Stream)
    {
        Stream.Write(ValueBytes(FPalettes.Count, 4));
        for (int I = 0, last = FPalettes.Count - 1; I <= last; I++)
        {
            (FPalettes[I]).SaveToStream(Stream);
        }
    }

    // ImageLib.pas:7025
    public void Clear()
    {
        for (int I = FPalettes.Count - 1; I >= 0; I--)
        {
            (FPalettes[I])?.Dispose();
        }

        FPalettes.Clear();
    }

    // ImageLib.pas:7033
    public LibraryImagePalette LinkToPalette(int Index)
    {
        LibraryImagePalette Result = FPalettes[Index];
        Result.FRefCount += 1;
        return Result;
    }

    // ImageLib.pas:7039
    public LibraryImagePalette RegisterPalette(GFXEPalette Palette)
    {
        LibraryImagePalette Result;
        int I;
        bool Found;
        LibraryImagePalette P;
        I = FPalettes.Count;
        Found = false;
        while (I != 0 && !Found)
        {
            I -= 1;
            Found = Palette.Compare((FPalettes[I]).FPalette);
        }

        if (!Found)
        {
            P = new LibraryImagePalette();
            P.FPalette.Assign(Palette);
            P.FIndex = FPalettes.Count;
            P.FRefCount = 1;
            FPalettes.Add(P);
            Result = P;
        }
        else
        {
            var libraryImagePalette = FPalettes[I];
            ((LibraryImagePalette)libraryImagePalette).FRefCount += 1;
            Result = FPalettes[I];
        }

        return Result;
    }

    // ImageLib.pas:7068
    public void UnregisterPalette(int Index)
    {
        LibraryImagePalette P;
        P = FPalettes[Index];
        P.FRefCount -= 1;
        if (P.FRefCount <= 0)
        {
            FPalettes.RemoveAt(Index);
            UpdateIndices();
            P?.Dispose();
        }
    }

    public int Count { get => GetCount(); }

    public sealed class PalettesAccessor(PaletteManager target)
    {
        public LibraryImagePalette this[int Index] { get => target.GetPalette(Index); }
    }

    public PalettesAccessor Palettes => new PalettesAccessor(this);

}
