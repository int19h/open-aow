#nullable disable
// Translated from ImageLib.pas.
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

public class GrabInfo
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
        int transparentColor = TransparentColor;
        stream.ReadExactly(Bytes.Of(ref transparentColor));
        TransparentColor = transparentColor;
        bool findRectangles = FindRectangles;
        stream.ReadExactly(Bytes.Of(ref findRectangles));
        FindRectangles = findRectangles;
        int transparentPixel = TransparentPixel;
        stream.ReadExactly(Bytes.Of(ref transparentPixel));
        TransparentPixel = transparentPixel;
        string fileName = FileName;
        LibraryDrawing.ReadStringFromStream(ref fileName, stream);
        FileName = fileName;
        int fileIndex = FileIndex;
        stream.ReadExactly(Bytes.Of(ref fileIndex));
        FileIndex = fileIndex;
    }

    // ImageLib.pas:9643
    public virtual void SaveToStream(Stream stream)
    {
        int transparentColor = TransparentColor;
        stream.Write(Bytes.Of(ref transparentColor));
        TransparentColor = transparentColor;
        bool findRectangles = FindRectangles;
        stream.Write(Bytes.Of(ref findRectangles));
        FindRectangles = findRectangles;
        int transparentPixel = TransparentPixel;
        stream.Write(Bytes.Of(ref transparentPixel));
        TransparentPixel = transparentPixel;
        LibraryDrawing.WriteStringToStream(FileName, stream);
        int fileIndex = FileIndex;
        stream.Write(Bytes.Of(ref fileIndex));
        FileIndex = fileIndex;
    }

    // ImageLib.pas:9629
    public virtual GrabInfoClass GrabInfoClass() => Descriptor;

    // ImageLib.pas:9652
    public virtual GrabInfo Copy()
    {
        MemoryStream stream = new();
        GrabInfo result;
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
