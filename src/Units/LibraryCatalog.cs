using System.Buffers.Binary;
using System.Runtime.InteropServices;
using Aow.Game.Draw;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Hash;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

internal static class LibraryCatalog
{
    internal static LibraryImageClass GetLibraryImageClass(int Index) => (LibraryImageClass)Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses[Index];

    internal static int LibraryImageClassCount() => Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses.Count;

    internal static LibraryImageClass FindLibraryImageClass(int ClassID)
    {
        LibraryImageClass Result;
        int I;
        bool Found;
        Found = false;
        I = 0;
        while (I < Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses.Count && !Found)
        {
            Found = ClassID == ((LibraryImageClass)Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses[I]).ClassID;
            I += 1;
        }

        if (Found)
        {
            Result = (LibraryImageClass)Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses[I - 1];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

}
