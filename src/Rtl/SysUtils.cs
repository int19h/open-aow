using System.Globalization;

namespace Aow.Rtl;

public static partial class Rtl
{
    public static AnsiString IntToStr(int value) => value.ToString(CultureInfo.InvariantCulture);
    // SYSUTILS.PAS IntToHex uses FormatBuf, which ignores precision outside 0..15.
    public static AnsiString IntToHex(int value, int digits) => ((uint)value).ToString("X", CultureInfo.InvariantCulture).PadLeft(digits is > 0 and < 16 ? digits : 0, '0');
    public static int StrToInt(AnsiString value) => TryInteger(value, out int result) ? result
        : throw new EConvertError($"'{value}' is not a valid integer value");
    public static int StrToIntDef(AnsiString value, int fallback) => TryInteger(value, out int result) ? result : fallback;
    // SYSTEM.PAS _ValLong, called by SYSUTILS.PAS StrToInt.
    private static bool TryInteger(AnsiString text, out int result)
    {
        result = 0;
        int i = 1;
        byte At(int n) => n <= text.Length ? text[n] : (byte)0;
        while (At(i) == 32) i++;
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
            byte c = UpCase(At(i));
            int digit = c is >= 48 and <= 57 ? c - 48 : c is >= 65 and <= 70 ? c - 55 : -1;
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
    private static AnsiString Map(AnsiString value, Func<byte, byte> map) => AnsiString.FromBytes(value.Span.ToArray().Select(map).ToArray());
    public static AnsiString UpperCase(AnsiString value) => Map(value, UpCase);
    public static AnsiString LowerCase(AnsiString value) => Map(value, c => c is >= 65 and <= 90 ? (byte)(c + 32) : c);
    internal static byte Fold1252(byte c) => c switch
    {
        >= 65 and <= 90 or >= 192 and <= 214 or >= 216 and <= 222 => (byte)(c + 32),
        138 => 154,
        140 => 156,
        142 => 158,
        159 => 255,
        _ => c
    };
    public static AnsiString AnsiLowerCase(AnsiString value) => Map(value, Fold1252);
    public static AnsiString AnsiUpperCase(AnsiString value) => Map(value, c => c switch
    {
        >= 97 and <= 122 or >= 224 and <= 246 or >= 248 and <= 254 => (byte)(c - 32),
        154 => 138,
        156 => 140,
        158 => 142,
        255 => 159,
        _ => c
    });
    // SYSUTILS.PAS CompareStr and CompareText compare lengths after an equal prefix.
    private static int CompareBytes(AnsiString a, AnsiString b, Func<byte, byte> map)
    {
        for (int i = 1; i <= Math.Min(a.Length, b.Length); i++)
        {
            int difference = map(a[i]) - map(b[i]);
            if (difference != 0) return difference;
        }
        return a.Length - b.Length;
    }
    public static int CompareStr(AnsiString a, AnsiString b) => CompareBytes(a, b, c => c);
    public static int CompareText(AnsiString a, AnsiString b) => CompareBytes(a, b, UpCase);
    // strings.pas records two extra equivalences in Windows word collation.
    private static byte CollationByte(byte c) => c switch { 173 => 0, 136 => 94, _ => c };
    public static int AnsiCompareStr(AnsiString a, AnsiString b) => CompareBytes(a, b, CollationByte);
    public static int AnsiCompareText(AnsiString a, AnsiString b) => CompareBytes(a, b, c => CollationByte(Fold1252(c)));
    public static AnsiString TrimLeft(AnsiString value)
    {
        int i = 1;
        while (i <= value.Length && value[i] <= 32) i++;
        return Copy(value, i, int.MaxValue);
    }
    public static AnsiString TrimRight(AnsiString value)
    {
        int n = value.Length;
        while (n > 0 && value[n] <= 32) n--;
        return Copy(value, 1, n);
    }
    public static AnsiString Trim(AnsiString value) => TrimLeft(TrimRight(value));
    public static AnsiString QuotedStr(AnsiString value) => "'" + (AnsiString)value.ToString().Replace("'", "''", StringComparison.Ordinal) + "'";
    public static AnsiString StringOfChar(byte character, int count) => AnsiString.FromBytes(Enumerable.Repeat(character, Math.Max(0, count)).ToArray());
    public static int AnsiPos(AnsiString needle, AnsiString value) => Pos(needle, value);
    public static AnsiString Format(AnsiString format, params TVarRec[] args) => ExceptionText.Format(format, args);
}
