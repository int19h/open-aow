namespace Aow.Rtl;

public static partial class RtlUnit
{
    public static int ReadString(Stream stream, ref string value, int start, int count, bool exact)
    {
        byte[] bytes = Cp1252.Encode(value).ToArray();
        var buffer = bytes.AsSpan(start, count);
        if (exact && count == 0) return 0;
        int read = stream.Read(buffer);
        value = Cp1252.Decode(bytes);
        if (exact && read != count) throw new EndOfStreamException("Stream read error");
        return read;
    }
}
