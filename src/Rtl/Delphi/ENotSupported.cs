namespace Aow.Rtl;

public class ENotSupported(string message = "") : Exception(message)
{
    public static new ENotSupported Create(string message = "") => new(message);
    public static new ENotSupported CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(RtlUnit.Format(format, args));
}
