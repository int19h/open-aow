
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

public static class ImageLibraries
{
    internal static void RegisterLibraryImageClasses(ReadOnlySpan<LibraryImageClass> classes)
    {
        for (int I = 0, last = classes.Length - 1; I <= last; I++)
        {
            if (classes[I].ClassId != 0 && LibraryCatalog.FindLibraryImageClass(classes[I].ClassId) == null)
            {
                LibraryImageClasses.Add(classes[I]);
            }
            else
            {
                throw new InvalidOperationException("Image ClassID is already used (" + classes[I].StorageKey + ")");
            }
        }
    }

    internal static string BaseImageLibraryPath = "";

    public static List<LibraryImageClass> LibraryImageClasses;

    public static void Initialize()
    {
        BaseImageLibraryPath = ExtractFilePath(RtlUnit.Application.ExeName);
        LibraryImageClasses = new List<LibraryImageClass>();
        RegisterLibraryImageClasses([LibraryImageClass.Of<Picture08>(), LibraryImageClass.Of<RleSprite08>()]);
        RegisterLibraryImageClasses([LibraryImageClass.Of<Picture16>(), LibraryImageClass.Of<TransparentPicture16>(), LibraryImageClass.Of<RleSprite16>(), LibraryImageClass.Of<TransparentRleSprite16>(), LibraryImageClass.Of<Shadow>(), LibraryImageClass.Of<ImageNode>(), LibraryImageClass.Of<Sprite16>()]);
    }

    public static void Finalize()
    {
        LibraryImageClasses?.Clear();
    }

}
