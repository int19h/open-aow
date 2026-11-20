namespace Aow.Rtl;

public static class ShortStrings
{
    public static string ReadShortString(ReadOnlySpan<byte> image) => Cp1252.Decode(image.Slice(1, length: image[0]).ToArray());
}
