namespace Aow.Rtl;

public static class ShortStrings
{
    public static string ReadShortString(ReadOnlySpan<byte> image) => Cp1252.Decode(image.Slice(1, length: image[0]).ToArray());

    public static void WriteShortString(Span<byte> image, string value)
    {
        int length = Math.Min(value.Length, image.Length - 1);
        image[0] = (byte)length;
        Cp1252.Encode(value).AsSpan()[..length].CopyTo(image[1..]);
    }
}
