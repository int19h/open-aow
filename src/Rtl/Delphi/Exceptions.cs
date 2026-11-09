namespace Aow.Rtl;

public class EAbort(string message = "") : System.Exception(message)
{
    public static EAbort Create(string message = "") => new(message);
    public static EAbort CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(RtlUnit.Format(format, args));
}

public class EAssertionFailed(string message = "") : System.Exception(message)
{
    public static EAssertionFailed Create(string message = "") => new(message);
    public static EAssertionFailed CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(RtlUnit.Format(format, args));
}

public class EAbstractError(string message = "") : System.Exception(message)
{
    public static EAbstractError Create(string message = "") => new(message);
    public static EAbstractError CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(RtlUnit.Format(format, args));
}
