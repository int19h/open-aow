using System.Globalization;

namespace Aow.Rtl;

public static partial class Rtl
{
    public static void ConsoleWrite(bool newline, params object[] values) => ConsoleWrite(Console.OpenStandardOutput(), newline, values);

    public static void ConsoleWrite(Stream output, bool newline, params object[] values)
    {
        foreach (object value in values)
        {
            AnsiString text = ConsoleFormat(value);
            output.Write(text.Span);
        }
        if (newline) output.Write("\r\n"u8);
    }

    public static AnsiString ConsoleFormat(object value, int width = 0, int? precision = null)
    {
        string Scientific(double real)
        {
            string text = real.ToString("E14", CultureInfo.InvariantCulture);
            int exponent = text.IndexOf('E');
            if (exponent >= 0) text = text[..(exponent + 2)] + text[(exponent + 2)..].PadLeft(4, '0');
            return real >= 0 ? " " + text : text;
        }
        AnsiString result = value switch
        {
            AnsiString bytes => bytes,
            bool flag => flag ? "TRUE" : "FALSE",
            double real => precision is int digits ? real.ToString("F" + Math.Clamp(digits, 0, 18), CultureInfo.InvariantCulture) : Scientific(real),
            float real => precision is int digits ? real.ToString("F" + Math.Clamp(digits, 0, 18), CultureInfo.InvariantCulture) : Scientific(real),
            IFormattable number => number.ToString(null, CultureInfo.InvariantCulture),
            _ => value.ToString() ?? ""
        };
        return width > result.Length ? (AnsiString)new string(' ', width - result.Length) + result : result;
    }
}
