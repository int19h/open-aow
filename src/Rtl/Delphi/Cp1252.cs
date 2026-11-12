namespace Aow.Rtl;

public static class Cp1252
{
    // This bijection is the frozen text table for the 9.7 document format.
    private static readonly char[] s_characters = MakeCharacters();
    private static readonly byte[] s_bytes = MakeBytes();
    private static readonly string[] s_texts = s_characters.Select(c => new string(c, 1)).ToArray();

    private static char[] MakeCharacters()
    {
        const string Controls = "\u20ac\u0081\u201a\u0192\u201e\u2026\u2020\u2021\u02c6\u2030\u0160\u2039\u0152\u008d\u017d\u008f\u0090\u2018\u2019\u201c\u201d\u2022\u2013\u2014\u02dc\u2122\u0161\u203a\u0153\u009d\u017e\u0178";
        var result = new char[256];
        for (int i = 0; i < result.Length; i++)
            result[i] = i is >= 128 and < 160 ? Controls[i - 128] : (char)i;
        return result;
    }

    private static byte[] MakeBytes()
    {
        var result = new byte[char.MaxValue + 1];
        Array.Fill(result, (byte)'?');
        for (int i = 0; i < s_characters.Length; i++) result[s_characters[i]] = (byte)i;
        return result;
    }

    public static char Char(byte value) => s_characters[value];
    public static byte Byte(char value) => s_bytes[value];
    public static bool IsMapped(char value) => s_characters[s_bytes[value]] == value;
    public static string Text(byte value) => s_texts[value];

    public static IComparer<string> Comparer { get; } = System.Collections.Generic.Comparer<string>.Create(Compare);

    public static string Normalize(string? value)
    {
        value = RtlUnit.RequireString(value);
        return string.Create(value.Length, value, (chars, text) =>
        {
            for (int i = 0; i < chars.Length; i++) chars[i] = Char(Byte(text[i]));
        });
    }

    internal static int Key(char value) => IsMapped(value) ? Byte(value) : 256 + value;

    public static string Decode(ReadOnlySpan<byte> value)
    {
        var chars = new char[value.Length];
        for (int i = 0; i < chars.Length; i++) chars[i] = Char(value[i]);
        return new string(chars);
    }

    public static byte[] Encode(string? value)
    {
        value = RtlUnit.RequireString(value);
        var result = new byte[value.Length];
        for (int i = 0; i < result.Length; i++) result[i] = Byte(value[i]);
        return result;
    }

    public static int Compare(string? a, string? b)
    {
        a = RtlUnit.RequireString(a);
        b = RtlUnit.RequireString(b);
        for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
        {
            int difference = Key(a[i]) - Key(b[i]);
            if (difference != 0) return difference;
        }
        return a.Length - b.Length;
    }
}
