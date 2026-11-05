using System.Globalization;
using System.Text;

namespace Aow.Rtl;

public class Exception : System.Exception
{
    public new string Message { get; set; } = "";
    public int HelpContext { get; set; }
    public Exception(string message = "") : base(message) => Message = message;
    public static Exception Create(string message = "") => new(message);
    public static Exception CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(RtlUnit.Format(format, args));
}

public sealed class EHalt(int exitCode) : System.Exception
{
    public int ExitCode { get; } = exitCode;
}
