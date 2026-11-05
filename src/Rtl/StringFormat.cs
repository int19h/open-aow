using System.Globalization;
using System.Text;

namespace Aow.Rtl;

public static partial class Rtl
{
    // SYSUTILS.PAS FormatBuf also serves exception constructors.
    public static string Format(string? format, params ReadOnlySpan<FormatArg> args)
    {
        var result = new StringBuilder();
        string text = RequireString(format);
        int next = 0;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] != '%') { result.Append(text[i]); continue; }
            if (++i >= text.Length) break;
            if (text[i] == '%') { result.Append('%'); continue; }
            int start = i;
            int width;
            bool left;
            // FormatBuf's A0 loop permits another index after each colon.
            while (true)
            {
                if (i >= text.Length) return result.ToString();
                left = text[i] == '-';
                if (left) i++;
                width = ReadNumber(text, ref i, args, ref next);
                if (i >= text.Length) return result.ToString();
                if (text[i] != ':') break;
                next = width;
                i++;
            }
            int precision = -1;
            if (i < text.Length && text[i] == '.') { i++; precision = ReadNumber(text, ref i, args, ref next); }
            if (i >= text.Length) break;
            if (next < 0 || next >= args.Length) throw new EConvertError("Missing format argument");
            char conversion = UpCase(text[i]);
            string output = args[next++] switch
            {
                int value when conversion is 'D' or 'U' or 'X' => FormatInteger(conversion, value, precision),
                string value when conversion == 'S' => Truncate(value, precision),
                char value when conversion == 'S' => value.ToString(),
                PointerArg value when conversion == 'P' => value.Value.ToString("X8", CultureInfo.InvariantCulture),
                int or bool or char or double or string or TObject or PointerArg or null => throw Mismatch(conversion, start),
            };
            result.Append(left ? output.PadRight(Math.Max(0, width)) : output.PadLeft(Math.Max(0, width)));
        }
        return result.ToString();
    }

    private static int ReadNumber(string text, ref int position, ReadOnlySpan<FormatArg> args, ref int next)
    {
        if (position < text.Length && text[position] == '*')
        {
            position++;
            if (next < 0 || next >= args.Length) return 0;
            return args[next++] is int value ? value : 0;
        }
        int number = 0;
        while (position < text.Length && char.IsAsciiDigit(text[position]))
            number = unchecked(number * 10 + text[position++] - '0');
        return number;
    }

    private static string FormatInteger(char conversion, int value, int precision)
    {
        string output = conversion == 'D' ? value.ToString(CultureInfo.InvariantCulture)
            : unchecked((uint)value).ToString(conversion == 'X' ? "X" : "D", CultureInfo.InvariantCulture);
        if (precision > 0 && precision < 16)
            output = output.StartsWith('-') ? "-" + output[1..].PadLeft(precision, '0') : output.PadLeft(precision, '0');
        return output;
    }

    private static string Truncate(string value, int precision) =>
        precision >= 0 && value.Length > precision ? value[..precision] : value;

    private static EConvertError Mismatch(char conversion, int start) => conversion switch
    {
        'D' or 'U' or 'X' => new EConvertError("Integer format argument required"),
        'S' => new EConvertError("String format argument required"),
        'P' => new EConvertError("Pointer format argument required"),
        _ => new EConvertError($"Unsupported format at {start}"),
    };
}
