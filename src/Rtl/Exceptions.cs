namespace Aow.Rtl;

public class EStringListError(string message = "") : Exception(message)
{
    public static new EStringListError Create(string message = "") => new(message);
    public static new EStringListError CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EWin32Error(string message = "") : Exception(message)
{
    public int ErrorCode { get; set; }
    public static new EWin32Error Create(string message = "") => new(message);
    public static new EWin32Error CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EAbort(string message = "") : Exception(message)
{
    public static new EAbort Create(string message = "") => new(message);
    public static new EAbort CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EConvertError(string message = "") : Exception(message)
{
    public static new EConvertError Create(string message = "") => new(message);
    public static new EConvertError CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EListError(string message = "") : Exception(message)
{
    public static new EListError Create(string message = "") => new(message);
    public static new EListError CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EStreamError(string message = "") : Exception(message)
{
    public static new EStreamError Create(string message = "") => new(message);
    public static new EStreamError CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EReadError(string message = "") : EFilerError(message)
{
    public static new EReadError Create(string message = "") => new(message);
    public static new EReadError CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EWriteError(string message = "") : EFilerError(message)
{
    public static new EWriteError Create(string message = "") => new(message);
    public static new EWriteError CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EFOpenError(string message = "") : EStreamError(message)
{
    public static new EFOpenError Create(string message = "") => new(message);
    public static new EFOpenError CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EFCreateError(string message = "") : EStreamError(message)
{
    public static new EFCreateError Create(string message = "") => new(message);
    public static new EFCreateError CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EInvalidCast(string message = "") : Exception(message)
{
    public static new EInvalidCast Create(string message = "") => new(message);
    public static new EInvalidCast CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EAccessViolation(string message = "") : Exception(message)
{
    public static new EAccessViolation Create(string message = "") => new(message);
    public static new EAccessViolation CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EDivByZero(string message = "") : EIntError(message)
{
    public static new EDivByZero Create(string message = "") => new(message);
    public static new EDivByZero CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class ERangeError(string message = "") : EIntError(message)
{
    public static new ERangeError Create(string message = "") => new(message);
    public static new ERangeError CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EIntOverflow(string message = "") : EIntError(message)
{
    public static new EIntOverflow Create(string message = "") => new(message);
    public static new EIntOverflow CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EInvalidOp(string message = "") : EMathError(message)
{
    public static new EInvalidOp Create(string message = "") => new(message);
    public static new EInvalidOp CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EAssertionFailed(string message = "") : Exception(message)
{
    public static new EAssertionFailed Create(string message = "") => new(message);
    public static new EAssertionFailed CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EComponentError(string message = "") : Exception(message)
{
    public static new EComponentError Create(string message = "") => new(message);
    public static new EComponentError CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EInvalidOperation(string message = "") : Exception(message)
{
    public static new EInvalidOperation Create(string message = "") => new(message);
    public static new EInvalidOperation CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EOutOfMemory(string message = "") : Exception(message)
{
    public static new EOutOfMemory Create(string message = "") => new(message);
    public static new EOutOfMemory CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EInOutError(string message = "") : Exception(message)
{
    public int ErrorCode;
    public static new EInOutError Create(string message = "") => new(message);
    public static new EInOutError CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EAbstractError(string message = "") : Exception(message)
{
    public static new EAbstractError Create(string message = "") => new(message);
    public static new EAbstractError CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EIntError(string message = "") : Exception(message)
{
    public static new EIntError Create(string message = "") => new(message);
    public static new EIntError CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EMathError(string message = "") : Exception(message)
{
    public static new EMathError Create(string message = "") => new(message);
    public static new EMathError CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EFilerError(string message = "") : EStreamError(message)
{
    public static new EFilerError Create(string message = "") => new(message);
    public static new EFilerError CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EZeroDivide(string message = "") : EMathError(message)
{
    public static new EZeroDivide Create(string message = "") => new(message);
    public static new EZeroDivide CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}

public class EOverflow(string message = "") : EMathError(message)
{
    public static new EOverflow Create(string message = "") => new(message);
    public static new EOverflow CreateFmt(string format, params ReadOnlySpan<FormatArg> args) => new(Rtl.Format(format, args));
}
