#nullable disable
// Translated from ImageLib.pas.
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

public partial class GrabInfo
{
    public static GrabInfoClass Descriptor { get; } = new("GrabInfo", static () => new GrabInfo());

    public int TransparentColor { get; internal set; }
    public bool FindRectangles { get; internal set; }
    public int TransparentPixel { get; internal set; }
    public string FileName { get; internal set; } = "";
    public int FileIndex { get; internal set; }

    // ImageLib.pas:9634
    public virtual void LoadFromStream(Stream stream)
    {
        var transparentColor = TransparentColor;
        stream.ReadExactly(Bytes.Of(ref transparentColor, 4));
        TransparentColor = transparentColor;
        var findRectangles = FindRectangles;
        stream.ReadExactly(Bytes.Of(ref findRectangles, 1));
        FindRectangles = findRectangles;
        var transparentPixel = TransparentPixel;
        stream.ReadExactly(Bytes.Of(ref transparentPixel, 4));
        TransparentPixel = transparentPixel;
        var fileName = FileName;
        LibraryDrawing.ReadStringFromStream(ref fileName, stream);
        FileName = fileName;
        var fileIndex = FileIndex;
        stream.ReadExactly(Bytes.Of(ref fileIndex, 4));
        FileIndex = fileIndex;
    }

    // ImageLib.pas:9643
    public virtual void SaveToStream(Stream stream)
    {
        var transparentColor = TransparentColor;
        stream.Write(Bytes.Of(ref transparentColor, 4));
        TransparentColor = transparentColor;
        var findRectangles = FindRectangles;
        stream.Write(Bytes.Of(ref findRectangles, 1));
        FindRectangles = findRectangles;
        var transparentPixel = TransparentPixel;
        stream.Write(Bytes.Of(ref transparentPixel, 4));
        TransparentPixel = transparentPixel;
        LibraryDrawing.WriteStringToStream(FileName, stream);
        var fileIndex = FileIndex;
        stream.Write(Bytes.Of(ref fileIndex, 4));
        FileIndex = fileIndex;
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
