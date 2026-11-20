#nullable disable

using Aow.Rtl;

namespace Aow.Game.Draw.Images;

public static class ImageStreams
{

    public static LibraryImage ReadImageFromStream(Stream stream, CustomImageLibrary owner)
    {
        LibraryImage result;
        int id = default;
        LibraryImageClass imageClass;
        stream.ReadExactly(Bytes.Of(ref id, 4));
        imageClass = LibraryCatalog.FindLibraryImageClass(id);
        if (imageClass is not null)
        {
            result = imageClass.CreateInstance(owner);
            result.LoadFromStream(stream);
        }
        else
        {
            result = null;
        }

        return result;
    }

    internal static LibraryImage ReadImageFromStream(Stream stream, Stream dataStream, CustomImageLibrary owner)
    {
        LibraryImage result;
        int id = default;
        LibraryImageClass imageClass;
        stream.ReadExactly(Bytes.Of(ref id, 4));
        imageClass = LibraryCatalog.FindLibraryImageClass(id);
        if (imageClass is not null)
        {
            result = imageClass.CreateInstance(owner);
            result.LoadFromStream(stream, dataStream);
        }
        else
        {
            result = null;
        }

        return result;
    }

}
