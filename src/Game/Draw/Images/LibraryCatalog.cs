#nullable disable

namespace Aow.Game.Draw.Images;

internal static class LibraryCatalog
{
    internal static LibraryImageClass GetLibraryImageClass(int index) => ImageLibraries.LibraryImageClasses[index];

    internal static int LibraryImageClassCount() => ImageLibraries.LibraryImageClasses.Count;

    internal static LibraryImageClass FindLibraryImageClass(int classId)
    {
        bool found = false;
        int i = 0;
        while (i < ImageLibraries.LibraryImageClasses.Count && !found)
        {
            found = classId == ImageLibraries.LibraryImageClasses[i].ClassId;
            i += 1;
        }

        return found ? ImageLibraries.LibraryImageClasses[i - 1] : null;
    }

}
