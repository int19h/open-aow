#nullable disable warnings

using Aow.Rtl;

namespace Aow.Game.Draw.Images;

public static class ImageStreams
{

    public static LibraryImage? ReadImageFromStream(Stream stream, CustomImageLibrary? owner)
    {
        int id = default;
        stream.ReadExactly(Bytes.Of(ref id));
        LibraryImageClass? imageClass = LibraryCatalog.FindLibraryImageClass(id);
        LibraryImage result;
        if (imageClass is not null)
        {
            result = imageClass.CreateInstance(owner);
            result.LoadFromStream(stream);
        }
        else
        {
            return null;
        }

        return result;
    }

    internal static LibraryImage? ReadImageFromStream(Stream stream, Stream dataStream, CustomImageLibrary? owner)
    {
        int id = default;
        stream.ReadExactly(Bytes.Of(ref id));
        LibraryImageClass? imageClass = LibraryCatalog.FindLibraryImageClass(id);
        LibraryImage result;
        if (imageClass is not null)
        {
            result = imageClass.CreateInstance(owner);
            result.LoadFromStream(stream, dataStream);
        }
        else
        {
            return null;
        }

        return result;
    }
}
