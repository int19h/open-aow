using System.Globalization;
using System.Text;

namespace Aow.Rtl;

public class Exception : System.Exception
{
    public new AnsiString Message { get; set; }
    public int HelpContext { get; set; }
    public Exception(AnsiString message = default) : base(message.ToString()) => Message = message;
    public static Exception Create(AnsiString message = default) => new(message);
    public static Exception CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

internal static class ExceptionText
{
    // The integer and string FormatBuf grammar also serves exception constructors.
    internal static AnsiString Format(AnsiString format, TVarRec[] args)
    {
        var result = new StringBuilder();
        string text = format.ToString();
        int next = 0;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] != '%') { result.Append(text[i]); continue; }
            if (++i >= text.Length) throw new EConvertError("Incomplete format specifier");
            if (text[i] == '%') { result.Append('%'); continue; }
            int ReadNumber(ref int position)
            {
                if (position < text.Length && text[position] == '*')
                {
                    position++;
                    var argument = Take();
                    if (argument.Kind != VarKind.Integer) throw new EConvertError("Integer format width required");
                    return checked((int)argument.Int);
                }
                int value = 0;
                while (position < text.Length && char.IsAsciiDigit(text[position]))
                    value = checked(value * 10 + text[position++] - '0');
                return value;
            }
            TVarRec Take()
            {
                if (next < 0 || next >= args.Length) throw new EConvertError("Missing format argument");
                return args[next++];
            }
            int start = i;
            int width = 0;
            if (text[i] == '*' || char.IsAsciiDigit(text[i]))
            {
                int number = ReadNumber(ref i);
                if (i < text.Length && text[i] == ':') { next = number; i++; }
                else { width = number; }
            }
            bool left = i < text.Length && text[i] == '-';
            if (left) i++;
            if (i < text.Length && (text[i] == '*' || char.IsAsciiDigit(text[i]))) width = ReadNumber(ref i);
            int precision = -1;
            if (i < text.Length && text[i] == '.') { i++; precision = ReadNumber(ref i); }
            if (i >= text.Length) throw new EConvertError("Incomplete format specifier");
            var value = Take();
            string output;
            switch (char.ToUpperInvariant(text[i]))
            {
                case 'D':
                case 'U':
                case 'X':
                    if (value.Kind != VarKind.Integer) throw new EConvertError("Integer format argument required");
                    char kind = char.ToUpperInvariant(text[i]);
                    output = kind == 'D' ? ((int)value.Int).ToString(CultureInfo.InvariantCulture)
                        : ((uint)value.Int).ToString(kind == 'X' ? "X" : "D", CultureInfo.InvariantCulture);
                    if (precision > 0)
                        output = output.StartsWith('-') ? "-" + output[1..].PadLeft(precision, '0') : output.PadLeft(precision, '0');
                    break;
                case 'S':
                    output = value.Kind switch
                    {
                        VarKind.String => value.Str.ToString(),
                        VarKind.Char => AnsiString.FromBytes(value.Chr).ToString(),
                        _ => throw new EConvertError("String format argument required")
                    };
                    if (precision >= 0 && output.Length > precision) output = output[..precision];
                    break;
                default: throw new EConvertError($"Unsupported format at {start}");
            }
            result.Append(left ? output.PadRight(Math.Max(0, width)) : output.PadLeft(Math.Max(0, width)));
        }
        return result.ToString();
    }
}

public sealed class EHalt(int exitCode) : System.Exception
{
    public int ExitCode { get; } = exitCode;
}
