#nullable disable

namespace Aow.Game.Draw.Images;

internal static class LibraryCatalog
{
    internal static LibraryImageClass GetLibraryImageClass(int index) => ImageLibraries.LibraryImageClasses[index];

    internal static int LibraryImageClassCount() => ImageLibraries.LibraryImageClasses.Count;

    internal static LibraryImageClass FindLibraryImageClass(int classId)
    {
        LibraryImageClass result;
        int i;
        bool found;
        found = false;
        i = 0;
        while (i < ImageLibraries.LibraryImageClasses.Count && !found)
        {
            found = classId == (ImageLibraries.LibraryImageClasses[i]).ClassId;
            i += 1;
        }

        if (found)
        {
            result = ImageLibraries.LibraryImageClasses[i - 1];
        }
        else
        {
            result = null;
        }

        return result;
    }

}
