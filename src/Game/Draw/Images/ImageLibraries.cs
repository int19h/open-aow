#nullable disable warnings

using Aow.Rtl;
using static Aow.Rtl.DelphiPaths;

namespace Aow.Game.Draw.Images;

public static class ImageLibraries
{
    internal static void RegisterLibraryImageClasses(ReadOnlySpan<LibraryImageClass> classes)
    {
        for (int i = 0, last = classes.Length - 1; i <= last; i++)
        {
            if (classes[i].ClassId != 0 && LibraryCatalog.FindLibraryImageClass(classes[i].ClassId) is null)
            {
                LibraryImageClasses.Add(classes[i]);
            }
            else
            {
                throw new InvalidOperationException($"Image ClassID is already used ({classes[i].StorageKey})");
            }
        }
    }

    internal static string BaseImageLibraryPath { get; set; } = "";

    public static List<LibraryImageClass> LibraryImageClasses { get; set; }

    public static void Initialize()
    {
        BaseImageLibraryPath = ExtractFilePath(LegacyApplication.Application.ExeName);
        LibraryImageClasses = [];
        RegisterLibraryImageClasses([LibraryImageClass.Of<Picture08>(), LibraryImageClass.Of<RleSprite08>()]);
        RegisterLibraryImageClasses([LibraryImageClass.Of<Picture16>(), LibraryImageClass.Of<TransparentPicture16>(), LibraryImageClass.Of<RleSprite16>(), LibraryImageClass.Of<TransparentRleSprite16>(), LibraryImageClass.Of<Shadow>(), LibraryImageClass.Of<ImageNode>(), LibraryImageClass.Of<Sprite16>()]);
    }

    public static void Finalize() => LibraryImageClasses?.Clear();

}
