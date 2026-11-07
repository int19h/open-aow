namespace Aow.Rtl;

public static partial class RtlUnit
{
    public static string SetString(Ptr<byte> source, int length)
    {
        if (length <= 0) return "";
        if (source.IsNull) return Resized("", length);
        return Cp1252.Decode(source.Array!.AsSpan(source.Index, length).ToArray());
    }

    public static int StrLen(Ptr<byte> value)
    {
        int count = 0;
        while (value[count] != 0) count++;
        return count;
    }

    public static string StrPas(Ptr<byte> value) => FromPChar(value);

    public static string StrPasBuffer(ReadOnlySpan<byte> value)
    {
        int end = value.IndexOf((byte)0);
        return Cp1252.Decode((end < 0 ? value : value[..end]).ToArray());
    }

    public static Ptr<byte> StrCopy(Ptr<byte> destination, Ptr<byte> source)
    {
        Ptr<byte>.Copy(source, destination, StrLen(source) + 1);
        return destination;
    }

    public static int StrLComp(Ptr<byte> a, Ptr<byte> b, int maxLength)
    {
        for (int i = 0; i < maxLength; i++)
        {
            int difference = a[i] - b[i];
            if (difference != 0 || a[i] == 0) return difference;
        }
        return 0;
    }

    public static Ptr<byte> StrScan(Ptr<byte> value, byte character)
    {
        for (int i = 0; ; i++)
        {
            if (value[i] == character) return value + i;
            if (value[i] == 0) return default;
        }
    }

    public static void SetString(ref string value, Ptr<byte> source, int length) => value = length <= 0 ? ""
        : source.IsNull ? new string('\0', length) : Cp1252.Decode(source.AsSpan(length));

    public static string FromPChar(Ptr<byte> value) => value.IsNull ? "" : Cp1252.Decode(value.AsSpan(StrLen(value)));

    public static string NullTerminatedText(string? value)
    {
        value = RequireString(value);
        int end = value.IndexOf('\0');
        return end < 0 ? value : value[..end];
    }
}
