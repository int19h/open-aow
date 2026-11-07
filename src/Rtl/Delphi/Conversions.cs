using System.Globalization;

namespace Aow.Rtl;

public static partial class RtlUnit
{
    public static string IntToStr(int value) => value.ToString(CultureInfo.InvariantCulture);
    // SYSUTILS.PAS IntToHex uses FormatBuf, which ignores precision outside 0..15.
    public static string IntToHex(int value, int digits) => ((uint)value).ToString("X", CultureInfo.InvariantCulture).PadLeft(digits is > 0 and < 16 ? digits : 0, '0');

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
