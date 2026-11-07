namespace Aow.Rtl;

public static partial class RtlUnit
{
    public static bool StrictStrings { get; set; }

    internal static string RequireString(string? value) => value
        ?? (StrictStrings ? throw new NullStringException() : "");

    public static bool IsEmpty(string? value) => RequireString(value).Length == 0;

    public static string[] Strings(int length)
    {
        var result = new string[length];
        Array.Fill(result, "");
        return result;
    }

    public static string WithChar(string? value, int index, char character)
    {
        value = RequireString(value);
        if (index < 1 || index > value.Length) throw new ERangeError("String index out of bounds");
        return string.Create(value.Length, (value, index, character), (chars, state) =>
        {
            state.value.AsSpan().CopyTo(chars);
            chars[state.index - 1] = state.character;
        });
    }

    public static string Resized(string? value, int length)
    {
        if (length < 0) throw new ERangeError("Negative string length");
        value = RequireString(value);
        return length <= value.Length ? value[..length] : value.PadRight(length, '\0');
    }

    public static string Copy(string? value, int index, int count)
    {
        value = RequireString(value);
        index = Math.Max(1, index);
        return index > value.Length || count <= 0 ? ""
            : value.Substring(index - 1, Math.Min(count, value.Length - index + 1));
    }

    public static string Delete(string? value, int index, int count)
    {
        value = RequireString(value);
        return index < 1 || index > value.Length || count <= 0 ? value
            : value.Remove(index - 1, Math.Min(count, value.Length - index + 1));
    }

    public static void Delete(ref string value, int index, int count) => value = Delete(value, index, count);

    public static string Insert(string? source, string? value, int index)
    {
        value = RequireString(value);
        int offset = index <= 1 ? 0 : Math.Min(index - 1, value.Length);
        return value.Insert(offset, RequireString(source));
    }

    public static void Insert(string? source, ref string value, int index) => value = Insert(source, value, index);

    public static int Pos(string? needle, string? haystack)
    {
        needle = RequireString(needle);
        haystack = RequireString(haystack);
        return needle.Length == 0 ? 0 : haystack.AsSpan().IndexOf(needle.AsSpan()) + 1;
    }

    public static void SetLength(ref string value, int length) => value = Resized(value, length);
    public static int Length(string? value) => RequireString(value).Length;
    public static int Ord(char value) => Cp1252.Byte(value);
    public static char UpCase(char value) => value is >= 'a' and <= 'z' ? (char)(value - 32) : value;

    public static void SetString(ref string value, Ptr<byte> source, int length) => value = length <= 0 ? ""
        : source.IsNull ? new string('\0', length) : Cp1252.Decode(source.AsSpan(length));

    public static string FromPChar(Ptr<byte> value) => value.IsNull ? "" : Cp1252.Decode(value.AsSpan(StrLen(value)));

    public static string NullTerminatedText(string? value)
    {
        value = RequireString(value);
        int end = value.IndexOf('\0');
        return end < 0 ? value : value[..end];
    }

    public static void Assert(bool condition, string? message, string file, int line)
    {
        message = RequireString(message);
        if (!condition) throw new EAssertionFailed($"{(string.IsNullOrEmpty(message) ? "Assertion failed" : message)} ({file}, line {line})");
    }

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

    public static string TrimLeft(string? value)
    {
        value = RequireString(value);
        int i = 0;
        while (i < value.Length && value[i] <= 32) i++;
        return value[i..];
    }

    public static string TrimRight(string? value)
    {
        value = RequireString(value);
        int n = value.Length;
        while (n > 0 && value[n - 1] <= 32) n--;
        return value[..n];
    }

    public static string Trim(string? value) => TrimLeft(TrimRight(value));
    public static string QuotedStr(string? value) => "'" + RequireString(value).Replace("'", "''") + "'";
    public static string StringOfChar(byte character, int count) => StringOfChar(Cp1252.Char(character), count);
    public static string StringOfChar(char character, int count) => new(character, Math.Max(0, count));
    public static int AnsiPos(string? needle, string? value) => Pos(needle, value);

    public static int StrToInt(string? value) => TryInteger(value, out int result) ? result
        : throw new EConvertError($"'{value}' is not a valid integer value");

    public static int StrToIntDef(string? value, int fallback) => TryInteger(value, out int result) ? result : fallback;

    // SYSTEM.PAS _ValLong stops at the first zero and accepts only leading spaces.
    private static bool TryInteger(string? text, out int result)
    {
        text = RequireString(text);
        result = 0;
        int i = 0;
        char At(int n) => n < text.Length ? text[n] : '\0';
        while (At(i) == ' ') i++;
        bool negative = At(i) == '-';
        bool signed = negative || At(i) == '+';
        if (signed) i++;
        bool hex = At(i) == '$';
        if (hex && signed) return false;
        if (hex) i++;
        int start = i;
        ulong number = 0;
        for (; At(i) != 0; i++)
        {
            char c = UpCase(At(i));
            int digit = c is >= '0' and <= '9' ? c - '0' : c is >= 'A' and <= 'F' ? c - 'A' + 10 : -1;
            int radix = hex ? 16 : 10;
            if (digit < 0 || digit >= radix) return false;
            number = number * (uint)radix + (uint)digit;
            if (number > (hex ? uint.MaxValue : int.MaxValue)) return false;
        }
        if (i == start) return false;
        result = unchecked((int)(uint)number);
        if (negative) result = unchecked(-result);
        return true;
    }
}
