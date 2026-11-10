using System.Buffers.Binary;
using System.Runtime.InteropServices;
using Aow.Game.Draw;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

public static class ImageLibraries
{
    internal static void RegisterLibraryImageClasses(ReadOnlySpan<LibraryImageClass> AClasses)
    {
        for (int I = 0, last = AClasses.Length - 1; I <= last; I++)
        {
            if (AClasses[I].ClassID != 0 && LibraryCatalog.FindLibraryImageClass(AClasses[I].ClassID) == null)
            {
                LibraryImageClasses.Add(AClasses[I]);
            }
            else
            {
                throw new InvalidOperationException("Image ClassID is already used (" + AClasses[I].ClassName + ")");
            }
        }
    }

    internal static string BaseImageLibraryPath = "";

    public static DelphiList LibraryImageClasses;

    public static void Initialize()
    {
        BaseImageLibraryPath = ExtractFilePath(RtlUnit.Application.ExeName);
        LibraryImageClasses = new DelphiList();
        RegisterLibraryImageClasses([LibraryImageClass.Of<Picture08>(), LibraryImageClass.Of<RLESprite08>()]);
        RegisterLibraryImageClasses([LibraryImageClass.Of<Picture16>(), LibraryImageClass.Of<TransparentPicture16>(), LibraryImageClass.Of<RLESprite16>(), LibraryImageClass.Of<TransparentRLESprite16>(), LibraryImageClass.Of<Shadow>(), LibraryImageClass.Of<ImageNode>(), LibraryImageClass.Of<Sprite16>()]);
    }

    public static void Finalize()
    {
        LibraryImageClasses?.Dispose();
    }

}
