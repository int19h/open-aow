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
}
