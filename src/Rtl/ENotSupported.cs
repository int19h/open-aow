namespace Aow.Rtl;

public class ENotSupported(AnsiString message = default) : Exception(message)
{
    public static new ENotSupported Create(AnsiString message = default) => new(message);
    public static new ENotSupported CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}
