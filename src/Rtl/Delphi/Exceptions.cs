namespace Aow.Rtl;

public class AbortException(string message = "") : System.Exception(message)
{
    public static AbortException Create(string message = "") => new(message);
    public static AbortException CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(RtlUnit.Format(format, args));
}

public class AssertionFailedException(string message = "") : System.Exception(message)
{
    public static AssertionFailedException Create(string message = "") => new(message);
    public static AssertionFailedException CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(RtlUnit.Format(format, args));
}

public class AbstractErrorException(string message = "") : System.Exception(message)
{
    public static AbstractErrorException Create(string message = "") => new(message);
    public static AbstractErrorException CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(RtlUnit.Format(format, args));
}
