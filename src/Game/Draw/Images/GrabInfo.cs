// Translated from ImageLib.pas.
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

public partial class GrabInfo
{
    public static GrabInfoClass Descriptor { get; } = new("GrabInfo", static () => new GrabInfo());

    public int TransparentColor;
    public bool FindRectangles;
    public int TransparentPixel;
    public string FileName = "";
    public int FileIndex;

    // ImageLib.pas:9634
    public virtual void LoadFromStream(Stream Stream)
    {
        Stream.ReadExactly(Bytes.Of(ref TransparentColor, 4));
        Stream.ReadExactly(Bytes.Of(ref FindRectangles, 1));
        Stream.ReadExactly(Bytes.Of(ref TransparentPixel, 4));
        LibraryDrawing.ReadStringFromStream(ref FileName, Stream);
        Stream.ReadExactly(Bytes.Of(ref FileIndex, 4));
    }

    // ImageLib.pas:9643
    public virtual void SaveToStream(Stream Stream)
    {
        Stream.Write(Bytes.Of(ref TransparentColor, 4));
        Stream.Write(Bytes.Of(ref FindRectangles, 1));
        Stream.Write(Bytes.Of(ref TransparentPixel, 4));
        LibraryDrawing.WriteStringToStream(FileName, Stream);
        Stream.Write(Bytes.Of(ref FileIndex, 4));
    }

    // ImageLib.pas:9629
    public virtual GrabInfoClass GrabInfoClass() => Descriptor;

    // ImageLib.pas:9652
    public virtual GrabInfo Copy()
    {
        GrabInfo Result = default;
        MemoryStream Stream;
        Stream = new MemoryStream();
        try
        {
            SaveToStream(Stream);
            Stream.Position = 0;
            Result = GrabInfoClass().Create();
            Result.LoadFromStream(Stream);
        }
        finally
        {
            Stream?.Dispose();
        }

        return Result;
    }

}
