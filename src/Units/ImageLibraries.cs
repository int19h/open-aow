using System.Buffers.Binary;
using System.Runtime.InteropServices;
using Aow.Game.Draw;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Hash;
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
            if (AClasses[I].ClassID != 0 && Aow.Game.Draw.Images.LibraryCatalog.FindLibraryImageClass(AClasses[I].ClassID) == null)
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

    public static TList LibraryImageClasses;

    public static void Initialize()
    {
        BaseImageLibraryPath = ExtractFilePath(Application.ExeName);
        LibraryImageClasses = new TList();
        RegisterLibraryImageClasses([LibraryImageClass.Of<TPicture08>(), LibraryImageClass.Of<TRLESprite08>()]);
        RegisterLibraryImageClasses([LibraryImageClass.Of<TPicture16>(), LibraryImageClass.Of<TTransparentPicture16>(), LibraryImageClass.Of<TRLESprite16>(), LibraryImageClass.Of<TTransparentRLESprite16>(), LibraryImageClass.Of<TShadow>(), LibraryImageClass.Of<TImageNode>(), LibraryImageClass.Of<TSprite16>()]);
    }

    public static void Finalize()
    {
        LibraryImageClasses?.Dispose();
    }

}
