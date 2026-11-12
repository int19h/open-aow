// Translated from ImageLib.pas.
using System.Buffers.Binary;

namespace Aow.Game.Draw.Images;

// { How an image is shown (8 bytes): the blend value is a percentage into the blend table
//     collection of the blend mode, or the blend table itself for bmUser. }
public struct ShowInfo
{
    public ShowMode ShowMode { get; set; }
    public BlendMode BlendMode { get; set; }
    public BlendValue Blend { get; set; }

    // The file image: the mode bytes at 0 and 1, the padding at 2 and the percentage at 4. A
    // table writes 0 where the original wrote its address, which no reader of a file uses, and
    // reading always yields the percentage case.
    private ushort _imagePadding;

    private byte[] Image()
    {
        byte[] image = new byte[8];
        image[0] = (byte)ShowMode;
        image[1] = (byte)BlendMode;
        BinaryPrimitives.WriteUInt16LittleEndian(image.AsSpan(2), _imagePadding);
        BinaryPrimitives.WriteInt32LittleEndian(image.AsSpan(4), Blend.TryGetValue(out int percentage) ? percentage : 0);
        return image;
    }

    public int ReadImage(Stream stream, bool exact)
    {
        byte[] image = Image();
        int count = stream.Read(image);
        ShowMode = (ShowMode)image[0];
        BlendMode = (BlendMode)image[1];
        _imagePadding = BinaryPrimitives.ReadUInt16LittleEndian(image.AsSpan(2));
        Blend = BinaryPrimitives.ReadInt32LittleEndian(image.AsSpan(4));
        if (exact && count != 8)
            throw new EndOfStreamException("Stream read error");
        return count;
    }

    public int WriteImage(Stream stream, bool exact)
    {
        byte[] image = Image();
        stream.Write(image);
        int count = image.Length;
        if (exact && count != 8)
            throw new IOException("Stream write error");
        return count;
    }
}
