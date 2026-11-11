
namespace Aow.Game.Draw.Images;

internal static class LibraryCatalog
{
    internal static LibraryImageClass GetLibraryImageClass(int Index) => ImageLibraries.LibraryImageClasses[Index];

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
            Found = ClassID == (ImageLibraries.LibraryImageClasses[I]).ClassID;
            I += 1;
        }

        if (Found)
        {
            Result = ImageLibraries.LibraryImageClasses[I - 1];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

}
