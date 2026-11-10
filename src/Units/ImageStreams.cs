using System.Buffers.Binary;
using System.Runtime.InteropServices;
using Aow.Game.Draw;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Hash;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

public static class ImageStreams
{

    public static LibraryImage ReadImageFromStream(Stream Stream, CustomImageLibrary AOwner)
    {
        LibraryImage Result;
        int ID = default;
        LibraryImageClass ImageClass;
        Stream.ReadBytes(Bytes.Of(ref ID, 4));
        ImageClass = LibraryCatalog.FindLibraryImageClass(ID);
        if (ImageClass != null)
        {
            Result = ImageClass.Create(AOwner);
            Result.LoadFromStream(Stream);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    internal static LibraryImage ReadImageFromStreamEx(Stream Stream, Stream DataStream, CustomImageLibrary AOwner)
    {
        LibraryImage Result;
        int ID = default;
        LibraryImageClass ImageClass;
        Stream.ReadBytes(Bytes.Of(ref ID, 4));
        ImageClass = LibraryCatalog.FindLibraryImageClass(ID);
        if (ImageClass != null)
        {
            Result = ImageClass.Create(AOwner);
            Result.LoadFromStreamEx(Stream, DataStream);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

}
