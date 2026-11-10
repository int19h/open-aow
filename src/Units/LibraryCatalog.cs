using System.Buffers.Binary;
using System.Runtime.InteropServices;
using Aow.Game.Draw;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

internal static class LibraryCatalog
{
    internal static LibraryImageClass GetLibraryImageClass(int Index) => (LibraryImageClass)ImageLibraries.LibraryImageClasses[Index];

    internal static int LibraryImageClassCount() => ImageLibraries.LibraryImageClasses.Count;

    internal static LibraryImageClass FindLibraryImageClass(int ClassID)
    {
        LibraryImageClass Result;
        int I;
        bool Found;
        Found = false;
        I = 0;
        while (I < ImageLibraries.LibraryImageClasses.Count && !Found)
        {
            Found = ClassID == ((LibraryImageClass)ImageLibraries.LibraryImageClasses[I]).ClassID;
            I += 1;
        }

        if (Found)
        {
            Result = (LibraryImageClass)ImageLibraries.LibraryImageClasses[I - 1];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

}
