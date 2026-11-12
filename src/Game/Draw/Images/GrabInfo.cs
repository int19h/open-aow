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
    public virtual void LoadFromStream(Stream stream)
    {
        stream.ReadExactly(Bytes.Of(ref TransparentColor, 4));
        stream.ReadExactly(Bytes.Of(ref FindRectangles, 1));
        stream.ReadExactly(Bytes.Of(ref TransparentPixel, 4));
        LibraryDrawing.ReadStringFromStream(ref FileName, stream);
        stream.ReadExactly(Bytes.Of(ref FileIndex, 4));
    }

    // ImageLib.pas:9643
    public virtual void SaveToStream(Stream stream)
    {
        stream.Write(Bytes.Of(ref TransparentColor, 4));
        stream.Write(Bytes.Of(ref FindRectangles, 1));
        stream.Write(Bytes.Of(ref TransparentPixel, 4));
        LibraryDrawing.WriteStringToStream(FileName, stream);
        stream.Write(Bytes.Of(ref FileIndex, 4));
    }

    // ImageLib.pas:9629
    public virtual GrabInfoClass GrabInfoClass() => Descriptor;

    // ImageLib.pas:9652
    public virtual GrabInfo Copy()
    {
        GrabInfo result = default;
        MemoryStream stream;
        stream = new MemoryStream();
        try
        {
            SaveToStream(stream);
            stream.Position = 0;
            result = GrabInfoClass().CreateInstance();
            result.LoadFromStream(stream);
        }
        finally
        {
            stream?.Dispose();
        }

        return result;
    }

}
