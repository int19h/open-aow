
namespace Aow.Game.Draw.Images;

internal static class LibraryCatalog
{
    internal static LibraryImageClass GetLibraryImageClass(int index) => ImageLibraries.LibraryImageClasses[index];

    internal static int LibraryImageClassCount() => ImageLibraries.LibraryImageClasses.Count;

    internal static LibraryImageClass FindLibraryImageClass(int classId)
    {
        LibraryImageClass result;
        int I;
        bool found;
        found = false;
        I = 0;
        while (I < ImageLibraries.LibraryImageClasses.Count && !found)
        {
            found = classId == (ImageLibraries.LibraryImageClasses[I]).ClassId;
            I += 1;
        }

        if (found)
        {
            result = ImageLibraries.LibraryImageClasses[I - 1];
        }
        else
        {
            result = null;
        }

        return result;
    }

}
