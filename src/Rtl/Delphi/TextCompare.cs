namespace Aow.Rtl;

public static partial class RtlUnit
{
    private static string Map(string? value, Func<char, char> map)
    {
        value = RequireString(value);
        return string.Create(value.Length, (value, map), (chars, state) =>
        {
            for (int i = 0; i < chars.Length; i++) chars[i] = state.map(state.value[i]);
        });
    }

    public static string UpperCase(string? value) => Map(value, UpCase);
    public static string LowerCase(string? value) => Map(value, c => c is >= 'A' and <= 'Z' ? (char)(c + 32) : c);
    public static string AnsiLowerCase(string? value) => Map(value, c => Cp1252.IsMapped(c) ? Cp1252.Char(Fold1252(Cp1252.Byte(c))) : c);

    public static string AnsiUpperCase(string? value) => Map(value, c => Cp1252.IsMapped(c) ? Cp1252.Char(Cp1252.Byte(c) switch
    {
        >= 97 and <= 122 or >= 224 and <= 246 or >= 248 and <= 254 => (byte)(Cp1252.Byte(c) - 32),
        154 => 138,
        156 => 140,
        158 => 142,
        255 => 159,
        byte b => b
    }) : c);

    private static int CompareBytes(string? a, string? b, Func<byte, byte> map)
    {
        a = RequireString(a);
        b = RequireString(b);
        for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
        {
            int Key(char c) => Cp1252.IsMapped(c) ? map(Cp1252.Byte(c)) : Cp1252.Key(c);
            int difference = Key(a[i]) - Key(b[i]);
            if (difference != 0) return difference;
        }
        return a.Length - b.Length;
    }

    public static int CompareStr(string? a, string? b) => Cp1252.Compare(a, b);
    public static int CompareText(string? a, string? b) => CompareBytes(a, b, UpCase);
    public static int AnsiCompareStr(string? a, string? b) => CompareBytes(a, b, CollationByte);
    public static int AnsiCompareText(string? a, string? b) => CompareBytes(a, b, c => CollationByte(Fold1252(c)));

    internal static byte Fold1252(byte c) => c switch
    {
        >= 65 and <= 90 or >= 192 and <= 214 or >= 216 and <= 222 => (byte)(c + 32),
        138 => 154,
        140 => 156,
        142 => 158,
        159 => 255,
        _ => c
    };

    private static byte CollationByte(byte c) => c switch { 173 => 0, 136 => 94, _ => c };
}
